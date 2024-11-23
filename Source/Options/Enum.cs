namespace FancyUI.Options;

public class EnumOption<T>(string id, T defaultValue, Func<T, bool> setActive = null, Dictionary<string, string> mapping = null, Action<T> onChanged = null) : DropdownOption<T>(id,
    defaultValue, OptionType.EnumDropdown, () => Enum.GetNames(typeof(T)), setActive, mapping, onChanged) where T : struct, Enum
{
    public override void SetString(string value) => Set(Enum.Parse<T>(value));

    public override void ModifySetting()
    {
        throw new NotImplementedException();
    }
}