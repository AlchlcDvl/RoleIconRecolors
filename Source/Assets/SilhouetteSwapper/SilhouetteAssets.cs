namespace FancyUI.Assets.SilhouetteSwapper;

public class SilhouetteAssets(string name)
{
    public string Name { get; } = name;
    public int Count { get; set; }

    public Dictionary<string, SilhouetteAnimation> Animations { get; set; } = [];

    public void Debug()
    {
        Fancy.Instance.Message($"Debugging {Name}");
        Count = 0;

        foreach (var (name1, animation) in Animations)
        {
            if (animation.IsValid())
            {
                Fancy.Instance.Message($"{name1} has an animation!");
                Count++;
            }
        }

        Fancy.Instance.Message($"{Animations.Count} assets loaded!");
    }
}