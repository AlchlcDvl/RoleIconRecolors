namespace FancyUI;

public static class Constants
{
    public static readonly Dictionary<bool, Dictionary<ColorType, Material>> AllMaterials = [];

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

    public static FactionType GetSelectedFaction() => Fancy.SelectTestingFaction.Value;

    public static bool ShowFactionalSettings() => GetMainUIThemeType() == UITheme.Faction;

    public static Color GetUIThemeColor(ColorType type, FactionType? faction = null) => GetMainUIThemeType() switch
    {
        UITheme.Faction => GetThemeColor(type, faction),
        UITheme.Custom => Fancy.CustomUIColorsMap[type].Value.ToColor(),
        _ => Color.clear
    };

    private static Color GetThemeColor(ColorType color, FactionType? faction = null)
    {
        if (!Fancy.ColorShadeToggleMap[color].Value)
            return Fancy.CustomUIColorsMap.TryGetValue(color, out var custom) ? custom.Value.ToColor() : Color.clear;

        if (!faction.HasValue)
        {
            if (SettingsAndTestingUI.Instance)
                faction = GetSelectedFaction();
            else if (Pepper.IsNonePhase() || Pepper.IsLobbyOrPickNamesPhase())
                faction = FactionType.NONE;
            else if (Leo.IsGameScene())
                faction = Pepper.GetMyFaction();
        }

        var shouldUseCustom = faction is null or FactionType.NONE;
        var colorString = "";

        if (!shouldUseCustom)
        {
            if (Fancy.FactionToColorMap.TryGetValue(faction.Value, out var dict) && dict.TryGetValue(color, out var opt))
                colorString = opt.Value;
            else
                shouldUseCustom = true;
        }

        if (shouldUseCustom)
            colorString = Fancy.CustomUIColorsMap.TryGetValue(color, out var opt) ? opt.Value : "#000000";

        return colorString.ToColor();
    }

    public static float GeneralBrightness() => Fancy.GeneralBrightness.Value * 5f / 100f;

    public static float GrayscaleAmount() => Fancy.GrayscaleAmount.Value / 100f;

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