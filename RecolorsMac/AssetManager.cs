namespace RecolorsMac;

public static class AssetManager
{
    private static readonly List<string> ToRemove = new() { Recolors.Base, Recolors.EasterEggs, Recolors.TT, ".png", "_IconA", "_IconB", "_IconC", "_IconD", "_Flame1", "_Flame2" };

    public static Sprite GetSprite(string name)
    {
        if (name == "Blank")
            return Recolors.Instance.Blank;

        if (!Recolors.Instance.RegIcons.TryGetValue(name, out var sprites))
        {
            Console.WriteLine($"[Recolors] Couldn't find regular {name} in recources");
            return Recolors.Instance.Blank;
        }

        if (sprites.Count > 1 && ((URandom.RandomRangeInt(1, 100) <= Constants.EasterEggChance && Constants.EasterEggChance > 0) || Constants.EasterEggChance == 100))
            return sprites.Skip(1).Random();

        return sprites[0];
    }

    public static Sprite GetTTSprite(string name)
    {
        if (name == "Blank")
            return Recolors.Instance.Blank;

        if (!Recolors.Instance.TTIcons.TryGetValue(name, out var sprites))
        {
            Console.WriteLine($"[Recolors] Couldn't find TT {name} in recources");
            return Recolors.Instance.Blank;
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
                    Recolors.Instance.Blank = sprite;
                else if (x.Contains("Thumbnail"))
                    Recolors.Instance.Thumbnail = sprite;
                else if (x.Contains(Recolors.TT))
                {
                    if (Recolors.Instance.TTIcons.ContainsKey(name))
                        Recolors.Instance.TTIcons[name].Add(sprite);
                    else
                        Recolors.Instance.TTIcons.Add(name, new() { sprite });
                }
                else
                {
                    if (Recolors.Instance.RegIcons.ContainsKey(name))
                        Recolors.Instance.RegIcons[name].Add(sprite);
                    else
                        Recolors.Instance.RegIcons.Add(name, new() { sprite });
                }
            }
        });

        Console.WriteLine($"[Recolors] {Recolors.Instance.RegIcons.Count} Regular Assets loaded!");
        Console.WriteLine($"[Recolors] {Recolors.Instance.TTIcons.Count} TT Assets loaded!");
        Recolors.Instance.RegIcons.ForEach((x, y) => Console.WriteLine($"[Recolors] {x} has {y.Count} sprite(s)!"));
    }
}