namespace Adventure.Kern;

/// <summary>
/// Etwas, mit dem der Spieler etwas tun kann, wenn er davor steht:
/// eine Tür aufschließen, eine Truhe öffnen.
/// </summary>
public interface IInteragierbar
{
    /// <summary>Führt die Interaktion aus und liefert eine Meldung für den Spieler.</summary>
    string Interagieren(Spieler spieler);
}
