using Home.Common.Settings;

namespace RecolorsMac;

public static class AssetManager
{
    private const string Resources = "RecolorsMac.Resources.";

    public static readonly Dictionary<string, List<Sprite>> RegEEIcons = new();
    public static readonly Dictionary<string, List<Sprite>> TTEEIcons = new();
    public static readonly Dictionary<string, List<Sprite>> VIPEEIcons = new();
    public static readonly Dictionary<string, IconPack> IconPacks = new();

    private static readonly List<string> SkippableNames = new() { "Admirer_Ability", "Amnesiac_Ability", "Arsonist_Ability", "Attributes_Coven", "Baker_Ability", "Berserker_Ability",
        "Bodyguard_Ability", "Cleric_Ability", "Coroner_Examine", "CovenLeader_Ability", "Crusader_Ability", "CursedSoul_Ability", "Death_Ability", "Dreamweaver_Ability", "Enchanter_Ability",
        "Executioner_Ability", "Famine_Ability", "HexMaster_Ability", "Illusionist_Ability", "Investigator_Ability", "Jailor_Ability", "Jester_Ability", "Jinx_Ability", "Lookout_Ability",
        "Medusa_Ability", "Monarch_Ability", "Necromancer_Ability_1", "Necromancer_Ability_2", "Pestilence_Ability", "Plaguebearer_Ability", "Poisoner_Ability", "PotionMaster_Ability_1",
        "PotionMaster_Ability_2", "Psychic_Ability", "Retributionist_Ability_1", "Retributionist_Ability_2", "Seer_Ability_1", "Seer_Ability_2", "SerialKiller_Ability", "Sheriff_Ability",
        "Shroud_Ability", "SoulCollector_Ability", "Spy_Ability", "TavernKeeper_Ability", "Tracker_Ability", "Trapper_Ability", "Trickster_Ability", "Vampire_Ability", "Veteran_Ability",
        "Vigilante_Ability", "VoodooMaster_Ability", "War_Ability_1", "War_Ability_2", "Werewolf_Ability_1", "Werewolf_Ability_2", "Wildling_Ability", "Witch_Ability_1", "Witch_Ability_2" };

    public static Sprite Blank;
    public static Sprite Thumbnail;
    public static Sprite DownloadRecolors;
    public static Sprite DownloadVanilla;
    public static Sprite CursedSoul;
    public static Sprite Vampire;

    public static string ModPath => Path.Combine(Path.GetDirectoryName(Application.dataPath), "SalemModLoader", "ModFolders", "Recolors");
    public static string DefaultPath => Path.Combine(ModPath, "Recolors");
    public static string VanillaPath => Path.Combine(ModPath, "Vanilla");
    public static readonly string[] Folders = new[] { "Base", "TTBase", "VIPBase", "EasterEggs", "TTEasterEggs", "VIPEasterEggs" };
    private static readonly string[] Avoid = new[] { "Attributes", "Necronomicon" };
    private static readonly string[] ToRemove = new[] { ".png" };

    private static Assembly Core => typeof(Recolors).Assembly;

    public static bool Skippable(string name) => SkippableNames.Contains(name);

    public static Sprite GetSprite(string name, bool allowEE = true)
    {
        try
        {
            if (name.Contains("Blank") || !Constants.EnableIcons || IconPacks.Count == 0)
                return Blank;

            if (!IconPacks.TryGetValue(Constants.CurrentPack, out var pack))
            {
                Recolors.LogError($"Error finding {Constants.CurrentPack} in loaded packs");
                ModSettings.SetString("Selected Icon Pack", "Vanilla", "alchlcsystm.recolors.windows");
                return Blank;
            }

            var nosettings = UObject.FindObjectOfType<GameGuideItemTemplate>() == null;

            if (!Avoid.Any(name.Contains))
            {
                if (nosettings && Constants.IsLocalTT)
                    return GetTTSprite(pack, name, allowEE);
                else if (nosettings && Constants.IsLocalVIP)
                    return GetVIPSprite(pack, name, allowEE);
                else
                    return GetRegSprite(pack, name, allowEE);
            }
            else
                return GetRegSprite(pack, name, allowEE);
        }
        catch (Exception e)
        {
            Recolors.LogError(e, true);
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
                else if (x.Contains("DownloadVanilla"))
                    DownloadVanilla = sprite;
                else if (x.Contains("DownloadRecolors"))
                    DownloadRecolors = sprite;
                else if (x.Contains("CursedSoul"))
                    CursedSoul = sprite;
                else if (x.Contains("Vampire"))
                    Vampire = sprite;
            }
        });

        TryLoadingSprites(Constants.CurrentPack, false);
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
            Recolors.LogError(e, true);
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

    public static void TryLoadingSprites(string packName) => TryLoadingSprites(packName, true);

    public static void TryLoadingSprites(string packName, bool loadSheet)
    {
        if (packName == "Vanilla")
            return;

        var folder = Path.Combine(ModPath, packName);

        if (!Directory.Exists(folder))
        {
            Recolors.LogError($"{packName} was missing", true);
            ModSettings.SetString("Selected Icon Pack", "Vanilla", "alchlcsystm.recolors.windows");
            return;
        }

        try
        {
            if (IconPacks.TryGetValue(packName, out var exists) && exists != null)
                exists.Reload();
            else
            {
                IconPacks.Remove(packName);
                var pack = new IconPack(packName);
                pack.Load(loadSheet);
                IconPacks.Add(packName, pack);
            }
        }
        catch (Exception e)
        {
            Recolors.LogError(e, true);
        }
    }
}