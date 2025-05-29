using Home.Shared;
using NewModLoading;

namespace FancyUI;

public class Fancy : BaseMod<Fancy>
{
    public override string Name => "Fancy UI";
    public override string HarmonyId => "alchlcsystm.fancy.ui";
    public override string[] Bundles => [ "Assets" ];
    public override bool HasFolder => true;

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

        Instance.Message("Fancy!", true);
    }

    public override void UponAssetsHandled()
    {
        Blank = Assets.GetSprite("Blank");
        FancyAssetManager.Attack = Assets.GetSprite("Attack");
        FancyAssetManager.Defense = Assets.GetSprite("Defense");
        Ethereal = Assets.GetSprite("Ethereal");
        MenuButton.FancyMenu.Icon = Assets.GetSprite("Thumbnail");

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

        TryLoadingSprites(SelectedIconPack.Value, PackType.IconPacks);
        LoadVanillaSpriteSheets();

        try
        {
            LoadBtos2SpriteSheet();
        }
        catch { }
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
    public static ToggleOption ReplaceNAwithRA;
    public static ToggleOption GradientBuckets;
    public static ToggleOption DisableBTOSTribunal;

    public static ToggleOption MajorColors;
    public static ToggleOption LethalColors;
    public static ColorOption JuryColor;

    public static ColorOption NeutralStart;
    public static ColorOption NeutralEnd;
    public static ColorOption BucketStart;
    public static ColorOption BucketEnd;
    public static ColorOption KeywordStart;
    public static ColorOption KeywordEnd;
    public static ColorOption AchievementStart;
    public static ColorOption AchievementEnd;

    public static FloatOption GeneralBrightness;
    public static FloatOption GrayscaleAmount;

    public static FloatOption DeadChatDesaturation;

    public static FloatOption PlayerNumber;

    public static StringInputOption RecruitLabelTown;
    public static StringInputOption RecruitLabelCoven;
    public static StringInputOption RecruitLabelApocalypse;
    public static StringInputOption RecruitLabelSerialKiller;
    public static StringInputOption RecruitLabelArsonist;
    public static StringInputOption RecruitLabelWerewolf;
    public static StringInputOption RecruitLabelShroud;
    public static StringInputOption RecruitLabelPandora;
    public static StringInputOption RecruitLabelCompliance;
    public static StringInputOption RecruitLabelCursedSoul;
    public static StringInputOption CovenTraitorLabel;
    public static StringInputOption ApocTraitorLabel;
    public static StringInputOption PandoraTraitorLabel;
    public static StringInputOption VipLabel;
    public static StringInputOption CourtLabel;
    public static StringInputOption JuryLabel;
    public static StringInputOption PirateLabel;

    private static readonly Dictionary<string, ColorOption> ColorOptions = [];
    public static readonly Dictionary<string, (string Start, string End, string Major, string Middle, string Lethal)> Colors = new()
    {
        { "TOWN", ("#06E00C", "#06E00C", "#06E00C", null, "#06E00C") },
        { "COVEN", ("#B545FF", "#B545FF", "#B545FF", null, "#B545FF") },
        { "APOCALYPSE", ("#FF004E", "#FF004E", "#FF004E", null, "#FF004E") },
        { "VAMPIRE", ("#A22929", "#A22929", "#A22929", null, "#A22929") },
        { "CURSEDSOUL", ("#4FFF9F", "#B54FFF", "#4FFF9F", null, "#4FFF9F") },
        { "PANDORA", ("#B545FF", "#FF004E", "#FF004E", null, "#FF004E") },
        { "COMPLIANCE", ("#2D44B5", "#FC9F32", "#FC9F32", "#AE1B1E", null) },
        { "SERIALKILLER", ("#1D4DFC", "#1D4DFC", "#1D4DFC", null, null) },
        { "ARSONIST", ("#DB7601", "#DB7601", "#DB7601", null, null) },
        { "WEREWOLF", ("#9D7038", "#9D7038", "#9D7038", null, null) },
        { "SHROUD", ("#6699FF", "#6699FF", "#6699FF", null, null) },
        { "JACKAL", ("#404040", "#D0D0D0", "#D0D0D0", null, "#D0D0D0") },
        { "EGOTIST", ("#359f3f", "#3f359f", "#3f359f", null, "#3f359f") },
        { "JESTER", ("#F5A6D4", "#F5A6D4", null, null, null) },
        { "EXECUTIONER", ("#949797", "#949797", null, null, null) },
        { "DOOMSAYER", ("#00CC99", "#00CC99", null, null, null) },
        { "PIRATE", ("#ECC23E", "#ECC23E", null, null, null) },
        { "INQUISITOR", ("#821252", "#821252", null, null, null) },
        { "AUDITOR", ("#AEBA87", "#E8FCC5", null, null, null) },
        { "JUDGE", ("#C77364", "#C93D50", null, null, null) },
        { "STARSPAWN", ("#FCE79A", "#999CFF", null, null, null) },
        { "FROGS", ("#1e49cf", "#1e49cf", "#1e49cf", null, "#1e49cf") },
        { "HAWKS", ("#A81538", "#A81538", "#A81538", null, "#A81538") },
        { "LIONS", ("#FFC34F", "#FFC34F", "#FFC34F", null, "#FFC34F") },
        { "LOVERS", ("#FEA6FA", null, null, null, null) },
        //{ "JURY", ("#FCCE3B", null, null, null, null) },
        { "STONED_HIDDEN", ("#9C9A9A", null, null, null, null) }
    };

    public static readonly Dictionary<FactionType, EnumDropdownOption<CinematicType>> CinematicMap = [];

    private static EnumDropdownOption<ColorType> SelectColorFilter;
    public static EnumDropdownOption<FactionType> SelectTestingFaction;

    // TODO: Implement these
    public static EnumDropdownOption<Role> SelectTestingRole;
    public static EnumDropdownOption<EffectType> SelectTestingEffect;

    public static EnumDropdownOption<RecruitEndType> RecruitEndingColor;
    public static EnumDropdownOption<FactionLabelOption> RoleCardFactionLabel;
    public static EnumDropdownOption<FactionLabelOption> FactionNameNextToRole;
    public static EnumDropdownOption<DisplayType> SelectDisplay;

    private static FactionType[] VanillaFactions;
    private static FactionType[] BTOS2Factions;
    private static Role[] VanillaRoles;
    private static Role[] BTOS2Roles;

    public static readonly Dictionary<FactionType, Dictionary<ColorType, ColorOption>> FactionToColorMap = [];
    public static readonly Dictionary<ColorType, ColorOption> CustomUIColorsMap = [];
    public static readonly Dictionary<ColorType, ToggleOption> ColorShadeToggleMap = [];
    public static readonly Dictionary<ColorType, FloatOption> ColorShadeMap = [];

    public override void BindConfigs()
    {
        VanillaFactions = [.. GeneralUtils.GetEnumValues<FactionType>()!.Except([FactionType.UNKNOWN])];
        BTOS2Factions = [.. AccessTools.GetDeclaredFields(typeof(Btos2Faction)).Select(x => (FactionType)x.GetRawConstantValue())];
        VanillaRoles = [.. GeneralUtils.GetEnumValues<Role>()!.Where(role => (byte)role is < 57 or > 250).Except([Role.STONED, Role.UNKNOWN])];
        BTOS2Roles = [.. AccessTools.GetDeclaredFields(typeof(Btos2Role)).Select(x => (Role)x.GetRawConstantValue()).Where(role => (byte)role is < 63 or > 249).Except([Role.STONED,
            Role.UNKNOWN])];

        var colors = GeneralUtils.GetEnumValues<ColorType>()!.Where(x => x != ColorType.All).ToDictionary(x => x, x => x.ToString().ToUpperInvariant());
        var filteredFactions = BTOS2Factions.Where(x => x is not (Btos2Faction.Cannibal or Btos2Faction.None or Btos2Faction.Lovers));
        var filteredRoles = BTOS2Roles.Where(x => (int)x is < 57 or > 249 && x is not (Role.STONED or Role.UNKNOWN)).ToList();

        var factions = filteredFactions.ToDictionary(x => x, x => Utils.FactionName(x, Constants.BTOS2Exists() ? GameModType.BTOS2 : GameModType.Vanilla, false).ToUpperInvariant());

        BTOS2Factions.Do(x => FactionToColorMap[x] = []);

        SelectDisplay = new("SELECT_DISPLAY", DisplayType.RoleCard, PackType.None, useTranslations: true);
        SelectTestingFaction = new("SELECTED_TESTING_FACTION", FactionType.NONE, PackType.None, useTranslations: true, values:
            () => SettingsAndTestingUI.Instance?.IsBTOS2 == true ? [.. filteredFactions.AddItem(FactionType.NONE)] : VanillaFactions);
        SelectTestingRole = new("SELECTED_TESTING_ROLE", Role.ADMIRER, PackType.None, useTranslations: true, values:
            () => SettingsAndTestingUI.Instance?.IsBTOS2 == true ? BTOS2Roles : VanillaRoles); //  AS it has to be like this unless you wanna make it work with filteredRoles (which it currently does not)

        SelectedIconPack = new("SELECTED_ICON_PACK", "Vanilla", PackType.IconPacks, () => GetPackNames(PackType.IconPacks), onChanged: x => TryLoadingSprites(x, PackType.IconPacks));
        SelectedSilhouetteSet = new("SELECTED_SIL_SET", "Vanilla", PackType.SilhouetteSets, () => GetPackNames(PackType.SilhouetteSets), onChanged: x => TryLoadingSprites(x,
            PackType.SilhouetteSets));

        SelectedUITheme = new("SELECTED_UI_THEME", UITheme.Vanilla, PackType.RecoloredUI, useTranslations: true);

        MentionStyle1 = new("MENTION_STYLE_1", "Regular", PackType.IconPacks, () => GetOptions(GameModType.Vanilla, true), Constants.EnableIcons);
        MentionStyle2 = new("MENTION_STYLE_2", "Regular", PackType.IconPacks, () => GetOptions(GameModType.BTOS2, true), () => Constants.BTOS2Exists() && Constants.EnableIcons());

        FactionOverride1 = new("FACTION_OVERRIDE_1", "None", PackType.IconPacks, () => GetOptions(GameModType.Vanilla, false), Constants.EnableIcons);
        FactionOverride2 = new("FACTION_OVERRIDE_2", "None", PackType.IconPacks, () => GetOptions(GameModType.BTOS2, false), () => Constants.BTOS2Exists() && Constants.EnableIcons());

        SelectColorFilter = new("COLOR_FILTER", ColorType.Wood, PackType.RecoloredUI, setActive: Constants.EnableCustomUI, useTranslations: true);

        foreach (var (type, name) in colors)
        {
            CustomUIColorsMap[type] = new($"UI_{name}", "#FFFFFF", PackType.RecoloredUI, () => Constants.EnableCustomUI() && SelectColorFilter.Value.IsAny(type, ColorType.All));
            ColorShadeToggleMap[type] = new($"COLOR_{name}", true, PackType.RecoloredUI, () => SelectedUITheme.Value == UITheme.Faction && SelectColorFilter.Value.IsAny(type, ColorType.All));
            ColorShadeMap[type] = new($"{name}_SHADE", 0, PackType.RecoloredUI, -100, 100, true, () => Constants.EnableCustomUI() && SelectColorFilter.Value.IsAny(type, ColorType.All));

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

                FactionToColorMap[faction][type] = new($"{factionName}_UI_{name}", color, PackType.RecoloredUI, () => Constants.EnableCustomUI() && Constants.ShowFactionalSettings() &&
                    SelectTestingFaction.Value == faction && SelectColorFilter.Value.IsAny(type, ColorType.All));
            }
        }

        GeneralBrightness = new("GENERAL_BRIGHTNESS", 100, PackType.RecoloredUI, 0, 100, true, Constants.EnableCustomUI);
        GrayscaleAmount = new("GRAYSCALE_AMOUNT", 100, PackType.RecoloredUI, 0, 100, true, Constants.EnableCustomUI);

        EasterEggChance = new("EE_CHANCE", 5, PackType.IconPacks, 0, 100, true, Constants.EnableIcons);
        AnimationDuration = new("ANIM_DURATION", 2, PackType.SilhouetteSets, 0.5f, 10, setActive: Constants.EnableSwaps);

        CustomNumbers = new("CUSTOM_NUMBERS", false, PackType.IconPacks, Constants.EnableIcons);
        AllEasterEggs = new("ALL_EE", false, PackType.IconPacks, Constants.EnableIcons);
        PlayerPanelEasterEggs = new("PLAYER_PANEL_EE", false, PackType.IconPacks, Constants.EnableIcons);

        FactionalRoleNames = new("FACTIONAL_ROLE_NAMES", false, PackType.MiscRoleCustomisation, Constants.TextEditorExists);
        MajorColors = new("MAJOR_COLORS", false, PackType.MiscRoleCustomisation);
        LethalColors = new("LETHAL_COLORS", false, PackType.MiscRoleCustomisation);
        RecruitEndingColor = new("RECRUIT_ENDING", RecruitEndType.JackalEnd, PackType.MiscRoleCustomisation, useTranslations: true, setActive: Constants.BTOS2Exists);
        RoleCardFactionLabel = new("FACTION_LABEL", FactionLabelOption.Mismatch, PackType.MiscRoleCustomisation, useTranslations: true);
        FactionNameNextToRole = new("FACTION_NEXT_TO_ROLE", FactionLabelOption.Never, PackType.MiscRoleCustomisation, useTranslations: true);
        RecruitLabelTown = new("RECRUIT_LABEL_TOWN", "Recruited", PackType.MiscRoleCustomisation, setActive: () => Constants.BTOS2Exists() && SelectTestingFaction.Value == FactionType.TOWN);
        RecruitLabelCoven = new("RECRUIT_LABEL_COVEN", "Recruited", PackType.MiscRoleCustomisation, setActive: () => Constants.BTOS2Exists() && SelectTestingFaction.Value == FactionType.COVEN);
        RecruitLabelApocalypse = new("RECRUIT_LABEL_APOCALYPSE", "Recruited", PackType.MiscRoleCustomisation, setActive: () => Constants.BTOS2Exists() && SelectTestingFaction.Value == FactionType.APOCALYPSE);
        RecruitLabelSerialKiller = new("RECRUIT_LABEL_SERIAL_KILLER", "Recruited", PackType.MiscRoleCustomisation, setActive: () => Constants.BTOS2Exists() && SelectTestingFaction.Value == FactionType.SERIALKILLER);
        RecruitLabelArsonist = new("RECRUIT_LABEL_ARSONIST", "Recruited", PackType.MiscRoleCustomisation, setActive: () => Constants.BTOS2Exists() && SelectTestingFaction.Value == FactionType.ARSONIST);
        RecruitLabelWerewolf = new("RECRUIT_LABEL_WEREWOLF", "Recruited", PackType.MiscRoleCustomisation, setActive: () => Constants.BTOS2Exists() && SelectTestingFaction.Value == FactionType.WEREWOLF);
        RecruitLabelShroud = new("RECRUIT_LABEL_SHROUD", "Recruited", PackType.MiscRoleCustomisation, setActive: () => Constants.BTOS2Exists() && SelectTestingFaction.Value == FactionType.SHROUD);
        RecruitLabelPandora = new("RECRUIT_LABEL_PANDORA", "Recruited", PackType.MiscRoleCustomisation, setActive: () => Constants.BTOS2Exists() && SelectTestingFaction.Value == Btos2Faction.Pandora);
        RecruitLabelCompliance = new("RECRUIT_LABEL_COMPLIANCE", "Recruited", PackType.MiscRoleCustomisation, setActive: () => Constants.BTOS2Exists() && SelectTestingFaction.Value == Btos2Faction.Compliance);
        RecruitLabelCursedSoul = new("RECRUIT_LABEL_CURSEDSOUL", "Recruited", PackType.MiscRoleCustomisation, setActive: () => Constants.BTOS2Exists() && SelectTestingFaction.Value == FactionType.CURSED_SOUL);

        CovenTraitorLabel = new("COVEN_TRAITOR_LABEL", "Town Traitor", PackType.MiscRoleCustomisation);
        ApocTraitorLabel = new("APOC_TRAITOR_LABEL", "Town Traitor", PackType.MiscRoleCustomisation, setActive: Constants.BTOS2Exists);
        PandoraTraitorLabel = new("PANDORA_TRAITOR_LABEL", "Town Traitor", PackType.MiscRoleCustomisation, setActive: Constants.BTOS2Exists);
        VipLabel = new("VIP_LABEL", "VIP", PackType.MiscRoleCustomisation);
        CourtLabel = new("COURT_LABEL", "Court", PackType.MiscRoleCustomisation, setActive: Constants.BTOS2Exists);
        JuryLabel = new("JURY_LABEL", "Jury", PackType.MiscRoleCustomisation, setActive: Constants.BTOS2Exists);
        JuryColor = new("JURY_COLOR", "#FCCE3B", PackType.MiscRoleCustomisation, setActive: Constants.BTOS2Exists);
        PirateLabel = new("PIRATE_LABEL", "Pirate", PackType.MiscRoleCustomisation, setActive: Constants.BTOS2Exists);
        GradientBuckets = new("GRADIENT_BUCKETS", true, PackType.MiscRoleCustomisation);
        ReplaceNAwithRA = new("RANDOM_APOC_IN_VANILLA", false, PackType.MiscRoleCustomisation);
        DeadChatDesaturation = new("DEAD_CHAT_DESATURATION", 50, PackType.MiscRoleCustomisation, -1, 100, true);
        NeutralStart = new("NEUTRAL_START", "#A9A9A9", PackType.MiscRoleCustomisation);
        NeutralEnd = new("NEUTRAL_END", "#A9A9A9", PackType.MiscRoleCustomisation, setActive: () => GradientBuckets.Value);
        BucketStart = new("BUCKET_START", "#1F51FF", PackType.MiscRoleCustomisation, setActive: () => GradientBuckets.Value);
        BucketEnd = new("BUCKET_END", "#1F51FF", PackType.MiscRoleCustomisation, setActive: () => GradientBuckets.Value);
        KeywordStart = new("KEYWORD_START", "#007AFF", PackType.MiscRoleCustomisation);
        KeywordEnd = new("KEYWORD_END", "#007AFF", PackType.MiscRoleCustomisation);
        AchievementStart = new("ACHIEVEMENT_START", "#FFBE00", PackType.MiscRoleCustomisation);
        AchievementEnd = new("ACHIEVEMENT_END", "#FFBE00", PackType.MiscRoleCustomisation);

        PlayerNumber = new("PLAYER_NUMBER", 0, PackType.Testing, 0, 15, true, Constants.CustomNumbers);
        DumpSpriteSheets = new("DUMP_SHEETS", false, PackType.Testing);
        DebugPackLoading = new("DEBUG_LOADING", false, PackType.Testing);
        ShowOverlayWhenJailed = new("SHOW_TO_JAILED", true, PackType.Testing);
        ShowOverlayAsJailor = new("SHOW_TO_JAILOR", false, PackType.Testing);
        IconsInRoleReveal = new("ROLE_REVEAL_ICONS", true, PackType.Testing);
        DisableBTOSTribunal = new("DISABLE_BTOS_TRIBUNAL", true, PackType.Testing, setActive: Constants.BTOS2Exists);

        foreach (var faction in BTOS2Factions.Where(x => x is not (FactionType.NONE or (> FactionType.APOCALYPSE and < FactionType.VAMPIRE) or FactionType.CURSED_SOUL or FactionType.UNKNOWN or
            (> Btos2Faction.Hawks and < Btos2Faction.Pandora))))
        {
            CinematicMap[faction] = new(
                $"{Utils.FactionName(faction, GameModType.BTOS2, false).ToUpper()}_CINEMATIC",
                GetCinematic(faction),
                PackType.CinematicSwapper,
                setActive: () => (faction < FactionType.UNKNOWN || Constants.BTOS2Exists()) && SelectTestingFaction.Value == faction,
                useTranslations: true,
                values: AllowedCinematics,
                uponChanged: ReloadCinematics);
        }

        foreach (var (key, (start, end, major, middle, lethal)) in Colors)
        {
            if (start == null)
                continue;

            if (middle == null && major == null && end == null)
            {
                ColorOptions[$"{key}"] = new($"{key}", start, PackType.MiscRoleCustomisation, SetActive, uponChanged: ReloadColors);
                continue;
            }

            ColorOptions[$"{key}_START"] = new($"{key}_START", start, PackType.MiscRoleCustomisation, SetActive, uponChanged: ReloadColors);

            if (middle != null)
                ColorOptions[$"{key}_MIDDLE"] = new($"{key}_MIDDLE", middle, PackType.MiscRoleCustomisation, SetActive, uponChanged: ReloadColors);

            if (end != null)
                ColorOptions[$"{key}_END"] = new($"{key}_END", end, PackType.MiscRoleCustomisation, SetActive, uponChanged: ReloadColors);

            if (major != null)
                ColorOptions[$"{key}_MAJOR"] = new($"{key}_MAJOR", major, PackType.MiscRoleCustomisation, SetActive, uponChanged: ReloadColors);

            if (lethal != null)
                ColorOptions[$"{key}_LETHAL"] = new($"{key}_LETHAL", lethal, PackType.MiscRoleCustomisation, SetActive, uponChanged: ReloadColors);

            bool SetActive() => Utils.FactionName(SelectTestingFaction.Value, true, stoned: true).ToUpper() == key;
        }

        ReloadColors();
    }

    private static void ReloadColors()
    {
        foreach (var key in Colors.Keys.ToList())
        {
            var start  = ColorOptions.TryGetValue($"{key}_START", out var s) ? s.Value : (ColorOptions.TryGetValue($"{key}", out var s2) ? s2.Value : null);
            var end    = ColorOptions.TryGetValue($"{key}_END", out var e) ? e.Value : null;
            var major  = ColorOptions.TryGetValue($"{key}_MAJOR", out var m) ? m.Value : null;
            var middle = ColorOptions.TryGetValue($"{key}_MIDDLE", out var mi) ? mi.Value : null;
            var lethal = ColorOptions.TryGetValue($"{key}_LETHAL", out var l) ? l.Value : null;

            Colors[key] = (start, end, major, middle, lethal);
        }
    }

    private static void ReloadCinematics()
    {
        foreach (var faction in BTOS2Factions.Where(x => x is not (FactionType.NONE or
                (> FactionType.APOCALYPSE and < FactionType.VAMPIRE) or FactionType.CURSED_SOUL or
                FactionType.UNKNOWN or (> Btos2Faction.Hawks and < Btos2Faction.Pandora))))
        {
            if (CinematicMap.ContainsKey(faction)) continue;

            CinematicMap[faction] = new(
                $"{Utils.FactionName(faction, GameModType.BTOS2, false).ToUpper()}_CINEMATIC",
                GetCinematic(faction),
                PackType.CinematicSwapper,
                setActive: () => (faction < FactionType.UNKNOWN || Constants.BTOS2Exists()) && SelectTestingFaction.Value == faction,
                useTranslations: true,
                values: AllowedCinematics);
        }
    }

    public static CinematicType GetCinematic(FactionType faction) => faction switch
    {
        FactionType.TOWN => CinematicType.TownWins,
        FactionType.COVEN => CinematicType.CovenWins,
        FactionType.SERIALKILLER => CinematicType.SerialKillersWin,
        FactionType.ARSONIST => CinematicType.ArsonistsWins,
        FactionType.WEREWOLF => CinematicType.WerewolvesWin,
        FactionType.SHROUD => CinematicType.ShroudsWin,
        FactionType.APOCALYPSE => CinematicType.ApocolypseWins,
        FactionType.VAMPIRE => CinematicType.VampireWins,
        _ => CinematicType.FactionWins,
    };

    private static CinematicType[] AllowedCinematics() =>
    [
        CinematicType.FactionWins,
        CinematicType.TownWins,
        CinematicType.CovenWins,
        CinematicType.SerialKillersWin,
        CinematicType.ArsonistsWins,
        CinematicType.WerewolvesWin,
        CinematicType.ShroudsWin,
        CinematicType.ApocolypseWins,
        CinematicType.VampireWins,
    ];

    private static string[] GetPackNames(PackType type) =>
    [
        "Vanilla",
        .. from dir in Directory.EnumerateDirectories(Path.Combine(Instance.ModPath, type.ToString()))
            where !dir.ContainsAny("Vanilla", "BTOS2")
            select dir.FancySanitisePath()
    ];

    private static string[] GetOptions(GameModType mod, bool mentionStyle)
    {
        try
        {
            var result = new List<string>();

            if (IconPacks.TryGetValue(SelectedIconPack.Value, out var pack))
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