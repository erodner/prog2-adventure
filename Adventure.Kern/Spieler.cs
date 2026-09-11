namespace Adventure.Kern;

public class Spieler : Spielobjekt
{
    public int Lebenspunkte { get; private set; } = 3;

    public Spieler(string name, Position position) : base(name, position)
    {
    }

    public override char Symbol => '@';

    public override string Beschreibung()
    {
        return base.Beschreibung() + $", {Lebenspunkte} Lebenspunkte";
    }

    /// <summary>Versucht einen Schritt; bleibt stehen, wenn das Zielfeld belegt ist.</summary>
    public bool Bewegen(Richtung richtung, Spielfeld feld)
    {
        Position ziel = Position.Verschoben(richtung);
        if (!feld.IstFrei(ziel))
        {
            return false;
        }
        Position = ziel;
        return true;
    }
}
