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

    public ModSettings.DropdownSetting ChoiceMentions => new()
    {
        Name = "Selected Mention Style",
        Description = "The selected mention style will dictate which icons are used for the mentions. If the selection is TT or VIP and certain icons don't exist, the mod will instead use the main icons of the pack and if even those don't exist, then the vanilla icons. May require a game restart.",
        Options = GetMentionStyles()
    };

    public ModSettings.DropdownSetting DownloadIcons => new()
    {
        Name = "Download Recommended Icon Packs",
        Description = "Downloads icon packs recommended by the mod creator.\nVanilla - Icons used in the vanilla game to be used as a reference for icon packs.\nBTOS2 - Icons used in BTOS2 games to be used as a reference for icons specifically set for BTOS2.\nRecolors - Art by MysticMismagius, Haapsalu, faketier, splarg, Det, Wevit and Nidoskull.",
        Options = new() { "None", "Vanilla", "BTOS2", "Recolors" },
        OnChanged = Download.DownloadIcons
    };

    public ModSettings.DropdownSetting FactionOverride => new()
    {
        Name = "Override Faction",
        Description = "Only icons from the selected faction will appear.",
        Options = GetFactionOverrides()
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
            return new() { "Vanilla" };
        }
    }

    private static List<string> GetMentionStyles()
    {
        try
        {
            var result = new List<string>();

            if (AssetManager.IconPacks.TryGetValue(Constants.CurrentPack, out var pack))
            {
                foreach (var (folder, icons) in pack.BaseIcons)
                {
                    if (icons.Count > 0 && (pack.MentionStyles[folder] || pack.BTOS2MentionStyles[folder]))
                        result.Add(folder);
                }
            }

            result.Add("Vanilla");
            return result;
        }
        catch
        {
            return new() { "Vanilla" };
        }
    }

    private static List<string> GetFactionOverrides()
    {
        try
        {
            var result = new List<string>() { "None" };

            if (AssetManager.IconPacks.TryGetValue(Constants.CurrentPack, out var pack))
            {
                foreach (var (folder, icons) in pack.BaseIcons)
                {
                    if (icons.Count > 0)
                        result.Add(folder);
                }
            }

            return result;
        }
        catch
        {
            return new() { "None" };
        }
    }
}