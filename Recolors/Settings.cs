namespace Recolors;

public static class Settings
{
    public static bool EnableIcons => ModSettings.GetBool("Enable Recolors");
    public static string CurrentPack => ModSettings.GetString("Selected Icon Pack");
}