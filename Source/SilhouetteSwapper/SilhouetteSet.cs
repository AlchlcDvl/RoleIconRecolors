namespace FancyUI.SilhouetteSwapper;

public class SilhouetteSet(string name)
{
    public Dictionary<string, SilhouetteAssets> Silhouettes { get; set; } = [];

    public string Name { get; } = name;

    public bool Deleted { get; set; }

    private string PackPath => Path.Combine(AssetManager.ModPath, "SilhouetteSets", Name);

    private static readonly string[] AnimationTypes = [ "Running", "Idle", "Action" ];

    public void Debug()
    {

    }

    public void Load()
    {

    }

    public void Delete()
    {

    }

    public void Reload()
    {

    }
}