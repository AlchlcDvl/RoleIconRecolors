namespace RecolorsWindows;

public static class AssetManager
{
    private static readonly string[] ToRemove = new[] { Base, EasterEggs, EETT, RegTT, ".png", "_Icon1", "_Icon2", "_Icon3", "_Icon4" };

    private const string Resources = "RecolorsWindows.Resources.";
    private const string Base = $"{Resources}Base.";
    private const string EasterEggs = $"{Resources}EasterEggs.";
    private const string RegTT = $"{Resources}TTBase.";
    private const string EETT = $"{Resources}TTEasterEggs.";

    private static readonly Dictionary<string, Sprite> RegIcons = new();
    private static readonly Dictionary<string, Sprite> TTIcons = new();
    private static readonly Dictionary<string, List<Sprite>> RegEEIcons = new();
    private static readonly Dictionary<string, List<Sprite>> TTEEIcons = new();
    public static Sprite Blank;
    public static Sprite Thumbnail;

    public static Sprite GetSprite(string name, bool allowEE = true)
    {
        if (name == "Blank")
            return Blank;

        if (!RegIcons.TryGetValue(name, out var sprite))
        {
            Console.WriteLine($"[Recolors] Couldn't find regular {name} in recources");
            return Blank;
        }

        if (RegEEIcons.TryGetValue(name, out var sprites) && URandom.RandomRangeInt(1, 101) <= Constants.EasterEggChance && allowEE)
            return sprites.Random();

        return sprite;
    }

    public static Sprite GetTTSprite(string name, bool allowEE = true)
    {
        if (name == "Blank")
            return Blank;

        if (!TTIcons.TryGetValue(name, out var sprite))
        {
            Console.WriteLine($"[Recolors] Couldn't find TT {name} in recources");
            return GetSprite(name, allowEE);
        }

        if (TTEEIcons.TryGetValue(name, out var sprites) && URandom.RandomRangeInt(1, 101) <= Constants.EasterEggChance && allowEE)
            return sprites.Random();

        return sprite;
    }

    public static void LoadAssets()
    {
        RegEEIcons.Clear();
        TTEEIcons.Clear();
        TTIcons.Clear();
        RegIcons.Clear();

        Recolors.Core.GetManifestResourceNames().ForEach(x =>
        {
            if (x.EndsWith(".png"))
            {
                var name = x;
                var sprite = FromResources.LoadSprite(x);
                ToRemove.ForEach(y => name = name.Replace(y, ""));

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
                    TTIcons.Add(name, sprite);
                else
                    RegIcons.Add(name, sprite);
            }
        });

        RegIcons.ForEach((x, _) => Console.WriteLine($"[Recolors] {x} has a sprite!"));
        Console.WriteLine($"[Recolors] {RegIcons.Count} Regular Assets loaded!");
        TTIcons.ForEach((x, _) => Console.WriteLine($"[Recolors] {x} has a sprite!"));
        Console.WriteLine($"[Recolors] {TTIcons.Count} TT Assets loaded!");
        RegEEIcons.ForEach((x, y) => Console.WriteLine($"[Recolors] {x} has {y.Count} easter egg sprite(s)!"));
        TTEEIcons.ForEach((x, y) => Console.WriteLine($"[Recolors] {x} has {y.Count} tt easter egg sprite(s)!"));
        Recolors.MenuButton1.Icon = Recolors.MenuButton2.Icon = Thumbnail;
    }

    //thanks pat
    public static void DumpModAssets()
    {
        DeleteModAssets();

        if (!Directory.Exists(Recolors.DefaultPath))
            Directory.CreateDirectory(Recolors.DefaultPath);

        foreach (var (name, sprite) in RegIcons)
        {
            var directory = Path.Combine(Recolors.DefaultPath, "Base");
            Directory.CreateDirectory(directory);
            File.WriteAllBytes($"{directory}/{name}.png", sprite.texture.Decompress().EncodeToPNG());
        }

        foreach (var (name, sprite) in TTIcons)
        {
            var directory = Path.Combine(Recolors.DefaultPath, "TTBase");
            Directory.CreateDirectory(directory);
            File.WriteAllBytes($"{directory}/{name}.png", sprite.texture.Decompress().EncodeToPNG());
        }

        foreach (var (name, sprites) in RegEEIcons)
        {
            var directory = Path.Combine(Recolors.DefaultPath, "EasterEggs");
            Directory.CreateDirectory(directory);

            if (sprites.Count == 1)
                File.WriteAllBytes($"{directory}/{name}.png", sprites[0].texture.Decompress().EncodeToPNG());
            else
            {
                for (var i = 0; i < sprites.Count; i++)
                    File.WriteAllBytes($"{directory}/{name}_Icon{i + 1}.png", sprites[i].texture.Decompress().EncodeToPNG());
            }
        }

        foreach (var (name, sprites) in TTEEIcons)
        {
            var directory = Path.Combine(Recolors.DefaultPath, "TTEasterEggs");
            Directory.CreateDirectory(directory);

            if (sprites.Count == 1)
                File.WriteAllBytes($"{directory}/{name}.png", sprites[0].texture.Decompress().EncodeToPNG());
            else
            {
                for (var i = 0; i < sprites.Count; i++)
                    File.WriteAllBytes($"{directory}/{name}_Icon{i + 1}.png", sprites[i].texture.Decompress().EncodeToPNG());
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

    private static void DeleteModAssets()
    {
        if (!Directory.Exists(Recolors.DefaultPath))
            return;

        var defaultPath = new DirectoryInfo(Recolors.DefaultPath);
        defaultPath.GetFiles("*.png").Select(x => x.FullName).ForEach(File.Delete);
        defaultPath.GetDirectories().Select(x => x.FullName).ForEach(Directory.Delete);
        Directory.Delete(Recolors.DefaultPath);
    }
}