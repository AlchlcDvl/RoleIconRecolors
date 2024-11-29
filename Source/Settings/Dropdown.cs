namespace FancyUI.Settings;

public class DropdownSetting : Setting
{
    public Dropdown Dropdown { get; set; }
    public IDropdown Option { get; set; }

    public override void Awake()
    {
        base.Awake();
        Dropdown = transform.GetComponent<Dropdown>("Dropdown");
    }


    public void Start()
    {
        if (Option == null)
            return;

        Option.OptionCreated();

        Dropdown.AddOptions(Option.DisplayOptions().ToList());
        Dropdown.value = Option.GetInt();
        Dropdown.onValueChanged.AddListener(OnValueChanged);
    }

    public void OnValueChanged(int index)
    {
        var options = Option.Options();
        Option.SetString(options.ElementAtOrDefault(index) ?? options.FirstOrDefault() ?? "Error");
        Option.OnChanged();
        gameObject.SetActive(Option.SetActive());
    }
}