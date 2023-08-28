namespace Main;

[SalemMod]
public class Main
{
    public void Start() => Console.WriteLine("Time travelling!");
}

[SalemMenuItem]
public class MenuItem
{
    public static SalemMenuButton menuButtonName = new()
    {
        Label = "Role Icon Recolors",
        Icon = FromResources.LoadSprite("Recolors.Resources.thumbnail.png"),
        OnClick = () => {}
    };
}