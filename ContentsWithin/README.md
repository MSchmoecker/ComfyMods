# ContentsWithin


## About

Show the contents of any chests when hovering over it **using the existing container UI**.

![Showcase](https://github.com/MSchmoecker/ComfyMods/blob/fa1ed01535dc93e83a9319a5d29a8641057017f5/ContentsWithin/ContentWithinPreview.png)

This is a reworked fork of [Redseiko's ContentsWithin](https://valheim.thunderstore.io/package/ComfyMods/ContentsWithin/), licensed under GPL-3.0,
that doesn't transpile player methods but only patches GUI methods. This makes it compatible with [MultiUserChest](https://valheim.thunderstore.io/package/MSchmoecker/MultiUserChest/)
and maybe even a few other mods.

## Installation
This mod requires [BepInEx](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/).
Extract all files to `BepInEx/plugins/ContentsWithin`


## Instructions

  * Launch the game and go hover over any chest.
  * You can interact with the chest regularly and it should bring up the regular player inventory when you are the chest instance owner.
  * You can press hot-key `RightShift + P` (configurable) to toggle off/on the "show container contents" feature.
    * Turning this off/on (as well as toggling `isModEnabled`) while looking at a chest can cause weird UI behaviour.


## Notes

  * The info panel, crafting panel and player inventory panel are hidden when in hovering mode.
  * Hovering works when another player has the chest open and updates the contents being changed.
  * Opening a chest of which you are not instance owner of will try to go through the "chest" interact sequence and may
    take a moment to activate.

## Changelog

2.0.0
  * Reworked code to not use transpiler but only rely on GUI methods
  * Fixed the info panel and crafting panel are hidden on chest interaction.
  * Fixed items can be moved in chest while in ESC menu
  * Changed Tab-key doesn't open the chest but the player inventory as normally. This behaves more vanilla like and doesn't accidentally blocks chests

1.0.1
  * Add a check for the `Container.CheckAccess()` during hover.

1.0.0
  * Initial release.