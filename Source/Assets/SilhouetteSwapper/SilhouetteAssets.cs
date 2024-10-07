namespace FancyUI.Assets.SilhouetteSwapper;

public class SilhouetteAssets(string name)
{
    public string Name { get; } = name;
    public int Count { get; set; }

    public Dictionary<string, SilhouetteAnimation> Animations { get; set; } = [];

    public void Debug()
    {
        Logging.LogMessage($"Debugging {Name}");
        Count = 0;

        foreach (var (name1, animation) in Animations)
        {
            if (animation.IsValid())
            {
                Logging.LogMessage($"{name1} has an animation!");
                Count++;
            }
        }

        Logging.LogMessage($"{Animations.Count} assets loaded!");
    }
}