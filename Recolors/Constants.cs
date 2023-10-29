namespace Recolors;

public static class Constants
{
    public static bool EnableIcons => ModSettings.GetBool("Enable Recolors", "alchlcdvl.recolors");
    public static int EasterEggChance => ModSettings.GetInt("Easter Egg Icon Chance", "alchlcdvl.recolors");
    //public static string CurrentPack => ModSettings.GetString("Selected Icon Pack", "alchlcdvl.recolors");
}