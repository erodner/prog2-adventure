using Adventure.Daten;
using Adventure.Kern;
namespace Adventure.Tests;

[TestFixture]
public class SpielfeldTests
{
    private static Spielfeld Feld(params string[] zeilen) => LevelParser.Parsen(new Level("t", zeilen));

    [Test] public void Wand_Blockiert()
    {
        Spielfeld f = Feld("#####", "#@#..", "#####");
        f.SpielerZieht(Richtung.Rechts);
        Assert.That(f.Spieler.Position, Is.EqualTo(new Position(1, 1)));
        Assert.That(f.LetzteMeldung, Does.Contain("nicht weiter"));
        f.SpielerZieht(Richtung.Oben);
        Assert.That(f.Spieler.Position, Is.EqualTo(new Position(1, 1)));
    }

    [Test] public void Schluessel_Aufheben_Und_Tuer_Oeffnen()
    {
        Spielfeld f = Feld("@kD.E");
        f.SpielerZieht(Richtung.Rechts);
        Assert.That(f.Spieler.Inventar.Enthaelt<Schluessel>(), Is.True);
        Assert.That(f.StatischesObjektAn(new Position(1, 0)), Is.Null);
        f.SpielerZieht(Richtung.Rechts);            // vor der Tür: aufschließen, nicht bewegen
        Assert.That(f.Spieler.Position, Is.EqualTo(new Position(1, 0)));
        Assert.That(((Tuer)f.StatischesObjektAn(new Position(2, 0))!).IstOffen, Is.True);
        Assert.That(f.Spieler.Inventar.Anzahl, Is.EqualTo(0));
        f.SpielerZieht(Richtung.Rechts); f.SpielerZieht(Richtung.Rechts); f.SpielerZieht(Richtung.Rechts);
        Assert.That(f.Status, Is.EqualTo(Spielstatus.Gewonnen));
    }

    [Test] public void Tuer_Ohne_Schluessel_Bleibt_Zu()
    {
        Spielfeld f = Feld("@D");
        f.SpielerZieht(Richtung.Rechts);
        Assert.That(f.LetzteMeldung, Does.Contain("Schlüssel"));
        Assert.That(f.Spieler.Position, Is.EqualTo(new Position(0, 0)));
    }

    [Test] public void Truhe_Loest_Ereignis_Aus()
    {
        Spielfeld f = Feld("@T");
        int punkte = -1;
        f.Spieler.SchatzGefunden += (s, e) => punkte = e.Punkte;
        f.SpielerZieht(Richtung.Rechts);
        Assert.That(punkte, Is.EqualTo(100));
        Assert.That(f.Spieler.Punkte, Is.EqualTo(100));
        f.SpielerZieht(Richtung.Rechts);
        Assert.That(f.LetzteMeldung, Does.Contain("leer"));
    }

    [Test] public void Wache_Dreht_Um()
    {
        Spielfeld f = Feld("@....", "#W.##");
        Wache w = (Wache)f.Gegner[0];
        f.SpielerZieht(Richtung.Rechts);   // Wache nach rechts auf (2,1)
        Assert.That(w.Position, Is.EqualTo(new Position(2, 1)));
        f.SpielerZieht(Richtung.Rechts);   // blockiert -> umdrehen, nach links (1,1)
        Assert.That(w.Position, Is.EqualTo(new Position(1, 1)));
        Assert.That(w.Laufrichtung, Is.EqualTo(Richtung.Links));
    }

    [Test] public void Verfolger_Jagt_Und_Trifft()
    {
        Spielfeld f = Feld("@..V");
        f.SpielerZieht(Richtung.Oben);   // Spieler bleibt, Verfolger auf (2,0)
        Assert.That(f.Gegner[0].Position, Is.EqualTo(new Position(2, 0)));
        f.SpielerZieht(Richtung.Oben);   // (1,0)
        f.SpielerZieht(Richtung.Oben);   // Treffer
        Assert.That(f.Spieler.Lebenspunkte, Is.EqualTo(2));
        f.SpielerZieht(Richtung.Oben); f.SpielerZieht(Richtung.Oben);
        Assert.That(f.Status, Is.EqualTo(Spielstatus.Verloren));
    }

    [Test] public void Verfolger_Sieht_Nicht_Durch_Waende()
    {
        Spielfeld f = Feld("@.#.V");
        f.SpielerZieht(Richtung.Oben);
        Assert.That(f.Gegner[0].Position, Is.EqualTo(new Position(4, 0)));
    }

    [Test] public void Trank_Heilt()
    {
        Spielfeld f = Feld("@!");
        f.Spieler.SchadenNehmen(2);
        f.SpielerZieht(Richtung.Rechts);
        Assert.That(f.Spieler.Lebenspunkte, Is.EqualTo(2));
        Assert.That(f.Spieler.Inventar.Anzahl, Is.EqualTo(0));
    }

    [Test] public void Eingebaute_Level_Parsen()
    {
        EingebauteLevelQuelle q = new();
        foreach (string n in q.LevelNamen)
        {
            Spielfeld f = LevelParser.Parsen(q.Laden(n));
            Assert.That(f.AlsText().TrimEnd().Split('\n'), Has.Length.EqualTo(q.Laden(n).Zeilen.Count));
            Assert.That(f.AlsText(), Does.Contain("@"));
        }
    }
}
