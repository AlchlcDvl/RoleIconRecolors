namespace FancyUI.Options;

public abstract class BaseDropdownOption<T>(string id, T defaultValue, PackType page, Func<string[]> options, Func<bool> setActive = null, Func<string, string> mapping = null, Action<T> onChanged
    = null, bool useTranslations = false, Action uponChanged = null) : Option<T, DropdownSetting>(id, defaultValue, page, setActive, onChanged, uponChanged), IDropdown
{
    public Func<string[]> Options { get; } = options;
    public Func<string, string> Mapping { get; protected set; } = mapping ?? (x => x);
    public bool UseTranslations { get; } = useTranslations;

    public string ValueString => Entry.Value.ToString();

    public abstract void SetString(string value);

    public override void Set(string value) => SetString(value);
}