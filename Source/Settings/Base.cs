namespace FancyUI.Settings;

public class Option
{
    public static readonly List<Option> All = [];
    public string NameID { get; }
    public string DescID { get; }
    public string Value { get; set; }
    public OptionType Type { get; }

    public Option(string nameId, string descId, OptionType type)
    {
        NameID = nameId;
        DescID = descId;
        Type = type;
        All.Add(this);
    }

    public void SetBase(string value)
    {
        if (this is ToggleOption toggle)
            toggle.Set(bool.Parse(value));
    }
}

public class Option<TValue, TSetting>(string nameId, string descId, TValue defaultValue, OptionType type, Func<TValue, bool> setActive = null) : Option(nameId, descId, type) where TSetting :
    Setting
{
    public Config<TValue> Entry { get; } = Fancy.Instance.Configs.Bind(nameId, descId, defaultValue);
    public Func<TValue, bool> SetActive { get; } = setActive ?? (_ => true);
    public TSetting Setting { get; set; }

    public TValue Get() => Entry.Value;

    public void Set(TValue value)
    {
        Entry.Value = value;

        if (!Setting)
            return;

        Setting.gameObject.SetActive(SetActive(value));
    }
}

public class ToggleOption(string nameId, string descId, bool defaultValue, Func<bool, bool> setActive = null) : Option<bool, ToggleSetting>(nameId, descId, defaultValue, OptionType.Toggle,
    setActive);

public class DropdownOption<T>(string nameId, string descId, T defaultValue, OptionType type, Func<string[]> options, Func<T, bool> setActive = null, Dictionary<string, string> mapping = null)
    : Option<T, DropdownSetting>(nameId, descId, defaultValue, type, setActive), IDropdown
{
    public Func<string[]> Options { get; } = options;
    public Dictionary<string, string> Mapping { get; } = mapping ?? [];
}

public interface IDropdown
{
    Func<string[]> Options { get; }
    Dictionary<string, string> Mapping { get; }

    List<string> DisplayOptions()
    {
        var result = new List<string>();

        foreach (var option in Options())
        {
            if (Mapping.TryGetValue(option, out var display))
                result.Add(display);
            else
                result.Add(option);
        }

        return result;
    }
}

public enum OptionType
{
    Color,
    Number,
    Toggle,
    Slider,
    Dropdown
}