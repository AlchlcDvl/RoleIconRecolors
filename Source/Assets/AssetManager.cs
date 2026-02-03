using FancyUI.Assets.IconPacks;
using FancyUI.Assets.SilhouetteSwapper;
using NewModLoading;
using SalemModLoader;
using UnityEngine.TextCore;

namespace FancyUI.Assets;

public static class FancyAssetManager
{
    public static readonly Dictionary<string, Dictionary<string, HashSet<Sprite>>> GlobalEasterEggs = [];
    public static readonly Dictionary<string, IconPack> IconPacks = [];
    private static readonly Dictionary<int, Sprite> CacheScrollSprites = [];

    public static Sprite Blank { get; set; }
    public static Sprite Attack { get; set; }
    public static Sprite Defense { get; set; }
    public static Sprite Ethereal { get; set; }

    public static TMP_SpriteAsset Vanilla1 { get; private set; }
    public static TMP_SpriteAsset Vanilla2 { get; private set; }
    public static TMP_SpriteAsset Vanilla3 { get; private set; }
    public static TMP_SpriteAsset BTOS21 { get; private set; }
    public static TMP_SpriteAsset BTOS22 { get; private set; }

    public static Material Grayscale { get; set; }

    public static Gif LoadingGif { get; set; }
    public static Gif Flame { get; set; }
    public static SilhouetteAnimation Loading { get; set; }

    public static string IPPath => Path.Combine(Fancy.Instance.ModPath, "IconPacks");
    public static string SSPath => Path.Combine(Fancy.Instance.ModPath, "SilhouetteSets");

    private static readonly string[] Avoid = [ "Recruit", "Doused", "ExeTarget", "Hexed", "Knighted", "Bread", "Revealed", "Disconnected", "Connecting", "Plagued", "Revealed", "Trapped",
        "Hangover", "Silenced", "Dreamwoven", "Insane", "Bugged", "Tracked", "Sickness", "Reaped", "Deafened", "Audited", "Enchanted", "Accompanied", "Banned", "WarlockCursed" ];

    public static Sprite GetSprite(bool skipFactionless, string name, string faction, bool allowEe = false, string packName = null) => GetSprite(name, allowEe, faction, packName,
        skipFactionless);

    public static Sprite GetSprite(string name, string faction, bool allowEe = true, string packName = null, bool skipFactionless = false) => GetSprite(name, allowEe, faction, packName,
        skipFactionless);

    public static Sprite GetSprite(string name, bool allowEe = true, string faction = null, string packName = null, bool skipFactionless = false)
    {
        if (name.Contains("Blank") || !Constants.EnableIcons() || IconPacks.Count == 0)
            return Blank;

        packName ??= Fancy.SelectedIconPack.Value;

        if (!IconPacks.TryGetValue(packName, out var pack))
        {
            Fancy.Instance.Error($"Error finding {packName} in loaded packs");
            Fancy.SelectedIconPack.Value = packName;
            return Blank;
        }

        if (NewModLoading.Utils.IsNullEmptyOrWhiteSpace(faction) || faction == "Blank" || Avoid.Any(name.Contains))
            faction = "Regular";

        var og = faction;

        if (Constants.IsNecroActive())
            faction = "Necronomicon";
        else if (Constants.IsLocalVip())
            faction = "VIP";

        var mod = Utils.GetGameType();

        try
        {
            var sprite = pack.GetSprite($"{name}_{mod}", allowEe, faction);

            if (!sprite.IsValid())
                sprite = pack.GetSprite(name, allowEe, faction);

            if (!sprite.IsValid() && og != faction)
            {
                sprite = pack.GetSprite($"{name}_{mod}", allowEe, og);

                if (!sprite.IsValid())
                    sprite = pack.GetSprite(name, allowEe, og);
            }

            if (faction != "Regular" && !sprite.IsValid())
            {
                sprite = pack.GetSprite($"{name}_{mod}", allowEe, "Regular");

                if (faction != "Regular" && !sprite.IsValid())
                    sprite = pack.GetSprite(name, allowEe, "Regular");
            }

            if (!sprite.IsValid() && faction != "Factionless" && !skipFactionless)
            {
                sprite = pack.GetSprite($"{name}_{mod}", allowEe, "Factionless");

                if (!sprite.IsValid())
                    sprite = pack.GetSprite(name, allowEe, "Factionless");
            }
            sprite.texture.anisoLevel = 4;
            sprite.texture.mipMapBias = -2;
            return sprite ?? Blank;
        }
        catch (Exception e)
        {
            Fancy.Instance.Error($"Error finding {name}'s sprite from {packName} {faction} during a {mod} game\n{e}");
            return Blank;
        }
    }

