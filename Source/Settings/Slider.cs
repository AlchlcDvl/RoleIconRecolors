namespace FancyUI.Settings;

public class SliderSetting : Setting
{
    public Slider Slider { get; set; }
    public TMP_InputField Input { get; set; }
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
        Input = transform.GetComponent<TMP_InputField>("Input");
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
        Input.restoreOriginalTextOnEscape = true;
        Input.onValueChanged.AddListener(OnTextValueChanged);
    }

    public void OnValueChanged(float value)
    {
        Option.Value = value;
        Input.SetTextWithoutNotify($"{value:0.##}");
        SettingsAndTestingUI.Instance.Refresh();
    }

    public void OnTextValueChanged(string value)
    {
        if (float.TryParse(value, out var value2))
        {
            if (value2.IsInRange(Option.Min, Option.Max, true, true))
                Slider.SetValueWithoutNotify(value2);
            else if (value2 < Option.Min)
                Slider.SetValueWithoutNotify(value2);
            else if (value2 > Option.Max)
                Slider.SetValueWithoutNotify(value2);
        }

        SettingsAndTestingUI.Instance.Refresh();
    }

    public override bool SetActive() => Option.SetActive(Option.Value) && Option.Page == SettingsAndTestingUI.Instance.Page;
}