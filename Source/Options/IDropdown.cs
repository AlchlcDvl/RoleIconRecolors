namespace FancyUI.Options;

public interface IDropdown
{
    Func<IEnumerable<string>> Options { get; }
    Dictionary<string, string> Mapping { get; }

    void SetString(string value);

    List<string> DisplayOptions()
    {
        var result = new List<string>();

        foreach (var option in Options())
        {
            if (Mapping.TryGetValue(option, out var display))
                result.Add(display);
            else
                result.Add(option);
        }

        return result;
    }
}