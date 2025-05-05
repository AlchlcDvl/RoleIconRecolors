namespace FancyUI.Settings;

public class SliderSetting : BaseInputSetting
{
    public Slider Slider { get; set; }
    public FloatOption Option { get; set; }

    public override Option BoxedOption
    {
        get => Option;
        set => Option = (FloatOption)value;
    }

    public override void Awake()
    {
        base.Awake();
        Slider = transform.GetComponent<Slider>("Slider");
    }

    public void Start()
    {
        if (Option == null)
            return;

        Slider.minValue = Option.Min;
        Slider.maxValue = Option.Max;
        Slider.wholeNumbers = Option.UseWhole;
        Slider.SetValueWithoutNotify(Option.Value);
        Slider.onValueChanged.AddListener(OnValueChanged);

        Input.SetTextWithoutNotify($"{Slider.value:0.##}");
        Input.onValueChanged.AddListener(OnValueChanged);
    }

    public void OnValueChanged(float value)
    {
        Option.Value = value;
        Input.SetTextWithoutNotify($"{value:0.##}");
        SettingsAndTestingUI.Instance.Refresh();
    }

    public void OnValueChanged(string value)
    {
        if (float.TryParse(value, out var value2))
        {
            value2 = Mathf.Clamp(value2, Option.Min, Option.Max);
            Slider.SetValueWithoutNotify(value2);
            Option.Value = value2;
        }

        SettingsAndTestingUI.Instance.Refresh();
    }
}