namespace IconPacks;

public static class Utils
{
    private static readonly string[] VanillaSkippableNames = [ "Admirer_Ability", "Amnesiac_Ability", "Arsonist_Ability", "Attributes_Coven", "Baker_Ability", "Berserker_Ability",
        "Bodyguard_Ability", "Cleric_Ability", "Coroner_Ability", "CovenLeader_Ability", "Crusader_Ability", "CursedSoul_Ability", "Death_Ability", "Dreamweaver_Ability", "Enchanter_Ability",
        "Executioner_Ability", "Famine_Ability", "HexMaster_Ability", "Illusionist_Ability", "Investigator_Ability", "Jailor_Ability","Jailor_Ability_2", "Jester_Ability", "Jinx_Ability",
        "Lookout_Ability", "Medusa_Ability", "Monarch_Ability", "Necromancer_Ability_1", "Necromancer_Ability_2", "Pestilence_Ability", "Plaguebearer_Ability", "Poisoner_Ability",
        "PotionMaster_Ability_1", "PotionMaster_Ability_2", "Psychic_Ability", "Retributionist_Ability_1", "Retributionist_Ability_2", "Seer_Ability_1", "Seer_Ability_2", "Shroud_Special",
        "SerialKiller_Ability", "Sheriff_Ability", "Shroud_Ability", "SoulCollector_Ability", "Spy_Ability", "TavernKeeper_Ability", "Tracker_Ability", "Trapper_Ability", "Oracle_Special",
        "Trickster_Ability", "Vampire_Ability", "Vigilante_Ability", "VoodooMaster_Ability", "War_Ability_1", "War_Ability_2", "Werewolf_Ability_1", "Werewolf_Ability_2", "Wildling_Ability",
        "Witch_Ability_1", "Witch_Ability_2", "Jailor_Special", "Cleric_Special", "Mayor_Special", "Jester_Special", "Executioner_Special", "Bodyguard_Special", "Veteran_Special",
        "Trapper_Special", "Pirate_Special", "Admirer_Special", "Arsonist_Special", "Marshal_Special", "Socialite_Special", "Poisoner_Special", "CovenLeader_Special", "Coroner_Special",
        "SerialKiller_Special" ];
    private static readonly string[] BTOS2SkippableNames = [ "Admirer_Ability", "Amnesiac_Ability", "Arsonist_Ability", "Attributes_Coven", "Baker_Ability_1", "Berserker_Ability",
        "Bodyguard_Ability", "Cleric_Ability", "Coroner_Ability", "CovenLeader_Ability", "Crusader_Ability", "CursedSoul_Ability", "Death_Ability", "Dreamweaver_Ability", "Enchanter_Ability",
        "Famine_Ability", "HexMaster_Ability", "Illusionist_Ability", "Investigator_Ability", "Jailor_Ability","Jailor_Ability_2", "Jester_Ability", "Jinx_Ability", "Judge_Ability",
        "Lookout_Ability", "Medusa_Ability", "Monarch_Ability", "Necromancer_Ability_1", "Necromancer_Ability_2", "Pestilence_Ability", "Plaguebearer_Ability", "Poisoner_Ability",
        "PotionMaster_Ability_1", "PotionMaster_Ability_2", "Psychic_Ability", "Retributionist_Ability_1", "Retributionist_Ability_2", "Seer_Ability_1", "Seer_Ability_2", "Shroud_Special",
        "SerialKiller_Ability", "Sheriff_Ability", "Shroud_Ability", "SoulCollector_Ability", "Spy_Ability", "TavernKeeper_Ability", "Tracker_Ability", "Trapper_Ability", "Oracle_Special",
        "Trickster_Ability", "Vampire_Ability", "Vigilante_Ability", "VoodooMaster_Ability", "War_Ability_1", "War_Ability_2", "Werewolf_Ability_1", "Werewolf_Ability_2", "Wildling_Ability",
        "Witch_Ability_1", "Witch_Ability_2", "Jailor_Special", "Cleric_Special", "Mayor_Special", "Jester_Special", "Bodyguard_Special", "Veteran_Special", "Trapper_Special",
        "Pirate_Special", "Admirer_Special", "Arsonist_Special", "Marshal_Special", "Poisoner_Special", "CovenLeader_Special", "Coroner_Special", "SerialKiller_Special", "Starspawn_Ability",
        "Banshee_Ability", "Jackal_Ability", "Auditor_Ability", "Inquisitor_Ability", "Judge_Special", "Auditor_Special", "Inquisitor_Special", "Starspawn_Special", "Baker_Ability_2",
        "Baker_Special" ];

    //List of roles modified by Dum's mod
    private static readonly Role[] ChangedByToS1UI = [ Role.JAILOR, Role.CLERIC, Role.MAYOR, Role.JESTER, Role.EXECUTIONER, Role.BODYGUARD, Role.VETERAN, Role.TRAPPER, Role.PIRATE,
        Role.ADMIRER, Role.ARSONIST, Role.MARSHAL, Role.SOCIALITE, Role.POISONER, Role.COVENLEADER, Role.CORONER, Role.SERIALKILLER, Role.SHROUD, /*Role.ROLE_COUNT, (Role)57, (Role)58,
        (Role)59, (Role)60, (Role)61*/ ];

