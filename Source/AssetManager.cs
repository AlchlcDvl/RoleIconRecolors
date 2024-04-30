namespace IconPacks;

public static class AssetManager
{
    public const string Resources = "IconPacks.Resources.";

    public static readonly Dictionary<string, Dictionary<string, List<Sprite>>> GlobalEasterEggs = [];
    public static readonly Dictionary<string, IconPack> IconPacks = [];
    public static readonly Dictionary<int, Sprite> CacheScrollSprites = [];

    public static Sprite Blank { get; private set; }
    public static Sprite Thumbnail { get; private set; }
    public static Sprite Attack { get; private set; }
    public static Sprite Defense { get; private set; }
    public static Sprite Ethereal { get; private set; }

    public static TMP_SpriteAsset Vanilla1 { get; private set; }
    public static TMP_SpriteAsset Vanilla2 { get; private set; }
    public static TMP_SpriteAsset BTOS2_1 { get; private set; }
    public static TMP_SpriteAsset BTOS2_2 { get; private set; }

    public static string ModPath => Path.Combine(Path.GetDirectoryName(Application.dataPath), "SalemModLoader", "ModFolders", "Recolors");

    private static readonly string[] Avoid = [ "Necronomicon", "Neutral", "Town", "Coven", "SlowMode", "FastMode", "Any", "Recruit", "Stoned", "SecretKillers", "HiddenRoles", "OneTrial",
        "RandomApocalypse", "TownTraitor", "PerfectTown", "NecroPass", "Anon", "WalkingDead", "ExeTarget", "Hexed", "Knighted", "Bread", "Revealed", "Disconnected", "Connecting", "Lovers",
        "Doused", "Plagued", "Revealed", "Trapped", "Hangover", "Silenced", "Dreamwoven", "Insane", "Bugged", "Tracked", "Sickness", "Reaped", "Deafened", "Audited", "Enchanted", "Egoist",
        "Accompanied", "Banned", "SpeakingSpirits" ];

    private static readonly string[] ToRemove = [ ".png", ".jpg" ];

    private static Assembly Core => typeof(Recolors).Assembly;

    public static Sprite GetSprite(bool skipRegular, string name, string faction, bool allowEE = false, string packName = null) => GetSprite(name, allowEE, faction, packName, skipRegular);

    public static Sprite GetSprite(string name, string faction, bool allowEE = true, string packName = null, bool skipRegular = false) => GetSprite(name, allowEE, faction, packName,
        skipRegular);

    public static Sprite GetSprite(string name, bool allowEE = true, string faction = null, string packName = null, bool skipRegular = false)
    {
        if (name.Contains("Blank") || !Constants.EnableIcons || IconPacks.Count == 0 || !Constants.EnableIcons)
            return Blank;

        packName ??= Constants.CurrentPack;

        if (faction is null or "Blank" || (Avoid.Any(name.Contains) && !name.Contains("Leader")))
            faction = "Regular";

        if (!IconPacks.TryGetValue(packName, out var pack))
        {
            Logging.LogError($"Error finding {packName} in loaded packs");
            ModSettings.SetString("Selected Icon Pack", "Vanilla", "alchlcsystm.recolors");
            return Blank;
        }

        var type = Constants.IsLocalVIP ? "VIP" : faction;
        var mod = Utils.GetGameType();

        try
        {
            var sprite = pack.GetSprite(name + $"_{mod}", allowEE, type);

            if (!sprite.IsValid())
                sprite = pack.GetSprite(name, allowEE, type);

            if (type != "Regular" && !sprite.IsValid() && !skipRegular)
            {
                sprite = pack.GetSprite(name + $"_{mod}", allowEE, "Regular");

                if (type != "Regular" && !sprite.IsValid())
                    sprite = pack.GetSprite(name, allowEE, "Regular");
            }

            return sprite ?? Blank;
        }
        catch (Exception e)
        {
            Logging.LogError($"Error finding {name}'s sprite from {packName} {type} during a {mod} game\n{e}");
            return Blank;
        }
    }

    public static void LoadAssets()
    {
        if (!Directory.Exists(ModPath))
            Directory.CreateDirectory(ModPath);

        var vanilla = Path.Combine(ModPath, "Vanilla");

        if (!Directory.Exists(vanilla))
            Directory.CreateDirectory(vanilla);

        Core.GetManifestResourceNames().ForEach(x =>
        {
            if (x.EndsWith(".png"))
            {
                var sprite = FromResources.LoadSprite(x).DontDestroy();
                sprite.hideFlags |= HideFlags.DontUnloadUnusedAsset;
                sprite.name = sprite.texture.name = x.SanitisePath();

                if (x.Contains("Blank"))
                    Blank = sprite;
                else if (x.Contains("Thumbnail"))
                    Thumbnail = sprite;
            }
        });

        Attack = Witchcraft.Witchcraft.Assets["Attack"];
        Defense = Witchcraft.Witchcraft.Assets["Defense"];
        Ethereal = Witchcraft.Witchcraft.Assets["Ethereal"];

        TryLoadingSprites(Constants.CurrentPack);
        LoadVanillaSpriteSheets();

        try
        {
            DumpBTOSSpriteSheet();
        } catch {}
    }

