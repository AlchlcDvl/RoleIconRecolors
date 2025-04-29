// using FancyUI.Assets.SilhouetteSwapper;
using Home.Shared;

namespace FancyUI;

public static class Utils
{
    private static readonly string[] VanillaSkippableNames = ["Baker_Ability", "Pirate_Ability"];
    private static readonly string[] BTOS2SkippableNames = [ "Baker_Ability_1", "Baker_Ability_2", "Jackal_Ability", "Auditor_Ability", "Inquisitor_Ability", "Banshee_Ability", "Judge_Ability",
        "Warlock_Ability", "Wildling_Ability_2", "Starspawn_Ability", "Attributes_Pandora" ];
    private static readonly string[] CommonSkippableNames = [ "Admirer_Ability", "Amnesiac_Ability", "Arsonist_Ability", "Attributes_Coven", "Berserker_Ability", "Bodyguard_Ability",
        "Cleric_Ability", "Coroner_Ability", "Crusader_Ability", "CursedSoul_Ability", "Death_Ability", "Dreamweaver_Ability", "Enchanter_Ability", "Famine_Ability", "HexMaster_Ability",
        "Illusionist_Ability", "Investigator_Ability", "Jailor_Ability", "Jester_Ability", "Jinx_Ability", "Lookout_Ability", "Medusa_Ability", "Monarch_Ability", "Seer_Ability_1",
        "Pestilence_Ability", "Plaguebearer_Ability", "Poisoner_Ability", "PotionMaster_Ability_1", "PotionMaster_Ability_2", "Psychic_Ability", "War_Ability_1", "Werewolf_Ability_2",
        "Seer_Ability_2", "SerialKiller_Ability", "Sheriff_Ability", "Shroud_Ability", "SoulCollector_Ability", "Trickster_Ability_1", "Trickster_Ability_2", "Spy_Ability", "Wildling_Ability",
        "TavernKeeper_Ability", "Tracker_Ability", "Trapper_Ability", "Vampire_Ability", "Vigilante_Ability", "VoodooMaster_Ability", "War_Ability_2", "Witch_Ability_1", "Witch_Ability_2",
        "Werewolf_Ability_1" ];

    private static readonly Dictionary<GameModType, (Dictionary<string, string>, Dictionary<string, int>)> RoleStuff = [];

    private static readonly int Color1 = Shader.PropertyToID("_Color");
    private static readonly int Vanilla = Shader.PropertyToID("_Vanilla");
    private static readonly int Brightness = Shader.PropertyToID("_Brightness");
    private static readonly int GrayscaleAmount = Shader.PropertyToID("_GrayscaleAmount");

    public static string RoleName(Role role, GameModType? mod = null)
    {
        try
        {
            return (mod ?? GetGameType()) switch
            {
                GameModType.BTOS2 => BTOSRoleName(role),
                _ => VanillaRoleName(role)
            };
        }
        catch
        {
            return VanillaRoleName(role);
        }
    }