    public static readonly Dictionary<ModType, Dictionary<string, (string, int)>> RoleStuff = [];

    public static string RoleName(Role role, ModType? mod = null)
    {
        try
        {
            mod ??= GetGameType();
            return mod switch
            {
                ModType.Vanilla => VanillaRoleName(role),
                ModType.BTOS2 => BTOSRoleName(role),
                //ModType.Legacy => LegacyRoleName(role),
                _ => Constants.IsBTOS2 ? BTOSRoleName(role) : /*(Constants.IsLegacy ? LegacyRoleName(role) : */VanillaRoleName(role),//),
            };
        }
        catch
        {
            return VanillaRoleName(role);
        }
    }

    private static string BTOSRoleName(Role role) => role switch
    {
        BTOS2Role.Admirer => "Admirer",
        BTOS2Role.Amnesiac => "Amnesiac",
        BTOS2Role.Bodyguard => "Bodyguard",
        BTOS2Role.Cleric => "Cleric",
        BTOS2Role.Coroner => "Coroner",
        BTOS2Role.Crusader => "Crusader",
        BTOS2Role.Deputy => "Deputy",
        BTOS2Role.Investigator => "Investigator",
        BTOS2Role.Jailor => "Jailor",
        BTOS2Role.Lookout => "Lookout",
        BTOS2Role.Mayor => "Mayor",
        BTOS2Role.Monarch => "Monarch",
        BTOS2Role.Prosecutor => "Prosecutor",
        BTOS2Role.Psychic => "Psychic",
        BTOS2Role.Retributionist => "Retributionist",
        BTOS2Role.Seer => "Seer",
        BTOS2Role.Sheriff => "Sheriff",
        BTOS2Role.Spy => "Spy",
        BTOS2Role.TavernKeeper => "TavernKeeper",
        BTOS2Role.Tracker => "Tracker",
        BTOS2Role.Trapper => "Trapper",
        BTOS2Role.Trickster => "Trickster",
        BTOS2Role.Veteran => "Veteran",
        BTOS2Role.Vigilante => "Vigilante",
        BTOS2Role.Conjurer => "Conjurer",
        BTOS2Role.CovenLeader => "CovenLeader",
        BTOS2Role.Dreamweaver => "Dreamweaver",
        BTOS2Role.Enchanter => "Enchanter",
        BTOS2Role.HexMaster => "HexMaster",
        BTOS2Role.Illusionist => "Illusionist",
        BTOS2Role.Jinx => "Jinx",
        BTOS2Role.Medusa => "Medusa",
        BTOS2Role.Necromancer => "Necromancer",
        BTOS2Role.Poisoner => "Poisoner",
        BTOS2Role.PotionMaster => "PotionMaster",
        BTOS2Role.Ritualist => "Ritualist",
        BTOS2Role.VoodooMaster => "VoodooMaster",
        BTOS2Role.Wildling => "Wildling",
        BTOS2Role.Witch => "Witch",
        BTOS2Role.Arsonist => "Arsonist",
        BTOS2Role.Baker => "Baker",
        BTOS2Role.Berserker => "Berserker",
        BTOS2Role.Doomsayer => "Doomsayer",
        BTOS2Role.Executioner => "Executioner",
        BTOS2Role.Jester => "Jester",
        BTOS2Role.Pirate => "Pirate",
        BTOS2Role.Plaguebearer => "Plaguebearer",
        BTOS2Role.SerialKiller => "SerialKiller",
        BTOS2Role.Shroud => "Shroud",
        BTOS2Role.SoulCollector => "SoulCollector",
        BTOS2Role.Werewolf => "Werewolf",
        BTOS2Role.Famine => "Famine",
        BTOS2Role.War => "War",
        BTOS2Role.Pestilence => "Pestilence",
        BTOS2Role.Death => "Death",
        BTOS2Role.CursedSoul => "CursedSoul",
        BTOS2Role.Banshee => "Banshee",
        BTOS2Role.Jackal => "Jackal",
        BTOS2Role.Marshal => "Marshal",
        BTOS2Role.Judge => "Judge",
        BTOS2Role.Auditor => "Auditor",
        BTOS2Role.Inquisitor => "Inquisitor",
        BTOS2Role.Starspawn => "Starspawn",
        BTOS2Role.Oracle => "Oracle",
        BTOS2Role.Vampire => "Vampire",
        BTOS2Role.Stoned => "Stoned",
        BTOS2Role.RandomTown => "Town",
        BTOS2Role.RandomCoven => "Coven",
        BTOS2Role.RandomNeutral => "Neutral",
        BTOS2Role.TownInvestigative => "TownInvestigative",
        BTOS2Role.TownProtective => "TownProtective",
        BTOS2Role.TownKilling => "TownKilling",
        BTOS2Role.TownSupport => "TownSupport",
        BTOS2Role.TownPower => "TownPower",
        BTOS2Role.CovenKilling => "CovenKilling",
        BTOS2Role.CovenUtility => "CovenUtility",
        BTOS2Role.CovenDeception => "CovenDeception",
        BTOS2Role.CovenPower => "CovenPower",
        BTOS2Role.NeutralKilling => "NeutralKilling",
        BTOS2Role.NeutralEvil => "NeutralEvil",
        BTOS2Role.TrueAny => "TrueAny",
        BTOS2Role.CommonCoven => "CommonCoven",
        BTOS2Role.CommonTown => "CommonTown",
        BTOS2Role.RandomApocalypse => "RandomApocalypse",
        BTOS2Role.NeutralPariah => "NeutralPariah",
        BTOS2Role.NeutralSpecial => "NeutralSpecial",
        BTOS2Role.Any => "Any",
        BTOS2Role.CovenTownTraitor => "CovenTownTraitor",
        BTOS2Role.ApocTownTraitor => "ApocTownTraitor",
        BTOS2Role.PerfectTown => "PerfectTown",
        BTOS2Role.GhostTown => "GhostTown",
        BTOS2Role.VIP => "VIP",
        BTOS2Role.SlowMode => "SlowMode",
        BTOS2Role.FastMode => "FastMode",
        BTOS2Role.AnonVoting => "AnonVoting",
        BTOS2Role.SecretKillers => "SecretKillers",
        BTOS2Role.HiddenRoles => "HiddenRoles",
        BTOS2Role.OneTrial => "OneTrial",
        BTOS2Role.NecroPass => "NecroPass",
        BTOS2Role.Teams => "Teams",
        BTOS2Role.AnonNames => "AnonNames",
        BTOS2Role.WalkingDead => "WalkingDead",
        BTOS2Role.Hidden => "Hidden",
        _ => "Blank"
    };

