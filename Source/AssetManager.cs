using Home.Services;

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
    public static Sprite JailorSpecial;
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

    public static readonly Role[] ExceptRoles = { Role.NONE, Role.ROLE_COUNT, Role.UNKNOWN, Role.HANGMAN };

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
                else if (x.Contains("JailorSpecial"))
                    JailorSpecial = sprite;
                else if (x.Contains("Attack"))
                    Attack = sprite;
                else if (x.Contains("Defense"))
                    Defense = sprite;
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
                    SetScrollSprites(Service.Home.Scrolls);
            }
        }
        catch (Exception e)
        {
            Recolors.LogError(e);
        }
    }

    public static void ChangeSpriteSheets(string packName)
    {
        try
        {
            if (packName == "Vanilla" && Asset)
            {
                MaterialReferenceManager.instance.m_SpriteAssetReferenceLookup[CacheDefaultSpriteSheet.Cache] = Asset;
                MaterialReferenceManager.instance.m_FontMaterialReferenceLookup[CacheDefaultSpriteSheet.Cache] = Asset.material;
            }
            else if (packName != "Vanilla" && IconPacks.TryGetValue(packName, out var pack) && pack != null && pack.Asset)
            {
                MaterialReferenceManager.instance.m_SpriteAssetReferenceLookup[CacheDefaultSpriteSheet.Cache] = pack.Asset;
                MaterialReferenceManager.instance.m_FontMaterialReferenceLookup[CacheDefaultSpriteSheet.Cache] = pack.Asset.material;
            }
            else
                Recolors.LogError("Uh oh, something happened here in AssetManager.ChangeSpriteSheets");
        }
        catch (Exception e)
        {
            Recolors.LogError(e);
        }
    }

    public static void SetScrollSprites(HomeScrollService __instance)
    {
        try
        {
            __instance.scrollInfoLookup_.ForEach((_, y) =>
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

            __instance.cursedScrollInfoLookup_.ForEach((_, y) =>
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

    //thanks stackoverflow and pat
    //https://stackoverflow.com/questions/51315918/how-to-encodetopng-compressed-textures-in-unity
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

            // these roles dont have sprites so just ignore them
            var roles = ((Role[])Enum.GetValues(typeof(Role))).Except(ExceptRoles);

            // map all roles to (role name, role number) so we can make a dict
            var rolesWithIndex = roles.Select(role => (role.ToString().ToLower(), (int)role));

            // dict allows us to find dict[rolename.tolower] and get Role{number} for later use in spritecharacters
            var rolesWithIndexDict = rolesWithIndex.ToDictionary(rolesSelect => rolesSelect.Item1.ToLower(), rolesSelect => $"Role{rolesSelect.Item2}");
            var textures = new List<Texture2D>();

            // now get all the sprites that we want to load
            foreach (var (role, roleInt) in rolesWithIndex)
            {
                var actualRole = (Role)roleInt;
                var sprite = actualRole switch
                {
                    Role.VAMPIRE => Vampire,
                    Role.CURSED_SOUL => CursedSoul,
                    Role.GHOST_TOWN => GhostTown,
                    _ => Service.Game.Roles.roleInfoLookup[actualRole].sprite
                };
                sprite.texture.name = role;
                textures.Add(sprite.texture);
            }

            var assetBuilder = new SpriteAssetBuilder(256, 256, 10);
            Asset = assetBuilder.BuildGlyphs(textures.ToArray(), "RoleIcons", x => x.name = rolesWithIndexDict[(x.glyph as TMP_SpriteGlyph).sprite.name.ToLower()]);
            // set spritecharacter name to "Role{number}" so that the game can find correct roles
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
}