namespace RecolorsPlatformless;

public static class Constants
{
    public static bool PlayerPanelEasterEggs => ModSettings.GetBool("Enable Easter Eggs In Player Panel", "alchlcsystm.recolors");
    public static bool AllEasterEggs => ModSettings.GetBool("All Easter Eggs Are Active", "alchlcsystm.recolors");
    public static bool Debug => ModSettings.GetBool("Enable Debugging", "alchlcsystm.recolors");
    public static int EasterEggChance => ModSettings.GetInt("Easter Egg Icon Chance", "alchlcsystm.recolors");
    public static string CurrentPack => ModSettings.GetString("Selected Icon Pack", "alchlcsystm.recolors");
    public static bool EnableIcons => CurrentPack != "Vanilla";
    public static bool IsNecroActive
    {
        get
        {
            try
            {
                return Service.Game?.Sim?.info?.roleCardObservation?.Data?.powerUp == POWER_UP_TYPE.NECRONOMICON;
            }
            catch
            {
                return false;
            }
        }
    }
    public static bool IsTransformed
    {
        get
        {
            try
            {
                return Service.Game?.Sim?.info?.roleCardObservation?.Data?.powerUp == POWER_UP_TYPE.HORSEMAN;
            }
            catch
            {
                return false;
            }
        }
    }
    public static bool IsLocalVIP
    {
        get
        {
            try
            {
                return Service.Game?.Sim?.info?.roleCardObservation?.Data?.modifier == ROLE_MODIFIER.VIP;
            }
            catch
            {
                return false;
            }
        }
    }
    public static bool IsLocalTT
    {
        get
        {
            try
            {
                return Service.Game?.Sim?.info?.roleCardObservation?.Data?.modifier == ROLE_MODIFIER.TRAITOR;
            }
            catch
            {
                return false;
            }
        }
    }
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