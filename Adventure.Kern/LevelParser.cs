namespace Adventure.Kern;

/// <summary>
/// Baut aus einer Textkarte ein Spielfeld.
/// #  Wand   D Tür   T Truhe   k Schlüssel   ! Trank   $ Schatz   E Ausgang
/// @  Spieler   W Wache   V Verfolger   .  Boden
/// </summary>
public static class LevelParser
{
    public static Spielfeld Parsen(Level level)
    {
        IReadOnlyList<string> zeilen = level.Zeilen;
        if (zeilen.Count == 0)
        {
            throw new ArgumentException("Das Level ist leer.");
        }

        int breite = zeilen.Max(z => z.Length);
        Position? spielerStart = null;
        List<(char zeichen, Position pos)> felder = new();

        for (int y = 0; y < zeilen.Count; y++)
        {
            for (int x = 0; x < zeilen[y].Length; x++)
            {
                Position p = new(x, y);
                char c = zeilen[y][x];
                if (c == '@')
                {
                    spielerStart = p;
                }
                else if (c != '.' && c != ' ')
                {
                    felder.Add((c, p));
                }
            }
        }

        if (spielerStart is null)
        {
            throw new ArgumentException("Das Level enthält keinen Spieler (@).");
        }

        Spielfeld feld = new(breite, zeilen.Count, new Spieler("Held", spielerStart.Value));
        foreach ((char zeichen, Position pos) in felder)
        {
            feld.Hinzufuegen(ObjektFuer(zeichen, pos));
        }
        return feld;
    }

    private static Spielobjekt ObjektFuer(char zeichen, Position pos)
    {
        return zeichen switch
        {
            '#' => new Wand(pos),
            'D' => new Tuer(pos),
            'T' => new Truhe(pos, wert: 100),
            'k' => new Schluessel(pos),
            '!' => new Trank(pos),
            '$' => new Schatz(pos, wert: 25),
            'E' => new Ausgang(pos),
            'W' => new Wache(pos),
            'V' => new Verfolger(pos),
            _ => throw new ArgumentException($"Unbekanntes Zeichen '{zeichen}' bei {pos}.")
        };
    }
}
