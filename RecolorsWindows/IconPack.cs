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
        TTIcons = new();
        RegIcons = new();
        TTEEIcons = new();
        RegEEIcons = new();
    }

    public void Debug()
    {
        RegIcons.ForEach((x, _) => Utils.Log($"{Name} Icon Pack {x} has a sprite!"));
        Utils.Log($"{Name} Icon Pack {RegIcons.Count} Regular Assets loaded!");
        TTIcons.ForEach((x, _) => Utils.Log($"{Name} Icon Pack {x} has a TT sprite!"));
        Utils.Log($"{Name} Icon Pack {TTIcons.Count} TT Assets loaded!");
        RegEEIcons.ForEach((x, y) => Utils.Log($"{x} has {y.Count} easter egg sprite(s)!"));
        Utils.Log($"{Name} Icon Pack {RegEEIcons.Count} easter egg Assets loaded!");
        TTEEIcons.ForEach((x, y) => Utils.Log($"{x} has {y.Count} TT easter egg sprite(s)!"));
        Utils.Log($"{Name} Icon Pack {TTEEIcons.Count} TT easter egg Assets loaded!");
    }
}