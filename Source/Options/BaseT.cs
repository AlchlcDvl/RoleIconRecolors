namespace FancyUI.Options;

public abstract class Option<TValue, TSetting>(string id, TValue defaultValue, OptionType type, PackType page, Func<TValue, bool> setActive = null, Action<TValue> onChanged = null) : Option
    (id, type, page) where TSetting : Setting
{
    public Config<TValue> Entry { get; } = Fancy.Instance.Configs.Bind(id, defaultValue);
    public Func<TValue, bool> SetActive { get; } = setActive ?? (_ => true);
    public Action<TValue> OnChanged { get; } = onChanged ?? (_ => {});
    public TValue DefaultValue { get; } = defaultValue;

    private TSetting _setting;
    public TSetting Setting
    {
        get => _setting;
        set
        {
            _setting = value;
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

    public virtual void OptionCreated()
    {
        Setting.name = ID;
        Setting.TitleText.SetText(SettingsAndTestingUI.Instance.l10n($"FANCY_{ID}_NAME"));
        Setting.Background.EnsureComponent<HoverEffect>().LookupKey = $"FANCY_{ID}_DESC";
        SettingsAndTestingUI.Instance.Settings.Add(Setting);
    }
}