    public static void LoadBtos()
    {
        if (!Constants.BTOS2Exists())
            return;

        Fancy.Instance.Message("BTOS2 Detected; Initiating Compatibility...");
        Btos2Compatibility.Btos2Patched = Btos2Compatibility.Init();

        if (!Btos2Compatibility.Btos2Patched)
            return;

        var btos = Path.Combine(IPPath, "BTOS2");

        if (!Directory.Exists(btos))
            Directory.CreateDirectory(btos);

        btos = Path.Combine(SSPath, "BTOS2");

        if (!Directory.Exists(btos))
            Directory.CreateDirectory(btos);
    }

    public static string FancySanitisePath(this string path, bool removeIcon = false)
    {
        path = path.SanitisePath();
        path = path.Replace("Marshall", "Marshal");

        if (removeIcon)
        {
            for (var i = 0; i >= 0 && path.Contains("_Icon"); i++)
                path = path.Replace($"_Icon{i}", "");
        }

        return path;
    }

    public static void TryLoadingSprites(string packName, PackType type)
    {
        var folder = Path.Combine(Fancy.Instance.ModPath, $"{type}", packName);

        if (!Directory.Exists(folder))
        {
            Fancy.Instance.Error($"{packName} was missing");
            Fancy.SelectedIconPack.Value = "Vanilla";
            return;
        }

        if (type == PackType.IconPacks)
        {
            ReplaceTMPSpritesPatch.ClearCache();

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
    }

    public static void RunDiagnostics(Exception e)
    {
        IconPack pack = null;
        var game = Utils.GetGameType();
        var diagnostic = $"Uh oh, something happened here\nPack Name: {Fancy.SelectedIconPack.Value}\nStyle Name: {Constants.CurrentStyle()}\nFaction Override: {Constants.FactionOverride()}\nCustom Numbers: {Constants.CustomNumbers()}";

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
            if (!BTOS21)
                diagnostic += "\nBTOS2 Sheet Does Not Exist";

            if (!BTOS22)
                diagnostic += "\nModified BTOS2 Sheet Does Not Exist";
        }

        diagnostic += $"\nCurrently In A {game} Game";

        if (Constants.EnableIcons() && !IconPacks.TryGetValue(Fancy.SelectedIconPack.Value, out pack))
            diagnostic += "\nNo Loaded Icon Pack";
        else if (pack == null)
            diagnostic += "\nLoaded Icon Pack Was Null";
        else
        {
            if (!pack.PlayerNumbers)
                diagnostic += "\nLoaded Player Numbers Was Null";

            if (!pack.Emojis)
                diagnostic += "\nLoaded Emojis Was Null";

            if (!pack.Assets.TryGetValue(game, out var assets))
                diagnostic += "\nInvalid Game Type Was Detected";
            else if (!assets.MentionStyles.TryGetValue(Constants.CurrentStyle(), out var asset))
                diagnostic += "\nLoaded Icon Pack Does Not Have A Valid Mention Style";
            else if (!asset)
                diagnostic += "\nLoaded Mention Style Was Null";
        }

        diagnostic += $"\nError: {e}";
        Fancy.Instance.Error(diagnostic);
    }