    private static string BTOSRoleName(Role role) => role switch
    {
        Btos2Role.Admirer => "Admirer",
        Btos2Role.Amnesiac => "Amnesiac",
        Btos2Role.Bodyguard => "Bodyguard",
        Btos2Role.Cleric => "Cleric",
        Btos2Role.Coroner => "Coroner",
        Btos2Role.Crusader => "Crusader",
        Btos2Role.Deputy => "Deputy",
        Btos2Role.Investigator => "Investigator",
        Btos2Role.Jailor => "Jailor",
        Btos2Role.Lookout => "Lookout",
        Btos2Role.Mayor => "Mayor",
        Btos2Role.Monarch => "Monarch",
        Btos2Role.Prosecutor => "Prosecutor",
        Btos2Role.Psychic => "Psychic",
        Btos2Role.Retributionist => "Retributionist",
        Btos2Role.Seer => "Seer",
        Btos2Role.Sheriff => "Sheriff",
        Btos2Role.Spy => "Spy",
        Btos2Role.TavernKeeper => "TavernKeeper",
        Btos2Role.Tracker => "Tracker",
        Btos2Role.Trapper => "Trapper",
        Btos2Role.Trickster => "Trickster",
        Btos2Role.Veteran => "Veteran",
        Btos2Role.Vigilante => "Vigilante",
        Btos2Role.Conjurer => "Conjurer",
        Btos2Role.CovenLeader => "CovenLeader",
        Btos2Role.Dreamweaver => "Dreamweaver",
        Btos2Role.Enchanter => "Enchanter",
        Btos2Role.HexMaster => "HexMaster",
        Btos2Role.Illusionist => "Illusionist",
        Btos2Role.Jinx => "Jinx",
        Btos2Role.Medusa => "Medusa",
        Btos2Role.Necromancer => "Necromancer",
        Btos2Role.Poisoner => "Poisoner",
        Btos2Role.PotionMaster => "PotionMaster",
        Btos2Role.Ritualist => "Ritualist",
        Btos2Role.VoodooMaster => "VoodooMaster",
        Btos2Role.Wildling => "Wildling",
        Btos2Role.Witch => "Witch",
        Btos2Role.Arsonist => "Arsonist",
        Btos2Role.Baker => "Baker",
        Btos2Role.Berserker => "Berserker",
        Btos2Role.Doomsayer => "Doomsayer",
        Btos2Role.Executioner => "Executioner",
        Btos2Role.Jester => "Jester",
        Btos2Role.Pirate => "Pirate",
        Btos2Role.Plaguebearer => "Plaguebearer",
        Btos2Role.SerialKiller => "SerialKiller",
        Btos2Role.Shroud => "Shroud",
        Btos2Role.SoulCollector => "SoulCollector",
        Btos2Role.Werewolf => "Werewolf",
        Btos2Role.Famine => "Famine",
        Btos2Role.War => "War",
        Btos2Role.Pestilence => "Pestilence",
        Btos2Role.Death => "Death",
        Btos2Role.CursedSoul => "CursedSoul",
        Btos2Role.Banshee => "Banshee",
        Btos2Role.Jackal => "Jackal",
        Btos2Role.Marshal => "Marshal",
        Btos2Role.Judge => "Judge",
        Btos2Role.Auditor => "Auditor",
        Btos2Role.Inquisitor => "Inquisitor",
        Btos2Role.Starspawn => "Starspawn",
        Btos2Role.Oracle => "Oracle",
        Btos2Role.Warlock => "Warlock",
        Btos2Role.Vampire => "Vampire",
        Btos2Role.Stoned => "Stoned",
        Btos2Role.RandomTown => "Town",
        Btos2Role.RandomCoven => "Coven",
        Btos2Role.RandomNeutral => "Neutral",
        Btos2Role.TownInvestigative => "TownInvestigative",
        Btos2Role.TownProtective => "TownProtective",
        Btos2Role.TownKilling => "TownKilling",
        Btos2Role.TownSupport => "TownSupport",
        Btos2Role.TownPower => "TownPower",
        Btos2Role.CovenKilling => "CovenKilling",
        Btos2Role.CovenUtility => "CovenUtility",
        Btos2Role.CovenDeception => "CovenDeception",
        Btos2Role.CovenPower => "CovenPower",
        Btos2Role.NeutralKilling => "NeutralKilling",
        Btos2Role.NeutralEvil => "NeutralEvil",
        Btos2Role.TrueAny => "TrueAny",
        Btos2Role.CommonCoven => "CommonCoven",
        Btos2Role.CommonTown => "CommonTown",
        Btos2Role.RandomApocalypse => "RandomApocalypse",
        Btos2Role.NeutralPariah => "NeutralPariah",
        Btos2Role.NeutralSpecial => "NeutralSpecial",
        Btos2Role.Any => "Any",
        Btos2Role.CovenTownTraitor => "CovenTownTraitor",
        Btos2Role.ApocTownTraitor => "ApocTownTraitor",
        Btos2Role.PerfectTown => "PerfectTown",
        Btos2Role.GhostTown => "GhostTown",
        Btos2Role.Vip => "VIP",
        Btos2Role.SlowMode => "SlowMode",
        Btos2Role.FastMode => "FastMode",
        Btos2Role.AnonVoting => "AnonVoting",
        Btos2Role.SecretKillers => "SecretKillers",
        Btos2Role.HiddenRoles => "HiddenRoles",
        Btos2Role.OneTrial => "OneTrial",
        Btos2Role.NecroPass => "NecroPass",
        Btos2Role.Teams => "Teams",
        Btos2Role.AnonNames => "AnonNames",
        Btos2Role.WalkingDead => "WalkingDead",
        Btos2Role.Egotist => "Egotist",
        Btos2Role.SpeakingSpirits => "SpeakingSpirits",
        Btos2Role.SecretObjectives => "SecretObjectives",
        Btos2Role.NoLastWills => "NoLastWills",
        Btos2Role.Immovable => "Immovable",
        Btos2Role.CompliantKillers => "CompliantKillers",
        Btos2Role.PandorasBox => "PandorasBox",
        Btos2Role.BallotVoting => "BallotVoting",
        Btos2Role.Individuality => "Individuality",
        Btos2Role.Snitch => "Snitch",
        Btos2Role.CovenVip => "CovenVIP",
        Btos2Role.SecretWhispers => "SecretWhispers",
        Btos2Role.Hidden => "Hidden",
        Btos2Role.CommonNeutral => "CommonNeutral",
        Btos2Role.Vc => "VC",
        Btos2Role.Lovers => "Lovers",
        Btos2Role.FeelinLucky => "FeelinLucky",
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
        Role.ORACLE => "Oracle",
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
        Role.COMMON_COVEN => "CommonCoven",
        Role.COMMON_TOWN => "CommonTown",
        Role.FOUR_HORSEMEN => "4Horsemen",
        Role.ANON_PLAYERS => "AnonPlayers",
        Role.FEELIN_LUCKY => "FeelinLucky",
        Role.HIDDEN => "Hidden",
        _ => "Blank"
    };

