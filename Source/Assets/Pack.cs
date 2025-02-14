namespace FancyUI.Assets;

public abstract class Pack(string name, PackType type)
{
    protected string Name { get; } = name;
    private PackType Type { get; } = type;

    protected bool Deleted { get; set; }

    protected string PackPath => Path.Combine(Fancy.Instance.ModPath, $"{Type}", Name);

    // public static readonly string[] Roles = [ "Admirer", "Amnesiac", "Bodyguard", "Cleric", "Coroner", "Crusader", "Deputy", "Investigator", "Jailor", "Lookout", "Amnesiac", "Amnesiac", "War",
    //     "Amnesiac", "Amnesiac", "Amnesiac", "Amnesiac", "Amnesiac", "Mayor", "Monarch", "Prosecutor", "Psychic", "Retributionist", "Seer", "Sheriff", "Spy", "TavernKeeper", "Tracker", "Death",
    //     "Trapper", "Trickster", "Veteran", "Vigilante", "Conjurer", "CovenLeader", "Dreamweaver", "Enchanter", "HexMaster", "Illusionist", "Jinx", "Medusa", "Necromancer", "Poisoner", "Judge",
    //     "PotionMaster", "Ritualist", "VoodooMaster", "Wildling", "Witch", "Arsonist", "Baker", "Berserker", "Doomsayer", "Executioner", "Jester", "Pirate", "Plaguebearer", "SerialKiller",
    //     "Shroud", "SoulCollector", "Werewolf", "Vampire", "CursedSoul", "Banshee", "Jackal", "Marshal", "Auditor", "Inquisitor", "Starspawn", "Oracle", "Warlock", "Famine", "Pestilence" ];
    private static readonly string[] CommonFolders = [ "Regular", "Town", "Coven", "SerialKiller", "Arsonist", "Werewolf", "Shroud", "Apocalypse", "VIP", "Jester", "Pirate", "Doomsayer",
        "Vampire", "CursedSoul", "Executioner", "Necronomicon", "Factionless" ];
    private static readonly string[] VanillaFolders = [];
    private static readonly string[] Btos2Folders = [ "Judge", "Auditor", "Starspawn", "Inquisitor", "Jackal", "Lions", "Frogs", "Hawks", "Pandora", "Egotist", "Compliance", "Lovers" ];
    protected static readonly string[] MainFolders = [ "Common", "Vanilla", "BTOS2", "PlayerNumbers", "Emojis" ];
    protected static readonly string[] Mods = [ "Vanilla", "BTOS2" ];
    protected static readonly Dictionary<string, string[]> ModsToFolders = new()
    {
        { "Common", CommonFolders },
        { "Vanilla", VanillaFolders },
        { "BTOS2", Btos2Folders }
    };
    protected static readonly string[] FileTypes = [ "png", "jpg" ];

    public abstract void Debug();

    public abstract void Load();

    public abstract void Delete();

    public abstract void Reload();

    public static implicit operator bool(Pack exists) => exists != null;

    protected static ModType GetModKey(string folder)
    {
        var key = ModsToFolders.Find(x => x.Value.Contains(folder)).Key;

        if (StringUtils.IsNullEmptyOrWhiteSpace(key))
            key = "Common";

        return Enum.Parse<ModType>(key);
    }
}