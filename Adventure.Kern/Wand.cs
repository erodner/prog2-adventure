namespace Adventure.Kern;

public sealed class Wand : StatischesObjekt
{
    public Wand(Koordinate position) : base("Wand", position)
    {
    }

    public override char Symbol => '#';
}
