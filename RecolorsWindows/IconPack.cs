using TMPro;

namespace RecolorsWindows;

public class IconPack
{
    public Dictionary<string, Sprite> RegIcons { get; set; }
    public Dictionary<string, Sprite> TTIcons { get; set; }
    public Dictionary<string, Sprite> VIPIcons { get; set; }
    public Dictionary<string, List<Sprite>> RegEEIcons { get; set; }
    public Dictionary<string, List<Sprite>> TTEEIcons { get; set; }
    public Dictionary<string, List<Sprite>> VIPEEIcons { get; set; }
    public string Name { get; }
    public TMP_SpriteAsset Asset;
    public bool SpriteSheetLoaded;

    private static readonly Role[] ExceptRoles = { Role.NONE, Role.ROLE_COUNT, Role.UNKNOWN, Role.HANGMAN };

    public IconPack(string name)
    {
        Name = name;
        TTIcons = new();
        RegIcons = new();
        TTEEIcons = new();
        RegEEIcons = new();
        VIPIcons = new();
        VIPEEIcons = new();
    }

    public void Debug()
    {
        RegIcons.ForEach((x, _) => Recolors.LogMessage($"{Name} {x} has a sprite!"));
        Recolors.LogMessage($"{Name} {RegIcons.Count} Assets loaded!");
        TTIcons.ForEach((x, _) => Recolors.LogMessage($"{Name} {x} has a TT sprite!"));
        Recolors.LogMessage($"{Name} {TTIcons.Count} TT Assets loaded!");
        VIPIcons.ForEach((x, _) => Recolors.LogMessage($"{Name} {x} has a VIP sprite!"));
        Recolors.LogMessage($"{Name} {VIPIcons.Count} VIP Assets loaded!");
        RegEEIcons.ForEach((x, y) => Recolors.LogMessage($"{Name} {x} has {y.Count} Easter Egg sprite(s)!"));
        Recolors.LogMessage($"{Name} {RegEEIcons.Count} Easter Egg Assets loaded!");
        TTEEIcons.ForEach((x, y) => Recolors.LogMessage($"{Name} {x} has {y.Count} TT Easter Egg sprite(s)!"));
        Recolors.LogMessage($"{Name} {TTEEIcons.Count} TT Easter Egg Assets loaded!");
        VIPEEIcons.ForEach((x, y) => Recolors.LogMessage($"{Name} {x} has {y.Count} VIP Easter Egg sprite(s)!"));
        Recolors.LogMessage($"{Name} {VIPEEIcons.Count} VIP Easter Egg Assets loaded!");

        if (Asset != null)
            Recolors.LogMessage($"{Name} Sprite Asset exists!");
    }

    public void Reload()
    {
        Recolors.LogMessage($"Reloading {Name}");
        RegIcons.Values.ForEach(UObject.Destroy);
        RegIcons.Clear();
        TTIcons.Values.ForEach(UObject.Destroy);
        TTIcons.Clear();
        VIPIcons.Values.ForEach(UObject.Destroy);
        VIPIcons.Clear();
        RegEEIcons.Values.ForEach(x => x.ForEach(UObject.Destroy));
        RegEEIcons.Values.ForEach(x => x.Clear());
        RegEEIcons.Clear();
        VIPEEIcons.Values.ForEach(x => x.ForEach(UObject.Destroy));
        VIPEEIcons.Values.ForEach(x => x.Clear());
        VIPEEIcons.Clear();
        TTEEIcons.Values.ForEach(x => x.ForEach(UObject.Destroy));
        TTEEIcons.Values.ForEach(x => x.Clear());
        TTEEIcons.Clear();
        UObject.Destroy(Asset);
        SpriteSheetLoaded = false;
        Load(true);
    }

    public static bool operator ==(IconPack a, IconPack b)
    {
        if (a is null && b is null)
            return true;
        else if (a is null || b is null)
            return false;
        else
            return a.Name == b.Name;
    }

    public static bool operator !=(IconPack a, IconPack b) => !(a == b);

    private bool Equals(IconPack other) => Name == other.Name && GetHashCode() == other.GetHashCode();

    public override bool Equals(object obj)
    {
        if (obj is null)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (obj.GetType() != typeof(IconPack))
            return false;

        return Equals((IconPack)obj);
    }

    public override int GetHashCode() => HashCode.Combine(Name);

    public override string ToString() => Name;

