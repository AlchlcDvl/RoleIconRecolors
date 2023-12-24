namespace RecolorsPlatformless;

public static class AssetManager
{
    private const string Resources = "RecolorsPlatformless.Resources.";

    public static readonly Dictionary<string, List<Sprite>> RegEEIcons = new();
    public static readonly Dictionary<string, List<Sprite>> TTEEIcons = new();
    public static readonly Dictionary<string, List<Sprite>> VIPEEIcons = new();
    public static readonly Dictionary<string, IconPack> IconPacks = new();
    public static readonly Dictionary<int, Sprite> CacheScrollSprites = new();

    public static Sprite Blank;
    public static Sprite Thumbnail;
    public static Sprite CursedSoul;
    public static Sprite GhostTown;
    public static Sprite Vampire;
    public static Sprite Attack;
    public static Sprite Defense;

    public static TMP_SpriteAsset Asset;
    public static bool SpriteSheetLoaded;

    public static string ModPath => Path.Combine(Path.GetDirectoryName(Application.dataPath), "SalemModLoader", "ModFolders", "Recolors");
    public static string DefaultPath => Path.Combine(ModPath, "Recolors");
    public static string VanillaPath => Path.Combine(ModPath, "Vanilla");

    public static readonly string[] Folders = new[] { "Base", "TTBase", "VIPBase", "EasterEggs", "TTEasterEggs", "VIPEasterEggs" };

    private static readonly string[] Avoid = new[] { "Attributes", "Necronomicon", "Neutral", "NeutralApocalypse", "NeutralEvil", "NeutralKilling", "Town", "TownInvestigative",
        "TownKilling", "TownSupport", "TownProtective", "TownPower", "CovenKilling", "CovenDeception", "CovenUtility", "CovenPower", "Coven", "SlowMode", "FastMode", "AnonVotes",
        "HiddenKillers", "HiddenRoles", "OneTrial" };

    private static readonly string[] ToRemove = new[] { ".png" };

    private static Assembly Core => typeof(Recolors).Assembly;

    public static Sprite GetSprite(string name, bool allowEE = true)
    {
        if (name.Contains("Blank") || !Constants.EnableIcons || IconPacks.Count == 0)
            return Blank;

        if (!IconPacks.TryGetValue(Constants.CurrentPack, out var pack))
        {
            Recolors.LogError($"Error finding {Constants.CurrentPack} in loaded packs");
            ModSettings.SetString("Selected Icon Pack", "Vanilla", "alchlcsystm.recolors");
            return Blank;
        }

        try
        {
            if (Avoid.Any(name.Contains))
                return GetRegSprite(pack, name, allowEE);
            else if (Constants.IsLocalTT)
                return GetTTSprite(pack, name, allowEE);
            else if (Constants.IsLocalVIP)
                return GetVIPSprite(pack, name, allowEE);
            else
                return GetRegSprite(pack, name, allowEE);
        }
        catch (Exception e)
        {
            Recolors.LogError(e);
            return Blank;
        }
    }

    private static Sprite GetRegSprite(IconPack pack, string name, bool allowEE = true)
    {
        if (!pack.RegIcons.TryGetValue(name, out var sprite))
        {
            Recolors.LogWarning($"Couldn't find regular {name} in {pack.Name}'s recources");
            return Blank;
        }

        if (URandom.RandomRangeInt(1, 101) <= Constants.EasterEggChance && allowEE)
        {
            if (pack.RegEEIcons.TryGetValue(name, out var sprites) && !Constants.AllEasterEggs)
                return sprites.Random();
            else if (RegEEIcons.TryGetValue(name, out sprites))
                return sprites.Random();
        }

        return sprite;
    }

    private static Sprite GetTTSprite(IconPack pack, string name, bool allowEE = true)
    {
        if (!pack.TTIcons.TryGetValue(name, out var sprite))
        {
            Recolors.LogWarning($"Couldn't find TT {name} in {pack.Name}'s recources");
            return GetRegSprite(pack, name, allowEE);
        }

        if (URandom.RandomRangeInt(1, 101) <= Constants.EasterEggChance && allowEE)
        {
            if (pack.TTEEIcons.TryGetValue(name, out var sprites) && !Constants.AllEasterEggs)
                return sprites.Random();
            else if (TTEEIcons.TryGetValue(name, out sprites))
                return sprites.Random();
        }

        return sprite;
    }