    private static void DumpBTOSSpriteSheet()
    {
        if (!Constants.BTOS2Exists)
            return;

        var btos = Path.Combine(ModPath, "BTOS2");

        if (!Directory.Exists(btos))
            Directory.CreateDirectory(btos);

        BTOS2_1 = BetterTOS2.BTOSInfo.assetBundle.LoadAsset<TMP_SpriteAsset>("Roles");

        for (var i = 0; i < BTOS2_1.spriteCharacterTable.Count; i++)
        {
            BTOS2_1.spriteGlyphTable[i].metrics = new()
            {
                horizontalBearingX = 0f,
                horizontalBearingY = 224f
            };
        }

        Utils.DumpSprite(BTOS2_1.spriteSheet as Texture2D, "BTOSRoleIcons", Path.Combine(ModPath, "BTOS2"), true);
        LoadBTOS2SpriteSheet();

        BTOS2Compatibility.BTOS2Patched = BTOS2Compatibility.Init();
    }

    private static Texture2D EmptyTexture() => new(2, 2, TextureFormat.ARGB32, true);

    private static Texture2D LoadDiskTexture(string fileName, string path)
    {
        try
        {
            fileName = fileName.SanitisePath();
            var texture = EmptyTexture();
            texture.LoadImage(File.ReadAllBytes(path), false);
            texture.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            texture.name = fileName;
            return texture.DontDestroy();
        }
        catch (Exception e)
        {
            Logging.LogError($"Error loading {path}\n{e}");
            return null;
        }
    }

    public static Sprite LoadDiskSprite(string fileName, string subfolder, string folder, string superfolder, string filetype) => LoadDiskSprite(fileName, Path.Combine(ModPath, superfolder,
        folder, subfolder, $"{fileName.SanitisePath()}.{filetype}"));

    public static Sprite LoadDiskSprite(string fileName, string subfolder, string folder, string filetype) => LoadDiskSprite(fileName, Path.Combine(ModPath, folder, subfolder,
        $"{fileName.SanitisePath()}.{filetype}"));

    public static Sprite LoadDiskSprite(string fileName, string path)
    {
        try
        {
            fileName = fileName.SanitisePath();

            if (!File.Exists(path))
            {
                Logging.LogError($"Path {path} was missing");
                return null;
            }

            var texture = LoadDiskTexture(fileName, path);

            if (texture == null)
            {
                Logging.LogError($"Uh oh texture loading error at {path}");
                return null;
            }

            var sprite = Sprite.Create(texture, new(0, 0, texture.width, texture.height), new(0.5f, 0.5f), 100);

            if (sprite == null)
            {
                Logging.LogError($"Uh oh sprite loading error at {path}");
                return null;
            }

            sprite.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            sprite.name = texture.name;
            return sprite.DontDestroy();
        }
        catch (Exception e)
        {
            Logging.LogError($"Unable to set sprite for {path} because:\n{e}");
            return null;
        }
    }

    public static string SanitisePath(this string path, bool removeIcon = false)
    {
        path = path.Split('/').Last();
        path = path.Split('\\').Last();
        ToRemove.ForEach(x => path = path.Replace(x, ""));
        path = path.Replace(Resources, "");
        path = path.Replace("Marshall", "Marshal");
        path = path.Split('.').Last();

        if (removeIcon)
        {
            var i = 0;

            while (i >= 0 && path.Contains("_Icon"))
            {
                if (path.Contains($"_Icon{i}"))
                    path = path.Replace($"_Icon{i}", "");

                i++;
            }
        }

        return path;
    }

    public static void TryLoadingSprites(string packName)
    {
        var folder = Path.Combine(ModPath, packName);

        if (!Directory.Exists(folder))
        {
            Logging.LogError($"{packName} was missing");
            ModSettings.SetString("Selected Icon Pack", "Vanilla", "alchlcsystm.recolors");
            return;
        }

        try
        {
            if (IconPacks.TryGetValue(packName, out var exists) && exists)
                exists.Reload();
            else
            {
                exists = IconPacks[packName] = new(packName);
                exists.Load();

                if (CacheDefaults.ServiceExists)
                    SetScrollSprites();

                exists.Debug();
            }

            foreach (var (pack, ip) in IconPacks)
            {
                if (pack != packName)
                    ip.Delete();
            }
        }
        catch (Exception e)
        {
            Logging.LogError($"Unable to load icon pack {packName} because:\n{e}");
        }
    }

