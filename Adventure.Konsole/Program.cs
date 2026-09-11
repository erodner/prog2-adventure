using Adventure.Kern;

// Konsolenversion des Spiels. Steuerung: Pfeiltasten oder W A S D, Q beendet.
ILevelQuelle levelQuelle = new EingebauteLevel();
string levelName = args.Length > 0 ? args[0] : levelQuelle.LevelNamen[0];
Spielfeld feld = LevelParser.Parsen(levelQuelle.Laden(levelName));

feld.Spieler.SchatzGefunden += (sender, e) =>
{
    Console.Beep();
};

while (feld.Status == Spielstatus.Laeuft)
{
    Console.Clear();
    Console.WriteLine($"Level: {levelName}   Runde {feld.Runde}");
    Console.WriteLine(feld.AlsText());
    Console.WriteLine(feld.Spieler.Beschreibung());
    Console.WriteLine(feld.LetzteMeldung);
    Console.Write("Zug (Pfeiltasten/WASD, Q = Ende): ");

    ConsoleKey taste = Console.ReadKey(true).Key;
    if (taste == ConsoleKey.Q) return;

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
