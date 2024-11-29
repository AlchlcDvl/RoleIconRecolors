namespace FancyUI.Options;

public abstract class BaseDropdownOption<T>(string id, T defaultValue, OptionType type, Func<IEnumerable<string>> options, Func<T, bool> setActive = null, Func<string, string> mapping = null,
    Action<T> onChanged = null) : Option<T, DropdownSetting>(id, defaultValue, type, setActive, onChanged), IDropdown
{
    public Func<IEnumerable<string>> Options { get; } = options;
    public Func<string, string> Mapping { get; } = mapping ?? (x => x);

    public abstract void SetString(string value);

    void IDropdown.OnChanged() => OnChanged(Get());

    bool IDropdown.SetActive() => SetActive(Get());
}