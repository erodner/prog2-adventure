namespace Adventure.Kern;

/// <summary>Alle Gegner bewegen sich einmal pro Runde – wie, entscheidet jede Art selbst.</summary>
public abstract class Gegner : BeweglichesObjekt
{
    protected Gegner(string name, Position position) : base(name, position)
    {
    }

    /// <summary>Liefert die Richtung für diese Runde oder null, wenn der Gegner stehen bleibt.</summary>
    public abstract Richtung? NaechsterZug(Spielfeld feld);
}

/// <summary>Eine Wache läuft stur geradeaus und dreht um, wenn sie anstößt.</summary>
public sealed class Wache : Gegner
{
    public Richtung Laufrichtung { get; private set; }

    public Wache(Position position, Richtung laufrichtung = Richtung.Rechts) : base("Wache", position)
    {
        Laufrichtung = laufrichtung;
    }

    public override char Symbol => 'W';

    public override Richtung? NaechsterZug(Spielfeld feld)
    {
        if (!feld.IstFrei(Position.Verschoben(Laufrichtung)))
        {
            Laufrichtung = Umkehren(Laufrichtung);
        }
        return feld.IstFrei(Position.Verschoben(Laufrichtung)) ? Laufrichtung : null;
    }

    private static Richtung Umkehren(Richtung r) => r switch
    {
        Richtung.Oben => Richtung.Unten,
        Richtung.Unten => Richtung.Oben,
        Richtung.Links => Richtung.Rechts,
        _ => Richtung.Links
    };
}

/// <summary>Ein Verfolger wartet, bis er den Spieler sieht – dann nimmt er die Jagd auf.</summary>
public sealed class Verfolger : Gegner
{
    public int Sichtweite { get; }

    public Verfolger(Position position, int sichtweite = 5) : base("Verfolger", position)
    {
        Sichtweite = sichtweite;
    }

    public override char Symbol => 'V';

    public override Richtung? NaechsterZug(Spielfeld feld)
    {
        Position ziel = feld.Spieler.Position;
        if (Position.Entfernung(ziel) > Sichtweite || !feld.HatSichtlinie(Position, ziel))
        {
            return null;
        }

        int dx = ziel.X - Position.X;
        int dy = ziel.Y - Position.Y;

        // Erst die Achse mit dem größeren Abstand probieren, dann die andere.
        Richtung erste = Math.Abs(dx) >= Math.Abs(dy)
            ? (dx > 0 ? Richtung.Rechts : Richtung.Links)
            : (dy > 0 ? Richtung.Unten : Richtung.Oben);
        Richtung zweite = Math.Abs(dx) >= Math.Abs(dy)
            ? (dy > 0 ? Richtung.Unten : Richtung.Oben)
            : (dx > 0 ? Richtung.Rechts : Richtung.Links);

        if (feld.IstFrei(Position.Verschoben(erste)) || Position.Verschoben(erste) == ziel) return erste;
        if (feld.IstFrei(Position.Verschoben(zweite)) || Position.Verschoben(zweite) == ziel) return zweite;
        return null;
    }
}
