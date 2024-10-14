namespace FancyUI.Assets.SilhouetteSwapper;

public class SilhouetteSet(string name) : Pack(name, PackType.SilhouetteSets)
{
    public Dictionary<ModType, SilhouetteAssets> Assets { get; set; } = [];

    private static readonly string[] AnimationTypes = [ "Running", "Idle", "RandomAction", "Victory" ];

    public override void Debug()
    {
        Fancy.Instance.Message($"Debugging {Name}");

        var count = 0;

        foreach (var assets in Assets.Values)
        {
            assets.Debug();
            count += assets.Count;
        }

        Fancy.Instance.Message($"{Name} {Assets.Count} asset sets loaded!");
        Fancy.Instance.Message($"{Name} {count} total assets exist!");
        Fancy.Instance.Message($"{Name} Debugged!");
    }

    public override void Load()
    {
        if (Name is "Vanilla" or "BTOS2")
        {
            Deleted = true;
            return;
        }

        Fancy.Instance.Message($"Loading {Name} Silhouette Set", true);
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
                    Fancy.Instance.Warning($"{Name} {mod} folder doesn't exist");
                    continue;
                }

                if (Enum.TryParse<ModType>(mod, out var type))
                {
                    var assets = Assets[type] = new(mod);

                    if (type == ModType.BTOS2 && !Constants.BTOS2Exists())
                        continue;

                    foreach (var name1 in ModsToFolders[mod])
                    {
                        var folder = Path.Combine(modPath, name1);

                        if (Directory.Exists(folder))
                        {
                            // foreach (var type1 in FileTypes)
                            // {
                            //     foreach (var file in Directory.GetFiles(folder, $"*.{type1}"))
                            //     {
                            //         var filePath = Path.Combine(baseFolder, $"{file.FancySanitisePath()}.{type1}");
                            //         var sprite = AssetManager.LoadDiskSprite(filePath.FancySanitisePath(), baseName, mod, Name, type1);

                            //         if (sprite.IsValid())
                            //             assets.BaseIcons[name1][filePath.FancySanitisePath(true)] = sprite;
                            //     }
                            // }
                        }
                        else
                        {
                            Fancy.Instance.Warning($"{Name} {mod} {folder} folder doesn't exist");
                            Directory.CreateDirectory(folder);
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Fancy.Instance.Error($"Unable to load sprites for {Name} because:\n{e}");
        }
    }

    public override void Delete()
    {

    }

    public override void Reload()
    {

    }
}