    /*private static string LegacyRoleName(Role role) => role switch
    {
        LegacyRole.Admirer => "Admirer",
        LegacyRole.Amnesiac => "Amnesiac",
        LegacyRole.Bodyguard => "Bodyguard",
        LegacyRole.Cleric => "Cleric",
        LegacyRole.Coroner => "Coroner",
        LegacyRole.Crusader => "Crusader",
        LegacyRole.Deputy => "Deputy",
        LegacyRole.Investigator => "Investigator",
        LegacyRole.Jailor => "Jailor",
        LegacyRole.Lookout => "Lookout",
        LegacyRole.Mayor => "Mayor",
        LegacyRole.Monarch => "Monarch",
        LegacyRole.Prosecutor => "Prosecutor",
        LegacyRole.Psychic => "Psychic",
        LegacyRole.Retributionist => "Retributionist",
        LegacyRole.Seer => "Seer",
        LegacyRole.Sheriff => "Sheriff",
        LegacyRole.Spy => "Spy",
        LegacyRole.TavernKeeper => "TavernKeeper",
        LegacyRole.Tracker => "Tracker",
        LegacyRole.Trapper => "Trapper",
        LegacyRole.Trickster => "Trickster",
        LegacyRole.Veteran => "Veteran",
        LegacyRole.Vigilante => "Vigilante",
        LegacyRole.Conjurer => "Conjurer",
        LegacyRole.CovenLeader => "CovenLeader",
        LegacyRole.Dreamweaver => "Dreamweaver",
        LegacyRole.Enchanter => "Enchanter",
        LegacyRole.HexMaster => "HexMaster",
        LegacyRole.Illusionist => "Illusionist",
        LegacyRole.Jinx => "Jinx",
        LegacyRole.Medusa => "Medusa",
        LegacyRole.Necromancer => "Necromancer",
        LegacyRole.Poisoner => "Poisoner",
        LegacyRole.PotionMaster => "PotionMaster",
        LegacyRole.Ritualist => "Ritualist",
        LegacyRole.VoodooMaster => "VoodooMaster",
        LegacyRole.Wildling => "Wildling",
        LegacyRole.Witch => "Witch",
        LegacyRole.Arsonist => "Arsonist",
        LegacyRole.Baker => "Baker",
        LegacyRole.Berserker => "Berserker",
        LegacyRole.Doomsayer => "Doomsayer",
        LegacyRole.Executioner => "Executioner",
        LegacyRole.Jester => "Jester",
        LegacyRole.Pirate => "Pirate",
        LegacyRole.Plaguebearer => "Plaguebearer",
        LegacyRole.SerialKiller => "SerialKiller",
        LegacyRole.Shroud => "Shroud",
        LegacyRole.Reaper => "Reaper",
        LegacyRole.Werewolf => "Werewolf",
        LegacyRole.Vampire => "Vampire",
        LegacyRole.CursedSoul => "CursedSoul",
        LegacyRole.Socialite => "Socialite",
        LegacyRole.Marshal => "Marshal",
        LegacyRole.Famine => "Famine",
        LegacyRole.War => "War",
        LegacyRole.Pestilence => "Pestilence",
        LegacyRole.Death => "Death",
        LegacyRole.Medium => "Medium",
        LegacyRole.Saint => "Saint",
        LegacyRole.Transporter => "Transporter",
        LegacyRole.Ambusher => "Ambusher",
        LegacyRole.Assassin => "Assassin",
        LegacyRole.Blackmailer => "Blackmailer",
        LegacyRole.Consigliere => "Consigliere",
        LegacyRole.Consort => "Consort",
        LegacyRole.Disguiser => "Disguiser",
        LegacyRole.Framer => "Framer",
        LegacyRole.Forger => "Forger",
        LegacyRole.Godfather => "Godfather",
        LegacyRole.Hypnotist => "Hypnotist",
        LegacyRole.Janitor => "Janitor",
        LegacyRole.Mafioso => "Mafioso",
        LegacyRole.Wraith => "Wraith",
        LegacyRole.GuardianAngel => "GuardianAngel",
        LegacyRole.Juggernaut => "Juggernaut",
        LegacyRole.RandomTown => "RandomTown",
        LegacyRole.TownInvestigative => "TownInvestigative",
        LegacyRole.TownProtective => "TownProtective",
        LegacyRole.TownKilling => "TownKilling",
        LegacyRole.TownSupport => "TownSupport",
        LegacyRole.TownPower => "TownPower",
        LegacyRole.RandomCoven => "RandomCoven",
        LegacyRole.CovenKilling => "CovenKilling",
        LegacyRole.CovenUtility => "CovenUtility",
        LegacyRole.CovenDeception => "CovenDeception",
        LegacyRole.CovenPower => "CovenPower",
        LegacyRole.RandomNeutral => "RandomNeutral",
        LegacyRole.NeutralKilling => "NeutralKilling",
        LegacyRole.NeutralEvil => "NeutralEvil",
        LegacyRole.NeutralApocalypse => "NeutralApocalypse",
        LegacyRole.NeutralChaos => "NeutralChaos",
        LegacyRole.RandomMafia => "RandomMafia",
        LegacyRole.MafiaDeception => "MafiaDeception",
        LegacyRole.MafiaKilling => "MafiaKilling",
        LegacyRole.MafiaPower => "MafiaPower",
        LegacyRole.MafiaUtility => "MafiaUtility",
        LegacyRole.AnyEvil => "AnyEvil",
        LegacyRole.FactionedEvil => "FactionedEvil",
        LegacyRole.NonTown => "NonTown",
        LegacyRole.NonCoven => "NonCoven",
        LegacyRole.NonMafia => "NonMafia",
        LegacyRole.NonNeutral => "NonNeutral",
        LegacyRole.Any => "Any",
        LegacyRole.TrueAny => "TrueAny",
        LegacyRole.CovenTT => "CovenTownTraitor",
        LegacyRole.MafiaTT => "MafiaTownTraitor",
        LegacyRole.GhostTown => "GhostTown",
        LegacyRole.PerfectTown => "PerfectTown",
        LegacyRole.SlowMode => "SlowMode",
        LegacyRole.FastMode => "FastMode",
        LegacyRole.AnonVotes => "AnonVotes",
        LegacyRole.SecretKillers => "SecretKillers",
        LegacyRole.HiddenRoles => "HiddenRoles",
        LegacyRole.OneTrial => "OneTrial",
        LegacyRole.TownVEvils => "TownVEvils",
        LegacyRole.Lovers => "Lovers",
        LegacyRole.Hidden => "Hidden",
        LegacyRole.Cleaned => "Cleaned",
        LegacyRole.Stoned => "Stoned",
        _ => "Blank"
    };*/

