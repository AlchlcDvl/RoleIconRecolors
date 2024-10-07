using Game.Achievements;

namespace FancyUI.Patches;

[HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.Start))]
public static class RoleCardPanelPatch
{
    public static void Postfix(RoleCardPanel __instance)
    {
        if (!Constants.CustomMainUIEnabled())
            return;

        __instance.transform.GetChild(5).GetComponent<Image>().SetImageColor(ColorType.Wood);
        __instance.transform.GetChild(10).GetComponent<Image>().SetImageColor(ColorType.Wood);
    }
}

[HarmonyPatch(typeof(RoleCardPopupPanel), nameof(RoleCardPopupPanel.Start))]
public static class RoleCardPopupControllerPatch
{
    public static void Postfix(RoleCardPopupController __instance)
    {
        if (!Constants.CustomMainUIEnabled())
            return;

        __instance.transform.GetChild(4).GetComponent<Image>().SetImageColor(ColorType.Wood);
        __instance.transform.GetChild(10).GetComponent<Image>().SetImageColor(ColorType.Wood);
        __instance.transform.GetChild(13).GetComponent<Image>().SetImageColor(ColorType.Wood);
    }
}

[HarmonyPatch(typeof(AchievementItem), nameof(AchievementItem.SetAchievement))]
public static class AchievementItemPatch
{
    public static void Postfix(AchievementItem __instance)
    {
        if (Constants.CustomMainUIEnabled())
            __instance.transform.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
    }
}

[HarmonyPatch(typeof(PartyLobbyPanel), nameof(PartyLobbyPanel.Start))]
public static class PartyLobbyPanelPatch
{
    public static void Postfix(PartyLobbyPanel __instance)
    {
        if (Constants.CustomMainUIEnabled())
            __instance.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(12).GetComponent<Image>().SetImageColor(ColorType.Wood);
    }
}

[HarmonyPatch(typeof(HudDockPanel), nameof(HudDockPanel.OnGameInfoChanged))]
public static class HudDockPanelPatch
{
    public static void Postfix(HudDockPanel __instance)
    {
        if (Constants.CustomMainUIEnabled())
            __instance.GetComponent<Image>().SetImageColor(ColorType.Wood);
    }
}

[HarmonyPatch(typeof(PooledChatViewSwitcher), nameof(PooledChatViewSwitcher.Start))]
public static class PooledChatViewSwitcherPatch
{
    public static void Postfix(PooledChatViewSwitcher __instance)
    {
        if (!Constants.CustomMainUIEnabled())
            return;

        __instance.transform.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
        __instance.transform.GetChild(3).GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Metal);
        var parts = __instance.transform.GetChild(1).GetChild(1);
        parts.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
        parts.GetChild(1).GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
        parts.GetChild(2).GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
        parts.GetChild(5).GetComponent<Image>().SetImageColor(ColorType.Metal);
        
    }
}

[HarmonyPatch(typeof(ChatInputController), nameof(ChatInputController.SetChatState))]
public static class ChatInputControllerPatch
{
    public static void Postfix(ChatInputController __instance)
    {
        if (Constants.CustomMainUIEnabled())
            __instance.parchmentBackgroundImage.SetImageColor(ColorType.Paper);
    }
}

[HarmonyPatch(typeof(LobbyInfoClassicPanel), nameof(LobbyInfoClassicPanel.Start))]
public static class LobbyInfoClassicPanelPatch
{
    public static void Postfix(LobbyInfoClassicPanel __instance)
    {
        if (Constants.CustomMainUIEnabled())
            __instance.GetComponent<Image>().SetImageColor(ColorType.Wood);
    }
}