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

public enum OverrideStartType : byte
{
    Default,
    FactionStart,
    FactionEnd,
	FactionBothColors,
	BlendFactionStart,
    BlendFactionEnd,
    BlendFactionBothColors
}

public enum OverrideEndType : byte
{
    Default,
    FactionStart,
    FactionEnd,
	FactionBothColors,
	BlendFactionStart,
    BlendFactionEnd,
    BlendFactionBothColors
}

public enum FactionLabelOption : byte
{
    Mismatch,
    Conditional,
    Always,
    Never
}

public enum PlayerListRoleIconOption : byte
{
    Disabled,
    Left,
    Right
}

public enum NecroPassingFormatOption : byte
{
    Vanilla,
    Classic,
    InvertedClassic
}

// Purely to define every vanilla and modded role as its own enum.
public enum RolePlus : byte
{
	None,
	Admirer,
	Amnesiac,
	Bodyguard,
	Cleric,
	Coroner,
	Crusader,
	Deputy,
	Investigator,
	Jailor,
	Lookout,
	Mayor,
	Monarch,
	Prosecutor,
	Psychic,
	Retributionist,
	Seer,
	Sheriff,
	Spy,
	TavernKeeper,
	Tracker,
	Trapper,
	Trickster,
	Veteran,
	Vigilante,
	Conjurer,
	CovenLeader,
	Dreamweaver,
	Enchanter,
	HexMaster,
	Illusionist,
	Jinx,
	Medusa,
	Necromancer,
	Poisoner,
	PotionMaster,
	Ritualist,
	VoodooMaster,
	Wildling,
	Witch,
	Arsonist,
	Baker,
	Berserker,
	Doomsayer,
	Executioner,
	Jester,
	Pirate,
	Plaguebearer,
	SerialKiller,
	Shroud,
	SoulCollector,
	Werewolf,
	Vampire,
	CursedSoul,
	Socialite,
	Marshal,
	Oracle,
	Pilgrim,
	Covenite,
	Catalyst,
	Cultist,
	Pacifist,
	Banshee,
	Warlock,
	Inquisitor,
	Jackal,
	Auditor,
	Judge,
	Starspawn,
	Famine,
	War,
	Pestilence,
	Death,
	Hidden,
	Stoned,
	Unknown
}