using Home.Shared;

namespace FancyUI;

[SalemMod]
public class Fancy : BaseMod<Fancy>
{
    public override string Name => "Fancy UI";
    public override string[] Bundles => [ "Assets" ];
    public override bool HasFolder => true;

    public static FieldInfo InputRegex;

    public override void Start()
    {
        Instance.Message("Fancying...", true);

        if (!Directory.Exists(IPPath))
            Directory.CreateDirectory(IPPath);

        var json = Path.Combine(IPPath, "OtherPacks.json");

        if (!File.Exists(json))
            File.WriteAllText(json, "");

        var vanilla = Path.Combine(IPPath, "Vanilla");

        if (!Directory.Exists(vanilla))
            Directory.CreateDirectory(vanilla);

        if (!Directory.Exists(SSPath))
            Directory.CreateDirectory(SSPath);

        json = Path.Combine(SSPath, "OtherSets.json");

        if (!File.Exists(json))
            File.WriteAllText(json, "");

        vanilla = Path.Combine(SSPath, "Vanilla");

        if (!Directory.Exists(vanilla))
            Directory.CreateDirectory(vanilla);

        try
        {
            LoadBtos();
        }
        catch { }

        InputRegex = AccessTools.Field(typeof(TMP_InputField), "m_RegexValue");

        Instance.Message("Fancy!", true);
    }

    public override void UponAssetsLoaded()
    {
        Blank = Assets.GetSprite("Blank");
        FancyAssetManager.Attack = Assets.GetSprite("Attack");
        FancyAssetManager.Defense = Assets.GetSprite("Defense");
        Ethereal = Assets.GetSprite("Ethereal");

        Grayscale = Assets.GetMaterial("GrayscaleMaterial");

        var normalMats = Constants.AllMaterials[true] = [];
        var guideMats = Constants.AllMaterials[false] = [];

        foreach (var type in GeneralUtils.GetEnumValues<ColorType>()!.Where(x => x != ColorType.All))
        {
            normalMats[type] = new(Grayscale);
            guideMats[type] = new(Grayscale);
        }

        Utils.UpdateMaterials();

        LoadingGif = Assets.GetGif("Placeholder")!;
        LoadingGif.RenderAllFrames();
        Loading = new("Loading") { Frames = [.. LoadingGif.Frames.Select(x => x.RenderedSprite)] };

        Flame = Assets.GetGif("Flame")!;
        Flame.RenderAllFrames();

        TryLoadingSprites(Constants.CurrentPack(), PackType.IconPacks);
        LoadVanillaSpriteSheets();

        try
        {
            LoadBtos2SpriteSheet();
        } catch {}

        MenuButton.FancyMenu.Icon = Assets.GetSprite("Thumbnail");
    }

    public static StringDropdownOption SelectedIconPack;
    public static StringDropdownOption SelectedSilhouetteSet;

    public static EnumDropdownOption<UITheme> SelectedUITheme;

    public static StringDropdownOption MentionStyle1;
    public static StringDropdownOption MentionStyle2;

    public static StringDropdownOption FactionOverride1;
    public static StringDropdownOption FactionOverride2;

    public static FloatOption EasterEggChance;
    public static FloatOption AnimationDuration;

    public static ToggleOption CustomNumbers;
    public static ToggleOption AllEasterEggs;
    public static ToggleOption PlayerPanelEasterEggs;
    public static ToggleOption DumpSpriteSheets;
    public static ToggleOption DebugPackLoading;
    public static ToggleOption ShowOverlayWhenJailed;
    public static ToggleOption ShowOverlayAsJailor;
    public static ToggleOption IconsInRoleReveal;
    public static ToggleOption FactionalRoleNames;
    public static ToggleOption FactionNameNextToRole; // Could not figure out why the code for this setting did not work

    public static ToggleOption MajorColors;

    public static FloatOption GeneralBrightness;
    public static FloatOption GrayscaleAmount;

