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

    public virtual void Start()
    {}
}

public class ToggleSetting : Setting
{
    public ToggleOption Option { get; set; }
}

public abstract class UserInputSetting : Setting;

public class ColorSetting : UserInputSetting
{

}

public class NumberSetting : UserInputSetting
{

}

public class InputSetting : UserInputSetting
{

}