namespace FancyUI.Settings;

public class SliderSetting : Setting
{
    public Slider Slider { get; set; }
    public TMP_InputField Input { get; set; }
    public FloatOption Option { get; set; }

    public override void Awake()
    {
        base.Awake();
        Slider = transform.GetComponent<Slider>("Slider");
        Input = transform.GetComponent<TMP_InputField>("Input");
        Input.GetComponent<Image>().SetImageColor(ColorType.Metal);
        (Slider.targetGraphic as Image).SetImageColor(ColorType.Metal);
        Slider.transform.GetComponent<Image>("Background").SetImageColor(ColorType.Metal);
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
                Slider.value = value2;
            else if (value2 < Option.Min)
                Slider.value = Option.Min;
            else if (value2 > Option.Max)
                Slider.value = Option.Max;
        }

        SettingsAndTestingUI.Instance.Refresh();
    }

    public override bool SetActive() => Option.SetActive(Option.Value) && Option.Page == SettingsAndTestingUI.Instance.Page;
}