    public static FloatOption PlayerNumber;

    public static StringInputOption RecruitLabel;
    public static StringInputOption TraitorLabel;
    public static StringInputOption VIPLabel;
    public static StringInputOption CourtLabel;
    public static StringInputOption JuryLabel;
    public static StringInputOption PirateLabel;

    public static Dictionary<string, ColorOption> ColorOptions = new();
    public static Dictionary<string, (string start, string end, string major, string middle)> Colors;


    public static EnumDropdownOption<FactionType> SelectTestingFaction;
    private static EnumDropdownOption<ColorType> SelectColorFilter;
    public static EnumDropdownOption<Role> SelectTestingRole; // TODO: Implement this
    public static EnumDropdownOption<RecruitEndType> RecruitEndingColor;
    public static EnumDropdownOption<FactionLabelOption> RoleCardFactionLabel;
    public static EnumDropdownOption<DisplayType> SelectDisplay;
    public static EnumDropdownOption<CinematicType> TownCinematic;
    public static EnumDropdownOption<CinematicType> CovenCinematic;
    public static EnumDropdownOption<CinematicType> SerialKillerCinematic;
    public static EnumDropdownOption<CinematicType> ArsonistCinematic;
    public static EnumDropdownOption<CinematicType> WerewolfCinematic;
    public static EnumDropdownOption<CinematicType> ShroudCinematic;
    public static EnumDropdownOption<CinematicType> ApocalypseCinematic;
    public static EnumDropdownOption<CinematicType> VampireCinematic;
    public static EnumDropdownOption<CinematicType> JackalCinematic;
    public static EnumDropdownOption<CinematicType> FrogsCinematic;
    public static EnumDropdownOption<CinematicType> LionsCinematic;
    public static EnumDropdownOption<CinematicType> HawksCinematic;
    public static EnumDropdownOption<CinematicType> PandoraCinematic;
    public static EnumDropdownOption<CinematicType> ComplianceCinematic;
    public static EnumDropdownOption<CinematicType> LoversCinematic;

    private static FactionType[] VanillaFactions;
    private static FactionType[] BTOS2Factions;

    public static readonly Dictionary<FactionType, Dictionary<ColorType, ColorOption>> FactionToColorMap = [];
    public static readonly Dictionary<ColorType, ColorOption> CustomUIColorsMap = [];
    public static readonly Dictionary<ColorType, ToggleOption> ColorShadeToggleMap = [];
    public static readonly Dictionary<ColorType, FloatOption> ColorShadeMap = [];

