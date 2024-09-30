namespace FancyUI.Assets.SilhouetteSwapper;

public class SilhouetteAnimation(string name)
{
    public string Name { get; } = name;

    public List<Sprite> Frames { get; set; } = [];
}