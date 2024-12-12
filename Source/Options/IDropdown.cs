namespace FancyUI.Options;

public interface IDropdown
{
    Func<IEnumerable<string>> Options { get; }
    Func<string, string> Mapping { get; }
    PackType Page { get; }
    string ValueString { get; }
    bool UseTranslations { get; }
    DropdownSetting Setting { get; set; }

    bool SetActive();

    void SetString(string value);

    void OptionCreated();

    int GetInt() => Options().IndexOf(x => x == ValueString);

    IEnumerable<string> DisplayOptions() => Options().Select(Mapping);
}