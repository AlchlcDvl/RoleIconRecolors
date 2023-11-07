//using System.Diagnostics;

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
            DownloadVanilla.Icon = DownloadRecolors.Icon = AssetManager.Download;
        }
        catch (Exception e)
        {
            Utils.Log($"Something failed because this happened D:\n{e}");
        }

        Utils.Log("Recolored!", true);
    }

    public static readonly SalemMenuButton DownloadVanilla = new()
    {
        Label = "Download Vanilla Icons",
        OnClick = Download.DownloadVanilla
    };

    public static readonly SalemMenuButton DownloadRecolors = new()
    {
        Label = "Download Recolored Icons",
        OnClick = Download.DownloadRecolors
    };

    /*public static readonly SalemMenuButton MenuButton = new()
    {
        Label = "Dump Icons",
        OnClick = DumpAndOpen
    };

    private static void DumpAndOpen()
    {
        AssetManager.DumpModAssets();
        //code stolen from jan who stole from tuba
        Process.Start("open", $"\"{AssetManager.DefaultPath}\"");
    }*/
}