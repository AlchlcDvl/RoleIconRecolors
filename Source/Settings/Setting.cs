namespace FancyUI.Settings;

public abstract class Setting : MonoBehaviour
{
    public TextMeshProUGUI TitleText { get; set; }
    public GameObject Background { get; set; }

    public virtual void Awake()
    {
        TitleText = transform.Find("SettingName").GetComponent<TextMeshProUGUI>();
        Background = transform.Find("Background").gameObject;
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

public class SliderSetting : Setting
{

}

public abstract class InputSetting : Setting;

public class ColorSetting : InputSetting
{

}

public class NumberSetting : InputSetting
{

}