namespace FancyUI.Settings;

public class DropdownSetting : Setting
{
    public TMP_Dropdown Dropdown { get; set; }
    public IDropdown Option { get; set; }
    public Image Arrow { get; set; }

    public override Option BoxedOption
    {
        get => Option as Option;
        set => Option = (IDropdown)value;
    }

    public override void Awake()
    {
        base.Awake();
        Dropdown = transform.GetComponent<TMP_Dropdown>("Dropdown")!;
        Arrow = Dropdown.transform.GetComponent<Image>("Arrow");
    }

    public void Start()
    {
        if (Option != null)
            Dropdown.onValueChanged.AddListener(OnValueChanged);
    }

    public void OnValueChanged(int index) => Option.SetString(Option.Options()[index]);

    public override void Refresh()
    {
        Arrow.sprite = Fancy.Instance.Assets.GetSprite("DropDown_ArrowDown");

        if (Option == null)
            return;

        Dropdown.ClearOptions();
        Dropdown.AddOptions([..Option.DisplayOptions().Select(x => Option.UseTranslations ? l10n($"FANCY_{x.ToUpper()}") : x)]);
        Dropdown.SetValueWithoutNotify(Option.GetInt());
    }
}