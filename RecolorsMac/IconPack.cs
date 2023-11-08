namespace RecolorsMac;

public class IconPack
{
    public Dictionary<string, Sprite> RegIcons { get; set; }
    public Dictionary<string, Sprite> TTIcons { get; set; }
    public Dictionary<string, List<Sprite>> RegEEIcons { get; set; }
    public Dictionary<string, List<Sprite>> TTEEIcons { get; set; }
    public string Name { get; }

    public IconPack(string name)
    {
        Name = name;
        TTIcons = new();
        RegIcons = new();
        TTEEIcons = new();
        RegEEIcons = new();
    }

    public void Debug()
    {
        RegIcons.ForEach((x, _) => Utils.Log($"{Name} Icon Pack {x} has a sprite!"));
        Utils.Log($"{Name} Icon Pack {RegIcons.Count} Regular Assets loaded!");
        TTIcons.ForEach((x, _) => Utils.Log($"{Name} Icon Pack {x} has a TT sprite!"));
        Utils.Log($"{Name} Icon Pack {TTIcons.Count} TT Assets loaded!");
        RegEEIcons.ForEach((x, y) => Utils.Log($"{x} has {y.Count} easter egg sprite(s)!"));
        Utils.Log($"{Name} Icon Pack {RegEEIcons.Count} easter egg Assets loaded!");
        TTEEIcons.ForEach((x, y) => Utils.Log($"{x} has {y.Count} TT easter egg sprite(s)!"));
        Utils.Log($"{Name} Icon Pack {TTEEIcons.Count} TT easter egg Assets loaded!");
    }

    public static bool operator ==(IconPack a, IconPack b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Name == b.Name;
    }

    public static bool operator !=(IconPack a, IconPack b) => !(a == b);

    private bool Equals(IconPack other) => Name == other.Name && GetHashCode() == other.GetHashCode();

    public override bool Equals(object obj)
    {
        if (obj is null)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (obj.GetType() != typeof(IconPack))
            return false;

        return Equals((IconPack)obj);
    }

    public override int GetHashCode() => HashCode.Combine(Name);

    public override string ToString() => Name;

    public void Load()
    {
        var folder = $"{AssetManager.ModPath}\\{Name}";
        var baseFolder = $"{folder}\\Base";

        if (Directory.Exists(baseFolder))
        {
            foreach (var file in Directory.EnumerateFiles(baseFolder, "*.png"))
            {
                var filePath = $"{baseFolder}\\{file}";
                var sprite = AssetManager.LoadSpriteFromDisk(filePath, "Base", Name);
                filePath = filePath.SanitisePath(true);
                RegIcons.TryAdd(filePath, sprite);
            }
        }
        else
            Utils.Log($"{Name} Base folder doesn't exist");

        var ttFolder = $"{folder}\\TTBase";

        if (Directory.Exists(ttFolder))
        {
            foreach (var file in Directory.EnumerateFiles(ttFolder, "*.png"))
            {
                var filePath = $"{ttFolder}\\{file}";
                var sprite = AssetManager.LoadSpriteFromDisk(filePath, "TTBase", Name);
                filePath = filePath.SanitisePath(true);
                TTIcons.TryAdd(filePath, sprite);
            }
        }
        else
            Utils.Log($"{Name} TTBase folder doesn't exist");

        var eeFolder = $"{folder}\\EasterEggs";

        if (Directory.Exists(eeFolder))
        {
            foreach (var file in Directory.EnumerateFiles(eeFolder, "*.png"))
            {
                var filePath = $"{eeFolder}\\{file}";
                var sprite = AssetManager.LoadSpriteFromDisk(filePath, "EasterEggs", Name);
                filePath = filePath.SanitisePath(true);

                if (RegEEIcons.ContainsKey(filePath))
                    RegEEIcons[filePath].Add(sprite);
                else
                    RegEEIcons.TryAdd(filePath, new() { sprite });

                if (AssetManager.RegEEIcons.ContainsKey(filePath))
                    AssetManager.RegEEIcons[filePath].Add(sprite);
                else
                    AssetManager.RegEEIcons.TryAdd(filePath, new() { sprite });
            }
        }
        else
            Utils.Log($"{Name} EasterEggs folder doesn't exist");

        var tteeFolder = $"{folder}\\TTEasterEggs";

        if (Directory.Exists(tteeFolder))
        {
            foreach (var file in Directory.EnumerateFiles(tteeFolder, "*.png"))
            {
                var filePath = $"{tteeFolder}\\{file}";
                var sprite = AssetManager.LoadSpriteFromDisk(filePath, "TTEasterEggs", Name);
                filePath = filePath.SanitisePath(true);

                if (TTEEIcons.ContainsKey(filePath))
                    TTEEIcons[filePath].Add(sprite);
                else
                    TTEEIcons.TryAdd(filePath, new() { sprite });

                if (AssetManager.TTEEIcons.ContainsKey(filePath))
                    AssetManager.TTEEIcons[filePath].Add(sprite);
                else
                    AssetManager.TTEEIcons.TryAdd(filePath, new() { sprite });
            }
        }
        else
            Utils.Log($"{Name} TTEasterEggs folder doesn't exist");
    }
}