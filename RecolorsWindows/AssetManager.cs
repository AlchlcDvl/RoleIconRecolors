namespace RecolorsWindows;

public static class AssetManager
{
    private static readonly string[] ToRemove = new[] { Base, EasterEggs, EETT, RegTT, ".png", "_Icon1", "_Icon2", "_Icon3", "_Icon4" };

    private const string Resources = "RecolorsWindows.Resources.";
    private const string Base = $"{Resources}Base.";
    private const string EasterEggs = $"{Resources}EasterEggs.";
    private const string RegTT = $"{Resources}TTBase.";
    private const string EETT = $"{Resources}TTEasterEggs.";

    private static readonly Dictionary<string, List<Sprite>> RegEEIcons = new();
    private static readonly Dictionary<string, List<Sprite>> TTEEIcons = new();
    private static readonly Dictionary<string, IconPack> IconPacks = new();
    public static Sprite Blank;
    public static Sprite Thumbnail;

    public static IconPack Default;

    public static string ModPath => $"{Path.GetDirectoryName(Application.dataPath)}\\SalemModLoader\\ModFolders\\Recolors";
    public static string DefaultPath => $"{ModPath}\\Default";

    private static Assembly Core => typeof(Recolors).Assembly;

    public static Sprite GetSprite(string name, bool allowEE = true)
    {
        if (name.Contains("Blank"))
            return Blank;

        var iconPack = IconPacks.TryGetValue(Constants.CurrentPack, out var pack) ? pack : Default;

        if (!iconPack.RegIcons.TryGetValue(name, out var sprite))
        {
            Console.WriteLine($"[Recolors] Couldn't find regular {name} in {iconPack.Name}'s recources");

            if (!Default.RegIcons.TryGetValue(name, out sprite))
            {
                Console.WriteLine($"[Recolors] Couldn't find regular {name} in mod recources");
                return Blank;
            }
            else
                return sprite;
        }

        if (RegEEIcons.TryGetValue(name, out var sprites) && URandom.RandomRangeInt(1, 101) <= Constants.EasterEggChance && allowEE)
            return sprites.Random();

        return sprite;
    }

    public static Sprite GetTTSprite(string name, bool allowEE = true)
    {
        if (name.Contains("Blank"))
            return Blank;

        var iconPack = IconPacks.TryGetValue(Constants.CurrentPack, out var pack) ? pack : Default;

        if (!iconPack.TTIcons.TryGetValue(name, out var sprite))
        {
            Console.WriteLine($"[Recolors] Couldn't find TT {name} in {iconPack.Name}'s recources");

            if (!Default.TTIcons.TryGetValue(name, out sprite))
            {
                Console.WriteLine($"[Recolors] Couldn't find TT {name} in mod recources");
                return GetSprite(name, allowEE);
            }
            else
                return sprite;
        }

        if (TTEEIcons.TryGetValue(name, out var sprites) && URandom.RandomRangeInt(1, 101) <= Constants.EasterEggChance && allowEE)
            return sprites.Random();

        return sprite;
    }

