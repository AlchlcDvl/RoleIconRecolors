namespace FancyUI.Options;

public sealed class FloatOption(string id, float defaultValue, PackType page, float min, float max, bool useWhole = false, Func<bool> setActive = null, Action<float> onChanged = null, Action uponChanged
    = null) : Option<float, SliderSetting>(id, defaultValue, page, setActive, onChanged, uponChanged), IInput
{
    public float Min { get; } = min;
    public float Max { get; } = max;
    public bool UseWhole { get; } = useWhole;

    public string ValueString
    {
        get => $"{Value:0.##}";
        set
        {
            if (!float.TryParse(value, out var value2))
                return;

            value2 = Mathf.Clamp(value2, Min, Max);
            Setting.Slider.SetValueWithoutNotify(value2);
            Value = value2;
        }
    }

    protected override void UponValueChanged(float value) => Setting.Input.SetTextWithoutNotify($"{value:0.##}");
}