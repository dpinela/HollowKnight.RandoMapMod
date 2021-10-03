# Randomizer Map S
Randomizer Map S is a Hollow Knight mod used with Randomizer Mod. It gives the option to show the player where items are on the World Map, and optionally what they are.

![Example Screenshot](./readmeAssets/worldmap.jpg)
![Example Screenshot](./readmeAssets/quickmap.jpg)
![Example Screenshot](./readmeAssets/GUI.PNG)

This fork of CaptainDapper's original mod has been expanded on with more features, bug fixes and a Pause Menu UI. It is currently compatible with:
- Randomizer v3.13(888) - recommended
- Randomizer v3.12c(884)
- Randomizer v3.12(573)
- Randomizer ItemSync 1.3.0
- Randomizer Multiworld 0.1.1

https://github.com/Shadudev/HollowKnight.RandomizerMod
https://github.com/Shadudev/HollowKnight.MultiWorld

# Features
- Use `CTRL-M` during a game to enable the mod and give all Maps to the player. This can also be done by talking to Elderbug a few times.

- The World Map will now show Pins for every item in the game.
    - Big Pins means the items are reachable according to RandomizerMod's logic
    - Small Pins means the items are not randomized or not reachable
    - Pins for randomized items will disappear as you check them
    - Randomizer Map's settings are displayed at the bottom
    - Quick Map can also be used to only show Pins in the current Map area

- The Pause Menu UI has the following buttons:
    - "Spoilers": Toggle Pins between vanilla (non-spoiler) and spoiler item pools
    - "Style": Toggle the style of the Pins
    - "Randomized": Toggle all randomized items on/off
    - "Others": Toggle all other items on/off (excluding Shops)
    - "Show/Hide Pools": Open/close a panel with a toggle for each spoiler item pool

- You can also use these hotkeys at any time during the game:
    - `CTRL-1`: "Spoilers"
    - `CTRL-2`: "Style"
    - `Ctrl-3`: "Randomized"
    - `Ctrl-4`: "Others"

The Pin settings are saved between game loads.

# How To Install
1. Make sure you have either RandomizerMod v3.12(573) or v3.12c(884) properly installed.
2. Download the latest release of `RandoMapMod.zip`.
3. Unzip and copy RandoMapMod.dll to the folder `...\Steam\steamapps\common\Hollow Knight\hollow_knight_Data\Managed\Mods`.
4. That's it!

# Acknowledgements
- The Hollow Knight/Hollow Knight Speedrun Discord Channels for always giving very sound advice and suggestions.
- CaptainDapper for making the original mod
- Chaktis for helping with sprite art

# Version 1.1.0 Changes
- Now with a UI in the Pause Menu! You can now manually toggle each item pool on/off.
- Also added a text overlay to the World Map and Quick Map
- Added the option to switch to old pin styles
- Changed the hotkeys to match the UI
- Quick Map now only shows Pins in the current Map area
- Fixed some Pins not showing
- Updated some Pin art
- Various bug fixes
- More code cleanup

# Known Bugs / Missing Features
- Not compatible when RandomizerMod isn't installed
- Non-randomized items don't get checked off on the Map