    private static string VanillaRoleName(Role role) => role switch
    {
        Role.ADMIRER => "Admirer",
        Role.AMNESIAC => "Amnesiac",
        Role.BODYGUARD => "Bodyguard",
        Role.CLERIC => "Cleric",
        Role.CORONER => "Coroner",
        Role.CRUSADER => "Crusader",
        Role.DEPUTY => "Deputy",
        Role.INVESTIGATOR => "Investigator",
        Role.JAILOR => "Jailor",
        Role.LOOKOUT => "Lookout",
        Role.MARSHAL => "Marshal",
        Role.MAYOR => "Mayor",
        Role.MONARCH => "Monarch",
        Role.PROSECUTOR => "Prosecutor",
        Role.PSYCHIC => "Psychic",
        Role.RETRIBUTIONIST => "Retributionist",
        Role.SEER => "Seer",
        Role.SHERIFF => "Sheriff",
        Role.SOCIALITE => "Socialite",
        Role.SPY => "Spy",
        Role.TAVERNKEEPER => "TavernKeeper",
        Role.TRACKER => "Tracker",
        Role.TRAPPER => "Trapper",
        Role.TRICKSTER => "Trickster",
        Role.VETERAN => "Veteran",
        Role.VIGILANTE => "Vigilante",
        Role.CONJURER => "Conjurer",
        Role.COVENLEADER => "CovenLeader",
        Role.DREAMWEAVER => "Dreamweaver",
        Role.ENCHANTER => "Enchanter",
        Role.HEXMASTER => "HexMaster",
        Role.ILLUSIONIST => "Illusionist",
        Role.JINX => "Jinx",
        Role.MEDUSA => "Medusa",
        Role.NECROMANCER => "Necromancer",
        Role.POISONER => "Poisoner",
        Role.POTIONMASTER => "PotionMaster",
        Role.RITUALIST => "Ritualist",
        Role.VOODOOMASTER => "VoodooMaster",
        Role.WILDLING => "Wildling",
        Role.WITCH => "Witch",
        Role.ARSONIST => "Arsonist",
        Role.BAKER => "Baker",
        Role.BERSERKER => "Berserker",
        Role.DOOMSAYER => "Doomsayer",
        Role.EXECUTIONER => "Executioner",
        Role.JESTER => "Jester",
        Role.PIRATE => "Pirate",
        Role.PLAGUEBEARER => "Plaguebearer",
        Role.SERIALKILLER => "SerialKiller",
        Role.SHROUD => "Shroud",
        Role.SOULCOLLECTOR => "SoulCollector",
        Role.WEREWOLF => "Werewolf",
        Role.FAMINE => "Famine",
        Role.WAR => "War",
        Role.PESTILENCE => "Pestilence",
        Role.DEATH => "Death",
        Role.CURSED_SOUL => "CursedSoul",
        Role.VAMPIRE => "Vampire",
        Role.STONED => "Stoned",
        Role.RANDOM_TOWN => "Town",
        Role.RANDOM_COVEN => "Coven",
        Role.RANDOM_NEUTRAL => "Neutral",
        Role.TOWN_INVESTIGATIVE => "TownInvestigative",
        Role.TOWN_PROTECTIVE => "TownProtective",
        Role.TOWN_KILLING => "TownKilling",
        Role.TOWN_SUPPORT => "TownSupport",
        Role.TOWN_POWER => "TownPower",
        Role.COVEN_KILLING => "CovenKilling",
        Role.COVEN_UTILITY => "CovenUtility",
        Role.COVEN_DECEPTION => "CovenDeception",
        Role.COVEN_POWER => "CovenPower",
        Role.NEUTRAL_KILLING => "NeutralKilling",
        Role.NEUTRAL_EVIL => "NeutralEvil",
        Role.NEUTRAL_APOCALYPSE => "NeutralApocalypse",
        Role.ANY => "Any",
        Role.TOWN_TRAITOR => "TownTraitor",
        Role.NO_TOWN_HANGED => "PerfectTown",
        Role.GHOST_TOWN => "GhostTown",
        Role.VIP => "VIP",
        Role.SLOW_MODE => "SlowMode",
        Role.FAST_MODE => "FastMode",
        Role.ANONYMOUS_VOTES => "AnonVoting",
        Role.KILLER_ROLES_HIDDEN => "SecretKillers",
        Role.ROLES_ON_DEATH_HIDDEN => "HiddenRoles",
        Role.ONE_TRIAL_PER_DAY => "OneTrial",
        Role.HIDDEN => "Hidden",
        _ => "Blank"
    };

