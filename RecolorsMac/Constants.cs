namespace RecolorsMac;

public static class Constants
{
    public static bool EnableIcons => ModSettings.GetBool("Enable Custom Icons", "alchlcdvl.recolors.mac");
    public static int EasterEggChance => ModSettings.GetInt("Easter Egg Icon Chance", "alchlcdvl.recolors.mac");
    //public static string CurrentPack => ModSettings.GetString("Selected Icon Pack", "alchlcdvl.recolors.mac");
}