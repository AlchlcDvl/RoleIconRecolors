namespace RecolorsWindows;

[SalemMod]
[SalemMenuItem]
public class Recolors
{
    public void Start()
    {
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

    public static readonly SalemMenuButton MenuButton = new()
    {
        Label = "Dump Icons",
        OnClick = DumpAndOpen
    };

    private static void DumpAndOpen()
    {
        AssetManager.DumpModAssets();
        //code stolen from jan who stole from tuba
        Application.OpenURL($"file://{AssetManager.DefaultPath}");
    }
}