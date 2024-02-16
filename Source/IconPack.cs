namespace IconPacks;

public class IconPack
{
    public Dictionary<string, Dictionary<string, Sprite>> BaseIcons { get; set; }
    public Dictionary<string, Dictionary<string, List<Sprite>>> EasterEggs { get; set; }

    public Dictionary<string, TMP_SpriteAsset> MentionStyles { get; set; }
    public Dictionary<string, TMP_SpriteAsset> BTOS2MentionStyles { get; set; }

    public string Name { get; }

    private string PackPath => Path.Combine(AssetManager.ModPath, Name);

    private static readonly string[] Folders = { "Regular", "Town", "Coven", "SerialKiller", "Arsonist", "Werewolf", "Shroud", "Apocalypse", "Executioner", "Jester", "Pirate",
        "Doomsayer", "Judge", "Auditor", "Starspawn", "Inquisitor", "Vampire", "CursedSoul", "Jackal", "Lions", "Frogs", "Hawks", "VIP", /*"Mafia", "Amnesiac", "Juggernaut",
        "GuardianAngel", "Survivor", "PlayerNumbers",*/ "Custom" };
    public static readonly string[] FileTypes = { "png", "jpg" };

    public IconPack(string name)
    {
        Name = name;
        BaseIcons = new();
        EasterEggs = new();
        MentionStyles = new();
        BTOS2MentionStyles = new();
    }

    public void Debug()
    {
        BaseIcons.ForEach((x, y) => y.ForEach((a, _) => Logging.LogMessage($"{Name} {a} has a(n) {x} sprite!")));
        Logging.LogMessage($"{Name} {BaseIcons.Count} Base Assets loaded!");

        EasterEggs.ForEach((x, y) => y.ForEach((a, b) => Logging.LogMessage($"{Name} {a} has {b.Count} {x} easter egg sprite(s)!")));
        Logging.LogMessage($"{Name} {EasterEggs.Count} Easter Egg Assets loaded!");

        MentionStyles.ForEach((x, _) => Logging.LogMessage($"{Name} {x} mention style exists!"));
        Logging.LogMessage($"{Name} {MentionStyles.Count} mention styles exist!");

        BTOS2MentionStyles.ForEach((x, _) => Logging.LogMessage($"{Name} {x} BTOS2 mention style exists!"));
        Logging.LogMessage($"{Name} {BTOS2MentionStyles.Count} BTOS2 mention styles exist!");
    }

    public void Delete()
    {
        Logging.LogMessage($"Deleteing {Name}");

        BaseIcons.ForEach((_, x) => x.Values.ForEach(UObject.Destroy));
        BaseIcons.ForEach((_, x) => x.Clear());
        BaseIcons.Clear();

        EasterEggs.Values.ForEach(x => x.Values.ForEach(y => y.ForEach(UObject.Destroy)));
        EasterEggs.Values.ForEach(x => x.Values.ForEach(y => y.Clear()));
        EasterEggs.Values.ForEach(x => x.Clear());
        EasterEggs.Clear();

        MentionStyles.Values.ForEach(UObject.Destroy);
        MentionStyles.Clear();

        BTOS2MentionStyles.Values.ForEach(UObject.Destroy);
        BTOS2MentionStyles.Clear();
    }

    public void Reload()
    {
        Logging.LogMessage($"Reloading {Name}");
        Delete();
        Load();
        AssetManager.SetScrollSprites();
        Debug();
    }