    public static void LoadAssets()
    {
        RegEEIcons.Clear();
        TTEEIcons.Clear();
        Default = new("Default");

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
                else if (x.Contains(EasterEggs))
                {
                    if (RegEEIcons.ContainsKey(name))
                        RegEEIcons[name].Add(sprite);
                    else
                        RegEEIcons.Add(name, new() { sprite });
                }
                else if (x.Contains(EETT))
                {
                    if (TTEEIcons.ContainsKey(name))
                        TTEEIcons[name].Add(sprite);
                    else
                        TTEEIcons.Add(name, new() { sprite });
                }
                else if (x.Contains(RegTT))
                    Default.TTIcons.Add(name, sprite);
                else if (x.Contains(Base))
                    Default.RegIcons.Add(name, sprite);
            }
        });

        IconPacks.Add("Default", Default);
        Recolors.MenuButton.Icon = Thumbnail;
    }

    //thanks pat
    public static void DumpModAssets()
    {
        if (!Directory.Exists(ModPath))
            Directory.CreateDirectory(ModPath);

        if (!Directory.Exists(DefaultPath))
            Directory.CreateDirectory(DefaultPath);

        foreach (var (name, sprite) in Default.RegIcons)
        {
            var directory = $"{DefaultPath}\\Base";
            Directory.CreateDirectory(directory);
            var file = $"{directory}\\{name}.png";
            File.WriteAllBytes($"{directory}\\{name}.png", sprite.texture.Decompress().EncodeToPNG());
        }

        foreach (var (name, sprite) in Default.TTIcons)
        {
            var directory = $"{DefaultPath}\\TTBase";
            Directory.CreateDirectory(directory);
            File.WriteAllBytes($"{directory}\\{name}.png", sprite.texture.Decompress().EncodeToPNG());
        }

        foreach (var (name, sprites) in RegEEIcons)
        {
            var directory = $"{DefaultPath}\\EasterEggs";
            Directory.CreateDirectory(directory);

            if (sprites.Count == 1)
                File.WriteAllBytes($"{directory}\\{name}.png", sprites[0].texture.Decompress().EncodeToPNG());
            else
            {
                for (var i = 0; i < sprites.Count; i++)
                    File.WriteAllBytes($"{directory}\\{name}_Icon{i + 1}.png", sprites[i].texture.Decompress().EncodeToPNG());
            }
        }

        foreach (var (name, sprites) in TTEEIcons)
        {
            var directory = $"{DefaultPath}\\TTEasterEggs";
            Directory.CreateDirectory(directory);

            if (sprites.Count == 1)
                File.WriteAllBytes($"{directory}\\{name}.png", sprites[0].texture.Decompress().EncodeToPNG());
            else
            {
                for (var i = 0; i < sprites.Count; i++)
                    File.WriteAllBytes($"{directory}\\{name}_Icon{i + 1}.png", sprites[i].texture.Decompress().EncodeToPNG());
            }
        }
    }

    //thanks stackoverflow and pat
    //https://stackoverflow.com/questions/51315918/how-to-encodetopng-compressed-textures-in-unity
    private static Texture2D Decompress(this Texture2D source)
    {
        var renderTex = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
        Graphics.Blit(source, renderTex);
        var previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        var readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }

    private static Texture2D EmptyTexture() => new(2, 2, TextureFormat.ARGB32, true);

    private static Texture2D LoadDiskTexture(string fileName, string subfolder, string folder)
    {
        fileName = fileName.SanitisePath();
        var path = $"{ModPath}\\{folder}\\{subfolder}\\{fileName}.png";

        try
        {
            var texture = EmptyTexture();
            _ = ImageConversion.LoadImage(texture, File.ReadAllBytes(path), false);
            texture.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontUnloadUnusedAsset;
            texture.name = fileName;
            UObject.DontDestroyOnLoad(texture);
            return texture;
        }
        catch
        {
            Console.WriteLine($"[Recolors] Error loading {path} from disk");
            return null;
        }
    }

    private static Sprite LoadSpriteFromDisk(string fileName, string subfolder, string folder)
    {
        fileName = fileName.SanitisePath();
        var path = $"{ModPath}\\{folder}\\{subfolder}\\{fileName}.png";

        if (!File.Exists(path))
        {
            Console.WriteLine($"[Recolors] Path {folder} > {subfolder} > {fileName} was missing");
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

    private static string SanitisePath1(this string path) => path.Split('/')[^1];

    private static string SanitisePath2(this string path) => path.Split('\\')[^1];

    private static string SanitisePath(this string path)
    {
        path = path.SanitisePath1();
        path = path.SanitisePath2();
        ToRemove.ForEach(x => path = path.Replace(x, ""));
        return path;
    }

    public static void TryLoadingSprites(string packName)
    {
        if (IconPacks.ContainsKey(packName))
            return;

        var folder = $"{ModPath}\\{packName}";

        if (!Directory.Exists(folder))
        {
            Console.WriteLine($"[Recolors] Path to {folder} was missing");
            return;
        }

        var pack = new IconPack(packName);
        var baseFolder = $"{folder}\\Base";

        if (Directory.Exists(baseFolder))
        {
            foreach (var file in Directory.EnumerateFiles(baseFolder, "*.png"))
            {
                var filePath = $"{baseFolder}\\{file}";
                var sprite = LoadSpriteFromDisk(filePath, "Base", packName);
                filePath = filePath.SanitisePath();
                pack.RegIcons.Add(filePath, sprite);
            }
        }
        else
            Console.WriteLine($"[Recolors] {packName} Base folder doesn't exist");

        var ttFolder = $"{folder}\\TTBase";

        if (Directory.Exists(ttFolder))
        {
            foreach (var file in Directory.EnumerateFiles(ttFolder, "*.png"))
            {
                var filePath = $"{ttFolder}\\{file}";
                var sprite = LoadSpriteFromDisk(filePath, "TTBase", packName);
                filePath = filePath.SanitisePath();
                pack.TTIcons.Add(filePath, sprite);
            }
        }
        else
            Console.WriteLine($"[Recolors] {packName} TTBase folder doesn't exist");

        var eeFolder = $"{folder}\\EasterEggs";

        if (Directory.Exists(eeFolder))
        {
            foreach (var file in Directory.EnumerateFiles(eeFolder, "*.png"))
            {
                var filePath = $"{eeFolder}\\{file}";
                var sprite = LoadSpriteFromDisk(filePath, "EasterEggs", packName);
                filePath = filePath.SanitisePath();

                if (RegEEIcons.ContainsKey(filePath))
                    RegEEIcons[filePath].Add(sprite);
                else
                    RegEEIcons.Add(filePath, new() { sprite });
            }
        }
        else
            Console.WriteLine($"[Recolors] {packName} EasterEggs folder doesn't exist");

        var tteeFolder = $"{folder}\\TTEasterEggs";

        if (Directory.Exists(tteeFolder))
        {
            foreach (var file in Directory.EnumerateFiles(tteeFolder, "*.png"))
            {
                var filePath = $"{tteeFolder}\\{file}";
                var sprite = LoadSpriteFromDisk(filePath, "TTEasterEggs", packName);
                filePath = filePath.SanitisePath();

                if (TTEEIcons.ContainsKey(filePath))
                    TTEEIcons[filePath].Add(sprite);
                else
                    TTEEIcons.Add(filePath, new() { sprite });
            }
        }
        else
            Console.WriteLine($"[Recolors] {packName} TTEasterEggs folder doesn't exist");

        IconPacks.Add(packName, pack);
    }
}