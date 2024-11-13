namespace FancyUI.Settings;

public class Option<TValue, TSetting>(string nameId, string descId, TValue defaultValue, OptionType type)
{
    public string NameID { get; } = nameId;
    public string DescID { get; } = descId;
    public OptionType Type { get; } = type;
    public Config<TValue> Entry { get; } = Fancy.Instance.Configs.Bind(nameId, descId, defaultValue);
    public TSetting Setting { get; set; }

    public TValue Get() => Entry.Value;
}

public class ToggleOption(string nameId, string descId, bool defaultValue) : Option<bool, ToggleSetting>(nameId, descId, defaultValue, OptionType.Toggle);

public enum OptionType
{
    Text,
    Color,
    Number,
    Toggle,
    Slider,
    Dropdown
}