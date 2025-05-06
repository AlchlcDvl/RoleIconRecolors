namespace FancyUI.Options;

public abstract class Option
{
    public static readonly List<Option> All = [];

    protected string ID { get; }
    public PackType Page { get; }
    public Func<bool> SetActive { get; }
    public Action UponChanged { get; }

    public abstract Setting BoxedSetting { get; set; }
    public abstract object BoxedValue { get; set; }

    protected Option(string id, PackType page, Func<bool> setActive, Action uponChanged)
    {
        ID = id;
        Page = page;
        SetActive = setActive ?? Blanks.BlankTrue;
        UponChanged = uponChanged ?? Blanks.BlankVoid;
        All.Add(this);
    }
}