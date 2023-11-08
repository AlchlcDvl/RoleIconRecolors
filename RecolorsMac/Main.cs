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
            DownloadRecolors.Icon = AssetManager.DownloadRecolors;
            DownloadVanilla.Icon = AssetManager.DownloadVanilla;
            MenuButton.Icon = AssetManager.Thumbnail;
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

    public static readonly SalemMenuButton MenuButton = new()
    {
        Label = "Open Icons Folder",
        OnClick = Open
    };

    private static void Open()
    {
        //code stolen from jan who stole from tuba
        Process.Start("open", $"\"{AssetManager.ModPath}\"");
    }
}