namespace FancyUI.Settings;

public class DropdownSetting : Setting
{
    public TMP_Dropdown Dropdown { get; set; }
    public IDropdown Option { get; set; }

    public override void Awake()
    {
        base.Awake();
        Dropdown = transform.GetComponent<TMP_Dropdown>("Dropdown");
        transform.FindRecursive("Template").Find("Background").GetComponent<Image>().SetImageColor(ColorType.Metal);
        Dropdown.GetComponent<Image>().SetImageColor(ColorType.Metal);
        Dropdown.transform.GetComponent<Image>("Arrow").SetImageColor(ColorType.Metal);
    }

    public void Start()
    {
        if (Option != null)
            Dropdown.onValueChanged.AddListener(OnValueChanged);
    }

    public void OnValueChanged(int index)
    {
        var options = Option.Options();
        Option.SetString(options[index] ?? options.FirstOrDefault() ?? "Error");
        SettingsAndTestingUI.Instance.Refresh();
    }

    public override bool SetActive() => Option.SetActive() && Option.Page == SettingsAndTestingUI.Instance.Page;

    public override void Refresh()
    {
        base.Refresh();

        if (Option == null)
            return;

        Dropdown.ClearOptions();
        Dropdown.AddOptions(Option.DisplayOptions().Select(x => Option.UseTranslations ? l10n($"FANCY_{x.ToUpper()}") : x).ToList());
        Dropdown.SetValueWithoutNotify(Option.GetInt());
    }
}