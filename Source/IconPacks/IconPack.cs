namespace FancyUI.IconPacks;

public class IconPack(string name)
{
    public Dictionary<ModType, IconAssets> Assets { get; set; } = [];
    public Dictionary<string, Sprite> NumberSprites { get; set; } = [];
    public Dictionary<string, bool> SpriteSheetCanExist { get; set; } = [];
    public TMP_SpriteAsset PlayerNumbers { get; set; }

    public string Name { get; } = name;

    public bool Deleted { get; set; }

    public string PackPath => Path.Combine(AssetManager.ModPath, "IconPacks", Name);

    private static readonly string[] CommonFolders = [ "Regular", "Town", "Coven", "SerialKiller", "Arsonist", "Werewolf", "Shroud", "Apocalypse", "VIP", "Jester", "Pirate", "Doomsayer",
        "Vampire", "CursedSoul", "Executioner", "Necronomicon", "Factionless" ];
    private static readonly string[] VanillaFolders = [];
    private static readonly string[] BTOS2Folders = [ "Judge", "Auditor", "Starspawn", "Inquisitor", "Jackal", "Lions", "Frogs", "Hawks", "Pandora", "Egotist", "Compliance" ];
    private static readonly string[] MainFolders = [ "Common", "Vanilla", "BTOS2", "PlayerNumbers" ];
    private static readonly string[] Mods = [ "Vanilla", "BTOS2" ];
    private static readonly Dictionary<string, string[]> ModsToFolders = new()
    {
        { "Common", CommonFolders },
        { "Vanilla", VanillaFolders },
        { "BTOS2", BTOS2Folders }
    };
    public static readonly string[] FileTypes = [ "png", "jpg" ];

    public void Debug()
    {
        Logging.LogMessage($"Debugging {Name}");

        var count = 0;

        foreach (var assets in Assets.Values)
        {
            assets.Debug();
            count += assets.Count;
        }

        Logging.LogMessage($"{Name} {Assets.Count} asset sets loaded!");
        Logging.LogMessage($"{Name} {count} total assets exist!");

        if (PlayerNumbers)
            Logging.LogMessage($"{Name} has a PlayerNumbers sprite sheet!");

        Logging.LogMessage($"{Name} Debugged!");
    }

    public void Delete()
    {
        if (Deleted)
            return;

        Logging.LogMessage($"Deleteing {Name}", true);
        Assets.Values.ForEach(x => x.Delete());
        Logging.LogMessage($"{Name} Deleted!", true);
    }

    public void Reload()
    {
        Logging.LogMessage($"Reloading {Name}", true);
        Delete();
        Load();
        AssetManager.SetScrollSprites();
        Debug();
        Logging.LogMessage($"{Name} Reloaded!", true);
    }

