namespace IconPacks;

public static class AssetManager
{
    private const string Resources = "IconPacks.Resources.";

    public static readonly Dictionary<string, Dictionary<string, List<Sprite>>> GlobalEasterEggs = new();

    public static readonly Dictionary<string, IconPack> IconPacks = new();
    public static readonly Dictionary<int, Sprite> CacheScrollSprites = new();

    public static Sprite Blank;
    public static Sprite Thumbnail;
    public static Sprite Attack;
    public static Sprite Defense;

    public static TMP_SpriteAsset VanillaAsset;
    public static bool SpriteSheetLoaded;

    public static string ModPath => Path.Combine(Path.GetDirectoryName(Application.dataPath), "SalemModLoader", "ModFolders", "Recolors");
    public static string VanillaPath => Path.Combine(ModPath, "Vanilla");

    private static readonly string[] Avoid = { "Attributes", "Necronomicon", "Neutral", "NeutralApocalypse", "NeutralEvil", "NeutralKilling", "Town", "TownInvestigative",
        "TownKilling", "TownSupport", "TownProtective", "TownPower", "CovenKilling", "CovenDeception", "CovenUtility", "CovenPower", "Coven", "SlowMode", "FastMode", "AnonVoting",
        "SecretKillers", "HiddenRoles", "OneTrial", "Any", "PerfectTown", "Hidden", "Stoned" };

    private static readonly string[] ToRemove = { ".png", ".jpg" };

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
            Recolors.LogError($"Error finding {packName} in loaded packs");
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
            Recolors.LogError($"Error finding {name}'s sprite\n{e}");
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
                var sprite = FromResources.LoadSprite(x);
                UObject.DontDestroyOnLoad(sprite);

