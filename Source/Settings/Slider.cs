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
        Slider.onValueChanged.AddListener(_ => SettingsAndTestingUI.Instance.RefreshOptions());

        Input.SetTextWithoutNotify($"{Slider.value:0.##}");
        Input.onValueChanged.AddListener(OnTextValueChanged);
        Input.onValueChanged.AddListener(_ => SettingsAndTestingUI.Instance.RefreshOptions());
    }

    public void OnValueChanged(float value)
    {
        Option.Value = value;
        Input.SetTextWithoutNotify($"{value:0.##}");
    }

    public void OnTextValueChanged(string value)
    {
        if (float.TryParse(value, out var value2) && value2.IsInRange(Option.Min, Option.Max, true, true))
            Slider.value = value2;
    }

    public override bool SetActive() => Option.SetActive(Option.Value) && Option.Page == SettingsAndTestingUI.Instance.Page;
}