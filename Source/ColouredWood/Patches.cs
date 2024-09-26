namespace FancyUI.ColouredWood;

[HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.Start))]
public static class RoleCardPanelPatch
{
    public static void Postfix(RoleCardPanel __instance)
    {
        if (!Constants.CustomMainUIEnabled())
            return;

        var frame = __instance.transform.GetChild(5).GetComponent<Image>();
        frame.material = AssetManager.Grayscale;
        frame.color = Constants.GetMainUIThemeColor();

        var slots = __instance.transform.GetChild(10).GetComponent<Image>();
        slots.material = AssetManager.Grayscale;
        slots.color = Constants.GetMainUIThemeColor();
    }
}