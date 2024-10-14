namespace FancyUI.Assets.IconPacks;

public class IconPack(string name) : Pack(name, PackType.IconPacks)
{
    public Dictionary<ModType, IconAssets> Assets { get; set; } = [];
    public Dictionary<string, Sprite> NumberSprites { get; set; } = [];
    public Dictionary<string, bool> SpriteSheetCanExist { get; set; } = [];
    public TMP_SpriteAsset PlayerNumbers { get; set; }

    public override void Debug()
    {
        Fancy.Instance.Message($"Debugging {Name}");

        var count = 0;

        foreach (var assets in Assets.Values)
        {
            assets.Debug();
            count += assets.Count;
        }

        if (PlayerNumbers)
        {
            Fancy.Instance.Message($"{Name} has a PlayerNumbers sprite sheet!");
            count++;
        }

        Fancy.Instance.Message($"{Name} {Assets.Count} asset sets loaded!");
        Fancy.Instance.Message($"{Name} {count} total assets exist!");
        Fancy.Instance.Message($"{Name} Debugged!");
    }

    public override void Delete()
    {
        if (Deleted)
            return;

        Fancy.Instance.Message($"Deleteing {Name}", true);
        Assets.Values.ForEach(x => x.Delete());
        Fancy.Instance.Message($"{Name} Deleted!", true);
    }

    public override void Reload()
    {
        Fancy.Instance.Message($"Reloading {Name}", true);
        Delete();
        Load();
        SetScrollSprites();
        Debug();
        Fancy.Instance.Message($"{Name} Reloaded!", true);
    }

