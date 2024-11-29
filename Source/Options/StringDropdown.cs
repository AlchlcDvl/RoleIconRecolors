namespace FancyUI.Options;

public class StringDropdownOption(string id, string defaultValue, Func<IEnumerable<string>> options, Func<string, bool> setActive = null, Func<string, string> mapping = null, Action<string>
    onChanged = null) : BaseDropdownOption<string>(id, defaultValue, OptionType.StringDropdown, options, setActive, mapping, onChanged)
{
    public override void SetString(string value) => Set(value);
}