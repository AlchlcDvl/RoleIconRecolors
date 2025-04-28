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
    public static ColorOption TownStart;
    public static ColorOption TownEnd;
    public static ColorOption CovenStart;
    public static ColorOption CovenEnd;
    public static ColorOption ApocalypseStart;
    public static ColorOption ApocalypseEnd;
    public static ColorOption VampireStart;
    public static ColorOption VampireEnd;
    public static ColorOption CursedSoulStart;
    public static ColorOption CursedSoulEnd;
    public static ColorOption PandoraStart;
    public static ColorOption PandoraEnd;
    public static ColorOption ComplianceStart;
    public static ColorOption ComplianceMiddle;
    public static ColorOption ComplianceEnd;
    public static ColorOption SerialKillerStart;
    public static ColorOption SerialKillerEnd;
    public static ColorOption ArsonistStart;
    public static ColorOption ArsonistEnd;
    public static ColorOption WerewolfStart;
    public static ColorOption WerewolfEnd;
    public static ColorOption ShroudStart;
    public static ColorOption ShroudEnd;
    public static ColorOption JackalStart;
    public static ColorOption JackalEnd;
    public static ColorOption FrogsStart;
    public static ColorOption FrogsEnd;
    public static ColorOption HawksStart;
    public static ColorOption HawksEnd;
    public static ColorOption LionsStart;
    public static ColorOption LionsEnd;
    public static ColorOption EgotistStart;
    public static ColorOption EgotistEnd;
    public static ColorOption JesterStart;
    public static ColorOption JesterEnd;
    public static ColorOption ExecutionerStart;
    public static ColorOption ExecutionerEnd;
    public static ColorOption DoomsayerStart;
    public static ColorOption DoomsayerEnd;
    public static ColorOption PirateStart;
    public static ColorOption PirateEnd;
    public static ColorOption InquisitorStart;
    public static ColorOption InquisitorEnd;
    public static ColorOption StarspawnStart;
    public static ColorOption StarspawnEnd;
    public static ColorOption JudgeStart;
    public static ColorOption JudgeEnd;
    public static ColorOption AuditorStart;
    public static ColorOption AuditorEnd;
    public static ColorOption Neutral;
    public static ColorOption Lovers;
    public static ColorOption StonedHidden;
    public static ColorOption TownMajor;
    public static ColorOption CovenMajor;
    public static ColorOption ApocalypseMajor;
    public static ColorOption VampireMajor;
    public static ColorOption CursedSoulMajor;
    public static ColorOption PandoraMajor;
    public static ColorOption ComplianceMajor;
    public static ColorOption SerialKillerMajor;
    public static ColorOption ArsonistMajor;
    public static ColorOption WerewolfMajor;
    public static ColorOption ShroudMajor;
    public static ColorOption JackalMajor;
    public static ColorOption FrogsMajor;
    public static ColorOption HawksMajor;
    public static ColorOption LionsMajor;
    public static ColorOption EgotistMajor;



    public static EnumDropdownOption<FactionType> SelectTestingFaction;
    private static EnumDropdownOption<ColorType> SelectColorFilter;
    public static EnumDropdownOption<Role> SelectTestingRole; // TODO: Implement this
    public static EnumDropdownOption<RecruitEndType> RecruitEndingColor;
    public static EnumDropdownOption<FactionLabelOption> RoleCardFactionLabel;
    public static EnumDropdownOption<DisplayType> SelectDisplay;

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
        TownStart = new("TOWN_START", "#06E00C", PackType.MiscRoleCustomisation);
        TownEnd = new("TOWN_END", "#06E00C", PackType.MiscRoleCustomisation);
        TownMajor = new("TOWN_MAJOR", "#06E00C", PackType.MiscRoleCustomisation);
        CovenStart = new("COVEN_START", "#B545FF", PackType.MiscRoleCustomisation);
        CovenEnd = new("COVEN_END", "#B545FF", PackType.MiscRoleCustomisation);
        CovenMajor = new("COVEN_MAJOR", "#B545FF", PackType.MiscRoleCustomisation);
        ApocalypseStart = new("APOCALYPSE_START", "#FF004E", PackType.MiscRoleCustomisation);
        ApocalypseEnd = new("APOCALYPSE_END", "#FF004E", PackType.MiscRoleCustomisation);
        ApocalypseMajor = new("APOCALYPSE_MAJOR", "#FF004E", PackType.MiscRoleCustomisation);
        JesterStart = new("JESTER_START", "#F5A6D4", PackType.MiscRoleCustomisation);
        JesterEnd = new("JESTER_END", "#F5A6D4", PackType.MiscRoleCustomisation);
        DoomsayerStart = new("DOOMSAYER_START", "#00CC99", PackType.MiscRoleCustomisation);
        DoomsayerEnd = new("DOOMSAYER_END", "#00CC99", PackType.MiscRoleCustomisation);
        PirateStart = new("PIRATE_START", "#ECC23E", PackType.MiscRoleCustomisation);
        PirateEnd = new("PIRATE_END", "#ECC23E", PackType.MiscRoleCustomisation);
        ExecutionerStart = new("EXECUTIONER_START", "#949797", PackType.MiscRoleCustomisation);
        ExecutionerEnd = new("EXECUTIONER_END", "#949797", PackType.MiscRoleCustomisation);
        InquisitorStart = new("INQUISITOR_START", "#821252", PackType.MiscRoleCustomisation);
        InquisitorEnd = new("INQUISITOR_END", "#821252", PackType.MiscRoleCustomisation);
        ArsonistStart = new("ARSONIST_START", "#DB7601", PackType.MiscRoleCustomisation);
        ArsonistEnd = new("ARSONIST_END", "#DB7601", PackType.MiscRoleCustomisation);
        ArsonistMajor = new("ARSONIST_MAJOR", "#DB7601", PackType.MiscRoleCustomisation);
        SerialKillerStart = new("SERIALKILLER_START", "#1D4DFC", PackType.MiscRoleCustomisation);
        SerialKillerEnd = new("SERIALKILLER_END", "#1D4DFC", PackType.MiscRoleCustomisation);
        SerialKillerMajor = new("SERIALKILLER_MAJOR", "#1D4DFC", PackType.MiscRoleCustomisation);
        ShroudStart = new("SHROUD_START", "#6699FF", PackType.MiscRoleCustomisation);
        ShroudEnd = new("SHROUD_END", "#6699FF", PackType.MiscRoleCustomisation);
        ShroudMajor = new("SHROUD_MAJOR", "#6699FF", PackType.MiscRoleCustomisation);
        WerewolfStart = new("WEREWOLF_START", "#9D7038", PackType.MiscRoleCustomisation);
        WerewolfEnd = new("WEREWOLF_END", "#9D7038", PackType.MiscRoleCustomisation);
        WerewolfMajor = new("WEREWOLF_MAJOR", "#9D7038", PackType.MiscRoleCustomisation);
        VampireStart = new("VAMPIRE_START", "#A22929", PackType.MiscRoleCustomisation);
        VampireEnd = new("VAMPIRE_END", "#A22929", PackType.MiscRoleCustomisation);
        VampireMajor = new("VAMPIRE_MAJOR", "#A22929", PackType.MiscRoleCustomisation);
        AuditorStart = new("AUDITOR_START", "#AEBA87", PackType.MiscRoleCustomisation);
        AuditorEnd = new("AUDITOR_END", "#E8FCC5", PackType.MiscRoleCustomisation);
        JudgeStart = new("JUDGE_START", "#C77364", PackType.MiscRoleCustomisation);
        JudgeEnd = new("JUDGE_END", "#C93D50", PackType.MiscRoleCustomisation);
        StarspawnStart = new("STARSPAWN_START", "#FCE79A", PackType.MiscRoleCustomisation);
        StarspawnEnd = new("STARSPAWN_END", "#999CFF", PackType.MiscRoleCustomisation);
        CursedSoulStart = new("CURSEDSOUL_START", "#4FFF9F", PackType.MiscRoleCustomisation);
        CursedSoulEnd = new("CURSEDSOUL_END", "#B54FFF", PackType.MiscRoleCustomisation);
        CursedSoulMajor = new("CURSEDSOUL_MAJOR", "#4FFF9F", PackType.MiscRoleCustomisation);
        JackalStart = new("JACKAL_START", "#404040", PackType.MiscRoleCustomisation);
        JackalEnd = new("JACKAL_END", "#D0D0D0", PackType.MiscRoleCustomisation);
        JackalMajor = new("JACKAL_MAJOR", "#D0D0D0", PackType.MiscRoleCustomisation);
        PandoraStart = new("PANDORA_START", "#B545FF", PackType.MiscRoleCustomisation);
        PandoraEnd = new("PANDORA_END", "#FF004E", PackType.MiscRoleCustomisation);
        PandoraMajor = new("PANDORA_MAJOR", "#FF004E", PackType.MiscRoleCustomisation);
        ComplianceStart = new("COMPLIANCE_START", "#2D44B5", PackType.MiscRoleCustomisation);
        ComplianceMiddle = new("COMPLIANCE_MIDDLE", "#AE1B1E", PackType.MiscRoleCustomisation);
        ComplianceEnd = new("COMPLIANCE_END", "#FC9F32", PackType.MiscRoleCustomisation);
        ComplianceMajor = new("COMPLIANCE_MAJOR", "#FC9F32", PackType.MiscRoleCustomisation);
        EgotistStart = new("EGOTIST_START", "#359f3f", PackType.MiscRoleCustomisation);
        EgotistEnd = new("EGOTIST_END", "#3f359f", PackType.MiscRoleCustomisation);
        EgotistMajor = new("EGOTIST_MAJOR", "#3f359f", PackType.MiscRoleCustomisation);
        Neutral = new("NEUTRAL", "#A9A9A9", PackType.MiscRoleCustomisation);
        StonedHidden = new("STONED_HIDDEN", "#9C9A9A", PackType.MiscRoleCustomisation);
        Lovers = new("LOVERS", "#FEA6FA", PackType.MiscRoleCustomisation);
        FrogsStart = new("FROGS_START", "#1e49cf", PackType.MiscRoleCustomisation);
        FrogsEnd = new("FROGS_END", "#1e49cf", PackType.MiscRoleCustomisation);
        FrogsMajor = new("FROGS_MAJOR", "#1e49cf", PackType.MiscRoleCustomisation);
        LionsStart = new("LIONS_START", "#ffc34f", PackType.MiscRoleCustomisation);
        LionsEnd = new("LIONS_END", "#ffc34f", PackType.MiscRoleCustomisation);
        LionsMajor = new("LIONS_MAJOR", "#ffc34f", PackType.MiscRoleCustomisation);
        HawksStart = new("HAWKS_START", "#a81538", PackType.MiscRoleCustomisation);
        HawksEnd = new("HAWKS_END", "#a81538", PackType.MiscRoleCustomisation);
        HawksMajor = new("HAWKS_MAJOR", "#a81538", PackType.MiscRoleCustomisation);



        PlayerNumber = new("PLAYER_NUMBER", 0, PackType.Testing, 0, 15, true, _ => Constants.CustomNumbers());
        DumpSpriteSheets = new("DUMP_SHEETS", false, PackType.Testing);
        DebugPackLoading = new("DEBUG_LOADING", false, PackType.Testing);
        ShowOverlayWhenJailed = new("SHOW_TO_JAILED", true, PackType.Testing);
        ShowOverlayAsJailor = new("SHOW_TO_JAILOR", false, PackType.Testing);
        IconsInRoleReveal = new("ROLE_REVEAL_ICONS", true, PackType.Testing);
        SelectDisplay = new("SELECT_DISPLAY", DisplayType.RoleCard, PackType.Testing, useTranslations: true);
        SelectTestingFaction = new("SELECTED_TESTING_FACTION", FactionType.NONE, PackType.Testing, useTranslations: true, values:
            () => SettingsAndTestingUI.Instance?.IsBTOS2 == true ? BTOS2Factions : VanillaFactions);
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