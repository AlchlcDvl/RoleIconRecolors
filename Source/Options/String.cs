namespace FancyUI.Options;

public class StringOption(string id, string defaultValue, Func<IEnumerable<string>> options, Func<string, bool> setActive = null, Dictionary<string, string> mapping = null, Action<string>
    onChanged = null) : DropdownOption<string>(id, defaultValue, OptionType.StringDropdown, options, setActive, mapping, onChanged)
{
    public override void SetString(string value) => Set(value);

    public override void ModifySetting()
    {
        throw new NotImplementedException();
    }
}