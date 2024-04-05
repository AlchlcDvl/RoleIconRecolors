using System.Diagnostics;

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
            MenuButton.Icon = AssetManager.Thumbnail;
            ModSettings.SetString("Download Recommended Icon Packs", "None", "alchlcsystm.recolors");
        }
        catch (Exception e)
        {
            Logging.LogError($"Something failed because this happened D:\n{e}");
        }

        Logging.LogMessage("Recolored!", true);
    }

    public static readonly SalemMenuButton MenuButton = new()
    {
        Label = "Open Icons Folder",
        OnClick = Open
    };

    public static void Open()
    {
        // code stolen from jan who stole from tuba
        if (Environment.OSVersion.Platform is PlatformID.MacOSX or PlatformID.Unix)
            Process.Start("open", $"\"{AssetManager.ModPath}\"");
        else
            Application.OpenURL($"file://{AssetManager.ModPath}");
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
        Description = "The selected mention style will dictate which icons are used for the mentions. May require a game restart.",
        Options = GetMentionStyles(ModType.Vanilla)
    };

    public ModSettings.DropdownSetting ChoiceMentions2 => new()
    {
        Name = "Selected BTOS2 Mention Style",
        Description = "The selected mention style will dictate which icons are used for the mentions. May require a game restart.",
        Options = GetMentionStyles(ModType.BTOS2),
        Available = Constants.BTOS2Exists
    };

    public ModSettings.DropdownSetting FactionOverride1 => new()
    {
        Name = "Override Vanilla Faction",
        Description = "Only icons from the selected faction will appear in vanilla games.",
        Options = GetFactionOverrides(ModType.Vanilla)
    };

    public ModSettings.DropdownSetting FactionOverride2 => new()
    {
        Name = "Override BTOS2 Faction",
        Description = "Only icons from the selected faction will appear in BTOS2 games.",
        Options = GetFactionOverrides(ModType.BTOS2),
        Available = Constants.BTOS2Exists
    };

    public ModSettings.CheckboxSetting CustomNumbers => new()
    {
        Name = "Use Custom Numbers",
        Description = "Select whether you want to use the mod's rendition of player numbers or the game's.",
        DefaultValue = false
    };

    public ModSettings.DropdownSetting DownloadIcons => new()
    {
        Name = "Download Recommended Icon Packs",
        Description = "Downloads icon packs recommended by the mod creator.\nVanilla - Icons used in the vanilla game to be used as a reference for icon packs.\nBTOS2 - Icons used in BTOS2 games to be used as a reference for icons specifically set for BTOS2.\nRecolors - Art by MysticMismagius, Haapsalu, faketier, splarg, Det, Wevit, Nova and Nidoskull.",
        Options = [ "None", "Vanilla", "BTOS2", "Recolors" ],
        OnChanged = Download.DownloadIcons
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

    private static List<string> GetMentionStyles(ModType mod)
    {
        try
        {
            var result = new List<string>();

            if (AssetManager.IconPacks.TryGetValue(Constants.CurrentPack, out var pack))
            {
                if (pack.Assets.TryGetValue(mod, out var assets))
                {
                    foreach (var (folder, icons) in assets.BaseIcons)
                    {
                        if (icons.Count > 0 && assets.MentionStyles.TryGetValue(folder, out var sheet) && sheet)
                            result.Add(folder);
                    }
                }
            }

            result.Add(mod.ToString());
            return result;
        }
        catch
        {
            return [ mod.ToString() ];
        }
    }

    private static List<string> GetFactionOverrides(ModType mod)
    {
        try
        {
            var result = new List<string>() { "None" };

            if (AssetManager.IconPacks.TryGetValue(Constants.CurrentPack, out var pack))
            {
                if (pack.Assets.TryGetValue(mod, out var assets))
                {
                    foreach (var (folder, icons) in assets.BaseIcons)
                    {
                        if (icons.Count > 0)
                            result.Add(folder);
                    }
                }
            }

            return result;
        }
        catch
        {
            return [ "None" ];
        }
    }
}