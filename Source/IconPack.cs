namespace IconPacks;

public class IconPack(string name)
{
    public Dictionary<ModType, IconAssets> Assets { get; set; } = [];
    public Dictionary<string, Sprite> NumberSprites { get; set; } = [];
    public TMP_SpriteAsset PlayerNumbers { get; set; }

    public string Name { get; } = name;

    private string PackPath => Path.Combine(AssetManager.ModPath, Name);

    private static readonly string[] VanillaFolders = [ "Regular", "Town", "Coven", "SerialKiller", "Arsonist", "Werewolf", "Shroud", "Apocalypse", "VIP", "Jester", "Pirate", "Doomsayer",
        "Vampire", "CursedSoul", "Executioner" ];
    /*private static readonly string[] LegacyFolders = [ "Regular", "Town", "Coven", "SerialKiller", "Arsonist", "Werewolf", "Shroud", "Apocalypse", "VIP", "Jester", "Pirate", "Doomsayer",
        "Vampire", "CursedSoul", "Mafia", "Amnesiac", "Juggernaut", "GuardianAngel", "Evils" ];*/
    private static readonly string[] BTOS2Folders = [ "Regular", "Town", "Coven", "SerialKiller", "Arsonist", "Werewolf", "Shroud", "Apocalypse", "VIP", "Jester", "Pirate", "Doomsayer",
        "Vampire", "CursedSoul", "Judge", "Auditor", "Starspawn", "Inquisitor", "Jackal", "Lions", "Frogs", "Hawks", "Pandora", "Egoist" ];
    private static readonly string[] Mods = [ "Vanilla", "BTOS2"/*, "Legacy"*/, "PlayerNumbers" ];
    private static readonly Dictionary<string, string[]> ModsToFolders = new() { { "Vanilla", VanillaFolders }, { "BTOS2", BTOS2Folders }/*, { "Legacy", LegacyFolders }*/ };
    public static readonly string[] FileTypes = [ "png", "jpg" ];

    public void Debug()
    {
        Logging.LogMessage($"Debugging {Name}");

        Assets.ForEach((_, y) => y.Debug());
        Logging.LogMessage($"{Name} {Assets.Count} asset sets loaded!");

        var count = 0;
        Assets.ForEach((_, y) => count += y.CountAssets());
        Logging.LogMessage($"{Name} {count} assets exist!");

        if (PlayerNumbers)
            Logging.LogMessage($"{Name} has a PlayerNumbers sprite sheet!");

        Logging.LogMessage($"{Name} Debugged!");
    }

    public void Delete()
    {
        Logging.LogMessage($"Deleteing {Name}", true);
        Assets.ForEach((_, y) => y.Delete());
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
            return;

        Logging.LogMessage($"Loading {Name}", true);

        try
        {
            foreach (var mod in Mods)
            {
                if (Enum.TryParse<ModType>(mod, out var type))
                {
                    if (type == ModType.BTOS2 && !Constants.BTOS2Exists)
                    //if ((type == ModType.BTOS2 && !Constants.BTOS2Exists) || (type == ModType.Legacy && !Constants.LegacyExists))
                        continue;

                    var assets = Assets[type] = new(mod);
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
                        sprite = Witchcraft.Witchcraft.Assets.TryGetValue($"{i}", out var sprite1) ? sprite1 : AssetManager.Blank;

                    if (sprite.IsValid())
                    {
                        sprite.name = sprite.texture.name = $"PlayerNumbers_{i}";
                        sprites.Add(sprite);
                    }
                    else
                        Logging.LogWarning($"NO NUMBER ICON FOR {i}?!");

                    numbers.Add($"PlayerNumbers_{i}");
                }

                PlayerNumbers = AssetManager.BuildGlyphs([..sprites], [..sprites.Select(x => x.texture)], $"PlayerNumbers ({Name})", numbers.ToDictionary(x => x, x => (x, 0)), false);
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
            foreach (var (style, icons) in assets.BaseIcons)
            {
                if (icons.Count == 0)
                    continue;

                try
                {
                    var index = Utils.Filtered(mod);
                    var sprites = new List<Sprite>();

                    foreach (var (role, (_, roleInt)) in index)
                    {
                        var name2 = Utils.RoleName((Role)roleInt, mod);
                        var sprite = icons.TryGetValue(name2, out var sprite1) ? sprite1 : AssetManager.Blank;

                        if (!sprite.IsValid() && style != "Regular" && assets.BaseIcons.TryGetValue("Regular", out var icons2))
                            sprite = icons2.TryGetValue(name2, out sprite1) ? sprite1 : AssetManager.Blank;

                        if (!sprite.IsValid())
                            sprite = Witchcraft.Witchcraft.Assets.TryGetValue(name2 + $"_{mod}", out sprite1) ? sprite1 : AssetManager.Blank;

                        if (!sprite.IsValid())
                            sprite = Witchcraft.Witchcraft.Assets.TryGetValue(name2, out sprite1) ? sprite1 : AssetManager.Blank;

                        if (sprite.IsValid())
                        {
                            sprite.name = sprite.texture.name = role;
                            sprites.Add(sprite);
                        }
                        else
                            Logging.LogWarning($"NO {mod.ToString().ToUpper()} ICON FOR {name2}?!");
                    }

                    var asset = AssetManager.BuildGlyphs([..sprites], [..sprites.Select(x => x.texture)], $"{mod}RoleIcons ({Name}, {style})", index);
                    assets.MentionStyles[style] = asset;
                    Utils.DumpSprite(asset.spriteSheet as Texture2D, $"{style}{mod}RoleIcons", Path.Combine(PackPath, $"{mod}"));
                }
                catch (Exception e)
                {
                    Logging.LogError($"Unable to create custom role icons for {Name} {style} because:\n{e}");
                    assets.MentionStyles[style] = null;
                }
            }
        }

        Logging.LogMessage($"{Name} Loaded!", true);
    }

    public Sprite GetSprite(string iconName, bool allowEE, string type, ModType? mod = null)
    {
        mod ??= Utils.GetGameType();

        if (!Assets.TryGetValue(mod.Value, out var assets))
        {
            Logging.LogError($"Error finding {iconName} in {Name}'s {type} > {mod} resources");
            return AssetManager.Blank;
        }

        if (!assets.BaseIcons[type].TryGetValue(iconName + $"_{mod}", out var sprite))
        {
            if (type != "Regular")
                assets.BaseIcons["Regular"].TryGetValue(iconName + $"_{mod}", out sprite);
        }

        if (!sprite.IsValid() && !assets.BaseIcons[type].TryGetValue(iconName, out sprite))
        {
            if (type != "Regular")
                assets.BaseIcons["Regular"].TryGetValue(iconName, out sprite);
        }

        if (!sprite.IsValid() || (URandom.RandomRangeInt(1, 101) <= Constants.EasterEggChance && allowEE))
        {
            var sprites = new List<Sprite>();

            if (Constants.AllEasterEggs)
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
                if (!assets.EasterEggs[type].TryGetValue(iconName, out sprites))
                {
                    if (type != "Regular")
                        assets.EasterEggs["Regular"].TryGetValue(iconName, out sprites);
                }

                sprites ??= [];
            }

            if (sprites.Count > 0)
                return sprites.Random();
        }

        return sprite ?? AssetManager.Blank;
    }

    public static implicit operator bool(IconPack exists) => exists != null;
}