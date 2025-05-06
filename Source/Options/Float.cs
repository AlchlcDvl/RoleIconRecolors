namespace FancyUI.Options;

public class FloatOption(string id, float defaultValue, PackType page, float min, float max, bool useWhole = false, Func<bool> setActive = null, Action<float> onChanged = null, Action uponChanged
    = null) : Option<float, SliderSetting>(id, defaultValue, page, setActive, onChanged, uponChanged)
{
    public float Min { get; } = min;
    public float Max { get; } = max;
    public bool UseWhole { get; } = useWhole;

    public override void Set(string value) => Value = float.TryParse(value, out var val2) ? val2 : Value;
}