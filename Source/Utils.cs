namespace IconPacks;

public static class Utils
{
    private static readonly string[] SkippableNames = { "Admirer_Ability", "Amnesiac_Ability", "Arsonist_Ability", "Attributes_Coven", "Baker_Ability", "Berserker_Ability",
        "Bodyguard_Ability", "Cleric_Ability", "Coroner_Ability", "CovenLeader_Ability", "Crusader_Ability", "CursedSoul_Ability", "Death_Ability", "Dreamweaver_Ability", "Enchanter_Ability",
        "Executioner_Ability", "Famine_Ability", "HexMaster_Ability", "Illusionist_Ability", "Investigator_Ability", "Jailor_Ability","Jailor_Ability_2", "Jester_Ability", "Jinx_Ability",
        "Lookout_Ability", "Medusa_Ability", "Monarch_Ability", "Necromancer_Ability_1", "Necromancer_Ability_2", "Pestilence_Ability", "Plaguebearer_Ability", "Poisoner_Ability",
        "PotionMaster_Ability_1", "PotionMaster_Ability_2", "Psychic_Ability", "Retributionist_Ability_1", "Retributionist_Ability_2", "Seer_Ability_1", "Seer_Ability_2",
        "SerialKiller_Ability", "Sheriff_Ability", "Shroud_Ability", "SoulCollector_Ability", "Spy_Ability", "TavernKeeper_Ability", "Tracker_Ability", "Trapper_Ability",
        "Trickster_Ability", "Vampire_Ability", "Vigilante_Ability", "VoodooMaster_Ability", "War_Ability_1", "War_Ability_2", "Werewolf_Ability_1", "Werewolf_Ability_2", "Wildling_Ability",
        "Witch_Ability_1", "Witch_Ability_2", "Jailor_Special", "Cleric_Special", "Mayor_Special", "Jester_Special", "Executioner_Special", "Bodyguard_Special", "Veteran_Special",
        "Trapper_Special", "Pirate_Special", "Admirer_Special", "Arsonist_Special", "Marshal_Special", "Socialite_Special", "Poisoner_Special", "CovenLeader_Special", "Coroner_Special",
        "SerialKiller_Special", "Shroud_Special", "Starspawn_Ability", "Banshee_Ability", "Judge_Ability", "Jackal_Ability", "Auditor_Ability", "Inquisitor_Ability",
        "Judge_Special", "Auditor_Special", "Inquisitor_Special", "Starspawn_Special", "Oracle_Special" };

    private static readonly Role[] ExceptRoles = { Role.NONE, Role.ROLE_COUNT, Role.UNKNOWN, Role.HANGMAN };

    //List of roles modified by Dum's mod
    private static readonly Role[] ChangedByToS1UI = { Role.JAILOR, Role.CLERIC, Role.MAYOR, Role.JESTER, Role.EXECUTIONER, Role.BODYGUARD, Role.VETERAN, Role.TRAPPER, Role.PIRATE,
        Role.ADMIRER, Role.ARSONIST, Role.MARSHAL, Role.SOCIALITE, Role.POISONER, Role.COVENLEADER, Role.CORONER, Role.SERIALKILLER, Role.SHROUD, Role.ROLE_COUNT, (Role)57, (Role)58,
        (Role)59, (Role)60, (Role)61 };

