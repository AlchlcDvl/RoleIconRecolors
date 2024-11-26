namespace FancyUI.Options;

public interface IDropdown
{
    public Func<IEnumerable<string>> Options { get; }
    public Dictionary<string, string> Mapping { get; }
    public string Value { get; set; }

    public void OnChanged();

    public bool SetActive();

    public void SetString(string value);

    public int GetInt() => Options().IndexOf(x => x == Value);

    public List<string> DisplayOptions()
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