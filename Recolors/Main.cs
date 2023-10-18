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

    private static readonly List<string> ToRemove = new() { Base, EasterEggs, TT, ".png", "_A", "_B", "_Flame1", "_Flame2" };

    public void Start()
    {
        Instance = this;

        if (!Directory.Exists(ModPath))
            Directory.CreateDirectory(ModPath);

        try
        {
            LoadAssets();
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

    private void LoadAssets()
    {
        Core.GetManifestResourceNames().ForEach(x =>
        {
            if (!x.Contains("Thumbnail") && x.EndsWith(".png"))
            {
                var name = "";
                ToRemove.ForEach(y => name = x.Replace(y, ""));
                var sprite = FromResources.LoadSprite(x);

                if (x.Contains(TT))
                {
                    if (TTIcons.ContainsKey(name))
                        TTIcons[name].Add(sprite);
                    else
                        TTIcons.Add(name, new() { sprite });
                }
                else
                {
                    if (RegIcons.ContainsKey(name))
                        RegIcons[name].Add(sprite);
                    else
                        RegIcons.Add(name, new() { sprite });
                }
            }
        });

        Console.WriteLine($"[Recolors] {RegIcons.Count} Assets loaded!");
    }
}

[SalemMenuItem]
public class MenuItem
{
    public static SalemMenuButton menuButtonName = new()
    {
        Label = "Icon Recolors",
        Icon = FromResources.LoadSprite("Recolors.Resources.Thumbnail.png"),
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