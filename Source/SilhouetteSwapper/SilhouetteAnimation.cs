namespace FancyUI.SilhouetteSwapper;

public class SilhouetteAnimation(string name)
{
    public string Name { get; set; } = name;

    public List<Sprite> Frames { get; set; } = [];
}