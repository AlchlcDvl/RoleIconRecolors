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

        Option.OptionCreated();

        Toggle.isOn = Option.Get();
        Toggle.onValueChanged.AddListener(OnValueChanged);

        ValueText.SetText(Toggle.isOn ? "On" : "Off");
    }

    public void OnValueChanged(bool value)
    {
        Option.Set(value);
        Option.OnChanged(value);
        gameObject.SetActive(Option.SetActive(value));
        ValueText.SetText(value ? "On" : "Off");
        Button.sprite = Fancy.Assets.GetSprite($"Button{(value ? "Green" : "Red")}");
    }
}