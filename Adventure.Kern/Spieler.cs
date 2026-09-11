namespace Adventure.Kern;

public class SchatzEventArgs : EventArgs
{
    public Schatz Schatz { get; }
    public int Punkte { get; }

    public SchatzEventArgs(Schatz schatz, int punkte)
    {
        Schatz = schatz;
        Punkte = punkte;
    }
}

public class Spieler : BeweglichesObjekt
{
    public const int MaxLebenspunkte = 3;

    public int Lebenspunkte { get; private set; } = MaxLebenspunkte;
    public int Punkte { get; private set; }
    public Inventar<Gegenstand> Inventar { get; } = new();

    /// <summary>Wird ausgelöst, wenn der Spieler einen Schatz findet – z. B. für die Anzeige.</summary>
    public event EventHandler<SchatzEventArgs>? SchatzGefunden;

    public Spieler(string name, Position position) : base(name, position)
    {
    }

    public override char Symbol => '@';
    public bool IstAmLeben => Lebenspunkte > 0;

    public override string Beschreibung()
    {
        return $"{Name} bei {Position}, {Lebenspunkte}/{MaxLebenspunkte} Lebenspunkte, {Punkte} Punkte, Inventar: {Inventar}";
    }

    public void SchadenNehmen(int schaden = 1)
    {
        Lebenspunkte = Math.Max(0, Lebenspunkte - schaden);
    }

    public void Heilen(int heilung)
    {
        Lebenspunkte = Math.Min(MaxLebenspunkte, Lebenspunkte + heilung);
    }

    /// <summary>Setzt Lebenspunkte und Punkte direkt – nur beim Laden eines Spielstands.</summary>
    public void Wiederherstellen(int lebenspunkte, int punkte)
    {
        Lebenspunkte = Math.Clamp(lebenspunkte, 0, MaxLebenspunkte);
        Punkte = punkte;
    }

    public void SchatzEinsammeln(Schatz schatz)
    {
        Punkte += schatz.Wert;
        SchatzGefunden?.Invoke(this, new SchatzEventArgs(schatz, Punkte));
    }
}
