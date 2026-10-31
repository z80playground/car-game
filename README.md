# Car Game

A two-player (or player-vs-computer) physics car game that runs in the browser: random levels, power-ups, special rounds, first to 5 wins the match. The full description of how it behaves is in [PRD.md](PRD.md).

## Play it online

https://z80playground.github.io/car-game/ - published from `CarGame/wwwroot` by the GitHub Pages workflow on every push to main.

## Run it

Needs the .NET 10 SDK.

```bash
dotnet run --project CarGame
```n
Then open http://localhost:5286. The page loads the Matter.js physics library from a CDN, so it needs an internet connection.
