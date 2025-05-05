namespace FancyUI.Options;

public class StringInputOption(string id, string defaultValue, PackType page, string regex = null, Func<bool> setActive = null, Action<string> onChanged = null) :
    BaseInputOption<StringInputSetting>(id, defaultValue, page, regex, setActive, onChanged);