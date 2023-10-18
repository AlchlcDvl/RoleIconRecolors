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
}