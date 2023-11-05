using System.Diagnostics;

namespace RecolorsWindows;

[SalemMod]
[SalemMenuItem]
public class Recolors
{
    public static string ModPath => Path.Combine(Directory.GetCurrentDirectory(), "SalemModLoader", "ModFolders", "Recolors");
    public static string DefaultPath => Path.Combine(Directory.GetCurrentDirectory(), "SalemModLoader", "ModFolders", "Recolors", "Default");

    public static Assembly Core => typeof(Recolors).Assembly;

    public void Start()
    {
        if (!Directory.Exists(ModPath))
            Directory.CreateDirectory(ModPath);

        try
        {
            AssetManager.LoadAssets();
        }
        catch (Exception e)
        {
            Console.WriteLine($"[Recolors] Something failed because this happened D:\n{e}");
        }

        Console.WriteLine("[Recolors] Recolored!");
    }

    public static SalemMenuButton MenuButton1 = new()
    {
        Label = "Icon Packs",
        OnClick = OpenDirectory
    };

    public static SalemMenuButton MenuButton2 = new()
    {
        Label = "Dump Icons",
        OnClick = OpenDefaultDirectory
    };

    private static void OpenDirectory()
    {
        if (Environment.OSVersion.Platform is PlatformID.MacOSX or PlatformID.Unix)
            Process.Start("open", $"\"{ModPath}\""); //code stolen from jan who stole from tuba
        else
            Application.OpenURL($"file://{ModPath}");
    }

    private static void OpenDefaultDirectory()
    {
        AssetManager.DumpModAssets();

        if (Environment.OSVersion.Platform is PlatformID.MacOSX or PlatformID.Unix)
            Process.Start("open", $"\"{DefaultPath}\"");
        else
            Application.OpenURL($"file://{DefaultPath}");
    }
}