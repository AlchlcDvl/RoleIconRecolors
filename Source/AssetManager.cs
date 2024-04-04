namespace IconPacks;

public static class AssetManager
{
    private const string Resources = "IconPacks.Resources.";

    public static readonly Dictionary<string, Dictionary<string, List<Sprite>>> GlobalEasterEggs = [];
    public static readonly Dictionary<string, IconPack> IconPacks = [];
    public static readonly Dictionary<int, Sprite> CacheScrollSprites = [];

    public static Sprite Blank;
    public static Sprite Thumbnail;
    public static Sprite Attack;
    public static Sprite Defense;
    public static Sprite Ethereal;

    public static TMP_SpriteAsset VanillaAsset1;
    public static TMP_SpriteAsset VanillaAsset2;
    public static TMP_SpriteAsset BTOS2Asset;

    public static string ModPath => Path.Combine(Path.GetDirectoryName(Application.dataPath), "SalemModLoader", "ModFolders", "Recolors");

    private static readonly string[] Avoid = [ "Attributes", "Necronomicon", "Neutral", "NeutralApocalypse", "NeutralEvil", "NeutralKilling", "Town", "TownInvestigative", "Teams", "Hidden",
        "TownKilling", "TownSupport", "TownProtective", "TownPower", "CovenKilling", "CovenDeception", "CovenUtility", "CovenPower", "Coven", "SlowMode", "FastMode", "AnonVoting", "Stoned",
        "SecretKillers", "HiddenRoles", "OneTrial", "RandomApocalypse", "Any", "CommonCoven", "CommonTown", "NeutralPariah", "NeutralSpecial", "TownTraitor", "PerfectTown", "NecroPass",
        "AnonNames", "WalkingDead", "Mafia", "MafiaDeception", "MafiaKilling", "MafiaUtility", "MafiaPower", "Cleaned", "NeutralChaos", "TownVEvils", "NonTown", "NonCoven", "NonMafia",
        "NonNeutral", "FactionedEvil", "Lovers" ];

    private static readonly string[] ToRemove = [ ".png", ".jpg" ];

    private static Assembly Core => typeof(Recolors).Assembly;

    public static Sprite GetSprite(string name, string faction, bool allowEE = true, string packName = null, bool log = true) => GetSprite(name, allowEE, faction, packName, log);

    public static Sprite GetSprite(string name, bool allowEE = true, string faction = null, string packName = null, bool log = true)
    {
        if (name.Contains("Blank") || !Constants.EnableIcons || IconPacks.Count == 0)
            return Blank;

        packName ??= Constants.CurrentPack;
        faction ??= "Regular";

        if (faction == "Blank")
            faction = "Regular";

        if (!IconPacks.TryGetValue(packName, out var pack))
        {
            Logging.LogError($"Error finding {packName} in loaded packs");
            ModSettings.SetString("Selected Icon Pack", "Vanilla", "alchlcsystm.recolors");
            return Blank;
        }

        var type = faction;

        if (Avoid.Any(name.Contains))
            type = "Regular";
        else if (Constants.IsLocalVIP)
            type = "VIP";

        try
        {
            return pack.GetSprite(name, allowEE, type, log);
        }
        catch (Exception e)
        {
            Logging.LogError($"Error finding {name}'s sprite from {packName} {type}\n{e}");
            return Blank;
        }
    }

    public static void LoadAssets()
    {
        if (!Directory.Exists(ModPath))
            Directory.CreateDirectory(ModPath);

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

        if (Constants.BTOS2Exists)
        {
            BTOS2Asset = BTOSInfo.assetBundle.LoadAsset<TMP_SpriteAsset>("Roles");

            for (var i = 0; i < BTOS2Asset.spriteCharacterTable.Count; i++)
            {
                BTOS2Asset.spriteGlyphTable[i].metrics = new()
                {
                    horizontalBearingX = 0f,
                    horizontalBearingY = 224f
                };
            }

            Utils.DumpSprite(BTOS2Asset.spriteSheet as Texture2D, "BTOSRoleIcons_Modified", Path.Combine(ModPath, "BTOS2"));
        }

        TryLoadingSprites(Constants.CurrentPack);
        LoadVanillaSpriteSheets();
    }

