namespace RecolorsWindows;

public static class Constants
{
    public static bool PlayerPanelEasterEggs => ModSettings.GetBool("Enable Easter Eggs In Player Panel", "alchlcdvl.recolors.windows");
    public static bool ReplaceMissing => ModSettings.GetBool("Mod Assets Replace Missing Icons", "alchlcdvl.recolors.windows");
    public static bool AllEasterEggs => ModSettings.GetBool("All Easter Eggs Are Active", "alchlcdvl.recolors.windows");
    public static int EasterEggChance => ModSettings.GetInt("Easter Egg Icon Chance", "alchlcdvl.recolors.windows");
    public static string CurrentPack => ModSettings.GetString("Selected Icon Pack", "alchlcdvl.recolors.windows");
    public static bool EnableIcons => CurrentPack != "Vanilla";
}