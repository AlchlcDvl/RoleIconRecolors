namespace IconPacks;

[SalemMod]
[SalemMenuItem]
[DynamicSettings]
public class Recolors
{
    public void Start()
    {
        Logging.InitVoid("IconPacks");
        Logging.LogMessage("Recolouring...", true);

        try
        {
            AssetManager.LoadAssets();
            IconPacks.Icon = AssetManager.Thumbnail;
        }
        catch (Exception e)
        {
            Logging.LogError($"Something failed because this happened D:\n{e}");
        }

        Logging.LogMessage("Recoloured!", true);
    }

    public static readonly SalemMenuButton IconPacks = new()
    {
        Label = "Icon Packs",
        OnClick = OpenMenu
    };

    public static void OpenMenu()
    {
        var go = UObject.Instantiate(FromAssetBundle.LoadGameObject($"{AssetManager.Resources}DownloaderUI", "DownloaderUI"), CacheHomeSceneController.Controller.SafeArea.transform, false);
        go.transform.localPosition = new(0, 0, 0);
        go.transform.localScale = new(2.25f, 2.25f, 2.25f);
        go.AddComponent<DownloadController>();
    }

    public ModSettings.DropdownSetting SelectedIconPack => new()
    {
        Name = "Selected Icon Pack",
        Description = "The selected icon will start replacing the visible icons with the images you put in. If it can't find the valid image or pack, it will be replaced by the mod's default files. May require a game restart for the mentions to change.\nVanilla - No pack selected.",
        Options = GetPackNames(),
        OnChanged = AssetManager.TryLoadingSprites
    };

    public ModSettings.DropdownSetting ChoiceMentions1 => new()
    {
        Name = "Selected Vanilla Mention Style",
        Description = "The selected mention style will dictate which icons are used for the icons in text. May require a game restart.",
        Options = GetOptions(ModType.Vanilla, true),
        Available = Constants.EnableIcons
    };

    public ModSettings.DropdownSetting ChoiceMentions2 => new()
    {
        Name = "Selected BTOS2 Mention Style",
        Description = "The selected mention style will dictate which icons are used for the icons in text. May require a game restart.",
        Options = GetOptions(ModType.BTOS2, true),
        Available = Constants.BTOS2Exists && Constants.EnableIcons
    };

    public ModSettings.DropdownSetting FactionOverride1 => new()
    {
        Name = "Override Vanilla Faction",
        Description = "Only icons from the selected faction will appear in vanilla games.",
        Options = GetOptions(ModType.Vanilla, false),
        Available = Constants.EnableIcons
    };

    public ModSettings.DropdownSetting FactionOverride2 => new()
    {
        Name = "Override BTOS2 Faction",
        Description = "Only icons from the selected faction will appear in BTOS2 games.",
        Options = GetOptions(ModType.BTOS2, false),
        Available = Constants.BTOS2Exists && Constants.EnableIcons
    };

    public ModSettings.CheckboxSetting CustomNumbers => new()
    {
        Name = "Use Custom Numbers",
        Description = "Select whether you want to use the mod's rendition of player numbers or the game's."
    };

    private static List<string> GetPackNames()
    {
        try
        {
            var result = new List<string>() { "Vanilla" };

            foreach (var dir in Directory.EnumerateDirectories(AssetManager.ModPath))
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

            if (AssetManager.IconPacks.TryGetValue(Constants.CurrentPack, out var pack))
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
}