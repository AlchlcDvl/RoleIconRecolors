namespace FancyUI;

public enum GameModType : byte
{
    Common,
    Vanilla,
    BTOS2,
    None
}

public enum PackType : byte
{
    Testing,
    RecoloredUI,
    IconPacks,
    SilhouetteSets,
    MiscRoleCustomisation,
    CinematicSwapper,
    None
}

public enum ColorType : byte
{
    Wood,
    Metal,
    Paper,
    Leather,
    Fire,
    Wax,
    All
}

public enum UITheme : byte
{
    Vanilla,
    Faction,
    Custom
}

public enum DisplayType : byte
{
    RoleCard,
    PlayerList,
    RoleList,
    Graveyard,
    RlPlusGy,
    RoleDeck,
    SpecialAbilityPopup,
    Chat
}

// public enum IconType : byte
// {
//     Ability1,
//     Ability2,
//     Attribute,
//     Necronomicon,
//     Effect
// }

public enum RecruitEndType : byte
{
    JackalEnd,
    FactionStart,
    FactionEnd
}

public enum FactionLabelOption : byte
{
    Mismatch,
    Conditional,
    Always,
    Never
}