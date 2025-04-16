using Home.Shared;

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

    public static FloatOption GeneralBrightness;
    public static FloatOption GrayscaleAmount;

    public static FloatOption PlayerNumber;

    public static EnumDropdownOption<FactionType> SelectTestingFaction;
    public static EnumDropdownOption<ColorType> SelectColorFilter;
    public static EnumDropdownOption<Role> SelectTestingRole; // TODO: Implement this

    private static FactionType[] VanillaFactions;
    private static FactionType[] BTOS2Factions;

    public static readonly Dictionary<FactionType, Dictionary<ColorType, ColorOption>> FactionToColorMap = [];
    public static readonly Dictionary<ColorType, ColorOption> CustomUIColorsMap = [];
    public static readonly Dictionary<ColorType, ToggleOption> ColorShadeToggleMap = [];
    public static readonly Dictionary<ColorType, FloatOption> ColorShadeMap = [];

    [LoadConfigs]
    public static void LoadConfigs()
    {
        VanillaFactions = [.. GeneralUtils.GetEnumValues<FactionType>().Except([FactionType.UNKNOWN])];
        BTOS2Factions = [.. AccessTools.GetDeclaredFields(typeof(Btos2Faction)).Select(x => (FactionType)x.GetRawConstantValue())];

        BTOS2Factions.ForEach(x => FactionToColorMap[x] = []);

        SelectedIconPack = new("SELECTED_ICON_PACK", "Vanilla", PackType.IconPacks, () => GetPackNames(PackType.IconPacks), onChanged: x => TryLoadingSprites(x, PackType.IconPacks));
        SelectedSilhouetteSet = new("SELECTED_SIL_SET", "Vanilla", PackType.SilhouetteSets, () => GetPackNames(PackType.SilhouetteSets), onChanged: x => TryLoadingSprites(x,
            PackType.SilhouetteSets));

        SelectedUITheme = new("SELECTED_UI_THEME", UITheme.Vanilla, PackType.RecoloredUI, useTranslations: true);

        MentionStyle1 = new("MENTION_STYLE_1", "Regular", PackType.IconPacks, () => GetOptions(ModType.Vanilla, true), _ => Constants.EnableIcons());
        MentionStyle2 = new("MENTION_STYLE_2", "Regular", PackType.IconPacks, () => GetOptions(ModType.BTOS2, true), _ => Constants.BTOS2Exists() && Constants.EnableIcons());

        FactionOverride1 = new("FACTION_OVERRIDE_1", "None", PackType.IconPacks, () => GetOptions(ModType.Vanilla, false), _ => Constants.EnableIcons());
        FactionOverride2 = new("FACTION_OVERRIDE_2", "None", PackType.IconPacks, () => GetOptions(ModType.BTOS2, false), _ => Constants.BTOS2Exists() && Constants.EnableIcons());

        SelectColorFilter = new("COLOR_FILTER", ColorType.Wood, PackType.RecoloredUI, useTranslations: true);

        var colors = GeneralUtils.GetEnumValues<ColorType>().Where(x => x != ColorType.All).ToDictionary(x => x, x => x.ToString().ToUpperInvariant());
        var factions = BTOS2Factions.Where(x => x is not (Btos2Faction.Lovers or Btos2Faction.Cannibal or Btos2Faction.None)).ToDictionary(x => x, x => Utils.FactionName(x,
            Constants.BTOS2Exists() ? ModType.BTOS2 : ModType.Vanilla).ToUpperInvariant());

        foreach (var (type, name) in colors)
        {
            CustomUIColorsMap[type] = new($"UI_{name}", "#FFFFFF", PackType.RecoloredUI, _ => Constants.EnableCustomUI() && SelectColorFilter.Value.IsAny(type, ColorType.All));
            ColorShadeToggleMap[type] = new($"COLOR_{name}", true, PackType.RecoloredUI, _ => Constants.GetMainUIThemeType() == UITheme.Faction && SelectColorFilter.Value.IsAny(type,
                ColorType.All));
            ColorShadeMap[type] = new($"{name}_SHADE", 0, PackType.RecoloredUI, -100, 100, true, _ => Constants.EnableCustomUI() && SelectColorFilter.Value.IsAny(type, ColorType.All));

            foreach (var (faction, factionName) in factions)
            {
                var color = "";

                if (faction is < Btos2Faction.CursedSoul or (> Btos2Faction.Jackal and < Btos2Faction.Judge) or Btos2Faction.Inquisitor || (faction == Btos2Faction.CursedSoul &&
                    !Constants.BTOS2Exists()))
                {
                    color = faction.GetFactionColor();
                }
                else if (faction is Btos2Faction.Compliance or Btos2Faction.CursedSoul or Btos2Faction.Pandora)
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
                }
                else if (type == ColorType.Metal && faction is Btos2Faction.Town or Btos2Faction.Coven or Btos2Faction.Apocalypse or Btos2Faction.Vampire or Btos2Faction.Jackal or Btos2Faction.CursedSoul)
                {
                    color = faction switch
                    {
                        Btos2Faction.Town => "#737373",
                        Btos2Faction.Coven => "#CFDEE6",
                        Btos2Faction.Apocalypse => "#6E472C",
                        Btos2Faction.Vampire => "#2c1a1a",
                        Btos2Faction.Jackal => "#D9BF41",
                        Btos2Faction.CursedSoul => "#52C2EF",
                        _ => "#808080",
                    };
                }
                else
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