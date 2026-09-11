namespace Adventure.Kern;

/// <summary>Ein Feld auf dem Spielfeld. X zählt nach rechts, Y nach unten.</summary>
public readonly record struct Position(int X, int Y)
{
    public Position Verschoben(Richtung richtung)
    {
        return richtung switch
        {
            Richtung.Oben => new Position(X, Y - 1),
            Richtung.Unten => new Position(X, Y + 1),
            Richtung.Links => new Position(X - 1, Y),
            _ => new Position(X + 1, Y)
        };
    }

    /// <summary>Manhattan-Entfernung: Anzahl der Schritte ohne Diagonalen.</summary>
    public int Entfernung(Position andere)
    {
        return Math.Abs(X - andere.X) + Math.Abs(Y - andere.Y);
    }

    public override string ToString() => $"({X}, {Y})";
}
