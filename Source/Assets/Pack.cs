namespace FancyUI.Assets;

public abstract class Pack(string name, PackType type)
{
    public string Name { get; } = name;
    public PackType Type { get; } = type;

    public bool Deleted { get; set; }

    public string PackPath => Path.Combine(AssetManager.ModPath, $"{Type}", Name);

    public static readonly string[] Roles = [ "Admirer", "Amnesiac", "Bodyguard", "Cleric", "Coroner", "Crusader", "Deputy", "Investigator", "Jailor", "Lookout", "Amnesiac", "Amnesiac", "War",
        "Amnesiac", "Amnesiac", "Amnesiac", "Amnesiac", "Amnesiac", "Mayor", "Monarch", "Prosecutor", "Psychic", "Retributionist", "Seer", "Sheriff", "Spy", "TavernKeeper", "Tracker", "Death",
        "Trapper", "Trickster", "Veteran", "Vigilante", "Conjurer", "CovenLeader", "Dreamweaver", "Enchanter", "HexMaster", "Illusionist", "Jinx", "Medusa", "Necromancer", "Poisoner", "Judge",
        "PotionMaster", "Ritualist", "VoodooMaster", "Wildling", "Witch", "Arsonist", "Baker", "Berserker", "Doomsayer", "Executioner", "Jester", "Pirate", "Plaguebearer", "SerialKiller",
        "Shroud", "SoulCollector", "Werewolf", "Vampire", "CursedSoul", "Banshee", "Jackal", "Marshal", "Auditor", "Inquisitor", "Starspawn", "Oracle", "Warlock", "Famine", "Pestilence" ];
    public static readonly string[] CommonFolders = [ "Regular", "Town", "Coven", "SerialKiller", "Arsonist", "Werewolf", "Shroud", "Apocalypse", "VIP", "Jester", "Pirate", "Doomsayer",
        "Vampire", "CursedSoul", "Executioner", "Necronomicon", "Factionless" ];
    public static readonly string[] VanillaFolders = [];
    public static readonly string[] BTOS2Folders = [ "Judge", "Auditor", "Starspawn", "Inquisitor", "Jackal", "Lions", "Frogs", "Hawks", "Pandora", "Egotist", "Compliance" ];
    public static readonly string[] MainFolders = [ "Common", "Vanilla", "BTOS2", "PlayerNumbers" ];
    public static readonly string[] Mods = [ "Vanilla", "BTOS2" ];
    public static readonly Dictionary<string, string[]> ModsToFolders = new()
    {
        { "Common", CommonFolders },
        { "Vanilla", VanillaFolders },
        { "BTOS2", BTOS2Folders }
    };
    public static readonly string[] FileTypes = [ "png", "jpg" ];

    public abstract void Debug();

    public abstract void Load();

    public abstract void Delete();

    public abstract void Reload();
}