    public static string FactionName(FactionType faction, ModType? mod = null)
    {
        if (Constants.FactionOverridden)
            return Constants.FactionOverride;

        try
        {
            mod ??= GetGameType();
            return mod switch
            {
                ModType.Vanilla => VanillaFactionName(faction),
                ModType.BTOS2 => BTOSFactionName(faction),
                //ModType.Legacy => LegacyFactionName(faction),
                _ => Constants.IsBTOS2 ? BTOSFactionName(faction) : /*(Constants.IsLegacy ? LegacyFactionName(faction) : */VanillaFactionName(faction),//),
            };
        }
        catch
        {
            return VanillaFactionName(faction);
        }
    }

    private static string BTOSFactionName(FactionType faction) => faction switch
    {
        BTOS2Faction.Town => "Town",
        BTOS2Faction.Coven => "Coven",
        BTOS2Faction.SerialKiller => "SerialKiller",
        BTOS2Faction.Arsonist => "Arsonist",
        BTOS2Faction.Werewolf => "Werewolf",
        BTOS2Faction.Shroud => "Shroud",
        BTOS2Faction.Apocalypse => "Apocalypse",
        BTOS2Faction.Executioner => "Executioner",
        BTOS2Faction.Jester => "Jester",
        BTOS2Faction.Pirate => "Pirate",
        BTOS2Faction.Doomsayer => "Doomsayer",
        BTOS2Faction.Vampire => "Vampire",
        BTOS2Faction.CursedSoul => "CursedSoul",
        BTOS2Faction.Jackal => "Jackal",
        BTOS2Faction.Frogs => "Frogs",
        BTOS2Faction.Lions => "Lions",
        BTOS2Faction.Hawks => "Hawks",
        BTOS2Faction.Judge => "Judge",
        BTOS2Faction.Auditor => "Auditor",
        BTOS2Faction.Starspawn => "Starspawn",
        BTOS2Faction.Inquisitor => "Inquisitor",
        _ => "Blank"
    };

