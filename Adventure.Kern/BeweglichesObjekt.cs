namespace Adventure.Kern;

/// <summary>Objekte, die sich über das Spielfeld bewegen: der Spieler und alle Gegner.</summary>
public abstract class BeweglichesObjekt : Spielobjekt
{
    protected BeweglichesObjekt(string name, Position position) : base(name, position)
    {
    }

    /// <summary>Versucht einen Schritt; bleibt stehen, wenn das Zielfeld nicht frei ist.</summary>
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

    /// <summary>Setzt das Objekt direkt auf ein Feld – z. B. beim Laden eines Spielstands.</summary>
    public void Versetzen(Position neuePosition)
    {
        Position = neuePosition;
    }
}
