namespace RecolorsWindows;

public static class AssetManager
{
    private static readonly string[] ToRemove = new[] { ".png" };

    private const string Resources = "RecolorsWindows.Resources.";

    public static readonly Dictionary<string, List<Sprite>> RegEEIcons = new();
    public static readonly Dictionary<string, List<Sprite>> TTEEIcons = new();
    public static readonly Dictionary<string, IconPack> IconPacks = new();
    public static Sprite Blank;
    public static Sprite Thumbnail;
    public static Sprite DownloadRecolors;
    public static Sprite DownloadVanilla;

    public static IconPack Recolors;

    public static string ModPath => Path.Combine(Path.GetDirectoryName(Application.dataPath), "SalemModLoader", "ModFolders", "Recolors");
    public static string DefaultPath => Path.Combine(ModPath, "Recolors");
    public static string VanillaPath => Path.Combine(ModPath, "Vanilla");
    public static string[] Folders = new[] { "Base", "TTBase", "EasterEggs", "TTEasterEggs" };

    private static Assembly Core => typeof(Recolors).Assembly;

    public static Sprite GetSprite(string name, bool allowEE = true)
    {
        if (name.Contains("Blank") || !Constants.EnableIcons || IconPacks.Count == 0)
            return Blank;

        var iconPack = IconPacks.TryGetValue(Constants.CurrentPack, out var pack) ? pack : Recolors;

        if (iconPack == null)
            return Blank;

        if (!iconPack.RegIcons.TryGetValue(name, out var sprite))
        {
            Utils.Log($"Couldn't find regular {name} in {iconPack.Name}'s recources");

            if (!Constants.ReplaceMissing || Recolors == null || iconPack == Recolors)
                return Blank;
            else if (!Recolors.RegIcons.TryGetValue(name, out sprite))
            {
                Utils.Log($"Couldn't find regular {name} in mod recources");
                return Blank;
            }
            else
                return sprite;
        }

        if (URandom.RandomRangeInt(1, 101) <= Constants.EasterEggChance && allowEE)
        {
            if (pack.RegEEIcons.TryGetValue(name, out var sprites) && !Constants.AllEasterEggs)
                return sprites.Random();
            else if (RegEEIcons.TryGetValue(name, out sprites))
                return sprites.Random();
        }

        return sprite;
    }

    public static Sprite GetTTSprite(string name, bool allowEE = true)
    {
        if (name.Contains("Blank") || !Constants.EnableIcons || IconPacks.Count == 0)
            return Blank;

        var iconPack = IconPacks.TryGetValue(Constants.CurrentPack, out var pack) ? pack : Recolors;

        if (iconPack == null)
            return Blank;

        if (!iconPack.TTIcons.TryGetValue(name, out var sprite))
        {
            Utils.Log($"Couldn't find TT {name} in {iconPack.Name}'s recources");

            if (!Constants.ReplaceMissing || Recolors == null || iconPack == Recolors)
                return GetSprite(name, allowEE);
            else if (!Recolors.TTIcons.TryGetValue(name, out sprite))
            {
                Utils.Log($"Couldn't find TT {name} in mod recources");
                return GetSprite(name, allowEE);
            }
            else
                return sprite;
        }

        if (URandom.RandomRangeInt(1, 101) <= Constants.EasterEggChance && allowEE)
        {
            if (pack.TTEEIcons.TryGetValue(name, out var sprites) && !Constants.AllEasterEggs)
                return sprites.Random();
            else if (TTEEIcons.TryGetValue(name, out sprites))
                return sprites.Random();
        }

        return sprite;
    }

    public static void LoadAssets()
    {
        if (!Directory.Exists(ModPath))
            Directory.CreateDirectory(ModPath);

        if (!Directory.Exists(VanillaPath))
            Directory.CreateDirectory(VanillaPath);

        if (!Directory.Exists(DefaultPath))
            Directory.CreateDirectory(DefaultPath);

        foreach (var folder in Folders)
        {
            var path = Path.Combine(DefaultPath, folder);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        RegEEIcons.Clear();
        TTEEIcons.Clear();

        Core.GetManifestResourceNames().ForEach(x =>
        {
            if (x.EndsWith(".png"))
            {
                var name = x.SanitisePath();
                var sprite = FromResources.LoadSprite(x);

                if (x.Contains("Blank"))
                    Blank = sprite;
                else if (x.Contains("Thumbnail"))
                    Thumbnail = sprite;
                else if (x.Contains("DownloadVanilla"))
                    DownloadVanilla = sprite;
                else if (x.Contains("DownloadRecolors"))
                    DownloadRecolors = sprite;
            }
        });

        TryLoadingSprites(Constants.CurrentPack);
    }

    private static Texture2D EmptyTexture() => new(2, 2, TextureFormat.ARGB32, true);

    private static Texture2D LoadDiskTexture(string fileName, string subfolder, string folder)
    {
        fileName = fileName.SanitisePath();
        var path = Path.Combine(ModPath, folder, subfolder, $"{fileName}.png");

        try
        {
            var texture = EmptyTexture();
            ImageConversion.LoadImage(texture, File.ReadAllBytes(path), false);
            texture.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontUnloadUnusedAsset;
            texture.name = fileName;
            UObject.DontDestroyOnLoad(texture);
            return texture;
        }
        catch
        {
            Utils.Log($"Error loading {path} from disk");
            return null;
        }
    }

    public static Sprite LoadSpriteFromDisk(string fileName, string subfolder, string folder)
    {
        fileName = fileName.SanitisePath();
        var path = Path.Combine(ModPath, folder, subfolder, $"{fileName}.png");

        if (!File.Exists(path))
        {
            Utils.Log($"Path {folder} > {subfolder} > {fileName} was missing");
            return null;
        }

        var texture = LoadDiskTexture(path, subfolder, folder);

        if (texture == null)
            return null;

        var sprite = Sprite.Create(texture, new(0, 0, texture.width, texture.height), new(0.5f, 0.5f), 100f);

        if (sprite == null)
            return null;

        sprite.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontUnloadUnusedAsset;
        sprite.name = fileName;
        UObject.DontDestroyOnLoad(sprite);
        return sprite;
    }

    public static string SanitisePath(this string path, bool removeIcon = false)
    {
        path = path.Split('/')[^1];
        path = path.Split('\\')[^1];
        ToRemove.ForEach(x => path = path.Replace(x, ""));
        path = path.Replace(Resources, "");
        var i = 1;

        while (i > 0 && path.Contains("_Icon") && removeIcon)
        {
            if (path.Contains($"_Icon{i}"))
                path = path.Replace($"_Icon{i}", "");

            i++;
        }

        return path;
    }

    public static void TryLoadingSprites(string packName)
    {
        if (IconPacks.ContainsKey(packName))
        {
            Utils.Log($"{packName} is already loaded");
            return;
        }

        var folder = Path.Combine(ModPath, packName);

        if (!Directory.Exists(folder))
        {
            Utils.Log($"Path to {folder} was missing");
            return;
        }

        try
        {
            var pack = new IconPack(packName);
            pack.Load();
            IconPacks.Add(packName, pack);
        }
        catch (Exception e)
        {
            Utils.Log($"ISSUE: {e}");
        }
    }
}