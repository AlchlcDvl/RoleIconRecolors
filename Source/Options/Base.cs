namespace FancyUI.Options;

public abstract class Option
{
    public static readonly List<Option> All = [];
    public string ID { get; }
    public OptionType Type { get; }
    public PackType Page { get; }

    protected Option(string id, OptionType type, PackType page)
    {
        ID = id;
        Type = type;
        Page = page;
        All.Add(this);
    }

    public void SetBase(string value)
    {
        if (this is ToggleOption toggle)
            toggle.Value = bool.TryParse(value, out var val) && val;
        else if (this is IDropdown dropdown)
            dropdown.SetString(value);
        else if (this is FloatOption slider)
            slider.Value = float.TryParse(value, out var val2) ? val2 : slider.Value;
        else if (this is ColorOption color)
            color.Value = value;
    }
}