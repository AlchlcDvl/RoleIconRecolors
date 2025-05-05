namespace FancyUI.Options;

public abstract class BaseInputOption<T>(string id, string defaultValue, PackType page, string regex = null, Func<bool> setActive = null, Action<string> onChanged = null) : Option<string, T>(id,
    defaultValue, page, setActive, onChanged) where T : Setting
{
    public string Regex { get; } = regex ?? "";

    public override void Set(string value) => Value = value;
}