namespace FancyUI.Options;

public class EnumDropdownOption<T> : BaseDropdownOption<T> where T : struct, Enum
{
    public EnumDropdownOption(string id, T defaultValue, PackType page, Func<IEnumerable<string>> options = null, Func<T, bool> setActive = null, Func<string, string> mapping = null, Action<T>
        onChanged = null, bool useTranslations = false, Func<T, string> enumMapping = null) : base(id, defaultValue, OptionType.EnumDropdown, page, options ?? (() => Enum.GetNames(typeof(T))),
        setActive, mapping, onChanged, useTranslations)
    {
        if (mapping == null && enumMapping != null)
            mapping = x => enumMapping(Enum.Parse<T>(x));

        Mapping = mapping ?? (x => x);
    }

    public override void SetString(string value) => Set(Enum.Parse<T>(value));
}