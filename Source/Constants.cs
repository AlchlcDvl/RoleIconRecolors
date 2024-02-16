namespace IconPacks;

public static class Constants
{
    public static bool PlayerPanelEasterEggs => ModSettings.GetBool("Enable Easter Eggs In Player Panel", "alchlcsystm.recolors");
    public static bool AllEasterEggs => ModSettings.GetBool("All Easter Eggs Are Active", "alchlcsystm.recolors");
    public static int EasterEggChance => ModSettings.GetInt("Easter Egg Icon Chance", "alchlcsystm.recolors");
    public static string CurrentPack => ModSettings.GetString("Selected Icon Pack", "alchlcsystm.recolors");
    public static string CurrentStyle => ModSettings.GetString("Selected Mention Style", "alchlcsystm.recolors");
    public static string FactionOverride => ModSettings.GetString("Override Faction", "alchlcsystm.recolors");
    public static bool FactionOverridden => FactionOverride != "None";
    public static bool EnableIcons => CurrentPack != "Vanilla";
    public static bool BTOS2Exists => ModStates.IsLoaded("curtis.tuba.better.tos2");
    public static bool IsBTOS2
    {
        get
        {
            try
            {
                if (BTOS2Exists)
                    return BTOSInfo.IS_MODDED;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
    }
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
    public static string TTColor
    {
        get
        {
            if (ModStates.IsLoaded("curtis.colour.swapper"))
                return ModSettings.GetString("Coven", "curtis.colour.swapper");
            else
                return "#B545FF";
        }
    }
    public static string VIPColor
    {
        get
        {
            if (ModStates.IsLoaded("curtis.colour.swapper"))
                return ModSettings.GetString("Town", "curtis.colour.swapper");
            else
                return "#06E00C";
        }
    }
}