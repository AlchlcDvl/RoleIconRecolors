namespace IconPacks;

public class IconPack
{
    public Dictionary<string, Dictionary<string, Sprite>> BaseIcons { get; set; }
    public Dictionary<string, Dictionary<string, List<Sprite>>> EasterEggs { get; set; }

    public Dictionary<string, TMP_SpriteAsset> MentionStyles { get; set; }

    public string Name { get; }

    private string PackPath => Path.Combine(AssetManager.ModPath, Name);

    private static readonly string[] Folders = new[] { "Regular", "Town", "Coven", "SerialKiller", "Arsonist", "Werewolf", "Shroud", "Apocalypse", "Executioner", "Jester", "Pirate",
        "Doomsayer", "Vampire", "CursedSoul", /*"PlayerNumbers",*/ "Custom" };
    public static readonly string[] FileTypes = new[] { "png", "jpg" };

    public IconPack(string name)
    {
        Name = name;
        BaseIcons = new();
        EasterEggs = new();
        MentionStyles = new();
    }

    public void Debug()
    {
        BaseIcons.ForEach((x, y) => y.ForEach((a, _) => Recolors.LogMessage($"{Name} {a} has a(n) {x} sprite!")));
        Recolors.LogMessage($"{Name} {BaseIcons.Count} Base Assets loaded!");

        EasterEggs.ForEach((x, y) => y.ForEach((a, b) => Recolors.LogMessage($"{Name} {a} has {b.Count} {x} easter egg sprite(s)!")));
        Recolors.LogMessage($"{Name} {EasterEggs.Count} Easter Egg Assets loaded!");

        MentionStyles.ForEach((x, _) => Recolors.LogMessage($"{Name} {x} mention style exists!"));
        Recolors.LogMessage($"{Name} {MentionStyles.Count} mention styles exist!");
    }

    public void Delete()
    {
        Recolors.LogMessage($"Deleteing {Name}");

        BaseIcons.ForEach((_, x) => x.Values.ForEach(UObject.Destroy));
        BaseIcons.ForEach((_, x) => x.Clear());
        BaseIcons.Clear();

        EasterEggs.Values.ForEach(x => x.Values.ForEach(y => y.ForEach(UObject.Destroy)));
        EasterEggs.Values.ForEach(x => x.Values.ForEach(y => y.Clear()));
        EasterEggs.Values.ForEach(x => x.Clear());
        EasterEggs.Clear();

        MentionStyles.Values.ForEach(UObject.Destroy);
        MentionStyles.Clear();
    }

    public void Reload()
    {
        Recolors.LogMessage($"Reloading {Name}");
        Delete();
        Load();
        LoadSpriteSheets(true);
        AssetManager.SetScrollSprites();
    }

    public void Load()
    {
        try
        {
            Recolors.LogMessage($"Loading {Name}");

            foreach (var name in Folders)
            {
                if (!AssetManager.GlobalEasterEggs.ContainsKey(name))
                    AssetManager.GlobalEasterEggs[name] = new();

                BaseIcons[name] = new();
                EasterEggs[name] = new();
                var baseName = name + (name is "Custom" or "PlayerNumbers" ? "" : "Base");
                var baseFolder = Path.Combine(PackPath, baseName);

                if (Directory.Exists(baseFolder))
                {
                    foreach (var type in FileTypes)
                    {
                        foreach (var file in Directory.GetFiles(baseFolder, $"*.{type}"))
                        {
                            var filePath = Path.Combine(baseFolder, $"{file.SanitisePath()}.{type}");
                            var sprite = AssetManager.LoadDiskSprite(filePath.SanitisePath(), baseName, Name, type);

                            if (sprite == null)
                                continue;

                            BaseIcons[name][filePath.SanitisePath(true)] = sprite;
                        }
                    }
                }
                else
                {
                    Recolors.LogWarning($"{Name} {baseName} folder doesn't exist");
                    Directory.CreateDirectory(baseFolder);
                }

                if (name is not ("Custom" or "PlayerNumbers"))
                {
                    var eeName = name + "EasterEggs";
                    var eeFolder = Path.Combine(PackPath, eeName);

                    if (Directory.Exists(eeFolder))
                    {
                        foreach (var type in FileTypes)
                        {
                            foreach (var file in Directory.GetFiles(eeFolder, $"*.{type}"))
                            {
                                var filePath = Path.Combine(eeFolder, $"{file.SanitisePath()}.{type}");
                                var sprite = AssetManager.LoadDiskSprite(filePath.SanitisePath(), eeName, Name, type);
                                filePath = filePath.SanitisePath(true);

                                if (sprite == null)
                                    continue;

                                if (!EasterEggs[name].ContainsKey(filePath))
                                    EasterEggs[name][filePath] = new();

                                if (!AssetManager.GlobalEasterEggs[name].ContainsKey(filePath))
                                    AssetManager.GlobalEasterEggs[name][filePath] = new();

                                EasterEggs[name][filePath].Add(sprite);
                                AssetManager.GlobalEasterEggs[name][filePath].Add(sprite);
                            }
                        }
                    }
                    else
                    {
                        Recolors.LogWarning($"{Name} {eeName} folder doesn't exist");
                        Directory.CreateDirectory(eeFolder);
                    }
                }
            }

            Debug();
        }
        catch (Exception e)
        {
            Recolors.LogError(e);
        }
    }

