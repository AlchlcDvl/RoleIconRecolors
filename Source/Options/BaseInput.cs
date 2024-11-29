namespace FancyUI.Options;

public abstract class BaseInputOption<T>(string id, string defaultValue, OptionType type, string regex = null, Func<string, bool> setActive = null, Action<string> onChanged = null) :
    Option<string, T>(id, defaultValue, type, setActive, onChanged) where T : Setting
{
    public string Regex { get; set; } = regex ?? "";
}