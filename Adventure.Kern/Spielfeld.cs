using System.Text;

namespace Adventure.Kern;

/// <summary>Das Raster mit allen Objekten. Noch ganz einfach: eine Liste.</summary>
public class Spielfeld
{
    public int Breite { get; }
    public int Hoehe { get; }
    public Spieler Spieler { get; }

    private readonly List<Spielobjekt> objekte = new();

    public Spielfeld(int breite, int hoehe, Spieler spieler)
    {
        Breite = breite;
        Hoehe = hoehe;
        Spieler = spieler;
    }

    public void Hinzufuegen(Spielobjekt objekt)
    {
        objekte.Add(objekt);
    }

    public Spielobjekt? ObjektAn(Position position)
    {
        foreach (Spielobjekt o in objekte)
        {
            if (o.Position == position) return o;
        }
        return null;
    }

    public bool IstInnerhalb(Position p)
    {
        return p.X >= 0 && p.Y >= 0 && p.X < Breite && p.Y < Hoehe;
    }

    public bool IstFrei(Position p)
    {
        if (!IstInnerhalb(p)) return false;
        Spielobjekt? o = ObjektAn(p);
        return o is null || o.IstPassierbar;
    }

    /// <summary>Zeichnet das Spielfeld als Text – Zeile für Zeile.</summary>
    public string AlsText()
    {
        StringBuilder sb = new();
        for (int y = 0; y < Hoehe; y++)
        {
            for (int x = 0; x < Breite; x++)
            {
                Position p = new(x, y);
                char zeichen = p == Spieler.Position ? Spieler.Symbol : ObjektAn(p)?.Symbol ?? '.';
                sb.Append(zeichen);
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }
}
