using FancyUI.Assets.IconPacks;
using FancyUI.Assets.SilhouetteSwapper;
using NewModLoading;

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

            Vanilla1 = AssetManager.BuildGlyphs(sprites, "RoleIcons", index.Item1);
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

            Vanilla2 = AssetManager.BuildGlyphs(sprites, "PlayerNumbers", dict);
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

            for (var i = 1; i < 7; i++)
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

            Vanilla3 = AssetManager.BuildGlyphs(sprites, "Emoji", dict);
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

            BTOS22 = AssetManager.BuildGlyphs(sprites, "BTOSRoleIcons", index.Item1);
            Utils.DumpSprite(BTOS22.spriteSheet as Texture2D, "BTOS2RoleIcons_Modified", Path.Combine(IPPath, "BTOS2"));
        }
        catch (Exception e)
        {
            Fancy.Instance.Error($"Unable to create modified btos role icons sheet because:\n{e}");
            BTOS22 = null;
        }
    }
}