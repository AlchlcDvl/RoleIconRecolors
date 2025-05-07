using NewModLoading;

namespace FancyUI.Options;

public abstract class Option<TValue, TSetting>(string id, TValue defaultValue, PackType page, Func<bool> setActive = null, Action<TValue> onChanged = null, Action uponChanged = null) : Option(id,
    page, setActive, uponChanged) where TSetting : Setting
{
    protected Config<TValue> Entry { get; } = Fancy.Instance.Configs!.Bind(id, defaultValue);
    private Action<TValue> OnChanged { get; } = onChanged ?? BlankVoid;

    private TSetting setting;
    public TSetting Setting
    {
        get => setting;
        set
        {
            setting = value;
            OptionCreated();
        }
    }

    public TValue Value
    {
        get => Entry.Value;
        set
        {
            Entry.Value = value;
            OnChanged(value);
            UponChanged();
            UponValueChanged(value);
            SettingsAndTestingUI.Instance?.Refresh();
        }
    }

    public override Setting BoxedSetting
    {
        get => Setting;
        set => Setting = (TSetting)value;
    }

    public override object BoxedValue
    {
        get => Value;
        set => Value = (TValue)value;
    }

    public virtual void OptionCreated()
    {
        Setting.name = ID;
        Setting.TitleText.SetText(SettingsAndTestingUI.Instance.l10n($"FANCY_{ID}_NAME"));
        Setting.Background.EnsureComponent<HoverEffect>()!.LookupKey = $"FANCY_{ID}_DESC";
        Setting.BoxedOption = this;
    }

    protected virtual void UponValueChanged(TValue value) { }

    private static void BlankVoid(TValue _) { }

    public static implicit operator TValue(Option<TValue, TSetting> option) => option.Value;

    public static implicit operator TSetting(Option<TValue, TSetting> option) => option.setting;
}