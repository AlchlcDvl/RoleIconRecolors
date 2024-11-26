namespace FancyUI.Options;

public abstract class DropdownOption<T>(string id, T defaultValue, OptionType type, Func<IEnumerable<string>> options, Func<T, bool> setActive = null, Dictionary<string, string> mapping = null,
    Action<T> onChanged = null) : Option<T, DropdownSetting>(id, defaultValue, type, setActive, onChanged), IDropdown
{
    public Func<IEnumerable<string>> Options { get; } = options;
    public Dictionary<string, string> Mapping { get; } = mapping ?? [];

    public abstract void SetString(string value);

    void IDropdown.OnChanged() => OnChanged(Get());

    bool IDropdown.SetActive() => SetActive(Get());
}