    public static string FactionName(FactionType faction, bool allowOverrides, GameModType? mod = null) => FactionName(faction, mod, allowOverrides);

    public static string FactionName(FactionType faction, GameModType? mod = null, bool allowOverrides = true)
    {
        if (Constants.FactionOverridden() && allowOverrides)
            return Constants.FactionOverride();

        try
        {
            return (mod ?? GetGameType()) switch
            {
                GameModType.BTOS2 => BTOSFactionName(faction),
                _ => VanillaFactionName(faction)
            };
        }
        catch
        {
            return VanillaFactionName(faction);
        }
    }

    private static string BTOSFactionName(FactionType faction) => faction switch
    {
        Btos2Faction.Town => "Town",
        Btos2Faction.Coven => "Coven",
        Btos2Faction.SerialKiller => "SerialKiller",
        Btos2Faction.Arsonist => "Arsonist",
        Btos2Faction.Werewolf => "Werewolf",
        Btos2Faction.Shroud => "Shroud",
        Btos2Faction.Apocalypse => "Apocalypse",
        Btos2Faction.Executioner => "Executioner",
        Btos2Faction.Jester => "Jester",
        Btos2Faction.Pirate => "Pirate",
        Btos2Faction.Doomsayer => "Doomsayer",
        Btos2Faction.Vampire => "Vampire",
        Btos2Faction.CursedSoul => "CursedSoul",
        Btos2Faction.Jackal => "Jackal",
        Btos2Faction.Frogs => "Frogs",
        Btos2Faction.Lions => "Lions",
        Btos2Faction.Hawks => "Hawks",
        Btos2Faction.Judge => "Judge",
        Btos2Faction.Auditor => "Auditor",
        Btos2Faction.Starspawn => "Starspawn",
        Btos2Faction.Inquisitor => "Inquisitor",
        Btos2Faction.Egotist => "Egotist",
        Btos2Faction.Pandora => "Pandora",
        Btos2Faction.Compliance => "Compliance",
        Btos2Faction.Lovers => "Lovers",
        _ => "Factionless"
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
        _ => "Factionless"
    };

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

    public static bool Skippable(string name)
    {
        if (CommonSkippableNames.Contains(name))
            return true;

        return GetGameType() switch
        {
            GameModType.BTOS2 => BTOS2SkippableNames.Contains(name),
            _ => VanillaSkippableNames.Contains(name)
        };
    }

    public static (Dictionary<string, string>, Dictionary<string, int>) Filtered(GameModType mod = GameModType.Vanilla)
    {
        if (RoleStuff.TryGetValue(mod, out var result))
            return result;

        var roles = mod switch
        {
            GameModType.BTOS2 => AccessTools.GetDeclaredFields(typeof(Btos2Role))
                .Select(x => (Role)x.GetRawConstantValue())
                .Where(x => x is not (Btos2Role.None or Btos2Role.Hangman or Btos2Role.Unknown or Btos2Role.RoleCount)),
            _ => GeneralUtils.GetEnumValues<Role>()!.Where(x => x is not (Role.NONE or Role.ROLE_COUNT or Role.UNKNOWN or Role.HANGMAN))
        };
        var dict = roles.ToDictionary(x => x.ToString(), x => (int)x);
        return RoleStuff[mod] = (dict.ToDictionary(x => x.Key, x => $"Role{x.Value}"), dict);
    }

    public static void DumpSprite(Texture2D texture, string fileName = null, string path = null, bool decompress = false, bool skipSetting = false)
    {
        if (!texture || (!Constants.DumpSheets() && !skipSetting))
            return;

        path ??= Fancy.Instance.ModPath;
        fileName ??= texture.name;
        var assetPath = Path.Combine(path, $"{fileName}.png");

        if (File.Exists(assetPath))
            File.Delete(assetPath);

        File.WriteAllBytes(assetPath, (decompress ? AssetManager.Decompress(texture) : texture).EncodeToPNG());
    }

