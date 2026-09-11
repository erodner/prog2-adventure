namespace Adventure.Kern;

/// <summary>Alles, was auf dem Spielfeld liegt: Wände, Türen, Gegenstände, der Spieler, Gegner.</summary>
public class Spielobjekt
{
    public string Name { get; }
    public Position Position { get; protected set; }

    public Spielobjekt(string name, Position position)
    {
        Name = name;
        Position = position;
    }

    /// <summary>Das Zeichen, mit dem das Objekt auf der Karte gezeichnet wird.</summary>
    public virtual char Symbol => '?';

    /// <summary>Kann der Spieler dieses Feld betreten?</summary>
    public virtual bool IstPassierbar => false;

    public virtual string Beschreibung()
    {
        return $"{Name} bei {Position}";
    }

    public override string ToString() => Beschreibung();
}