    public static void RunDiagnostics(Exception e)
    {
        IconPack pack = null;
        TMP_SpriteAsset asset = null;
        IconAssets assets = null;
        var game = Utils.GetGameType();
        var diagnostic = $"Uh oh, something happened here\nPack Name: {Constants.CurrentPack}\nStyle Name: {Constants.CurrentStyle}\nFaction Override: {Constants.FactionOverride}\n" +
            $"Custom Numbers: {Constants.CustomNumbers}";

        if (!CacheDefaults.RoleIcons)
            diagnostic += "\nVanilla Sheet Does Not Exist";

        if (!CacheDefaults.Numbers)
            diagnostic += "\nVanilla Player Numbers Sheet Does Not Exist";

        if (!Vanilla1)
            diagnostic += "\nModified Vanilla Sheet Does Not Exist";

        if (!Vanilla2)
            diagnostic += "\nModified Player Numbers Sheet Does Not Exist";

        if (Constants.BTOS2Exists)
        {
            if (!BTOS2_1)
                diagnostic += "\nBTOS2 Sheet Does Not Exist";

            if (!BTOS2_2)
                diagnostic += "\nModified BTOS2 Sheet Does Not Exist";
        }

        diagnostic += $"\nCurrently In A {game} Game";

        if (Constants.EnableIcons && !IconPacks.TryGetValue(Constants.CurrentPack, out pack))
            diagnostic += "\nNo Loaded Icon Pack";
        else if (pack == null)
            diagnostic += "\nLoaded Icon Pack Was Null";
        else if (!pack.PlayerNumbers)
            diagnostic += "\nLoaded Player Numbers Was Null";
        else if (!pack.Assets.TryGetValue(game, out assets))
            diagnostic += "\nInvalid Game Type Was Detected";
        else if (!assets.MentionStyles.TryGetValue(Constants.CurrentStyle, out asset))
            diagnostic += "\nLoaded Icon Pack Does Not Have A Valid Mention Style";
        else if (!asset)
            diagnostic += "\nLoaded Mention Style Was Null";

        diagnostic += $"\nError: {e}";
        Logging.LogError(diagnostic);
    }

    public static void SetScrollSprites()
    {
        try
        {
            Service.Home.Scrolls.scrollInfoLookup_.Values.ForEach(y =>
            {
                var sprite = GetSprite(Utils.RoleName(y.role), Utils.FactionName(y.role.GetFactionType()), false);

                if (!CacheScrollSprites.ContainsKey(y.id))
                    CacheScrollSprites[y.id] = y.decoration.sprite;

                if (sprite.IsValid() || CacheScrollSprites.TryGetValue(y.id, out sprite))
                    y.decoration.sprite = sprite;
                else
                    CacheScrollSprites[y.id] = y.decoration.sprite;
            });

            Service.Home.Scrolls.cursedScrollInfoLookup_.Values.ForEach(y =>
            {
                var sprite = GetSprite(Utils.RoleName(y.role), Utils.FactionName(y.role.GetFactionType()), false);

                if (!CacheScrollSprites.ContainsKey(y.id))
                    CacheScrollSprites[y.id] = y.decoration.sprite;

                if (sprite.IsValid() || CacheScrollSprites.TryGetValue(y.id, out sprite))
                    y.decoration.sprite = sprite;
                else
                    CacheScrollSprites[y.id] = y.decoration.sprite;
            });
        }
        catch (Exception e)
        {
            Logging.LogError($"Unable to set scroll sprites because:\n{e}");
        }
    }

