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
        Label = "Salem's Legacy",
        Icon = FromResources.LoadSprite("Mod.Resources.thumbnail.png"),
        OnClick = () => {}
    };
}