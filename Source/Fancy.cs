namespace FancyUI;

[SalemMod, WitchcraftMod(typeof(Fancy), "Fancy UI", [ "Assets" ], true)]
public class Fancy
{
    public static WitchcraftMod Instance { get; private set; }

    public void Start()
    {
        Instance = ModSingleton<Fancy>.Instance!;

        Instance.Message("Fancying...", true);

        Assets = Instance.Assets;

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
        } catch {}

        Instance.Message("Fancy!", true);
    }

    public static AssetManager Assets { get; private set; }

    [UponAssetsLoaded]
    public static void UponLoad()
    {
        Blank = Assets.GetSprite("Blank");
        FancyAssetManager.Attack = Assets.GetSprite("Attack");
        FancyAssetManager.Defense = Assets.GetSprite("Defense");
        Ethereal = Assets.GetSprite("Ethereal");

        Grayscale = Assets.GetMaterial("GrayscaleMaterial");

        LoadingGif = Assets.GetGif("Placeholder")!;
        LoadingGif.RenderAllFrames();
        Loading = new("Loading") { Frames = [ .. LoadingGif.Frames.Select(x => x.RenderedSprite) ] };

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

    public static ColorOption MainUIThemeWood;
    public static ColorOption MainUIThemePaper;
    public static ColorOption MainUIThemeLeather;
    public static ColorOption MainUIThemeMetal;
    public static ColorOption MainUIThemeFire;
    public static ColorOption MainUIThemeWax;

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

    public static FloatOption WoodShade;
    public static FloatOption PaperShade;
    public static FloatOption MetalShade;
    public static FloatOption LeatherShade;
    public static FloatOption FireShade;
    public static FloatOption WaxShade;

    public static FloatOption GeneralBrightness;
    public static FloatOption GrayscaleAmount;

    public static ToggleOption ColorWood;
    public static ToggleOption ColorPaper;
    public static ToggleOption ColorMetal;
    public static ToggleOption ColorLeather;
    public static ToggleOption ColorFire;
    public static ToggleOption ColorWax;

    // === Town (1) ===
    public static ColorOption TownUIThemeWood;
    public static ColorOption TownUIThemePaper;
    public static ColorOption TownUIThemeLeather;
    public static ColorOption TownUIThemeMetal;
    public static ColorOption TownUIThemeFire;
    public static ColorOption TownUIThemeWax;

    // === Coven (2) ===
    public static ColorOption CovenUIThemeWood;
    public static ColorOption CovenUIThemePaper;
    public static ColorOption CovenUIThemeLeather;
    public static ColorOption CovenUIThemeMetal;
    public static ColorOption CovenUIThemeFire;
    public static ColorOption CovenUIThemeWax;

    // === SerialKiller (3) ===
    public static ColorOption SerialKillerUIThemeWood;
    public static ColorOption SerialKillerUIThemePaper;
    public static ColorOption SerialKillerUIThemeLeather;
    public static ColorOption SerialKillerUIThemeMetal;
    public static ColorOption SerialKillerUIThemeFire;
    public static ColorOption SerialKillerUIThemeWax;

    // === Arsonist (4) ===
    public static ColorOption ArsonistUIThemeWood;
    public static ColorOption ArsonistUIThemePaper;
    public static ColorOption ArsonistUIThemeLeather;
    public static ColorOption ArsonistUIThemeMetal;
    public static ColorOption ArsonistUIThemeFire;
    public static ColorOption ArsonistUIThemeWax;

    // === Werewolf (5) ===
    public static ColorOption WerewolfUIThemeWood;
    public static ColorOption WerewolfUIThemePaper;
    public static ColorOption WerewolfUIThemeLeather;
    public static ColorOption WerewolfUIThemeMetal;
    public static ColorOption WerewolfUIThemeFire;
    public static ColorOption WerewolfUIThemeWax;

    // === Shroud (6) ===
    public static ColorOption ShroudUIThemeWood;
    public static ColorOption ShroudUIThemePaper;
    public static ColorOption ShroudUIThemeLeather;
    public static ColorOption ShroudUIThemeMetal;
    public static ColorOption ShroudUIThemeFire;
    public static ColorOption ShroudUIThemeWax;

    // === Apocalypse (7) ===
    public static ColorOption ApocalypseUIThemeWood;
    public static ColorOption ApocalypseUIThemePaper;
    public static ColorOption ApocalypseUIThemeLeather;
    public static ColorOption ApocalypseUIThemeMetal;
    public static ColorOption ApocalypseUIThemeFire;
    public static ColorOption ApocalypseUIThemeWax;

    // === Executioner (8) ===
    public static ColorOption ExecutionerUIThemeWood;
    public static ColorOption ExecutionerUIThemePaper;
    public static ColorOption ExecutionerUIThemeLeather;
    public static ColorOption ExecutionerUIThemeMetal;
    public static ColorOption ExecutionerUIThemeFire;
    public static ColorOption ExecutionerUIThemeWax;

    // === Jester (9) ===
    public static ColorOption JesterUIThemeWood;
    public static ColorOption JesterUIThemePaper;
    public static ColorOption JesterUIThemeLeather;
    public static ColorOption JesterUIThemeMetal;
    public static ColorOption JesterUIThemeFire;
    public static ColorOption JesterUIThemeWax;

    // === Pirate (10) ===
    public static ColorOption PirateUIThemeWood;
    public static ColorOption PirateUIThemePaper;
    public static ColorOption PirateUIThemeLeather;
    public static ColorOption PirateUIThemeMetal;
    public static ColorOption PirateUIThemeFire;
    public static ColorOption PirateUIThemeWax;

    // === Doomsayer (11) ===
    public static ColorOption DoomsayerUIThemeWood;
    public static ColorOption DoomsayerUIThemePaper;
    public static ColorOption DoomsayerUIThemeLeather;
    public static ColorOption DoomsayerUIThemeMetal;
    public static ColorOption DoomsayerUIThemeFire;
    public static ColorOption DoomsayerUIThemeWax;

    // === Vampire (12) ===
    public static ColorOption VampireUIThemeWood;
    public static ColorOption VampireUIThemePaper;
    public static ColorOption VampireUIThemeLeather;
    public static ColorOption VampireUIThemeMetal;
    public static ColorOption VampireUIThemeFire;
    public static ColorOption VampireUIThemeWax;

    // === CursedSoul (13) ===
    public static ColorOption CursedSoulUIThemeWood;
    public static ColorOption CursedSoulUIThemePaper;
    public static ColorOption CursedSoulUIThemeLeather;
    public static ColorOption CursedSoulUIThemeMetal;
    public static ColorOption CursedSoulUIThemeFire;
    public static ColorOption CursedSoulUIThemeWax;

    // === Jackal (33) ===
    public static ColorOption JackalUIThemeWood;
    public static ColorOption JackalUIThemePaper;
    public static ColorOption JackalUIThemeLeather;
    public static ColorOption JackalUIThemeMetal;
    public static ColorOption JackalUIThemeFire;
    public static ColorOption JackalUIThemeWax;

    // === Frogs (34) ===
    public static ColorOption FrogsUIThemeWood;
    public static ColorOption FrogsUIThemePaper;
    public static ColorOption FrogsUIThemeLeather;
    public static ColorOption FrogsUIThemeMetal;
    public static ColorOption FrogsUIThemeFire;
    public static ColorOption FrogsUIThemeWax;

    // === Lions (35) ===
    public static ColorOption LionsUIThemeWood;
    public static ColorOption LionsUIThemePaper;
    public static ColorOption LionsUIThemeLeather;
    public static ColorOption LionsUIThemeMetal;
    public static ColorOption LionsUIThemeFire;
    public static ColorOption LionsUIThemeWax;

    // === Hawks (36) ===
    public static ColorOption HawksUIThemeWood;
    public static ColorOption HawksUIThemePaper;
    public static ColorOption HawksUIThemeLeather;
    public static ColorOption HawksUIThemeMetal;
    public static ColorOption HawksUIThemeFire;
    public static ColorOption HawksUIThemeWax;

    // === Judge (38) ===
    public static ColorOption JudgeUIThemeWood;
    public static ColorOption JudgeUIThemePaper;
    public static ColorOption JudgeUIThemeLeather;
    public static ColorOption JudgeUIThemeMetal;
    public static ColorOption JudgeUIThemeFire;
    public static ColorOption JudgeUIThemeWax;

    // === Auditor (39) ===
    public static ColorOption AuditorUIThemeWood;
    public static ColorOption AuditorUIThemePaper;
    public static ColorOption AuditorUIThemeLeather;
    public static ColorOption AuditorUIThemeMetal;
    public static ColorOption AuditorUIThemeFire;
    public static ColorOption AuditorUIThemeWax;

    // === Inquisitor (40) ===
    public static ColorOption InquisitorUIThemeWood;
    public static ColorOption InquisitorUIThemePaper;
    public static ColorOption InquisitorUIThemeLeather;
    public static ColorOption InquisitorUIThemeMetal;
    public static ColorOption InquisitorUIThemeFire;
    public static ColorOption InquisitorUIThemeWax;

    // === Starspawn (41) ===
    public static ColorOption StarspawnUIThemeWood;
    public static ColorOption StarspawnUIThemePaper;
    public static ColorOption StarspawnUIThemeLeather;
    public static ColorOption StarspawnUIThemeMetal;
    public static ColorOption StarspawnUIThemeFire;
    public static ColorOption StarspawnUIThemeWax;

    // === Egotist (42) ===
    public static ColorOption EgotistUIThemeWood;
    public static ColorOption EgotistUIThemePaper;
    public static ColorOption EgotistUIThemeLeather;
    public static ColorOption EgotistUIThemeMetal;
    public static ColorOption EgotistUIThemeFire;
    public static ColorOption EgotistUIThemeWax;

    // === Pandora (43) ===
    public static ColorOption PandoraUIThemeWood;
    public static ColorOption PandoraUIThemePaper;
    public static ColorOption PandoraUIThemeLeather;
    public static ColorOption PandoraUIThemeMetal;
    public static ColorOption PandoraUIThemeFire;
    public static ColorOption PandoraUIThemeWax;

    // === Compliance (44) ===
    public static ColorOption ComplianceUIThemeWood;
    public static ColorOption ComplianceUIThemePaper;
    public static ColorOption ComplianceUIThemeLeather;
    public static ColorOption ComplianceUIThemeMetal;
    public static ColorOption ComplianceUIThemeFire;
    public static ColorOption ComplianceUIThemeWax;


    public static FloatOption PlayerNumber;

    public static EnumDropdownOption<FactionType> SelectTestingFaction;
    public static EnumDropdownOption<Role> SelectTestingRole; // TODO: Implement this

    private static FactionType[] VanillaFactions;
    private static FactionType[] BTOS2Factions;

    [LoadConfigs]
    public static void LoadConfigs()
    {
        VanillaFactions = [.. GeneralUtils.GetEnumValues<FactionType>().Except([FactionType.UNKNOWN])];
        BTOS2Factions = [.. AccessTools.GetDeclaredFields(typeof(Btos2Faction)).Select(x => (FactionType)x.GetRawConstantValue())];

        SelectedIconPack = new("SELECTED_ICON_PACK", "Vanilla", PackType.IconPacks, () => GetPackNames(PackType.IconPacks), onChanged: x => TryLoadingSprites(x, PackType.IconPacks));
        SelectedSilhouetteSet = new("SELECTED_SIL_SET", "Vanilla", PackType.SilhouetteSets, () => GetPackNames(PackType.SilhouetteSets), onChanged: x => TryLoadingSprites(x,
            PackType.SilhouetteSets));

        SelectedUITheme = new("SELECTED_UI_THEME", UITheme.Vanilla, PackType.RecoloredUI, useTranslations: true);

        MentionStyle1 = new("MENTION_STYLE_1", "Regular", PackType.IconPacks, () => GetOptions(ModType.Vanilla, true), _ => Constants.EnableIcons());
        MentionStyle2 = new("MENTION_STYLE_2", "Regular", PackType.IconPacks, () => GetOptions(ModType.BTOS2, true), _ => Constants.BTOS2Exists() && Constants.EnableIcons());

        FactionOverride1 = new("FACTION_OVERRIDE_1", "None", PackType.IconPacks, () => GetOptions(ModType.Vanilla, false), _ => Constants.EnableIcons());
        FactionOverride2 = new("FACTION_OVERRIDE_2", "None", PackType.IconPacks, () => GetOptions(ModType.BTOS2, false), _ => Constants.BTOS2Exists() && Constants.EnableIcons());

        MainUIThemeFire = new("UI_FIRE", "#FFFFFF", PackType.RecoloredUI, _ => Constants.EnableCustomUI());
        MainUIThemePaper = new("UI_PAPER", "#FFFFFF", PackType.RecoloredUI, _ => Constants.EnableCustomUI());
        MainUIThemeMetal = new("UI_METAL", "#FFFFFF", PackType.RecoloredUI, _ => Constants.EnableCustomUI());
        MainUIThemeLeather = new("UI_LEATHER", "#FFFFFF", PackType.RecoloredUI, _ => Constants.EnableCustomUI());
        MainUIThemeWood = new("UI_WOOD", "#FFFFFF", PackType.RecoloredUI, _ => Constants.EnableCustomUI());
        MainUIThemeWax = new("UI_WAX", "#FFFFFF", PackType.RecoloredUI, _ => Constants.EnableCustomUI());

        FireShade = new("FIRE_SHADE", 0, PackType.RecoloredUI, -100, 100, true, _ => Constants.EnableCustomUI());
        PaperShade = new("PAPER_SHADE", 0, PackType.RecoloredUI, -100, 100, true, _ => Constants.EnableCustomUI());
        MetalShade = new("METAL_SHADE", 0, PackType.RecoloredUI, -100, 100, true, _ => Constants.EnableCustomUI());
        LeatherShade = new("LEATHER_SHADE", 0, PackType.RecoloredUI, -100, 100, true, _ => Constants.EnableCustomUI());
        WoodShade = new("WOOD_SHADE", 0, PackType.RecoloredUI, -100, 100, true, _ => Constants.EnableCustomUI());
        WaxShade = new("WAX_SHADE", 0, PackType.RecoloredUI, -100, 100, true, _ => Constants.EnableCustomUI());

        ColorFire = new("COLOR_FIRE", true, PackType.RecoloredUI, _ => Constants.GetMainUIThemeType() == UITheme.Faction);
        ColorPaper = new("COLOR_PAPER", true, PackType.RecoloredUI, _ => Constants.GetMainUIThemeType() == UITheme.Faction);
        ColorMetal = new("COLOR_METAL", true, PackType.RecoloredUI, _ => Constants.GetMainUIThemeType() == UITheme.Faction);
        ColorLeather = new("COLOR_LEATHER", true, PackType.RecoloredUI, _ => Constants.GetMainUIThemeType() == UITheme.Faction);
        ColorWood = new("COLOR_WOOD", true, PackType.RecoloredUI, _ => Constants.GetMainUIThemeType() == UITheme.Faction);
        ColorWax = new("COLOR_WAX", true, PackType.RecoloredUI, _ => Constants.GetMainUIThemeType() == UITheme.Faction);

        GeneralBrightness = new("GENERAL_BRIGHTNESS", 100, PackType.RecoloredUI, 0, 100, true, _ => Constants.EnableCustomUI());
        GrayscaleAmount = new("GRAYSCALE_AMOUNT", 100, PackType.RecoloredUI, 0, 100, true, _ => Constants.EnableCustomUI());

        // === Town (1) ===
        TownUIThemeFire    = new("TOWN_UI_FIRE",    "#06E00C", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.TOWN);
        TownUIThemePaper   = new("TOWN_UI_PAPER",   "#06E00C", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.TOWN);
        TownUIThemeMetal   = new("TOWN_UI_METAL",   "#06E00C", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.TOWN);
        TownUIThemeLeather = new("TOWN_UI_LEATHER", "#06E00C", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.TOWN);
        TownUIThemeWood    = new("TOWN_UI_WOOD",    "#06E00C", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.TOWN);
        TownUIThemeWax     = new("TOWN_UI_WAX",     "#06E00C", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.TOWN);

        // === Coven (2) ===
        CovenUIThemeFire    = new("COVEN_UI_FIRE",    "#B545FF", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.COVEN);
        CovenUIThemePaper   = new("COVEN_UI_PAPER",   "#B545FF", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.COVEN);
        CovenUIThemeMetal   = new("COVEN_UI_METAL",   "#B545FF", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.COVEN);
        CovenUIThemeLeather = new("COVEN_UI_LEATHER", "#B545FF", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.COVEN);
        CovenUIThemeWood    = new("COVEN_UI_WOOD",    "#B545FF", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.COVEN);
        CovenUIThemeWax     = new("COVEN_UI_WAX",     "#B545FF", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.COVEN);

        // === SerialKiller (3) ===
        SerialKillerUIThemeFire    = new("SERIALKILLER_UI_FIRE",    "#1D4DFC", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.SERIALKILLER);
        SerialKillerUIThemePaper   = new("SERIALKILLER_UI_PAPER",   "#1D4DFC", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.SERIALKILLER);
        SerialKillerUIThemeMetal   = new("SERIALKILLER_UI_METAL",   "#1D4DFC", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.SERIALKILLER);
        SerialKillerUIThemeLeather = new("SERIALKILLER_UI_LEATHER", "#1D4DFC", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.SERIALKILLER);
        SerialKillerUIThemeWood    = new("SERIALKILLER_UI_WOOD",    "#1D4DFC", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.SERIALKILLER);
        SerialKillerUIThemeWax     = new("SERIALKILLER_UI_WAX",     "#1D4DFC", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.SERIALKILLER);

        // === Arsonist (4) ===
        ArsonistUIThemeFire    = new("ARSONIST_UI_FIRE",    "#DB7601", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.ARSONIST);
        ArsonistUIThemePaper   = new("ARSONIST_UI_PAPER",   "#DB7601", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.ARSONIST);
        ArsonistUIThemeMetal   = new("ARSONIST_UI_METAL",   "#DB7601", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.ARSONIST);
        ArsonistUIThemeLeather = new("ARSONIST_UI_LEATHER", "#DB7601", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.ARSONIST);
        ArsonistUIThemeWood    = new("ARSONIST_UI_WOOD",    "#DB7601", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.ARSONIST);
        ArsonistUIThemeWax     = new("ARSONIST_UI_WAX",     "#DB7601", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.ARSONIST);

        // === Werewolf (5) ===
        WerewolfUIThemeFire    = new("WEREWOLF_UI_FIRE",    "#9D7038", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.WEREWOLF);
        WerewolfUIThemePaper   = new("WEREWOLF_UI_PAPER",   "#9D7038", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.WEREWOLF);
        WerewolfUIThemeMetal   = new("WEREWOLF_UI_METAL",   "#9D7038", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.WEREWOLF);
        WerewolfUIThemeLeather = new("WEREWOLF_UI_LEATHER", "#9D7038", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.WEREWOLF);
        WerewolfUIThemeWood    = new("WEREWOLF_UI_WOOD",    "#9D7038", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.WEREWOLF);
        WerewolfUIThemeWax     = new("WEREWOLF_UI_WAX",     "#9D7038", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.WEREWOLF);

        // === Shroud (6) ===
        ShroudUIThemeFire    = new("SHROUD_UI_FIRE",    "#6699FF", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.SHROUD);
        ShroudUIThemePaper   = new("SHROUD_UI_PAPER",   "#6699FF", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.SHROUD);
        ShroudUIThemeMetal   = new("SHROUD_UI_METAL",   "#6699FF", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.SHROUD);
        ShroudUIThemeLeather = new("SHROUD_UI_LEATHER", "#6699FF", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.SHROUD);
        ShroudUIThemeWood    = new("SHROUD_UI_WOOD",    "#6699FF", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.SHROUD);
        ShroudUIThemeWax     = new("SHROUD_UI_WAX",     "#6699FF", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.SHROUD);

        // === Apocalypse (7) ===
        ApocalypseUIThemeFire    = new("APOCALYPSE_UI_FIRE",    "#FF004E", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.APOCALYPSE);
        ApocalypseUIThemePaper   = new("APOCALYPSE_UI_PAPER",   "#FF004E", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.APOCALYPSE);
        ApocalypseUIThemeMetal   = new("APOCALYPSE_UI_METAL",   "#FF004E", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.APOCALYPSE);
        ApocalypseUIThemeLeather = new("APOCALYPSE_UI_LEATHER", "#FF004E", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.APOCALYPSE);
        ApocalypseUIThemeWood    = new("APOCALYPSE_UI_WOOD",    "#FF004E", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.APOCALYPSE);
        ApocalypseUIThemeWax     = new("APOCALYPSE_UI_WAX",     "#FF004E", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.APOCALYPSE);

        // === Executioner (8) ===
        ExecutionerUIThemeFire    = new("EXECUTIONER_UI_FIRE",    "#949797", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.EXECUTIONER);
        ExecutionerUIThemePaper   = new("EXECUTIONER_UI_PAPER",   "#949797", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.EXECUTIONER);
        ExecutionerUIThemeMetal   = new("EXECUTIONER_UI_METAL",   "#949797", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.EXECUTIONER);
        ExecutionerUIThemeLeather = new("EXECUTIONER_UI_LEATHER", "#949797", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.EXECUTIONER);
        ExecutionerUIThemeWood    = new("EXECUTIONER_UI_WOOD",    "#949797", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.EXECUTIONER);
        ExecutionerUIThemeWax     = new("EXECUTIONER_UI_WAX",     "#949797", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.EXECUTIONER);

        // === Jester (9) ===
        JesterUIThemeFire    = new("JESTER_UI_FIRE",    "#F5A6D4", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.JESTER);
        JesterUIThemePaper   = new("JESTER_UI_PAPER",   "#F5A6D4", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.JESTER);
        JesterUIThemeMetal   = new("JESTER_UI_METAL",   "#F5A6D4", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.JESTER);
        JesterUIThemeLeather = new("JESTER_UI_LEATHER", "#F5A6D4", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.JESTER);
        JesterUIThemeWood    = new("JESTER_UI_WOOD",    "#F5A6D4", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.JESTER);
        JesterUIThemeWax     = new("JESTER_UI_WAX",     "#F5A6D4", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.JESTER);

        // === Pirate (10) ===
        PirateUIThemeFire    = new("PIRATE_UI_FIRE",    "#ECC23E", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.PIRATE);
        PirateUIThemePaper   = new("PIRATE_UI_PAPER",   "#ECC23E", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.PIRATE);
        PirateUIThemeMetal   = new("PIRATE_UI_METAL",   "#ECC23E", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.PIRATE);
        PirateUIThemeLeather = new("PIRATE_UI_LEATHER", "#ECC23E", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.PIRATE);
        PirateUIThemeWood    = new("PIRATE_UI_WOOD",    "#ECC23E", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.PIRATE);
        PirateUIThemeWax     = new("PIRATE_UI_WAX",     "#ECC23E", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.PIRATE);

        // === Doomsayer (11) ===
        DoomsayerUIThemeFire    = new("DOOMSAYER_UI_FIRE",    "#00CC99", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.DOOMSAYER);
        DoomsayerUIThemePaper   = new("DOOMSAYER_UI_PAPER",   "#00CC99", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.DOOMSAYER);
        DoomsayerUIThemeMetal   = new("DOOMSAYER_UI_METAL",   "#00CC99", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.DOOMSAYER);
        DoomsayerUIThemeLeather = new("DOOMSAYER_UI_LEATHER", "#00CC99", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.DOOMSAYER);
        DoomsayerUIThemeWood    = new("DOOMSAYER_UI_WOOD",    "#00CC99", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.DOOMSAYER);
        DoomsayerUIThemeWax     = new("DOOMSAYER_UI_WAX",     "#00CC99", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.DOOMSAYER);

        // === Vampire (12) ===
        VampireUIThemeFire    = new("VAMPIRE_UI_FIRE",    "#A22929", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.VAMPIRE);
        VampireUIThemePaper   = new("VAMPIRE_UI_PAPER",   "#A22929", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.VAMPIRE);
        VampireUIThemeMetal   = new("VAMPIRE_UI_METAL",   "#A22929", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.VAMPIRE);
        VampireUIThemeLeather = new("VAMPIRE_UI_LEATHER", "#A22929", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.VAMPIRE);
        VampireUIThemeWood    = new("VAMPIRE_UI_WOOD",    "#A22929", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.VAMPIRE);
        VampireUIThemeWax     = new("VAMPIRE_UI_WAX",     "#A22929", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.VAMPIRE);

        // === Cursed Soul (13) ===
        CursedSoulUIThemeFire    = new("CURSED_SOUL_UI_FIRE",    "#B54FFF", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.CURSED_SOUL);
        CursedSoulUIThemePaper   = new("CURSED_SOUL_UI_PAPER",   "#7500AF", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.CURSED_SOUL);
        CursedSoulUIThemeMetal   = new("CURSED_SOUL_UI_METAL",   "#7500AF", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.CURSED_SOUL);
        CursedSoulUIThemeLeather = new("CURSED_SOUL_UI_LEATHER", "#7500AF", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.CURSED_SOUL);
        CursedSoulUIThemeWood    = new("CURSED_SOUL_UI_WOOD",    "#4FFF9F", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.CURSED_SOUL);
        CursedSoulUIThemeWax     = new("CURSED_SOUL_UI_WAX",     "#4FFF9F", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == FactionType.CURSED_SOUL);

        // === Jackal (33) ===
        JackalUIThemeFire    = new("JACKAL_UI_FIRE",    "#D0D0D0", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)33);
        JackalUIThemePaper   = new("JACKAL_UI_PAPER",   "#404040", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)33);
        JackalUIThemeMetal   = new("JACKAL_UI_METAL",   "#404040", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)33);
        JackalUIThemeLeather = new("JACKAL_UI_LEATHER", "#404040", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)33);
        JackalUIThemeWood    = new("JACKAL_UI_WOOD",    "#D0D0D0", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)33);
        JackalUIThemeWax     = new("JACKAL_UI_WAX",     "#D0D0D0", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)33);

        // === Frogs (34) ===
        FrogsUIThemeFire    = new("FROGS_UI_FIRE",    "#1E49CF", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)34);
        FrogsUIThemePaper   = new("FROGS_UI_PAPER",   "#1E49CF", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)34);
        FrogsUIThemeMetal   = new("FROGS_UI_METAL",   "#1E49CF", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)34);
        FrogsUIThemeLeather = new("FROGS_UI_LEATHER", "#1E49CF", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)34);
        FrogsUIThemeWood    = new("FROGS_UI_WOOD",    "#1E49CF", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)34);
        FrogsUIThemeWax     = new("FROGS_UI_WAX",     "#1E49CF", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)34);

        // === Lions (35) ===
        LionsUIThemeFire    = new("LIONS_UI_FIRE",    "#FFC34F", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)35);
        LionsUIThemePaper   = new("LIONS_UI_PAPER",   "#FFC34F", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)35);
        LionsUIThemeMetal   = new("LIONS_UI_METAL",   "#FFC34F", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)35);
        LionsUIThemeLeather = new("LIONS_UI_LEATHER", "#FFC34F", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)35);
        LionsUIThemeWood    = new("LIONS_UI_WOOD",    "#FFC34F", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)35);
        LionsUIThemeWax     = new("LIONS_UI_WAX",     "#FFC34F", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)35);

        // === Hawks (36) ===
        HawksUIThemeFire    = new("HAWKS_UI_FIRE",    "#A81538", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)36);
        HawksUIThemePaper   = new("HAWKS_UI_PAPER",   "#A81538", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)36);
        HawksUIThemeMetal   = new("HAWKS_UI_METAL",   "#A81538", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)36);
        HawksUIThemeLeather = new("HAWKS_UI_LEATHER", "#A81538", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)36);
        HawksUIThemeWood    = new("HAWKS_UI_WOOD",    "#A81538", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)36);
        HawksUIThemeWax     = new("HAWKS_UI_WAX",     "#A81538", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)36);

        // === Judge (38) ===
        JudgeUIThemeFire    = new("JUDGE_UI_FIRE",    "#C93D50", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)38);
        JudgeUIThemePaper   = new("JUDGE_UI_PAPER",   "#C77364", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)38);
        JudgeUIThemeMetal   = new("JUDGE_UI_METAL",   "#C77364", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)38);
        JudgeUIThemeLeather = new("JUDGE_UI_LEATHER", "#C77364", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)38);
        JudgeUIThemeWood    = new("JUDGE_UI_WOOD",    "#C93D50", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)38);
        JudgeUIThemeWax     = new("JUDGE_UI_WAX",     "#C93D50", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)38);

        // === Auditor (39) ===
        AuditorUIThemeFire    = new("AUDITOR_UI_FIRE",    "#E8FCC5", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)39);
        AuditorUIThemePaper   = new("AUDITOR_UI_PAPER",   "#AEBA87", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)39);
        AuditorUIThemeMetal   = new("AUDITOR_UI_METAL",   "#AEBA87", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)39);
        AuditorUIThemeLeather = new("AUDITOR_UI_LEATHER", "#AEBA87", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)39);
        AuditorUIThemeWood    = new("AUDITOR_UI_WOOD",    "#E8FCC5", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)39);
        AuditorUIThemeWax     = new("AUDITOR_UI_WAX",     "#E8FCC5", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)39);

        // === Inquisitor (40) ===
        InquisitorUIThemeFire    = new("INQUISITOR_UI_FIRE",    "#821252", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)40);
        InquisitorUIThemePaper   = new("INQUISITOR_UI_PAPER",   "#821252", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)40);
        InquisitorUIThemeMetal   = new("INQUISITOR_UI_METAL",   "#821252", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)40);
        InquisitorUIThemeLeather = new("INQUISITOR_UI_LEATHER", "#821252", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)40);
        InquisitorUIThemeWood    = new("INQUISITOR_UI_WOOD",    "#821252", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)40);
        InquisitorUIThemeWax     = new("INQUISITOR_UI_WAX",     "#821252", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)40);

        // === Starspawn (41) ===
        StarspawnUIThemeFire    = new("STARSPAWN_UI_FIRE",    "#999CFF", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)41);
        StarspawnUIThemePaper   = new("STARSPAWN_UI_PAPER",   "#FCE79A", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)41);
        StarspawnUIThemeMetal   = new("STARSPAWN_UI_METAL",   "#FCE79A", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)41);
        StarspawnUIThemeLeather = new("STARSPAWN_UI_LEATHER", "#FCE79A", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)41);
        StarspawnUIThemeWood    = new("STARSPAWN_UI_WOOD",    "#999CFF", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)41);
        StarspawnUIThemeWax     = new("STARSPAWN_UI_WAX",     "#999CFF", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)41);

        // === Egotist (42) ===
        EgotistUIThemeFire    = new("EGOTIST_UI_FIRE",    "#3F359F", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)42);
        EgotistUIThemePaper   = new("EGOTIST_UI_PAPER",   "#359F3F", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)42);
        EgotistUIThemeMetal   = new("EGOTIST_UI_METAL",   "#359F3F", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)42);
        EgotistUIThemeLeather = new("EGOTIST_UI_LEATHER", "#359F3F", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)42);
        EgotistUIThemeWood    = new("EGOTIST_UI_WOOD",    "#3F359F", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)42);
        EgotistUIThemeWax     = new("EGOTIST_UI_WAX",     "#3F359F", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)42);

        // === Pandora (43) ===
        PandoraUIThemeFire    = new("PANDORA_UI_FIRE",    "#DA23A7", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)43);
        PandoraUIThemePaper   = new("PANDORA_UI_PAPER",   "#B545FF", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)43);
        PandoraUIThemeMetal   = new("PANDORA_UI_METAL",   "#B545FF", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)43);
        PandoraUIThemeLeather = new("PANDORA_UI_LEATHER", "#B545FF", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)43);
        PandoraUIThemeWood    = new("PANDORA_UI_WOOD",    "#FF004E", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)43);
        PandoraUIThemeWax     = new("PANDORA_UI_WAX",     "#FF004E", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)43);

        // === Compliance (44) ===
        ComplianceUIThemeFire    = new("COMPLIANCE_UI_FIRE",    "#FC9F32", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)44);
        ComplianceUIThemePaper   = new("COMPLIANCE_UI_PAPER",   "#AE1B1E", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)44);
        ComplianceUIThemeMetal   = new("COMPLIANCE_UI_METAL",   "#AE1B1E", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)44);
        ComplianceUIThemeLeather = new("COMPLIANCE_UI_LEATHER", "#AE1B1E", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)44);
        ComplianceUIThemeWood    = new("COMPLIANCE_UI_WOOD",    "#2D44B5", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)44);
        ComplianceUIThemeWax     = new("COMPLIANCE_UI_WAX",     "#2D44B5", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && Constants.ShowFactionalWoodSettings() && SelectTestingFaction.Value == (FactionType)44);


        EasterEggChance = new("EE_CHANCE", 5, PackType.IconPacks, 0, 100, true, _ => Constants.EnableIcons());
        AnimationDuration = new("ANIM_DURATION", 2, PackType.SilhouetteSets, 0.5f, 10, setActive: _ => Constants.EnableSwaps());

        CustomNumbers = new("CUSTOM_NUMBERS", false, PackType.IconPacks, _ => Constants.EnableIcons());
        AllEasterEggs = new("ALL_EE", false, PackType.IconPacks, _ => Constants.EnableIcons());
        PlayerPanelEasterEggs = new("PLAYER_PANEL_EE", false, PackType.IconPacks, _ => Constants.EnableIcons());

        PlayerNumber = new("PLAYER_NUMBER", 0, PackType.Testing, 0, 15, true, _ => Constants.CustomNumbers());
        DumpSpriteSheets = new("DUMP_SHEETS", false, PackType.Testing);
        DebugPackLoading = new("DEBUG_LOADING", false, PackType.Testing);
        ShowOverlayWhenJailed = new("SHOW_TO_JAILED", true, PackType.Testing);
        ShowOverlayAsJailor = new("SHOW_TO_JAILOR", false, PackType.Testing);
        IconsInRoleReveal = new("ROLE_REVEAL_ICONS", true, PackType.Testing);
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

    private static string[] GetOptions(ModType mod, bool mentionStyle)
    {
        try
        {
            var result = new List<string>();

            if (IconPacks.TryGetValue(Constants.CurrentPack(), out var pack))
            {
                result.Add(mentionStyle ? "Regular" : "None");

                if (pack.Assets.TryGetValue(ModType.Common, out var assets))
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
            return [ mentionStyle ? mod.ToString() : "None" ];
        }
    }

    // private static void AttemptCreateSpriteSheet(FancyUI.ModType mod, string name)
    // {
    //     if (AssetManager.IconPacks.TryGetValue(Constants.CurrentPack(), out var pack))
    //     {
    //         if (!pack.Assets.TryGetValue(mod, out var iconAssets))
    //             return;

    //         var modName = mod.ToString();

    //         if ((!iconAssets.MentionStyles.TryGetValue(name, out var asset) || !asset) && iconAssets.BaseIcons.TryGetValue(name, out var baseIcons))
    //         {
    //             iconAssets.MentionStyles[name] = asset = pack.BuildSpriteSheet(mod, modName, name, baseIcons);
    //             Utils.DumpSprite(asset?.spriteSheet as Texture2D, $"{name}{mod}RoleIcons", Path.Combine(pack.PackPath, modName));
    //         }
    //     }
    // }
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
        var go = UObject.Instantiate(Fancy.Assets.GetGameObject("FancyUI"), CacheHomeSceneController.Controller.SafeArea.transform, false);
        go.transform.localPosition = new(0, 0, 0);
        go.transform.localScale = Vector3.one * 2f;
        go.AddComponent<UI.FancyUI>();
    }
}