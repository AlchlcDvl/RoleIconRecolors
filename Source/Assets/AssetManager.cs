using FancyUI.Assets.SilhouetteSwapper;
using FancyUI.Assets.IconPacks;

namespace FancyUI.Assets;

public static class FancyAssetManager
{
    public const string Resources = "FancyUI.Resources.";

    public static readonly Dictionary<string, Dictionary<string, List<Sprite>>> GlobalEasterEggs = [];
    public static readonly Dictionary<string, IconPack> IconPacks = [];
    public static readonly Dictionary<int, Sprite> CacheScrollSprites = [];

    public static Sprite Blank { get; set; }
    public static Sprite Thumbnail { get; set; }
    public static Sprite Attack { get; set; }
    public static Sprite Defense { get; set; }
    public static Sprite Ethereal { get; set; }

    public static TMP_SpriteAsset Vanilla1 { get; set; }
    public static TMP_SpriteAsset Vanilla2 { get; set; }
    public static TMP_SpriteAsset BTOS2_1 { get; set; }
    public static TMP_SpriteAsset BTOS2_2 { get; set; }

    public static Material Grayscale { get; set; }
    public static Material DefaultWood { get; set; }

    public static SilhouetteAnimation Loading { get; set; }

    public static string IPPath => Path.Combine(Fancy.Instance.ModPath, "IconPacks");
    public static string SSPath => Path.Combine(Fancy.Instance.ModPath, "SilhouetteSets");

    private static readonly string[] Avoid = [ "Necronomicon", "Recruit", "Doused", "ExeTarget", "Hexed", "Knighted", "Bread", "Revealed", "Disconnected", "Connecting", "Plagued", "Revealed",
        "Trapped", "Hangover", "Silenced", "Dreamwoven", "Insane", "Bugged", "Tracked", "Sickness", "Reaped", "Deafened", "Audited", "Enchanted", "Accompanied", "Banned", "WarlockCursed" ];

    private static Assembly Core => typeof(Fancy).Assembly;

    public static Sprite GetSprite(bool skipFactionless, string name, string faction, bool allowEE = false, string packName = null) => GetSprite(name, allowEE, faction, packName,
        skipFactionless);

    public static Sprite GetSprite(string name, string faction, bool allowEE = true, string packName = null, bool skipFactionless = false) => GetSprite(name, allowEE, faction, packName,
        skipFactionless);

    public static Sprite GetSprite(string name, bool allowEE = true, string faction = null, string packName = null, bool skipFactionless = false)
    {
        if (name.Contains("Blank") || !Constants.EnableIcons() || IconPacks.Count == 0)
            return Blank;

        packName ??= Constants.CurrentPack();

        if (!IconPacks.TryGetValue(packName, out var pack))
        {
            Fancy.Instance.Error($"Error finding {packName} in loaded packs");
            ModSettings.SetString("Selected Icon Pack", "Vanilla", "alchlcsystm.fancy.ui");
            return Blank;
        }

        if (StringUtils.IsNullEmptyOrWhiteSpace(faction) || faction == "Blank" || Avoid.Any(name.Contains))
            faction = "Regular";

        var og = faction;

        if (Constants.IsNecroActive())
            faction = "Necronomicon";
        else if (Constants.IsLocalVIP())
            faction = "VIP";

        var mod = Utils.GetGameType();

        try
        {
            var sprite = pack.GetSprite($"{name}_{mod}", allowEE, faction);

            if (!sprite.IsValid())
                sprite = pack.GetSprite(name, allowEE, faction);

            if (!sprite.IsValid() && og != faction)
            {
                sprite = pack.GetSprite($"{name}_{mod}", allowEE, og);

                if (!sprite.IsValid())
                    sprite = pack.GetSprite(name, allowEE, og);
            }

            if (faction != "Regular" && !sprite.IsValid())
            {
                sprite = pack.GetSprite($"{name}_{mod}", allowEE, "Regular");

                if (faction != "Regular" && !sprite.IsValid())
                    sprite = pack.GetSprite(name, allowEE, "Regular");
            }

            if (!sprite.IsValid() && faction != "Factionless" && !skipFactionless)
            {
                sprite = pack.GetSprite($"{name}_{mod}", allowEE, "Factionless");

                if (!sprite.IsValid())
                    sprite = pack.GetSprite(name, allowEE, "Factionless");
            }

            return sprite ?? Blank;
        }
        catch (Exception e)
        {
            Fancy.Instance.Error($"Error finding {name}'s sprite from {packName} {faction} during a {mod} game\n{e}");
            return Blank;
        }
    }

