namespace FancyUI.Options;

public abstract class Option
{
    public static readonly List<Option> All = [];

    protected string ID { get; }
    public PackType Page { get; }
    public Func<bool> SetActive { get; }

    public virtual Setting BoxedSetting { get; set; }

    protected Option(string id, PackType page, Func<bool> setActive)
    {
        ID = id;
        Page = page;
        SetActive = setActive ?? Blanks.BlankTrue;
        All.Add(this);
    }

    public abstract void Set(string value);
}