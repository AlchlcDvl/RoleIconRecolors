namespace FancyUI;

public static class Constants
{
    public static bool PlayerPanelEasterEggs() => Fancy.PlayerPanelEasterEggs.Value;

    public static bool AllEasterEggs() => Fancy.AllEasterEggs.Value;

    public static int EasterEggChance() => (int)Fancy.EasterEggChance.Value;

    public static string CurrentPack() => Fancy.SelectedIconPack.Value;

    public static string CurrentStyle(ModType? mod = null) => (mod ?? Utils.GetGameType()) switch
    {
        ModType.BTOS2 => Fancy.MentionStyle2.Value,
        _ => Fancy.MentionStyle1.Value
    };

    public static string FactionOverride(ModType? mod = null) => (mod ?? Utils.GetGameType()) switch
    {
        ModType.BTOS2 => Fancy.FactionOverride2.Value,
        _ => Fancy.FactionOverride1.Value
    };

    public static bool CustomNumbers() => Fancy.CustomNumbers.Value;

    public static bool DumpSheets() => Fancy.DumpSpriteSheets.Value;

    public static bool PackDebug() => Fancy.DebugPackLoading.Value;

    public static string CurrentSet() => Fancy.SelectedSilhouetteSet.Value;

    public static float AnimationDuration() => Fancy.AnimationDuration.Value;

    public static UITheme GetMainUIThemeType() => Fancy.SelectedUITheme.Value;

    public static Color GetMainUIThemeWoodColor() => Fancy.MainUIThemeWood.Value.ToColor();

    public static Color GetMainUIThemeMetalColor() => Fancy.MainUIThemeMetal.Value.ToColor();

    public static Color GetMainUIThemePaperColor() => Fancy.MainUIThemePaper.Value.ToColor();

    public static Color GetMainUIThemeLeatherColor() => Fancy.MainUIThemeLeather.Value.ToColor();

    public static Color GetMainUIThemeFireColor() => Fancy.MainUIThemeFire.Value.ToColor();

    public static Color GetMainUIThemeWaxColor() => Fancy.MainUIThemeWax.Value.ToColor();

    public static int PlayerNumber() => (int)Fancy.PlayerNumber.Value;

    public static bool FactionOverridden() => FactionOverride() != "None";

    public static bool EnableIcons() => CurrentPack() != "Vanilla";

    public static bool EnableSwaps() => CurrentSet() != "Vanilla";

    public static bool EnableCustomUI() => GetMainUIThemeType() != UITheme.Default;

    public static bool BTOS2Exists() => ModStates.IsEnabled("curtis.tuba.better.tos2");

    public static bool MiscRoleExists() => ModStates.IsEnabled("det.rolecustomizationmod");

    public static bool IsBTOS2()
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