    public static void SetScrollSprites()
    {
        try
        {
            Service.Home.Scrolls.scrollInfoLookup_.Values.Do(y =>
            {
                var sprite = GetSprite(Utils.RoleName(y.role), Utils.FactionName(y.role.GetFactionType()), false);

                if (!CacheScrollSprites.ContainsKey(y.id))
                    CacheScrollSprites[y.id] = y.decoration.sprite;

                if (sprite.IsValid() || CacheScrollSprites.TryGetValue(y.id, out sprite))
                    y.decoration.sprite = sprite;
                else
                    CacheScrollSprites[y.id] = y.decoration.sprite;
            });

            Service.Home.Scrolls.cursedScrollInfoLookup_.Values.Do(y =>
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

    // love ya pat
    public static void LoadVanillaSpriteSheets()
    {
        try
        {
            var index = Utils.Filtered();
            var sprites = new List<Sprite>();

            foreach (var (role, roleInt) in index.Item2)
            {
                var name = Utils.RoleName((Role)roleInt, GameModType.Vanilla);
                var sprite = Fancy.Instance.Assets.GetSprite($"{name}_Vanilla") ?? Blank;

                if (!sprite.IsValid())
                    sprite = Fancy.Instance.Assets.GetSprite(name) ?? Blank;

                if (sprite.IsValid())
                {
                    sprite.name = sprite.texture.name = role;
                    sprites.Add(sprite);
                }
                else
                    Fancy.Instance.Warning($"NO VANILLA ICON FOR {name}?!");
            }

            Vanilla1 = BuildGlyphs(sprites, "RoleIcons", index.Item1, 384f, 384f, 0f, 300f, 390f);
            Utils.DumpSprite(Vanilla1.spriteSheet as Texture2D, "RoleIcons_Modified", Path.Combine(IPPath, "Vanilla"));
        }
        catch (Exception e)
        {
            Fancy.Instance.Error($"Unable to create modified vanilla role icons sheet because:\n{e}");
            Vanilla1 = null;
        }

        try
        {
            var dict = new Dictionary<string, string>();
            var sprites = new List<Sprite>();

            for (var i = 0; i < 16; i++)
            {
                var sprite = Fancy.Instance.Assets.GetSprite($"{i}") ?? Blank;

                if (sprite.IsValid())
                {
                    sprite.name = sprite.texture.name = $"PlayerNumbers_{i}";
                    sprites.Add(sprite);
                }
                else
                    Fancy.Instance.Warning($"NO NUMBER ICON FOR {i}?!");

                dict.Add($"PlayerNumbers_{i}", $"PlayerNumbers_{i}");
            }

            Vanilla2 = BuildGlyphs(sprites, "PlayerNumbers", dict, 128f, 128f, 0f, 105f, 150f);
            Utils.DumpSprite(Vanilla2.spriteSheet as Texture2D, "PlayerNumbers_Modified", Path.Combine(IPPath, "Vanilla"));
        }
        catch (Exception e)
        {
            Fancy.Instance.Error($"Unable to create modified player numbers sheet because:\n{e}");
            Vanilla2 = null;
        }

        try
        {
            var dict = new Dictionary<string, string>();
            var sprites = new List<Sprite>();

            for (var i = 1; i < 11; i++)
            {
                var sprite = Fancy.Instance.Assets.GetSprite($"Emoji{i}") ?? Blank;

                if (sprite.IsValid())
                {
                    sprite.name = sprite.texture.name = $"Emoji{i}";
                    sprites.Add(sprite);
                }
                else
                    Fancy.Instance.Warning($"NO EMOJI FOR {i}?!");

                dict.Add($"Emoji{i}", $"Emoji{i}");
            }

            Vanilla3 = BuildGlyphs(sprites, "Emoji", dict, 384f, 384f, 0f, 300f, 390f);
            Utils.DumpSprite(Vanilla3.spriteSheet as Texture2D, "Emoji_Modified", Path.Combine(IPPath, "Vanilla"));
        }
        catch (Exception e)
        {
            Fancy.Instance.Error($"Unable to create modified emoji sheet because:\n{e}");
            Vanilla3 = null;
        }
    }

    public static void LoadBtos2SpriteSheet()
    {
        if (!Constants.BTOS2Exists())
            return;

        BTOS21 = BetterTOS2.BTOSInfo.assetBundle.LoadAsset<TMP_SpriteAsset>("Roles");

        foreach (var character in BTOS21.spriteGlyphTable)
        {
            character.metrics = new()
            {
                horizontalBearingX = 0f,
                horizontalBearingY = 224f
            };
        }

        Utils.DumpSprite(BTOS21.spriteSheet as Texture2D, "BTOSRoleIcons", Path.Combine(IPPath, "BTOS2"), true);
        Fancy.Instance.Assets.RegisterBundle(BetterTOS2.BTOSInfo.assetBundle);

        try
        {
            var index = Utils.Filtered(GameModType.BTOS2);
            var sprites = new List<Sprite>();

            foreach (var (role, roleInt) in index.Item2)
            {
                var name = Utils.RoleName((Role)roleInt, GameModType.BTOS2);
                var sprite = Fancy.Instance.Assets.GetSprite($"{name}_BTOS2") ?? Blank;

                if (!sprite.IsValid())
                    sprite = Fancy.Instance.Assets.GetSprite(name) ?? Blank;

                if (sprite.IsValid())
                {
                    sprite.name = sprite.texture.name = role;
                    sprites.Add(sprite);
                }
                else
                    Fancy.Instance.Warning($"NO BTOS2 ICON FOR {name}?!");
            }

            BTOS22 = BuildGlyphs(sprites, "BTOSRoleIcons", index.Item1, 256f, 256f, 0f, 224f, 256f);
            Utils.DumpSprite(BTOS22.spriteSheet as Texture2D, "BTOS2RoleIcons_Modified", Path.Combine(IPPath, "BTOS2"));
        }
        catch (Exception e)
        {
            Fancy.Instance.Error($"Unable to create modified btos role icons sheet because:\n{e}");
            BTOS22 = null;
        }
    }

    public static TMP_SpriteAsset BuildGlyphs(IEnumerable<Sprite> sprites, string spriteAssetName, Dictionary<string, string> index, float metricsWidth, float metricsHeight, float metricsHBX, float metricsHBY, float metricsHA)
    {
        Texture2D[] array = sprites.Select((Sprite x) => x.texture).ToArray();
        TMP_SpriteAsset tMP_SpriteAsset = ScriptableObject.CreateInstance<TMP_SpriteAsset>();
        Texture2D texture2D = new Texture2D(4096, 4096, TextureFormat.RGBA32, false)
        {
            name = spriteAssetName,
            filterMode = FilterMode.Trilinear,
            anisoLevel = 4,
            mipMapBias = -2
        };
        Rect[] array2 = texture2D.PackTextures(array, 2, 4096);
        for (var i = 0; i < array2.Length; i++)
        {
            Rect rect = array2[i];
            TMP_SpriteGlyph tMP_SpriteGlyph = new TMP_SpriteGlyph
            {
                glyphRect = new GlyphRect
                {
                    x = (int)(rect.x * (float)texture2D.width),
                    y = (int)(rect.y * (float)texture2D.height),
                    width = (int)(rect.width * (float)texture2D.width),
                    height = (int)(rect.height * (float)texture2D.height)
                },
                metrics = new GlyphMetrics
                {
                    width = metricsWidth,
                    height = metricsHeight,
                    horizontalBearingX = metricsHBX,
                    horizontalBearingY = metricsHBY,
                    horizontalAdvance = metricsHA
                },
                index = (uint)i,
                sprite = sprites.ElementAtOrDefault(i),
                scale = 1f
            };
            tMP_SpriteAsset.spriteGlyphTable.Add(tMP_SpriteGlyph);
            tMP_SpriteAsset.spriteCharacterTable.Add(new TMP_SpriteCharacter(0u, tMP_SpriteAsset, tMP_SpriteGlyph)
            {
                name = index[tMP_SpriteGlyph.sprite.name],
                glyphIndex = (uint)i,
                scale = 1f
            });
        }

        tMP_SpriteAsset.name = spriteAssetName;
        tMP_SpriteAsset.material = new Material(Shader.Find("TextMeshPro/Sprite"));
        Launch.TmpVersion.SetValue(tMP_SpriteAsset, "1.1.0");
        tMP_SpriteAsset.spriteSheet = texture2D;
        tMP_SpriteAsset.UpdateLookupTables();
        tMP_SpriteAsset.hashCode = TMP_TextUtilities.GetSimpleHashCode(tMP_SpriteAsset.name);
        tMP_SpriteAsset.material.SetTexture(ShaderUtilities.ID_MainTex, tMP_SpriteAsset.spriteSheet);
        return tMP_SpriteAsset.DontDestroy();
    }
}