    public override void LoadConfigs()
    {
        
        VanillaFactions = [.. GeneralUtils.GetEnumValues<FactionType>()!.Except([FactionType.UNKNOWN])];
        BTOS2Factions = [.. AccessTools.GetDeclaredFields(typeof(Btos2Faction)).Select(x => (FactionType)x.GetRawConstantValue())];

        BTOS2Factions.ForEach(x => FactionToColorMap[x] = []);

        SelectedIconPack = new("SELECTED_ICON_PACK", "Vanilla", PackType.IconPacks, () => GetPackNames(PackType.IconPacks), onChanged: x => TryLoadingSprites(x, PackType.IconPacks));
        SelectedSilhouetteSet = new("SELECTED_SIL_SET", "Vanilla", PackType.SilhouetteSets, () => GetPackNames(PackType.SilhouetteSets), onChanged: x => TryLoadingSprites(x,
            PackType.SilhouetteSets));

        SelectedUITheme = new("SELECTED_UI_THEME", UITheme.Vanilla, PackType.RecoloredUI, useTranslations: true);

        MentionStyle1 = new("MENTION_STYLE_1", "Regular", PackType.IconPacks, () => GetOptions(GameModType.Vanilla, true), _ => Constants.EnableIcons());
        MentionStyle2 = new("MENTION_STYLE_2", "Regular", PackType.IconPacks, () => GetOptions(GameModType.BTOS2, true), _ => Constants.BTOS2Exists() && Constants.EnableIcons());

        FactionOverride1 = new("FACTION_OVERRIDE_1", "None", PackType.IconPacks, () => GetOptions(GameModType.Vanilla, false), _ => Constants.EnableIcons());
        FactionOverride2 = new("FACTION_OVERRIDE_2", "None", PackType.IconPacks, () => GetOptions(GameModType.BTOS2, false), _ => Constants.BTOS2Exists() && Constants.EnableIcons());

        SelectColorFilter = new("COLOR_FILTER", ColorType.Wood, PackType.RecoloredUI, setActive: _ => Constants.EnableCustomUI(), useTranslations: true);


        var colors = GeneralUtils.GetEnumValues<ColorType>()!.Where(x => x != ColorType.All).ToDictionary(x => x, x => x.ToString().ToUpperInvariant());
        var factions = BTOS2Factions.Where(x => x is not (Btos2Faction.Lovers or Btos2Faction.Cannibal or Btos2Faction.None)).ToDictionary(x => x, x => Utils.FactionName(x,
            Constants.BTOS2Exists() ? GameModType.BTOS2 : GameModType.Vanilla).ToUpperInvariant());

        foreach (var (type, name) in colors)
        {
            CustomUIColorsMap[type] = new($"UI_{name}", "#FFFFFF", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && SelectColorFilter.Value.IsAny(type, ColorType.All));
            ColorShadeToggleMap[type] = new($"COLOR_{name}", true, PackType.RecoloredUI, _ => Constants.GetMainUIThemeType() == UITheme.Faction && SelectColorFilter.Value.IsAny(type,
                ColorType.All));
            ColorShadeMap[type] = new($"{name}_SHADE", 0, PackType.RecoloredUI, -100, 100, true, _ => Constants.EnableCustomUI() && SelectColorFilter.Value.IsAny(type, ColorType.All));

            foreach (var (faction, factionName) in factions)
            {
                string color;

                if (type == ColorType.Metal)
                {
                    color = faction switch
                    {
                        Btos2Faction.Town => "#737373",
                        Btos2Faction.Apocalypse => "#6E472C",
                        Btos2Faction.Coven => "#CFDEE6",
                        Btos2Faction.CursedSoul when Constants.BTOS2Exists() => "#52C2EF",
                        Btos2Faction.CursedSoul when !Constants.BTOS2Exists() => "#8500BF",
                        Btos2Faction.Jackal => "#D9BF41",
                        Btos2Faction.Doomsayer => "#CCCCCC",
                        Btos2Faction.Shroud => "#70FAF1",
                        Btos2Faction.Compliance => "#AE1B1E",
                        Btos2Faction.Pandora => "#B545FF",
                        Btos2Faction.Judge => "#C77364",
                        Btos2Faction.Auditor => "#AEBA87",
                        Btos2Faction.Starspawn => "#FCE79A",
                        _ => faction.GetFactionColor()
                    };
                }
                else switch (faction)
                {
                    case < Btos2Faction.CursedSoul or (> Btos2Faction.Jackal and < Btos2Faction.Judge) or Btos2Faction.Inquisitor:
                    case Btos2Faction.CursedSoul when !Constants.BTOS2Exists():
                    {
                        color = faction.GetFactionColor();
                        break;
                    }
                    case Btos2Faction.Compliance or Btos2Faction.Pandora:
                    case Btos2Faction.CursedSoul when Constants.BTOS2Exists():
                    {
                        color = type switch
                        {
                            ColorType.Fire => faction switch
                            {
                                Btos2Faction.Compliance => "#FC9F32",
                                Btos2Faction.CursedSoul => "#B54FFF",
                                _ => "#DA23A7",
                            },
                            ColorType.Wood or ColorType.Wax => faction switch
                            {
                                Btos2Faction.Compliance => "#2D44B5",
                                Btos2Faction.CursedSoul => "#4FFF9F",
                                _ => "#FF004E",
                            },
                            _ => faction switch
                            {
                                Btos2Faction.Compliance => "#AE1B1E",
                                Btos2Faction.CursedSoul => "#7500AF",
                                _ => "#B545FF",
                            }
                        };
                        break;
                    }
                    default:
                    {
                        color = type switch
                        {
                            ColorType.Wood or ColorType.Wax or ColorType.Fire => faction switch
                            {
                                Btos2Faction.Jackal => "#D0D0D0",
                                Btos2Faction.Judge => "#C93D50",
                                Btos2Faction.Auditor => "#E8FCC5",
                                Btos2Faction.Starspawn => "#999CFF",
                                _ => "#3F359F",
                            },
                            _ => faction switch
                            {
                                Btos2Faction.Jackal => "#404040",
                                Btos2Faction.Judge => "#C77364",
                                Btos2Faction.Auditor => "#AEBA87",
                                Btos2Faction.Starspawn => "#FCE79A",
                                _ => "#359F3F",
                            },
                        };
                        break;
                    }
                }

                FactionToColorMap[faction][type] = new($"{factionName}_UI_{name}", color, PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalSettings() &&
                    SelectTestingFaction.Value == faction && SelectColorFilter.Value.IsAny(type, ColorType.All));
            }
        }

        GeneralBrightness = new("GENERAL_BRIGHTNESS", 100, PackType.RecoloredUI, 0, 100, true, _ => Constants.EnableCustomUI());
        GrayscaleAmount = new("GRAYSCALE_AMOUNT", 100, PackType.RecoloredUI, 0, 100, true, _ => Constants.EnableCustomUI());

        EasterEggChance = new("EE_CHANCE", 5, PackType.IconPacks, 0, 100, true, _ => Constants.EnableIcons());
        AnimationDuration = new("ANIM_DURATION", 2, PackType.SilhouetteSets, 0.5f, 10, setActive: _ => Constants.EnableSwaps());

        CustomNumbers = new("CUSTOM_NUMBERS", false, PackType.IconPacks, _ => Constants.EnableIcons());
        AllEasterEggs = new("ALL_EE", false, PackType.IconPacks, _ => Constants.EnableIcons());
        PlayerPanelEasterEggs = new("PLAYER_PANEL_EE", false, PackType.IconPacks, _ => Constants.EnableIcons());

        FactionalRoleNames = new("FACTIONAL_ROLE_NAMES", false, PackType.MiscRoleCustomisation);
        MajorColors = new("MAJOR_COLORS", false, PackType.MiscRoleCustomisation);
        RecruitEndingColor = new("RECRUIT_ENDING", RecruitEndType.JackalEnd, PackType.MiscRoleCustomisation, useTranslations: true);
        RoleCardFactionLabel = new("FACTION_LABEL", FactionLabelOption.Mismatch, PackType.MiscRoleCustomisation, useTranslations: true);
        RecruitLabel = new("RECRUIT_LABEL", "Recruited", PackType.MiscRoleCustomisation);
        TraitorLabel = new("TRAITOR_LABEL", "Town Traitor", PackType.MiscRoleCustomisation);
        VIPLabel = new("VIP_LABEL", "VIP", PackType.MiscRoleCustomisation);
        CourtLabel = new("COURT_LABEL", "Court", PackType.MiscRoleCustomisation);
        JuryLabel = new("JURY_LABEL", "Jury", PackType.MiscRoleCustomisation);
        PirateLabel = new("PIRATE_LABEL", "Pirate", PackType.MiscRoleCustomisation);


        PlayerNumber = new("PLAYER_NUMBER", 0, PackType.Testing, 0, 15, true, _ => Constants.CustomNumbers());
        DumpSpriteSheets = new("DUMP_SHEETS", false, PackType.Testing);
        DebugPackLoading = new("DEBUG_LOADING", false, PackType.Testing);
        ShowOverlayWhenJailed = new("SHOW_TO_JAILED", true, PackType.Testing);
        ShowOverlayAsJailor = new("SHOW_TO_JAILOR", false, PackType.Testing);
        IconsInRoleReveal = new("ROLE_REVEAL_ICONS", true, PackType.Testing);
        SelectDisplay = new("SELECT_DISPLAY", DisplayType.RoleCard, PackType.Testing, useTranslations: true);
        SelectTestingFaction = new("SELECTED_TESTING_FACTION", FactionType.NONE, PackType.Testing, useTranslations: true, values:
            () => SettingsAndTestingUI.Instance?.IsBTOS2 == true ? BTOS2Factions : VanillaFactions);
        
        TownCinematic = new EnumDropdownOption<CinematicType>(
            "TOWN_CINEMATIC", CinematicType.TownWins, PackType.Testing,
            useTranslations: true,
            values: () => AllowedCinematics());

        CovenCinematic = new EnumDropdownOption<CinematicType>(
            "COVEN_CINEMATIC", CinematicType.CovenWins, PackType.Testing,
            useTranslations: true,
            values: () => AllowedCinematics());

        SerialKillerCinematic = new EnumDropdownOption<CinematicType>(
            "SERIALKILLER_CINEMATIC", CinematicType.SerialKillersWin, PackType.Testing,
            useTranslations: true,
            values: () => AllowedCinematics());

        ArsonistCinematic = new EnumDropdownOption<CinematicType>(
            "ARSONIST_CINEMATIC", CinematicType.ArsonistsWins, PackType.Testing,
            useTranslations: true,
            values: () => AllowedCinematics());

        WerewolfCinematic = new EnumDropdownOption<CinematicType>(
            "WEREWOLF_CINEMATIC", CinematicType.WerewolvesWin, PackType.Testing,
            useTranslations: true,
            values: () => AllowedCinematics());

        ShroudCinematic = new EnumDropdownOption<CinematicType>(
            "SHROUD_CINEMATIC", CinematicType.ShroudsWin, PackType.Testing,
            useTranslations: true,
            values: () => AllowedCinematics());

        ApocalypseCinematic = new EnumDropdownOption<CinematicType>(
            "APOCALYPSE_CINEMATIC", CinematicType.ApocolypseWins, PackType.Testing,
            useTranslations: true,
            values: () => AllowedCinematics());

        VampireCinematic = new EnumDropdownOption<CinematicType>(
            "VAMPIRE_CINEMATIC", CinematicType.VampireWins, PackType.Testing,
            useTranslations: true,
            values: () => AllowedCinematics());

        JackalCinematic = new EnumDropdownOption<CinematicType>(
            "JACKAL_CINEMATIC", CinematicType.FactionWins, PackType.Testing,
            useTranslations: true,
            values: () => AllowedCinematics());

        FrogsCinematic = new EnumDropdownOption<CinematicType>(
            "FROGS_CINEMATIC", CinematicType.FactionWins, PackType.Testing,
            useTranslations: true,
            values: () => AllowedCinematics());

        LionsCinematic = new EnumDropdownOption<CinematicType>(
            "LIONS_CINEMATIC", CinematicType.FactionWins, PackType.Testing,
            useTranslations: true,
            values: () => AllowedCinematics());

        HawksCinematic = new EnumDropdownOption<CinematicType>(
            "HAWKS_CINEMATIC", CinematicType.FactionWins, PackType.Testing,
            useTranslations: true,
            values: () => AllowedCinematics());

        PandoraCinematic = new EnumDropdownOption<CinematicType>(
            "PANDORA_CINEMATIC", CinematicType.FactionWins, PackType.Testing,
            useTranslations: true,
            values: () => AllowedCinematics());

        ComplianceCinematic = new EnumDropdownOption<CinematicType>(
            "COMPLIANCE_CINEMATIC", CinematicType.FactionWins, PackType.Testing,
            useTranslations: true,
            values: () => AllowedCinematics());

        LoversCinematic = new EnumDropdownOption<CinematicType>(
            "LOVERS_CINEMATIC", CinematicType.FactionWins, PackType.Testing,
            useTranslations: true,
            values: () => AllowedCinematics());

        InitializeColors();
    }

