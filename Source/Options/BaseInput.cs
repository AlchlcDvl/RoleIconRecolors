namespace FancyUI.Options;

public abstract class BaseInputOption<T>(string id, string defaultValue, PackType page, string regex = null, Func<bool> setActive = null, Action<string> onChanged = null, Action uponChanged =
    null) : Option<string, T>(id, defaultValue, page, setActive, onChanged, uponChanged), IInput where T : Setting
{
    public string Regex { get; } = regex ?? "";

    public virtual string ValueString
    {
        get => Value;
        set => Value = value;
    }
}