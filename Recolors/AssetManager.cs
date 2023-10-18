namespace Recolors;

public static class AssetManager
{
    public static Sprite GetSprite(string name)
    {
        if (name == "Blank")
            return Recolors.Instance.Blank;

        if (!Recolors.Instance.RegIcons.TryGetValue(name, out var sprites))
        {
            Console.WriteLine($"[Recolors] Couldn't find regular {name} in recources");
            return Recolors.Instance.RegIcons["Blank"][0];
        }

        if ((sprites.Count > 1 && URandom.RandomRangeInt(1, 100) <= Settings.EasterEggChance && Settings.EasterEggChance > 0) || Settings.EasterEggChance == 100)
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

        return sprites.Random();
    }

    public static void LoadAssets()
    {
        Recolors.Core.GetManifestResourceNames().ForEach(x =>
        {
            if (!x.Contains("Thumbnail") && x.EndsWith(".png"))
            {
                var name = x;
                var sprite = FromResources.LoadSprite(x);
                Recolors.ToRemove.ForEach(y => name = name.Replace(y, ""));

                if (x.Contains(Recolors.TT))
                {
                    if (Recolors.Instance.TTIcons.ContainsKey(name))
                        Recolors.Instance.TTIcons[name].Add(sprite);
                    else
                        Recolors.Instance.TTIcons.Add(name, new() { sprite });
                }
                else if (x.Contains("Blank"))
                    Recolors.Instance.Blank = sprite;
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
        Recolors.Instance.RegIcons.ForEach((x, y) => Console.WriteLine($"{x} has {y.Count} sprite(s)!"));
    }
}