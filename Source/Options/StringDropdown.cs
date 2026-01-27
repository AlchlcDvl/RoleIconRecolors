namespace FancyUI.Options;

public sealed class StringDropdownOption(string id, string defaultValue, PackType page, Func<string[]> options, Func<bool> setActive = null, Func<string, string> mapping = null, Action<string> onChanged
    = null, bool useTranslations = false, Action uponChanged = null) : BaseDropdownOption<string>(id, defaultValue, page, options, setActive, mapping, onChanged, useTranslations, uponChanged)
{
    public override void SetString(string value) => Value = value;
}