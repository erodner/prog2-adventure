using System.Net.Http.Json;
using Adventure.Kern;

namespace Adventure.Daten;

/// <summary>
/// Lädt Level von einem Webserver. Unter der Basis-URL liegt eine index.json mit den
/// Levelnamen und pro Level eine Textdatei, z. B. https://.../levels/kerker.txt
/// </summary>
public class HttpLevelQuelle : ILevelQuelle
{
    private static readonly HttpClient http = new();
    private readonly string basisUrl;
    private readonly List<string> namen;

    private HttpLevelQuelle(string basisUrl, List<string> namen)
    {
        this.basisUrl = basisUrl;
        this.namen = namen;
    }

    /// <summary>Holt zuerst die Liste der Level – deshalb asynchron und über eine Fabrikmethode.</summary>
    public static async Task<HttpLevelQuelle> ErzeugenAsync(string basisUrl)
    {
        basisUrl = basisUrl.TrimEnd('/') + "/";
        List<string> namen = await http.GetFromJsonAsync<List<string>>(basisUrl + "index.json")
                             ?? new List<string>();
        return new HttpLevelQuelle(basisUrl, namen);
    }

    public IReadOnlyList<string> LevelNamen => namen;

    public Level Laden(string name)
    {
        // Synchron warten ist hier vertretbar: ein Level ist klein und wird einmal geladen.
        string text = http.GetStringAsync(basisUrl + name + ".txt").GetAwaiter().GetResult();
        List<string> zeilen = text.Split('\n')
            .Select(z => z.TrimEnd('\r'))
            .Where(z => z.Trim().Length > 0)
            .ToList();
        return new Level(name, zeilen);
    }
}
