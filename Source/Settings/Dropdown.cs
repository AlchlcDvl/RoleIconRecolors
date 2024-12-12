namespace FancyUI.Settings;

public class DropdownSetting : Setting
{
    public TMP_Dropdown Dropdown { get; set; }
    public IDropdown Option { get; set; }

    public override void Awake()
    {
        base.Awake();
        Dropdown = transform.GetComponent<TMP_Dropdown>("Dropdown");
    }

    public void Start()
    {
        if (Option == null)
            return;

        Dropdown.ClearOptions();
        Dropdown.AddOptions(Option.DisplayOptions().Select(x => Option.UseTranslations ? l10n($"FANCY_{x.ToUpper()}") : x).ToList());
        Dropdown.SetValueWithoutNotify(Option.GetInt());
        Dropdown.onValueChanged.AddListener(OnValueChanged);
        Dropdown.onValueChanged.AddListener(_ => SettingsAndTestingUI.Instance.RefreshOptions());
    }

    public void OnValueChanged(int index)
    {
        var options = Option.Options();
        Option.SetString(options.ElementAtOrDefault(index) ?? options.FirstOrDefault() ?? "Error");
    }

    public override bool SetActive() => Option.SetActive() && Option.Page == SettingsAndTestingUI.Instance.Page;
}