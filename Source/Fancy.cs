namespace FancyUI;

[SalemMod]
[SalemMenuItem]
[DynamicSettings]
public class Fancy
{
    public void Start()
    {
        Logging.InitVoid("Fancy");
        Logging.LogMessage("Fancifying...", true);

        try
        {
            AssetManager.LoadAssets();
            FancyMenu.Icon = AssetManager.Thumbnail;
        }
        catch (Exception e)
        {
            Logging.LogError($"Something failed because this happened D:\n{e}");
        }

        Logging.LogMessage("Fancy!", true);
    }

    public static readonly SalemMenuButton FancyMenu = new()
    {
        Label = "Fancy UI",
        OnClick = OpenMenu
    };

    public static void OpenMenu()
    {
        var go = UObject.Instantiate(AssetManager.AssetGOs["DownloaderUI"], CacheHomeSceneController.Controller.SafeArea.transform, false);
        go.transform.localPosition = new(0, 0, 0);
        go.transform.localScale = new(2.25f, 2.25f, 2.25f);
        go.AddComponent<FancyMenu>();
    }

    public ModSettings.DropdownSetting SelectedIconPack => new()
    {
        Name = "Selected Icon Pack",
        Description = "The selected icon will start replacing the visible icons with the images you put in. If it can't find the valid image or pack, it will be replaced by the mod's default files. May require a game restart for the in-text icons to change.\nVanilla - No pack selected.",
        Options = GetPackNames(PackType.IconPacks),
        OnChanged = x => AssetManager.TryLoadingSprites(x, PackType.IconPacks)
    };

    public ModSettings.DropdownSetting ChoiceMentions1 => new()
    {
        Name = "Selected Vanilla Mention Style",
        Description = "The selected mention style will dictate which icons are used for the icons in text. May require a game restart.",
        Options = GetOptions(ModType.Vanilla, true),
        Available = Constants.EnableIcons(),
        OnChanged = x => AttemptCreateSpriteSheet(ModType.Vanilla, x)
    };

    public ModSettings.DropdownSetting ChoiceMentions2 => new()
    {
        Name = "Selected BTOS2 Mention Style",
        Description = "The selected mention style will dictate which icons are used for the icons in text. May require a game restart.",
        Options = GetOptions(ModType.BTOS2, true),
        Available = Constants.BTOS2Exists() && Constants.EnableIcons(),
        OnChanged = x => AttemptCreateSpriteSheet(ModType.BTOS2, x)
    };

    public ModSettings.DropdownSetting FactionOverride1 => new()
    {
        Name = "Override Vanilla Faction",
        Description = "Only icons from the selected faction will appear in vanilla games.",
        Options = GetOptions(ModType.Vanilla, false),
        Available = Constants.EnableIcons()
    };

    public ModSettings.DropdownSetting FactionOverride2 => new()
    {
        Name = "Override BTOS2 Faction",
        Description = "Only icons from the selected faction will appear in BTOS2 games.",
        Options = GetOptions(ModType.BTOS2, false),
        Available = Constants.BTOS2Exists() && Constants.EnableIcons()
    };

    public ModSettings.CheckboxSetting CustomNumbers => new()
    {
        Name = "Use Custom Numbers",
        Description = "Select whether you want to use the icon pack's rendition of player numbers or the game's.",
        Available = Constants.EnableIcons()
    };

    public ModSettings.IntegerInputSetting EasterEggChance => new()
    {
        Name = "Easter Egg Icon Chance",
        Description = "The chance of a different than normal icon appearing for roles.",
        DefaultValue = 5,
        AvailableInGame = true,
        Available = Constants.EnableIcons()
    };

    public ModSettings.CheckboxSetting AllEasterEggs => new()
    {
        Name = "All Easter Eggs Are Active",
        Description = "Toggles whether all of the previously selected icon pack's easter eggs will be used, or only the selected icon pack's.",
        DefaultValue = false,
        AvailableInGame = true,
        Available = AssetManager.GlobalEasterEggs.Count > 0
    };

    public ModSettings.CheckboxSetting PlayerPanelEasterEggs => new()
    {
        Name = "Enable Easter Eggs In Player Panel",
        Description = "Toggles whether easter egg icons appear or not in the ability panel.",
        DefaultValue = false,
        AvailableInGame = true,
        Available = Constants.EnableIcons() && Constants.EasterEggChance() > 0
    };

    public ModSettings.DropdownSetting SelectedSilhouetteSet => new()
    {
        Name = "Selected Silhouette Set",
        Description = "The selected set will start replacing the current silhouettes with ones created from the images you put in. If it can't find the valid image or set, it will be replaced by the mod's default files.\nVanilla - No set selected.",
        Options = GetPackNames(PackType.SilhouetteSets),
        OnChanged = x => AssetManager.TryLoadingSprites(x, PackType.SilhouetteSets)
    };

    private static List<string> GetPackNames(PackType type)
    {
        try
        {
            var result = new List<string>() { "Vanilla" };

            foreach (var dir in Directory.EnumerateDirectories(Path.Combine(AssetManager.ModPath, type.ToString())))
            {
                if (!dir.Contains("Vanilla") && !dir.Contains("BTOS2"))
                    result.Add(dir.SanitisePath());
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

            if (AssetManager.IconPacks.TryGetValue(Constants.CurrentPack(), out var pack))
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

            return result;
        }
        catch
        {
            return [ mentionStyle ? mod.ToString() : "None" ];
        }
    }

    private static void AttemptCreateSpriteSheet(ModType mod, string name)
    {
        if (AssetManager.IconPacks.TryGetValue(Constants.CurrentPack(), out var pack))
        {
            if (!pack.Assets.TryGetValue(mod, out var iconAssets))
                return;

            var modName = mod.ToString();

            if ((!iconAssets.MentionStyles.TryGetValue(name, out var asset) || !asset) && iconAssets.BaseIcons.TryGetValue(name, out var baseIcons))
            {
                iconAssets.MentionStyles[name] = asset = pack.BuildSpriteSheet(mod, modName, name, baseIcons);

                if (asset)
                    Utils.DumpSprite(asset.spriteSheet as Texture2D, $"{name}{mod}RoleIcons", Path.Combine(pack.PackPath, modName));
            }
        }
    }
}