    public void Load()
    {
        try
        {
            Logging.LogMessage($"Loading {Name}");

            foreach (var name in Folders)
            {
                if (IsBTOS2ModdedFolder(name) && !Constants.BTOS2Exists)
                    continue;

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
                    Logging.LogWarning($"{Name} {baseName} folder doesn't exist");
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
                        Logging.LogWarning($"{Name} {eeName} folder doesn't exist");
                        Directory.CreateDirectory(eeFolder);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Logging.LogError(e);
        }

        // love ya pat

        var (rolesWithIndexDict, rolesWithIndex) = Utils.Filtered();

        foreach (var style in Folders)
        {
            if (MentionStyles.ContainsKey(style) || (BaseIcons.TryGetValue(style, out var list) && list.Count == 0) || !BaseIcons.ContainsKey(style) || IsBTOS2ModdedFolder(style))
                continue;

            try
            {
                var textures = new List<Texture2D>();
                var sprites = new List<Sprite>();

                // now get all the sprites that we want to load
                foreach (var (role, roleInt) in rolesWithIndex)
                {
                    var actualRole = (Role)roleInt;
                    var name = Utils.RoleName(actualRole);
                    var sprite = GetSprite(name, false, style, false);

                    if (sprite == AssetManager.Blank && style != "Regular")
                        sprite = GetSprite(name, false, "Regular", false);

                    if (sprite == AssetManager.Blank)
                        sprite = Witchcraft.Witchcraft.Assets.TryGetValue(name, out var sprite1) ? sprite1 : AssetManager.Blank;

                    if (sprite != AssetManager.Blank)
                    {
                        sprite.name = sprite.texture.name = role;
                        textures.Add(sprite.texture);
                        sprites.Add(sprite);
                    }
                    else
                        Logging.LogWarning($"NO ICON FOR {name}?!");
                }

                // set spritecharacter name to "Role{number}" so that the game can find correct roles
                var asset = AssetManager.BuildGlyphs(sprites.ToArray(), textures.ToArray(), $"RoleIcons ({Name}, {style})", rolesWithIndexDict);
                MentionStyles[style] = asset;
                Utils.DumpSprite(asset.spriteSheet as Texture2D, $"{style}RoleIcons", PackPath);
            }
            catch (Exception e)
            {
                Logging.LogError(e);
                MentionStyles[style] = null;
            }
        }

        if (Constants.BTOS2Exists)
        {
            (rolesWithIndexDict, rolesWithIndex) = Utils.Filtered(false);

            foreach (var style in Folders)
            {
                if (BTOS2MentionStyles.ContainsKey(style) || (BaseIcons.TryGetValue(style, out var list) && list.Count == 0) || !BaseIcons.ContainsKey(style) || style == "Executioner")
                    continue;

                try
                {
                    var textures = new List<Texture2D>();
                    var sprites = new List<Sprite>();

                    // now get all the sprites that we want to load
                    foreach (var (role, roleInt) in rolesWithIndex)
                    {
                        var name = Utils.RoleName((Role)roleInt, true);
                        var sprite = GetSprite(name, false, style, false);

                        if (sprite == AssetManager.Blank && style != "Regular")
                            sprite = GetSprite(name, false, "Regular", false);

                        if (sprite == AssetManager.Blank)
                            sprite = Witchcraft.Witchcraft.Assets.TryGetValue(name + "_BTOS2", out var sprite1) ? sprite1 : AssetManager.Blank;

                        if (sprite == AssetManager.Blank)
                            sprite = Witchcraft.Witchcraft.Assets.TryGetValue(name, out var sprite1) ? sprite1 : AssetManager.Blank;

                        if (sprite != AssetManager.Blank)
                        {
                            sprite.name = sprite.texture.name = role;
                            textures.Add(sprite.texture);
                            sprites.Add(sprite);
                        }
                        else
                            Logging.LogWarning($"NO BTOS2 ICON FOR {name}?!");
                    }

                    // set spritecharacter name to "Role{number}" so that the game can find correct roles
                    var asset = AssetManager.BuildGlyphs(sprites.ToArray(), textures.ToArray(), $"BTOSRoleIcons ({Name}, {style})", rolesWithIndexDict);
                    BTOS2MentionStyles[style] = asset;
                    Utils.DumpSprite(asset.spriteSheet as Texture2D, $"{style}BTOS2RoleIcons", PackPath);
                }
                catch (Exception e)
                {
                    Logging.LogError(e);
                    BTOS2MentionStyles[style] = null;
                }
            }
        }
    }

    public Sprite GetSprite(string name, bool allowEE, string type, bool log)
    {
        if (!BaseIcons[type].TryGetValue(name, out var sprite))
        {
            if (log)
                Logging.LogWarning($"Couldn't find {name} in {Name}'s {type} resources");

            if (type != "Regular" && !BaseIcons["Regular"].TryGetValue(name, out sprite))
            {
                if (log)
                    Logging.LogWarning($"Couldn't find {name} in {Name}'s Regular resources");

                sprite = null;
            }
        }

        if ((URandom.RandomRangeInt(1, 101) <= Constants.EasterEggChance && allowEE) || !sprite)
        {
            var sprites = new List<Sprite>();

            if (Constants.AllEasterEggs)
            {
                if (!AssetManager.GlobalEasterEggs[type].TryGetValue(name, out sprites))
                {
                    if (type != "Regular")
                        AssetManager.GlobalEasterEggs["Regular"].TryGetValue(name, out sprites);
                }

                sprites ??= new();
            }

            if (sprites.Count == 0)
            {
                if (!EasterEggs[type].TryGetValue(name, out sprites))
                {
                    if (type != "Regular")
                        EasterEggs["Regular"].TryGetValue(name, out sprites);
                }

                sprites ??= new();
            }

            if (sprites.Count > 0)
                return sprites.Random();
        }

        return sprite ?? AssetManager.Blank;
    }

    public static implicit operator bool(IconPack exists) => exists != null;

    private static bool IsBTOS2ModdedFolder(string folderName) => folderName is "Jackal" or "Judge" or "Auditor" or "Starspawn" or "Inquisitor" or "Lions" or "Frogs" or "Hawks";

    //private static bool IsLegacyModdedFolder(string folderName) => folderName is "Mafia" or "Amnesiac" or "Juggernaut" or "GuardianAngel" or "Survivor";
}