    // thanks stackoverflow and pat
    // https://stackoverflow.com/questions/51315918/how-to-encodetopng-compressed-textures-in-unity
    public static Texture2D Decompress(this Texture2D source)
    {
        try
        {
            var renderTex = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            Graphics.Blit(source, renderTex);
            var previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            var readableText = new Texture2D(source.width, source.height);
            readableText.ReadPixels(new(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            readableText.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            return readableText.DontDestroy();
        }
        catch (Exception e)
        {
            Logging.LogError($"Unable to decompress {source.name} because:\n{e}");
            return source;
        }
    }

    // love ya pat
    public static void LoadVanillaSpriteSheets()
    {
        try
        {
            var index = Utils.Filtered(ModType.Vanilla);
            var sprites = new List<Sprite>();

            foreach (var (role, (_, roleInt)) in index)
            {
                var name = Utils.RoleName((Role)roleInt, ModType.Vanilla);

                if (Witchcraft.Witchcraft.Assets.TryGetValue(name, out var sprite) && sprite.IsValid())
                {
                    sprite.name = sprite.texture.name = role;
                    sprites.Add(sprite);
                }
                else
                    Logging.LogWarning($"NO VANILLA ICON FOR {name}?!");
            }

            Vanilla1 = BuildGlyphs([..sprites], "RoleIcons", index);
            Utils.DumpSprite(Vanilla1.spriteSheet as Texture2D, "RoleIcons_Modified", Path.Combine(ModPath, "Vanilla"));
        }
        catch (Exception e)
        {
            Logging.LogError($"Unable to create modified vanilla role icons sheet because:\n{e}");
            Vanilla1 = null;
        }

        try
        {
            var dict = new List<string>();
            var sprites = new List<Sprite>();

            for (var i = 0; i < 16; i++)
            {
                var sprite = Witchcraft.Witchcraft.Assets.TryGetValue($"{i}", out var sprite1) ? sprite1 : Blank;

                if (sprite.IsValid())
                {
                    sprite.name = sprite.texture.name = $"PlayerNumbers_{i}";
                    sprites.Add(sprite);
                }
                else
                    Logging.LogWarning($"NO NUMBER ICON FOR {i}?!");

                dict.Add($"PlayerNumbers_{i}");
            }

            Vanilla2 = BuildGlyphs([..sprites], "PlayerNumbers", dict.ToDictionary(x => x, x => (x, 0)), false);
            Utils.DumpSprite(Vanilla2.spriteSheet as Texture2D, "PlayerNumbers_Modified", Path.Combine(ModPath, "Vanilla"));
        }
        catch (Exception e)
        {
            Logging.LogError($"Unable to create modified player numbers sheet because:\n{e}");
            Vanilla2 = null;
        }
    }
    public static void LoadBTOS2SpriteSheet()
    {
        try
        {
            var index = Utils.Filtered(ModType.BTOS2);
            var sprites = new List<Sprite>();

            foreach (var (role, (_, roleInt)) in index)
            {
                var name = Utils.RoleName((Role)roleInt, ModType.BTOS2);
                var sprite = Witchcraft.Witchcraft.Assets.TryGetValue(name + "_BTOS2", out var sprite1) ? sprite1 : Blank;

                if (!sprite.IsValid())
                    sprite = Witchcraft.Witchcraft.Assets.TryGetValue(name, out sprite1) ? sprite1 : Blank;

                if (sprite.IsValid())
                {
                    sprite.name = sprite.texture.name = role;
                    sprites.Add(sprite);
                }
                else
                    Logging.LogWarning($"NO BTOS2 ICON FOR {name}?!");
            }

            BTOS2_2 = BuildGlyphs([..sprites], "BTOSRoleIcons", index);
            Utils.DumpSprite(BTOS2_2.spriteSheet as Texture2D, "BTOS2RoleIcons_Modified", Path.Combine(ModPath, "BTOS2"));
        }
        catch (Exception e)
        {
            Logging.LogError($"Unable to create modified btos role icons sheet because:\n{e}");
            BTOS2_2 = null;
        }
    }

    // courtesy of pat, love ya mate
    public static TMP_SpriteAsset BuildGlyphs(Sprite[] sprites, string spriteAssetName, Dictionary<string, (string, int)> index, bool shouldLower = true)
    {
        var textures = sprites.Select(x => x.texture).ToArray();
        var asset = ScriptableObject.CreateInstance<TMP_SpriteAsset>();
        var image = new Texture2D(2048, 2048) { name = spriteAssetName };
        var rects = image.PackTextures(textures, 2);

        for (var i = 0; i < rects.Length; i++)
        {
            var glyph = new TMP_SpriteGlyph()
            {
                glyphRect = new()
                {
                    x = (int)(rects[i].x * image.width),
                    y = (int)(rects[i].y * image.height),
                    width = (int)(rects[i].width * image.width),
                    height = (int)(rects[i].height * image.height),
                },
                metrics = new()
                {
                    width = textures[i].width,
                    height = textures[i].height,
                    horizontalBearingY = textures[i].width * 0.75f,
                    horizontalBearingX = 0,
                    horizontalAdvance = textures[i].width
                },
                index = (uint)i,
                sprite = sprites[i],
            };

            var character = new TMP_SpriteCharacter(0, asset, glyph)
            {
                name = index[shouldLower ? glyph.sprite.name.ToLower() : glyph.sprite.name].Item1,
                glyphIndex = (uint)i,
            };

            asset.spriteGlyphTable.Add(glyph);
            asset.spriteCharacterTable.Add(character);
        }

        asset.name = spriteAssetName;
        asset.material = new(Shader.Find("TextMeshPro/Sprite"));
        AccessTools.Property(asset.GetType(), "version").SetValue(asset, "1.1.0");
        asset.material.mainTexture = asset.spriteSheet = image;
        asset.UpdateLookupTables();
        asset.hideFlags |= HideFlags.DontUnloadUnusedAsset;
        return asset.DontDestroy();
    }
}