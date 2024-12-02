namespace FancyUI.Options;

public class ColorOption(string id, string defaultValue, PackType page, Func<string, bool> setActive = null, Action<string> onChanged = null) : BaseInputOption<ColorSetting>(id, defaultValue,
    OptionType.ColorInput, page, "^[a-zA-Z#]+$", setActive, onChanged);