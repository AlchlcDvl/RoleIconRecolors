namespace FancyUI.Settings;

public class ColorSetting : BaseInputSetting
{
    public Image ValueBg { get; set; }
    public ColorOption Option { get; set; }

    public override Option BoxedOption
    {
        get => Option;
        set => Option = (ColorOption)value;
    }

    public override void Awake()
    {
        base.Awake();
        ValueBg = Input.transform.GetComponent<Image>();
    }

    public void Start()
    {
        if (Option == null)
            return;

        Input.SetTextWithoutNotify(Option.Value);
        Input.onValueChanged.AddListener(OnValueChanged);
    }

    public void OnValueChanged(string value)
    {
        Option.Value = value;
        SettingsAndTestingUI.Instance.Refresh();
    }

    public override void Refresh()
    {
        if (Option != null)
            ValueBg.color = Option.Value.ToColor();
    }
}