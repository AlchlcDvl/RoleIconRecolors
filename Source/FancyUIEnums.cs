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
    RecoloredUI,
    IconPacks,
    SilhouetteSets,
    MiscRoleCustomisation,
    Testing
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

public enum IconType : byte
{
    Ability1,
    Ability2,
    Attribute,
    Necronomicon,
    Effect
}

public enum RecruitEndType : byte
{
    Default,
    FactionStart,
    FactionEnd,
    FactionMajor
}