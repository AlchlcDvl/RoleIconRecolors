using Game.Achievements;

namespace FancyUI.Patches;

[HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.Start))]
public static class RoleCardPanelPatch
{
    public static void Postfix(RoleCardPanel __instance)
    {
        if (!Constants.EnableCustomUI())
            return;

        __instance.transform.GetChild(5).GetComponent<Image>().SetImageColor(ColorType.Wood);
        __instance.transform.GetChild(10).GetComponent<Image>().SetImageColor(ColorType.Wood);
    }
}

[HarmonyPatch(typeof(SpecialAbilityPanel), nameof(SpecialAbilityPanel.Start))]
public static class SpecialAbilityPanelPatch1
{
    public static void Postfix(SpecialAbilityPanel __instance)
    {
        if (!Constants.EnableCustomUI())
            return;

        var og = __instance.transform.GetChild(1).GetComponent<Image>();
        var copy = UObject.Instantiate(og, og.transform.parent);

        og.sprite = Fancy.Assets.GetSprite("RoleCard_Ability_Main_W");
        copy.sprite = Fancy.Assets.GetSprite("RoleCard_Ability_Main_M");

        og.SetImageColor(ColorType.Wood); // Main wood container
        copy.SetImageColor(ColorType.Metal); // The metal support
        copy.transform.SetSiblingIndex(1);
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupPanel), nameof(SpecialAbilityPopupPanel.Start))]
public static class SpecialAbilityPopupPanelPatch
{
    public static void Postfix(SpecialAbilityPopupPanel __instance)
    {
        if (!Constants.EnableCustomUI())
            return;

        var og = __instance.transform.GetChild(0).GetComponent<Image>();
        var copy = UObject.Instantiate(og, og.transform.parent);

        og.sprite = Fancy.Assets.GetSprite("RoleCard_Ability_Main_W");
        copy.sprite = Fancy.Assets.GetSprite("RoleCard_Ability_Main_M");

        og.SetImageColor(ColorType.Wood); // Main wood container
        copy.SetImageColor(ColorType.Metal); // The metal support
        copy.transform.SetSiblingIndex(0);
    }
}

[HarmonyPatch(typeof(RoleCardPopupPanel), nameof(RoleCardPopupPanel.Start))]
public static class RoleCardPopupControllerPatch
{
    public static void Postfix(RoleCardPopupPanel __instance)
    {
        if (!Constants.EnableCustomUI())
            return;

        __instance.transform.GetChild(4).GetComponent<Image>().SetImageColor(ColorType.Wood); // Frame
        __instance.transform.GetChild(10).GetComponent<Image>().SetImageColor(ColorType.Wood); // Slot count for display
        __instance.transform.GetChild(13).GetComponent<Image>().SetImageColor(ColorType.Wood); // Slot count prefab 1
        __instance.transform.GetChild(14).GetComponent<Image>().SetImageColor(ColorType.Wood); // Slot count prefab 2
        __instance.transform.GetChild(15).GetComponent<Image>().SetImageColor(ColorType.Wood); // Slot count prefab 3
        __instance.transform.GetChild(16).GetComponent<Image>().SetImageColor(ColorType.Wood); // Slot count prefab 4
        __instance.transform.GetChild(17).GetComponent<Image>().SetImageColor(ColorType.Metal); // Closing screw
    }
}

[HarmonyPatch(typeof(AchievementItem), nameof(AchievementItem.SetAchievement))]
public static class AchievementItemPatch
{
    public static void Postfix(AchievementItem __instance)
    {
        if (Constants.EnableCustomUI())
            __instance.transform.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
    }
}

[HarmonyPatch(typeof(HudDockPanel), nameof(HudDockPanel.OnGameInfoChanged))]
public static class HudDockPanelPatch
{
    public static void Postfix(HudDockPanel __instance)
    {
        if (Constants.EnableCustomUI())
            __instance.GetComponent<Image>().SetImageColor(ColorType.Wood);
    }
}

