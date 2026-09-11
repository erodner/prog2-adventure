namespace Adventure.Kern;

public sealed class Wand : StatischesObjekt
{
    public Wand(Position position) : base("Wand", position)
    {
    }

    public override char Symbol => '#';
}
