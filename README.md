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
dotnet run --project Adventure.Web  # dieselbe Spiellogik im Browser
```

Voraussetzung: .NET SDK 10.

## Spielregeln

Du steuerst den Helden `@` mit den Pfeiltasten oder mit W A S D. In der Konsole speicherst du mit F5, lädst mit F9 und beendest mit Q.

Dein Ziel: Finde den Schlüssel `k`, schließe damit die Tür `D` auf, plündere die Truhe `T` und erreiche den Ausgang `E`.

Eine Runde läuft immer gleich ab: Erst ziehst du, danach zieht jeder Gegner einmal. Läufst du gegen eine verschlossene Tür oder eine Truhe, gehst du nicht – du interagierst mit ihr. Gegenstände hebst du auf, indem du über sie läufst.

| Zeichen | Bedeutung | Regel |
|---|---|---|
| `@` | Held | startet mit 3 Lebenspunkten |
| `#` | Wand | blockiert Bewegung und Sichtlinie |
| `D` / `/` | Tür (zu / offen) | braucht einen Schlüssel, der dabei verbraucht wird |
| `T` / `t` | Truhe (zu / geplündert) | enthält einen Schatz im Wert von 100 Punkten |
| `k` | Schlüssel | wandert ins Inventar |
| `!` | Trank | wird sofort getrunken und heilt 1 Lebenspunkt (höchstens bis 3) |
| `$` | Schatz | 25 Punkte |
| `E` | Ausgang | Betreten gewinnt das Level |
| `W` | Wache | läuft geradeaus und dreht um, wenn sie anstößt |
| `V` | Verfolger | nimmt die Jagd auf, sobald du höchstens 5 Felder entfernt und in Sichtlinie bist |
| `.` | Boden | frei begehbar |

Zieht ein Gegner auf dein Feld, kostet dich das einen Lebenspunkt. Bei null Lebenspunkten ist das Spiel verloren.

## Mitmachen

Das Spiel ist absichtlich klein gehalten, damit du es erweitern kannst. Die Übungsaufgaben der [Kurswebseite](https://www.erodner.de/prog2-lecture/) führen dich durch genau diese Erweiterungen:

- **Eine neue Gegnerart:** ein Bogenschütze, der stehen bleibt und schießt, sobald du in gerader Linie vor ihm stehst. Neue Klasse in `Adventure.Kern/Gegner.cs`, ein Kartenzeichen im `LevelParser` und eine Zeile im Rundenablauf von `Spielfeld` – mehr braucht es nicht, weil `Gegner.NaechsterZug` schon der Vertrag für jedes Verhalten ist.
- **Eine Falle:** ein statisches Objekt, das man betreten kann und das beim ersten Betreten Schaden macht. Führt auf die Frage, ob dafür eine Basisklasse oder ein neues Interface (`IBetretbar`) das richtige Mittel ist.
- **Ein zweites Level:** eine weitere Textkarte in `levels/` und ein Eintrag in `levels/index.json`. Danach kannst du Levelwechsel nach dem Ausgang einbauen.

Vorgehen: Branch anlegen, klein und oft committen, Pull Request stellen. Für jede Erweiterung gehört ein NUnit-Test in `Adventure.Tests` dazu.

## Bauen und Testen

```bash
dotnet build
dotnet test
```

Beides läuft auch automatisch: Bei jedem Push auf `main` und bei jedem Pull Request baut und testet GitHub Actions das Projekt (`.github/workflows/dotnet.yml`). Bleibt der Haken grün, passt deine Erweiterung zum Rest.

## Projekte

| Projekt | Inhalt |
|---|---|
| `Adventure.Kern` | Spielobjekte, Spielfeld und Regeln – kennt keine Oberfläche |
| `Adventure.Daten` | Level aus Dateien und aus dem Netz, Spielstände als JSON |
| `Adventure.Konsole` | Textoberfläche |
| `Adventure.Web` | Blazor-Weboberfläche |
| `Adventure.Tests` | NUnit-Tests |

Lizenz: CC BY 4.0 – Erik Rodner, HTW Berlin
