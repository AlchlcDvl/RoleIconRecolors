namespace FancyUI.Settings;

public class ToggleSetting : Setting
{
    public Toggle Toggle { get; set; }
    public Image OnBG { get; set; }
    public Image OffBG { get; set; }
    public TextMeshProUGUI ValueText { get; set; }

    public ToggleOption Option { get; set; }

    public override void Awake()
    {
        base.Awake();
        Toggle = transform.GetComponent<Toggle>("Toggle");
        OnBG = Toggle.transform.GetComponent<Image>("OnBG");
        OffBG = Toggle.transform.GetComponent<Image>("OffBG");
        ValueText = Toggle.transform.GetComponent<TextMeshProUGUI>("Text");
    }

    public void Start()
    {
        if (Option == null)
            return;

        Toggle.isOn = Option.Get();
        Toggle.onValueChanged.AddListener(OnValueChanged);
        Toggle.onValueChanged.AddListener(_ => SettingsAndTestingUI.Instance.RefreshOptions());

        ValueText.SetText(Toggle.isOn ? "On" : "Off");

        OnBG.gameObject.SetActive(Toggle.isOn);
        OffBG.gameObject.SetActive(!Toggle.isOn);
        Toggle.targetGraphic = Toggle.isOn ? OnBG : OffBG;
    }

    public void OnValueChanged(bool value)
    {
        Option.Set(value);
        Option.OnChanged(value);
        ValueText.SetText(value ? "On" : "Off");
        OnBG.gameObject.SetActive(value);
        OffBG.gameObject.SetActive(!value);
        Toggle.targetGraphic = value ? OnBG : OffBG;
    }

    public override bool SetActive() => Option.SetActive(Option.Get());
}