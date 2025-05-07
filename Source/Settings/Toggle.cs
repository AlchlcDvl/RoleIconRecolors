namespace FancyUI.Settings;

public class ToggleSetting : Setting
{
    public Toggle Toggle { get; set; }
    public Image OnBg { get; set; }
    public Image OffBg { get; set; }
    public TextMeshProUGUI ValueText { get; set; }
    public ToggleOption Option { get; set; }

    public override Option BoxedOption
    {
        get => Option;
        set => Option = (ToggleOption)value;
    }

    public override void Awake()
    {
        base.Awake();
        Toggle = transform.GetComponent<Toggle>("Toggle")!;
        OnBg = Toggle.transform.GetComponent<Image>("OnBG");
        OffBg = Toggle.transform.GetComponent<Image>("OffBG");
        ValueText = Toggle.transform.GetComponent<TextMeshProUGUI>("Text");
    }

    public void Start()
    {
        if (Option == null)
            return;

        Toggle.SetIsOnWithoutNotify(Option.Value);
        ValueText.SetText(Option.Value ? "On" : "Off");
        OnBg.gameObject.SetActive(Option.Value);
        OffBg.gameObject.SetActive(!Option.Value);
        Toggle.targetGraphic = Option.Value ? OnBg : OffBg;
        Toggle.onValueChanged.AddListener(OnValueChanged);
    }

    public void OnValueChanged(bool value) => Option.Value = value;
}