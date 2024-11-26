namespace FancyUI.Options;

public class ColorOption(string id, string defaultValue, Func<string, bool> setActive = null, Action<string> onChanged = null) : Option<string, ColorSetting>(id, defaultValue, OptionType.Color,
    setActive, onChanged)
{
}