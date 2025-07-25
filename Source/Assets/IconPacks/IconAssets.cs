namespace FancyUI.Assets.IconPacks;

public sealed class IconAssets(string name)
{
    private string Name { get; } = name;

    public Dictionary<string, Dictionary<string, Sprite>> BaseIcons { get; } = [];
    public Dictionary<string, Dictionary<string, HashSet<Sprite>>> EasterEggs { get; } = [];
    public Dictionary<string, TMP_SpriteAsset> MentionStyles { get; } = [];

    public int Count { get; private set; }

    public void Debug()
    {
        Fancy.Instance.Message($"Debugging {Name}");
        Count = 0;

        foreach (var (name1, icons) in BaseIcons)
        {
            foreach (var (name2, icon) in icons)
            {
                if (!icon.IsValid())
                    continue;

                Fancy.Instance.Message($"{name2} has a(n) {name1} sprite!");
                Count++;
            }
        }

        Fancy.Instance.Message($"{BaseIcons.Count} base assets loaded!");

        foreach (var (name1, icons) in EasterEggs)
        {
            foreach (var (name2, icon) in icons)
            {
                var count = icon.Count(x => x.IsValid());

                if (count <= 0)
                    continue;

                Fancy.Instance.Message($"{name2} has {count} {name1} easter egg sprite(s)!");
                Count += count;
            }
        }

        Fancy.Instance.Message($"{EasterEggs.Count} easter egg assets loaded!");
        Fancy.Instance.Message($"{Count} image assets exist!");

        foreach (var (name2, sheet) in MentionStyles)
        {
            if (!sheet)
                continue;

            Fancy.Instance.Message($"{name2} mention style exists!");
            Count++;
        }

        Fancy.Instance.Message($"{MentionStyles.Count} mention styles exist!");
        Fancy.Instance.Message($"{Count} net assets exist!");
        Fancy.Instance.Message($"{Name} Debugged!");
    }

    public void Delete()
    {
        BaseIcons.Values.Do(x => x.Values.Do(UObject.Destroy));
        BaseIcons.Values.Do(x => x.Clear());
        BaseIcons.Clear();

        EasterEggs.Values.Do(x => x.Values.Do(y => y.Do(UObject.Destroy)));
        EasterEggs.Values.Do(x => x.Values.Do(y => y.Clear()));
        EasterEggs.Values.Do(x => x.Clear());
        EasterEggs.Clear();

        MentionStyles.Values.Do(UObject.Destroy);
        MentionStyles.Clear();
    }
}