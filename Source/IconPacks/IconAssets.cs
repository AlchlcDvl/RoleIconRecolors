namespace FancyUI.IconPacks;

public class IconAssets(string name)
{
    public string Name { get; } = name;

    public Dictionary<string, Dictionary<string, Sprite>> BaseIcons { get; set; } = [];
    public Dictionary<string, Dictionary<string, List<Sprite>>> EasterEggs { get; set; } = [];
    public Dictionary<string, TMP_SpriteAsset> MentionStyles { get; set; } = [];

    public int Count { get; set; }

    public void Debug()
    {
        Logging.LogMessage($"Debugging {Name}");
        Count = 0;

        foreach (var (name1, icons) in BaseIcons)
        {
            foreach (var (name2, icon) in icons)
            {
                if (icon.IsValid())
                {
                    Logging.LogMessage($"{name2} has a(n) {name1} sprite!");
                    Count++;
                }
            }
        }

        Logging.LogMessage($"{BaseIcons.Count} base assets loaded!");

        foreach (var (name1, icons) in EasterEggs)
        {
            foreach (var (name2, icon) in icons)
            {
                var count = icon.Count(x => x.IsValid());

                if (count > 0)
                {
                    Logging.LogMessage($"{name2} has {count} {name1} easter egg sprite(s)!");
                    Count += count;
                }
            }
        }

        Logging.LogMessage($"{EasterEggs.Count} easter egg assets loaded!");
        Logging.LogMessage($"{Count} image assets exist!");

        foreach (var (name2, sheet) in MentionStyles)
        {
            if (sheet)
            {
                Logging.LogMessage($"{name2} mention style exists!");
                Count++;
            }
        }

        Logging.LogMessage($"{MentionStyles.Count} mention styles exist!");
        Logging.LogMessage($"{Count} net assets exist!");
        Logging.LogMessage($"{Name} Debugged!");
    }

    public void Delete()
    {
        BaseIcons.Values.ForEach(x => x.Values.ForEach(UObject.Destroy));
        BaseIcons.Values.ForEach(x => x.Clear());
        BaseIcons.Clear();

        EasterEggs.Values.ForEach(x => x.Values.ForEach(y => y.ForEach(UObject.Destroy)));
        EasterEggs.Values.ForEach(x => x.Values.ForEach(y => y.Clear()));
        EasterEggs.Values.ForEach(x => x.Clear());
        EasterEggs.Clear();

        MentionStyles.Values.ForEach(UObject.Destroy);
        MentionStyles.Clear();
    }
}