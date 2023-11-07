namespace RecolorsWindows;

public static class AssetManager
{
    private static readonly string[] ToRemove = new[] { /*Base, EasterEggs, EETT, RegTT,*/ ".png" };

    private const string Resources = "RecolorsWindows.Resources.";
    /*private const string Base = $"{Resources}Base.";
    private const string EasterEggs = $"{Resources}EasterEggs.";
    private const string RegTT = $"{Resources}TTBase.";
    private const string EETT = $"{Resources}TTEasterEggs.";*/

    private static readonly Dictionary<string, List<Sprite>> RegEEIcons = new();
    private static readonly Dictionary<string, List<Sprite>> TTEEIcons = new();
    public static readonly Dictionary<string, IconPack> IconPacks = new();
    public static Sprite Blank;
    public static Sprite Thumbnail;
    public static Sprite Download;

    public static IconPack Recolors;

    public static string ModPath => $"{Path.GetDirectoryName(Application.dataPath)}\\SalemModLoader\\ModFolders\\Recolors";
    public static string DefaultPath => $"{ModPath}\\Recolors";
    public static string VanillaPath => $"{ModPath}\\Vanilla";

    private static Assembly Core => typeof(Recolors).Assembly;

    public static Sprite GetSprite(string name, bool allowEE = true)
    {
        if (name.Contains("Blank") || !Constants.EnableIcons)
            return Blank;

        var iconPack = IconPacks.TryGetValue(Constants.CurrentPack, out var pack) ? pack : Recolors;

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
        if (name.Contains("Blank") || !Constants.EnableIcons)
            return Blank;

        var iconPack = IconPacks.TryGetValue(Constants.CurrentPack, out var pack) ? pack : Recolors;

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
        RegEEIcons.Clear();
        TTEEIcons.Clear();
        //Recolors = new("Recolors");

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
                else if (x.Contains("Download"))
                    Download = sprite;
                /*else if (x.Contains(EasterEggs))
                {
                    if (RegEEIcons.ContainsKey(name))
                        RegEEIcons[name].Add(sprite);
                    else
                        RegEEIcons.TryAdd(name, new() { sprite });

                    if (Recolors.RegEEIcons.ContainsKey(name))
                        Recolors.RegEEIcons[name].Add(sprite);
                    else
                        Recolors.RegEEIcons.TryAdd(name, new() { sprite });
                }
                else if (x.Contains(EETT))
                {
                    if (TTEEIcons.ContainsKey(name))
                        TTEEIcons[name].Add(sprite);
                    else
                        TTEEIcons.TryAdd(name, new() { sprite });

                    if (Recolors.TTEEIcons.ContainsKey(name))
                        Recolors.TTEEIcons[name].Add(sprite);
                    else
                        Recolors.TTEEIcons.TryAdd(name, new() { sprite });
                }
                else if (x.Contains(RegTT))
                    Recolors.TTIcons.TryAdd(name, sprite);
                else if (x.Contains(Base))
                    Recolors.RegIcons.TryAdd(name, sprite);*/
            }
        });

        //IconPacks.Add("Recolors", Recolors);
        //Recolors.MenuButton.Icon = Thumbnail;
    }

    /*//thanks pat
    public static void DumpModAssets()
    {
        if (!Directory.Exists(ModPath))
            Directory.CreateDirectory(ModPath);

        if (!Directory.Exists(DefaultPath))
            Directory.CreateDirectory(DefaultPath);

        foreach (var (name, sprite) in Recolors.RegIcons)
        {
            var directory = $"{DefaultPath}\\Base";
            Directory.CreateDirectory(directory);
            var file = $"{directory}\\{name}.png";
            File.WriteAllBytes($"{directory}\\{name}.png", sprite.texture.Decompress().EncodeToPNG());
        }

        foreach (var (name, sprite) in Recolors.TTIcons)
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
    }*/

    private static Texture2D EmptyTexture() => new(2, 2, TextureFormat.ARGB32, true);

    private static Texture2D LoadDiskTexture(string fileName, string subfolder, string folder)
    {
        fileName = fileName.SanitisePath();
        var path = $"{ModPath}\\{folder}\\{subfolder}\\{fileName}.png";

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
        var path = $"{ModPath}\\{folder}\\{subfolder}\\{fileName}.png";

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
            return;

        var folder = $"{ModPath}\\{packName}";

        if (!Directory.Exists(folder))
        {
            Utils.Log($"Path to {folder} was missing");
            return;
        }

        var pack = new IconPack(packName);
        pack.Load();
        IconPacks.Add(packName, pack);
    }
}