                if (x.Contains("Blank"))
                    Blank = sprite;
                else if (x.Contains("Thumbnail"))
                    Thumbnail = sprite;
            }
        });

        TryLoadingSprites(Constants.CurrentPack);
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
            texture.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontUnloadUnusedAsset;
            texture.name = fileName;
            UObject.DontDestroyOnLoad(texture);
            return texture.Decompress();
        }
        catch (Exception e)
        {
            Recolors.LogError($"Error loading {folder} > {subfolder} > {fileName} ({filetype})\n{e}");
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
                Recolors.LogError($"Path {folder} > {subfolder} > {fileName} was missing");
                return null;
            }

            var texture = LoadDiskTexture(path.SanitisePath(), subfolder, folder, filetype);

            if (texture == null)
            {
                Recolors.LogError($"Uh oh texture loading error at {path}");
                return null;
            }

            var sprite = Sprite.Create(texture, new(0, 0, texture.width, texture.height), new(0.5f, 0.5f), 100);

            if (sprite == null)
            {
                Recolors.LogError($"Uh oh sprite loading error at {path}");
                return null;
            }

            sprite.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontUnloadUnusedAsset;
            sprite.name = texture.name;
            UObject.DontDestroyOnLoad(sprite);
            return sprite;
        }
        catch (Exception e)
        {
            Recolors.LogError(e);
            return null;
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
                exists?.Delete();
                exists = new(packName);
                exists.Load();
                IconPacks.Add(packName, exists);

                if (PatchRoleService.ServiceExists)
                    exists.LoadSpriteSheets(true);

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
            if ((packName == "Vanilla" || Constants.CurrentStyle == "Vanilla" ) && VanillaAsset)
            {
                MaterialReferenceManager.instance.m_SpriteAssetReferenceLookup[CacheDefaultSpriteSheet.Hash1] = VanillaAsset ?? CacheDefaultSpriteSheet.Cache1;
                MaterialReferenceManager.instance.m_FontMaterialReferenceLookup[CacheDefaultSpriteSheet.Hash1] = VanillaAsset?.material ?? CacheDefaultSpriteSheet.Cache1?.material;
            }
            else if (packName != "Vanilla" && IconPacks.TryGetValue(packName, out pack) && pack != null && pack.MentionStyles.TryGetValue(Constants.CurrentStyle, out asset) && asset)
            {
                MaterialReferenceManager.instance.m_SpriteAssetReferenceLookup[CacheDefaultSpriteSheet.Hash1] = asset ?? CacheDefaultSpriteSheet.Cache1;
                MaterialReferenceManager.instance.m_FontMaterialReferenceLookup[CacheDefaultSpriteSheet.Hash1] = asset?.material ?? CacheDefaultSpriteSheet.Cache1?.material;
            }
        }
        catch (Exception e)
        {
            RunDiagnostics(e);
        }
    }

    public static void ChangeSpriteSheetStyles(string styleName)
    {
        IconPack pack = null;
        TMP_SpriteAsset asset = null;

        try
        {
            if ((styleName == "Vanilla" || Constants.CurrentPack == "Vanilla") && VanillaAsset)
            {
                MaterialReferenceManager.instance.m_SpriteAssetReferenceLookup[CacheDefaultSpriteSheet.Hash1] = VanillaAsset ?? CacheDefaultSpriteSheet.Cache1;
                MaterialReferenceManager.instance.m_FontMaterialReferenceLookup[CacheDefaultSpriteSheet.Hash1] = VanillaAsset?.material ?? CacheDefaultSpriteSheet.Cache1?.material;
            }
            else if (styleName != "Vanilla" && IconPacks.TryGetValue(Constants.CurrentPack, out pack) && pack != null && pack.MentionStyles.TryGetValue(styleName, out asset) && asset)
            {
                MaterialReferenceManager.instance.m_SpriteAssetReferenceLookup[CacheDefaultSpriteSheet.Hash1] = asset ?? CacheDefaultSpriteSheet.Cache1;
                MaterialReferenceManager.instance.m_FontMaterialReferenceLookup[CacheDefaultSpriteSheet.Hash1] = asset?.material ?? CacheDefaultSpriteSheet.Cache1?.material;
            }
        }
        catch (Exception e)
        {
            RunDiagnostics(e);
        }
    }

    private static void RunDiagnostics(Exception e)
    {
        IconPack pack = null;
        TMP_SpriteAsset asset = null;
        var diagnostic = $"Uh oh, something happened here in AssetManager.ChangeSpriteSheets\nPack Name: {Constants.CurrentPack}\nStyle Name: {Constants.CurrentStyle}";

        if (!CacheDefaultSpriteSheet.Cache1)
            diagnostic += "\nVanilla Sheet Does Not Exist";

        if (!VanillaAsset)
            diagnostic += "\nModified Vanilla Sheet Does Not Exist";

        if (Constants.EnableIcons && !IconPacks.TryGetValue(Constants.CurrentPack, out pack))
            diagnostic += "\nNo Loaded Icon Pack";
        else if (pack == null)
            diagnostic += "\nLoaded Icon Pack Was Null";
        else if (!pack.MentionStyles.TryGetValue(Constants.CurrentStyle, out asset))
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

                if (!CacheScrollSprites.ContainsKey(y.id))
                    CacheScrollSprites[y.id] = y.decoration.sprite;

                if (sprite != Blank)
                    y.decoration.sprite = sprite;
                else if (CacheScrollSprites.TryGetValue(y.id, out sprite))
                    y.decoration.sprite = sprite;
                else
                    CacheScrollSprites[y.id] = y.decoration.sprite;
            });

            Service.Home.Scrolls.cursedScrollInfoLookup_.ForEach((_, y) =>
            {
                var sprite = GetSprite(Utils.RoleName(y.role), false);

                if (!CacheScrollSprites.ContainsKey(y.id))
                    CacheScrollSprites[y.id] = y.decoration.sprite;

                if (sprite != Blank)
                    y.decoration.sprite = sprite;
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
            if (SpriteSheetLoaded)
                return;

            SpriteSheetLoaded = true;
            var (rolesWithIndexDict, rolesWithIndex) = Utils.Filtered();
            var textures = new List<Texture2D>();
            var sprites = new List<Sprite>();

            // now get all the sprites that we want to load
            foreach (var (role, roleInt) in rolesWithIndex)
            {
                var actualRole = (Role)roleInt;
                var sprite = Service.Game.Roles.roleInfoLookup[actualRole].sprite;
                sprite.name = sprite.texture.name = role;
                textures.Add(sprite.texture);
                sprites.Add(sprite);
            }

            // set spritecharacter name to "Role{number}" so that the game can find correct roles
            VanillaAsset = BuildGlyphs(sprites.ToArray(), textures.ToArray(), "RoleIcons", rolesWithIndexDict);
            Utils.DumpSprite(VanillaAsset.spriteSheet as Texture2D, "RoleIcons_Modified.png", VanillaPath);
            Recolors.LogMessage("Vanilla Sprite Asset loaded!");

            if (change)
            {
                ChangeSpriteSheets("Vanilla");
                Recolors.LogMessage("Set to Vanilla Sprite Asset!");
            }
        }
        catch (Exception e)
        {
            Recolors.LogError(e);
            VanillaAsset = null;
            SpriteSheetLoaded = false;
        }
    }

    // courtesy of pat, love ya mate
    public static TMP_SpriteAsset BuildGlyphs(Sprite[] sprites, Texture2D[] textures, string spriteAssetName, Dictionary<string, string> rolesWithIndexDict)
    {
        var asset = ScriptableObject.CreateInstance<TMP_SpriteAsset>();
        var image = new Texture2D(4096, 2048) { name = spriteAssetName };
        var rects = image.PackTextures(textures, 2);

        for (uint i = 0; i < rects.Length; i++)
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
                index = i,
                sprite = sprites[i],
            };

            var character = new TMP_SpriteCharacter(0, glyph)
            {
                // renaming to $"Role{(int)role}" should occur here
                name = rolesWithIndexDict[glyph.sprite.name.ToLower()],
                glyphIndex = i,
            };


            asset.spriteGlyphTable.Add(glyph);
            asset.spriteCharacterTable.Add(character);
        }

        asset.name = spriteAssetName;
        asset.material = new(Shader.Find("TextMeshPro/Sprite"));
        asset.version = "1.1.0";
        asset.material.mainTexture = image;
        asset.spriteSheet = image;
        asset.UpdateLookupTables();
        asset.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontUnloadUnusedAsset;
        return asset;
    }
}