[HarmonyPatch(typeof(PooledChatViewSwitcher), nameof(PooledChatViewSwitcher.Start))]
public static class PooledChatViewSwitcherPatch
{
    public static void Postfix(PooledChatViewSwitcher __instance)
    {
        if (!Constants.EnableCustomUI())
            return;

        __instance.transform.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
        var parts = __instance.transform.GetChild(1).GetChild(1);
        parts.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
        parts.GetChild(1).GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
        parts.GetChild(2).GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
        var parts2 = parts.GetChild(5).GetChild(0);
        parts2.parent.GetComponent<Image>().SetImageColor(ColorType.Metal);
        parts2.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Metal);
        parts2.GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Metal);
        var nameplate = __instance.transform.GetChild(3).GetChild(0); // Nameplate
        nameplate.GetComponent<Image>().SetImageColor(ColorType.Metal);

        // This is me fixing an issue in the weirdest way possible
        var name = nameplate.Find("Name");

        if (name)
            name.SetParent(nameplate.parent);

        var cutout = nameplate.Find("Cutout");

        if (cutout)
            cutout.SetParent(nameplate.parent);
    }
}

[HarmonyPatch(typeof(ChatInputController), nameof(ChatInputController.SetChatState))]
public static class ChatInputControllerPatch
{
    public static void Postfix(ChatInputController __instance)
    {
        if (Constants.EnableCustomUI())
            __instance.parchmentBackgroundImage.SetImageColor(ColorType.Paper);
    }
}

[HarmonyPatch(typeof(LobbyInfoClassicPanel), nameof(LobbyInfoClassicPanel.Start))]
public static class LobbyInfoClassicPanelPatch
{
    public static void Postfix(LobbyInfoClassicPanel __instance)
    {
        if (Constants.EnableCustomUI())
            __instance.GetComponent<Image>().SetImageColor(ColorType.Wood);
    }
}

[HarmonyPatch(typeof(LobbyTimer), nameof(LobbyTimer.HandleOnLobbyDataChanged))]
public static class LobbyTimerPatch
{
    public static void Postfix(LobbyTimer __instance)
    {
        if (Constants.EnableCustomUI())
            __instance.GetComponent<Image>().SetImageColor(ColorType.Wood);
    }
}

[HarmonyPatch(typeof(HudTimeChangePanel), nameof(HudTimeChangePanel.OnDaytimeChanged))]
public static class HudTimeChangePanelPatch
{
    public static void Postfix(HudTimeChangePanel __instance)
    {
        if (!Constants.EnableCustomUI())
            return;

        __instance.transform.GetChild(0).GetChild(3).GetComponent<Image>().SetImageColor(ColorType.Wood);
        __instance.transform.GetChild(1).GetChild(3).GetComponent<Image>().SetImageColor(ColorType.Wood);
    }
}

[HarmonyPatch(typeof(LobbyGameModeChoicePanel), nameof(LobbyGameModeChoicePanel.Start))]
public static class LobbyGameModeChoicePanelPatch
{
    public static void Postfix(LobbyGameModeChoicePanel __instance)
    {
        if (!Constants.EnableCustomUI())
            return;

        __instance.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(12).GetComponent<Image>().SetImageColor(ColorType.Wood);
        __instance.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(2).GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Wood);
        __instance.transform.GetChild(0).GetChild(2).GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Wood);
        __instance.transform.GetChild(0).GetChild(2).GetChild(1).GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Metal);
        __instance.transform.GetChild(0).GetChild(2).GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Metal);
    }
}

// [HarmonyPatch(typeof(SafeAreaController), nameof(SafeAreaController.OnEnable))]
// public static class SafeAreaController_OnEnable
// {
//     public static void Postfix(SafeAreaController __instance)
//     {
//         if (!Constants.EnableCustomUI() || Pepper.IsLobbyOrGamePhase())
//             return;

//         var parts = __instance.transform.GetChild(12);
//         parts.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood); // Wood frame at the back
//         parts.GetChild(13).GetComponent<Image>().SetImageColor(ColorType.Wood); // Wood frame in the front
//     }
// }

// HudRoleListPanel - Roles = 3, Bans = 4

// [HarmonyPatch(typeof(HudGraveyardPanel), nameof(HudGraveyardPanel.Start))]
// public static class HudRoleListPanelPatch
// {
//     public static void Postfix(HudGraveyardPanel __instance)
//     {
//         if (!Constants.EnableCustomUI())
//             return;

//     }
// }