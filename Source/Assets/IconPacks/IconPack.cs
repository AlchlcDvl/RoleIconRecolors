using NewModLoading;

namespace FancyUI.Assets.IconPacks;

public class IconPack(string name) : Pack(name, PackType.IconPacks)
{
    public Dictionary<GameModType, IconAssets> Assets { get; } = [];
    private Dictionary<string, Sprite> NumberSprites { get; } = [];
    private Dictionary<string, Sprite> EmojiSprites { get; } = [];
    public TMP_SpriteAsset PlayerNumbers { get; private set; }
    public TMP_SpriteAsset Emojis { get; private set; }

    public override void Debug()
    {
        if (!Constants.PackDebug())
            return;

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

        if (Emojis)
        {
            Fancy.Instance.Message($"{Name} has an Emoji sprite sheet!");
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

        Fancy.Instance.Message($"Deleting {Name}", true);
        Assets.Values.Do(x => x.Delete());
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
                }

                if (Enum.TryParse<GameModType>(mod, out var type))
                {
                    var assets = Assets[type] = new(mod);

                    if (type == GameModType.BTOS2 && !Constants.BTOS2Exists())
                        continue;

                    foreach (var name1 in ModsToFolders[mod])
                    {
                        var baseIcons = assets.BaseIcons[name1] = [];
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
                                        baseIcons[file.FancySanitisePath(true)] = sprite;
                                }
                            }
                        }
                        else
                        {
                            Fancy.Instance.Warning($"{Name} {mod} {baseName} folder doesn't exist");
                            Directory.CreateDirectory(baseFolder);
                        }

                        if (!GlobalEasterEggs.TryGetValue(name1, out var globalEasterEggs))
                            GlobalEasterEggs[name1] = globalEasterEggs = [];

                        var easterEggs = assets.EasterEggs[name1] = [];
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

                                    if (!easterEggs.TryGetValue(name, out var icons))
                                        easterEggs[name] = icons = [];

                                    if (!globalEasterEggs.TryGetValue(name, out var icons2))
                                        globalEasterEggs[name] = icons2 = [];