    public static void LoadBTOS()
    {
        if (!Constants.BTOS2Exists())
            return;

        Fancy.Instance.Message("BTOS2 Detected; Initiating Compatibility...");
        BTOS2Compatibility.BTOS2Patched = BTOS2Compatibility.Init();

        if (!BTOS2Compatibility.BTOS2Patched)
            return;

        var btos = Path.Combine(IPPath, "BTOS2");

        if (!Directory.Exists(btos))
            Directory.CreateDirectory(btos);

        btos = Path.Combine(SSPath, "BTOS2");

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

        Utils.DumpSprite(BTOS2_1.spriteSheet as Texture2D, "BTOSRoleIcons", Path.Combine(IPPath, "BTOS2"), true);

        Fancy.Assets.Bundles[BetterTOS2.BTOSInfo.assetBundle.name] = BetterTOS2.BTOSInfo.assetBundle;
        BetterTOS2.BTOSInfo.assetBundle.GetAllAssetNames().ForEach(x => Fancy.Assets.ObjectToBundle[AssetManager.ConvertToBaseName(x)] = BetterTOS2.BTOSInfo.assetBundle.name);
    }

    public static string FancySanitisePath(this string path, bool removeIcon = false)
    {
        path = path.SanitisePath();
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

    public static void TryLoadingSprites(string packName, PackType type)
    {
        var folder = Path.Combine(Fancy.Instance.ModPath, $"{type}", packName);

        if (!Directory.Exists(folder))
        {
            Fancy.Instance.Error($"{packName} was missing");
            ModSettings.SetString("Selected Icon Pack", "Vanilla", "alchlcsystm.fancy.ui");
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
            Fancy.Instance.Error($"Unable to load icon pack {packName} because:\n{e}");
        }
    }

    public static void RunDiagnostics(Exception e)
    {
        IconPack pack = null;
        TMP_SpriteAsset asset = null;
        IconAssets assets = null;
        var game = Utils.GetGameType();
        var diagnostic = $"Uh oh, something happened here\nPack Name: {Constants.CurrentPack()}\nStyle Name: {Constants.CurrentStyle()}\nFaction Override: {Constants.FactionOverride()}\n" +
            $"Custom Numbers: {Constants.CustomNumbers()}";

        if (!CacheDefaults.RoleIcons)
            diagnostic += "\nVanilla Sheet Does Not Exist";

        if (!CacheDefaults.Numbers)
            diagnostic += "\nVanilla Player Numbers Sheet Does Not Exist";

        if (!Vanilla1)
            diagnostic += "\nModified Vanilla Sheet Does Not Exist";

        if (!Vanilla2)
            diagnostic += "\nModified Player Numbers Sheet Does Not Exist";

        if (Constants.BTOS2Exists())
        {
            if (!BTOS2_1)
                diagnostic += "\nBTOS2 Sheet Does Not Exist";

            if (!BTOS2_2)
                diagnostic += "\nModified BTOS2 Sheet Does Not Exist";
        }

        diagnostic += $"\nCurrently In A {game} Game";

        if (Constants.EnableIcons() && !IconPacks.TryGetValue(Constants.CurrentPack(), out pack))
            diagnostic += "\nNo Loaded Icon Pack";
        else if (pack == null)
            diagnostic += "\nLoaded Icon Pack Was Null";
        else if (!pack.PlayerNumbers)
            diagnostic += "\nLoaded Player Numbers Was Null";
        else if (!pack.Assets.TryGetValue(game, out assets))
            diagnostic += "\nInvalid Game Type Was Detected";
        else if (!assets.MentionStyles.TryGetValue(Constants.CurrentStyle(), out asset))
            diagnostic += "\nLoaded Icon Pack Does Not Have A Valid Mention Style";
        else if (!asset)
            diagnostic += "\nLoaded Mention Style Was Null";

        diagnostic += $"\nError: {e}";
        Fancy.Instance.Error(diagnostic);
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
            Fancy.Instance.Error($"Unable to set scroll sprites because:\n{e}");
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
            return readableText.DontUnload().DontDestroy();
        }
        catch (Exception e)
        {
            Fancy.Instance.Error($"Unable to decompress {source.name} because:\n{e}");
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
                var sprite = Fancy.Assets.GetSprite(name + "_Vanilla") ?? Blank;

                if (!sprite.IsValid())
                    sprite = Fancy.Assets.GetSprite(name) ?? Blank;

                if (sprite.IsValid())
                {
                    sprite.name = sprite.texture.name = role;
                    sprites.Add(sprite);
                }
                else
                    Fancy.Instance.Warning($"NO VANILLA ICON FOR {name}?!");
            }

            Vanilla1 = BuildGlyphs([..sprites], "RoleIcons", index);
            Utils.DumpSprite(Vanilla1.spriteSheet as Texture2D, "RoleIcons_Modified", Path.Combine(IPPath, "Vanilla"));
        }
        catch (Exception e)
        {
            Fancy.Instance.Error($"Unable to create modified vanilla role icons sheet because:\n{e}");
            Vanilla1 = null;
        }

