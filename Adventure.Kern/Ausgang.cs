namespace Adventure.Kern;

/// <summary>Wer den Ausgang betritt, hat das Level geschafft.</summary>
public sealed class Ausgang : StatischesObjekt
{
    public Ausgang(Position position) : base("Ausgang", position)
    {
    }

    public override char Symbol => 'E';
    public override bool IstPassierbar => true;
}
