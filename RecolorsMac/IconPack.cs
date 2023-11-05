namespace RecolorsMac;

public class IconPack
{
    public Dictionary<string, Sprite> RegIcons { get; set; }
    public Dictionary<string, Sprite> TTIcons { get; set; }
    public string Name { get; }

    public IconPack(string name)
    {
        Name = name;
        TTIcons = new();
        RegIcons = new();
    }

    public void Debug()
    {
        RegIcons.ForEach((x, _) => Console.WriteLine($"[Recolors] {Name} Icon Pack {x} has a sprite!"));
        Console.WriteLine($"[Recolors] {Name} Icon Pack {RegIcons.Count} Regular Assets loaded!");
        TTIcons.ForEach((x, _) => Console.WriteLine($"[Recolors] {Name} Icon Pack {x} has a TT sprite!"));
        Console.WriteLine($"[Recolors] {Name} Icon Pack {TTIcons.Count} TT Assets loaded!");
    }
}