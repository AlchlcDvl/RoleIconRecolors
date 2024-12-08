namespace FancyUI.Options;

public abstract class BaseDropdownOption<T>(string id, T defaultValue, OptionType type, PackType page, Func<IEnumerable<string>> options, Func<T, bool> setActive = null, Func<string, string>
    mapping = null, Action<T> onChanged = null, bool useTranslations = false) : Option<T, DropdownSetting>(id, defaultValue, type, page, setActive, onChanged), IDropdown
{
    public Func<IEnumerable<string>> Options { get; } = options;
    public Func<string, string> Mapping { get; set; } = mapping ?? (x => x);
    public bool UseTranslations { get; } = useTranslations;

    public string Value => Entry.Value.ToString();

    public abstract void SetString(string value);

    void IDropdown.OnChanged() => OnChanged(Get());

    bool IDropdown.SetActive() => SetActive(Get());
}