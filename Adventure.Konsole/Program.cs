using Adventure.Daten;
using Adventure.Kern;

// Konsolenversion des Spiels. Pfeiltasten/WASD bewegen, F5 speichert, F9 lädt, Q beendet.
string levelOrdner = Path.Combine(AppContext.BaseDirectory, "levels");
ILevelQuelle levelQuelle = Directory.Exists(levelOrdner)
    ? new TextdateiLevelQuelle(levelOrdner)
    : new EingebauteLevelQuelle();
ISpielstandSpeicher speicher = new JsonSpielstandSpeicher("spielstand.json");

string levelName = args.Length > 0 ? args[0] : levelQuelle.LevelNamen[0];
Level level = levelQuelle.Laden(levelName);
Spielfeld feld = LevelParser.Parsen(level);
string hinweis = "";

feld.Spieler.SchatzGefunden += (sender, e) => Console.Beep();

while (feld.Status == Spielstatus.Laeuft)
{
    Console.Clear();
    Console.WriteLine($"Level: {levelName}   Runde {feld.Runde}");
    Console.WriteLine(feld.AlsText());
    Console.WriteLine(feld.Spieler.Beschreibung());
    Console.WriteLine(feld.LetzteMeldung);
    Console.WriteLine(hinweis);
    Console.Write("Zug (Pfeiltasten/WASD, F5 speichern, F9 laden, Q = Ende): ");
    hinweis = "";

    ConsoleKey taste = Console.ReadKey(true).Key;
    if (taste == ConsoleKey.Q) return;
    if (taste == ConsoleKey.F5)
    {
        speicher.Speichern(feld.Erfassen(levelName, level));
        hinweis = "Spielstand gespeichert.";
        continue;
    }
    if (taste == ConsoleKey.F9)
    {
        Spielstand? stand = speicher.Laden();
        if (stand is not null)
        {
            levelName = stand.LevelName;
            level = levelQuelle.Laden(levelName);
            feld = Spielfeld.Wiederherstellen(level, stand);
            feld.Spieler.SchatzGefunden += (sender, e) => Console.Beep();
            hinweis = "Spielstand geladen.";
        }
        else
        {
            hinweis = "Kein Spielstand vorhanden.";
        }
        continue;
    }

    Richtung? richtung = taste switch
    {
        ConsoleKey.W or ConsoleKey.UpArrow => Richtung.Oben,
        ConsoleKey.S or ConsoleKey.DownArrow => Richtung.Unten,
        ConsoleKey.A or ConsoleKey.LeftArrow => Richtung.Links,
        ConsoleKey.D or ConsoleKey.RightArrow => Richtung.Rechts,
        _ => null
    };
    if (richtung is Richtung r)
    {
        feld.SpielerZieht(r);
    }
}

Console.Clear();
Console.WriteLine(feld.AlsText());
Console.WriteLine(feld.LetzteMeldung);
Console.WriteLine(feld.Status == Spielstatus.Gewonnen
    ? $"Gewonnen! {feld.Spieler.Punkte} Punkte in {feld.Runde} Runden."
    : "Verloren. Versuch es noch einmal.");
