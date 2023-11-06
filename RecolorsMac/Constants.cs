namespace RecolorsMac;

public static class Constants
{
    public static bool PlayerPanelEasterEggs => ModSettings.GetBool("Enable Easter Eggs In Player Panel", "alchlcdvl.recolors.mac");
    public static bool ReplaceMissing => ModSettings.GetBool("Mod Assets Replace Missing Icons", "alchlcdvl.recolors.mac");
    public static bool AllEasterEggs => ModSettings.GetBool("All Easter Eggs Are Active", "alchlcdvl.recolors.mac");
    public static int EasterEggChance => ModSettings.GetInt("Easter Egg Icon Chance", "alchlcdvl.recolors.mac");
    public static string CurrentPack => ModSettings.GetString("Selected Icon Pack", "alchlcdvl.recolors.mac");
    public static bool EnableIcons => CurrentPack != "Vanilla";
}