using Adventure.Kern;

namespace Adventure.Daten;

/// <summary>Liest Level aus Textdateien (*.txt) in einem Ordner – zeilenweise mit einem StreamReader.</summary>
public class TextdateiLevelQuelle : ILevelQuelle
{
    private readonly string ordner;

    public TextdateiLevelQuelle(string ordner)
    {
        if (!Directory.Exists(ordner))
        {
            throw new DirectoryNotFoundException($"Level-Ordner '{ordner}' nicht gefunden.");
        }
        this.ordner = ordner;
    }

    public IReadOnlyList<string> LevelNamen =>
        Directory.GetFiles(ordner, "*.txt")
            .Select(Path.GetFileNameWithoutExtension)
            .OrderBy(n => n)
            .ToList()!;

    public Level Laden(string name)
    {
        string pfad = Path.Combine(ordner, name + ".txt");
        if (!File.Exists(pfad))
        {
            throw new FileNotFoundException($"Es gibt kein Level namens '{name}'.", pfad);
        }

        List<string> zeilen = new();
        using StreamReader leser = new(pfad);
        string? zeile;
        while ((zeile = leser.ReadLine()) != null)
        {
            if (zeile.Trim().Length > 0) zeilen.Add(zeile.TrimEnd());
        }
        return new Level(name, zeilen);
    }
}
