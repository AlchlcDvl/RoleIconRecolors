namespace FancyUI.Options;

public abstract class BaseInputOption<T>(string id, string defaultValue, OptionType type, PackType page, string regex = null, Func<string, bool> setActive = null, Action<string> onChanged =
    null) : Option<string, T>(id, defaultValue, type, page, setActive, onChanged) where T : Setting
{
    public string Regex { get; set; } = regex ?? "";
}