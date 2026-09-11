namespace Adventure.Kern;

/// <summary>
/// Gemeinsame Basis für alles, was auf dem Spielfeld liegt: Wände, Türen, Gegenstände,
/// der Spieler und die Gegner. Abstrakt, weil es „irgendein Spielobjekt“ nicht gibt –
/// jedes konkrete Objekt muss mindestens sein Symbol festlegen.
/// </summary>
public abstract class Spielobjekt
{
    public string Name { get; }
    public Position Position { get; protected set; }

    protected Spielobjekt(string name, Position position)
    {
        Name = name;
        Position = position;
    }

    /// <summary>Das Zeichen, mit dem das Objekt auf der Karte gezeichnet wird.</summary>
    public abstract char Symbol { get; }

    /// <summary>Darf ein bewegliches Objekt dieses Feld betreten?</summary>
    public virtual bool IstPassierbar => false;

    public virtual string Beschreibung()
    {
        return $"{Name} bei {Position}";
    }

    public override string ToString() => Beschreibung();
}
