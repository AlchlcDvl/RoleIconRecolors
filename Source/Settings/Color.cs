using System.Text.RegularExpressions;

namespace FancyUI.Settings;

public class ColorSetting : BaseInputSetting
{
    public Image ValueBG { get; set; }
    public ColorOption Option { get; set; }

    public override void Awake()
    {
        base.Awake();
        ValueBG = Input.transform.GetComponent<Image>();
    }

    public void Start()
    {
        if (Option == null)
            return;

        Input.SetTextWithoutNotify(Option.Get());
        Input.restoreOriginalTextOnEscape = true;
        Input.onValueChanged.AddListener(OnValueChanged);
        Input.onValueChanged.AddListener(_ => SettingsAndTestingUI.Instance.RefreshOptions());

        ValueBG.color = Option.Get().ToColor();
    }

    public void OnValueChanged(string value)
    {
        var cache = value;

        if (!value.StartsWith("#"))
            value = "#" + value;

        if (!ColorUtility.TryParseHtmlString(value, out var color) || Regex.IsMatch(value, Option.Regex))
        {
            value = Option.DefaultValue;
            color = value.ToColor();
        }

        if (cache != value)
            Input.SetTextWithoutNotify(Option.Get());

        ValueBG.color = color;
        Option.Set(value);
        Option.OnChanged(value);
    }

    public override bool SetActive() => Option.SetActive(Option.Get());
}