        try
        {
            var dict = new List<string>();
            var sprites = new List<Sprite>();

            for (var i = 0; i < 16; i++)
            {
                var sprite = Fancy.Assets.GetSprite($"{i}") ?? Blank;

                if (sprite.IsValid())
                {
                    sprite.name = sprite.texture.name = $"PlayerNumbers_{i}";
                    sprites.Add(sprite);
                }
                else
                    Fancy.Instance.Warning($"NO NUMBER ICON FOR {i}?!");

                dict.Add($"PlayerNumbers_{i}");
            }

            Vanilla2 = BuildGlyphs([..sprites], "PlayerNumbers", dict.ToDictionary(x => x, x => (x, 0)), false);
            Utils.DumpSprite(Vanilla2.spriteSheet as Texture2D, "PlayerNumbers_Modified", Path.Combine(IPPath, "Vanilla"));
        }
        catch (Exception e)
        {
            Fancy.Instance.Error($"Unable to create modified player numbers sheet because:\n{e}");
            Vanilla2 = null;
        }
    }
    public static void LoadBTOS2SpriteSheet()
    {
        if (!Constants.BTOS2Exists())
            return;

        try
        {
            var index = Utils.Filtered(ModType.BTOS2);
            var sprites = new List<Sprite>();

            foreach (var (role, (_, roleInt)) in index)
            {
                var name = Utils.RoleName((Role)roleInt, ModType.BTOS2);
                var sprite = Fancy.Assets.GetSprite(name + "_BTOS2") ?? Blank;

                if (!sprite.IsValid())
                    sprite = Fancy.Assets.GetSprite(name) ?? Blank;

                if (sprite.IsValid())
                {
                    sprite.name = sprite.texture.name = role;
                    sprites.Add(sprite);
                }
                else
                    Fancy.Instance.Warning($"NO BTOS2 ICON FOR {name}?!");
            }

            BTOS2_2 = BuildGlyphs([..sprites], "BTOSRoleIcons", index);
            Utils.DumpSprite(BTOS2_2.spriteSheet as Texture2D, "BTOS2RoleIcons_Modified", Path.Combine(IPPath, "BTOS2"));
        }
        catch (Exception e)
        {
            Fancy.Instance.Error($"Unable to create modified btos role icons sheet because:\n{e}");
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
        return asset.DontUnload().DontDestroy();
    }
}