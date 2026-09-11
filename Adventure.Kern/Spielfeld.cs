using System.Text;

namespace Adventure.Kern;

public class RundeEventArgs : EventArgs
{
    public int Runde { get; }
    public string Meldung { get; }

    public RundeEventArgs(int runde, string meldung)
    {
        Runde = runde;
        Meldung = meldung;
    }
}

/// <summary>
/// Das Raster mit allen Objekten und die Spielregeln einer Runde:
/// erst zieht der Spieler, dann jeder Gegner.
/// </summary>
public class Spielfeld
{
    public int Breite { get; }
    public int Hoehe { get; }
    public Spieler Spieler { get; }
    public int Runde { get; private set; }
    public Spielstatus Status { get; private set; } = Spielstatus.Laeuft;
    public string LetzteMeldung { get; private set; } = "";

    // Statische Objekte nach Position: schneller Zugriff beim Zeichnen und bei Kollisionen.
    private readonly Dictionary<Position, StatischesObjekt> statische = new();
    private readonly List<Gegner> gegner = new();

    /// <summary>Wird nach jeder Runde ausgelöst – die Oberfläche zeichnet dann neu.</summary>
    public event EventHandler<RundeEventArgs>? RundeBeendet;

    public Spielfeld(int breite, int hoehe, Spieler spieler)
    {
        Breite = breite;
        Hoehe = hoehe;
        Spieler = spieler;
    }

    public IReadOnlyList<Gegner> Gegner => gegner;

    /// <summary>Alle Objekte auf dem Feld – erst die statischen, dann die Gegner, zuletzt der Spieler.</summary>
    public IEnumerable<Spielobjekt> AlleObjekte
    {
        get
        {
            foreach (StatischesObjekt s in statische.Values) yield return s;
            foreach (Gegner g in gegner) yield return g;
            yield return Spieler;
        }
    }

    public void Hinzufuegen(Spielobjekt objekt)
    {
        switch (objekt)
        {
            case StatischesObjekt s:
                statische[s.Position] = s;
                break;
            case Gegner g:
                gegner.Add(g);
                break;
            case Adventure.Kern.Spieler _:
                throw new ArgumentException("Der Spieler wird dem Spielfeld im Konstruktor übergeben.");
        }
    }

    public void Entfernen(Spielobjekt objekt)
    {
        if (objekt is StatischesObjekt s) statische.Remove(s.Position);
        if (objekt is Gegner g) gegner.Remove(g);
    }

    public StatischesObjekt? StatischesObjektAn(Position p)
    {
        return statische.TryGetValue(p, out StatischesObjekt? s) ? s : null;
    }

    public Spielobjekt? ObjektAn(Position p)
    {
        if (p == Spieler.Position) return Spieler;
        Gegner? g = gegner.FirstOrDefault(x => x.Position == p);
        return g ?? (Spielobjekt?)StatischesObjektAn(p);
    }

    public bool IstInnerhalb(Position p)
    {
        return p.X >= 0 && p.Y >= 0 && p.X < Breite && p.Y < Hoehe;
    }

    /// <summary>Frei = im Feld, kein blockierendes statisches Objekt, kein bewegliches Objekt.</summary>
    public bool IstFrei(Position p)
    {
        if (!IstInnerhalb(p)) return false;
        StatischesObjekt? s = StatischesObjektAn(p);
        if (s is not null && !s.IstPassierbar) return false;
        if (p == Spieler.Position) return false;
        return gegner.All(g => g.Position != p);
    }

    /// <summary>Gibt es eine gerade Linie ohne Wände zwischen zwei Feldern? (Bresenham-Algorithmus)</summary>
    public bool HatSichtlinie(Position von, Position nach)
    {
        int x = von.X, y = von.Y;
        int dx = Math.Abs(nach.X - x), dy = -Math.Abs(nach.Y - y);
        int sx = x < nach.X ? 1 : -1, sy = y < nach.Y ? 1 : -1;
        int fehler = dx + dy;

        while (x != nach.X || y != nach.Y)
        {
            int f2 = 2 * fehler;
            if (f2 >= dy) { fehler += dy; x += sx; }
            if (f2 <= dx) { fehler += dx; y += sy; }

            Position p = new(x, y);
            if (p == nach) break;
            StatischesObjekt? s = StatischesObjektAn(p);
            if (s is not null && !s.IstPassierbar) return false;
        }
        return true;
    }