    private static Texture2D EmptyTexture() => new(2, 2, TextureFormat.ARGB32, true);

    private static Texture2D LoadDiskTexture(string fileName, string subfolder, string folder, string filetype)
    {
        try
        {
            fileName = fileName.SanitisePath();
            var path = Path.Combine(ModPath, folder, subfolder, $"{fileName}.{filetype}");
            var texture = EmptyTexture();
            texture.LoadImage(File.ReadAllBytes(path), false);
            texture.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            texture.name = fileName;
            return texture.DontDestroy().Decompress();
        }
        catch (Exception e)
        {
            Logging.LogError($"Error loading {folder} > {subfolder} > {fileName} ({filetype})\n{e}");
            return null;
        }
    }

    public static Sprite LoadDiskSprite(string fileName, string subfolder, string folder, string filetype)
    {
        try
        {
            fileName = fileName.SanitisePath();
            var path = Path.Combine(ModPath, folder, subfolder, $"{fileName}.{filetype}");

            if (!File.Exists(path))
            {
                Logging.LogError($"Path {folder} > {subfolder} > {fileName} was missing");
                return null;
            }

            var texture = LoadDiskTexture(path.SanitisePath(), subfolder, folder, filetype);

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
            Logging.LogError(e);
            return null;
        }
    }

