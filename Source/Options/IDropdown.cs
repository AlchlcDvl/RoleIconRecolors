namespace FancyUI.Options;

public interface IDropdown
{
    public Func<IEnumerable<string>> Options { get; }
    public Func<string, string> Mapping { get; }
    public PackType Page { get; }
    public string Value { get; }
    public bool UseTranslations { get; }
    public DropdownSetting Setting { get; set; }

    public void OnChanged();

    public bool SetActive();

    public void SetString(string value);

    public void OptionCreated();

    public int GetInt() => Options().IndexOf(x => x == Value);

    public IEnumerable<string> DisplayOptions() => Options().Select(Mapping);
}