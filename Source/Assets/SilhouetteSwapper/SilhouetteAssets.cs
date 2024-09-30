namespace FancyUI.Assets.SilhouetteSwapper;

public class SilhouetteAssets(string name)
{
    public string Name { get; } = name;

    public Dictionary<string, SilhouetteAnimation> Animations { get; set; } = [];
}