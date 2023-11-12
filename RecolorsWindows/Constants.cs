namespace RecolorsWindows;

public static class Constants
{
    public static bool PlayerPanelEasterEggs => ModSettings.GetBool("Enable Easter Eggs In Player Panel", "alchlcsystm.recolors.windows");
    public static bool AllEasterEggs => ModSettings.GetBool("All Easter Eggs Are Active", "alchlcsystm.recolors.windows");
    public static bool Debug => ModSettings.GetBool("Enable Debugging", "alchlcsystm.recolors.windows");
    public static int EasterEggChance => ModSettings.GetInt("Easter Egg Icon Chance", "alchlcsystm.recolors.windows");
    public static string CurrentPack => ModSettings.GetString("Selected Icon Pack", "alchlcsystm.recolors.windows");
    public static bool EnableIcons => CurrentPack != "Vanilla";
    public static string TTColor
    {
        get
        {
            if (ModStates.IsEnabled("curtis.colour.swapper"))
                return ModSettings.GetString("Coven", "curtis.colour.swapper");
            else
                return "#B545FF";
        }
    }
    public static string VIPColor
    {
        get
        {
            if (ModStates.IsEnabled("curtis.colour.swapper"))
                return ModSettings.GetString("Town", "curtis.colour.swapper");
            else
                return "#06E00C";
        }
    }
}