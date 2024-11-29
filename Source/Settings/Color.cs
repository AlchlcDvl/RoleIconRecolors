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

        Input.text = Option.Get();
        Input.onValueChanged.AddListener(OnValueChanged);
        Input.restoreOriginalTextOnEscape = true;
    }

    public void OnValueChanged(string value)
    {
        var cache = value;

        if (!value.StartsWith("#"))
            value = "#" + value;

        if (!ColorUtility.TryParseHtmlString(value, out var color) || Regex.IsMatch(value.Replace("#", ""), Option.Regex))
        {
            value = Option.DefaultValue;
            ColorUtility.TryParseHtmlString(value, out color);
        }

        if (cache != value)
            Input.SetTextWithoutNotify(Option.Get());

        ValueBG.color = color;
        Option.Set(value);
        Option.OnChanged(value);
        gameObject.SetActive(Option.SetActive(value));
    }
}