    private static Sprite GetVIPSprite(IconPack pack, string name, bool allowEE = true)
    {
        if (!pack.VIPIcons.TryGetValue(name, out var sprite))
        {
            Recolors.LogWarning($"Couldn't find VIP {name} in {pack.Name}'s recources");
            return GetRegSprite(pack, name, allowEE);
        }

        if (URandom.RandomRangeInt(1, 101) <= Constants.EasterEggChance && allowEE)
        {
            if (pack.VIPEEIcons.TryGetValue(name, out var sprites) && !Constants.AllEasterEggs)
                return sprites.Random();
            else if (VIPEEIcons.TryGetValue(name, out sprites))
                return sprites.Random();
        }

        return sprite;
    }

    public static void LoadAssets()
    {
        if (!Directory.Exists(ModPath))
            Directory.CreateDirectory(ModPath);

        if (!Directory.Exists(VanillaPath))
            Directory.CreateDirectory(VanillaPath);

        if (!Directory.Exists(DefaultPath))
            Directory.CreateDirectory(DefaultPath);

        foreach (var folder in Folders)
        {
            var path = Path.Combine(DefaultPath, folder);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        RegEEIcons.Clear();
        TTEEIcons.Clear();
        VIPEEIcons.Clear();

        Core.GetManifestResourceNames().ForEach(x =>
        {
            if (x.EndsWith(".png"))
            {
                var name = x.SanitisePath();
                var sprite = FromResources.LoadSprite(x);
                UObject.DontDestroyOnLoad(sprite);

                if (x.Contains("Blank"))
                    Blank = sprite;
                else if (x.Contains("Thumbnail"))
                    Thumbnail = sprite;
                else if (x.Contains("CursedSoul"))
                    CursedSoul = sprite;
                else if (x.Contains("Vampire"))
                    Vampire = sprite;
                else if (x.Contains("GhostTown"))
                    GhostTown = sprite;
            }
        });

        TryLoadingSprites(Constants.CurrentPack);
    }

    private static Texture2D EmptyTexture() => new(2, 2, TextureFormat.ARGB32, true);

    private static Texture2D LoadDiskTexture(string fileName, string subfolder, string folder)
    {
        try
        {
            fileName = fileName.SanitisePath();
            var path = Path.Combine(ModPath, folder, subfolder, $"{fileName}.png");
            var texture = EmptyTexture();
            ImageConversion.LoadImage(texture, File.ReadAllBytes(path), false);
            texture.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontUnloadUnusedAsset;
            texture.name = fileName;
            UObject.DontDestroyOnLoad(texture);
            return texture;
        }
        catch
        {
            Recolors.LogError($"Error loading {folder} > {subfolder} > {fileName}");
            return null;
        }
    }

    public static Sprite LoadSpriteFromDisk(string fileName, string subfolder, string folder)
    {
        try
        {
            fileName = fileName.SanitisePath();
            var path = Path.Combine(ModPath, folder, subfolder, $"{fileName}.png");

            if (!File.Exists(path))
            {
                Recolors.LogError($"Path {folder} > {subfolder} > {fileName} was missing");
                return null;
            }

            var texture = LoadDiskTexture(path, subfolder, folder);

            if (texture == null)
                return null;

            var sprite = Sprite.Create(texture, new(0, 0, texture.width, texture.height), new(0.5f, 0.5f), 100);

            if (sprite == null)
                return null;

            sprite.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontUnloadUnusedAsset;
            sprite.name = fileName;
            UObject.DontDestroyOnLoad(sprite);
            return sprite;
        }
        catch (Exception e)
        {
            Recolors.LogError(e);
            return Blank;
        }
    }

    public static string SanitisePath(this string path, bool removeIcon = false)
    {
        path = path.Split('/')[^1];
        path = path.Split('\\')[^1];
        ToRemove.ForEach(x => path = path.Replace(x, ""));
        path = path.Replace(Resources, "");
        var i = 0;

        while (i >= 0 && path.Contains("_Icon") && removeIcon)
        {
            if (path.Contains($"_Icon{i}"))
                path = path.Replace($"_Icon{i}", "");

            i++;
        }

        return path;
    }

    public static void TryLoadingSprites(string packName)
    {
        if (packName == "Vanilla")
        {
            ChangeSpriteSheets(packName);
            return;
        }

        var folder = Path.Combine(ModPath, packName);

        if (!Directory.Exists(folder))
        {
            Recolors.LogError($"{packName} was missing");
            ModSettings.SetString("Selected Icon Pack", "Vanilla", "alchlcsystm.recolors");
            return;
        }

        try
        {
            if (IconPacks.TryGetValue(packName, out var exists) && exists != null)
                exists.Reload();
            else
            {
                IconPacks.Remove(packName);
                exists = new(packName);
                exists.Load();
                IconPacks.Add(packName, exists);

                if (PatchRoleService.ServiceExists)
                    exists.LoadSpriteSheet(true);

                if (PatchScrolls.ServiceExists)
                    SetScrollSprites();
            }
        }
        catch (Exception e)
        {
            Recolors.LogError(e);
        }
    }

    public static void ChangeSpriteSheets(string packName)
    {
        IconPack pack = null;
        TMP_SpriteAsset asset = null;

        try
        {
            if ((packName == "Vanilla" || Constants.CurrentStyle == "Vanilla" ) && Asset)
            {
                MaterialReferenceManager.instance.m_SpriteAssetReferenceLookup[CacheDefaultSpriteSheet.Cache] = Asset;
                MaterialReferenceManager.instance.m_FontMaterialReferenceLookup[CacheDefaultSpriteSheet.Cache] = Asset.material;
            }
            else if (packName != "Vanilla" && IconPacks.TryGetValue(packName, out pack) && pack != null && pack.MentionStyles.TryGetValue(Constants.CurrentStyle, out asset) && asset)
            {
                MaterialReferenceManager.instance.m_SpriteAssetReferenceLookup[CacheDefaultSpriteSheet.Cache] = asset;
                MaterialReferenceManager.instance.m_FontMaterialReferenceLookup[CacheDefaultSpriteSheet.Cache] = asset.material;
            }
        }
        catch (Exception e)
        {
            RunDiagnostics(packName, Constants.CurrentStyle, e);
        }
    }

    public static void ChangeSpriteSheetStyles(string styleName)
    {
        IconPack pack = null;
        TMP_SpriteAsset asset = null;

        try
        {
            if ((styleName == "Vanilla" || Constants.CurrentPack == "Vanilla") && Asset)
            {
                MaterialReferenceManager.instance.m_SpriteAssetReferenceLookup[CacheDefaultSpriteSheet.Cache] = Asset;
                MaterialReferenceManager.instance.m_FontMaterialReferenceLookup[CacheDefaultSpriteSheet.Cache] = Asset.material;
            }
            else if (styleName != "Vanilla" && IconPacks.TryGetValue(Constants.CurrentPack, out pack) && pack != null && pack.MentionStyles.TryGetValue(styleName, out asset) && asset)
            {
                MaterialReferenceManager.instance.m_SpriteAssetReferenceLookup[CacheDefaultSpriteSheet.Cache] = asset;
                MaterialReferenceManager.instance.m_FontMaterialReferenceLookup[CacheDefaultSpriteSheet.Cache] = asset.material;
            }
        }
        catch (Exception e)
        {
            RunDiagnostics(Constants.CurrentPack, styleName, e);
        }
    }

    private static void RunDiagnostics(string packName, string styleName, Exception e)
    {
        IconPack pack = null;
        TMP_SpriteAsset asset = null;
        var diagnostic = $"Uh oh, something happened here in AssetManager.ChangeSpriteSheets\nPack Name: {packName}\nStyle Name: {styleName}";

        if (!Asset)
            diagnostic += "\nVanilla Sheet Does Not Exist";

        if (packName != "Vanilla" && !IconPacks.TryGetValue(packName, out pack))
            diagnostic += "\nNo Loaded Icon Pack";
        else if (pack == null)
            diagnostic += "\nLoaded Icon Pack Was Null";
        else if (!pack.MentionStyles.TryGetValue(styleName, out asset))
            diagnostic += "\nLoaded Icon Pack Does Not Have A Valid Mention Style";
        else if (!asset)
            diagnostic += "\nLoaded Mention Style Was Null";

        diagnostic += $"\nError: {e}";
        Recolors.LogError(diagnostic);
    }

    public static void SetScrollSprites()
    {
        try
        {
            Service.Home.Scrolls.scrollInfoLookup_.ForEach((_, y) =>
            {
                var sprite = GetSprite(Utils.RoleName(y.role), false);

                if (sprite != Blank)
                {
                    if (!CacheScrollSprites.ContainsKey(y.id))
                        CacheScrollSprites[y.id] = y.decoration.sprite;

                    y.decoration.sprite = sprite;
                }
                else if (CacheScrollSprites.TryGetValue(y.id, out sprite))
                    y.decoration.sprite = sprite;
                else
                    CacheScrollSprites[y.id] = y.decoration.sprite;
            });

            Service.Home.Scrolls.cursedScrollInfoLookup_.ForEach((_, y) =>
            {
                var sprite = GetSprite(Utils.RoleName(y.role), false);

                if (sprite != Blank)
                {
                    if (!CacheScrollSprites.ContainsKey(y.id))
                        CacheScrollSprites[y.id] = y.decoration.sprite;

                    y.decoration.sprite = sprite;
                }
                else if (CacheScrollSprites.TryGetValue(y.id, out sprite))
                    y.decoration.sprite = sprite;
                else
                    CacheScrollSprites[y.id] = y.decoration.sprite;
            });
        }
        catch (Exception e)
        {
            Recolors.LogError(e);
        }
    }

    // thanks stackoverflow and pat
    // https://stackoverflow.com/questions/51315918/how-to-encodetopng-compressed-textures-in-unity
    public static Texture2D Decompress(this Texture2D source)
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
        return readableText;
    }

