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
        Role.ADMIRER, Role.ARSONIST, Role.MARSHAL, Role.SOCIALITE, Role.POISONER, Role.COVENLEADER, Role.CORONER, Role.SERIALKILLER, Role.SHROUD, Role.ROLE_COUNT, (Role)57, (Role)58,
        (Role)59, (Role)60, (Role)61 ];

    public static string RoleName(Role role, ModType mod = ModType.None)
    {
        try
        {
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
        BetterTOS2.RolePlus.ADMIRER => "Admirer",
        BetterTOS2.RolePlus.AMNESIAC => "Amnesiac",
        BetterTOS2.RolePlus.BODYGUARD => "Bodyguard",
        BetterTOS2.RolePlus.CLERIC => "Cleric",
        BetterTOS2.RolePlus.CORONER => "Coroner",
        BetterTOS2.RolePlus.CRUSADER => "Crusader",
        BetterTOS2.RolePlus.DEPUTY => "Deputy",
        BetterTOS2.RolePlus.INVESTIGATOR => "Investigator",
        BetterTOS2.RolePlus.JAILOR => "Jailor",
        BetterTOS2.RolePlus.LOOKOUT => "Lookout",
        BetterTOS2.RolePlus.MAYOR => "Mayor",
        BetterTOS2.RolePlus.MONARCH => "Monarch",
        BetterTOS2.RolePlus.PROSECUTOR => "Prosecutor",
        BetterTOS2.RolePlus.PSYCHIC => "Psychic",
        BetterTOS2.RolePlus.RETRIBUTIONIST => "Retributionist",
        BetterTOS2.RolePlus.SEER => "Seer",
        BetterTOS2.RolePlus.SHERIFF => "Sheriff",
        BetterTOS2.RolePlus.SPY => "Spy",
        BetterTOS2.RolePlus.TAVERN_KEEPER => "TavernKeeper",
        BetterTOS2.RolePlus.TRACKER => "Tracker",
        BetterTOS2.RolePlus.TRAPPER => "Trapper",
        BetterTOS2.RolePlus.TRICKSTER => "Trickster",
        BetterTOS2.RolePlus.VETERAN => "Veteran",
        BetterTOS2.RolePlus.VIGILANTE => "Vigilante",
        BetterTOS2.RolePlus.CONJURER => "Conjurer",
        BetterTOS2.RolePlus.COVEN_LEADER => "CovenLeader",
        BetterTOS2.RolePlus.DREAMWEAVER => "Dreamweaver",
        BetterTOS2.RolePlus.ENCHANTER => "Enchanter",
        BetterTOS2.RolePlus.HEX_MASTER => "HexMaster",
        BetterTOS2.RolePlus.ILLUSIONIST => "Illusionist",
        BetterTOS2.RolePlus.JINX => "Jinx",
        BetterTOS2.RolePlus.MEDUSA => "Medusa",
        BetterTOS2.RolePlus.NECROMANCER => "Necromancer",
        BetterTOS2.RolePlus.POISONER => "Poisoner",
        BetterTOS2.RolePlus.POTION_MASTER => "PotionMaster",
        BetterTOS2.RolePlus.RITUALIST => "Ritualist",
        BetterTOS2.RolePlus.VOODOO_MASTER => "VoodooMaster",
        BetterTOS2.RolePlus.WILDLING => "Wildling",
        BetterTOS2.RolePlus.WITCH => "Witch",
        BetterTOS2.RolePlus.ARSONIST => "Arsonist",
        BetterTOS2.RolePlus.BAKER => "Baker",
        BetterTOS2.RolePlus.BERSERKER => "Berserker",
        BetterTOS2.RolePlus.DOOMSAYER => "Doomsayer",
        BetterTOS2.RolePlus.EXECUTIONER => "Executioner",
        BetterTOS2.RolePlus.JESTER => "Jester",
        BetterTOS2.RolePlus.PIRATE => "Pirate",
        BetterTOS2.RolePlus.PLAGUEBEARER => "Plaguebearer",
        BetterTOS2.RolePlus.SERIAL_KILLER => "SerialKiller",
        BetterTOS2.RolePlus.SHROUD => "Shroud",
        BetterTOS2.RolePlus.SOUL_COLLECTOR => "SoulCollector",
        BetterTOS2.RolePlus.WEREWOLF => "Werewolf",
        BetterTOS2.RolePlus.FAMINE => "Famine",
        BetterTOS2.RolePlus.WAR => "War",
        BetterTOS2.RolePlus.PESTILENCE => "Pestilence",
        BetterTOS2.RolePlus.DEATH => "Death",
        BetterTOS2.RolePlus.CURSED_SOUL => "CursedSoul",
        BetterTOS2.RolePlus.BANSHEE => "Banshee",
        BetterTOS2.RolePlus.JACKAL => "Jackal",
        BetterTOS2.RolePlus.MARSHAL => "Marshal",
        BetterTOS2.RolePlus.JUDGE => "Judge",
        BetterTOS2.RolePlus.AUDITOR => "Auditor",
        BetterTOS2.RolePlus.INQUISITOR => "Inquisitor",
        BetterTOS2.RolePlus.STARSPAWN => "Starspawn",
        BetterTOS2.RolePlus.ORACLE => "Oracle",
        BetterTOS2.RolePlus.VAMPIRE => "Vampire",
        BetterTOS2.RolePlus.STONED => "Stoned",
        BetterTOS2.RolePlus.RANDOM_TOWN => "Town",
        BetterTOS2.RolePlus.RANDOM_COVEN => "Coven",
        BetterTOS2.RolePlus.RANDOM_NEUTRAL => "Neutral",
        BetterTOS2.RolePlus.TOWN_INVESTIGATIVE => "TownInvestigative",
        BetterTOS2.RolePlus.TOWN_PROTECTIVE => "TownProtective",
        BetterTOS2.RolePlus.TOWN_KILLING => "TownKilling",
        BetterTOS2.RolePlus.TOWN_SUPPORT => "TownSupport",
        BetterTOS2.RolePlus.TOWN_POWER => "TownPower",
        BetterTOS2.RolePlus.COVEN_KILLING => "CovenKilling",
        BetterTOS2.RolePlus.COVEN_UTILITY => "CovenUtility",
        BetterTOS2.RolePlus.COVEN_DECEPTION => "CovenDeception",
        BetterTOS2.RolePlus.COVEN_POWER => "CovenPower",
        BetterTOS2.RolePlus.NEUTRAL_KILLING => "NeutralKilling",
        BetterTOS2.RolePlus.NEUTRAL_EVIL => "NeutralEvil",
        BetterTOS2.RolePlus.TRUE_ANY => "TrueAny",
        BetterTOS2.RolePlus.REGULAR_COVEN => "CommonCoven",
        BetterTOS2.RolePlus.REGULAR_TOWN => "CommonTown",
        BetterTOS2.RolePlus.RANDOM_APOCALYPSE => "RandomApocalypse",
        BetterTOS2.RolePlus.NEUTRAL_PARIAH => "NeutralPariah",
        BetterTOS2.RolePlus.NEUTRAL_SPECIAL => "NeutralSpecial",
        BetterTOS2.RolePlus.ANY => "Any",
        BetterTOS2.RolePlus.COVEN_TOWN_TRAITOR => "CovenTownTraitor",
        BetterTOS2.RolePlus.APOC_TOWN_TRAITOR => "ApocTownTraitor",
        BetterTOS2.RolePlus.PERFECT_TOWN => "PerfectTown",
        BetterTOS2.RolePlus.GHOST_TOWN => "GhostTown",
        BetterTOS2.RolePlus.VIP => "VIP",
        BetterTOS2.RolePlus.SLOW_MODE => "SlowMode",
        BetterTOS2.RolePlus.FAST_MODE => "FastMode",
        BetterTOS2.RolePlus.ANON_VOTING => "AnonVoting",
        BetterTOS2.RolePlus.SECRET_KILLERS => "SecretKillers",
        BetterTOS2.RolePlus.HIDDEN_ROLES => "HiddenRoles",
        BetterTOS2.RolePlus.ONE_TRIAL => "OneTrial",
        BetterTOS2.RolePlus.NECRO_PASSING => "NecroPass",
        BetterTOS2.RolePlus.TEAMS => "Teams",
        BetterTOS2.RolePlus.ANON_PLAYERS => "AnonNames",
        BetterTOS2.RolePlus.WALKING_DEAD => "WalkingDead",
        BetterTOS2.RolePlus.HIDDEN => "Hidden",
        _ => "Blank"
    };

    /*private static string LegacyRoleName(Role role) => role switch
    {
        LegacyClient.Info.LegacyRole.Admirer => "Admirer",
        LegacyClient.Info.LegacyRole.Amnesiac => "Amnesiac",
        LegacyClient.Info.LegacyRole.Bodyguard => "Bodyguard",
        LegacyClient.Info.LegacyRole.Cleric => "Cleric",
        LegacyClient.Info.LegacyRole.Coroner => "Coroner",
        LegacyClient.Info.LegacyRole.Crusader => "Crusader",
        LegacyClient.Info.LegacyRole.Deputy => "Deputy",
        LegacyClient.Info.LegacyRole.Investigator => "Investigator",
        LegacyClient.Info.LegacyRole.Jailor => "Jailor",
        LegacyClient.Info.LegacyRole.Lookout => "Lookout",
        LegacyClient.Info.LegacyRole.Mayor => "Mayor",
        LegacyClient.Info.LegacyRole.Monarch => "Monarch",
        LegacyClient.Info.LegacyRole.Prosecutor => "Prosecutor",
        LegacyClient.Info.LegacyRole.Psychic => "Psychic",
        LegacyClient.Info.LegacyRole.Retributionist => "Retributionist",
        LegacyClient.Info.LegacyRole.Seer => "Seer",
        LegacyClient.Info.LegacyRole.Sheriff => "Sheriff",
        LegacyClient.Info.LegacyRole.Spy => "Spy",
        LegacyClient.Info.LegacyRole.TavernKeeper => "TavernKeeper",
        LegacyClient.Info.LegacyRole.Tracker => "Tracker",
        LegacyClient.Info.LegacyRole.Trapper => "Trapper",
        LegacyClient.Info.LegacyRole.Trickster => "Trickster",
        LegacyClient.Info.LegacyRole.Veteran => "Veteran",
        LegacyClient.Info.LegacyRole.Vigilante => "Vigilante",
        LegacyClient.Info.LegacyRole.Conjurer => "Conjurer",
        LegacyClient.Info.LegacyRole.CovenLeader => "CovenLeader",
        LegacyClient.Info.LegacyRole.Dreamweaver => "Dreamweaver",
        LegacyClient.Info.LegacyRole.Enchanter => "Enchanter",
        LegacyClient.Info.LegacyRole.HexMaster => "HexMaster",
        LegacyClient.Info.LegacyRole.Illusionist => "Illusionist",
        LegacyClient.Info.LegacyRole.Jinx => "Jinx",
        LegacyClient.Info.LegacyRole.Medusa => "Medusa",
        LegacyClient.Info.LegacyRole.Necromancer => "Necromancer",
        LegacyClient.Info.LegacyRole.Poisoner => "Poisoner",
        LegacyClient.Info.LegacyRole.PotionMaster => "PotionMaster",
        LegacyClient.Info.LegacyRole.Ritualist => "Ritualist",
        LegacyClient.Info.LegacyRole.VoodooMaster => "VoodooMaster",
        LegacyClient.Info.LegacyRole.Wildling => "Wildling",
        LegacyClient.Info.LegacyRole.Witch => "Witch",
        LegacyClient.Info.LegacyRole.Arsonist => "Arsonist",
        LegacyClient.Info.LegacyRole.Baker => "Baker",
        LegacyClient.Info.LegacyRole.Berserker => "Berserker",
        LegacyClient.Info.LegacyRole.Doomsayer => "Doomsayer",
        LegacyClient.Info.LegacyRole.Executioner => "Executioner",
        LegacyClient.Info.LegacyRole.Jester => "Jester",
        LegacyClient.Info.LegacyRole.Pirate => "Pirate",
        LegacyClient.Info.LegacyRole.Plaguebearer => "Plaguebearer",
        LegacyClient.Info.LegacyRole.SerialKiller => "SerialKiller",
        LegacyClient.Info.LegacyRole.Shroud => "Shroud",
        LegacyClient.Info.LegacyRole.Reaper => "Reaper",
        LegacyClient.Info.LegacyRole.Werewolf => "Werewolf",
        LegacyClient.Info.LegacyRole.Vampire => "Vampire",
        LegacyClient.Info.LegacyRole.CursedSoul => "CursedSoul",
        LegacyClient.Info.LegacyRole.Socialite => "Socialite",
        LegacyClient.Info.LegacyRole.Marshal => "Marshal",
        LegacyClient.Info.LegacyRole.Famine => "Famine",
        LegacyClient.Info.LegacyRole.War => "War",
        LegacyClient.Info.LegacyRole.Pestilence => "Pestilence",
        LegacyClient.Info.LegacyRole.Death => "Death",
        LegacyClient.Info.LegacyRole.Medium => "Medium",
        LegacyClient.Info.LegacyRole.Saint => "Saint",
        LegacyClient.Info.LegacyRole.Transporter => "Transporter",
        LegacyClient.Info.LegacyRole.Ambusher => "Ambusher",
        LegacyClient.Info.LegacyRole.Assassin => "Assassin",
        LegacyClient.Info.LegacyRole.Blackmailer => "Blackmailer",
        LegacyClient.Info.LegacyRole.Consigliere => "Consigliere",
        LegacyClient.Info.LegacyRole.Consort => "Consort",
        LegacyClient.Info.LegacyRole.Disguiser => "Disguiser",
        LegacyClient.Info.LegacyRole.Framer => "Framer",
        LegacyClient.Info.LegacyRole.Forger => "Forger",
        LegacyClient.Info.LegacyRole.Godfather => "Godfather",
        LegacyClient.Info.LegacyRole.Hypnotist => "Hypnotist",
        LegacyClient.Info.LegacyRole.Janitor => "Janitor",
        LegacyClient.Info.LegacyRole.Mafioso => "Mafioso",
        LegacyClient.Info.LegacyRole.Wraith => "Wraith",
        LegacyClient.Info.LegacyRole.GuardianAngel => "GuardianAngel",
        LegacyClient.Info.LegacyRole.Juggernaut => "Juggernaut",
        LegacyClient.Info.LegacyRole.RandomTown => "RandomTown",
        LegacyClient.Info.LegacyRole.TownInvestigative => "TownInvestigative",
        LegacyClient.Info.LegacyRole.TownProtective => "TownProtective",
        LegacyClient.Info.LegacyRole.TownKilling => "TownKilling",
        LegacyClient.Info.LegacyRole.TownSupport => "TownSupport",
        LegacyClient.Info.LegacyRole.TownPower => "TownPower",
        LegacyClient.Info.LegacyRole.RandomCoven => "RandomCoven",
        LegacyClient.Info.LegacyRole.CovenKilling => "CovenKilling",
        LegacyClient.Info.LegacyRole.CovenUtility => "CovenUtility",
        LegacyClient.Info.LegacyRole.CovenDeception => "CovenDeception",
        LegacyClient.Info.LegacyRole.CovenPower => "CovenPower",
        LegacyClient.Info.LegacyRole.RandomNeutral => "RandomNeutral",
        LegacyClient.Info.LegacyRole.NeutralKilling => "NeutralKilling",
        LegacyClient.Info.LegacyRole.NeutralEvil => "NeutralEvil",
        LegacyClient.Info.LegacyRole.NeutralApocalypse => "NeutralApocalypse",
        LegacyClient.Info.LegacyRole.NeutralChaos => "NeutralChaos",
        LegacyClient.Info.LegacyRole.RandomMafia => "RandomMafia",
        LegacyClient.Info.LegacyRole.MafiaDeception => "MafiaDeception",
        LegacyClient.Info.LegacyRole.MafiaKilling => "MafiaKilling",
        LegacyClient.Info.LegacyRole.MafiaPower => "MafiaPower",
        LegacyClient.Info.LegacyRole.MafiaUtility => "MafiaUtility",
        LegacyClient.Info.LegacyRole.AnyEvil => "AnyEvil",
        LegacyClient.Info.LegacyRole.FactionedEvil => "FactionedEvil",
        LegacyClient.Info.LegacyRole.NonTown => "NonTown",
        LegacyClient.Info.LegacyRole.NonCoven => "NonCoven",
        LegacyClient.Info.LegacyRole.NonMafia => "NonMafia",
        LegacyClient.Info.LegacyRole.NonNeutral => "NonNeutral",
        LegacyClient.Info.LegacyRole.Any => "Any",
        LegacyClient.Info.LegacyRole.TrueAny => "TrueAny",
        LegacyClient.Info.LegacyRole.CovenTT => "CovenTownTraitor",
        LegacyClient.Info.LegacyRole.MafiaTT => "MafiaTownTraitor",
        LegacyClient.Info.LegacyRole.GhostTown => "GhostTown",
        LegacyClient.Info.LegacyRole.PerfectTown => "PerfectTown",
        LegacyClient.Info.LegacyRole.SlowMode => "SlowMode",
        LegacyClient.Info.LegacyRole.FastMode => "FastMode",
        LegacyClient.Info.LegacyRole.AnonVotes => "AnonVotes",
        LegacyClient.Info.LegacyRole.SecretKillers => "SecretKillers",
        LegacyClient.Info.LegacyRole.HiddenRoles => "HiddenRoles",
        LegacyClient.Info.LegacyRole.OneTrial => "OneTrial",
        LegacyClient.Info.LegacyRole.TownVEvils => "TownVEvils",
        LegacyClient.Info.LegacyRole.Lovers => "Lovers",
        LegacyClient.Info.LegacyRole.Hidden => "Hidden",
        LegacyClient.Info.LegacyRole.Cleaned => "Cleaned",
        LegacyClient.Info.LegacyRole.Stoned => "Stoned",
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

    public static string FactionName(FactionType faction, ModType mod = ModType.None)
    {
        if (Constants.FactionOverridden)
            return Constants.FactionOverride;

        try
        {
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
        BetterTOS2.FactionTypePlus.TOWN => "Town",
        BetterTOS2.FactionTypePlus.COVEN => "Coven",
        BetterTOS2.FactionTypePlus.SERIALKILLER => "SerialKiller",
        BetterTOS2.FactionTypePlus.ARSONIST => "Arsonist",
        BetterTOS2.FactionTypePlus.WEREWOLF => "Werewolf",
        BetterTOS2.FactionTypePlus.SHROUD => "Shroud",
        BetterTOS2.FactionTypePlus.APOCALYPSE => "Apocalypse",
        BetterTOS2.FactionTypePlus.EXECUTIONER => "Executioner",
        BetterTOS2.FactionTypePlus.JESTER => "Jester",
        BetterTOS2.FactionTypePlus.PIRATE => "Pirate",
        BetterTOS2.FactionTypePlus.DOOMSAYER => "Doomsayer",
        BetterTOS2.FactionTypePlus.VAMPIRE => "Vampire",
        BetterTOS2.FactionTypePlus.CURSEDSOUL => "CursedSoul",
        BetterTOS2.FactionTypePlus.JACKAL => "Jackal",
        BetterTOS2.FactionTypePlus.FROGS => "Frogs",
        BetterTOS2.FactionTypePlus.LIONS => "Lions",
        BetterTOS2.FactionTypePlus.HAWKS => "Hawks",
        BetterTOS2.FactionTypePlus.JUDGE => "Judge",
        BetterTOS2.FactionTypePlus.AUDITOR => "Auditor",
        BetterTOS2.FactionTypePlus.STARSPAWN => "Starspawn",
        BetterTOS2.FactionTypePlus.INQUISITOR => "Inquisitor",
        /*BetterTOS2.FactionTypePlus.EGOIST => "Egoist",
        BetterTOS2.FactionTypePlus.COMPLIANT => "Compliant",
        BetterTOS2.FactionTypePlus.PANDORA => "Pandora",*/
        _ => "Blank"
    };

    /*private static string LegacyFactionName(FactionType faction) => faction switch
    {
        LegacyClient.Info.LegacyFactionType.Amnesiac => "Amnesiac",
        LegacyClient.Info.LegacyFactionType.Town => "Town",
        LegacyClient.Info.LegacyFactionType.Coven => "Coven",
        LegacyClient.Info.LegacyFactionType.Mafia => "Mafia",
        LegacyClient.Info.LegacyFactionType.SerialKiller => "SerialKiller",
        LegacyClient.Info.LegacyFactionType.Arsonist => "Arsonist",
        LegacyClient.Info.LegacyFactionType.Werewolf => "Werewolf",
        LegacyClient.Info.LegacyFactionType.Shroud => "Shroud",
        LegacyClient.Info.LegacyFactionType.Apocalypse => "Apocalypse",
        LegacyClient.Info.LegacyFactionType.Executioner => "Executioner",
        LegacyClient.Info.LegacyFactionType.Jester => "Jester",
        LegacyClient.Info.LegacyFactionType.Pirate => "Pirate",
        LegacyClient.Info.LegacyFactionType.Doomsayer => "Doomsayer",
        LegacyClient.Info.LegacyFactionType.Vampire => "Vampire",
        LegacyClient.Info.LegacyFactionType.CursedSoul => "CursedSoul",
        LegacyClient.Info.LegacyFactionType.Juggernaut => "Juggernaut",
        LegacyClient.Info.LegacyFactionType.GuardianAngel => "GuardianAngel",
        LegacyClient.Info.LegacyFactionType.Evils => "Evils",
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

    public static bool Exists(this List<BaseAbilityButton> list, int index)
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

    public static bool IsApoc(this Role role, ModType mod = ModType.None)
    {
        try
        {
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

    public static bool IsApocBTOS2(this Role role) => role is BetterTOS2.RolePlus.BERSERKER or BetterTOS2.RolePlus.WAR or BetterTOS2.RolePlus.BAKER or BetterTOS2.RolePlus.FAMINE or
        BetterTOS2.RolePlus.SOUL_COLLECTOR or BetterTOS2.RolePlus.DEATH or BetterTOS2.RolePlus.PLAGUEBEARER or BetterTOS2.RolePlus.PESTILENCE;

    /*public static bool IsApocLegacy(this Role role) => role is LegacyClient.Info.LegacyRole.Berserker or LegacyClient.Info.LegacyRole.War or LegacyClient.Info.LegacyRole.Baker or
        LegacyClient.Info.LegacyRole.Famine or LegacyClient.Info.LegacyRole.Reaper or LegacyClient.Info.LegacyRole.Death or LegacyClient.Info.LegacyRole.Plaguebearer or
        LegacyClient.Info.LegacyRole.Pestilence;*/

    public static bool Skippable(string name) => GetGameType() switch
    {
        ModType.BTOS2 => BTOS2SkippableNames.Contains(name),
        _ => VanillaSkippableNames.Contains(name)
    };

    public static (Dictionary<string, string>, Dictionary<string, int>) Filtered(ModType mod = ModType.None)
    {
        var roles = mod switch
        {
            ModType.BTOS2 => typeof(BetterTOS2.RolePlus)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(Role))
                .Select(x => (Role)x.GetRawConstantValue())
                .Where(x => x is not (BetterTOS2.RolePlus.NONE or Role.HANGMAN or BetterTOS2.RolePlus.UNKNOWN or BetterTOS2.RolePlus.ROLE_COUNT)),
            /*ModType.Legacy => typeof(LegacyClient.Info.LegacyRole)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(Role))
                .Select(x => (Role)x.GetRawConstantValue())
                .Where(x => x is not (LegacyClient.Info.LegacyRole.None or LegacyClient.Info.LegacyRole.Hangman or LegacyClient.Info.LegacyRole.Unknown or
                    LegacyClient.Info.LegacyRole.RoleCount)),*/
            _ => ((Role[])Enum.GetValues(typeof(Role))).Where(x => x is not (Role.NONE or Role.ROLE_COUNT or Role.UNKNOWN or Role.HANGMAN))
        };

        var rolesWithIndex = roles.Select(role => (role.ToString().ToLower(), (int)role)).ToDictionary(rolesSelect => rolesSelect.Item1.ToLower(), rolesSelect => rolesSelect.Item2);
        return (rolesWithIndex.ToDictionary(rolesSelect => rolesSelect.Key.ToLower(), rolesSelect => $"Role{rolesSelect.Value}"), rolesWithIndex);
    }

    public static void DumpSprite(Texture2D texture, string fileName, string path = null)
    {
        path ??= AssetManager.ModPath;
        var assetPath = Path.Combine(path, $"{fileName}.png");

        if (File.Exists(assetPath))
            File.Delete(assetPath);

        File.WriteAllBytes(assetPath, texture.Decompress().EncodeToPNG());
    }

    public static bool IsTransformedApoc(this Role role)
    {
        try
        {
            return Constants.IsBTOS2 ? IsTransformedApocBTOS(role) : IsTransformedApocVanilla(role);
        }
        catch
        {
            return IsTransformedApocVanilla(role);
        }
    }

    private static bool IsTransformedApocBTOS(this Role role) => role is BetterTOS2.RolePlus.DEATH or BetterTOS2.RolePlus.FAMINE or BetterTOS2.RolePlus.WAR or BetterTOS2.RolePlus.PESTILENCE;

    private static bool IsTransformedApocVanilla(this Role role) => role is Role.DEATH or Role.FAMINE or Role.WAR or Role.PESTILENCE;

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
        EffectType.PESTILENCE => "Pestilence",
        EffectType.REVEALED_MARSHAL => "RevealedMarshal",
        (EffectType)100 => Constants.IsBTOS2 ? "Recruit" : "Blank",
        (EffectType)101 => Constants.IsBTOS2 ? "Deafened" : "Blank",
        (EffectType)102 => Constants.IsBTOS2 ? "CovenTownTraitor" : "Blank",
        (EffectType)103 => Constants.IsBTOS2 ? "ApocTownTraitor" : "Blank",
        (EffectType)104 => Constants.IsBTOS2 ? "Audited" : "Blank",
        /*(EffectType)100 => Constants.IsBTOS2 ? "Recruit" : (Constants.IsLegacy ? "CovenTownTraitor" : "Blank"),
        (EffectType)101 => Constants.IsBTOS2 ? "Deafened" : (Constants.IsLegacy ? "MafiaTownTraitor": "Blank"),
        (EffectType)102 => Constants.IsBTOS2 ? "CovenTownTraitor" : (Constants.IsLegacy ? "Transported": "Blank"),
        (EffectType)103 => Constants.IsBTOS2 ? "ApocTownTraitor" : (Constants.IsLegacy ? "Enchanted": "Blank"),
        (EffectType)104 => Constants.IsBTOS2 ? "Audited" : (Constants.IsLegacy ? "Reaped": "Blank"),*/
        (EffectType)105 => Constants.IsBTOS2 ? "Enchanted" : "Blank",
        (EffectType)106 => Constants.IsBTOS2 ? "Accompanied" : "Blank",
        (EffectType)107 => Constants.IsBTOS2 ? "PandoraTownTraitor" : "Blank",
        (EffectType)108 => Constants.IsBTOS2 ? "Egoist" : "Blank",
        (EffectType)109 => Constants.IsBTOS2 ? "Reaped" : "Blank",
        _ => "Blank"
    };

    public static ModType GetGameType()
    {
        if (Constants.IsBTOS2)
            return ModType.BTOS2;
        /*else if (Constants.IsLegacy)
            return ModType.Legacy;*/
        else
            return ModType.Vanilla;
    }

    public static bool IsEthereal(this UIRoleData.UIRoleDataInstance ui)
    {
        try
        {
            return BetterTOS2.UIRoleDataExtension.EtherealList.TryGetValue(ui.role, out var eth) && eth;
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
                    BetterTOS2.RolePlus.BAKER => BetterTOS2.RolePlus.FAMINE,
                    BetterTOS2.RolePlus.BERSERKER => BetterTOS2.RolePlus.WAR,
                    BetterTOS2.RolePlus.SOUL_COLLECTOR => BetterTOS2.RolePlus.DEATH,
                    BetterTOS2.RolePlus.PLAGUEBEARER => BetterTOS2.RolePlus.PESTILENCE,
                    _ => role
                },
                /*ModType.Legacy => role switch
                {
                    LegacyClient.Info.LegacyRole.Baker => LegacyClient.Info.LegacyRole.Famine,
                    LegacyClient.Info.LegacyRole.Berserker => LegacyClient.Info.LegacyRole.War,
                    LegacyClient.Info.LegacyRole.Reaper => LegacyClient.Info.LegacyRole.Death,
                    LegacyClient.Info.LegacyRole.Plaguebearer => LegacyClient.Info.LegacyRole.Pestilence,
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
                ModType.BTOS2 => BetterTOS2.RolePlus.NECROMANCER,
                //ModType.Legacy => LegacyClient.Info.LegacyRole.Necromancer,
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
                ModType.BTOS2 => BetterTOS2.RolePlus.WAR,
                //ModType.Legacy => LegacyClient.Info.LegacyRole.War,
                _ => Role.WAR,
            };
        }
        catch
        {
            return Role.WAR;
        }
    }
}