namespace FancyUI.Options;

public class ToggleOption(string id, bool defaultValue, PackType page, Func<bool> setActive = null, Action<bool> onChanged = null, Action uponChanged = null) : Option<bool, ToggleSetting>(id,
    defaultValue, page, setActive, onChanged, uponChanged)
{
    protected override void UponValueChanged(bool value)
    {
        Setting.ValueText.SetText(value ? "On" : "Off");
        Setting.OnBg.gameObject.SetActive(value);
        Setting.OffBg.gameObject.SetActive(!value);
        Setting.Toggle.targetGraphic = value ? Setting.OnBg : Setting.OffBg;
    }
}