namespace FancyUI.Options;

public interface IDropdown
{
    Func<string[]> Options { get; }
    Func<string, string> Mapping { get; }
    PackType Page { get; }
    string ValueString { get; }
    bool UseTranslations { get; }
    DropdownSetting Setting { get; set; }

    void SetString(string value);

    int GetInt() => Options().IndexOf(x => x == ValueString);

    IEnumerable<string> DisplayOptions() => Options().Select(Mapping);
}