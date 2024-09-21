using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System.Reflection;
using BepInEx.Bootstrap;
using UnityEngine;

namespace ContentsWithin {
  [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
  public class ContentsWithin : BaseUnityPlugin {
    public const string PluginGUID = "com.maxsch.valheim.contentswithin";
    public const string PluginName = "ContentsWithin";
    public const string PluginVersion = "2.1.5";

    private static ConfigEntry<bool> isModEnabled;
    private static ConfigEntry<bool> startHidden;
    private static ConfigEntry<KeyboardShortcut> toggleShowContentsShortcut;
    private static ConfigEntry<float> openDelayTime;

    private static bool isRealGuiVisible;
    private static bool showContent = true;
    private static float delayTime;
    private static Inventory emptyInventory = new Inventory("", null, 0, 0);

    private static HashSet<InventoryGrid> initializedGrids = new HashSet<InventoryGrid>();

    private static Container lastHoverContainer;
    private static GameObject lastHoverObject;

    private static GameObject inventoryPanel;
    private static GameObject infoPanel;
    private static GameObject craftingPanel;
    private static GameObject takeAllButton;
    private static GameObject stackAllButton;

    private Harmony harmony;

    private static FieldInfo? jewelcraftingOpenInventoryField;

    private static FieldInfo? GetJewelcraftingOpenInventoryField() {
      Type addFakeSocketsContainerType = AccessTools.TypeByName("Jewelcrafting.GemStones+AddFakeSocketsContainer");
      return AccessTools.Field(addFakeSocketsContainerType, "openInventory");
    }

    public void Awake() {
      isModEnabled = Config.Bind("_Global", "isModEnabled", true, "Globally enable or disable this mod. When disabled, no container contents will be shown and hotkeys will not work.");
      startHidden = Config.Bind("Settings", "startHidden", false, "Hide the container contents until the hotkey is pressed.");
      openDelayTime = Config.Bind("Settings", "openDelayTime", 0.3f, "Time before the UI is closed when not hovering over a chest. This reduces the amount of animations when switching between chests.");
      toggleShowContentsShortcut = Config.Bind("Hotkeys", "toggleShowContentsShortcut", new KeyboardShortcut(KeyCode.P, KeyCode.RightShift), "Shortcut to toggle on/off the 'show container contents' feature.");

      showContent = !startHidden.Value;

      harmony = new Harmony(PluginGUID);
      harmony.PatchAll();
    }

    private void Start() {
      if (Chainloader.PluginInfos.ContainsKey("org.bepinex.plugins.jewelcrafting")) {
        jewelcraftingOpenInventoryField = GetJewelcraftingOpenInventoryField();
      }
    }

    private void Update() {
      if (!isModEnabled.Value) {
        return;
      }

      if (toggleShowContentsShortcut.Value.IsDown()) {
        showContent = !showContent;

        if (MessageHud.instance) {
          MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, $"ShowContainerContents: {showContent}");
        }

        if (!showContent && !isRealGuiVisible && InventoryGui.instance) {
          InventoryGui.instance.Hide();
        }
      }
    }

    private static bool ShowRealGUI() {
      return !isModEnabled.Value || !showContent || isRealGuiVisible;
    }

    [HarmonyPatch(typeof(Player))]
    public class PlayerPatch {
      [HarmonyPatch(nameof(Player.UpdateHover)), HarmonyPostfix]
      public static void UpdateHoverPostfix(Player __instance) {
        if (!isModEnabled.Value || lastHoverObject == __instance.m_hovering) {
          return;
        }

        lastHoverObject = __instance.m_hovering;
        lastHoverContainer = lastHoverObject ? lastHoverObject.GetComponentInParent<Container>() : null;
      }
    }

    [HarmonyPatch(typeof(InventoryGrid))]
    public static class InventoryGridPatch {
      [HarmonyPatch(nameof(InventoryGrid.Awake)), HarmonyPostfix]
      public static void AwakePostfix(InventoryGrid __instance) {
        initializedGrids.Add(__instance);
      }
    }

    [HarmonyPatch(typeof(InventoryGui))]
    public class InventoryGuiPatch {
      [HarmonyPatch(nameof(InventoryGui.Awake)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
      public static void AwakePostfix(ref InventoryGui __instance) {
        inventoryPanel = __instance.m_player.Ref()?.gameObject;
        infoPanel = __instance.m_infoPanel.Ref()?.gameObject;
        craftingPanel = __instance.m_inventoryRoot.Find("Crafting").Ref()?.gameObject;
        takeAllButton = __instance.m_takeAllButton.Ref()?.gameObject;
        stackAllButton = __instance.m_stackAllButton.Ref()?.gameObject;

        if (Chainloader.PluginInfos.ContainsKey("randyknapp.mods.auga")) {
          craftingPanel = __instance.m_inventoryRoot.Find("RightPanel").Ref()?.gameObject;
        }
      }

      [HarmonyPatch(nameof(InventoryGui.Show)), HarmonyPostfix]
      public static void ShowPostfix() {
        isRealGuiVisible = true;
      }

      [HarmonyPatch(nameof(InventoryGui.Hide)), HarmonyPostfix]
      public static void HidePostfix() {
        isRealGuiVisible = false;
      }

      [HarmonyPatch(nameof(InventoryGui.Update)), HarmonyPrefix]
      public static void UpdatePrefix(InventoryGui __instance) {
        if (!ShowRealGUI()) {
          __instance.m_animator.SetBool("visible", false);
        }
      }

      [HarmonyPatch(nameof(InventoryGui.Update)), HarmonyPostfix]
      public static void UpdatePostfix(InventoryGui __instance) {
        bool showStackButtons = true;

        if (Chainloader.PluginInfos.ContainsKey("org.bepinex.plugins.jewelcrafting")) {
          Inventory? openInventory = jewelcraftingOpenInventoryField?.GetValue(null) as Inventory;
          showStackButtons = openInventory == null;
        }

        inventoryPanel.Ref()?.SetActive(ShowRealGUI());
        craftingPanel.Ref()?.SetActive(ShowRealGUI());
        infoPanel.Ref()?.SetActive(ShowRealGUI());
        takeAllButton.Ref()?.SetActive(showStackButtons && ShowRealGUI());
        stackAllButton.Ref()?.SetActive(showStackButtons && ShowRealGUI());

        if (takeAllButton && Chainloader.PluginInfos.ContainsKey("goldenrevolver.quick_stack_store")) {

          bool foundModdedQuickStackButton = false;

          // 'foreach in transform' only looks at direct children, so it's pretty performant for this use case
          // we can't save references to them because quickstackstore can destroy and respawn them in various situations
          foreach (Transform item in takeAllButton.transform.parent) {
            switch (item.name) {
              case "quickStackToContainerButton":
                foundModdedQuickStackButton = true;
                item.gameObject.SetActive(showStackButtons && ShowRealGUI());
                break;

              case "storeAllButton":
              case "sortContainerButton":
              case "restockFromContainerButton":
                item.gameObject.SetActive(showStackButtons && ShowRealGUI());
                break;
            }
          }
          
          // only hide when we found the modded quick stack button, in case someone is
          // in quickstackstore 'hotkey only' mode where it does not affect the ui
          if (foundModdedQuickStackButton) {
            // technically, this is based on a config value, but I don't think anyone
            // will ever have both buttons enabled
            stackAllButton.Ref()?.SetActive(false);
          }
        }

        if (ShowRealGUI()) {
          return;
        }

        if (HasContainerAccess(lastHoverContainer)) {
          ShowPreviewContainer(lastHoverContainer.GetInventory());
          delayTime = openDelayTime.Value;
        } else if (delayTime > 0) {
          ShowPreviewContainer(emptyInventory);
          delayTime -= Time.deltaTime;
        } else {
          InventoryGui.instance.m_animator.SetBool("visible", false);
          delayTime = 0;
        }
      }

      private static bool HasContainerAccess(Container container) {
        if (!container) {
          return false;
        }

        bool areaAccess = PrivateArea.CheckAccess(container.transform.position, 0f, false, false);
        bool chestAccess = container.CheckAccess(Game.instance.m_playerProfile.m_playerID);

        return areaAccess && chestAccess;
      }

      [HarmonyPatch(nameof(InventoryGui.SetupDragItem)), HarmonyPrefix]
      public static bool SetupDragItemPrefix() {
        return ShowRealGUI();
      }

      private static void ShowPreviewContainer(Inventory container) {
        InventoryGui.instance.m_animator.SetBool("visible", true);
        InventoryGui.instance.m_hiddenFrames = 10;
        InventoryGui.instance.m_container.gameObject.SetActive(true);

        // wait one frame to let the grid initialize properly
        if (!initializedGrids.Contains(InventoryGui.instance.m_containerGrid)) {
          return;
        }

        InventoryGui.instance.m_containerGrid.UpdateInventory(container, null, null);
        InventoryGui.instance.m_containerGrid.ResetView();
        InventoryGui.instance.m_containerName.text = Localization.instance.Localize(container.GetName());
        int containerWeight = Mathf.CeilToInt(container.GetTotalWeight());
        InventoryGui.instance.m_containerWeight.text = containerWeight.ToString();
      }
    }
  }

  public static class ObjectExtensions {
    public static T Ref<T>(this T o) where T : UnityEngine.Object {
      return o ? o : null;
    }
  }
}