    /*private static string LegacyFactionName(FactionType faction) => faction switch
    {
        LegacyFaction.Amnesiac => "Amnesiac",
        LegacyFaction.Town => "Town",
        LegacyFaction.Coven => "Coven",
        LegacyFaction.Mafia => "Mafia",
        LegacyFaction.SerialKiller => "SerialKiller",
        LegacyFaction.Arsonist => "Arsonist",
        LegacyFaction.Werewolf => "Werewolf",
        LegacyFaction.Shroud => "Shroud",
        LegacyFaction.Apocalypse => "Apocalypse",
        LegacyFaction.Executioner => "Executioner",
        LegacyFaction.Jester => "Jester",
        LegacyFaction.Pirate => "Pirate",
        LegacyFaction.Doomsayer => "Doomsayer",
        LegacyFaction.Vampire => "Vampire",
        LegacyFaction.CursedSoul => "CursedSoul",
        LegacyFaction.Juggernaut => "Juggernaut",
        LegacyFaction.GuardianAngel => "GuardianAngel",
        LegacyFaction.Evils => "Evils",
        _ => "Blank"
    };*/

    private static string VanillaFactionName(FactionType faction) => faction switch
    {
        FactionType.TOWN => "Town",
        FactionType.COVEN => "Coven",
        FactionType.SERIALKILLER => "SerialKiller",
        FactionType.ARSONIST => "Arsonist",
        FactionType.WEREWOLF => "Werewolf",
        FactionType.SHROUD => "Shroud",
        FactionType.APOCALYPSE => "Apocalypse",
        FactionType.EXECUTIONER => "Executioner",
        FactionType.JESTER => "Jester",
        FactionType.PIRATE => "Pirate",
        FactionType.DOOMSAYER => "Doomsayer",
        FactionType.VAMPIRE => "Vampire",
        FactionType.CURSED_SOUL => "CursedSoul",
        _ => "Blank"
    };

    public static bool ModifiedByToS1UI(Role role) => ChangedByToS1UI.Contains(role);

    public static bool IsValid(this List<BaseAbilityButton> list, int index)
    {
        try
        {
            return list[index];
        }
        catch
        {
            return false;
        }
    }

    public static string GetLevel(int value, bool attack) => value switch
    {
        1 => "Basic",
        2 => "Powerful",
        3 => attack ? "Unstoppable" : "Invincible",
        4 => "Ethereal",
        _ => "None"
    };

    public static bool IsApoc(this Role role, ModType? mod = null)
    {
        try
        {
            mod ??= GetGameType();
            return mod switch
            {
                ModType.Vanilla => IsApocVanilla(role),
                ModType.BTOS2 => IsApocBTOS2(role),
                //ModType.Legacy => IsApocLegacy(role),
                _ => Constants.IsBTOS2 ? IsApocBTOS2(role) : /*(Constants.IsLegacy ? IsApocLegacy(role) : */IsApocVanilla(role),//),
            };
        }
        catch
        {
            return IsApocVanilla(role);
        }
    }

    public static bool IsApocVanilla(this Role role) => role is Role.BERSERKER or Role.WAR or Role.BAKER or Role.FAMINE or Role.SOULCOLLECTOR or Role.DEATH or Role.PLAGUEBEARER or
        Role.PESTILENCE;

    public static bool IsApocBTOS2(this Role role) => role is BTOS2Role.Berserker or BTOS2Role.War or BTOS2Role.Baker or BTOS2Role.Famine or BTOS2Role.SoulCollector or BTOS2Role.Death or
        BTOS2Role.Plaguebearer or BTOS2Role.Pestilence;

    /*public static bool IsApocLegacy(this Role role) => role is LegacyRole.Berserker or LegacyRole.War or LegacyRole.Baker or LegacyRole.Famine or LegacyRole.Reaper or LegacyRole.Death or
        LegacyRole.Plaguebearer or LegacyRole.Pestilence;*/

    public static bool Skippable(string name) => GetGameType() switch
    {
        ModType.BTOS2 => BTOS2SkippableNames.Contains(name),
        _ => VanillaSkippableNames.Contains(name)
    };

    public static Dictionary<string, (string, int)> Filtered(ModType mod = ModType.Vanilla)
    {
        if (RoleStuff.TryGetValue(mod, out var result))
            return result;

        var roles = mod switch
        {
            ModType.BTOS2 => typeof(BTOS2Role)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(Role))
                .Select(x => (Role)x.GetRawConstantValue())
                .Where(x => x is not (BTOS2Role.None or BTOS2Role.Hangman or BTOS2Role.Unknown or BTOS2Role.RoleCount)),
            /*ModType.Legacy => typeof(LegacyRole)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(Role))
                .Select(x => (Role)x.GetRawConstantValue())
                .Where(x => x is not (LegacyRole.None or LegacyRole.Hangman or LegacyRole.Unknown or LegacyRole.RoleCount)),*/
            _ => ((Role[])Enum.GetValues(typeof(Role))).Where(x => x is not (Role.NONE or Role.ROLE_COUNT or Role.UNKNOWN or Role.HANGMAN))
        };