    private static CinematicType[] AllowedCinematics()
    {
        return new CinematicType[]
        {
            CinematicType.FactionWins,
            CinematicType.TownWins,
            CinematicType.CovenWins,
            CinematicType.SerialKillersWin,
            CinematicType.ArsonistsWins,
            CinematicType.WerewolvesWin,
            CinematicType.ShroudsWin,
            CinematicType.ApocolypseWins,
            CinematicType.VampireWins,
        };
    }

    public static void InitializeColors()
    {
        Colors = new Dictionary<string, (string start, string end, string major, string middle)>
        {
            { "TOWN", ("#06E00C", "#06E00C", "#06E00C", null) },
            { "COVEN", ("#B545FF", "#B545FF", "#B545FF", null) },
            { "APOCALYPSE", ("#FF004E", "#FF004E", "#FF004E", null) },
            { "VAMPIRE", ("#A22929", "#A22929", "#A22929", null) },
            { "CURSEDSOUL", ("#4FFF9F", "#B54FFF", "#4FFF9F", null) },
            { "PANDORA", ("#B545FF", "#FF004E", "#FF004E", null) },
            { "COMPLIANCE", ("#2D44B5", "#FC9F32", "#FC9F32", "#AE1B1E") },
            { "SERIALKILLER", ("#1D4DFC", "#1D4DFC", "#1D4DFC", null) },
            { "ARSONIST", ("#DB7601", "#DB7601", "#DB7601", null) },
            { "WEREWOLF", ("#9D7038", "#9D7038", "#9D7038", null) },
            { "SHROUD", ("#6699FF", "#6699FF", "#6699FF", null) },
            { "JACKAL", ("#404040", "#D0D0D0", "#D0D0D0", null) },
            { "FROGS", ("#1e49cf", "#1e49cf", "#1e49cf", null) },
            { "EGOTIST", ("#359f3f", "#3f359f", "#3f359f", null) },
            { "JESTER", ("#F5A6D4", "#F5A6D4", null, null) },
            { "EXECUTIONER", ("#949797", "#949797", null, null) },
            { "DOOMSAYER", ("#00CC99", "#00CC99", null, null) },
            { "PIRATE", ("#ECC23E", "#ECC23E", null, null) },
            { "INQUISITOR", ("#821252", "#821252", null, null) },
            { "AUDITOR", ("#AEBA87", "#E8FCC5", null, null) },
            { "JUDGE", ("#C77364", "#C93D50", null, null) },
            { "STARSPAWN", ("#FCE79A", "#999CFF", null, null) },
            { "HAWKS", ("#7E4C9C", "#7E4C9C", "#7E4C9C", null) },
            { "LIONS", ("#D19000", "#D19000", "#D19000", null) },
            { "NEUTRAL", ("#A9A9A9", null, null, null) },
            { "STONED_HIDDEN", ("#9C9A9A", null, null, null) },
            { "LOVERS", ("#FEA6FA", null, null, null) },
        };

        foreach (var kvp in Colors)
        {
            string key = kvp.Key;
            var (start, end, major, middle) = kvp.Value;

            if (start != null)
            {
                if (middle == null && major == null && end == null)
                {
                    ColorOptions[$"{key}"] = new($"{key}", start, PackType.MiscRoleCustomisation);
                }
                else
                {
                    ColorOptions[$"{key}_START"] = new($"{key}_START", start, PackType.MiscRoleCustomisation, onChanged: _ => ReloadColors());
                    if (middle != null)
                        ColorOptions[$"{key}_MIDDLE"] = new($"{key}_MIDDLE", middle, PackType.MiscRoleCustomisation, onChanged: _ => ReloadColors());
                    if (end != null)
                        ColorOptions[$"{key}_END"] = new($"{key}_END", end, PackType.MiscRoleCustomisation, onChanged: _ => ReloadColors());
                    if (major != null)
                        ColorOptions[$"{key}_MAJOR"] = new($"{key}_MAJOR", major, PackType.MiscRoleCustomisation, onChanged: _ => ReloadColors());

                }
            }
        }

        foreach (var key in Colors.Keys.ToList())
        {
            string start = ColorOptions.TryGetValue($"{key}_START", out var s) ? s.Value : ColorOptions.TryGetValue($"{key}", out var s2) ? s2.Value : null;
            string end = ColorOptions.TryGetValue($"{key}_END", out var e) ? e.Value : null;
            string major = ColorOptions.TryGetValue($"{key}_MAJOR", out var m) ? m.Value : null;
            string middle = ColorOptions.TryGetValue($"{key}_MIDDLE", out var mi) ? mi.Value : null;

            Colors[key] = (start, end, major, middle);
        }


    }
    public static void ReloadColors()
    {
        foreach (var key in Colors.Keys.ToList())
        {
            string start  = ColorOptions.TryGetValue($"{key}_START", out var s) ? s.Value
                        : ColorOptions.TryGetValue($"{key}", out var s2) ? s2.Value : null;
            string end    = ColorOptions.TryGetValue($"{key}_END", out var e) ? e.Value : null;
            string major  = ColorOptions.TryGetValue($"{key}_MAJOR", out var m) ? m.Value : null;
            string middle = ColorOptions.TryGetValue($"{key}_MIDDLE", out var mi) ? mi.Value : null;

            Colors[key] = (start, end, major, middle);
        }
    }

