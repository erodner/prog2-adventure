using Adventure.Kern;

// Ein kleiner Raum, noch von Hand gebaut. Steuerung: W A S D, Q beendet.
Spieler held = new Spieler("Held", new Position(1, 1));
Spielfeld feld = new Spielfeld(10, 6, held);

for (int x = 0; x < feld.Breite; x++)
{
    feld.Hinzufuegen(new Wand(new Position(x, 0)));
    feld.Hinzufuegen(new Wand(new Position(x, feld.Hoehe - 1)));
}
for (int y = 1; y < feld.Hoehe - 1; y++)
{
    feld.Hinzufuegen(new Wand(new Position(0, y)));
    feld.Hinzufuegen(new Wand(new Position(feld.Breite - 1, y)));
}
feld.Hinzufuegen(new Wand(new Position(5, 2)));
feld.Hinzufuegen(new Wand(new Position(5, 3)));

while (true)
{
    Console.Clear();
    Console.WriteLine(feld.AlsText());
    Console.WriteLine(held.Beschreibung());
    Console.Write("Richtung (WASD, Q = Ende): ");

    ConsoleKey taste = Console.ReadKey(true).Key;
    Richtung? richtung = taste switch
    {
        ConsoleKey.W or ConsoleKey.UpArrow => Richtung.Oben,
        ConsoleKey.S or ConsoleKey.DownArrow => Richtung.Unten,
        ConsoleKey.A or ConsoleKey.LeftArrow => Richtung.Links,
        ConsoleKey.D or ConsoleKey.RightArrow => Richtung.Rechts,
        _ => null
    };

    if (taste == ConsoleKey.Q) break;
    if (richtung is Richtung r)
    {
        held.Bewegen(r, feld);
    }
}
