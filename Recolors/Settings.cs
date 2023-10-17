namespace Recolors;

public static class Settings
{
    public static bool EnableIcons => ModSettings.GetBool("Enable Recolors", "alchlcdvl.recolors");
    public static string CurrentPack => ModSettings.GetString("Selected Icon Pack", "alchlcdvl.recolors");
}