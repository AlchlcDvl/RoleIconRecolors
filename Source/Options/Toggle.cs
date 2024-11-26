namespace FancyUI.Options;

public class ToggleOption(string id, bool defaultValue, Func<bool, bool> setActive = null, Action<bool> onChanged = null) : Option<bool, ToggleSetting>(id, defaultValue, OptionType.Toggle,
    setActive, onChanged)
{
}