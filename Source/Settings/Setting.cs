namespace FancyUI.Settings;

public abstract class Setting : UIController
{
    public TextMeshProUGUI TitleText { get; set; }
    public Image Background { get; set; }

    public virtual void Awake()
    {
        TitleText = transform.GetComponent<TextMeshProUGUI>("SettingName");
        Background = transform.GetComponent<Image>("Background");
    }

    public virtual void Refresh() {}

    public abstract bool SetActive();
}