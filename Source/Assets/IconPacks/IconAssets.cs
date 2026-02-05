namespace FancyUI.Assets.IconPacks;

public sealed class IconAssets(string name)
{
    private readonly string Name = name;

    public readonly Dictionary<string, Dictionary<string, Sprite>> BaseIcons = [];
    public readonly Dictionary<string, Dictionary<string, HashSet<Sprite>>> EasterEggs = [];
    public readonly Dictionary<string, TMP_SpriteAsset> MentionStyles = [];

    public int DebugCount { get; private set; }

    public void Debug()
    {
        Fancy.Instance.Message($"Debugging {Name}");
        DebugCount = 0;

        foreach (var (name1, icons) in BaseIcons)
        {
            foreach (var (name2, icon) in icons)
            {
                if (!icon.IsValid())
                    continue;

                Fancy.Instance.Message($"{name2} has a(n) {name1} sprite!");
                DebugCount++;
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
                DebugCount += count;
            }
        }

        Fancy.Instance.Message($"{EasterEggs.Count} easter egg assets loaded!");
        Fancy.Instance.Message($"{DebugCount} image assets exist!");

        foreach (var (name2, sheet) in MentionStyles)
        {
            if (!sheet)
                continue;

            Fancy.Instance.Message($"{name2} mention style exists!");
            DebugCount++;
        }

        Fancy.Instance.Message($"{MentionStyles.Count} mention styles exist!");
        Fancy.Instance.Message($"{DebugCount} net assets exist!");
        Fancy.Instance.Message($"{Name} Debugged!");
    }

    public void Delete()
    {
        BaseIcons.Values.Do(x => x.Values.Do(Utils.TrueDestroy));
        BaseIcons.Values.Do(x => x.Clear());
        BaseIcons.Clear();

        EasterEggs.Values.Do(x => x.Values.Do(y => y.Do(Utils.TrueDestroy)));
        EasterEggs.Values.Do(x => x.Values.Do(y => y.Clear()));
        EasterEggs.Values.Do(x => x.Clear());
        EasterEggs.Clear();

        MentionStyles.Values.Do(Utils.TrueDestroy);
        MentionStyles.Clear();
    }
}