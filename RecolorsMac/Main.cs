using System.Diagnostics;

namespace RecolorsMac;

[SalemMod]
[SalemMenuItem]
[DynamicSettings]
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
            Utils.Log($"Something failed because this happened D:\n{e}", true);
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

    public ModSettings.DropdownSetting SelectedIconPack
    {
        get
        {
            return new()
            {
                Name = "Selected Icon Pack",
                Description = "The selected icon will start replacing the visible icons with the images you put in. If it can't find the valid image or pack, it will be replaced by the mod's default files\nVanilla - No pack selected",
                Options = GetPackNames(),
                OnChanged = AssetManager.TryLoadingSprites
            };
        }
    }

    private static List<string> GetPackNames()
    {
        try
        {
            var result = new List<string>() { "Vanilla" };

            foreach (var dir in Directory.EnumerateDirectories(AssetManager.ModPath))
            {
                if (!dir.Contains("Vanilla"))
                    result.Add(dir.SanitisePath());
            }

            return result;
        }
        catch
        {
            return new() { "Vanilla" };
        }
    }
}