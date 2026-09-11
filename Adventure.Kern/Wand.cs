namespace Adventure.Kern;

public sealed class Wand : Spielobjekt
{
    public Wand(Koordinate position) : base("Wand", position)
    {
    }

    public override char Symbol => '#';
}
