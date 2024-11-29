namespace FancyUI.Options;

public class SliderOption(string id, float defaultValue, float min, float max, bool useWhole = false, Func<float, bool> setActive = null, Action<float> onChanged = null) : Option<float,
    SliderSetting>(id, defaultValue, OptionType.Slider, setActive, onChanged)
{
    public float Min { get; } = min;
    public float Max { get; } = max;
    public bool UseWhole { get; } = useWhole;
}