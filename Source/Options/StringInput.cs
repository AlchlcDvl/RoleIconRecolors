namespace FancyUI.Options;

public class StringInputOption(string id, string defaultValue, PackType page, string regex = null, Func<string, bool> setActive = null, Action<string> onChanged = null) :
    BaseInputOption<StringInputSetting>(id, defaultValue, OptionType.StringInput, page, regex, setActive, onChanged);