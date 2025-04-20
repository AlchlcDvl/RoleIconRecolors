namespace FancyUI.Settings;

public class ToggleSetting : Setting
{
    public Toggle Toggle { get; set; }
    public Image OnBg { get; set; }
    public Image OffBg { get; set; }
    public TextMeshProUGUI ValueText { get; set; }

    public ToggleOption Option { get; set; }

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
        Toggle.onValueChanged.AddListener(OnValueChanged);

        ValueText.SetText(Toggle.isOn ? "On" : "Off");

        OnBg.gameObject.SetActive(Toggle.isOn);
        OffBg.gameObject.SetActive(!Toggle.isOn);
        Toggle.targetGraphic = Toggle.isOn ? OnBg : OffBg;
    }

    public void OnValueChanged(bool value)
    {
        Option.Value = value;
        ValueText.SetText(value ? "On" : "Off");
        OnBg.gameObject.SetActive(value);
        OffBg.gameObject.SetActive(!value);
        Toggle.targetGraphic = value ? OnBg : OffBg;
        SettingsAndTestingUI.Instance.Refresh();
    }

    public override bool SetActive() => Option.SetActive(Option.Value) && Option.Page == SettingsAndTestingUI.Instance.Page;
}