namespace RecolorsWindows;

public static class Constants
{
    public static bool PlayerPanelEasterEggs => ModSettings.GetBool("Enable Easter Eggs In Player Panel", "alchlcsystm.recolors.windows");
    public static bool ReplaceMissing => ModSettings.GetBool("Mod Assets Replace Missing Icons", "alchlcsystm.recolors.windows");
    public static bool AllEasterEggs => ModSettings.GetBool("All Easter Eggs Are Active", "alchlcsystm.recolors.windows");
    public static bool Debug => ModSettings.GetBool("Enable Debugging", "alchlcsystm.recolors.windows");
    public static int EasterEggChance => ModSettings.GetInt("Easter Egg Icon Chance", "alchlcsystm.recolors.windows");
    public static string CurrentPack => ModSettings.GetString("Selected Icon Pack", "alchlcsystm.recolors.windows");
    public static bool EnableIcons => CurrentPack != "Vanilla";
}