namespace Adventure.Kern;

/// <summary>Zwei Level fest im Code – solange wir noch keine Dateien lesen können.</summary>
public class EingebauteLevel : ILevelQuelle
{
    private static readonly Dictionary<string, string[]> level = new()
    {
        ["Kerker"] = new[]
        {
            "####################",
            "#@.....#...........#",
            "#......#.....W.....#",
            "#..k...#...........#",
            "#......D...........#",
            "#......#...V.......#",
            "#......#.......T...#",
            "#..!...#..........E#",
            "####################",
        },
        ["Katakomben"] = new[]
        {
            "########################",
            "#@..#........W.........#",
            "#...#..###########.....#",
            "#...#..#....k....#..V..#",
            "#...D..#..####...#.....#",
            "#...#..#..#T.#...D.....#",
            "#...#..#..#..#...#..!..#",
            "#...#..#..####...#.....#",
            "#...#..#.........#.....#",
            "#...#..###########....E#",
            "########################",
        },
    };

    public IReadOnlyList<string> LevelNamen => level.Keys.ToList();

    public Level Laden(string name)
    {
        if (!level.TryGetValue(name, out string[]? zeilen))
        {
            throw new KeyNotFoundException($"Es gibt kein Level namens '{name}'.");
        }
        return new Level(name, zeilen);
    }
}
