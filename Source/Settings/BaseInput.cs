namespace FancyUI.Settings;

public abstract class BaseInputSetting : Setting
{
    public TMP_InputField Input { get; set; }

    public override void Awake()
    {
        base.Awake();
        Input = transform.GetComponent<TMP_InputField>("Input");
    }
}