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

    public static bool IsApocBTOS2(this Role role) => role is BTOS2Role.Berserker or BTOS2Role.War or BTOS2Role.Baker or BTOS2Role.Famine or BTOS2Role.SoulCollector or BTOS2Role.Death or
        BTOS2Role.Plaguebearer or BTOS2Role.Pestilence;

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
            ModType.BTOS2 => typeof(BTOS2Role)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(Role))
                .Select(x => (Role)x.GetRawConstantValue())
                .Where(x => x is not (BTOS2Role.None or BTOS2Role.Hangman or BTOS2Role.Unknown or BTOS2Role.RoleCount)),
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

    private static bool IsTransformedApocBTOS(this Role role) => role is BTOS2Role.Death or BTOS2Role.Famine or BTOS2Role.War or BTOS2Role.Pestilence;

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
                ModType.BTOS2 => BTOS2Role.Necromancer,
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
                ModType.BTOS2 => BTOS2Role.War,
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