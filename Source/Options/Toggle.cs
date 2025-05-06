namespace FancyUI.Options;

public class ToggleOption(string id, bool defaultValue, PackType page, Func<bool> setActive = null, Action<bool> onChanged = null, Action uponChanged = null) : Option<bool, ToggleSetting>(id,
    defaultValue, page, setActive, onChanged, uponChanged)
{
    public override void Set(string value) => Value = bool.TryParse(value, out var val) && val;
}