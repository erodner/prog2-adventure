namespace Adventure.Kern;

/// <summary>Etwas, das der Spieler aufheben und im Inventar tragen kann.</summary>
public interface ISammelbar
{
    string Name { get; }

    /// <summary>Wird aufgerufen, sobald der Spieler das Objekt aufhebt.</summary>
    string Aufheben(Spieler spieler);
}
