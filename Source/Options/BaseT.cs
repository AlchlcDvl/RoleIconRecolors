using NewModLoading;

namespace FancyUI.Options;

public abstract class Option<TValue, TSetting>(string id, TValue defaultValue, PackType page, Func<bool> setActive = null, Action<TValue> onChanged = null) : Option(id, page, setActive) where
    TSetting : Setting
{
    protected Config<TValue> Entry { get; } = Fancy.Instance.Configs!.Bind(id, defaultValue);
    private Action<TValue> OnChanged { get; } = onChanged ?? (_ => {});

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
        }
    }

    public override Setting BoxedSetting
    {
        get => Setting;
        set => Setting = (TSetting)value;
    }

    public virtual void OptionCreated()
    {
        Setting.name = ID;
        Setting.TitleText.SetText(SettingsAndTestingUI.Instance.l10n($"FANCY_{ID}_NAME"));
        Setting.Background.EnsureComponent<HoverEffect>()!.LookupKey = $"FANCY_{ID}_DESC";
        Setting.BoxedOption = this;
    }
}