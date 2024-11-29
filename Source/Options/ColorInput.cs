namespace FancyUI.Options;

public class ColorOption(string id, string defaultValue, Func<string, bool> setActive = null, Action<string> onChanged = null) : BaseInputOption<ColorSetting>(id, defaultValue,
    OptionType.ColorInput, "^[a-zA-Z]+$", setActive, onChanged);