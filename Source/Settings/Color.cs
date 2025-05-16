namespace FancyUI.Settings;

public class ColorSetting : BaseInputSetting
{
    public Image ValueBg { get; set; }

    public override void Awake()
    {
        base.Awake();
        ValueBg = Input.transform.GetComponent<Image>();
    }
}
