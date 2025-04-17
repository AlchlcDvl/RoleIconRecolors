namespace FancyUI.Settings;

public class ColorSetting : BaseInputSetting
{
    public Image ValueBg { get; set; }
    public ColorOption Option { get; set; }

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
        Input.restoreOriginalTextOnEscape = true;
        Input.onValueChanged.AddListener(OnValueChanged);
        Input.onValueChanged.AddListener(_ => SettingsAndTestingUI.Instance.RefreshOptions());

        ValueBg.color = Option.Value.ToColor();
    }

    public void OnValueChanged(string value)
    {
        if (ColorUtility.TryParseHtmlString(value, out var color))
            ValueBg.color = color;

        Option.Value = value;
    }

    public override bool SetActive() => Option.SetActive(Option.Value) && Option.Page == SettingsAndTestingUI.Instance.Page;
}