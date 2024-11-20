namespace FancyUI.Settings;

public abstract class Setting : MonoBehaviour
{
    public TextMeshProUGUI TitleText { get; set; }

    public virtual void Awake()
    {
        TitleText = transform.Find("SettingName").GetComponent<TextMeshProUGUI>();
    }
}

public class ToggleSetting : Setting
{
    public ToggleOption Option { get; set; }
}

public class DropdownSetting : Setting
{
    public IDropdown Option { get; set; }
}