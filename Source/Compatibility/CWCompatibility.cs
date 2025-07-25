namespace FancyUI.Compatibility;

public static class Btos2CwCompatibility
{
    public static bool Init(Type[] btos2Types)
    {
        var casualModeMenuController = btos2Types.FirstOrDefault(x => x.Name == "CasualModeMenuController");
        if (casualModeMenuController == null)
        {
            Fancy.Instance.Message("BTOS2 CW compatibility: CasualModeMenuController not found.");
            return false;
        }

        var fancyCasual = AccessTools.Method(casualModeMenuController, "Start");
        var compat = typeof(Btos2CwCompatibility);

        Btos2Compatibility.Btos2PatchesHarmony.Patch(fancyCasual, null, new(AccessTools.Method(compat, nameof(CasualModePostfix))));
        Fancy.Instance.Message("BTOS2 CW compatibility was successful.");
        return true;
    }

    // Removed RoleDeckPostfix (dead)
    public static void CasualModePostfix(dynamic __instance)
    {
        var globalChatPanel = (Transform)__instance.transform.GetChild(1);
        globalChatPanel.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Wax);

        var queuePanel = (Transform)__instance.transform.GetChild(3);
        queuePanel.GetComponent<Image>().SetImageColor(ColorType.Wood);

        var starspawn = GetSprite("Starspawn", "Starspawn");
        var alert = GetSprite("Veteran_Special", "Town");

        if (starspawn != Blank)
            queuePanel.GetChild(0).GetComponent<Image>().sprite = starspawn;

        if (alert != Blank)
            queuePanel.GetChild(3).GetComponent<Image>().sprite = alert;

        queuePanel.GetChild(1).GetComponent<TextMeshProUGUI>().SetGraphicColor(ColorType.Metal);
        queuePanel.GetChild(2).GetComponent<TextMeshProUGUI>().SetGraphicColor(ColorType.Metal);

        var chatWindow = globalChatPanel.GetChild(0).GetChild(0);
        chatWindow.GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Wood);
        chatWindow.GetChild(2).GetComponent<Image>().SetImageColor(ColorType.Metal);
    }
}
