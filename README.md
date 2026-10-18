# Life Sim RPG

A text-based open-world life simulator. The terminal is the game interface; OMIP will be a separate overlay that displays the player's location.

## Current setup

- C# / .NET console application
- `Program.cs` contains the main menu and start page only
- `World.cs` contains the new-world / character-start flow
- Character data is represented by individual variables and integer state flags, not a character class
- Saves are JSON files named from the character name, stored under `saves/`
- Selecting an existing character name loads that character's JSON save; a new name creates a new save

## Run

Install the .NET 8 SDK, then run:

```sh
dotnet run
```

The Options and Credits menu entries are intentionally placeholders and return to the menu.