namespace Adventure.Kern;

/// <summary>Eine Truhe enthält einen Schatz. Öffnen = Interagieren.</summary>
public sealed class Truhe : StatischesObjekt, IInteragierbar
{
    public Schatz Inhalt { get; }
    public bool IstGeoeffnet { get; private set; }

    public Truhe(Position position, int wert) : base("Truhe", position)
    {
        Inhalt = new Schatz(position, wert);
    }

    public override char Symbol => IstGeoeffnet ? 't' : 'T';

    public string Interagieren(Spieler spieler)
    {
        if (IstGeoeffnet)
        {
            return "Die Truhe ist leer.";
        }
        IstGeoeffnet = true;
        return Inhalt.Aufheben(spieler);
    }

    /// <summary>Für das Wiederherstellen eines Spielstands: Truhe gilt als geplündert.</summary>
    public void AlsGeoeffnetMarkieren()
    {
        IstGeoeffnet = true;
    }
}