    // love ya pat
    public void LoadSpriteSheets(bool change)
    {
        try
        {
            var (rolesWithIndexDict, rolesWithIndex) = Utils.Filtered();

            foreach (var style in Folders)
            {
                if (MentionStyles.ContainsKey(style))
                    continue;

                var textures = new List<Texture2D>();
                var sprites = new List<Sprite>();

                // now get all the sprites that we want to load
                foreach (var (role, roleInt) in rolesWithIndex)
                {
                    var actualRole = (Role)roleInt;
                    var name = Utils.RoleName(actualRole);
                    var sprite = AssetManager.GetSprite(name, style, false, Name, false);

                    if (sprite == AssetManager.Blank)
                        sprite = AssetManager.GetSprite(name, "Regular", false, Name, false);

                    if (sprite == AssetManager.Blank)
                        sprite = Service.Game.Roles.roleInfoLookup[actualRole].sprite;

                    sprite.name = sprite.texture.name = role;
                    textures.Add(sprite.texture);
                    sprites.Add(sprite);
                }

                // set spritecharacter name to "Role{number}" so that the game can find correct roles
                var asset = AssetManager.BuildGlyphs(sprites.ToArray(), textures.ToArray(), $"RoleIcons ({Name}, {style})", rolesWithIndexDict);
                MentionStyles[style] = asset;
                Utils.DumpSprite(asset.spriteSheet as Texture2D, $"{style}RoleIcons.png", PackPath);
                Recolors.LogMessage($"{Name} {style} Sprite Asset loaded!");
            }

            if (change)
            {
                AssetManager.ChangeSpriteSheets(Name);
                Recolors.LogMessage($"Changed to {Name} {Constants.CurrentStyle} Sprite Asset!");
            }
        }
        catch (Exception e)
        {
            Recolors.LogError(e);
        }
    }

    public Sprite GetSprite(string name, bool allowEE, string type, bool log)
    {
        if (!BaseIcons[type].TryGetValue(name, out var sprite))
        {
            if (log)
                Recolors.LogWarning($"Couldn't find {name} in {Name}'s {type} resources");

            if (type != "Regular" && !BaseIcons["Regular"].TryGetValue(name, out sprite))
            {
                if (log)
                    Recolors.LogWarning($"Couldn't find {name} in {Name}'s Regular resources");

                sprite = null;
            }
        }

        if (URandom.RandomRangeInt(1, 101) <= Constants.EasterEggChance && allowEE)
        {
            var sprites = new List<Sprite>();

            if (Constants.AllEasterEggs)
            {
                if (!AssetManager.GlobalEasterEggs[type].TryGetValue(name, out sprites))
                {
                    if (type != "Regular")
                        AssetManager.GlobalEasterEggs["Regular"].TryGetValue(name, out sprites);
                }
            }

            if (sprites.Count == 0)
            {
                if (!EasterEggs[type].TryGetValue(name, out sprites))
                {
                    if (type != "Regular")
                        EasterEggs["Regular"].TryGetValue(name, out sprites);
                }
            }

            if (sprites.Count > 0)
                return sprites.Random();
        }

        return sprite ?? AssetManager.Blank;
    }

    //private static bool IsBTOS2ModdedFolder(string folderName) => folderName is "Jackal" or "Judge" or "Auditor" or "Starspawn" or "Inquisitor" or "Lions" or "Frogs" or "Hawks";

    //private static bool IsLegacyModdedFolder(string folderName) => folderName is "Mafia" or "Amnesiac" or "Juggernaut" or "Sorcerer" or "GuardianAngel" or "Survivor";
}