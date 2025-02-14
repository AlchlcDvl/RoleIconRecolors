namespace FancyUI.Options;

public class ColorOption(string id, string defaultValue, PackType page, Func<string, bool> setActive = null, Action<string> onChanged = null) : BaseInputOption<ColorSetting>(id, defaultValue, page, "^[a-zA-Z#]+$", setActive, onChanged)
{
    public override void Set(string value) => Value = value;
}