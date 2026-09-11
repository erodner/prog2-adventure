namespace Adventure.Kern;

/// <summary>
/// Alles, was man braucht, um ein laufendes Spiel später fortzusetzen.
/// Bewusst nur Daten, keine Logik – so lässt es sich als JSON speichern.
/// </summary>
public class Spielstand
{
    public string LevelName { get; set; } = "";
    public int Runde { get; set; }
    public Position SpielerPosition { get; set; }
    public int Lebenspunkte { get; set; }
    public int Punkte { get; set; }
    public List<string> Inventar { get; set; } = new();
    public List<Position> EntfernteGegenstaende { get; set; } = new();
    public List<Position> OffeneTueren { get; set; } = new();
    public List<Position> GeoeffneteTruhen { get; set; } = new();
    public List<Position> GegnerPositionen { get; set; } = new();
}

/// <summary>Wo Spielstände landen (Datei, Browser, Datenbank), ist dem Spiel egal.</summary>
public interface ISpielstandSpeicher
{
    void Speichern(Spielstand spielstand);
    Spielstand? Laden();
}
