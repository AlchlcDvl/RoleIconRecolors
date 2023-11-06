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
            Utils.Log($"Something failed because this happened D:\n{e}");
        }

        Utils.Log("Recolored!", true);
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