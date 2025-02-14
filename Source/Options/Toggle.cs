namespace FancyUI.Options;

public class ToggleOption(string id, bool defaultValue, PackType page, Func<bool, bool> setActive = null, Action<bool> onChanged = null) : Option<bool, ToggleSetting>(id, defaultValue, page, setActive, onChanged)
{
    public override void Set(string value) => Value = bool.TryParse(value, out var val) && val;
}