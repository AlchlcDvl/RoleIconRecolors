namespace Recolors;

public static class AssetManager
{
    public static Sprite GetSprite(string name)
    {
        if (!Recolors.Instance.RegIcons.TryGetValue(name, out var sprites))
        {
            Console.WriteLine($"[Recolors] Couldn't find regular {name} in recources");
            return Recolors.Instance.RegIcons["Blank"][0];
        }

        if (sprites.Count > 1 && URandom.RandomRangeInt(0, 101) <= Settings.EasterEggChance)
            return sprites.Skip(1).Random();

        return sprites[0];
    }

    public static Sprite GetTTSprite(string name)
    {
        if (!Recolors.Instance.TTIcons.TryGetValue(name, out var sprites))
        {
            Console.WriteLine($"[Recolors] Couldn't find TT {name} in recources");
            return Recolors.Instance.RegIcons["Blank"][0];
        }

        return sprites.Random();
    }
}