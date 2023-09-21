namespace Recolors;

[SalemMod]
public class Recolors
{
    public static string ModPath => Path.Combine(Directory.GetCurrentDirectory(), "SalemModLoader", "ModFolders", "Recolors");

    public static Assembly Core => typeof(Recolors).Assembly;

    public const string Resources = "Recolors.Resources.";
    public const string Base = $"{Resources}Base.";
    public const string EasterEggs = $"{Resources}EasterEggs.";
    public const string TT = $"{Resources}TT.";

    public static Dictionary<string, List<Sprite>> Icons = new();

    public void Start()
    {
        if (!Directory.Exists(ModPath))
            Directory.CreateDirectory(ModPath);

        try
        {
            LoadAssets();
        }
        catch
        {
            Console.WriteLine("Asset loading failed D:");
        }

        try
        {
            Harmony.CreateAndPatchAll(typeof(Patches));
        }
        catch
        {
            Console.WriteLine("Patching failed D:");
        }

        Console.WriteLine("Recolored!");
    }

    private static void LoadAssets()
    {
        Core.GetManifestResourceNames().ToList().ForEach(x =>
        {
            if (!x.Contains("Thumbnail.png") && x.EndsWith(".png"))
            {
                var name = x.Replace(TT, "").Replace(Base, "").Replace(EasterEggs, "").Replace(".png", "");
                var sprite = FromResources.LoadSprite(x);

                if (Icons.ContainsKey(name))
                    Icons[name].Add(sprite);
                else
                    Icons.Add(name, new() { sprite });
            }
        });

        Console.WriteLine("Assets loaded!");
        Console.WriteLine($"{Icons.Count}!");
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
            System.Diagnostics.Process.Start("open", $"\"{Recolors.ModPath}\""); //code stolen from jan who stole from tuba
        else
            Application.OpenURL($"file://{Recolors.ModPath}");
    }
}