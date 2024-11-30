namespace FancyUI;

[SalemMod]
[SalemMenuItem]
[DynamicSettings]
[WitchcraftMod(typeof(Fancy), "Fancy UI", ["Assets", "WoodMaterials"], true)]
public class Fancy
{
    public static WitchcraftMod Instance { get; private set; }

    public void Start()
    {
        Instance = ModSingleton<Fancy>.Instance;

        Instance.Message("Fancifying...", true);

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
            LoadBTOS();
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

        Grayscale = Assets.GetMaterial("GrayscaleM");

        LoadingGif = Assets.GetGif("Placeholder");
        Loading = new("Loading") { Frames = LoadingGif.Frames };

        TryLoadingSprites(Constants.CurrentPack(), PackType.IconPacks);
        LoadVanillaSpriteSheets();

        try
        {
            LoadBTOS2SpriteSheet();
        } catch {}

        FancyMenu.Icon = Assets.GetSprite("Thumbnail");
    }

    public static StringDropdownOption SelectedIconPack;
    public static StringDropdownOption SelectedSilhouetteSet;

    public static EnumDropdownOption<UITheme> SelectedUITheme;

    public static EnumDropdownOption<Role> CurrentRoleVanilla;
    public static EnumDropdownOption<Role> CurrentRoleBTOS;

    public static EnumDropdownOption<FactionType> CurrentFactionVanilla;
    public static EnumDropdownOption<FactionType> CurrentFactionBTOS;

    public static StringDropdownOption PlayerNumber;

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

    public static SliderOption EasterEggChance;
    public static SliderOption AnimationDuration;

    public static ToggleOption CustomNumbers;
    public static ToggleOption AllEasterEggs;
    public static ToggleOption PlayerPanelEasterEggs;
    public static ToggleOption DumpSpriteSheets;
    public static ToggleOption DebugPackLoading;

    [LoadConfigs]
    public static void LoadConfigs()
    {
        SelectedIconPack = new("SELECTED_ICON_PACK", "Vanilla", () => GetPackNames(PackType.IconPacks), onChanged: x => TryLoadingSprites(x, PackType.IconPacks));
        SelectedSilhouetteSet = new("SELECTED_SIL_SET", "Vanilla", () => GetPackNames(PackType.SilhouetteSets), onChanged: x => TryLoadingSprites(x, PackType.SilhouetteSets));

        SelectedUITheme = new("SELECTED_UI_THEME", UITheme.Default);

        MentionStyle1 = new("MENTION_STYLE_1", "Regular", () => GetOptions(ModType.Vanilla, true), _ => Constants.EnableIcons());
        MentionStyle2 = new("MENTION_STYLE_2", "Regular", () => GetOptions(ModType.BTOS2, true), _ => Constants.BTOS2Exists() && Constants.EnableIcons());

        FactionOverride1 = new("FACTION_OVERRIDE_1", "None", () => GetOptions(ModType.Vanilla, false), _ => Constants.EnableIcons());
        FactionOverride2 = new("FACTION_OVERRIDE_2", "None", () => GetOptions(ModType.BTOS2, false), _ => Constants.BTOS2Exists() && Constants.EnableIcons());

        MainUIThemeFire = new("UI_FIRE", "#FFFFFF", _ => Constants.EnableCustomUI());
        MainUIThemePaper = new("UI_PAPER", "#FFFFFF", _ => Constants.EnableCustomUI());
        MainUIThemeMetal = new("UI_METAL", "#FFFFFF", _ => Constants.EnableCustomUI());
        MainUIThemeLeather = new("UI_LEATHER", "#FFFFFF", _ => Constants.EnableCustomUI());
        MainUIThemeWood = new("UI_WOOD", "#FFFFFF", _ => Constants.EnableCustomUI());
        MainUIThemeWax = new("UI_WAX", "#FFFFFF", _ => Constants.EnableCustomUI());

        PlayerNumber = new("PLAYER_NUMBER", "?", () => [ "?", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" ]);

        EasterEggChance = new("EE_CHANCE", 5, 0, 100, true, _ => Constants.EnableIcons());
        AnimationDuration = new("ANIM_DURATION", 2, 0.5f, 10, setActive: _ => Constants.EnableSwaps());

        CustomNumbers = new("CUSTOM_NUMBERS", false, _ => Constants.EnableIcons());
        AllEasterEggs = new("ALL_EE", false, _ => Constants.EnableIcons());
        PlayerPanelEasterEggs = new("PLAYER_PANEL_EE", false, _ => Constants.EnableIcons());
        DumpSpriteSheets = new("DUMP_SHEETS", false);
        DebugPackLoading = new("DEBUG_LOADING", false);
    }

    public static readonly SalemMenuButton FancyMenu = new()
    {
        Label = "Fancy UI",
        OnClick = OpenMenu
    };

    public static void OpenMenu()
    {
        var go = UObject.Instantiate(Assets.GetGameObject("FancyUI"), CacheHomeSceneController.Controller.SafeArea.transform, false);
        go.transform.localPosition = new(0, 0, 0);
        go.transform.localScale = Vector3.one * 2f;
        go.transform.SetAsLastSibling();
        go.AddComponent<UI.FancyUI>();
    }

    private static IEnumerable<string> GetPackNames(PackType type)
    {
        try
        {
            var result = new List<string>() { "Vanilla" };

            foreach (var dir in Directory.EnumerateDirectories(Path.Combine(Instance.ModPath, type.ToString())))
            {
                if (!dir.Contains("Vanilla") && !dir.Contains("BTOS2"))
                    result.Add(dir.FancySanitisePath());
            }

            return result;
        }
        catch
        {
            return [ "Vanilla" ];
        }
    }

    private static IEnumerable<string> GetOptions(ModType mod, bool mentionStyle)
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

            return result;
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

    //             if (asset)
    //                 Utils.DumpSprite(asset.spriteSheet as Texture2D, $"{name}{mod}RoleIcons", Path.Combine(pack.PackPath, modName));
    //         }
    //     }
    // }
}