    public static bool IsTransformedApoc(this Role role, GameModType? mod = null)
    {
        try
        {
            return (mod ?? GetGameType()) switch
            {
                GameModType.BTOS2 => IsTransformedApocBTOS(role),
                _ => IsTransformedApocVanilla(role),
            };
        }
        catch
        {
            return IsTransformedApocVanilla(role);
        }
    }

    private static bool IsTransformedApocBTOS(this Role role) => role is Btos2Role.Death or Btos2Role.Famine or Btos2Role.War or Btos2Role.Pestilence;

    private static bool IsTransformedApocVanilla(this Role role) => role is Role.DEATH or Role.FAMINE or Role.WAR or Role.PESTILENCE;

    public static bool IsValid(this Sprite sprite) => sprite && sprite != Blank;

    public static string EffectName(EffectType effect) => effect switch
    {
        // Vanilla
        EffectType.EXECUTIONER_TARGET => "ExeTarget",
        EffectType.HEXED => "Hexed",
        EffectType.KNIGHTED => "Knighted",
        EffectType.REVEALED_MAYOR => "RevealedMayor",
        EffectType.DISCONNECTED => "Disconnected",
        EffectType.CONNECTING => "Connecting",
        EffectType.LOVER => "Lover",
        EffectType.DOUSED => "Doused",
        EffectType.PLAGUED => "Plagued",
        EffectType.NECRONOMICON => "NecroHolder",
        EffectType.TRAPPED => "Trapped",
        EffectType.BREAD => "Bread",
        EffectType.HANGOVER => "Hangover",
        EffectType.VOODOOED => "Silenced",
        EffectType.DREAMWOVE => "Dreamwoven",
        EffectType.INSANE => "Insomniac",
        EffectType.VIP => "VIP",
        EffectType.BUG => "Bugged",
        EffectType.SCENT_TRACK => "Tracked",
        EffectType.PESTILENCE => "Sickness",
        EffectType.REVEALED_MARSHAL => "RevealedMarshal",
        EffectType.SOCIALITE_GUEST => "Guest",
        EffectType.REAPED or (EffectType)109 => "Reaped",
        EffectType.REVEALED_BY_PMWITCH => "Illuminated",

        // BTOS2
        (EffectType)100 => "Recruit",
        (EffectType)101 => "Deafened",
        (EffectType)102 => "CovenTownTraitor",
        (EffectType)103 => "ApocTownTraitor",
        (EffectType)104 => "Audited",
        (EffectType)105 => "Enchanted",
        (EffectType)106 => "Accompanied",
        (EffectType)107 => "PandoraTownTraitor",
        (EffectType)108 => "Egotist",
        (EffectType)110 => "WarlockCursed",
        (EffectType)111 => "Bestowed",
        (EffectType)112 => "Sensed",
        _ => "Blank"
    };

