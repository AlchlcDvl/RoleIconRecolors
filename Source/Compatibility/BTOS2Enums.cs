namespace FancyUI.Compatibility;

public static class Btos2Role
{
    public const Role None = Role.NONE;

    // Town
    public const Role Admirer = Role.ADMIRER;
    public const Role Amnesiac = Role.AMNESIAC;
    public const Role Bodyguard = Role.BODYGUARD;
    public const Role Cleric = Role.CLERIC;
    public const Role Coroner = Role.CORONER;
    public const Role Crusader = Role.CRUSADER;
    public const Role Deputy = Role.DEPUTY;
    public const Role Investigator = Role.INVESTIGATOR;
    public const Role Jailor = Role.JAILOR;
    public const Role Lookout = Role.LOOKOUT;
    public const Role Mayor = Role.MAYOR;
    public const Role Monarch = Role.MONARCH;
    public const Role Prosecutor = Role.PROSECUTOR;
    public const Role Psychic = Role.PSYCHIC;
    public const Role Retributionist = Role.RETRIBUTIONIST;
    public const Role Seer = Role.SEER;
    public const Role Sheriff = Role.SHERIFF;
    public const Role Spy = Role.SPY;
    public const Role TavernKeeper = Role.TAVERNKEEPER;
    public const Role Tracker = Role.TRACKER;
    public const Role Trapper = Role.TRAPPER;
    public const Role Trickster = Role.TRICKSTER;
    public const Role Veteran = Role.VETERAN;
    public const Role Vigilante = Role.VIGILANTE;

    // Coven
    public const Role Conjurer = Role.CONJURER;
    public const Role CovenLeader = Role.COVENLEADER;
    public const Role Dreamweaver = Role.DREAMWEAVER;
    public const Role Enchanter = Role.ENCHANTER;
    public const Role HexMaster = Role.HEXMASTER;
    public const Role Illusionist = Role.ILLUSIONIST;
    public const Role Jinx = Role.JINX;
    public const Role Medusa = Role.MEDUSA;
    public const Role Necromancer = Role.NECROMANCER;
    public const Role Poisoner = Role.POISONER;
    public const Role PotionMaster = Role.POTIONMASTER;
    public const Role Ritualist = Role.RITUALIST;
    public const Role VoodooMaster = Role.VOODOOMASTER;
    public const Role Wildling = Role.WILDLING;
    public const Role Witch = Role.WITCH;

    // Neutral
    public const Role Arsonist = Role.ARSONIST;
    public const Role Baker = Role.BAKER;
    public const Role Berserker = Role.BERSERKER;
    public const Role Doomsayer = Role.DOOMSAYER;
    public const Role Executioner = Role.EXECUTIONER;
    public const Role Jester = Role.JESTER;
    public const Role Pirate = Role.PIRATE;
    public const Role Plaguebearer = Role.PLAGUEBEARER;
    public const Role SerialKiller = Role.SERIALKILLER;
    public const Role Shroud = Role.SHROUD;
    public const Role SoulCollector = Role.SOULCOLLECTOR;
    public const Role Werewolf = Role.WEREWOLF;

    // Special
    public const Role Vampire = Role.VAMPIRE;
    public const Role CursedSoul = Role.CURSED_SOUL;
    public const Role Catalyst = (Role)63;
    public const Role Cultist = (Role)64;

    // Modded
    public const Role Banshee = Role.SOCIALITE;
    public const Role Jackal = Role.MARSHAL;
    public const Role Marshal = Role.ORACLE;
    public const Role Judge = Role.PILGRIM;
    public const Role Auditor = Role.COVENITE;
    public const Role Inquisitor = Role.CATALYST;
    public const Role Starspawn = Role.CULTIST;
    public const Role Oracle = Role.ROLE_COUNT;
    public const Role Warlock = (Role)62;
    public const Role Socialite = (Role)65;
    public const Role Pacifist = (Role)66;

    public const Role RoleCount = (Role)67;

    // Buckets
    public const Role Any = (Role)100;
    public const Role RandomTown = (Role)101;
    public const Role CommonTown = (Role)102;
    public const Role TownInvestigative = (Role)103;
    public const Role TownProtective = (Role)104;
    public const Role TownKilling = (Role)105;
    public const Role TownSupport = (Role)106;
    public const Role RandomCoven = (Role)107;
    public const Role CommonCoven = (Role)108;
    public const Role CovenDeception = (Role)109;
    public const Role CovenKilling = (Role)110;
    public const Role CovenPower = (Role)111;
    public const Role CovenUtility = (Role)112;
    public const Role RandomApocalypse = (Role)113;
    public const Role RandomNeutral = (Role)114;
    public const Role NeutralEvil = (Role)115;
    public const Role NeutralKilling = (Role)116;
    public const Role NeutralPariah = (Role)117;
    public const Role NeutralOutlier = (Role)118;
    public const Role TownExecutive = (Role)119;
    public const Role TownGovernment = (Role)120;

