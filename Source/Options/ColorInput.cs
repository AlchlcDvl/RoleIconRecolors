namespace FancyUI.Options;

public sealed class ColorOption(string id, string defaultValue, PackType page, Func<bool> setActive = null, Action<string> onChanged = null, Action uponChanged = null) : BaseInputOption<ColorSetting>
    (id, defaultValue, page, "[#a-fA-F0-9]+$", setActive, onChanged, uponChanged)
{
    protected override void UponValueChanged(string value) => Setting.ValueBg.color = Value.ToColor();

    public override void OptionCreated()
    {
        base.OptionCreated();
        Setting.ValueBg.color = Value.ToColor();
    }
}