    public void Load()
    {
        if (Name is "Vanilla" or "BTOS2")
        {
            Deleted = true;
            return;
        }

        Logging.LogMessage($"Loading {Name}", true);
        Deleted = false;

        try
        {
            foreach (var mod in MainFolders)
            {
                if (Enum.TryParse<ModType>(mod, out var type))
                {
                    var assets = Assets[type] = new(mod);

                    if (type == ModType.BTOS2 && !Constants.BTOS2Exists())
                        continue;

                    var modPath = Path.Combine(PackPath, mod);

                    foreach (var name1 in ModsToFolders[mod])
                    {
                        if (!AssetManager.GlobalEasterEggs.ContainsKey(name1))
                            AssetManager.GlobalEasterEggs[name1] = [];

                        assets.BaseIcons[name1] = [];
                        assets.EasterEggs[name1] = [];
                        var baseName = name1 + "Base";
                        var baseFolder = Path.Combine(modPath, baseName);

                        if (Directory.Exists(baseFolder))
                        {
                            foreach (var type1 in FileTypes)
                            {
                                foreach (var file in Directory.GetFiles(baseFolder, $"*.{type1}"))
                                {
                                    var filePath = Path.Combine(baseFolder, $"{file.SanitisePath()}.{type1}");
                                    var sprite = AssetManager.LoadDiskSprite(filePath.SanitisePath(), baseName, mod, Name, type1);

                                    if (sprite.IsValid())
                                        assets.BaseIcons[name1][filePath.SanitisePath(true)] = sprite;
                                }
                            }
                        }
                        else
                        {
                            Logging.LogWarning($"{Name} {mod} {baseName} folder doesn't exist");
                            Directory.CreateDirectory(baseFolder);
                        }

                        var eeName = name1 + "EasterEggs";
                        var eeFolder = Path.Combine(modPath, eeName);

                        if (Directory.Exists(eeFolder))
                        {
                            foreach (var type1 in FileTypes)
                            {
                                foreach (var file in Directory.GetFiles(eeFolder, $"*.{type1}"))
                                {
                                    var filePath = Path.Combine(eeFolder, $"{file.SanitisePath()}.{type1}");
                                    var sprite = AssetManager.LoadDiskSprite(filePath.SanitisePath(), eeName, mod, Name, type1);
                                    filePath = filePath.SanitisePath(true);

                                    if (!sprite.IsValid())
                                        continue;

                                    if (!assets.EasterEggs[name1].ContainsKey(filePath))
                                        assets.EasterEggs[name1][filePath] = [];

                                    if (!AssetManager.GlobalEasterEggs[name1].ContainsKey(filePath))
                                        AssetManager.GlobalEasterEggs[name1][filePath] = [];

                                    assets.EasterEggs[name1][filePath].Add(sprite);
                                    AssetManager.GlobalEasterEggs[name1][filePath].Add(sprite);
                                }
                            }
                        }
                        else
                        {
                            Logging.LogWarning($"{Name} {mod} {eeName} folder doesn't exist");
                            Directory.CreateDirectory(eeFolder);
                        }
                    }

                    if (type != ModType.Common)
                        continue;

                    if (!AssetManager.GlobalEasterEggs.ContainsKey("Custom"))
                        AssetManager.GlobalEasterEggs["Custom"] = [];

                    assets.BaseIcons["Custom"] = [];
                    assets.EasterEggs["Custom"] = [];
                    var folder = Path.Combine(modPath, "Custom");

                    if (Directory.Exists(folder))
                    {
                        foreach (var type1 in FileTypes)
                        {
                            foreach (var file in Directory.GetFiles(folder, $"*.{type1}"))
                            {
                                var filePath = Path.Combine(folder, $"{file.SanitisePath()}.{type1}");
                                var sprite = AssetManager.LoadDiskSprite(filePath.SanitisePath(), "Custom", mod, Name, type1);

                                if (sprite.IsValid())
                                    assets.BaseIcons["Custom"][filePath.SanitisePath(true)] = sprite;
                            }
                        }
                    }
                    else
                    {
                        Logging.LogWarning($"{Name} {mod} Custom folder doesn't exist");
                        Directory.CreateDirectory(folder);
                    }
                }
                else
                {

                    if (!AssetManager.GlobalEasterEggs.ContainsKey("PlayerNumbers"))
                        AssetManager.GlobalEasterEggs["PlayerNumbers"] = [];

                    var folder = Path.Combine(PackPath, "PlayerNumbers");

                    if (Directory.Exists(folder))
                    {
                        foreach (var type1 in FileTypes)
                        {
                            foreach (var file in Directory.GetFiles(folder, $"*.{type1}"))
                            {
                                var filePath = Path.Combine(folder, $"{file.SanitisePath()}.{type1}");
                                var sprite = AssetManager.LoadDiskSprite(filePath.SanitisePath(), "PlayerNumbers", Name, type1);

                                if (sprite.IsValid())
                                    NumberSprites[filePath.SanitisePath(true)] = sprite;
                            }
                        }
                    }
                    else
                    {
                        Logging.LogWarning($"{Name} PlayerNumbers folder doesn't exist");
                        Directory.CreateDirectory(folder);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Logging.LogError($"Unable to load sprites for {Name} because:\n{e}");
        }

        Logging.LogMessage($"Loaded {Name} sprites");

        // love ya pat
        var numbers = new List<string>();

        if (NumberSprites.Count > 0)
        {
            try
            {
                var sprites = new List<Sprite>();

                for (var i = 0; i < 16; i++)
                {
                    if (!NumberSprites.TryGetValue($"{i}", out var sprite))
                        sprite = AssetManager.Assets.TryGetValue($"{i}", out var sprite1) ? sprite1 : AssetManager.Blank;

                    if (sprite.IsValid())
                    {
                        sprite.name = sprite.texture.name = $"PlayerNumbers_{i}";
                        sprites.Add(sprite);
                    }
                    else
                        Logging.LogWarning($"NO NUMBER ICON FOR {i}?!");

                    numbers.Add($"PlayerNumbers_{i}");
                }

                PlayerNumbers = AssetManager.BuildGlyphs([..sprites], $"PlayerNumbers ({Name})", numbers.ToDictionary(x => x, x => (x, 0)), false);
                Utils.DumpSprite(PlayerNumbers.spriteSheet as Texture2D, "PlayerNumbers", Path.Combine(PackPath, "PlayerNumbers"));
            }
            catch (Exception e)
            {
                Logging.LogError($"Unable to create custom player numbers for {Name} because:\n{e}");
                PlayerNumbers = null;
            }
        }

        foreach (var (mod, assets) in Assets)
        {
            if (mod is ModType.Common or ModType.None)
                continue;

            foreach (var (style, icons) in assets.BaseIcons)
            {
                if (icons.Count == 0/* || (style != "Regular" && style != Constants.CurrentStyle(mod))*/)
                    continue;

                try
                {
                    var asset = BuildSpriteSheet(mod, mod.ToString(), style, icons);
                    assets.MentionStyles[style] = asset;
                    SpriteSheetCanExist[style] = true;

                    if (asset)
                        Utils.DumpSprite(asset.spriteSheet as Texture2D, $"{style}{mod}RoleIcons", Path.Combine(PackPath, $"{mod}"));
                }
                catch (Exception e)
                {
                    Logging.LogError($"Unable to create custom role icons for {Name} {style} because:\n{e}");
                    assets.MentionStyles[style] = null;
                }
            }
        }

        foreach (var (style, icons) in Assets[ModType.Common].BaseIcons)
        {
            if (icons.Count == 0/* || (style != "Regular" && style != Constants.CurrentStyle(ModType.Vanilla) && style != Constants.CurrentStyle(ModType.BTOS2))*/)
                continue;

            foreach (var mod in Mods)
            {
                try
                {
                    var type = Enum.Parse<ModType>(mod);
                    var asset = BuildSpriteSheet(type, mod, style, icons);
                    Assets[type].MentionStyles[style] = asset;
                    SpriteSheetCanExist[style] = true;

                    if (asset)
                        Utils.DumpSprite(asset.spriteSheet as Texture2D, $"{style}{mod}RoleIcons", Path.Combine(PackPath, mod));
                }
                catch (Exception e)
                {
                    Logging.LogError($"Unable to create custom role icons for {Name} {style} because:\n{e}");
                    Assets[ModType.Vanilla].MentionStyles[style] = Assets[ModType.BTOS2].MentionStyles[style] = null;
                }
            }
        }

        Logging.LogMessage($"{Name} Loaded!", true);
    }

    public TMP_SpriteAsset BuildSpriteSheet(ModType type, string mod, string style, Dictionary<string, Sprite> icons)
    {
        if (type == ModType.BTOS2 && !Constants.BTOS2Exists())
            return null;

        var index = Utils.Filtered(type);
        var sprites = new List<Sprite>();

        foreach (var (role, (_, roleInt)) in index)
        {
            var roleEnum = (Role)roleInt;
            var name2 = Utils.RoleName(roleEnum, type);
            var factionEnum = roleEnum.GetFactionType(type);
            var name3 = Utils.FactionName(factionEnum, type);
            var sprite = icons.TryGetValue(name2 + $"_{mod}", out var sprite1) ? sprite1 : AssetManager.Blank;

            if (!sprite.IsValid())
                sprite = icons.TryGetValue(name2, out sprite1) ? sprite1 : AssetManager.Blank;

            if (!sprite.IsValid() && name3 != style && Assets[type].BaseIcons.TryGetValue(name3, out var icons2))
            {
                sprite = icons2.TryGetValue(name2 + $"_{mod}", out sprite1) ? sprite1 : AssetManager.Blank;

                if (!sprite.IsValid())
                    sprite = icons2.TryGetValue(name2, out sprite1) ? sprite1 : AssetManager.Blank;
            }

            if (!sprite.IsValid() && name3 != style && Assets[ModType.Common].BaseIcons.TryGetValue(name3, out icons2))
            {
                sprite = icons2.TryGetValue(name2 + $"_{mod}", out sprite1) ? sprite1 : AssetManager.Blank;

                if (!sprite.IsValid())
                    sprite = icons2.TryGetValue(name2, out sprite1) ? sprite1 : AssetManager.Blank;
            }

            if (!sprite.IsValid() && style != "Regular" && Assets[ModType.Common].BaseIcons.TryGetValue("Regular", out icons2))
            {
                sprite = icons2.TryGetValue(name2 + $"_{mod}", out sprite1) ? sprite1 : AssetManager.Blank;

                if (!sprite.IsValid())
                    sprite = icons2.TryGetValue(name2, out sprite1) ? sprite1 : AssetManager.Blank;
            }

            if (!sprite.IsValid())
                sprite = AssetManager.Assets.TryGetValue(name2 + $"_{mod}", out sprite1) ? sprite1 : AssetManager.Blank;

            if (!sprite.IsValid())
                sprite = AssetManager.Assets.TryGetValue(name2, out sprite1) ? sprite1 : AssetManager.Blank;

            if (sprite.IsValid())
            {
                sprite.name = sprite.texture.name = role;
                sprites.Add(sprite);
            }
            else
                Logging.LogWarning($"NO {mod.ToUpper()} ICON FOR {name2}?!");
        }

        return AssetManager.BuildGlyphs([..sprites], $"{mod}RoleIcons ({Name}, {style})", index);
    }

    public Sprite GetSprite(string iconName, bool allowEE, string type)
    {
        var key = GetModKey(type);

        if (!Assets.TryGetValue(Enum.Parse<ModType>(key), out var assets))
            return AssetManager.Blank;

        if (!assets.BaseIcons.TryGetValue(type, out var icons))
        {
            if (type != "Regular")
                Assets[ModType.Common].BaseIcons.TryGetValue("Regular", out icons);
        }

        icons ??= [];
        icons.TryGetValue(iconName, out var sprite);

        if (URandom.RandomRangeInt(1, 101) <= Constants.EasterEggChance() && (allowEE || !sprite.IsValid()))
        {
            var sprites = new List<Sprite>();

            if (Constants.AllEasterEggs())
            {
                if (!AssetManager.GlobalEasterEggs[type].TryGetValue(iconName, out sprites))
                {
                    if (type != "Regular")
                        AssetManager.GlobalEasterEggs["Regular"].TryGetValue(iconName, out sprites);
                }

                sprites ??= [];
            }

            if (sprites.Count == 0)
            {
                if (!assets.EasterEggs.TryGetValue(type, out var icons3))
                {
                    if (type != "Regular")
                        Assets[ModType.Common].EasterEggs.TryGetValue("Regular", out icons3);
                }

                icons3 ??= [];

                if (!icons3.TryGetValue(iconName, out sprites))
                {
                    if (type != "Regular")
                        Assets[ModType.Common].EasterEggs["Regular"].TryGetValue(iconName, out sprites);
                }

                sprites ??= [];
            }

            if (sprites.Count > 0)
                return sprites.Random();
        }

        return sprite ?? AssetManager.Blank;
    }

    public static implicit operator bool(IconPack exists) => exists != null;

    public static string GetModKey(string folder)
    {
        var key = "";

        foreach (var (key2, folders) in ModsToFolders)
        {
            if (folders.Contains(folder))
                key = key2;
        }

        if (StringUtils.IsNullEmptyOrWhiteSpace(key))
            key = "Common";

        return key;
    }
}