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

    public static StringOption SelectedIconPack;
    public static StringOption MentionStyle1;
    public static StringOption MentionStyle2;
    public static StringOption FactionOverride1;
    public static StringOption FactionOverride2;

    public static SliderOption EasterEggChance;

    public static ToggleOption CustomNumbers;

    [LoadConfigs]
    public static void LoadConfigs()
    {
        SelectedIconPack = new("SELECTED_ICON_PACK", "Vanilla", () => GetPackNames(PackType.IconPacks), onChanged: x => TryLoadingSprites(x, PackType.IconPacks));
        CustomNumbers = new("CUSTOM_NUMBERS", false, _ => Constants.EnableIcons());
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

    // public ModSettings.DropdownSetting SelectedIconPack => new()
    // {
    //     Name = "Selected Icon Pack",
    //     Description = "The selected icon will start replacing the visible icons with the images you put in. If it can't find the valid image or pack, it will be replaced by the mod's default files. May require a game restart for the in-text icons to change.\nVanilla - No pack selected.",
    //     Options = GetPackNames(PackType.IconPacks),
    //     OnChanged = x => TryLoadingSprites(x, PackType.IconPacks)
    // };

    // public ModSettings.DropdownSetting ChoiceMentions1 => new()
    // {
    //     Name = "Selected Vanilla Mention Style",
    //     Description = "The selected mention style will dictate which icons are used for the icons in text. May require a game restart.",
    //     Options = GetOptions(ModType.Vanilla, true),
    //     Available = Constants.EnableIcons(),
    //     // OnChanged = x => AttemptCreateSpriteSheet(ModType.Vanilla, x)
    // };

    // public ModSettings.DropdownSetting ChoiceMentions2 => new()
    // {
    //     Name = "Selected BTOS2 Mention Style",
    //     Description = "The selected mention style will dictate which icons are used for the icons in text. May require a game restart.",
    //     Options = GetOptions(ModType.BTOS2, true),
    //     Available = Constants.BTOS2Exists() && Constants.EnableIcons(),
    //     // OnChanged = x => AttemptCreateSpriteSheet(ModType.BTOS2, x)
    // };

    // public ModSettings.DropdownSetting FactionOverride1 => new()
    // {
    //     Name = "Override Vanilla Faction",
    //     Description = "Only icons from the selected faction will appear in vanilla games.",
    //     Options = GetOptions(ModType.Vanilla, false),
    //     Available = Constants.EnableIcons()
    // };

    // public ModSettings.DropdownSetting FactionOverride2 => new()
    // {
    //     Name = "Override BTOS2 Faction",
    //     Description = "Only icons from the selected faction will appear in BTOS2 games.",
    //     Options = GetOptions(ModType.BTOS2, false),
    //     Available = Constants.BTOS2Exists() && Constants.EnableIcons()
    // };

    // public ModSettings.CheckboxSetting CustomNumbers => new()
    // {
    //     Name = "Use Custom Numbers",
    //     Description = "Select whether you want to use the icon pack's rendition of player numbers or the game's.",
    //     Available = Constants.EnableIcons()
    // };

    // public ModSettings.IntegerInputSetting EasterEggChance => new()
    // {
    //     Name = "Easter Egg Icon Chance",
    //     Description = "The chance of a different than normal icon appearing for roles.",
    //     DefaultValue = 5,
    //     AvailableInGame = true,
    //     Available = Constants.EnableIcons()
    // };

    // public ModSettings.CheckboxSetting AllEasterEggs => new()
    // {
    //     Name = "All Easter Eggs Are Active",
    //     Description = "Toggles whether all of the previously selected icon pack's easter eggs will be used, or only the selected icon pack's.",
    //     DefaultValue = false,
    //     AvailableInGame = true,
    //     Available = GlobalEasterEggs.Count > 0
    // };

    // public ModSettings.CheckboxSetting PlayerPanelEasterEggs => new()
    // {
    //     Name = "Enable Easter Eggs In Player Panel",
    //     Description = "Toggles whether easter egg icons appear or not in the ability panel.",
    //     DefaultValue = false,
    //     AvailableInGame = true,
    //     Available = Constants.EnableIcons() && Constants.EasterEggChance() > 0
    // };

    // public ModSettings.DropdownSetting SelectedSilhouetteSet => new()
    // {
    //     Name = "Selected Silhouette Set",
    //     Description = "The selected set will start replacing the current silhouettes with ones created from the images you put in. If it can't find the valid image or set, it will be replaced by the mod's default files.\nVanilla - No set selected.",
    //     Options = GetPackNames(PackType.SilhouetteSets),
    //     OnChanged = x => TryLoadingSprites(x, PackType.SilhouetteSets)
    // };

    // public ModSettings.DropdownSetting MainUITheme => new()
    // {
    //     Name = "Main UI Theme",
    //     Description = "Dictates how the wood of the main UI loooks like.",
    //     Options = [ "Default", "Custom Input" ],
    // };

    // public ModSettings.ColorPickerSetting MainUIThemeWoodInput => new()
    // {
    //     Name = "Main UI Theme Wood Color",
    //     Description = "Dictates how the wood of the main UI loooks like",
    //     Available = Constants.GetMainUIThemeType() == "Custom Input"
    // };

    // public ModSettings.ColorPickerSetting MainUIThemeMetalInput => new()
    // {
    //     Name = "Main UI Theme Metal Color",
    //     Description = "Dictates how the metal of the main UI loooks like",
    //     Available = Constants.GetMainUIThemeType() == "Custom Input"
    // };

    // public ModSettings.ColorPickerSetting MainUIThemePaperInput => new()
    // {
    //     Name = "Main UI Theme Paper Color",
    //     Description = "Dictates how the paper of the main UI loooks like",
    //     Available = Constants.GetMainUIThemeType() == "Custom Input"
    // };

    // public ModSettings.ColorPickerSetting MainUIThemeLeatherInput => new()
    // {
    //     Name = "Main UI Theme Leather Color",
    //     Description = "Dictates how the leather of the main UI loooks like",
    //     Available = Constants.GetMainUIThemeType() == "Custom Input"
    // };

    private static List<string> GetPackNames(PackType type)
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

    private static List<string> GetOptions(ModType mod, bool mentionStyle)
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