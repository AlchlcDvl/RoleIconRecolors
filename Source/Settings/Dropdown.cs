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

    public override void Start()
    {
        base.Start();

        Dropdown.AddOptions(Option.DisplayOptions());
        Dropdown.value = Option.GetInt();
        Dropdown.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(int index)
    {
        var value = Option.Options().ElementAtOrDefault(index);
        Option.SetString(value);
        gameObject.SetActive(Option.SetActive());
    }
}