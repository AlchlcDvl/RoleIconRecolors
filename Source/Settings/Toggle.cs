namespace FancyUI.Settings;

public class ToggleSetting : Setting
{
    public Toggle Toggle { get; set; }
    public Image Button { get; set; }
    public TextMeshProUGUI ValueText { get; set; }

    public ToggleOption Option { get; set; }

    public override void Awake()
    {
        base.Awake();
        Toggle = transform.GetComponent<Toggle>("Toggle");
        Button = Toggle.transform.GetComponent<Image>("Checkmark");
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
        Button.sprite = Fancy.Assets.GetSprite($"Button{(Toggle.isOn ? "Green" : "Red")}");
    }

    public void OnValueChanged(bool value)
    {
        Option.Set(value);
        Option.OnChanged(value);
        ValueText.SetText(value ? "On" : "Off");
        Button.sprite = Fancy.Assets.GetSprite($"Button{(value ? "Green" : "Red")}");
    }

    public override bool SetActive() => Option.SetActive(Option.Get());
}