namespace IconPacks;

public static class Utils
{
    private static readonly List<string> SkippableNames = new() { "Admirer_Ability", "Amnesiac_Ability", "Arsonist_Ability", "Attributes_Coven", "Baker_Ability", "Berserker_Ability",
        "Bodyguard_Ability", "Cleric_Ability", "Coroner_Ability", "CovenLeader_Ability", "Crusader_Ability", "CursedSoul_Ability", "Death_Ability", "Dreamweaver_Ability", "Enchanter_Ability",
        "Executioner_Ability", "Famine_Ability", "HexMaster_Ability", "Illusionist_Ability", "Investigator_Ability", "Jailor_Ability","Jailor_Ability_2", "Jester_Ability", "Jinx_Ability",
        "Lookout_Ability", "Medusa_Ability", "Monarch_Ability", "Necromancer_Ability_1", "Necromancer_Ability_2", "Pestilence_Ability", "Plaguebearer_Ability", "Poisoner_Ability",
        "PotionMaster_Ability_1", "PotionMaster_Ability_2", "Psychic_Ability", "Retributionist_Ability_1", "Retributionist_Ability_2", "Seer_Ability_1", "Seer_Ability_2",
        "SerialKiller_Ability", "Sheriff_Ability", "Shroud_Ability", "SoulCollector_Ability", "Spy_Ability", "TavernKeeper_Ability", "Tracker_Ability", "Trapper_Ability",
        "Trickster_Ability", "Vampire_Ability", "Vigilante_Ability", "VoodooMaster_Ability", "War_Ability_1", "War_Ability_2", "Werewolf_Ability_1", "Werewolf_Ability_2", "Wildling_Ability",
        "Witch_Ability_1", "Witch_Ability_2", "Jailor_Special", "Cleric_Special", "Mayor_Special", "Jester_Special", "Executioner_Special", "Bodyguard_Special", "Veteran_Special",
        "Trapper_Special", "Pirate_Special", "Admirer_Special", "Arsonist_Special" };

    public static readonly Role[] ExceptRoles = { Role.NONE, Role.ROLE_COUNT, Role.UNKNOWN, Role.HANGMAN };

    public static T Random<T>(this IEnumerable<T> input, T defaultVal = default)
    {
        var list = input.ToList();
        return list.Count == 0 ? defaultVal : list[URandom.Range(0, list.Count)];
    }

    //I need an list of roles modified by my mod
    private static readonly Role[] ChangedByToS1UI = new[] { Role.JAILOR, Role.CLERIC, Role.MAYOR, Role.JESTER, Role.EXECUTIONER, Role.BODYGUARD, Role.VETERAN, Role.TRAPPER, Role.PIRATE,
        Role.ADMIRER, Role.ARSONIST };

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) => source.ToList().ForEach(action);

    public static void ForEach<TKey, TValue>(this IDictionary<TKey, TValue> dict, Action<TKey, TValue> action) => dict.ToList().ForEach(pair => action(pair.Key, pair.Value));

    public static string RoleName(Role role) => role switch
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
        Role.MAYOR => "Mayor",
        Role.MONARCH => "Monarch",
        Role.PROSECUTOR => "Prosecutor",
        Role.PSYCHIC => "Psychic",
        Role.RETRIBUTIONIST => "Retributionist",
        Role.SEER => "Seer",
        Role.SHERIFF => "Sheriff",
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
        Role.NO_TOWN_HANGED => "NoTownHanged",
        Role.GHOST_TOWN => "GhostTown",
        Role.VIP => "VIP",
        Role.SLOW_MODE => "SlowMode",
        Role.FAST_MODE => "FastMode",
        Role.ANONYMOUS_VOTES => "AnonVotes",
        Role.KILLER_ROLES_HIDDEN => "HiddenKillers",
        Role.ROLES_ON_DEATH_HIDDEN => "HiddenRoles",
        Role.ONE_TRIAL_PER_DAY => "OneTrial",
        Role.HIDDEN => "Hidden",
        _ => "Blank"
    };

    public static string FactionName(FactionType faction, Role? role = null)
    {
        role ??= Pepper.GetMyRole();

        return faction switch
        {
            FactionType.TOWN => "Town",
            FactionType.COVEN => "Coven",
            FactionType.SERIALKILLER => "SerialKiller",
            FactionType.ARSONIST => "Arsonist",
            FactionType.WEREWOLF => "Werewolf",
            FactionType.SHROUD => "Shroud",
            FactionType.APOCALYPSE => role is Role.SOULCOLLECTOR or Role.BAKER or Role.PLAGUEBEARER or Role.BERSERKER ? "Apocalypse" : "Horsemen",
            FactionType.EXECUTIONER => "Executioner",
            FactionType.JESTER => "Jester",
            FactionType.PIRATE => "Pirate",
            FactionType.DOOMSAYER => "Doomsayer",
            FactionType.VAMPIRE => "Vampire",
            FactionType.CURSED_SOUL => "CursedSoul",
            _ => "Blank"
        };
    }

    public static bool ModifiedByToS1UI(Role role) => ChangedByToS1UI.Contains(role);

    public static void SaveLogs()
    {
        try
        {
            File.WriteAllText(Path.Combine(AssetManager.ModPath, "IconPackLogs.txt"), Recolors.SavedLogs);
        }
        catch
        {
            Recolors.LogError("Unable to save logs");
        }
    }

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
        _ => "None"
    };

    public static bool IsApoc(this Role role) => role is Role.BERSERKER or Role.WAR or Role.BAKER or Role.FAMINE or Role.SOULCOLLECTOR or Role.DEATH or Role.PLAGUEBEARER or Role.PESTILENCE;

    public static bool Skippable(string name) => SkippableNames.Contains(name);

    public static (Dictionary<string, string>, IEnumerable<(string, int)>) Filtered()
    {
        // these roles dont have sprites so just ignore them
        var roles = ((Role[])Enum.GetValues(typeof(Role))).Except(ExceptRoles);

        // map all roles to (role name, role number) so we can make a dict
        var rolesWithIndex = roles.Select(role => (role.ToString().ToLower(), (int)role));

        // dict allows us to find dict[rolename.tolower] and get Role{number} for later use in spritecharacters
        return (rolesWithIndex.ToDictionary(rolesSelect => rolesSelect.Item1.ToLower(), rolesSelect => $"Role{rolesSelect.Item2}"), rolesWithIndex);
    }
}