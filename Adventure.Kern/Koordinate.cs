namespace Adventure.Kern;

/// <summary>Ein Feld auf dem Spielfeld. X zählt nach rechts, Y nach unten.</summary>
public readonly record struct Koordinate(int X, int Y)
{
    public Koordinate Verschoben(Richtung richtung)
    {
        if (richtung == Richtung.Oben)
        {
            return new Koordinate(X, Y - 1);
        }
        else if (richtung == Richtung.Unten)
        {
            return new Koordinate(X, Y + 1);
        }
        else if (richtung == Richtung.Links)
        {
            return new Koordinate(X - 1, Y);
        }
        else
        {
            return new Koordinate(X + 1, Y);
        }
    }

    /// <summary>Manhattan-Entfernung: Anzahl der Schritte ohne Diagonalen.</summary>
    public int Entfernung(Koordinate andere)
    {
        return Math.Abs(X - andere.X) + Math.Abs(Y - andere.Y);
    }

    public override string ToString() => $"({X}, {Y})";
}
