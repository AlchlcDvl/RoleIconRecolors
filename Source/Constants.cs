namespace FancyUI;

public static class Constants
{
    public static bool PlayerPanelEasterEggs() => Fancy.PlayerPanelEasterEggs.Get();

    public static bool AllEasterEggs() => Fancy.AllEasterEggs.Get();

    public static int EasterEggChance() => (int)Fancy.EasterEggChance.Get();

    public static string CurrentPack() => Fancy.SelectedIconPack.Get();

    public static string CurrentStyle(ModType? mod = null) => (mod ?? Utils.GetGameType()) switch
    {
        ModType.BTOS2 => Fancy.MentionStyle2.Get(),
        _ => Fancy.MentionStyle1.Get()
    };

    public static string FactionOverride(ModType? mod = null) => (mod ?? Utils.GetGameType()) switch
    {
        ModType.BTOS2 => Fancy.FactionOverride2.Get(),
        _ => Fancy.FactionOverride1.Get()
    };

    public static bool CustomNumbers() => Fancy.CustomNumbers.Get();

    public static bool DumpSheets() => Fancy.DumpSpriteSheets.Get();

    public static bool PackDebug() => Fancy.DebugPackLoading.Get();

    public static string CurrentSet() => Fancy.SelectedSilhouetteSet.Get();

    public static float AnimationDuration() => Fancy.AnimationDuration.Get();

    public static UITheme GetMainUIThemeType() => Fancy.SelectedUITheme.Get();

    public static Color GetMainUIThemeWoodColor() => Fancy.MainUIThemeWood.Get().ToColor();

    public static Color GetMainUIThemeMetalColor() => Fancy.MainUIThemeMetal.Get().ToColor();

    public static Color GetMainUIThemePaperColor() => Fancy.MainUIThemePaper.Get().ToColor();

    public static Color GetMainUIThemeLeatherColor() => Fancy.MainUIThemeLeather.Get().ToColor();

    public static Color GetMainUIThemeFireColor() => Fancy.MainUIThemeFire.Get().ToColor();

    public static Color GetMainUIThemeWaxColor() => Fancy.MainUIThemeWax.Get().ToColor();

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