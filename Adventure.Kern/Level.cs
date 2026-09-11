namespace Adventure.Kern;

/// <summary>Ein Level ist eine Karte aus Textzeilen plus ein Name.</summary>
public record Level(string Name, IReadOnlyList<string> Zeilen);

/// <summary>Woher die Level kommen, ist dem Spiel egal – Datei, Netz oder fest im Code.</summary>
public interface ILevelQuelle
{
    IReadOnlyList<string> LevelNamen { get; }
    Level Laden(string name);
}
