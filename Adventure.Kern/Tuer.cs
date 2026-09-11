namespace Adventure.Kern;

/// <summary>Eine Tür ist eine Wand, die sich mit einem Schlüssel öffnen lässt.</summary>
public sealed class Tuer : StatischesObjekt, IInteragierbar
{
    public bool IstOffen { get; private set; }

    public Tuer(Position position) : base("Tür", position)
    {
    }

    public override char Symbol => IstOffen ? '/' : 'D';
    public override bool IstPassierbar => IstOffen;

    public string Interagieren(Spieler spieler)
    {
        if (IstOffen)
        {
            return "Die Tür ist schon offen.";
        }
        if (!spieler.Inventar.Enthaelt<Schluessel>())
        {
            return "Die Tür ist verschlossen. Du brauchst einen Schlüssel.";
        }
        spieler.Inventar.Entfernen<Schluessel>();
        Aufschliessen();
        return "Du schließt die Tür auf.";
    }

    public void Aufschliessen()
    {
        IstOffen = true;
    }
}
