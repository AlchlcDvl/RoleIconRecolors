using System.Diagnostics;

namespace RecolorsMac;

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

        Utils.Log("Recolored!");
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
        Process.Start("open", $"\"{AssetManager.DefaultPath}\"");
    }
}