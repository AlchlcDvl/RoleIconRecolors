using Home.Shared;

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

    public static bool ShowOverlayWhenJailed() => Fancy.ShowOverlayWhenJailed.Value;

    public static bool ShowOverlayAsJailor() => Fancy.ShowOverlayAsJailor.Value;
    
    public static bool IconsInRoleReveal() => Fancy.IconsInRoleReveal.Value;

    public static bool PackDebug() => Fancy.DebugPackLoading.Value;

    private static string CurrentSet() => Fancy.SelectedSilhouetteSet.Value;

    public static float AnimationDuration() => Fancy.AnimationDuration.Value;

    public static UITheme GetMainUIThemeType() => Fancy.SelectedUITheme.Value;

   public static Color GetMainUIThemeWoodColor() => GetMainUIThemeType() switch
    {
        UITheme.Faction when Pepper.GetMyFaction() != FactionType.NONE
            => Pepper.GetMyFaction().GetFactionColor().ToColor().ShadeColor((float)Fancy.WoodShade.Value / 100),
        _ => Fancy.MainUIThemeWood.Value.ToColor(),
    };

    public static Color GetMainUIThemeMetalColor() => GetMainUIThemeType() switch
    {
        UITheme.Faction when Pepper.GetMyFaction() != FactionType.NONE
            => Pepper.GetMyFaction().GetFactionColor().ToColor().ShadeColor((float)Fancy.MetalShade.Value / 100),
        _ => Fancy.MainUIThemeMetal.Value.ToColor(),
    };

    public static Color GetMainUIThemePaperColor() => GetMainUIThemeType() switch
    {
        UITheme.Faction when Pepper.GetMyFaction() != FactionType.NONE
            => Pepper.GetMyFaction().GetFactionColor().ToColor().ShadeColor((float)Fancy.PaperShade.Value / 100),
        _ => Fancy.MainUIThemePaper.Value.ToColor(),
    };

    public static Color GetMainUIThemeLeatherColor() => GetMainUIThemeType() switch
    {
        UITheme.Faction when Pepper.GetMyFaction() != FactionType.NONE
            => Pepper.GetMyFaction().GetFactionColor().ToColor().ShadeColor((float)Fancy.LeatherShade.Value / 100),
        _ => Fancy.MainUIThemeLeather.Value.ToColor(),
    };

    public static Color GetMainUIThemeFireColor() => GetMainUIThemeType() switch
    {
        UITheme.Faction when Pepper.GetMyFaction() != FactionType.NONE
            => Pepper.GetMyFaction().GetFactionColor().ToColor().ShadeColor((float)Fancy.FireShade.Value / 100),
        _ => Fancy.MainUIThemeFire.Value.ToColor(),
    };

    public static Color GetMainUIThemeWaxColor() => GetMainUIThemeType() switch
    {
        UITheme.Faction when Pepper.GetMyFaction() != FactionType.NONE
            => Pepper.GetMyFaction().GetFactionColor().ToColor().ShadeColor((float)Fancy.WaxShade.Value / 100),
        _ => Fancy.MainUIThemeWax.Value.ToColor(),
    };

    public static int PlayerNumber() => (int)Fancy.PlayerNumber.Value;

    public static bool FactionOverridden() => FactionOverride() != "None";

    public static bool EnableIcons() => CurrentPack() != "Vanilla";

    public static bool EnableSwaps() => CurrentSet() != "Vanilla";

    public static bool EnableCustomUI() => GetMainUIThemeType() != UITheme.Vanilla;

    public static bool BTOS2Exists() => ModStates.IsEnabled("curtis.tuba.better.tos2");

    public static bool MiscRoleExists() => ModStates.IsEnabled("det.rolecustomizationmod");

    public static bool IsBTOS2()
    {
        try
        {
            return Btos2Compatibility.Btos2Patched && IsBTOS2Bypass();
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

    public static bool IsLocalVip()
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