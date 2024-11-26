namespace FancyUI.Settings;

public class SliderSetting : Setting
{
    public Slider Slider { get; set; }
    public TextMeshProUGUI ValueText { get; set; }

    public SliderOption Option { get; set; }

    public override void Awake()
    {
        base.Awake();

        Slider = transform.GetComponent<Slider>("Slider");
        ValueText = Slider.transform.GetComponent<TextMeshProUGUI>("ValueText");
    }

    public override void Start()
    {
        base.Start();

        Slider.minValue = Option.Min;
        Slider.maxValue = Option.Max;
        Slider.wholeNumbers = Option.UseWhole;
        Slider.value = Option.Get();
        Slider.onValueChanged.AddListener(OnValueChanged);
    }

    public void OnValueChanged(float value)
    {
        Option.Set(value);
        ValueText.text = $"{value:0.##}";
        gameObject.SetActive(Option.SetActive(value));
    }
}