    /// <summary>Eine Spielrunde: Spieler zieht, hebt auf oder interagiert; danach ziehen die Gegner.</summary>
    public void SpielerZieht(Richtung richtung)
    {
        if (Status != Spielstatus.Laeuft) return;

        Runde++;
        StringBuilder meldung = new();
        Position ziel = Spieler.Position.Verschoben(richtung);
        StatischesObjekt? davor = StatischesObjektAn(ziel);

        if (davor is IInteragierbar interagierbar && !davor.IstPassierbar)
        {
            // Vor einer verschlossenen Tür oder einer Truhe: interagieren statt gehen.
            meldung.Append(interagierbar.Interagieren(Spieler));
        }
        else if (Spieler.Bewegen(richtung, this))
        {
            if (davor is Gegenstand gegenstand)
            {
                meldung.Append(gegenstand.Aufheben(Spieler));
                statische.Remove(gegenstand.Position);
                if (gegenstand is not Trank) Spieler.Inventar.Hinzufuegen(gegenstand);
            }
            if (davor is Ausgang)
            {
                Status = Spielstatus.Gewonnen;
                meldung.Append("Du hast den Ausgang erreicht!");
            }
        }
        else
        {
            meldung.Append("Da geht es nicht weiter.");
        }

        if (Status == Spielstatus.Laeuft)
        {
            GegnerZiehen(meldung);
        }

        LetzteMeldung = meldung.ToString().Trim();
        RundeBeendet?.Invoke(this, new RundeEventArgs(Runde, LetzteMeldung));
    }

    private void GegnerZiehen(StringBuilder meldung)
    {
        foreach (Gegner g in gegner)
        {
            Richtung? zug = g.NaechsterZug(this);
            if (zug is Richtung r)
            {
                Position ziel = g.Position.Verschoben(r);
                if (ziel == Spieler.Position)
                {
                    Spieler.SchadenNehmen();
                    meldung.Append($" {g.Name} erwischt dich!");
                }
                else
                {
                    g.Bewegen(r, this);
                }
            }
        }

        if (!Spieler.IstAmLeben)
        {
            Status = Spielstatus.Verloren;
            meldung.Append(" Du bist besiegt.");
        }
    }

    /// <summary>Fasst den aktuellen Zustand als Spielstand zusammen.</summary>
    public Spielstand Erfassen(string levelName, Level original)
    {
        Spielfeld frisch = LevelParser.Parsen(original);
        Spielstand stand = new()
        {
            LevelName = levelName,
            Runde = Runde,
            SpielerPosition = Spieler.Position,
            Lebenspunkte = Spieler.Lebenspunkte,
            Punkte = Spieler.Punkte,
            Inventar = Spieler.Inventar.Select(g => g.Name).ToList(),
            GegnerPositionen = gegner.Select(g => g.Position).ToList(),
        };
        foreach (StatischesObjekt s in frisch.statische.Values)
        {
            StatischesObjekt? jetzt = StatischesObjektAn(s.Position);
            if (s is Gegenstand && jetzt is null) stand.EntfernteGegenstaende.Add(s.Position);
            if (jetzt is Tuer t && t.IstOffen) stand.OffeneTueren.Add(t.Position);
            if (jetzt is Truhe tr && tr.IstGeoeffnet) stand.GeoeffneteTruhen.Add(tr.Position);
        }
        return stand;
    }

    /// <summary>Baut das Level frisch auf und spielt den Spielstand darüber.</summary>
    public static Spielfeld Wiederherstellen(Level level, Spielstand stand)
    {
        Spielfeld feld = LevelParser.Parsen(level);
        feld.Runde = stand.Runde;
        feld.Spieler.Versetzen(stand.SpielerPosition);
        feld.Spieler.Wiederherstellen(stand.Lebenspunkte, stand.Punkte);

        foreach (Position p in stand.EntfernteGegenstaende)
        {
            if (feld.StatischesObjektAn(p) is Gegenstand g)
            {
                feld.statische.Remove(p);
                if (g is Schluessel && stand.Inventar.Contains(g.Name)) feld.Spieler.Inventar.Hinzufuegen(g);
            }
        }
        foreach (Position p in stand.OffeneTueren)
        {
            if (feld.StatischesObjektAn(p) is Tuer t) t.Aufschliessen();
        }
        foreach (Position p in stand.GeoeffneteTruhen)
        {
            if (feld.StatischesObjektAn(p) is Truhe t) t.AlsGeoeffnetMarkieren();
        }
        for (int i = 0; i < Math.Min(stand.GegnerPositionen.Count, feld.gegner.Count); i++)
        {
            feld.gegner[i].Versetzen(stand.GegnerPositionen[i]);
        }
        return feld;
    }

    /// <summary>Zeichnet das Spielfeld als Text – Zeile für Zeile.</summary>
    public string AlsText()
    {
        StringBuilder sb = new();
        for (int y = 0; y < Hoehe; y++)
        {
            for (int x = 0; x < Breite; x++)
            {
                sb.Append(ObjektAn(new Position(x, y))?.Symbol ?? '.');
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }
}
