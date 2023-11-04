namespace RecolorsMac;

public static class AssetManager
{
    private static readonly string[] ToRemove = new[] { Base, EasterEggs, EETT, RegTT, ".png", "_Icon1", "_Icon2", "_Icon3", "_Icon4" };

    private const string Resources = "RecolorsMac.Resources.";
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
        foreach (var (name, sprite) in RegIcons)
        {
            var directory = Path.Combine(Recolors.ModPath, "Default", "Base");

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var file = $"{directory}/{name}.png";

            if (File.Exists(file))
                File.Delete(file);

            File.WriteAllBytes(file, sprite.texture.Decompress().EncodeToPNG());
        }

        foreach (var (name, sprite) in TTIcons)
        {
            var directory = Path.Combine(Recolors.ModPath, "Default", "TTBase");

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var file = $"{directory}/{name}.png";

            if (File.Exists(file))
                File.Delete(file);

            File.WriteAllBytes(file, sprite.texture.Decompress().EncodeToPNG());
        }

        foreach (var (name, sprites) in RegEEIcons)
        {
            var directory = Path.Combine(Recolors.ModPath, "Default", "EasterEggs");

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (sprites.Count == 1)
            {
                var file = $"{directory}/{name}.png";

                if (File.Exists(file))
                    File.Delete(file);

                File.WriteAllBytes(file, sprites[0].texture.Decompress().EncodeToPNG());
            }
            else
            {
                for (var i = 0; i < sprites.Count; i++)
                {
                    var file = $"{directory}/{name}_Icon{i}.png";

                    if (File.Exists(file))
                        File.Delete(file);

                    File.WriteAllBytes(file, sprites[i].texture.Decompress().EncodeToPNG());
                }
            }
        }

        foreach (var (name, sprites) in TTEEIcons)
        {
            var directory = Path.Combine(Recolors.ModPath, "Default", "TTEasterEggs");

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (sprites.Count == 1)
            {
                var file = $"{directory}/{name}.png";

                if (File.Exists(file))
                    File.Delete(file);

                File.WriteAllBytes(file, sprites[0].texture.Decompress().EncodeToPNG());
            }
            else
            {
                for (var i = 0; i < sprites.Count; i++)
                {
                    var file = $"{directory}/{name}_Icon{i}.png";

                    if (File.Exists(file))
                        File.Delete(file);

                    File.WriteAllBytes(file, sprites[i].texture.Decompress().EncodeToPNG());
                }
            }
        }
    }

    //thanks stackoverflow and pat
    //https://stackoverflow.com/questions/51315918/how-to-encodetopng-compressed-textures-in-unity
    public static Texture2D Decompress(this Texture2D source)
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
}