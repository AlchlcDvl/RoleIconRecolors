namespace FancyUI.Settings;

public class SliderSetting : BaseInputSetting
{
    public Slider Slider { get; set; }
    public FloatOption Option { get; set; }

    public override IInput BoxedOption2
    {
        get => Option;
        set => Option = (FloatOption)value;
    }

    public override void Awake()
    {
        base.Awake();
        Slider = transform.GetComponent<Slider>("Slider");
    }

    public override void Start()
    {
        base.Start();

        if (Option == null)
            return;

        Slider.minValue = Option.Min;
        Slider.maxValue = Option.Max;
        Slider.wholeNumbers = Option.UseWhole;
        Slider.SetValueWithoutNotify(Option.Value);
        Slider.onValueChanged.AddListener(OnValueChanged);
    }

    public void OnValueChanged(float value) => Option.Value = value;
}