namespace RecolorsMac;

public static class AssetManager
{
    private static readonly List<string> ToRemove = new() { Base, EasterEggs, TT, ".png", "_IconA", "_IconB", "_IconC", "_IconD" };

    private const string Resources = "RecolorsMac.Resources.";
    private const string Base = $"{Resources}Base.";
    private const string EasterEggs = $"{Resources}EasterEggs.";
    private const string TT = $"{Resources}TT.";

    public static Sprite GetSprite(string name, bool allowEE = true)
    {
        if (name == "Blank")
            return Recolors.Blank;

        if (!Recolors.RegIcons.TryGetValue(name, out var sprites))
        {
            Console.WriteLine($"[Recolors] Couldn't find regular {name} in recources");
            return Recolors.Blank;
        }

        if (sprites.Count > 1 && ((URandom.RandomRangeInt(1, 100) <= Constants.EasterEggChance) || Constants.EasterEggChance == 100) && allowEE)
            return sprites.Skip(1).Random();

        return sprites[0];
    }

    public static Sprite GetTTSprite(string name)
    {
        if (name == "Blank")
            return Recolors.Blank;

        if (!Recolors.TTIcons.TryGetValue(name, out var sprites))
        {
            Console.WriteLine($"[Recolors] Couldn't find TT {name} in recources");
            return Recolors.Blank;
        }

        return sprites[0];
    }

    public static void LoadAssets()
    {
        Recolors.Core.GetManifestResourceNames().ForEach(x =>
        {
            if (x.EndsWith(".png"))
            {
                var name = x;
                var sprite = FromResources.LoadSprite(x);
                ToRemove.ForEach(y => name = name.Replace(y, ""));

                if (x.Contains("Blank"))
                    Recolors.Blank = sprite;
                else if (x.Contains("Thumbnail"))
                    Recolors.Thumbnail = sprite;
                else if (x.Contains(TT))
                {
                    if (Recolors.TTIcons.ContainsKey(name))
                        Recolors.TTIcons[name].Add(sprite);
                    else
                        Recolors.TTIcons.Add(name, new() { sprite });
                }
                else
                {
                    if (Recolors.RegIcons.ContainsKey(name))
                        Recolors.RegIcons[name].Add(sprite);
                    else
                        Recolors.RegIcons.Add(name, new() { sprite });
                }
            }
        });

        Console.WriteLine($"[Recolors] {Recolors.RegIcons.Count} Regular Assets loaded!");
        Console.WriteLine($"[Recolors] {Recolors.TTIcons.Count} TT Assets loaded!");
        Recolors.RegIcons.ForEach((x, y) => Console.WriteLine($"[Recolors] {x} has {y.Count} sprite(s)!"));
        Recolors.MenuButton.Icon = Recolors.Thumbnail;
    }
}