// using FancyUI.Assets.SilhouetteSwapper;
using System.Text.RegularExpressions;
using Home.Shared;
using NewModLoading;
using Server.Shared.Extensions;
using System.Text;

namespace FancyUI;

public static class Utils
{
	// Note: Update this whenever a role's ability count changes in updates for Vanilla and BetterTOS2

    private static readonly string[] VanillaSkippableNames = ["Baker_Ability", "Pirate_Ability_1", "Pirate_Ability_2", "Dreamweaver_Ability_1"];
    private static readonly string[] BTOS2SkippableNames = [ "Baker_Ability_1", "Baker_Ability_2", "Jackal_Ability", "Auditor_Ability_1", "Auditor_Ability_2", "Inquisitor_Ability_1", "Inquisitor_Ability_2", "Banshee_Ability", "Judge_Ability",
        "Warlock_Ability", "Starspawn_Ability", "Dreamweaver_Ability_2", "Illusionist_Ability_2", "Pacifist_Ability_2", "CovenLeader_Ability" ];
    private static readonly string[] CommonSkippableNames = [ "Admirer_Ability_1", "Admirer_Ability_2", "Amnesiac_Ability", "Arsonist_Ability", "Attributes", "Berserker_Ability", "Bodyguard_Ability", "Catalyst_Ability",
        "Cleric_Ability", "Coroner_Ability", "Crusader_Ability", "CursedSoul_Ability", "Death_Ability", "Enchanter_Ability", "Famine_Ability", "HexMaster_Ability",
        "Illusionist_Ability", "Investigator_Ability", "Jailor_Ability", "Jester_Ability", "Jinx_Ability", "Lookout_Ability", "Medusa_Ability", "Monarch_Ability_2", "Seer_Ability_1",
        "Pestilence_Ability", "Plaguebearer_Ability", "Poisoner_Ability_1", "Poisoner_Ability_2", "PotionMaster_Ability_1", "PotionMaster_Ability_2", "Psychic_Ability", "War_Ability_1", "Werewolf_Ability_2",
        "Seer_Ability_2", "SerialKiller_Ability", "Sheriff_Ability", "Shroud_Ability", "SoulCollector_Ability", "Trickster_Ability_1", "Trickster_Ability_2", "Spy_Ability", "Wildling_Ability",
        "TavernKeeper_Ability", "Tracker_Ability", "Trapper_Ability", "Vampire_Ability_1", "Vampire_Ability_2", "Vigilante_Ability", "VoodooMaster_Ability", "War_Ability_2", "Witch_Ability_1", "Witch_Ability_2",
        "Werewolf_Ability_1", "Necromancer_Ability_2", "Socialite_Ability_2" ];

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
        Btos2Role.Catalyst => "Catalyst",
        Btos2Role.Cultist => "Cultist",
        Btos2Role.Socialite => "Socialite",
        Btos2Role.Pacifist => "Pacifist",
        Btos2Role.Stoned => "Stoned",
        Btos2Role.RandomTown => "Town",
        Btos2Role.RandomCoven => "Coven",
        Btos2Role.RandomNeutral => "Neutral",
        Btos2Role.TownInvestigative => "TownInvestigative",
        Btos2Role.TownProtective => "TownProtective",
        Btos2Role.TownKilling => "TownKilling",
        Btos2Role.TownSupport => "TownSupport",
        Btos2Role.CovenKilling => "CovenKilling",
        Btos2Role.CovenUtility => "CovenUtility",
        Btos2Role.CovenDeception => "CovenDeception",
        Btos2Role.CovenPower => "CovenPower",
        Btos2Role.NeutralKilling => "NeutralKilling",
        Btos2Role.NeutralEvil => "NeutralEvil",
        Btos2Role.CommonCoven => "CommonCoven",
        Btos2Role.CommonTown => "CommonTown",
        Btos2Role.RandomApocalypse => "RandomApocalypse",
        Btos2Role.NeutralPariah => "NeutralPariah",
        Btos2Role.NeutralOutlier => "NeutralOutlier",
        Btos2Role.Any => "Any",
        Btos2Role.TownExecutive => "TownExecutive",
        Btos2Role.TownGovernment => "TownGovernment",
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
        Btos2Role.AnonNames => "AnonPlayers",
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
        Btos2Role.Vc => "VC",
        Btos2Role.Lovers => "Lovers",
        Btos2Role.FeelinLucky => "FeelinLucky",
        Btos2Role.GracePeriod => "GracePeriod",
        Btos2Role.AllOutliers => "AllOutliers",
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
        Role.PILGRIM => "Pilgrim",
        Role.COVENITE => "Covenite",
        Role.CATALYST => "Catalyst",
        Role.CULTIST => "Cultist",
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
        Role.ELECTION => "Election",
        Role.NO_NIGHT_ONE_KILLS => "GracePeriod",
        Role.ALL_OUTLIERS => "AllOutliers",
        Role.HIDDEN => "Hidden",
        _ => "Blank"
    };

    public static string FactionName(FactionType faction, bool allowOverrides, GameModType? mod = null, bool stoned = false) => FactionName(faction, mod, allowOverrides, stoned);

    public static string FactionName(FactionType faction, GameModType? mod = null, bool allowOverrides = true, bool stoned = false)
    {
        if (allowOverrides && Constants.FactionOverridden())
            return Constants.FactionOverride();

        try
        {
            return (mod ?? GetGameType()) switch
            {
                GameModType.BTOS2 => BTOSFactionName(faction, stoned),
                _ => VanillaFactionName(faction, stoned)
            };
        }
        catch
        {
            return VanillaFactionName(faction, stoned);
        }
    }

    private static string BTOSFactionName(FactionType faction, bool stoned) => faction switch
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
        _ => stoned ? "Stoned_Hidden" : "Factionless"
    };

    private static string VanillaFactionName(FactionType faction, bool stoned) => faction switch
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
        _ => stoned ? "Stoned_Hidden" : "Factionless"
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
        if (!texture || (!Fancy.DumpSpriteSheets.Value && !skipSetting))
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
        EffectType.VIP => "StatusVIP",
        EffectType.BUG => "Bugged",
        EffectType.SCENT_TRACK => "Tracked",
        EffectType.PESTILENCE => "Sickness",
        EffectType.REVEALED_MARSHAL => "RevealedMarshal",
        EffectType.SOCIALITE_GUEST => "Guest",
        EffectType.REAPED => "Reaped",
        EffectType.REVEALED_BY_PMWITCH => "Illuminated",
        EffectType.BESTOWED or (EffectType)111 => "Bestowed",
        EffectType.SOVEREIGN => "Sovereign",
        EffectType.STONED => "Stone",

        // BTOS2
        (EffectType)100 => "Recruit",
        (EffectType)101 => "Deafened",
        (EffectType)102 => "CovenTraitor",
        (EffectType)103 => "ApocTraitor",
        (EffectType)104 => "Audited",
        (EffectType)105 => "Enchanted",
        (EffectType)106 => "Accompanied",
        (EffectType)107 => "PandoraTraitor",
        (EffectType)108 => "Egotist",
        (EffectType)109 => "Suspect",
        (EffectType)110 => "WarlockCursed",
        (EffectType)112 => "Sensed",
        _ => "Blank"
    };

    public static GameModType GetGameType()
    {
        if (Constants.IsBTOS2() || FindCasualQueue() || SettingsAndTestingUI.Instance?.IsBTOS2 == true)
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
        (_, Btos2Role.Banshee or Btos2Role.Cultist, GameModType.BTOS2) => Btos2Faction.Coven,
        (_, Btos2Role.Marshal or Btos2Role.Oracle or Btos2Role.Catalyst or Btos2Role.Socialite or Btos2Role.Pacifist, GameModType.BTOS2) => Btos2Faction.Town,
        (_, Btos2Role.Jackal, GameModType.BTOS2) => Btos2Faction.Jackal,
        (_, Btos2Role.Judge, GameModType.BTOS2) => Btos2Faction.Judge,
        (_, Btos2Role.Auditor, GameModType.BTOS2) => Btos2Faction.Auditor,
        (_, Btos2Role.Inquisitor, GameModType.BTOS2) => Btos2Faction.Inquisitor,
        (_, Btos2Role.Starspawn, GameModType.BTOS2) => Btos2Faction.Starspawn,
        (_, Btos2Role.Warlock, GameModType.BTOS2) => Btos2Faction.Apocalypse,

        // Vanilla specific role checks
        ( > 53 and < 57, _, GameModType.Vanilla) => FactionType.TOWN,
        ( _, Role.PILGRIM or Role.CATALYST, GameModType.Vanilla) => FactionType.TOWN,
        ( _, Role.COVENITE or Role.CULTIST, GameModType.Vanilla) => FactionType.COVEN,

        // Default case
        _ => FactionType.NONE
    };

    public static void SetImageColor(this Image img, ColorType type, bool notGuide = true)
    {
        if (Constants.AllMaterials.Count == 0)
            return;

        var hasMask = img.TryGetComponent<Mask>(out var mask);

        if (hasMask)
            mask.enabled = false;

        img.material = Constants.AllMaterials[notGuide][type];

        if (hasMask)
            mask.enabled = true;
    }

    public static void SetGraphicColor(this Graphic graphic, ColorType type, FactionType? faction = null, bool flip = true)
    {
        if (Constants.AllMaterials.Count == 0)
            return;

        var color2 = Constants.GetUIThemeColor(type, faction);

        if (color2 == Color.clear)
            return;

        color2 = color2.ShadeColor(type, flip, false);
        graphic.color = color2;
    }

    public static void UpdateMaterials(bool notGuide = true, FactionType? faction = null, bool flip = false, bool skipFactionCheck = false)
    {
        if (Constants.AllMaterials.Count == 0)
            return;

        if (Fancy.SelectedUITheme.Value == UITheme.Vanilla)
            Constants.AllMaterials[notGuide].Values.Do(x => x.SetFloat(Vanilla, 1));
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

    public static Color Desaturate(Color color, float amount = 0.5f)
    {
        // Convert RGB to HSL
        var hsb = new HsbColor(color);
        hsb.s = Mathf.Clamp01(hsb.s * amount);
        return hsb;
    }

    public static Gradient Desaturate(Gradient gradient, float amount = 0.5f)
    {
        var newGradient = new Gradient();
        var colorKeys = gradient.colorKeys.Select(k => new GradientColorKey(Desaturate(k.color, amount), k.time)).ToArray();
        newGradient.SetKeys(colorKeys, gradient.alphaKeys);
        return newGradient;
    }

    // public static bool IsValid(this SilhouetteAnimation anim) => anim != null && anim != Loading;

    public static bool GetRoleAndFaction(int pos, out Tuple<Role, FactionType> tuple) => Service.Game.Sim.simulation.knownRolesAndFactions.Data.TryGetValue(pos, out tuple);

    // public static void DebugSingleTransform(this Transform transform)
    // {
    //     var debug = $"[{transform.parent?.name ?? "None"} -> {transform.name}] = {{ ";
    //     transform.GetComponents<Component>().Do(x => debug += $"{x.GetType().Name}, ");
    //     Fancy.Instance.Fatal(debug + "}");
    // }

    // public static void DebugTransformRecursive(this Transform transform)
    // {
    //     transform.DebugSingleTransform();
    //
    //     for (var i = 0; i < transform.childCount; i++)
    //         transform.GetChild(i).DebugTransformRecursive();
    // }

    public static string ApplyGradient(string text, params string[] colors) => ApplyGradient(text, CreateGradient(colors));

    public static string ApplyGradient(string text, params Color[] colors) => ApplyGradient(text, CreateGradient(colors));

    public static Gradient CreateGradient(params string[] colors) => CreateGradient([.. colors.Select(x => x.ToColor())]);

    private static Gradient CreateGradient(params Color[] colors)
    {
        var length = colors.Length;

        if (length == 0)
            throw new InvalidOperationException("Colors are needed to create a gradient");

        var gradient = new Gradient();
        gradient.SetKeys(length == 1
            ? [new(colors[0], 0f), new(colors[0], 1f)]
            : [.. colors.Select((i, color) => new GradientColorKey(color, (float)i / (colors.Length - 1)))],
            [new(1f, 0f), new(1f, 1f)]);
        return gradient;
    }

    public static string ApplyGradient(string text, Gradient gradient)
    {
        if (!Fancy.VerticalGradients.Value)
        {
            var text2 = "";

            for (var i = 0; i < text.Length; i++)
                text2 += $"<#{gradient.Evaluate((float)i / text.Length).ToHtmlStringRGBA()}>{text[i]}</color>";

            return text2;
        }
        return "<color=#FFFFFFFF><gradient=\"" + Gradients.GetVerticalGradientKey(gradient) + "\">" + text + "</gradient></color>";
    }

    private static Color ShadeColor(this Color color, float strength = 0f)
    {
        strength = Mathf.Clamp(strength, -1f, 1f);
        return Color.Lerp(color, strength < 0 ? Color.white : Color.black, Mathf.Abs(strength));
    }

	public static string ToRoleFactionDisplayString(this Role role, FactionType faction)
	{
		if (!Fancy.FactionalRoleNames.Value)
			return role.ToDisplayString();

		if (role is Role.STONED or Role.HIDDEN or Role.UNKNOWN)
			faction = FactionType.NONE;

		var factionName = FactionName(faction, GameModType.BTOS2)?.ToUpper() ?? "NONE";
		if (factionName == "FACTIONLESS")
			factionName = "NONE";

		var isBTOS2 = Constants.IsBTOS2() || SettingsAndTestingUI.Instance?.IsBTOS2 == true;

		var newKey = $"{(isBTOS2 ? "BTOS_" : "GUI_")}ROLENAME_{(int)role}_{(int)faction}";
		var oldKey = $"FANCY_{(isBTOS2 ? "BTOS_" : "")}{factionName}_ROLENAME_{(int)role}";

		var loc = Service.Home.LocalizationService;

		if (loc.StringExists(newKey))
			return loc.GetLocalizedString(newKey);

		if (loc.StringExists(oldKey))
			return loc.GetLocalizedString(oldKey);

		return role.ToDisplayString();
	}

	public static string ToRoleFactionShortenedDisplayString(this Role role, FactionType faction)
	{
		if (!Fancy.FactionalRoleNames.Value)
			return role.ToShortenedDisplayString();

		if (role is Role.STONED or Role.HIDDEN or Role.UNKNOWN)
			faction = FactionType.NONE;

		var factionName = FactionName(faction, GameModType.BTOS2)?.ToUpper() ?? "NONE";
		if (factionName == "FACTIONLESS")
			factionName = "NONE";

		var isBTOS2 = Constants.IsBTOS2() || SettingsAndTestingUI.Instance?.IsBTOS2 == true;

		var newKey = $"{(isBTOS2 ? "BTOS_" : "GUI_")}SHORTNAME_{(int)role}_{(int)faction}";
		var oldKey = $"FANCY_{(isBTOS2 ? "BTOS_" : "")}{factionName}_SHORTNAME_{(int)role}";

		var loc = Service.Home.LocalizationService;

		if (loc.StringExists(newKey))
			return loc.GetLocalizedString(newKey);

		if (loc.StringExists(oldKey))
			return loc.GetLocalizedString(oldKey);

		return role.ToShortenedDisplayString();
	}


	public static string ToFactionalRoleBlurb(this Role role, FactionType faction)
	{
		if (!Fancy.FactionalRoleBlurbs.Value)
			return role.GetRoleBlurb();

		if (role is Role.STONED or Role.HIDDEN or Role.UNKNOWN)
			faction = FactionType.NONE;

		var factionName = FactionName(faction, GameModType.BTOS2)?.ToUpper() ?? "NONE";
		if (factionName == "FACTIONLESS")
			factionName = "NONE";

		var isBTOS2 = Constants.IsBTOS2() || SettingsAndTestingUI.Instance?.IsBTOS2 == true;

		var newKey = $"{(isBTOS2 ? "BTOS_" : "GUI_")}ROLE_BLURB_{(int)role}_{(int)faction}";
		var oldKey = $"FANCY_{(isBTOS2 ? "BTOS_" : "")}{factionName}_ROLE_BLURB_{(int)role}";

		var loc = Service.Home.LocalizationService;

		if (loc.StringExists(newKey))
			return loc.GetLocalizedString(newKey);

		if (loc.StringExists(oldKey))
			return loc.GetLocalizedString(oldKey);

		return role.GetRoleBlurb();
	}

    public static Color GetPlayerRoleColor(int pos)
    {
        if (!GetRoleAndFaction(pos, out var tuple) || tuple == null || tuple.Item1 is Role.STONED or Role.HIDDEN)
            return Fancy.UnknownColor.Value.ToColor();

        return tuple.Item2.GetFactionColor().ParseColor();
    }

    public static string GetRoleName(Role role, FactionType factionType, bool useBrackets = false)
    {
        var roleName = role.ToRoleFactionDisplayString(factionType);

        if (useBrackets)
            roleName = $"({roleName})";

        return roleName;
    }

    public static Color GetFactionStartingColor(FactionType faction) => Fancy.Colors[FactionName(faction, stoned: true).ToUpper()].Start.ToColor();

    public static Color GetFactionEndingColor(FactionType faction)
    {
        var name = FactionName(faction, stoned: true);

        // Explicit fallback to start color for STONED_HIDDEN
        return name == "Stoned_Hidden" ? Fancy.Colors["STONED_HIDDEN"].Start.ToColor() : Fancy.Colors[name.ToUpper()].End.ToColor();
    }

    public static string GetString(string key) => Service.Home.LocalizationService.GetLocalizedString(key);

    public static string RemoveVanillaGradientStyleTags(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return Regex.Replace(
            input,
            @"<style\s*=\s*""?(VampireColor|CursedSoulColor)""?\s*>(.*?)</style>",
            "$2",
            RegexOptions.Singleline);
    }

    public static bool ConditionalCompliancePandora(FactionType originalFaction, FactionType currentFaction)
    {
        if (currentFaction == (FactionType)43)
            return originalFaction is FactionType.COVEN or FactionType.APOCALYPSE;

        if (originalFaction == (FactionType)43)
            return currentFaction is FactionType.COVEN or FactionType.APOCALYPSE;

        if (currentFaction == Btos2Faction.Compliance)
            return originalFaction is FactionType.SERIALKILLER or FactionType.ARSONIST or FactionType.WEREWOLF or FactionType.SHROUD;

        if (originalFaction == Btos2Faction.Compliance)
            return currentFaction is FactionType.SERIALKILLER or FactionType.ARSONIST or FactionType.WEREWOLF or FactionType.SHROUD;

        return originalFaction == currentFaction;
    }

    public static string RemoveColorTags(this string input) => Regex.Replace(input, "<color=[^>]+>|<#[^>]+>|</color>", "");

    public static bool StartsWithVowel(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        var stripped = StripFormatting(input).TrimStart();

        if (stripped.Length == 0)
            return false;

        var firstChar = char.ToUpperInvariant(stripped[0]);
        return "AEIOU".IndexOf(firstChar) >= 0;
    }

    private static string StripFormatting(string input)
    {
        var sb = new StringBuilder();
        var insideTag = false;

        foreach (var c in input)
        {
            if (c == '<')
            {
                insideTag = true;
                continue;
            }
            if (c == '>' && insideTag)
            {
                insideTag = false;
                continue;
            }

            if (!insideTag)
                sb.Append(c);
        }

        return sb.ToString();
    }

    public static string ReplaceIcons(this string input)
    {
        if (Constants.IsBTOS2())
            return input.Replace("\"RoleIcons", "\"BTOSRoleIcons");
        else
            return input.Replace("\"BTOSRoleIcons", "\"RoleIcons");
    }

    public static string AddFactionToIcon(this string input, string faction) => input.Replace("RoleIcons\"", $"RoleIcons ({faction})\"");

    public static string GetFactionSprite(this Role role, FactionType factionType, bool replaceIcons = false)
    {
        var icon = role.GetTMPSprite();
        var factionName = FactionName(factionType);
        var factionIcon = icon.AddFactionToIcon(factionName);

        return replaceIcons ? factionIcon.ReplaceIcons() : factionIcon;
    }

    public static bool IsHorseman(this Role role) => RoleExtensions.horsemenList.Contains(role);

    public static string GetFormattedRoleName(Role role, FactionType faction, bool includeSprite = true)
    {
        var sprite = includeSprite
            ? $"<sprite=\"RoleIcons ({GetStyleForFaction(role, faction)})\" name=\"Role{(int)role}\">"
            : "";

        return sprite + role.ToColorizedDisplayString(faction);
    }

    private static string GetStyleForFaction(Role role, FactionType faction)
    {
        return (Constants.CurrentStyle() == "Regular" && role.GetFactionType() == faction)
            ? "Regular"
            : FactionName(faction, false);
    }

    public static string GetHangingMessage(Role role, FactionType faction)
    {
        var roleName = StripFormatting(role.ToColorizedDisplayString(faction));

        if (role == Role.STONED)
            return "FANCY_PLAYER_WAS_STONED";
        else if (role == Role.HIDDEN)
            return "FANCY_PLAYER_WAS_A_HIDDEN_ROLE";
        else if (role.IsHorseman())
            return "FANCY_PLAYER_WAS_ROLE";
        else if (StartsWithVowel(roleName))
            return "FANCY_PLAYER_WAS_AN_ROLE";
        else
            return "FANCY_PLAYER_WAS_A_ROLE";
    }

    public static string GetWdahMessage(Role role, FactionType faction)
    {
        var roleName = StripFormatting(role.ToColorizedDisplayString(faction));

        if (role == Role.STONED)
            return "FANCY_THEY_WERE_STONED";
        else if (role == Role.HIDDEN)
            return "FANCY_THEY_WERE_A_HIDDEN_ROLE";
        else if (role.IsHorseman())
            return "FANCY_THEY_WERE_ROLE";
        else if (StartsWithVowel(roleName))
            return "FANCY_THEY_WERE_AN_ROLE";
        else
            return "FANCY_THEY_WERE_A_ROLE";
    }

    public static string ToColorizedNoLabel(this Role role, FactionType faction)
    {
        var roleName = Fancy.FactionalRoleNames.Value ? role.ToRoleFactionDisplayString(faction) : role.ToDisplayString();
        var gradient = faction.GetChangedGradient(role);

        return gradient != null
            ? ApplyGradient(roleName, gradient)
            : $"<color={faction.GetFactionColor()}>{roleName}</color>";
    }

    public static string BuildPlayerTag(KillRecord killRecord)
            => $"[[@{killRecord.playerId + 1}]]";

    public static string BuildHiddenRoleTag(KillRecord killRecord)
    {
        if (killRecord.hiddenPlayerRole is not Role.NONE and not Role.HIDDEN)
            return $"[[#{(int)killRecord.hiddenPlayerRole},{(int)killRecord.hiddenPlayerFaction}]]";

        return null;
    }

    public static string BuildRoleText(KillRecord killRecord)
    {
        var text = $"[[#{(int)killRecord.playerRole},{(int)killRecord.playerFaction}]]";
        var hidden = BuildHiddenRoleTag(killRecord);

        if (hidden != null)
            text += $" ({hidden})";

        return text;
    }


    public static string ToFactionalDisplayString(this Role role, FactionType faction)
    {
        return Fancy.FactionalRoleNames.Value ? role.ToRoleFactionDisplayString(faction) : role.ToDisplayString();
    }

    public static string ToFactionalShortenedDisplayString(this Role role, FactionType faction)
    {
        return Fancy.FactionalRoleNames.Value ? role.ToRoleFactionShortenedDisplayString(faction) : role.ToShortenedDisplayString();
    }

	public static void ApplyDockItemIcon(HudDockItem item)
	{
		if (item == null) return;
		if (item.button == null) return;

		var img = item.button.image;
		if (img == null) return;

		Sprite sprite = null;

		switch (item.dockFunctionType)
		{
			case DockFunctionType.NECRO_PASS:
				sprite = GetSprite("NecroPass");
				break;

			// case DockFunctionType.LAST_WILL:
				// sprite = GetSprite("LastWill");
				// break;

			// case DockFunctionType.DEATH_NOTE:
				// sprite = GetSprite("DeathNote");
				// break;

			// case DockFunctionType.NOTEPAD:
				// sprite = GetSprite("Notepad");
				// break;

			// case DockFunctionType.CHAT:
				// sprite = GetSprite("Chat");
				// break;

			// case DockFunctionType.FOOLS_FORTUNE:
				// sprite = GetSprite("FoolsFortune");
				// break;

			// case DockFunctionType.GHOST_PLAY:
				// sprite = GetSprite("GhostPlay");
				// break;

			// case DockFunctionType.LOBBY_PREVIOUS_GAME_RESULTS:
				// sprite = GetSprite("PreviousGameResults");
				// break;

			// case DockFunctionType.LOBBY_CUSTOMIZATION:
				// sprite = GetSprite("Customization");
				// break;

			// case DockFunctionType.LOBBY_PLAYERS:
				// sprite = GetSprite("Players");
				// break;

			// case DockFunctionType.LOBBY_ROLE_DECK:
				// sprite = GetSprite("RoleDeck");
				// break;

			// case DockFunctionType.LOBBY_SETTINGS:
			// case DockFunctionType.SETTINGS:
				// sprite = GetSprite("Settings");
				// break;

			// case DockFunctionType.LOBBY_FRIENDS:
			// case DockFunctionType.FRIENDS:
				// sprite = GetSprite("Friends");
				// break;
		}

		if (sprite != null)
			img.sprite = sprite;
    }
    public static string GetKeyPrefix(bool fancy = false)
    {
        if (fancy)
            return Constants.IsBTOS2() ? "FBTOS" : "FGUI";

        return Constants.IsBTOS2() ? "BTOS" : "GUI";
    }
}