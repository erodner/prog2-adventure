namespace Adventure.Kern;

/// <summary>Ein Gegenstand liegt auf dem Boden, bis der Spieler darüber läuft.</summary>
public abstract class Gegenstand : StatischesObjekt, ISammelbar
{
    protected Gegenstand(string name, Koordinate position) : base(name, position)
    {
    }

    // Man kann auf einen Gegenstand treten – dabei wird er aufgehoben.
    public override bool IstPassierbar => true;

    public virtual string Aufheben(Spieler spieler)
    {
        return $"{spieler.Name} hebt {Name} auf.";
    }
}

public sealed class Schluessel : Gegenstand
{
    public Schluessel(Koordinate position) : base("Schlüssel", position)
    {
    }

    public override char Symbol => 'k';
}

public sealed class Trank : Gegenstand
{
    public int Heilung { get; }

    public Trank(Koordinate position, int heilung = 1) : base("Trank", position)
    {
        Heilung = heilung;
    }

    public override char Symbol => '!';

    // Ein Trank wird sofort getrunken statt ins Inventar gelegt.
    public override string Aufheben(Spieler spieler)
    {
        spieler.Heilen(Heilung);
        return $"{spieler.Name} trinkt einen Trank (+{Heilung}).";
    }
}

public sealed class Schatz : Gegenstand
{
    public int Wert { get; }

    public Schatz(Koordinate position, int wert) : base("Schatz", position)
    {
        Wert = wert;
    }

    public override char Symbol => '$';

    public override string Aufheben(Spieler spieler)
    {
        spieler.SchatzEinsammeln(this);
        return $"{spieler.Name} findet einen Schatz im Wert von {Wert}!";
    }
}