    private static string[] GetPackNames(PackType type)
    {
        var result = new List<string>() { "Vanilla" };

        foreach (var dir in Directory.EnumerateDirectories(Path.Combine(Instance.ModPath, type.ToString())))
        {
            if (!dir.ContainsAny("Vanilla", "BTOS2"))
                result.Add(dir.FancySanitisePath());
        }

        return [.. result];
    }

    private static string[] GetOptions(GameModType mod, bool mentionStyle)
    {
        try
        {
            var result = new List<string>();

            if (IconPacks.TryGetValue(Constants.CurrentPack(), out var pack))
            {
                result.Add(mentionStyle ? "Regular" : "None");

                if (pack.Assets.TryGetValue(GameModType.Common, out var assets))
                {
                    foreach (var (folder, icons) in assets.BaseIcons)
                    {
                        if (icons.Count > 0 && !result.Contains(folder) && folder != "Custom")
                            result.Add(folder);
                    }
                }

                if (pack.Assets.TryGetValue(mod, out assets))
                {
                    foreach (var (folder, icons) in assets.BaseIcons)
                    {
                        if (icons.Count > 0 && !result.Contains(folder) && folder != "Custom")
                            result.Add(folder);
                    }
                }

                result.Add("Custom");
            }
            else
                result.Add("None");

            if (mentionStyle)
                result.Add(mod.ToString());
            else
                result.Remove("Regular");

            return [.. result];
        }
        catch
        {
            return [mentionStyle ? mod.ToString() : "None"];
        }
    }
}

[SalemMenuItem]
public static class MenuButton
{
    public static readonly SalemMenuButton FancyMenu = new()
    {
        Label = "Fancy UI",
        OnClick = OpenMenu
    };

    private static void OpenMenu()
    {
        var go = UObject.Instantiate(Fancy.Instance.Assets.GetGameObject("FancyUI"), CacheHomeSceneController.Controller.SafeArea.transform, false);
        go.transform.localPosition = new(0, 0, 0);
        go.transform.localScale = Vector3.one * 2f;
        go.AddComponent<UI.FancyUI>();
    }
}