    public override void Load()
    {
        if (Name is "Vanilla" or "BTOS2")
        {
            Deleted = true;
            return;
        }

        Fancy.Instance.Message($"Loading {Name} Icon Pack", true);
        Deleted = false;

        try
        {
            foreach (var mod in MainFolders)
            {
                var modPath = Path.Combine(PackPath, mod);

                if (!Directory.Exists(modPath))
                {
                    Directory.CreateDirectory(modPath);
                    Fancy.Instance.Warning($"{Name} {mod} folder doesn't exist");
                    continue;
                }

                if (Enum.TryParse<ModType>(mod, out var type))
                {
                    var assets = Assets[type] = new(mod);

                    if (type == ModType.BTOS2 && !Constants.BTOS2Exists())
                        continue;

                    foreach (var name1 in ModsToFolders[mod])
                    {
                        if (!GlobalEasterEggs.ContainsKey(name1))
                            GlobalEasterEggs[name1] = [];

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
                                    var sprite = AssetManager.LoadSpriteFromDisk(file);

                                    if (sprite.IsValid())
                                        assets.BaseIcons[name1][file.FancySanitisePath(true)] = sprite;
                                }
                            }
                        }
                        else
                        {
                            Fancy.Instance.Warning($"{Name} {mod} {baseName} folder doesn't exist");
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
                                    var sprite = AssetManager.LoadSpriteFromDisk(file);
                                    var name = file.FancySanitisePath(true);

                                    if (!sprite.IsValid())
                                        continue;

                                    if (!assets.EasterEggs[name1].ContainsKey(name))
                                        assets.EasterEggs[name1][name] = [];

                                    if (!GlobalEasterEggs[name1].ContainsKey(name))
                                        GlobalEasterEggs[name1][name] = [];

                                    assets.EasterEggs[name1][name].Add(sprite);
                                    GlobalEasterEggs[name1][name].Add(sprite);
                                }
                            }
                        }
                        else
                        {
                            Fancy.Instance.Warning($"{Name} {mod} {eeName} folder doesn't exist");
                            Directory.CreateDirectory(eeFolder);
                        }
                    }

                    if (type != ModType.Common)
                        continue;

                    if (!GlobalEasterEggs.ContainsKey("Custom"))
                        GlobalEasterEggs["Custom"] = [];

                    assets.BaseIcons["Custom"] = [];
                    assets.EasterEggs["Custom"] = [];
                    var folder = Path.Combine(modPath, "Custom");

                    if (Directory.Exists(folder))
                    {
                        foreach (var type1 in FileTypes)
                        {
                            foreach (var file in Directory.GetFiles(folder, $"*.{type1}"))
                            {
                                var sprite = AssetManager.LoadSpriteFromDisk(file);

                                if (sprite.IsValid())
                                    assets.BaseIcons["Custom"][file.FancySanitisePath(true)] = sprite;
                            }
                        }
                    }
                    else
                    {
                        Fancy.Instance.Warning($"{Name} {mod} Custom folder doesn't exist");
                        Directory.CreateDirectory(folder);
                    }
                }
                else
                {
                    foreach (var type1 in FileTypes)
                    {
                        foreach (var file in Directory.GetFiles(modPath, $"*.{type1}"))
                        {
                            var sprite = AssetManager.LoadSpriteFromDisk(file);

                            if (sprite.IsValid())
                                NumberSprites[file.FancySanitisePath(true)] = sprite;
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Fancy.Instance.Error($"Unable to load sprites for {Name} because:\n{e}");
        }

        Fancy.Instance.Message($"Loaded {Name} sprites");

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
                        sprite = Fancy.Assets.GetSprite($"{i}") ?? Blank;

                    if (sprite.IsValid())
                    {
                        sprite.name = sprite.texture.name = $"PlayerNumbers_{i}";
                        sprites.Add(sprite);
                    }
                    else
                        Fancy.Instance.Warning($"NO NUMBER ICON FOR {i}?!");

                    numbers.Add($"PlayerNumbers_{i}");
                }

                PlayerNumbers = BuildGlyphs([..sprites], $"PlayerNumbers ({Name})", numbers.ToDictionary(x => x, x => (x, 0)), false);
                Utils.DumpSprite(PlayerNumbers.spriteSheet as Texture2D, "PlayerNumbers", Path.Combine(PackPath, "PlayerNumbers"));
            }
            catch (Exception e)
            {
                Fancy.Instance.Error($"Unable to create custom player numbers for {Name} because:\n{e}");
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
                    Fancy.Instance.Error($"Unable to create custom role icons for {Name} {style} because:\n{e}");
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
                    Fancy.Instance.Error($"Unable to create custom role icons for {Name} {style} because:\n{e}");
                    Assets[ModType.Vanilla].MentionStyles[style] = Assets[ModType.BTOS2].MentionStyles[style] = null;
                }
            }
        }

        Fancy.Instance.Message($"{Name} Loaded!", true);
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
            var sprite = icons.TryGetValue(name2 + $"_{mod}", out var sprite1) ? sprite1 : Blank;

            if (!sprite.IsValid())
                sprite = icons.TryGetValue(name2, out sprite1) ? sprite1 : Blank;

            if (!sprite.IsValid() && name3 != style && Assets[type].BaseIcons.TryGetValue(name3, out var icons2))
            {
                sprite = icons2.TryGetValue(name2 + $"_{mod}", out sprite1) ? sprite1 : Blank;

                if (!sprite.IsValid())
                    sprite = icons2.TryGetValue(name2, out sprite1) ? sprite1 : Blank;
            }

            if (!sprite.IsValid() && name3 != style && Assets[ModType.Common].BaseIcons.TryGetValue(name3, out icons2))
            {
                sprite = icons2.TryGetValue(name2 + $"_{mod}", out sprite1) ? sprite1 : Blank;

                if (!sprite.IsValid())
                    sprite = icons2.TryGetValue(name2, out sprite1) ? sprite1 : Blank;
            }

            if (!sprite.IsValid() && style != "Regular" && Assets[ModType.Common].BaseIcons.TryGetValue("Regular", out icons2))
            {
                sprite = icons2.TryGetValue(name2 + $"_{mod}", out sprite1) ? sprite1 : Blank;

                if (!sprite.IsValid())
                    sprite = icons2.TryGetValue(name2, out sprite1) ? sprite1 : Blank;
            }

            if (!sprite.IsValid() && style != "Factionless" && Assets[ModType.Common].BaseIcons.TryGetValue("Factionless", out icons2))
            {
                sprite = icons2.TryGetValue(name2 + $"_{mod}", out sprite1) ? sprite1 : Blank;

                if (!sprite.IsValid())
                    sprite = icons2.TryGetValue(name2, out sprite1) ? sprite1 : Blank;
            }

            if (!sprite.IsValid())
                sprite = Fancy.Assets.GetSprite(name2 + $"_{mod}") ?? Blank;

            if (!sprite.IsValid())
                sprite = Fancy.Assets.GetSprite(name2) ??  Blank;

            if (sprite.IsValid())
            {
                sprite.name = sprite.texture.name = role;
                sprites.Add(sprite);
            }
            else
                Fancy.Instance.Warning($"NO {mod.ToUpper()} ICON FOR {name2}?!");
        }

        return BuildGlyphs([..sprites], $"{mod}RoleIcons ({Name}, {style})", index);
    }

    public Sprite GetSprite(string iconName, bool allowEE, string type)
    {
        if (!Assets.TryGetValue(GetModKey(type), out var assets))
            return Blank;

        assets.BaseIcons.TryGetValue(type, out var icons);
        icons ??= [];
        icons.TryGetValue(iconName, out var sprite);

        if (URandom.RandomRangeInt(1, 101) <= Constants.EasterEggChance() && (allowEE || !sprite.IsValid()))
        {
            var sprites = new List<Sprite>();

            if (Constants.AllEasterEggs())
            {
                GlobalEasterEggs[type].TryGetValue(iconName, out sprites);
                sprites ??= [];
            }

            if (sprites.Count == 0)
            {
                assets.EasterEggs.TryGetValue(type, out var icons3);
                icons3 ??= [];
                icons3.TryGetValue(iconName, out sprites);
                sprites ??= [];
            }

            if (sprites.Count > 0)
                return sprites.Random();
        }

        return sprite ?? Blank;
    }

    public static void PopulateDirectory(string path)
    {
        if (path.EndsWith("Vanilla") || path.EndsWith("BTOS2"))
            return;

        foreach (var mod in MainFolders)
        {
            if (mod == "BTOS2" && !Constants.BTOS2Exists())
                continue;

            var modPath = Path.Combine(path, mod);

            if (!Directory.Exists(modPath))
                Directory.CreateDirectory(modPath);

            if (mod == "PlayerNumbers")
                continue;

            foreach (var name1 in ModsToFolders[mod])
            {
                var baseFolder = Path.Combine(modPath, name1 + "Base");

                if (!Directory.Exists(baseFolder))
                    Directory.CreateDirectory(baseFolder);

                var eeFolder = Path.Combine(modPath, name1 + "EasterEggs");

                if (!Directory.Exists(eeFolder))
                    Directory.CreateDirectory(eeFolder);
            }

            var customPath = Path.Combine(modPath, "Custom");

            if (!Directory.Exists(customPath))
                Directory.CreateDirectory(customPath);
        }
    }
}