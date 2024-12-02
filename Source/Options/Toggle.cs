namespace FancyUI.Options;

public class ToggleOption(string id, bool defaultValue, PackType page, Func<bool, bool> setActive = null, Action<bool> onChanged = null) : Option<bool, ToggleSetting>(id, defaultValue,
    OptionType.Toggle, page, setActive, onChanged);