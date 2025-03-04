## Changelog

2.1.7
* Fixed for Valheim 0.220.3

2.1.6
* Fixed a BepInEx cyclic dependency issue if certain mods are installed (AdventureBackpacks, Jewelcrafting, AAA_Crafting, AzuExtendedPlayerInventory and ContentsWithin)

2.1.5
* Fixed a conflict with Jewelcrafting where items can be moved out of sockets. Both TakeAll and PlaceStacks buttons are now always hidden on socketed items

2.1.4
* Added compatibility for 'Quick Stack - Store - Sort - Trash - Restock', hiding its chest buttons when hovering over a chest (thanks Goldenrevolver)

2.1.3
* Updated for Valheim 0.217.22, not compatible with older versions
* Updated and compiled for BepInExPack 5.4.2200

2.1.2
* Update for 0.217.14 (Hildir's Request), hiding the stack all button when hovering over a chest

2.1.1
* Update for Valheim 0.216.9

2.1.0
* Added an option to start the game with the container contents hidden
* Added a short delay before the UI is closed. This reduces the amount of animations when switching between chests. Can be adjusted in the config
* Removed patch of the invisible durability bar bug, this is now fixed in the vanilla game
* Internal code cleanup

2.0.2
* Fixed a conflict with [Jewelcrafting](https://valheim.thunderstore.io/package/Smoothbrain/Jewelcrafting/)

2.0.1
* Fixed invisible durability bar bug of chests
* Added compatibility for Auga

2.0.0
* Reworked code to not use transpiler but only rely on GUI methods
* Fixed the info panel and crafting panel were hidden on chest interaction
* Fixed items could be be moved in chest while in ESC menu
* Fixed mouse was shortly active after moving over a chest
* Changed Tab-key doesn't open the chest but the player inventory as normally. This behaves more vanilla like and doesn't accidentally blocks chests

1.0.1
* Add a check for the `Container.CheckAccess()` during hover.

1.0.0
* Initial release.
