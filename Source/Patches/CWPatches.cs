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
        if (!Constants.CustomMainUIEnabled())
            return;

        __instance.transform.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
    }
}

[HarmonyPatch(typeof(PartyLobbyPanel), nameof(PartyLobbyPanel.Start))]
public static class PartyLobbyPanelPatch
{
    public static void Postfix(PartyLobbyPanel __instance)
    {
        if (!Constants.CustomMainUIEnabled())
            return;

        __instance.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(12).GetComponent<Image>().SetImageColor(ColorType.Wood);
    }
}