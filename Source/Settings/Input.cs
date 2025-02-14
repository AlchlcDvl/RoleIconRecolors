using System.Text.RegularExpressions;

namespace FancyUI.Settings;

public class StringInputSetting : BaseInputSetting
{
    public StringInputOption Option { get; set; }

    public void Start()
    {
        if (Option == null)
            return;

        Input.SetTextWithoutNotify(Option.Value);
        Input.restoreOriginalTextOnEscape = true;
        Input.onValueChanged.AddListener(OnValueChanged);
        Input.onValueChanged.AddListener(_ => SettingsAndTestingUI.Instance.RefreshOptions());
    }

    public void OnValueChanged(string value)
    {
        var cache = value;

        if (Regex.IsMatch(value, Option.Regex))
            value = Regex.Replace(value, Option.Regex, "");

        if (cache != value)
            Input.SetTextWithoutNotify(Option.Value);

        Option.Value = value;
    }

    public override bool SetActive() => Option.SetActive(Option.Value) && Option.Page == SettingsAndTestingUI.Instance.Page;
}