namespace FancyUI.Settings;

public abstract class BaseInputSetting : Setting
{
    public TMP_InputField Input { get; set; }

    public virtual IInput BoxedOption2 { get; set; }

    public override Option BoxedOption
    {
        get => BoxedOption2 as Option;
        set => BoxedOption2 = (IInput)value;
    }

    public override void Awake()
    {
        base.Awake();
        Input = transform.GetComponent<TMP_InputField>("Input");
    }

    public virtual void Start()
    {
        if (BoxedOption2 == null)
            return;

        Input.SetTextWithoutNotify(BoxedOption2.ValueString);
        Input.onDeselect.AddListener(OnValueChanged);
    }

    public void OnValueChanged(string value) => BoxedOption2.ValueString = value;
}