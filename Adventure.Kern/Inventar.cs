using System.Collections;

namespace Adventure.Kern;

/// <summary>
/// Ein Behälter für sammelbare Dinge. Generisch, damit es Inventare für
/// Gegenstände, aber auch für Zaubersprüche oder Aufträge geben kann.
/// </summary>
public class Inventar<T> : IEnumerable<T> where T : ISammelbar
{
    private readonly List<T> inhalt = new();

    public int Anzahl => inhalt.Count;

    public void Hinzufuegen(T ding)
    {
        inhalt.Add(ding);
    }

    public bool Enthaelt<TArt>() where TArt : T
    {
        foreach (T ding in inhalt)
        {
            if (ding is TArt) return true;
        }
        return false;
    }

    public bool Entfernen<TArt>() where TArt : T
    {
        foreach (T ding in inhalt)
        {
            if (ding is TArt) return inhalt.Remove(ding);
        }
        return false;
    }

    public IEnumerator<T> GetEnumerator() => inhalt.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override string ToString()
    {
        if (inhalt.Count == 0) return "leer";
        List<string> namen = new();
        foreach (T ding in inhalt)
        {
            namen.Add(ding.Name);
        }
        return string.Join(", ", namen);
    }
}
