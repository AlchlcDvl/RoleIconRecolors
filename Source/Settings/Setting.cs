namespace FancyUI.Settings;

public abstract class Setting : MonoBehaviour
{
    public TextMeshProUGUI TitleText { get; set; }
    public Transform Background { get; set; }

    public virtual void Awake()
    {
        TitleText = transform.GetComponent<TextMeshProUGUI>("SettingName");
        Background = transform.Find("Background");
    }
}