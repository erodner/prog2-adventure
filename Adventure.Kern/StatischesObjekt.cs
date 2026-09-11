namespace Adventure.Kern;

/// <summary>Objekte, die sich nie bewegen: Wände, Türen, Truhen, Gegenstände auf dem Boden.</summary>
public abstract class StatischesObjekt : Spielobjekt
{
    protected StatischesObjekt(string name, Koordinate position) : base(name, position)
    {
    }
}