    // love ya pat
    public static void LoadVanillaSpriteSheet(bool change)
    {
        try
        {
            if (SpriteSheetLoaded || Asset)
                return;

            SpriteSheetLoaded = true;
            var (rolesWithIndexDict, rolesWithIndex) = Utils.Filtered();
            var textures = new List<Texture2D>();

            // now get all the sprites that we want to load
            foreach (var (role, roleInt) in rolesWithIndex)
            {
                var actualRole = (Role)roleInt;
                var sprite = Service.Game.Roles.roleInfoLookup[actualRole].sprite;
                sprite.texture.name = role;
                textures.Add(sprite.texture);
            }

            // set spritecharacter name to "Role{number}" so that the game can find correct roles
            Asset = BuildGlyphs(textures.ToArray(), "RoleIcons", x => x.name = rolesWithIndexDict[(x.glyph as TMP_SpriteGlyph).sprite.name.ToLower()]);
            Recolors.LogMessage("Vanilla Sprite Asset loaded!");

            if (change)
            {
                ChangeSpriteSheets("Vanilla");
                Recolors.LogMessage("Vanilla Sprite Asset added!");
            }

            var assetPath = Path.Combine(VanillaPath, "RoleIcons_Modified.png");

            if (File.Exists(assetPath))
                File.Delete(assetPath);

            File.WriteAllBytes(assetPath, (Asset.spriteSheet as Texture2D).Decompress().EncodeToPNG());
        }
        catch (Exception e)
        {
            Recolors.LogError(e);
            Asset = null;
            SpriteSheetLoaded = false;
        }
    }

