namespace IconPacks;

public static class Constants
{
    public static bool PlayerPanelEasterEggs => ModSettings.GetBool("Enable Easter Eggs In Player Panel", "alchlcsystm.recolors");
    public static bool AllEasterEggs => ModSettings.GetBool("All Easter Eggs Are Active", "alchlcsystm.recolors");
    public static int EasterEggChance => ModSettings.GetInt("Easter Egg Icon Chance", "alchlcsystm.recolors");
    public static string CurrentPack => ModSettings.GetString("Selected Icon Pack", "alchlcsystm.recolors");
    public static string CurrentStyle => ModSettings.GetString($"Selected {(IsBTOS2 ? "BTOS2" : "Vanilla")} Mention Style", "alchlcsystm.recolors");
    public static string FactionOverride => ModSettings.GetString($"Override {(IsBTOS2 ? "BTOS2" : "Vanilla")} Faction", "alchlcsystm.recolors");
    public static bool CustomNumbers => ModSettings.GetBool("Use Custom Numbers", "alchlcsystm.recolors");
    public static bool FactionOverridden => FactionOverride != "None";
    public static bool EnableIcons => CurrentPack != "Vanilla";
    public static bool BTOS2Exists => ModStates.IsLoaded("curtis.tuba.better.tos2");
    public static bool LegacyExists => ModStates.IsLoaded("legacy.salem");
    public static bool IsBTOS2
    {
        get
        {
            try
            {
                return BTOS2Exists && BetterTOS2.BTOSInfo.IS_MODDED;
            }
            catch
            {
                return false;
            }
        }
    }
    public static bool IsLegacy
    {
        get
        {
            try
            {
                return LegacyExists && LegacyClient.LegacyInfo.IsModded;
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
}