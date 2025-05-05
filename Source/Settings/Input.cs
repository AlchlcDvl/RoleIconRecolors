namespace FancyUI.Settings;

public class StringInputSetting : BaseInputSetting
{
    public StringInputOption Option { get; set; }

    public override Option BoxedOption
    {
        get => Option;
        set => Option = (StringInputOption)value;
    }

    public void Start()
    {
        if (Option == null)
            return;

        Input.SetTextWithoutNotify(Option.Value);
        Input.onValueChanged.AddListener(OnValueChanged);
    }

    public void OnValueChanged(string value)
    {
        Option.Value = value;
        SettingsAndTestingUI.Instance.Refresh();
    }
}