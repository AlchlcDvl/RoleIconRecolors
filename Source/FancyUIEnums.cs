namespace FancyUI;

public enum ModType : byte
{
    Common,
    Vanilla,
    BTOS2,
    None
}

public enum PackType : byte
{
    IconPacks,
    RecoloredUI,
    SilhouetteSets,
    MiscRoleCustomisation,
    Settings
}

public enum ColorType : byte
{
    Wood,
    Metal,
    Paper,
    Leather,
    Flame,
    Wax
}

public enum OptionType : byte
{
    Toggle,
    Slider,
    StringDropdown,
    EnumDropdown,
    StringInput,
    ColorInput
}

public enum UITheme : byte
{
    Default,
    Role,
    Faction,
    Custom
}