using BepInEx.Logging;

namespace RecolorsWindows;

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
            DownloadRecolors.Icon = AssetManager.DownloadRecolors;
            DownloadVanilla.Icon = AssetManager.DownloadVanilla;
            MenuButton.Icon = AssetManager.Thumbnail;
        }
        catch (Exception e)
        {
            LogError($"Something failed because this happened D:\n{e}", true);
        }

        LogMessage("Recolored!", true);
    }

    public static readonly SalemMenuButton DownloadVanilla = new()
    {
        Label = "Download Vanilla Icons",
        OnClick = Download.DownloadVanilla
    };

    public static readonly SalemMenuButton DownloadRecolors = new()
    {
        Label = "Download Recolored Icons",
        OnClick = Download.DownloadRecolors
    };

    public static readonly SalemMenuButton MenuButton = new()
    {
        Label = "Open Icons Folder",
        OnClick = Open
    };

    public static void Open()
    {
        //code stolen from jan who stole from tuba
        Application.OpenURL($"file://{AssetManager.ModPath}");
    }

    public ModSettings.DropdownSetting SelectedIconPack
    {
        get
        {
            return new()
            {
                Name = "Selected Icon Pack",
                Description = "The selected icon will start replacing the visible icons with the images you put in. If it can't find the valid image or pack, it will be replaced by the mod's default files\nVanilla - No pack selected",
                Options = GetPackNames(),
                OnChanged = AssetManager.TryLoadingSprites
            };
        }
    }

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

    private static void LogSomething(object message, LogLevel type, bool logIt)
    {
        logIt = logIt || Constants.Debug;

        if (logIt)
            Log?.Log(type, message);

        SavedLogs += $"[{type, -7}] {message}\n";
    }

    public static void LogError(object message, bool logIt = false) => LogSomething(message, LogLevel.Error, logIt);

    public static void LogMessage(object message, bool logIt = false) => LogSomething(message, LogLevel.Message, logIt);

    public static void LogFatal(object message, bool logIt = false) => LogSomething(message, LogLevel.Fatal, logIt);

    public static void LogInfo(object message, bool logIt = false) => LogSomething(message, LogLevel.Info, logIt);

    public static void LogWarning(object message, bool logIt = false) => LogSomething(message, LogLevel.Warning, logIt);

    public static void LogDebug(object message, bool logIt = false) => LogSomething(message, LogLevel.Debug, logIt);

    public static void LogNone(object message, bool logIt = false) => LogSomething(message, LogLevel.None, logIt);

    public static void LogAll(object message, bool logIt = false) => LogSomething(message, LogLevel.All, logIt);
}