    public static GameModType GetGameType()
    {
        if (Constants.IsBTOS2() || FindCasualQueue())
            return GameModType.BTOS2;

        return GameModType.Vanilla;
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

    private static bool FindCasualQueueBypass() => Constants.BTOS2Exists() && BetterTOS2.BTOSInfo.CasualModeController;

    public static bool IsEthereal(this UIRoleData.UIRoleDataInstance ui)
    {
        try
        {
            return Constants.IsBTOS2() && ui.role is Btos2Role.Judge or Btos2Role.Auditor or Btos2Role.Starspawn;
        }
        catch
        {
            return false;
        }
    }

    public static Role GetTransformedVersion(Role role, GameModType? mod = null)
    {
        try
        {
            mod ??= GetGameType();
            return mod switch
            {
                GameModType.BTOS2 => role switch
                {
                    Btos2Role.Baker => Btos2Role.Famine,
                    Btos2Role.Berserker => Btos2Role.War,
                    Btos2Role.SoulCollector or Btos2Role.Warlock => Btos2Role.Death,
                    Btos2Role.Plaguebearer => Btos2Role.Pestilence,
                    _ => role
                },
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

    public static Role GetWar(GameModType? mod = null)
    {
        try
        {
            mod ??= GetGameType();
            return mod switch
            {
                GameModType.BTOS2 => Btos2Role.War,
                _ => Role.WAR,
            };
        }
        catch
        {
            return Role.WAR;
        }
    }

    public static FactionType GetFactionType(this Role role, GameModType? mod = null) => ((int)role, role, mod ?? GetGameType()) switch
    {
        // Basic faction checks based on role ID ranges
        ( > 0 and < 25, _, _) => FactionType.TOWN,
        ( > 24 and < 40, _, _) => FactionType.COVEN,
        (41 or 42 or 47 or 50 or 250 or 251 or 252 or 253, _, _) => FactionType.APOCALYPSE,

        // Individual role checks for IDs < 54
        ( < 54, Role.ARSONIST, _) => FactionType.ARSONIST,
        ( < 54, Role.DOOMSAYER, _) => FactionType.DOOMSAYER,
        ( < 54, Role.EXECUTIONER, _) => FactionType.EXECUTIONER,
        ( < 54, Role.JESTER, _) => FactionType.JESTER,
        ( < 54, Role.PIRATE, _) => FactionType.PIRATE,
        ( < 54, Role.SERIALKILLER, _) => FactionType.SERIALKILLER,
        ( < 54, Role.SHROUD, _) => FactionType.SHROUD,
        ( < 54, Role.WEREWOLF, _) => FactionType.WEREWOLF,
        ( < 54, Role.VAMPIRE, _) => FactionType.VAMPIRE,
        ( < 54, Role.CURSED_SOUL, _) => FactionType.CURSED_SOUL,

        // BTOS2 specific role checks
        (_, Btos2Role.Banshee, GameModType.BTOS2) => Btos2Faction.Coven,
        (_, Btos2Role.Marshal or Btos2Role.Oracle, GameModType.BTOS2) => Btos2Faction.Town,
        (_, Btos2Role.Jackal, GameModType.BTOS2) => Btos2Faction.Jackal,
        (_, Btos2Role.Judge, GameModType.BTOS2) => Btos2Faction.Judge,
        (_, Btos2Role.Auditor, GameModType.BTOS2) => Btos2Faction.Auditor,
        (_, Btos2Role.Inquisitor, GameModType.BTOS2) => Btos2Faction.Inquisitor,
        (_, Btos2Role.Starspawn, GameModType.BTOS2) => Btos2Faction.Starspawn,
        (_, Btos2Role.Warlock, GameModType.BTOS2) => Btos2Faction.Apocalypse,

        // Vanilla specific role checks
        ( > 53 and < 57, _, GameModType.Vanilla) => FactionType.TOWN,

        // Default case
        _ => FactionType.NONE
    };

    public static void SetImageColor(this Image img, ColorType type, bool notGuide = true)
    {
        var hasMask = img.TryGetComponent<Mask>(out var mask);

        if (hasMask)
            mask.enabled = false;

        img.material = Constants.AllMaterials[notGuide][type];

        if (hasMask)
            mask.enabled = true;
    }

    public static void SetGraphicColor(this Graphic graphic, ColorType type, FactionType? faction = null, bool flip = true)
    {
        var color2 = Constants.GetUIThemeColor(type, faction);

        if (color2 == Color.clear)
            return;

        color2 = color2.ShadeColor(type, flip, false);
        graphic.color = color2;
    }

    public static void UpdateMaterials(bool notGuide = true, FactionType? faction = null, bool flip = false, bool skipFactionCheck = false)
    {
        if (Constants.GetMainUIThemeType() == UITheme.Vanilla)
            Constants.AllMaterials[notGuide].Values.ForEach(x => x.SetFloat(Vanilla, 1));
        else
        {
            var brightness = Constants.GeneralBrightness();
            var effectiveness = Constants.GrayscaleAmount();

            foreach (var (type, mat) in Constants.AllMaterials[notGuide])
            {
                var color = Constants.GetUIThemeColor(type, faction, skipFactionCheck).ShadeColor(type, flip);

                if (color == Color.clear)
                {
                    mat.SetFloat(Vanilla, 1);
                    continue;
                }

                mat.SetColor(Color1, color);
                mat.SetFloat(Brightness, brightness);
                mat.SetFloat(GrayscaleAmount, effectiveness);
                mat.SetFloat(Vanilla, 0);
            }
        }
    }

    private static Color ShadeColor(this Color color, ColorType type, bool flip = false, bool isImage = true)
    {
        if (color == Color.clear)
            return color;

        var shade = Fancy.ColorShadeMap[type].Value;

        if (shade == 0f && !isImage)
            shade = 50f;

        if (flip)
            shade = -shade;

        return color.ShadeColor(shade / 100f);
    }

    // public static bool IsValid(this SilhouetteAnimation anim) => anim != null && anim != Loading;

    public static bool GetRoleAndFaction(int pos, out Tuple<Role, FactionType> tuple) => Service.Game.Sim.simulation.knownRolesAndFactions.Data.TryGetValue(pos, out tuple);

    public static string ToFactionString(this Role role, FactionType faction)
    {
        if (Constants.MiscRoleExists())
        {
            try
            {
                return role.MrcDisplayString(faction);
            }
            catch { }
        }

        var result = role.ToDisplayString();

        if (role.GetFactionType() != faction)
            result += $" ({faction.ToDisplayString()})";

        return result;
    }

    public static string ToColorizedFactionStringParentheses(this Role role, FactionType faction)
    {
        if (Constants.MiscRoleExists())
        {
            try
            {
                return role.MrcDisplayStringParentheses(faction);
            }
            catch { }
        }

        return ("(" + role.ToDisplayString() + ")").ApplyFactionColor(faction);
    }

    private static string MrcDisplayString(this Role role, FactionType faction) => ModSettings.GetBool("Faction-Specific Role Names", "det.rolecustomizationmod") ?
        MiscRoleCustomisation.Utils.ToRoleFactionDisplayString(role, faction) : role.ToDisplayString();

    private static string MrcDisplayStringParentheses(this Role role, FactionType faction)
    {
        var changedGradient = MiscRoleCustomisation.GetChangedGradients.GetChangedGradient(faction, role);
        var result = "(" + MrcDisplayString(role, faction) + ")";
        return changedGradient != null ? ApplyGradient(result, changedGradient) : result.ApplyFactionColor(faction);
    }

    // public static void DebugSingleTransform(this Transform transform)
    // {
    //     var debug = $"[{transform.parent?.name ?? "None"} -> {transform.name}] = {{ ";
    //     transform.GetComponents(typeof(Component)).ForEach(x => debug += $"{x.GetType().Name}, ");
    //     Fancy.Instance.Fatal(debug + "}");
    // }

    // public static void DebugTransformRecursive(this Transform transform)
    // {
    //     transform.DebugSingleTransform();
    //
    //     for (var i = 0; i < transform.childCount; i++)
    //         transform.GetChild(i).DebugTransformRecursive();
    // }

    public static string ApplyGradient(string text, params Color32[] colors)
    {
        var gradient = new Gradient();
        gradient.SetKeys([.. colors.Select((i, color) => new GradientColorKey(color, (float)i / (colors.Length - 1)))], // Pays to know random ass math (that may or may not be 100% accurate)
        [
            new(1f, 0f),
            new(1f, 1f)
        ]);
        return ApplyGradient(text, gradient);
    }

    private static string ApplyGradient(string text, Gradient gradient)
    {
        var text2 = "";

        for (var i = 0; i < text.Length; i++)
            text2 += $"<#{gradient.Evaluate((float)i / text.Length).ToHtmlStringRGBA()}>{text[i]}</color>";

        return text2;
    }

    private static Color ShadeColor(this Color color, float strength = 0f)
    {
        strength = Mathf.Clamp(strength, -1f, 1f);
        return Color.Lerp(color, strength < 0 ? Color.white : Color.black, Mathf.Abs(strength));
    }

    public static string ToRoleFactionDisplayString(Role role, FactionType faction)
    {
        if (role is Role.STONED or Role.HIDDEN or Role.UNKNOWN) 
            faction = FactionType.NONE;

        var factionName = faction switch
        {
            FactionType.TOWN => "TOWN",
            FactionType.COVEN => "COVEN",
            FactionType.APOCALYPSE => "APOCALYPSE",
            FactionType.SERIALKILLER => "SERIALKILLER",
            FactionType.ARSONIST => "ARSONIST",
            FactionType.WEREWOLF => "WEREWOLF",
            FactionType.SHROUD => "SHROUD",
            FactionType.EXECUTIONER => "EXECUTIONER",
            FactionType.JESTER => "JESTER",
            FactionType.PIRATE => "PIRATE",
            FactionType.DOOMSAYER => "DOOMSAYER",
            FactionType.VAMPIRE => "VAMPIRE",
            FactionType.CURSED_SOUL => "CURSEDSOUL",
            (FactionType)33 => "JACKAL",
            (FactionType)34 => "FROGS",
            (FactionType)35 => "LIONS",
            (FactionType)36 => "HAWKS",
            (FactionType)38 => "JUDGE",
            (FactionType)39 => "AUDITOR",
            (FactionType)40 => "INQUISITOR",
            (FactionType)41 => "STARSPAWN",
            (FactionType)42 => "EGOTIST",
            (FactionType)43 => "PANDORA",
            (FactionType)44 => "COMPLIANCE",
            _ => "NONE",
        };

        if (Constants.IsBTOS2())
            factionName += "_BTOS";

        return Service.Home.LocalizationService.GetLocalizedString($"FANCY_{factionName}_ROLENAME_{(int)role}");
    }

    public static Color GetPlayerRoleColor(int pos)
    {
        var color = Color.white;
        Service.Game.Sim.simulation.knownRolesAndFactions.Data.TryGetValue(pos, out Tuple<Role, FactionType> tuple);
        Color color2;

        if (tuple == null)
            color2 = color;
        else
        {
            if (tuple.Item1 is Role.STONED or Role.HIDDEN)
                color2 = color;
            else
            {
                color = ClientRoleExtensions.GetFactionColor(tuple.Item2).ParseColor();
                color2 = color;
            }
        }

        return color2;
    }


    public static bool GetRoleInfo(int pos, out Tuple<Role, FactionType> value) => Service.Game.Sim.simulation.knownRolesAndFactions.Data.TryGetValue(pos, out value);

    public static string GetRoleName(Role role, FactionType factionType, bool useBrackets = false)
    {
        var roleName = ToRoleFactionDisplayString(role, factionType);
        if (useBrackets)
            roleName = $"({roleName})";

        return roleName;
    }
    public static string GetColorizedRoleName(Role role, FactionType factionType, bool useBrackets = false)
    {
        var roleName = ToRoleFactionDisplayString(role, factionType);
        if (useBrackets)
            roleName = $"({roleName})";

        var gradientText = Utils.TryApplyGradient(roleName, factionType);
        if (gradientText != null)
            return gradientText;

        var color = factionType.GetFactionColor();
        return $"<color={color}>{roleName}</color>";
    }
    public static string GetColorizedText(string text, FactionType factionType)
    {
        var gradientText = Utils.TryApplyGradient(text, factionType);
        if (gradientText != null)
            return gradientText;

        var color = factionType.GetFactionColor();
        return $"<color={color}>{text}</color>";
    }

    public static string TryApplyGradient(string text, FactionType factionType)
    {
        if (!Constants.IsBTOS2())
            return null;

        try
        {
            return GetGradient(text, factionType);
        }
        catch
        {
            return null;
        }
    }

    private static string GetGradient(string text, FactionType factionType)
    {
        var gradient = BetterTOS2.GetGradients.GetGradient(factionType);
        if (gradient != null)
        {
            return BetterTOS2.AddNewConversionTags.ApplyGradient(
                text,
                gradient.Evaluate(0f),
                gradient.Evaluate(1f)
            );
        }

        return null;
    }

    public static Color GetFactionStartingColor(FactionType faction) => faction switch
    {
        FactionType.TOWN => ModSettings.GetColor("Town Start", "det.rolecustomizationmod"),
        FactionType.COVEN => ModSettings.GetColor("Coven Start", "det.rolecustomizationmod"),
        FactionType.APOCALYPSE => ModSettings.GetColor("Apocalypse Start", "det.rolecustomizationmod"),
        FactionType.EXECUTIONER => ModSettings.GetColor("Executioner Start", "det.rolecustomizationmod"),
        FactionType.SERIALKILLER => ModSettings.GetColor("Serial Killer Start", "det.rolecustomizationmod"),
        FactionType.ARSONIST => ModSettings.GetColor("Arsonist Start", "det.rolecustomizationmod"),
        FactionType.WEREWOLF => ModSettings.GetColor("Werewolf Start", "det.rolecustomizationmod"),
        FactionType.SHROUD => ModSettings.GetColor("Shroud Start", "det.rolecustomizationmod"),
        FactionType.JESTER => ModSettings.GetColor("Jester Start", "det.rolecustomizationmod"),
        (FactionType)40 => ModSettings.GetColor("Inquisitor Start", "det.rolecustomizationmod"),
        FactionType.PIRATE => ModSettings.GetColor("Pirate Start", "det.rolecustomizationmod"),
        FactionType.DOOMSAYER => ModSettings.GetColor("Doomsayer Start", "det.rolecustomizationmod"),
        FactionType.VAMPIRE => ModSettings.GetColor("Vampire Start", "det.rolecustomizationmod"),
        FactionType.CURSED_SOUL => ModSettings.GetColor("Cursed Soul Start", "det.rolecustomizationmod"),
        (FactionType)33 => ModSettings.GetColor("Jackal/Recruit Start", "det.rolecustomizationmod"),
        (FactionType)38 => ModSettings.GetColor("Judge Start", "det.rolecustomizationmod"),
        (FactionType)39 => ModSettings.GetColor("Auditor Start", "det.rolecustomizationmod"),
        (FactionType)41 => ModSettings.GetColor("Starspawn Start", "det.rolecustomizationmod"),
        (FactionType)42 => ModSettings.GetColor("Egotist Start", "det.rolecustomizationmod"),
        (FactionType)43 => ModSettings.GetColor("Pandora Start", "det.rolecustomizationmod"),
        (FactionType)34 => ModSettings.GetColor("Frogs Start", "det.rolecustomizationmod"),
        (FactionType)35 => ModSettings.GetColor("Lions Start", "det.rolecustomizationmod"),
        (FactionType)36 => ModSettings.GetColor("Hawks Start", "det.rolecustomizationmod"),
        (FactionType)44 => ModSettings.GetColor("Compliance Start", "det.rolecustomizationmod"),
        _ => ModSettings.GetColor("Stoned/Hidden", "det.rolecustomizationmod"),
    };

    public static Color GetFactionEndingColor(FactionType faction) => faction switch
    {
        FactionType.TOWN => ModSettings.GetColor("Town End", "det.rolecustomizationmod"),
        FactionType.COVEN => ModSettings.GetColor("Coven End", "det.rolecustomizationmod"),
        FactionType.APOCALYPSE => ModSettings.GetColor("Apocalypse End", "det.rolecustomizationmod"),
        FactionType.EXECUTIONER => ModSettings.GetColor("Executioner End", "det.rolecustomizationmod"),
        FactionType.SERIALKILLER => ModSettings.GetColor("Serial Killer End", "det.rolecustomizationmod"),
        FactionType.ARSONIST => ModSettings.GetColor("Arsonist End", "det.rolecustomizationmod"),
        FactionType.WEREWOLF => ModSettings.GetColor("Werewolf End", "det.rolecustomizationmod"),
        FactionType.SHROUD => ModSettings.GetColor("Shroud End", "det.rolecustomizationmod"),
        FactionType.JESTER => ModSettings.GetColor("Jester End", "det.rolecustomizationmod"),
        (FactionType)40 => ModSettings.GetColor("Inquisitor End", "det.rolecustomizationmod"),
        FactionType.PIRATE => ModSettings.GetColor("Pirate End", "det.rolecustomizationmod"),
        FactionType.DOOMSAYER => ModSettings.GetColor("Doomsayer End", "det.rolecustomizationmod"),
        FactionType.VAMPIRE => ModSettings.GetColor("Vampire End", "det.rolecustomizationmod"),
        FactionType.CURSED_SOUL => ModSettings.GetColor("Cursed Soul End", "det.rolecustomizationmod"),
        (FactionType)33 => ModSettings.GetColor("Jackal/Recruit End", "det.rolecustomizationmod"),
        (FactionType)38 => ModSettings.GetColor("Judge End", "det.rolecustomizationmod"),
        (FactionType)39 => ModSettings.GetColor("Auditor End", "det.rolecustomizationmod"),
        (FactionType)41 => ModSettings.GetColor("Starspawn End", "det.rolecustomizationmod"),
        (FactionType)42 => ModSettings.GetColor("Egotist End", "det.rolecustomizationmod"),
        (FactionType)43 => ModSettings.GetColor("Pandora End", "det.rolecustomizationmod"),
        (FactionType)34 => ModSettings.GetColor("Frogs End", "det.rolecustomizationmod"),
        (FactionType)35 => ModSettings.GetColor("Lions End", "det.rolecustomizationmod"),
        (FactionType)36 => ModSettings.GetColor("Hawks End", "det.rolecustomizationmod"),
        (FactionType)44 => ModSettings.GetColor("Compliance End", "det.rolecustomizationmod"),
        _ => ModSettings.GetColor("Stoned/Hidden", "det.rolecustomizationmod"),
    };

    public static string GetString(string key)
    {
        return Service.Home.LocalizationService.GetLocalizedString(key);
    }
    
    public static string ApplyGradient(string text, Color color1, Color color2)
    {
        var gradient = new Gradient();
        gradient.SetKeys(
        [
            new(color1, 0f),
            new(color2, 1f)
        ],
        [
            new(1f, 0f),
            new(1f, 1f)
        ]);
        var text2 = "";

        for (var i = 0; i < text.Length; i++)
            text2 += $"<color={ToHexString(gradient.Evaluate((float)i / text.Length))}>{text[i]}</color>";

        return text2;
    }

    public static string ApplyThreeColorGradient(string text, Color color1, Color color2, Color color3)
    {
        var gradient = new Gradient();
        gradient.SetKeys(
        [
            new(color1, 0f),
            new(color2, 0.5f),
            new(color3, 1f)
        ],
        [
            new(1f, 0f),
            new(1f, 1f)
        ]);
        var text2 = "";

        for (var i = 0; i < text.Length; i++)
            text2 += $"<color={ToHexString(gradient.Evaluate((float)i / text.Length))}>{text[i]}</color>";

        return text2;
    }

    public static string SetGradient(string text, Gradient gradient)
    {
        var text2 = "";

        for (var i = 0; i < text.Length; i++)
            text2 += $"<color={ToHexString(gradient.Evaluate((float)i / text.Length))}>{text[i]}</color>";

        return text2;
    }

    public static string ToHexString(Color color)
    {
        Color32 color2 = color;
        return $"#{color2.r:X2}{color2.g:X2}{color2.b:X2}";
    }


}