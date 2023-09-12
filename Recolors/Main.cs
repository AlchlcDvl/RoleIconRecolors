namespace Recolors;

[SalemMod]
public class Recolors
{
    public static string ModPath => Path.Combine(Directory.GetCurrentDirectory(), "SalemModLoader", "ModFolders", "Recolors");

    public void Start()
    {
        if (!Directory.Exists(ModPath))
            Directory.CreateDirectory(ModPath);

        LoadAssets();

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

    }
}

[SalemMenuItem]
public class MenuItem
{
    public static SalemMenuButton menuButtonName = new()
    {
        Label = "Icon Recolors",
        Icon = FromResources.LoadSprite("Recolors.Resources.thumbnail.png"),
        OnClick = OpenDirectory
    };

    private static void OpenDirectory()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "SalemModLoader", "ModFolders", "Recolors");

        if (Environment.OSVersion.Platform is PlatformID.MacOSX or PlatformID.Unix)
            System.Diagnostics.Process.Start("open", "\"" + path + "\""); //code stolen from jan who stole from tuba
        else
            Application.OpenURL("file://" + path);
    }
}