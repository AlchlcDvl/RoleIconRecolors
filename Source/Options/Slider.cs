namespace FancyUI.Options;

public class FloatOption(string id, float defaultValue, PackType page, float min, float max, bool useWhole = false, Func<float, bool> setActive = null, Action<float> onChanged = null) :
    Option<float, SliderSetting>(id, defaultValue, OptionType.Slider, page, setActive, onChanged)
{
    public float Min { get; } = min;
    public float Max { get; } = max;
    public bool UseWhole { get; } = useWhole;
}