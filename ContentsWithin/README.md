# ContentsWithin


## About

Show the contents of any chests when hovering over it **using the existing container UI**.

![Showcase](https://raw.githubusercontent.com/MSchmoecker/ComfyMods/fa1ed01535dc93e83a9319a5d29a8641057017f5/ContentsWithin/ContentWithinPreview.png)

This mod is a reworked fork of [Redseiko's ContentsWithin](https://valheim.thunderstore.io/package/ComfyMods/ContentsWithin/), licensed under GPL-3.0, that doesn't transpile player methods but only patches GUI methods.

This change makes it compatible with [MultiUserChest](https://valheim.thunderstore.io/package/MSchmoecker/MultiUserChest/), [Auga](https://valheim.thunderstore.io/package/RandyKnapp/Auga/) and maybe even a few other mods.
Additionally it fixes a few minor bugs like the mouse beeing visible for shorts periods of time or being able to move items while the ESC manu is open.
See the changelog for more information.


## Manual Installation
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


## Links
- Thunderstore: https://valheim.thunderstore.io/package/MSchmoecker/ContentsWithin/
- Github: https://github.com/MSchmoecker/ComfyMods/tree/fork-upload/ContentsWithin/
- Discord: Margmas#9562. Feel free to DM or ping me in the [Jötunn discord](https://discord.gg/DdUt6g7gyA)


## Changelog

See [Changelog](https://github.com/MSchmoecker/ComfyMods/tree/fork-upload/ContentsWithin/CHANGELOG.md)
