namespace FancyUI.Options;

public class EnumDropdownOption<T> : BaseDropdownOption<T> where T : struct, Enum
{
    public EnumDropdownOption(string id, T defaultValue, PackType page, Func<string[]> options = null, Func<T, bool> setActive = null, Func<string, string> mapping = null, Action<T>
        onChanged = null, bool useTranslations = false, Func<T, string> enumMapping = null, Func<T[]> values = null) : base(id, defaultValue, page, options ?? (() => GetValues(values)),
        setActive, mapping, onChanged, useTranslations)
    {
        if (mapping == null && enumMapping != null)
            mapping = x => enumMapping(Enum.Parse<T>(x));

        Mapping = mapping ?? (x => x);
    }

    public override void SetString(string value) => Value = Enum.Parse<T>(value);

    private static string[] GetValues(Func<T[]> valuesInvoker)
    {
        var values = valuesInvoker?.Invoke();
        return values?.Select(x => x.ToString()).ToArray() ?? Enum.GetNames(typeof(T));
    }
}