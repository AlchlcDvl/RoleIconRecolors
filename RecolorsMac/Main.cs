using System.Diagnostics;
using RecolorsMac.Patches;

namespace RecolorsMac;

[SalemMod]
public class Recolors
{
    public static string ModPath => Path.Combine(Directory.GetCurrentDirectory(), "SalemModLoader", "ModFolders", "Recolors");

    public static Assembly Core => typeof(Recolors).Assembly;

    public static Recolors Instance;

    public const string Resources = "RecolorsMac.Resources.";
    public const string Base = $"{Resources}Base.";
    public const string EasterEggs = $"{Resources}EasterEggs.";
    public const string TT = $"{Resources}TT.";

    public Dictionary<string, List<Sprite>> RegIcons = new();
    public Dictionary<string, List<Sprite>> TTIcons = new();
    public Sprite Blank;
    public Sprite Thumbnail;

    private static readonly Dictionary<string, Type> PatchTypes = new()
    {
        { "alchlcdvl.recolors.mac.rolecardpatches", typeof(RoleCardPatches) },
        { "alchlcdvl.recolors.mac.abilityiconpatches", typeof(AbilityPanelPatches) }
    };

    public void Start()
    {
        Instance = this;

        if (!Directory.Exists(ModPath))
            Directory.CreateDirectory(ModPath);

        try
        {
            AssetManager.LoadAssets();
            PatchTypes.ForEach((x, y) => Harmony.CreateAndPatchAll(y, x));
        }
        catch (Exception e)
        {
            Console.WriteLine($"[Recolors] Something failed because this happened D:\n{e}");
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