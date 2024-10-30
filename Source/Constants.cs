namespace FancyUI;

public static class Constants
{
    public static bool PlayerPanelEasterEggs() => ModSettings.GetBool("Enable Easter Eggs In Player Panel", "alchlcsystm.fancy.ui");

    public static bool AllEasterEggs() => ModSettings.GetBool("All Easter Eggs Are Active", "alchlcsystm.fancy.ui");

    public static int EasterEggChance() => ModSettings.GetInt("Easter Egg Icon Chance", "alchlcsystm.fancy.ui");

    public static string CurrentPack() => ModSettings.GetString("Selected Icon Pack", "alchlcsystm.fancy.ui");

    public static string CurrentStyle(ModType? mod = null) => ModSettings.GetString($"Selected {mod ?? Utils.GetGameType()} Mention Style", "alchlcsystm.fancy.ui");

    public static string FactionOverride(ModType? mod = null) => ModSettings.GetString($"Override {mod ?? Utils.GetGameType()} Faction", "alchlcsystm.fancy.ui");

    public static bool CustomNumbers() => ModSettings.GetBool("Use Custom Numbers", "alchlcsystm.fancy.ui");

    public static bool DumpSheets() => ModSettings.GetBool("Dump Sprite Sheets", "alchlcsystm.fancy.ui");

    public static bool PackDebug() => ModSettings.GetBool("Debug Pack Loading", "alchlcsystm.fancy.ui");

    public static string CurrentSet() => ModSettings.GetString("Selected Silhouette Set", "alchlcsystm.fancy.ui");

    public static float AnimationDuration() => ModSettings.GetFloat("Silhouette Animation Duration", "alchlcsystm.fancy.ui");

    public static bool FactionOverridden() => FactionOverride() != "None";

    public static bool EnableIcons() => CurrentPack() != "Vanilla";

    public static bool EnableSwaps() => CurrentSet() != "Vanilla";

    public static string GetMainUIThemeType() => ModSettings.GetString("Main UI Theme", "alchlcsystm.fancy.ui");

    public static Color GetMainUIThemeWoodColor() => ModSettings.GetColor("Main UI Theme Wood Color", "alchlcsystm.fancy.ui");

    public static Color GetMainUIThemeMetalColor() => ModSettings.GetColor("Main UI Theme Metal Color", "alchlcsystm.fancy.ui");

    public static Color GetMainUIThemePaperColor() => ModSettings.GetColor("Main UI Theme Paper Color", "alchlcsystm.fancy.ui");

    public static Color GetMainUIThemeLeatherColor() => ModSettings.GetColor("Main UI Theme Leather Color", "alchlcsystm.fancy.ui");

    public static bool CustomMainUIEnabled() => GetMainUIThemeType() != "Default";

    public static bool BTOS2Exists() => ModStates.IsEnabled("curtis.tuba.better.tos2");

    public static bool MiscRoleExists() => ModStates.IsEnabled("det.rolecustomizationmod");

    public static bool IsBTOS2()
    {
        try
        {
            return BTOS2IPCompatibility.BTOS2IPPatched && IsBTOS2Bypass();
        }
        catch
        {
            return false;
        }
    }

    private static bool IsBTOS2Bypass() => BTOS2Exists() && BetterTOS2.BTOSInfo.IS_MODDED;

    public static bool IsNecroActive()
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

    public static bool IsTransformed()
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

    public static bool IsLocalVIP()
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