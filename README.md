# Randomizer Map S

This is a fork of Rando Map Mod v0.5.1, with some bug fixes and new features.

This has been tested with both RandomizerMod v3.12(573) and v3.12c(884) (the Discord beta).

In addition, the mod seems to be compatible with both Multiworld 0.1.1 and ItemSync 1.2.3, however further testing may be required.

![Example Screenshot](./readmeAssets/vanillavsrando.png)

# Features
- Use `CTRL-M` to give all Maps to the player + Quill. This can also be done by talking to Elderbug a few times.
- Use `CTRL-T` to toggle Pins between vanilla and randomizer item pools
- Use `CTRL-R` to toggle all Pins to question marks
- Use `CTRL-P` to toggle all Pins on/off
- Use `CTRL-1` ... `CTRL-6` to toggle Pin groups on/off
    - Ctrl + 1 - Toggles major progression items/skills
    - Ctrl + 2 - Toggles Mask Shards and Vessel Fragments
    - Ctrl + 3 - Toggles Charms and Charm Notches
    - Ctrl + 4 - Toggles Grubs, Essence Roots and Boss Essence
    - Ctrl + 5 - Toggles Relics, Eggs, Geo Deposits and Boss Geo
    - Ctrl + 6 - Toggles everything else

These settings are saved between game loads.

# Changelog
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
    - No Multiworld support for now (shold be easy to add later)
    - CTRL-M now gives you all Maps + Quill, but no Wayward Compass
    - Disabled other Pin styles
    - Disabled prereq markers (!)

# Known Bugs
- Mod load fails upon first try. Reopening the game should fix it.
- Shop Pin doesn't disappear when it is exhausted of items, but earlier instead
- Some Pins might not be in the right place, or don't appear at all
- The "reachable" Pins are based on the items found in the RandomizerHelperLog.txt. These don't include items that aren't randomized.
