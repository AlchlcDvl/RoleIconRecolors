namespace FancyUI.Options;

public class ColorOption(string id, string defaultValue, PackType page, Func<bool> setActive = null, Action<string> onChanged = null, Action uponChanged = null) : BaseInputOption<ColorSetting>
    (id, defaultValue, page, "[#a-fA-F0-9]+$", setActive, onChanged, uponChanged);