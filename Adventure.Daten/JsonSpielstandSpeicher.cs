using System.Text.Json;
using Adventure.Kern;

namespace Adventure.Daten;

/// <summary>Speichert den Spielstand als JSON-Datei.</summary>
public class JsonSpielstandSpeicher : ISpielstandSpeicher
{
    private static readonly JsonSerializerOptions optionen = new() { WriteIndented = true };
    private readonly string pfad;

    public JsonSpielstandSpeicher(string pfad)
    {
        this.pfad = pfad;
    }

    public void Speichern(Spielstand spielstand)
    {
        File.WriteAllText(pfad, JsonSerializer.Serialize(spielstand, optionen));
    }

    public Spielstand? Laden()
    {
        if (!File.Exists(pfad)) return null;
        return JsonSerializer.Deserialize<Spielstand>(File.ReadAllText(pfad), optionen);
    }
}
