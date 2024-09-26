namespace FancyUI.SilhouetteSwapper;

public class SilhouetteSet(string name) : Pack(name, PackType.SilhouetteSets)
{
    public Dictionary<ModType, SilhouetteAssets> Assets { get; set; } = [];

    private static readonly string[] AnimationTypes = [ "Running", "Idle", "RandomAction", "Victory" ];

    public override void Debug()
    {

    }

    public override void Load()
    {
        if (Name is "Vanilla" or "BTOS2")
        {
            Deleted = true;
            return;
        }

        Logging.LogMessage($"Loading {Name} Silhouette Set", true);
        Deleted = false;

        try
        {
            foreach (var mod in MainFolders)
            {
                if (mod == "PlayerNumbers")
                    continue;

                var modPath = Path.Combine(PackPath, mod);

                if (!Directory.Exists(modPath))
                {
                    Directory.CreateDirectory(modPath);
                    Logging.LogWarning($"{Name} {mod} folder doesn't exist");
                    continue;
                }

                if (Enum.TryParse<ModType>(mod, out var type))
                {
                }
            }
        }
        catch (Exception e)
        {
            Logging.LogError($"Unable to load sprites for {Name} because:\n{e}");
        }
    }

    public override void Delete()
    {

    }

    public override void Reload()
    {

    }
}