    public void Load(bool loadSheet)
    {
        try
        {
            Recolors.LogMessage($"Loading {Name}");
            var folder = Path.Combine(AssetManager.ModPath, Name);
            var baseFolder = Path.Combine(folder, "Base");

            if (Directory.Exists(baseFolder))
            {
                foreach (var file in Directory.EnumerateFiles(baseFolder, "*.png"))
                {
                    var filePath = Path.Combine(baseFolder, $"{file.SanitisePath()}.png");
                    var sprite = AssetManager.LoadSpriteFromDisk(filePath, "Base", Name);
                    filePath = filePath.SanitisePath(true);
                    RegIcons.TryAdd(filePath, sprite);
                }
            }
            else
            {
                Recolors.LogWarning($"{Name} Base folder doesn't exist");
                Directory.CreateDirectory(baseFolder);
            }

            var ttFolder = Path.Combine(folder, "TTBase");

            if (Directory.Exists(ttFolder))
            {
                foreach (var file in Directory.EnumerateFiles(ttFolder, "*.png"))
                {
                    var filePath = Path.Combine(ttFolder, $"{file.SanitisePath()}.png");
                    var sprite = AssetManager.LoadSpriteFromDisk(filePath, "TTBase", Name);
                    filePath = filePath.SanitisePath(true);
                    TTIcons.TryAdd(filePath, sprite);
                }
            }
            else
            {
                Recolors.LogWarning($"{Name} TTBase folder doesn't exist");
                Directory.CreateDirectory(ttFolder);
            }

            var vipFolder = Path.Combine(folder, "VIPBase");

            if (Directory.Exists(vipFolder))
            {
                foreach (var file in Directory.EnumerateFiles(vipFolder, "*.png"))
                {
                    var filePath = Path.Combine(vipFolder, $"{file.SanitisePath()}.png");
                    var sprite = AssetManager.LoadSpriteFromDisk(filePath, "VIPBase", Name);
                    filePath = filePath.SanitisePath(true);
                    VIPIcons.TryAdd(filePath, sprite);
                }
            }
            else
            {
                Recolors.LogWarning($"{Name} VIPBase folder doesn't exist");
                Directory.CreateDirectory(vipFolder);
            }

            var eeFolder = Path.Combine(folder, "EasterEggs");

            if (Directory.Exists(eeFolder))
            {
                foreach (var file in Directory.EnumerateFiles(eeFolder, "*.png"))
                {
                    var filePath = Path.Combine(eeFolder, $"{file.SanitisePath()}.png");
                    var sprite = AssetManager.LoadSpriteFromDisk(filePath, "EasterEggs", Name);
                    filePath = filePath.SanitisePath(true);

                    if (RegEEIcons.ContainsKey(filePath))
                        RegEEIcons[filePath].Add(sprite);
                    else
                        RegEEIcons.TryAdd(filePath, new() { sprite });

                    if (AssetManager.RegEEIcons.ContainsKey(filePath))
                        AssetManager.RegEEIcons[filePath].Add(sprite);
                    else
                        AssetManager.RegEEIcons.TryAdd(filePath, new() { sprite });
                }
            }
            else
            {
                Recolors.LogWarning($"{Name} EasterEggs folder doesn't exist");
                Directory.CreateDirectory(eeFolder);
            }

            var tteeFolder = Path.Combine(folder, "TTEasterEggs");

            if (Directory.Exists(tteeFolder))
            {
                foreach (var file in Directory.EnumerateFiles(tteeFolder, "*.png"))
                {
                    var filePath = Path.Combine(tteeFolder, $"{file.SanitisePath()}.png");
                    var sprite = AssetManager.LoadSpriteFromDisk(filePath, "TTEasterEggs", Name);
                    filePath = filePath.SanitisePath(true);

                    if (TTEEIcons.ContainsKey(filePath))
                        TTEEIcons[filePath].Add(sprite);
                    else
                        TTEEIcons.TryAdd(filePath, new() { sprite });

                    if (AssetManager.TTEEIcons.ContainsKey(filePath))
                        AssetManager.TTEEIcons[filePath].Add(sprite);
                    else
                        AssetManager.TTEEIcons.TryAdd(filePath, new() { sprite });
                }
            }
            else
            {
                Recolors.LogWarning($"{Name} TTEasterEggs folder doesn't exist");
                Directory.CreateDirectory(tteeFolder);
            }

            var vipeeFolder = Path.Combine(folder, "VIPEasterEggs");

            if (Directory.Exists(vipeeFolder))
            {
                foreach (var file in Directory.EnumerateFiles(vipeeFolder, "*.png"))
                {
                    var filePath = Path.Combine(vipeeFolder, $"{file.SanitisePath()}.png");
                    var sprite = AssetManager.LoadSpriteFromDisk(filePath, "VIPEasterEggs", Name);
                    filePath = filePath.SanitisePath(true);

                    if (VIPEEIcons.ContainsKey(filePath))
                        VIPEEIcons[filePath].Add(sprite);
                    else
                        VIPEEIcons.TryAdd(filePath, new() { sprite });

                    if (AssetManager.VIPEEIcons.ContainsKey(filePath))
                        AssetManager.VIPEEIcons[filePath].Add(sprite);
                    else
                        AssetManager.VIPEEIcons.TryAdd(filePath, new() { sprite });
                }
            }
            else
            {
                Recolors.LogWarning($"{Name} VIPEasterEggs folder doesn't exist");
                Directory.CreateDirectory(vipeeFolder);
            }

            if (loadSheet)
                LoadSpriteSheet();

            Debug();
        }
        catch (Exception e)
        {
            Recolors.LogError(e, true);
        }
    }

    // love ya pat
    public void LoadSpriteSheet()
    {
        try
        {
            if (SpriteSheetLoaded || Asset != null)
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

                if (!RegIcons.TryGetValue(Utils.RoleName(actualRole), out var sprite))
                {
                    sprite = actualRole switch
                    {
                        Role.VAMPIRE => AssetManager.Vampire,
                        Role.CURSED_SOUL => AssetManager.CursedSoul,
                        _ => Service.Game.Roles.roleInfoLookup[actualRole].sprite
                    };
                }

                sprite.texture.name = role;
                textures.Add(sprite.texture);
            }

            var assetBuilder = new SpriteAssetBuilder(256, 256, 10);
            Asset = assetBuilder.BuildGlyphs(textures.ToArray(), $"RoleIcons ({Name})", x => x.name = rolesWithIndexDict[(x.glyph as TMP_SpriteGlyph).sprite.name.ToLower()]);
            // set spritecharacter name to "Role{number}" so that the game can find correct roles
            Recolors.LogMessage($"{Name} Sprite Asset loaded!");
        }
        catch (Exception e)
        {
            Recolors.LogError(e, true);
            Asset = null;
            SpriteSheetLoaded = false;
        }
    }
}