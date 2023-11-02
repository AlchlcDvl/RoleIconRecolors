namespace RecolorsWindows;

public static class Constants
{
    public static bool EnableIcons => ModSettings.GetBool("Enable Custom Icons", "alchlcdvl.recolors.windows");
    public static bool PlayerPanelEasterEggs => ModSettings.GetBool("Enable Easter Eggs In Player Panel", "alchlcdvl.recolors.windows");
    public static int EasterEggChance => ModSettings.GetInt("Easter Egg Icon Chance", "alchlcdvl.recolors.windows");
    //public static string CurrentPack => ModSettings.GetString("Selected Icon Pack", "alchlcdvl.recolors.windows");
}