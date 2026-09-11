namespace Adventure.Kern;

public sealed class Wand : Spielobjekt
{
    public Wand(Position position) : base("Wand", position)
    {
    }

    public override char Symbol => '#';
}
