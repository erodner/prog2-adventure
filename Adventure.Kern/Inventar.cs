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
        return inhalt.Any(d => d is TArt);
    }

    public bool Entfernen<TArt>() where TArt : T
    {
        T? treffer = inhalt.FirstOrDefault(d => d is TArt);
        return treffer is not null && inhalt.Remove(treffer);
    }

    public IEnumerator<T> GetEnumerator() => inhalt.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override string ToString()
    {
        return inhalt.Count == 0 ? "leer" : string.Join(", ", inhalt.Select(d => d.Name));
    }
}
