using System.Diagnostics;
using RecolorsMac.Patches;

namespace RecolorsMac;

[SalemMod]
[SalemMenuItem]
public class Recolors
{
    public static string ModPath => Path.Combine(Directory.GetCurrentDirectory(), "SalemModLoader", "ModFolders", "Recolors");
    public static string DefaultPath => Path.Combine(Directory.GetCurrentDirectory(), "SalemModLoader", "ModFolders", "Recolors", "Default");

    public static Assembly Core => typeof(Recolors).Assembly;

    private static readonly Dictionary<string, Type> PatchTypes = new()
    {
        { "alchlcdvl.recolors.mac.rolecardpatches", typeof(RoleCardPatches) },
        { "alchlcdvl.recolors.mac.abilityiconpatches", typeof(AbilityPanelPatches) },
        { "alchlcdvl.recolors.mac.mentionpatches", typeof(MentionPatches) }
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

    public static SalemMenuButton MenuButton1 = new()
    {
        Label = "Icon Recolors",
        Icon = AssetManager.Thumbnail,
        OnClick = OpenDirectory
    };

    public static SalemMenuButton MenuButton2 = new()
    {
        Label = "Dump Mod Assets",
        Icon = AssetManager.Thumbnail,
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