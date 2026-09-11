# Adventure – das Beispielspiel zu „Programmierung 2“

Ein rundenbasiertes 2D-Dungeon-Spiel in C#, das die Vorlesung [Programmierung 2](https://www.erodner.de/prog2-lecture/) an der HTW Berlin begleitet. Es wächst von Vorlesung zu Vorlesung; die Git-Tags markieren den Stand nach der jeweiligen Vorlesung:

| Tag | Stand nach Vorlesung |
|---|---|
| `v01-vererbung` | 01 Vererbung – `Spielobjekt`, `Wand`, `Spieler`, ein Raum in der Konsole |
| `v02-interfaces` | 02 Abstrakte Klassen und Interfaces – spielbares Konsolenspiel mit Gegnern, Türen, Truhen |
| `v04-blazor` | 04 GUI mit Blazor – Weboberfläche und Schichten-Architektur |
| `v09-daten` | 09 Dateien, Streams und Serialisierung – Level aus Dateien, Spielstände als JSON |
| `v12-tests` | 12 Unit-Testing – NUnit-Tests (entspricht `main`) |

```bash
git checkout v02-interfaces        # Stand einer Vorlesung ansehen
git checkout main                  # zurück zum aktuellen Stand
dotnet run --project Adventure.Konsole
```

Voraussetzung: .NET SDK 10.

## Spielregeln

Du steuerst den Helden `@` mit den Pfeiltasten (oder W A S D). Finde den Schlüssel `k`, öffne die Tür `D`, hole den Schatz aus der Truhe `T` und erreiche den Ausgang `E`. Wachen `W` laufen ihre Route ab, Verfolger `V` nehmen die Jagd auf, sobald sie dich sehen. Jede Berührung kostet einen Lebenspunkt; Tränke `!` heilen.

Lizenz: CC BY 4.0 – Erik Rodner, HTW Berlin
