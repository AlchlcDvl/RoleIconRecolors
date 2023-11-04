namespace RecolorsWindows;

public class IconPack
{
    public Dictionary<string, Sprite> RegIcons { get; set; }
    public Dictionary<string, Sprite> TTIcons { get; set; }
    public Dictionary<string, List<Sprite>> RegEEIcons { get; set; }
    public Dictionary<string, List<Sprite>> TTEEIcons { get; set; }
    public string Name { get; }

    public IconPack(string name)
    {
        Name = name;
        RegEEIcons = new();
        TTEEIcons = new();
        TTIcons = new();
        RegIcons = new();
    }

    public void Debug()
    {
        RegIcons.ForEach((x, _) => Console.WriteLine($"[Recolors] {Name} Icon Pack {x} has a sprite!"));
        Console.WriteLine($"[Recolors] {Name} Icon Pack {RegIcons.Count} Regular Assets loaded!");
        TTIcons.ForEach((x, _) => Console.WriteLine($"[Recolors] {Name} Icon Pack {x} has a sprite!"));
        Console.WriteLine($"[Recolors] {Name} Icon Pack {TTIcons.Count} TT Assets loaded!");
        RegEEIcons.ForEach((x, y) => Console.WriteLine($"[Recolors] {Name} Icon Pack {x} has {y.Count} easter egg sprite(s)!"));
        TTEEIcons.ForEach((x, y) => Console.WriteLine($"[Recolors] {Name} Icon Pack {x} has {y.Count} tt easter egg sprite(s)!"));
    }
}