    public static T Random<T>(this IEnumerable<T> input)
    {
        var list = input.ToList();
        return list.Count == 0 ? default : list[URandom.Range(0, list.Count)];
    }

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) => source.ToList().ForEach(action);

    public static void ForEach<TKey, TValue>(this IDictionary<TKey, TValue> dict, Action<TKey, TValue> action) => dict.ToList().ForEach(pair => action(pair.Key, pair.Value));

    public static string RoleName(Role role, bool btos = false)
    {
        try
        {
            return Constants.IsBTOS2 || btos ? BTOSRoleName(role) : VanillaRoleName(role);
        }
        catch
        {
            return VanillaRoleName(role);
        }
    }

    private static string BTOSRoleName(Role role) => role switch
    {
        RolePlus.ADMIRER => "Admirer",
        RolePlus.AMNESIAC => "Amnesiac",
        RolePlus.BODYGUARD => "Bodyguard",
        RolePlus.CLERIC => "Cleric",
        RolePlus.CORONER => "Coroner",
        RolePlus.CRUSADER => "Crusader",
        RolePlus.DEPUTY => "Deputy",
        RolePlus.INVESTIGATOR => "Investigator",
        RolePlus.JAILOR => "Jailor",
        RolePlus.LOOKOUT => "Lookout",
        RolePlus.MAYOR => "Mayor",
        RolePlus.MONARCH => "Monarch",
        RolePlus.PROSECUTOR => "Prosecutor",
        RolePlus.PSYCHIC => "Psychic",
        RolePlus.RETRIBUTIONIST => "Retributionist",
        RolePlus.SEER => "Seer",
        RolePlus.SHERIFF => "Sheriff",
        RolePlus.SPY => "Spy",
        RolePlus.TAVERN_KEEPER => "TavernKeeper",
        RolePlus.TRACKER => "Tracker",
        RolePlus.TRAPPER => "Trapper",
        RolePlus.TRICKSTER => "Trickster",
        RolePlus.VETERAN => "Veteran",
        RolePlus.VIGILANTE => "Vigilante",
        RolePlus.CONJURER => "Conjurer",
        RolePlus.COVEN_LEADER => "CovenLeader",
        RolePlus.DREAMWEAVER => "Dreamweaver",
        RolePlus.ENCHANTER => "Enchanter",
        RolePlus.HEX_MASTER => "HexMaster",
        RolePlus.ILLUSIONIST => "Illusionist",
        RolePlus.JINX => "Jinx",
        RolePlus.MEDUSA => "Medusa",
        RolePlus.NECROMANCER => "Necromancer",
        RolePlus.POISONER => "Poisoner",
        RolePlus.POTION_MASTER => "PotionMaster",
        RolePlus.RITUALIST => "Ritualist",
        RolePlus.VOODOO_MASTER => "VoodooMaster",
        RolePlus.WILDLING => "Wildling",
        RolePlus.WITCH => "Witch",
        RolePlus.ARSONIST => "Arsonist",
        RolePlus.BAKER => "Baker",
        RolePlus.BERSERKER => "Berserker",
        RolePlus.DOOMSAYER => "Doomsayer",
        RolePlus.EXECUTIONER => "Executioner",
        RolePlus.JESTER => "Jester",
        RolePlus.PIRATE => "Pirate",
        RolePlus.PLAGUEBEARER => "Plaguebearer",
        RolePlus.SERIAL_KILLER => "SerialKiller",
        RolePlus.SHROUD => "Shroud",
        RolePlus.SOUL_COLLECTOR => "SoulCollector",
        RolePlus.WEREWOLF => "Werewolf",
        RolePlus.FAMINE => "Famine",
        RolePlus.WAR => "War",
        RolePlus.PESTILENCE => "Pestilence",
        RolePlus.DEATH => "Death",
        RolePlus.CURSED_SOUL => "CursedSoul",
        RolePlus.BANSHEE => "Banshee",
        RolePlus.JACKAL => "Jackal",
        RolePlus.MARSHAL => "Marshal",
        RolePlus.JUDGE => "Judge",
        RolePlus.AUDITOR => "Auditor",
        RolePlus.INQUISITOR => "Inquisitor",
        RolePlus.STARSPAWN => "Starspawn",
        RolePlus.ORACLE => "Oracle",
        RolePlus.VAMPIRE => "Vampire",
        RolePlus.STONED => "Stoned",
        RolePlus.RANDOM_TOWN => "Town",
        RolePlus.RANDOM_COVEN => "Coven",
        RolePlus.RANDOM_NEUTRAL => "Neutral",
        RolePlus.TOWN_INVESTIGATIVE => "TownInvestigative",
        RolePlus.TOWN_PROTECTIVE => "TownProtective",
        RolePlus.TOWN_KILLING => "TownKilling",
        RolePlus.TOWN_SUPPORT => "TownSupport",
        RolePlus.TOWN_POWER => "TownPower",
        RolePlus.COVEN_KILLING => "CovenKilling",
        RolePlus.COVEN_UTILITY => "CovenUtility",
        RolePlus.COVEN_DECEPTION => "CovenDeception",
        RolePlus.COVEN_POWER => "CovenPower",
        RolePlus.NEUTRAL_KILLING => "NeutralKilling",
        RolePlus.NEUTRAL_EVIL => "NeutralEvil",
        RolePlus.TRUE_ANY => "TrueAny",
        RolePlus.REGULAR_COVEN => "CommonCoven",
        RolePlus.REGULAR_TOWN => "CommonTown",
        RolePlus.RANDOM_APOCALYPSE => "RandomApocalypse",
        RolePlus.NEUTRAL_PARIAH => "NeutralPariah",
        RolePlus.NEUTRAL_SPECIAL => "NeutralSpecial",
        RolePlus.ANY => "Any",
        RolePlus.COVEN_TOWN_TRAITOR => "CovenTownTraitor",
        RolePlus.APOC_TOWN_TRAITOR => "ApocTownTraitor",
        RolePlus.PERFECT_TOWN => "PerfectTown",
        RolePlus.GHOST_TOWN => "GhostTown",
        RolePlus.VIP => "VIP",
        RolePlus.SLOW_MODE => "SlowMode",
        RolePlus.FAST_MODE => "FastMode",
        RolePlus.ANON_VOTING => "AnonVoting",
        RolePlus.SECRET_KILLERS => "SecretKillers",
        RolePlus.HIDDEN_ROLES => "HiddenRoles",
        RolePlus.ONE_TRIAL => "OneTrial",
        RolePlus.NECRO_PASSING => "NecroPass",
        RolePlus.TEAMS => "Teams",
        RolePlus.ANON_PLAYERS => "AnonNames",
        RolePlus.WALKING_DEAD => "WalkingDead",
        RolePlus.HIDDEN => "Hidden",
        _ => "Blank"
    };

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

    public static string FactionName(FactionType faction, bool btos = false)
    {
        try
        {
            return Constants.FactionOverriden ? Constants.FactionOverride : (Constants.IsBTOS2 || btos ? BTOSFactionName(faction) : VanillaFactionName(faction));
        }
        catch
        {
            return VanillaFactionName(faction);
        }
    }

    private static string BTOSFactionName(FactionType faction) => faction switch
    {
        FactionTypePlus.TOWN => "Town",
        FactionTypePlus.COVEN => "Coven",
        FactionTypePlus.SERIALKILLER => "SerialKiller",
        FactionTypePlus.ARSONIST => "Arsonist",
        FactionTypePlus.WEREWOLF => "Werewolf",
        FactionTypePlus.SHROUD => "Shroud",
        FactionTypePlus.APOCALYPSE => "Apocalypse",
        FactionTypePlus.EXECUTIONER => "Executioner",
        FactionTypePlus.JESTER => "Jester",
        FactionTypePlus.PIRATE => "Pirate",
        FactionTypePlus.DOOMSAYER => "Doomsayer",
        FactionTypePlus.VAMPIRE => "Vampire",
        FactionTypePlus.CURSEDSOUL => "CursedSoul",
        FactionTypePlus.JACKAL => "Jackal",
        FactionTypePlus.FROGS => "Frogs",
        FactionTypePlus.LIONS => "Lions",
        FactionTypePlus.HAWKS => "Hawks",
        FactionTypePlus.JUDGE => "Judge",
        FactionTypePlus.AUDITOR => "Auditor",
        FactionTypePlus.STARSPAWN => "Starspawn",
        FactionTypePlus.INQUISITOR => "Inquisitor",
        _ => "Blank"
    };

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

    public static bool IsApoc(this Role role, bool btos = false)
    {
        try
        {
            return Constants.IsBTOS2 || btos ? IsApocBTos(role) : IsApocVanilla(role);
        }
        catch
        {
            return IsApocVanilla(role);
        }
    }

    public static bool IsApocVanilla(this Role role) => role is Role.BERSERKER or Role.WAR or Role.BAKER or Role.FAMINE or Role.SOULCOLLECTOR or Role.DEATH or Role.PLAGUEBEARER or Role.PESTILENCE;

    public static bool IsApocBTos(this Role role) => role is RolePlus.BERSERKER or RolePlus.WAR or RolePlus.BAKER or RolePlus.FAMINE or RolePlus.SOUL_COLLECTOR or RolePlus.DEATH or RolePlus.PLAGUEBEARER or RolePlus.PESTILENCE;

    public static bool Skippable(string name) => SkippableNames.Contains(name);

    public static (Dictionary<string, string>, Dictionary<string, int>) Filtered(bool vanilla = true)
    {
        // these roles dont have sprites so just ignore them
        var roles = !vanilla ? typeof(RolePlus)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(Role))
            .Select(x => (Role)x.GetRawConstantValue())
            .Where(x => x is not (RolePlus.NONE or Role.HANGMAN or RolePlus.UNKNOWN or RolePlus.ROLE_COUNT)) :
            ((Role[])Enum.GetValues(typeof(Role))).Except(ExceptRoles);

        // map all roles to (role name, role number) so we can make a dict
        var rolesWithIndex = roles.Select(role => (role.ToString().ToLower(), (int)role)).ToDictionary(rolesSelect => rolesSelect.Item1.ToLower(), rolesSelect => rolesSelect.Item2);

        // dict allows us to find dict[rolename.tolower] and get Role{number} for later use in spritecharacters
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

    private static bool IsTransformedApocBTOS(this Role role) => role is RolePlus.DEATH or RolePlus.FAMINE or RolePlus.WAR or RolePlus.PESTILENCE;

    private static bool IsTransformedApocVanilla(this Role role) => role is Role.DEATH or Role.FAMINE or Role.WAR or Role.PESTILENCE;
}