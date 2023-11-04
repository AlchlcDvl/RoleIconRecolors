using System.Diagnostics;
using RecolorsWindows.Patches;

namespace RecolorsWindows;

[SalemMod]
[SalemMenuItem]
public class Recolors
{
    public static string ModPath => Path.Combine(Directory.GetCurrentDirectory(), "SalemModLoader", "ModFolders", "Recolors");

    public static Assembly Core => typeof(Recolors).Assembly;

    public static Dictionary<string, List<Sprite>> RegIcons = new();
    public static Dictionary<string, List<Sprite>> TTIcons = new();
    public static Sprite Blank;
    public static Sprite Thumbnail;

    private static readonly Dictionary<string, Type> PatchTypes = new()
    {
        { "alchlcdvl.recolors.windows.rolecardpatches", typeof(RoleCardPatches) },
        { "alchlcdvl.recolors.windows.abilityiconpatches", typeof(AbilityPanelPatches) },
        { "alchlcdvl.recolors.windows.mentionpatches", typeof(MentionPatches) }
    };

    public void Start()
    {
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

    public static SalemMenuButton MenuButton = new()
    {
        Label = "Icon Recolors",
        Icon = Thumbnail,
        OnClick = OpenDirectory
    };

    private static void OpenDirectory()
    {
        if (Environment.OSVersion.Platform is PlatformID.MacOSX or PlatformID.Unix)
            Process.Start("open", $"\"{ModPath}\""); //code stolen from jan who stole from tuba
        else
            Application.OpenURL($"file://{ModPath}");
    }
}