                                    icons.Add(sprite);
                                    icons2.Add(sprite);
                                }
                            }
                        }
                        else
                        {
                            Fancy.Instance.Warning($"{Name} {mod} {eeName} folder doesn't exist");
                            Directory.CreateDirectory(eeFolder);
                        }
                    }

                    if (type != GameModType.Common)
                        continue;

                    var icons3 = assets.BaseIcons["Custom"] = [];
                    var folder = Path.Combine(modPath, "Custom");

                    if (Directory.Exists(folder))
                    {
                        foreach (var type1 in FileTypes)
                        {
                            foreach (var file in Directory.GetFiles(folder, $"*.{type1}"))
                            {
                                var sprite = AssetManager.LoadSpriteFromDisk(file);

                                if (sprite.IsValid())
                                    icons3[file.FancySanitisePath(true)] = sprite;
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
                    var dict = mod.Contains("PlayerNumbers") ? NumberSprites : EmojiSprites;

                    foreach (var type1 in FileTypes)
                    {
                        foreach (var file in Directory.GetFiles(modPath, $"*.{type1}"))
                        {
                            var sprite = AssetManager.LoadSpriteFromDisk(file);

                            if (sprite.IsValid())
                                dict[file.FancySanitisePath(true)] = sprite;
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
        if (NumberSprites.Count > 0)
        {
            try
            {
                var sprites = new List<Sprite>();
                var dict = new Dictionary<string, string>();

                for (var i = 0; i < 16; i++)
                {
                    if (!NumberSprites.TryGetValue($"{i}", out var sprite))
                        sprite = Fancy.Instance.Assets.GetSprite($"{i}") ?? Blank;

                    if (sprite.IsValid())
                    {
                        sprite.name = sprite.texture.name = $"PlayerNumbers_{i}";
                        sprites.Add(sprite);
                    }
                    else
                    {
                        Fancy.Instance.Warning($"NO NUMBER ICON FOR {i}?!");
                        continue;
                    }

                    dict.Add($"PlayerNumbers_{i}", $"PlayerNumbers_{i}");
                }

                PlayerNumbers = AssetManager.BuildGlyphs(sprites, $"PlayerNumbers ({Name})", dict);
                Utils.DumpSprite(PlayerNumbers.spriteSheet as Texture2D, "PlayerNumbers", Path.Combine(PackPath, "PlayerNumbers"));
            }
            catch (Exception e)
            {
                Fancy.Instance.Error($"Unable to create custom player numbers for {Name} because:\n{e}");
                PlayerNumbers = null;
            }
        }

        if (EmojiSprites.Count > 0)
        {
            try
            {
                var sprites = new List<Sprite>();
                var dict = new Dictionary<string, string>();

                for (var i = 1; i < 7; i++)
                {
                    if (!EmojiSprites.TryGetValue($"Emoji{i}", out var sprite))
                        sprite = Fancy.Instance.Assets.GetSprite($"Emoji{i}") ?? Blank;

                    if (sprite.IsValid())
                    {
                        sprite.name = sprite.texture.name = $"Emoji{i}";
                        sprites.Add(sprite);
                    }
                    else
                    {
                        Fancy.Instance.Warning($"NO EMOJI FOR {i}?!");
                        continue;
                    }

                    dict.Add($"Emoji{i}", $"Emoji{i}");
                }

                Emojis = AssetManager.BuildGlyphs(sprites, $"Emojis ({Name})", dict);
                Utils.DumpSprite(PlayerNumbers.spriteSheet as Texture2D, "Emojis", Path.Combine(PackPath, "Emojis"));
            }
            catch (Exception e)
            {
                Fancy.Instance.Error($"Unable to create custom emojis for {Name} because:\n{e}");
                PlayerNumbers = null;
            }
        }

        foreach (var (mod, assets) in Assets)
        {
            if (mod is GameModType.Common or GameModType.None)
                continue;

            var index = Utils.Filtered(mod);

            foreach (var (style, icons) in assets.BaseIcons)
            {
                if (icons.Count == 0)
                    continue;

                try
                {
                    var asset = BuildSpriteSheet(mod, mod.ToString(), style, icons, index);
                    assets.MentionStyles[style] = asset;
                    Utils.DumpSprite(asset?.spriteSheet as Texture2D, $"{style}{mod}RoleIcons", Path.Combine(PackPath, $"{mod}"));
                }
                catch (Exception e)
                {
                    Fancy.Instance.Error($"Unable to create custom role icons for {Name} {style} because:\n{e}");
                    assets.MentionStyles[style] = null;
                }
            }
        }

        foreach (var (style, icons) in Assets[GameModType.Common].BaseIcons)
        {
            if (icons.Count == 0)
                continue;

            foreach (var mod in Mods)
            {
                try
                {
                    var type = Enum.Parse<GameModType>(mod);
                    var index = Utils.Filtered(type);
                    var asset = BuildSpriteSheet(type, mod, style, icons, index);
                    Assets[type].MentionStyles[style] = asset;
                    Utils.DumpSprite(asset?.spriteSheet as Texture2D, $"{style}{mod}RoleIcons", Path.Combine(PackPath, mod));
                }
                catch (Exception e)
                {
                    Fancy.Instance.Error($"Unable to create custom role icons for {Name} {style} because:\n{e}");
                    Assets[GameModType.Vanilla].MentionStyles[style] = Assets[GameModType.BTOS2].MentionStyles[style] = null;
                }
            }
        }

        var cursor = GetSprite("Cursor", false, null);

        if (cursor.IsValid())
            Cursor.SetCursor(cursor.texture, CursorMode.Auto);

        Fancy.Instance.Message($"{Name} Loaded!", true);
    }

    private TMP_SpriteAsset BuildSpriteSheet(GameModType type, string mod, string style, Dictionary<string, Sprite> icons, (Dictionary<string, string>, Dictionary<string, int>) index)
    {
        if (type == GameModType.BTOS2 && !Constants.BTOS2Exists())
            return null;

        var sprites = new List<Sprite>();

        foreach (var (role, roleInt) in index.Item2)
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

            if (!sprite.IsValid() && name3 != style && Assets[GameModType.Common].BaseIcons.TryGetValue(name3, out icons2))
            {
                sprite = icons2.TryGetValue(name2 + $"_{mod}", out sprite1) ? sprite1 : Blank;

                if (!sprite.IsValid())
                    sprite = icons2.TryGetValue(name2, out sprite1) ? sprite1 : Blank;
            }

            if (!sprite.IsValid() && style != "Regular" && Assets[GameModType.Common].BaseIcons.TryGetValue("Regular", out icons2))
            {
                sprite = icons2.TryGetValue(name2 + $"_{mod}", out sprite1) ? sprite1 : Blank;

                if (!sprite.IsValid())
                    sprite = icons2.TryGetValue(name2, out sprite1) ? sprite1 : Blank;
            }

            if (!sprite.IsValid() && style != "Factionless" && Assets[GameModType.Common].BaseIcons.TryGetValue("Factionless", out icons2))
            {
                sprite = icons2.TryGetValue(name2 + $"_{mod}", out sprite1) ? sprite1 : Blank;

                if (!sprite.IsValid())
                    sprite = icons2.TryGetValue(name2, out sprite1) ? sprite1 : Blank;
            }

            if (!sprite.IsValid())
                sprite = Fancy.Instance.Assets.GetSprite(name2 + $"_{mod}") ?? Blank;

            if (!sprite.IsValid())
                sprite = Fancy.Instance.Assets.GetSprite(name2) ??  Blank;

            if (sprite.IsValid())
            {
                sprite.name = sprite.texture.name = role;
                sprites.Add(sprite);
            }
            else
                Fancy.Instance.Warning($"NO {mod.ToUpper()} ICON FOR {name2}?!");
        }

        return AssetManager.BuildGlyphs(sprites, $"{mod}RoleIcons ({Name}, {style})", index.Item1);
    }

    public Sprite GetSprite(string iconName, bool allowEe, string type)
    {
        if (NewModLoading.Utils.IsNullEmptyOrWhiteSpace(type))
            type = "Regular";

        if (!Assets.TryGetValue(GetModKey(type), out var assets))
            return Blank;

        assets.BaseIcons.TryGetValue(type, out var icons);
        icons ??= [];
        icons.TryGetValue(iconName, out var sprite);

        if ((!allowEe && sprite.IsValid()) || (allowEe && URandom.RandomRangeInt(1, 101) > Constants.EasterEggChance()))
            return sprite ?? Blank;

        var sprites = new HashSet<Sprite>();

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
            return sprites.Random() ?? Blank;

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

            if (mod is "PlayerNumbers" or "Emojis")
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