        var rolesWithIndex = roles.ToDictionary(role => role.ToString().ToLower(), role => ($"Role{(int)role}", (int)role));
        return RoleStuff[mod] = rolesWithIndex;
    }

    public static void DumpSprite(Texture2D texture, string fileName, string path = null, bool decompress = false)
    {
        path ??= AssetManager.ModPath;
        var assetPath = Path.Combine(path, $"{fileName}.png");

        if (File.Exists(assetPath))
            File.Delete(assetPath);

        File.WriteAllBytes(assetPath, decompress ? texture.Decompress().EncodeToPNG() : texture.EncodeToPNG());
    }

    public static bool IsTransformedApoc(this Role role, ModType mod = ModType.None)
    {
        try
        {
            return mod switch
            {
                ModType.Vanilla => IsTransformedApocVanilla(role),
                ModType.BTOS2 => IsTransformedApocBTOS(role),
                //ModType.Legacy => LegacyFactionName(faction),
                _ => Constants.IsBTOS2 ? IsTransformedApocBTOS(role) : /*(Constants.IsLegacy ? LegacyFactionName(faction) : */IsTransformedApocVanilla(role),//),
            };
        }
        catch
        {
            return IsTransformedApocVanilla(role);
        }
    }

    private static bool IsTransformedApocBTOS(this Role role) => role is BTOS2Role.Death or BTOS2Role.Famine or BTOS2Role.War or BTOS2Role.Pestilence;

    private static bool IsTransformedApocVanilla(this Role role) => role is Role.DEATH or Role.FAMINE or Role.WAR or Role.PESTILENCE;

    //private static bool IsTransformedApocLegacy(this Role role) => role is LegacyRole.Death or LegacyRole.Famine or LegacyRole.War or LegacyRole.Pestilence;

    public static bool IsValid(this Sprite sprite) => sprite && sprite != AssetManager.Blank;

    public static string EffectName(EffectType effect) => effect switch
    {
        EffectType.EXECUTIONER_TARGET => "ExeTarget",
        EffectType.HEXED => "Hexed",
        EffectType.KNIGHTED => "Knighted",
        EffectType.REVEALED_MAYOR => "RevealedMayor",
        EffectType.DISCONNECTED => "Disconnected",
        EffectType.CONNECTING => "Connecting",
        EffectType.LOVER => "Lover",
        EffectType.DOUSED => "Doused",
        EffectType.PLAGUED => "Plagued",
        EffectType.NECRONOMICON => "Necronomicon",
        EffectType.TRAPPED => "Trapped",
        EffectType.BREAD => "Bread",
        EffectType.HANGOVER => "Hangover",
        EffectType.VOODOOED => "Silenced",
        EffectType.DREAMWOVE => "Dreamwoven",
        EffectType.INSANE => "Insane",
        EffectType.VIP => "VIP",
        EffectType.BUG => "Bugged",
        EffectType.SCENT_TRACK => "Tracked",
        EffectType.PESTILENCE => "Pest",
        EffectType.REVEALED_MARSHAL => "RevealedMarshal",
        (EffectType)100 => "Recruit",
        (EffectType)101 => "Deafened",
        (EffectType)102 or (EffectType)200 => "CovenTownTraitor",
        (EffectType)103 => "ApocTownTraitor",
        (EffectType)104 => "Audited",
        (EffectType)105 or (EffectType)203 => "Enchanted",
        (EffectType)106 => "Accompanied",
        (EffectType)107 => "PandoraTownTraitor",
        (EffectType)108 => "Egoist",
        (EffectType)204 or (EffectType)109 => "Reaped",
        (EffectType)201 => "MafiaTownTraitor",
        (EffectType)202 => "Transported",
        (EffectType)205 => "Hypnotised",
        (EffectType)206 => "Gazed",
        (EffectType)207 => "RevealedDeputy",
        (EffectType)208 => "Blackmailed",
        (EffectType)209 => "Blessed",
        (EffectType)210 => "Framed",
        (EffectType)211 => "RevealedProsecutor",
        _ => "Blank"
    };

    public static ModType GetGameType()
    {
        if (Constants.IsBTOS2 || FindCasualQueue())
            return ModType.BTOS2;
        /*else if (Constants.IsLegacy)
            return ModType.Legacy;*/
        else
            return ModType.Vanilla;
    }

    public static bool FindCasualQueue()
    {
        try
        {
            return FindCasualQueueBypass();
        }
        catch
        {
            return false;
        }
    }

    private static bool FindCasualQueueBypass() => BetterTOS2.BTOSInfo.CasualModeController;

    public static bool IsEthereal(this UIRoleData.UIRoleDataInstance ui)
    {
        try
        {
            return Constants.IsBTOS2 && ui.role is BTOS2Role.Judge or BTOS2Role.Auditor or BTOS2Role.Starspawn;
        }
        catch
        {
            return false;
        }
    }

    public static Role GetTransformedVersion(Role role, ModType? mod = null)
    {
        try
        {
            mod ??= GetGameType();
            return mod switch
            {
                ModType.BTOS2 => role switch
                {
                    BTOS2Role.Baker => BTOS2Role.Famine,
                    BTOS2Role.Berserker => BTOS2Role.War,
                    BTOS2Role.SoulCollector => BTOS2Role.Death,
                    BTOS2Role.Plaguebearer => BTOS2Role.Pestilence,
                    _ => role
                },
                /*ModType.Legacy => role switch
                {
                    LegacyRole.Baker => LegacyRole.Famine,
                    LegacyRole.Berserker => LegacyRole.War,
                    LegacyRole.Reaper => LegacyRole.Death,
                    LegacyRole.Plaguebearer => LegacyRole.Pestilence,
                    _ => role
                },*/
                _ => role switch
                {
                    Role.BAKER => Role.FAMINE,
                    Role.BERSERKER => Role.WAR,
                    Role.SOULCOLLECTOR => Role.DEATH,
                    Role.PLAGUEBEARER => Role.PESTILENCE,
                    _ => role
                }
            };
        }
        catch
        {
            return role switch
            {
                Role.BAKER => Role.FAMINE,
                Role.BERSERKER => Role.WAR,
                Role.SOULCOLLECTOR => Role.DEATH,
                Role.PLAGUEBEARER => Role.PESTILENCE,
                _ => role
            };
        }
    }

    public static Role GetNecro(ModType? mod = null)
    {
        try
        {
            mod ??= GetGameType();
            return mod switch
            {
                ModType.BTOS2 => BTOS2Role.Necromancer,
                //ModType.Legacy => LegacyRole.Necromancer,
                _ => Role.NECROMANCER,
            };
        }
        catch
        {
            return Role.NECROMANCER;
        }
    }

    public static Role GetWar(ModType? mod = null)
    {
        try
        {
            mod ??= GetGameType();
            return mod switch
            {
                ModType.BTOS2 => BTOS2Role.War,
                //ModType.Legacy => LegacyRole.War,
                _ => Role.WAR,
            };
        }
        catch
        {
            return Role.WAR;
        }
    }

    public static TKey GetKey<TKey, TValue>(this IDictionary<TKey, TValue> dict, TValue value)
    {
        foreach (var (key, value2) in dict)
        {
            if (value.Equals(value2))
                return key;
        }

        return default;
    }

    public static bool TryGetKey<TKey, TValue>(this IDictionary<TKey, TValue> dict, TValue value, out TKey key)
    {
        try
        {
            key = dict.GetKey(value);
            return true;
        }
        catch
        {
            key = default;
            return false;
        }
    }

    public static bool IsValid(this SpecialAbilityPopupPanel panel)
    {
        try
        {
            return panel && panel.useButton && panel.useButton.abilityIcon;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsValid(this SpecialAbilityPanel panel)
    {
        try
        {
            return panel && panel.useButton && panel.useButton.abilityIcon;
        }
        catch
        {
            return false;
        }
    }

    /*public static FactionType GetFactionType(this Role role, ModType? mod = null)
    {
        mod ??= GetGameType();

        if ((int)role is > 0 and < 25)
            return FactionType.TOWN;
        else if ((int)role is > 24 and < 40)
            return FactionType.COVEN;
        else if ((int)role is 41 or 42 or 47 or 50 or 250 or 251 or 252 or 253)
            return FactionType.APOCALYPSE;
        else if ((int)role is (41 or 42 or 47 or 50) and < 54)
        {
            return role switch
            {
                Role.ARSONIST => FactionType.ARSONIST,
                Role.DOOMSAYER => FactionType.DOOMSAYER,
                Role.EXECUTIONER => FactionType.EXECUTIONER,
                Role.JESTER => FactionType.JESTER,
                Role.PIRATE => FactionType.PIRATE,
                Role.SERIALKILLER => FactionType.SERIALKILLER,
                Role.SHROUD => FactionType.SHROUD,
                Role.WEREWOLF => FactionType.WEREWOLF,
                Role.VAMPIRE => FactionType.VAMPIRE,
                Role.CURSED_SOUL => FactionType.CURSED_SOUL,
                _ => FactionType.NONE
            };
        }

        if (mod == ModType.BTOS2)
        {
            return role switch
            {
                BTOS2Role.Banshee => BTOS2Faction.Coven,
                BTOS2Role.Marshal or BTOS2Role.Oracle => BTOS2Faction.Town,
                BTOS2Role.Jackal => BTOS2Faction.Jackal,
                BTOS2Role.Judge => BTOS2Faction.Judge,
                BTOS2Role.Auditor => BTOS2Faction.Auditor,
                BTOS2Role.Inquisitor => BTOS2Faction.Inquisitor,
                BTOS2Role.Starspawn => BTOS2Faction.Starspawn,
                _ => BTOS2Faction.None
            };
        }
        else if (mod == ModType.Vanilla)
        {
            if ((int)role is 54 or 55)
                return FactionType.TOWN;
        }

        return FactionType.NONE;
    }*/
}