    // courtesy of pat, love ya mate
    public static TMP_SpriteAsset BuildGlyphs(Texture2D[] textures, string spriteAssetName, Action<TMP_SpriteCharacter> action)
    {
        var asset = ScriptableObject.CreateInstance<TMP_SpriteAsset>();
        var image = new Texture2D(2048, 2048) { name = spriteAssetName };
        var rects = image.PackTextures(textures, 2);

        for (uint i = 0; i < rects.Length; i++)
        {
            var sprite = Sprite.Create(textures[i], new(0, 0, textures[i].width, textures[i].height), new(0, 0), 100);
            sprite.name = textures[i].name;

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
                index = i,
                sprite = sprite,
            };

            var character = new TMP_SpriteCharacter(0, glyph)
            {
                name = textures[i].name,
                glyphIndex = i,
            };

            // renaming to $"Role{(int)Role}" should occur here
            action?.Invoke(character);

            asset.spriteGlyphTable.Add(glyph);
            asset.spriteCharacterTable.Add(character);
        }

        asset.name = spriteAssetName;
        asset.material = new(Shader.Find("TextMeshPro/Sprite"));
        asset.version = "1.1.0";
        asset.material.mainTexture = image;
        asset.spriteSheet = image;
        asset.UpdateLookupTables();
        return asset;
    }
}