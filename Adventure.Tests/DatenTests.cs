using Adventure.Daten;
using Adventure.Kern;

namespace Adventure.Tests;

[TestFixture]
public class DatenTests
{
    private string ordner = "";

    [SetUp]
    public void Vorbereiten()
    {
        ordner = Path.Combine(Path.GetTempPath(), "adventure_" + Guid.NewGuid());
        Directory.CreateDirectory(ordner);
    }

    [TearDown]
    public void Aufraeumen()
    {
        Directory.Delete(ordner, recursive: true);
    }

    [Test]
    public void TextdateiLevelQuelle_ListetUndLaedtLevel()
    {
        File.WriteAllLines(Path.Combine(ordner, "mini.txt"), new[] { "#####", "#@.E#", "#####", "" });

        TextdateiLevelQuelle quelle = new(ordner);

        Assert.That(quelle.LevelNamen, Is.EqualTo(new[] { "mini" }));
        Assert.That(quelle.Laden("mini").Zeilen, Has.Count.EqualTo(3));
        Assert.That(() => quelle.Laden("gibtEsNicht"), Throws.TypeOf<FileNotFoundException>());
    }

    [Test]
    public void Spielstand_SpeichernUndLaden_StelltSpielWiederHer()
    {
        Level level = new("t", new[] { "@kD.E", "..T.V" });
        Spielfeld feld = LevelParser.Parsen(level);
        feld.SpielerZieht(Richtung.Rechts);   // Schlüssel aufheben
        feld.SpielerZieht(Richtung.Rechts);   // Tür aufschließen
        feld.SpielerZieht(Richtung.Unten);    // vor die Truhe? (1,1) ist frei -> Spieler auf (1,1)
        feld.SpielerZieht(Richtung.Rechts);   // Truhe öffnen
        string pfad = Path.Combine(ordner, "spielstand.json");
        JsonSpielstandSpeicher speicher = new(pfad);

        speicher.Speichern(feld.Erfassen("t", level));
        Spielstand? geladen = speicher.Laden();
        Spielfeld wieder = Spielfeld.Wiederherstellen(level, geladen!);

        Assert.That(File.ReadAllText(pfad), Does.Contain("\"Punkte\": 100"));
        Assert.That(wieder.Spieler.Position, Is.EqualTo(feld.Spieler.Position));
        Assert.That(wieder.Spieler.Punkte, Is.EqualTo(100));
        Assert.That(wieder.Spieler.Lebenspunkte, Is.EqualTo(feld.Spieler.Lebenspunkte));
        Assert.That(wieder.StatischesObjektAn(new Position(1, 0)), Is.Null);
        Assert.That(((Tuer)wieder.StatischesObjektAn(new Position(2, 0))!).IstOffen, Is.True);
        Assert.That(((Truhe)wieder.StatischesObjektAn(new Position(2, 1))!).IstGeoeffnet, Is.True);
        Assert.That(wieder.Gegner[0].Position, Is.EqualTo(feld.Gegner[0].Position));
        Assert.That(wieder.AlsText(), Is.EqualTo(feld.AlsText()));
    }

    [Test]
    public void Inventar_EnthaeltUndEntferntNachTyp()
    {
        Inventar<Gegenstand> inventar = new();
        inventar.Hinzufuegen(new Schluessel(new Position(0, 0)));

        Assert.That(inventar.Enthaelt<Schluessel>(), Is.True);
        Assert.That(inventar.Enthaelt<Schatz>(), Is.False);
        Assert.That(inventar.Entfernen<Schluessel>(), Is.True);
        Assert.That(inventar.Anzahl, Is.EqualTo(0));
    }

    [Test]
    public void Position_IstWertgleich_UndTaugtAlsSchluessel()
    {
        Dictionary<Position, string> namen = new() { [new Position(2, 3)] = "Truhe" };

        Assert.That(new Position(2, 3), Is.EqualTo(new Position(2, 3)));
        Assert.That(namen[new Position(2, 3)], Is.EqualTo("Truhe"));
        Assert.That(new Position(0, 0).Verschoben(Richtung.Unten), Is.EqualTo(new Position(0, 1)));
        Assert.That(new Position(0, 0).Entfernung(new Position(3, 4)), Is.EqualTo(7));
    }
}
