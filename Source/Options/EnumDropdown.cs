namespace FancyUI.Options;

public class EnumDropdownOption<T>(string id, T defaultValue, Func<IEnumerable<string>> options = null, Func<T, bool> setActive = null, Func<string, string> mapping = null, Action<T> onChanged
    = null) : BaseDropdownOption<T>(id, defaultValue, OptionType.EnumDropdown, options ?? (() => Enum.GetNames(typeof(T))), setActive, mapping, onChanged) where T : struct, Enum
{
    public override void SetString(string value) => Set(Enum.Parse<T>(value));
}