    public static string SanitisePath(this string path, bool removeIcon = false)
    {
        path = path.Split('/')[^1];
        path = path.Split('\\')[^1];
        ToRemove.ForEach(x => path = path.Replace(x, ""));
        path = path.Replace(Resources, "");
        path = path.Replace("Marshall", "Marshal");

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
                IconPacks.Remove(packName);
                exists?.Delete();
                exists = new(packName);
                exists.Load();

                if (CacheDefaultSpriteSheet.ServiceExists)
                    SetScrollSprites();

                exists.Debug();
                IconPacks.Add(packName, exists);
            }
        }
        catch (Exception e)
        {
            Logging.LogError(e);
        }
    }

    public static void RunDiagnostics(Exception e)
    {
        IconPack pack = null;
        TMP_SpriteAsset asset = null;
        var diagnostic = $"Uh oh, something happened here\nPack Name: {Constants.CurrentPack}\nStyle Name: {Constants.CurrentStyle}\nFaction Override: {Constants.FactionOverride}\n" +
            $"Custom Numbers: {Constants.CustomNumbers}";

        if (!CacheDefaultSpriteSheet.Cache1)
            diagnostic += "\nVanilla Sheet Does Not Exist";

        if (!CacheDefaultSpriteSheet.Cache2)
            diagnostic += "\nVanilla Player Numbers Sheet Does Not Exist";

        if (!VanillaAsset1)
            diagnostic += "\nModified Vanilla Sheet Does Not Exist";

        if (!VanillaAsset2)
            diagnostic += "\nModified Player Numbers Sheet Does Not Exist";

        if (!BTOS2Asset && Constants.BTOS2Exists)
            diagnostic += "\nBTOS2 Sheet Does Not Exist";

        diagnostic += $"\nCurrently In A {(Constants.IsBTOS2 ? "BTOS2" : "Vanilla")} Game";

        if (Constants.EnableIcons && !IconPacks.TryGetValue(Constants.CurrentPack, out pack))
            diagnostic += "\nNo Loaded Icon Pack";
        else if (pack == null)
            diagnostic += "\nLoaded Icon Pack Was Null";
        else if (!pack.MentionStyles.TryGetValue(Constants.CurrentStyle, out asset))
            diagnostic += "\nLoaded Icon Pack Does Not Have A Valid Mention Style";
        else if (!asset)
            diagnostic += "\nLoaded Mention Style Was Null";
        else if (!pack.PlayerNumbers)
            diagnostic += "\nLoaded Player Numbers Was Null";

        diagnostic += $"\nError: {e}";
        Logging.LogError(diagnostic);
    }

    public static void SetScrollSprites()
    {
        try
        {
            Service.Home.Scrolls.scrollInfoLookup_.ForEach((_, y) =>
            {
                var sprite = GetSprite(Utils.RoleName(y.role), false);

                if (!CacheScrollSprites.ContainsKey(y.id))
                    CacheScrollSprites[y.id] = y.decoration.sprite;

                if (sprite.IsValid() || CacheScrollSprites.TryGetValue(y.id, out sprite))
                    y.decoration.sprite = sprite;
                else
                    CacheScrollSprites[y.id] = y.decoration.sprite;
            });

            Service.Home.Scrolls.cursedScrollInfoLookup_.ForEach((_, y) =>
            {
                var sprite = GetSprite(Utils.RoleName(y.role), false);

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
            Logging.LogError(e);
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
            Logging.LogError(e);
            return source;
        }
    }

    // love ya pat
    public static void LoadVanillaSpriteSheets()
    {
        try
        {
            var (rolesWithIndexDict, rolesWithIndex) = Utils.Filtered();
            var textures = new List<Texture2D>();
            var sprites = new List<Sprite>();

            foreach (var (role, roleInt) in rolesWithIndex)
            {
                var actualRole = (Role)roleInt;
                var name = Utils.RoleName(actualRole, ModType.Vanilla);
                var sprite = Witchcraft.Witchcraft.Assets.TryGetValue(name, out var sprite1) ? sprite1 : Blank;

                if (!sprite.IsValid())
                {
                    sprite.name = sprite.texture.name = role;
                    textures.Add(sprite.texture);
                    sprites.Add(sprite);
                }
                else
                    Logging.LogWarning($"NO ICON FOR {name}?!");
            }

            VanillaAsset1 = BuildGlyphs([..sprites], [..textures], "RoleIcons", rolesWithIndexDict);
            Utils.DumpSprite(VanillaAsset1.spriteSheet as Texture2D, "RoleIcons_Modified", Path.Combine(ModPath, "Vanilla"));
        }
        catch (Exception e)
        {
            Logging.LogError(e);
            VanillaAsset1 = null;
        }

        try
        {
            var dict = new List<string>();
            var textures = new List<Texture2D>();
            var sprites = new List<Sprite>();

            for (var i = 0; i < 16; i++)
            {
                var sprite = Witchcraft.Witchcraft.Assets.TryGetValue($"{i}", out var sprite1) ? sprite1 : Blank;

                if (sprite.IsValid())
                {
                    sprite.name = sprite.texture.name = $"PlayerNumbers_{i}";
                    textures.Add(sprite.texture);
                    sprites.Add(sprite);
                }
                else
                    Logging.LogWarning($"NO ICON FOR {i}?!");

                dict.Add($"PlayerNumbers_{i}");
            }

            VanillaAsset2 = BuildGlyphs([..sprites], [..textures], "PlayerNumbers", dict.ToDictionary(x => x, x => x), false);
            Utils.DumpSprite(VanillaAsset2.spriteSheet as Texture2D, "PlayerNumbers_Modified", Path.Combine(ModPath, "Vanilla"));
        }
        catch (Exception e)
        {
            Logging.LogError(e);
            VanillaAsset2 = null;
        }
    }

    // courtesy of pat, love ya mate
    public static TMP_SpriteAsset BuildGlyphs(Sprite[] sprites, Texture2D[] textures, string spriteAssetName, Dictionary<string, string> rolesWithIndexDict, bool shouldLower = true)
    {
        var asset = ScriptableObject.CreateInstance<TMP_SpriteAsset>();
        var image = new Texture2D(4096, 2048) { name = spriteAssetName };
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
                name = rolesWithIndexDict[shouldLower ? glyph.sprite.name.ToLower() : glyph.sprite.name],
                glyphIndex = (uint)i,
            };

            asset.spriteGlyphTable.Add(glyph);
            asset.spriteCharacterTable.Add(character);
        }

        asset.name = spriteAssetName;
        asset.material = new(Shader.Find("TextMeshPro/Sprite"));
        AccessTools.Property(asset.GetType(), "version").SetValue(asset, "1.1.0");
        var decompressed = image.Decompress();
        asset.material.mainTexture = decompressed;
        asset.spriteSheet = decompressed;
        asset.UpdateLookupTables();
        asset.hideFlags |= HideFlags.DontUnloadUnusedAsset;
        return asset.DontDestroy();
    }
}