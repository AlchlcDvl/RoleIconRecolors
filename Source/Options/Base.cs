namespace FancyUI.Options;

public abstract class Option
{
    public static readonly List<Option> All = [];
    public string ID { get; }
    public string Value { get; set; }
    public OptionType Type { get; }

    protected Option(string id, OptionType type, string value)
    {
        ID = id;
        Type = type;
        Value = value;
        All.Add(this);
    }

    public void SetBase(string value)
    {
        Value = value;

        if (this is ToggleOption toggle)
            toggle.Set(bool.Parse(value));
        else if (this is IDropdown dropdown)
            dropdown.SetString(value);
        else if (this is SliderOption slider)
            slider.Set(float.Parse(value));
        else if (this is ColorOption color)
            color.Set(value);
    }
}