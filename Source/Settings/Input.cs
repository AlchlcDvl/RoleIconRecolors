using System.Text.RegularExpressions;

namespace FancyUI.Settings;

public class StringInputSetting : BaseInputSetting
{
    public StringInputOption Option { get; set; }

    public void Start()
    {
        if (Option == null)
            return;

        Input.SetTextWithoutNotify(Option.Get());
        Input.restoreOriginalTextOnEscape = true;
        Input.onValueChanged.AddListener(OnValueChanged);
        Input.onValueChanged.AddListener(_ => SettingsAndTestingUI.Instance.RefreshOptions());
    }

    public void OnValueChanged(string value)
    {
        var cache = value;

        if (Regex.IsMatch(value, Option.Regex))
            value = Option.DefaultValue;

        if (cache != value)
            Input.SetTextWithoutNotify(Option.Get());

        Option.Set(value);
        Option.OnChanged(value);
    }

    public override bool SetActive() => Option.SetActive(Option.Get()) && Option.Page == SettingsAndTestingUI.Instance.Page;
}