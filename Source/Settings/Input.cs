using System.Text.RegularExpressions;

namespace FancyUI.Settings;

public class StringInputSetting : BaseInputSetting
{
    public StringInputOption Option { get; set; }

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

        if (Regex.IsMatch(value.Replace("#", ""), Option.Regex))
            value = Option.DefaultValue;

        if (cache != value)
            Input.SetTextWithoutNotify(Option.Get());

        Option.Set(value);
        Option.OnChanged(value);
        gameObject.SetActive(Option.SetActive(value));
    }
}