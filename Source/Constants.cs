namespace IconPacks;

public static class Constants
{
    public static bool PlayerPanelEasterEggs => ModSettings.GetBool("Enable Easter Eggs In Player Panel", "alchlcsystm.recolors");
    public static bool AllEasterEggs => ModSettings.GetBool("All Easter Eggs Are Active", "alchlcsystm.recolors");
    public static int EasterEggChance => ModSettings.GetInt("Easter Egg Icon Chance", "alchlcsystm.recolors");
    public static string CurrentPack => ModSettings.GetString("Selected Icon Pack", "alchlcsystm.recolors");
    public static string CurrentStyle => ModSettings.GetString($"Selected {Utils.GetGameType()} Mention Style", "alchlcsystm.recolors");
    public static string FactionOverride => ModSettings.GetString($"Override {Utils.GetGameType()} Faction", "alchlcsystm.recolors");
    public static bool CustomNumbers => ModSettings.GetBool("Use Custom Numbers", "alchlcsystm.recolors");
    public static bool FactionOverridden => FactionOverride != "None";
    public static bool EnableIcons => CurrentPack != "Vanilla";
    public static bool BTOS2Exists => ModStates.IsEnabled("curtis.tuba.better.tos2");
    //public static bool LegacyExists => ModStates.IsEnabled("legacy.salem");
    public static bool IsBTOS2
    {
        get
        {
            try
            {
                return IsBTOS2Bypass();
            }
            catch
            {
                return false;
            }
        }
    }
    private static bool IsBTOS2Bypass() => BTOS2Exists && BetterTOS2.BTOSInfo.IS_MODDED;
    /*public static bool IsLegacy
    {
        get
        {
            try
            {
                return IsLegacyBypass();
            }
            catch
            {
                return false;
            }
        }
    }
    private static bool IsLegacyBypass() => LegacyExists && LegacyClient.LegacyInfo.IsModded;*/
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