    // Modifiers
    public const Role Vip = Role.VIP;
    public const Role CovenTownTraitor = Role.TOWN_TRAITOR;
    public const Role GhostTown = Role.GHOST_TOWN;
    public const Role PerfectTown = Role.NO_TOWN_HANGED;
    public const Role SlowMode = Role.SLOW_MODE;
    public const Role FastMode = Role.FAST_MODE;
    public const Role AnonVoting = Role.ANONYMOUS_VOTES;
    public const Role SecretKillers = Role.KILLER_ROLES_HIDDEN;
    public const Role HiddenRoles = Role.ROLES_ON_DEATH_HIDDEN;
    public const Role OneTrial = Role.ONE_TRIAL_PER_DAY;
    public const Role ApocTownTraitor = Role.FOUR_HORSEMEN;
    public const Role NecroPass = Role.ANON_PLAYERS;
    public const Role Teams = Role.FEELIN_LUCKY;
    public const Role AnonNames = Role.ALL_OUTLIERS;
    public const Role WalkingDead = Role.ELECTION;
    public const Role Egotist = Role.NO_NIGHT_ONE_KILLS;
    public const Role SpeakingSpirits = (Role)217;
    public const Role SecretObjectives = (Role)218;
    public const Role NoLastWills = (Role)219;
    public const Role Immovable = (Role)220;
    public const Role CompliantKillers = (Role)221;
    public const Role PandorasBox = (Role)222;
    public const Role BallotVoting = (Role)223;
    public const Role Individuality = (Role)224;
    public const Role Snitch = (Role)225;
    public const Role CovenVip = (Role)226;
    public const Role SecretWhispers = (Role)227;
    public const Role Lovers = (Role)228;
    public const Role Vc = (Role)229;
    public const Role FeelinLucky = (Role)230;
    public const Role AllOutliers = (Role)231;
    public const Role GracePeriod = (Role)232;

    // Special part 2
    public const Role Hangman = Role.HANGMAN;
    public const Role Hidden = Role.HIDDEN;
    public const Role Famine = Role.FAMINE;
    public const Role War = Role.WAR;
    public const Role Pestilence = Role.PESTILENCE;
    public const Role Death = Role.DEATH;
    public const Role Stoned = Role.STONED;

    public const Role Unknown = Role.UNKNOWN;
}

public static class Btos2Faction
{
    public const FactionType None = FactionType.NONE;
    public const FactionType Town = FactionType.TOWN;
    public const FactionType Coven = FactionType.COVEN;
    public const FactionType SerialKiller = FactionType.SERIALKILLER;
    public const FactionType Arsonist = FactionType.ARSONIST;
    public const FactionType Werewolf = FactionType.WEREWOLF;
    public const FactionType Shroud = FactionType.SHROUD;
    public const FactionType Apocalypse = FactionType.APOCALYPSE;
    public const FactionType Executioner = FactionType.EXECUTIONER;
    public const FactionType Jester = FactionType.JESTER;
    public const FactionType Pirate = FactionType.PIRATE;
    public const FactionType Doomsayer = FactionType.DOOMSAYER;
    public const FactionType Vampire = FactionType.VAMPIRE;
    public const FactionType CursedSoul = FactionType.CURSED_SOUL;
    public const FactionType Jackal = (FactionType)33;
    public const FactionType Frogs = (FactionType)34;
    public const FactionType Lions = (FactionType)35;
    public const FactionType Hawks = (FactionType)36;
    public const FactionType Cannibal = (FactionType)37;
    public const FactionType Judge = (FactionType)38;
    public const FactionType Auditor = (FactionType)39;
    public const FactionType Inquisitor = (FactionType)40;
    public const FactionType Starspawn = (FactionType)41;
    public const FactionType Egotist = (FactionType)42;
    public const FactionType Pandora = (FactionType)43;
    public const FactionType Compliance = (FactionType)44;
    public const FactionType Lovers = (FactionType)250;
}

public static class Btos2RoleAlignment
{
    public const RoleAlignment None = RoleAlignment.NONE;
    public const RoleAlignment Town = RoleAlignment.TOWN;
    public const RoleAlignment Coven = RoleAlignment.COVEN;
    public const RoleAlignment Neutral = RoleAlignment.NEUTRAL;
    public const RoleAlignment Any = RoleAlignment.ANY;
    public const RoleAlignment Vampire = RoleAlignment.VAMPIRE;
    public const RoleAlignment CursedSoul = RoleAlignment.CURSED_SOUL;
    public const RoleAlignment Unknown = RoleAlignment.UNKNOWN;
    public const RoleAlignment Apocalypse = (RoleAlignment)17;
    public const RoleAlignment Pandora = (RoleAlignment)100;
    public const RoleAlignment Compliance = (RoleAlignment)101;
}