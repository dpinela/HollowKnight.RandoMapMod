# Randomizer Map S

This is a fork of Rando Map Mod v0.5.1, with some bug fixes and new features.

This has been tested with both RandomizerMod v3.12(573) and v3.12c(884) (the Discord beta).

In addition, the mod seems to be compatible with both Multiworld 0.1.1 and ItemSync 1.2.3, however further testing may be required.

![Example Screenshot](./readmeAssets/vanillavsrando.png)

# Features
- Use `CTRL-M` to give all Maps to the player. This can also be done by talking to Elderbug a few times.
- Use `CTRL-T` to toggle Pins between vanilla (non-spoiler) and spoiler item pools
- Use `CTRL-R` to toggle all Pins to question marks
- Use `CTRL-P` to toggle all Pins on/off
- Use `CTRL-1` ... `CTRL-6` to toggle Pin groups on/off (sorted by spoiler item pools):
    - Ctrl + 1 - Toggles major progression items/skills
    - Ctrl + 2 - Toggles Mask Shards and Vessel Fragments
    - Ctrl + 3 - Toggles Charms and Charm Notches
    - Ctrl + 4 - Toggles Grubs, Essence Roots and Boss Essence
    - Ctrl + 5 - Toggles Relics, Eggs, Geo Deposits and Boss Geo
    - Ctrl + 6 - Toggles everything else

These settings are saved between game loads.

# Acknowledgements
A BIG thank you to the Hollow Knight/Hollow Knight Speedrun Discord Channels for always giving very sound advice and suggestions!

# Changelog
Version 1.0.6:
- Fully implemented new items in RandomizerMod v3.12c(884) (Junk Pit, Boss Geo, Mimics, etc.)
- All the pins have been clean-sweeped and are positioned better on the map
- The pins when reachable are now slightly smaller for better visibility
- Using Quick Map also properly updates the Pins
- Lots more code cleanup

Version 1.0.5:
- Quill is no longer given
- Fixed the broken map bug
- Elderbug will revert to normal behaviour after all maps are given

Version 1.0.4:
- Fixed the "FAILED TO LOAD" bug when the log file wasn't present
- Lots of code cleanup

Version 1.0.3:
- Added new items in v3.12(884). However the new Pin locations haven't been added yet.

Version 1.0.2:
- Fixed the hovering map markers bug
- The state of the Pin groups and Rando/Unknown sprite settings are saved between game loads

Version 1.0.1:
- Correct Pins show up when a new game is loaded

Version 1.0.0:
- Reachable logic applies to the shop Pins (kinda)
- Fixed some question mark Pins appearing by default

- Removed some stuff (based on my own preference/code clashes):
    - No Wayward Compass given
    - Disabled other Pin styles
    - Disabled prereq markers (!)

# Known Bugs / Missing Features
- Not compatible when RandomizerMod isn't installed
- Missing an option to switch to old question mark style pins
- Non-randomized items don't get checked off on the map
