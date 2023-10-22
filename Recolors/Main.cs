using System.Diagnostics;

namespace Recolors;

[SalemMod]
public class Recolors
{
    public static string ModPath => Path.Combine(Directory.GetCurrentDirectory(), "SalemModLoader", "ModFolders", "Recolors");

    public static Assembly Core => typeof(Recolors).Assembly;

    public static Recolors Instance;

    public const string Resources = "Recolors.Resources.";
    public const string Base = $"{Resources}Base.";
    public const string EasterEggs = $"{Resources}EasterEggs.";
    public const string TT = $"{Resources}TT.";

    public Dictionary<string, List<Sprite>> RegIcons = new();
    public Dictionary<string, List<Sprite>> TTIcons = new();
    public Sprite Blank;
    public Sprite Thumbnail;

    public static readonly List<string> ToRemove = new() { Base, EasterEggs, TT, ".png", "_IconA", "_IconB", "_Flame1", "_Flame2" };

    public void Start()
    {
        Instance = this;

        if (!Directory.Exists(ModPath))
            Directory.CreateDirectory(ModPath);

        try
        {
            AssetManager.LoadAssets();
        }
        catch
        {
            Console.WriteLine("[Recolors] Asset loading failed D:");
        }

        try
        {
            Harmony.CreateAndPatchAll(typeof(Patches), "alchlcdvl.recolors");
        }
        catch
        {
            Console.WriteLine("[Recolors] Patching failed D:");
        }

        Console.WriteLine("[Recolors] Recolored!");
    }
}

[SalemMenuItem]
public class MenuItem
{
    public static SalemMenuButton menuButtonName = new()
    {
        Label = "Icon Recolors",
        Icon = Recolors.Instance.Thumbnail,
        OnClick = OpenDirectory
    };

    private static void OpenDirectory()
    {
        if (Environment.OSVersion.Platform is PlatformID.MacOSX or PlatformID.Unix)
            Process.Start("open", $"\"{Recolors.ModPath}\""); //code stolen from jan who stole from tuba
        else
            Application.OpenURL($"file://{Recolors.ModPath}");
    }
}