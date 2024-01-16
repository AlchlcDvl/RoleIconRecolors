using BepInEx.Logging;
using System.Diagnostics;

namespace IconPacks;

[SalemMod]
[SalemMenuItem]
[DynamicSettings]
public class Recolors
{
    private static readonly ManualLogSource Log = BepInEx.Logging.Logger.CreateLogSource("IconPacks");
    public static string SavedLogs = "";

    public void Start()
    {
        try
        {
            AssetManager.LoadAssets();
            MenuButton.Icon = AssetManager.Thumbnail;
            ModSettings.SetString("Download Recommended Icon Packs", "None", "alchlcsystm.recolors");
        }
        catch (Exception e)
        {
            LogError($"Something failed because this happened D:\n{e}");
        }

        LogMessage("Recolored!", true);
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
        Options = GetMentionStyles(),
        OnChanged = AssetManager.ChangeSpriteSheetStyles
    };

    public ModSettings.DropdownSetting DownloadIcons => new()
    {
        Name = "Download Recommended Icon Packs",
        Description = "Downloads icon packs recommended by the mod creator\nVanilla - Icons used in the vanilla game to be used as a reference for icon packs\nRecolors - Art by MysticMismagius, Haapsalu, faketier and Nidoskull.",
        Options = new() { "None", "Vanilla", "Recolors" },
        OnChanged = Download.DownloadIcons
    };

    private static List<string> GetPackNames()
    {
        try
        {
            var result = new List<string>() { "Vanilla" };

            foreach (var dir in Directory.EnumerateDirectories(AssetManager.ModPath))
            {
                if (!dir.Contains("Vanilla"))
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
                    if (icons.Count > 0)
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

    private static void LogSomething(object message, LogLevel type, bool logIt)
    {
        message ??= "";
        message = $"[{DateTime.UtcNow}] {message}";

        if (logIt || Constants.Debug)
            Log?.Log(type, message);

        SavedLogs += $"[{type, -7}] {message}\n";
    }

    public static void LogError(object message) => LogSomething(message, LogLevel.Error, true);

    public static void LogMessage(object message, bool logIt = false) => LogSomething(message, LogLevel.Message, logIt);

    public static void LogFatal(object message) => LogSomething(message, LogLevel.Fatal, true);

    public static void LogInfo(object message, bool logIt = false) => LogSomething(message, LogLevel.Info, logIt);

    public static void LogWarning(object message, bool logIt = false) => LogSomething(message, LogLevel.Warning, logIt);

    public static void LogDebug(object message, bool logIt = false) => LogSomething(message, LogLevel.Debug, logIt);

    public static void LogNone(object message, bool logIt = false) => LogSomething(message, LogLevel.None, logIt);

    public static void LogAll(object message, bool logIt = false) => LogSomething(message, LogLevel.All, logIt);
}