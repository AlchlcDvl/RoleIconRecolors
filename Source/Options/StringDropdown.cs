namespace FancyUI.Options;

public class StringDropdownOption(string id, string defaultValue, PackType page, Func<IEnumerable<string>> options, Func<string, bool> setActive = null, Func<string, string> mapping = null,
    Action<string> onChanged = null, bool useTranslations = false) : BaseDropdownOption<string>(id, defaultValue, page, options, setActive, mapping, onChanged,
    useTranslations)
{
    public override void SetString(string value) => Value = value;
}