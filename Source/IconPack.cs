namespace IconPacks;

public class IconPack
{
    public Dictionary<string, Sprite> RegIcons { get; set; }
    public Dictionary<string, Sprite> TTIcons { get; set; }
    public Dictionary<string, Sprite> VIPIcons { get; set; }

    public Dictionary<string, List<Sprite>> RegEEIcons { get; set; }
    public Dictionary<string, List<Sprite>> TTEEIcons { get; set; }
    public Dictionary<string, List<Sprite>> VIPEEIcons { get; set; }

    public Dictionary<string, Sprite> CustomIcons { get; set; }

    public Dictionary<string, TMP_SpriteAsset> MentionStyles { get; set; }

    public Dictionary<string, List<Sprite>> RegAnimations { get; set; }
    public Dictionary<string, List<Sprite>> TTAnimations { get; set; }
    public Dictionary<string, List<Sprite>> VIPAnimations { get; set; }

    public Dictionary<string, List<List<Sprite>>> RegEEAnimations { get; set; }
    public Dictionary<string, List<List<Sprite>>> TTEEAnimations { get; set; }
    public Dictionary<string, List<List<Sprite>>> VIPEEAnimations { get; set; }

    public string Name { get; }

    private string PackPath => Path.Combine(AssetManager.ModPath, Name);

    private static readonly string[] Folders = new[] { "", "TT", "VIP", "Custom" };
    private static readonly string[] Styles = new[] { "Main", "Traitor", "VIP", "Custom" };
    public static readonly string[] FileTypes = new[] { "png", "jpg" };

    public IconPack(string name)
    {
        Name = name;
        RegIcons = new();
        RegEEIcons = new();
        TTIcons = new();
        TTEEIcons = new();
        VIPIcons = new();
        VIPEEIcons = new();
        RegAnimations = new();
        RegEEAnimations = new();
        TTAnimations = new();
        TTEEAnimations = new();
        VIPAnimations = new();
        VIPEEAnimations = new();
        CustomIcons = new();
        MentionStyles = new();
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

        CustomIcons.ForEach((x, _) => Recolors.LogMessage($"{Name} {x} has a Custom Mention sprite!"));
        Recolors.LogMessage($"{Name} {CustomIcons.Count} Custom Assets loaded!");

        MentionStyles.ForEach((x, _) => Recolors.LogMessage($"{Name} {x} mention style exists!"));
        Recolors.LogMessage($"{Name} {MentionStyles.Count} mention styles exist!");

        RegAnimations.ForEach((x, y) => Recolors.LogMessage($"{Name} {x} has an animation with {y.Count} frames!"));
        Recolors.LogMessage($"{Name} {RegAnimations.Count} Animations loaded!");
        TTAnimations.ForEach((x, y) => Recolors.LogMessage($"{Name} {x} has a TT animation with {y.Count} frames!"));
        Recolors.LogMessage($"{Name} {TTAnimations.Count} TT Animations loaded!");
        VIPAnimations.ForEach((x, y) => Recolors.LogMessage($"{Name} {x} has a VIP animation with {y.Count} frames!"));
        Recolors.LogMessage($"{Name} {VIPAnimations.Count} VIP Animations loaded!");

        RegEEAnimations.ForEach((x, y) => Recolors.LogMessage($"{Name} {x} has {y.Count} Easter Egg animation(s)!"));
        Recolors.LogMessage($"{Name} {RegEEAnimations.Count} Easter Egg Animations loaded!");
        TTEEAnimations.ForEach((x, y) => Recolors.LogMessage($"{Name} {x} has {y.Count} TT Easter Egg animation(s)!"));
        Recolors.LogMessage($"{Name} {TTEEAnimations.Count} TT Easter Egg Animations loaded!");
        VIPEEAnimations.ForEach((x, y) => Recolors.LogMessage($"{Name} {x} has {y.Count} VIP Easter Egg animation(s)!"));
        Recolors.LogMessage($"{Name} {VIPEEAnimations.Count} VIP Easter Egg Animations loaded!");
    }

    public void Delete()
    {
        Recolors.LogMessage($"Deleteing {Name}");
        RegIcons.Values.ForEach(UObject.Destroy);
        RegIcons.Clear();
        TTIcons.Values.ForEach(UObject.Destroy);
        TTIcons.Clear();
        VIPIcons.Values.ForEach(UObject.Destroy);
        VIPIcons.Clear();

        CustomIcons.Values.ForEach(UObject.Destroy);
        CustomIcons.Clear();

        RegEEIcons.Values.ForEach(x => x.ForEach(UObject.Destroy));
        RegEEIcons.Values.ForEach(x => x.Clear());
        RegEEIcons.Clear();
        VIPEEIcons.Values.ForEach(x => x.ForEach(UObject.Destroy));
        VIPEEIcons.Values.ForEach(x => x.Clear());
        VIPEEIcons.Clear();
        TTEEIcons.Values.ForEach(x => x.ForEach(UObject.Destroy));
        TTEEIcons.Values.ForEach(x => x.Clear());
        TTEEIcons.Clear();

        RegAnimations.Values.ForEach(x => x.ForEach(UObject.Destroy));
        RegAnimations.Values.ForEach(x => x.Clear());
        RegAnimations.Clear();
        VIPAnimations.Values.ForEach(x => x.ForEach(UObject.Destroy));
        VIPAnimations.Values.ForEach(x => x.Clear());
        VIPAnimations.Clear();
        TTAnimations.Values.ForEach(x => x.ForEach(UObject.Destroy));
        TTAnimations.Values.ForEach(x => x.Clear());
        TTAnimations.Clear();

        RegEEAnimations.Values.ForEach(x => x.ForEach(y => y.ForEach(UObject.Destroy)));
        RegEEAnimations.Values.ForEach(x => x.ForEach(y => y.Clear()));
        RegEEAnimations.Values.ForEach(x => x.Clear());
        RegEEAnimations.Clear();
        VIPEEAnimations.Values.ForEach(x => x.ForEach(y => y.ForEach(UObject.Destroy)));
        VIPEEAnimations.Values.ForEach(x => x.ForEach(y => y.Clear()));
        VIPEEAnimations.Values.ForEach(x => x.Clear());
        VIPEEAnimations.Clear();
        TTEEAnimations.Values.ForEach(x => x.ForEach(y => y.ForEach(UObject.Destroy)));
        TTEEAnimations.Values.ForEach(x => x.ForEach(y => y.Clear()));
        TTEEAnimations.Values.ForEach(x => x.Clear());
        TTEEAnimations.Clear();

        MentionStyles.Values.ForEach(UObject.Destroy);
        MentionStyles.Clear();
    }

    public void Reload()
    {
        Recolors.LogMessage($"Reloading {Name}");
        Delete();
        Load();
        LoadSpriteSheet(true);
        AssetManager.SetScrollSprites();
    }

    public void Load()
    {
        try
        {
            Recolors.LogMessage($"Loading {Name}");

            foreach (var name in Folders)
            {
                if (name != "Custom")
                {
                    var baseName = name + "Base";
                    var baseFolder = Path.Combine(PackPath, baseName);

                    if (Directory.Exists(baseFolder))
                    {
                        foreach (var type in FileTypes)
                        {
                            foreach (var file in Directory.GetFiles(baseFolder, $"*.{type}"))
                            {
                                var filePath = Path.Combine(baseFolder, $"{file.SanitisePath()}.{type}");
                                var sprite = AssetManager.LoadDiskSprite(filePath.SanitisePath(), baseName, Name, type);
                                filePath = filePath.SanitisePath(true);

                                if (sprite == null)
                                    continue;

                                if (name == "")
                                    RegIcons.TryAdd(filePath, sprite);
                                else if (name == "TT")
                                    TTIcons.TryAdd(filePath, sprite);
                                else if (name == "VIP")
                                    VIPIcons.TryAdd(filePath, sprite);
                            }
                        }

                        foreach (var folder in Directory.GetDirectories(baseFolder))
                        {
                            var filePath = Path.Combine(baseFolder, folder.SanitisePath());
                            var sprites = AssetManager.LoadDiskSprites(filePath.SanitisePath(), baseName, Name);
                            filePath = filePath.SanitisePath(true);

                            if (sprites == null)
                                continue;

                            if (name == "")
                                RegAnimations.TryAdd(filePath, sprites);
                            else if (name == "TT")
                                TTAnimations.TryAdd(filePath, sprites);
                            else if (name == "VIP")
                                VIPAnimations.TryAdd(filePath, sprites);
                        }
                    }
                    else
                    {
                        Recolors.LogWarning($"{Name} {baseName} folder doesn't exist");
                        Directory.CreateDirectory(baseFolder);
                    }

                    var eeName = name + "EasterEggs";
                    var eeFolder = Path.Combine(PackPath, eeName);

                    if (Directory.Exists(eeFolder))
                    {
                        foreach (var type in FileTypes)
                        {
                            foreach (var file in Directory.GetFiles(baseFolder, $"*.{type}"))
                            {
                                var filePath = Path.Combine(baseFolder, $"{file.SanitisePath()}.{type}");
                                var sprite = AssetManager.LoadDiskSprite(filePath.SanitisePath(), baseName, Name, type);
                                filePath = filePath.SanitisePath(true);

                                if (sprite == null)
                                    continue;

                                if (name == "")
                                {
                                    if (RegEEIcons.ContainsKey(filePath))
                                        RegEEIcons[filePath].Add(sprite);
                                    else
                                        RegEEIcons.TryAdd(filePath, new() { sprite });

                                    if (AssetManager.RegEEIcons.ContainsKey(filePath))
                                        AssetManager.RegEEIcons[filePath].Add(sprite);
                                    else
                                        AssetManager.RegEEIcons.TryAdd(filePath, new() { sprite });
                                }
                                else if (name == "TT")
                                {
                                    if (TTEEIcons.ContainsKey(filePath))
                                        TTEEIcons[filePath].Add(sprite);
                                    else
                                        TTEEIcons.TryAdd(filePath, new() { sprite });

                                    if (AssetManager.TTEEIcons.ContainsKey(filePath))
                                        AssetManager.TTEEIcons[filePath].Add(sprite);
                                    else
                                        AssetManager.TTEEIcons.TryAdd(filePath, new() { sprite });
                                }
                                else if (name == "VIP")
                                {
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
                        }

                        foreach (var folder in Directory.EnumerateDirectories(eeFolder))
                        {
                            var filePath = Path.Combine(baseFolder, folder.SanitisePath());
                            var sprites = AssetManager.LoadDiskSprites(filePath, baseName, Name);
                            filePath = filePath.SanitisePath(true);

                            if (sprites == null)
                                continue;

                            if (name == "")
                            {
                                if (RegEEAnimations.ContainsKey(filePath))
                                    RegEEAnimations[filePath].Add(sprites);
                                else
                                    RegEEAnimations.TryAdd(filePath, new() { sprites });

                                if (AssetManager.RegEEAnimations.ContainsKey(filePath))
                                    AssetManager.RegEEAnimations[filePath].Add(sprites);
                                else
                                    AssetManager.RegEEAnimations.TryAdd(filePath, new() { sprites });
                            }
                            else if (name == "TT")
                            {
                                if (TTEEAnimations.ContainsKey(filePath))
                                    TTEEAnimations[filePath].Add(sprites);
                                else
                                    TTEEAnimations.TryAdd(filePath, new() { sprites });

                                if (AssetManager.TTEEAnimations.ContainsKey(filePath))
                                    AssetManager.TTEEAnimations[filePath].Add(sprites);
                                else
                                    AssetManager.TTEEAnimations.TryAdd(filePath, new() { sprites });
                            }
                            else if (name == "VIP")
                            {
                                if (VIPEEAnimations.ContainsKey(filePath))
                                    VIPEEAnimations[filePath].Add(sprites);
                                else
                                    VIPEEAnimations.TryAdd(filePath, new() { sprites });

                                if (AssetManager.VIPEEAnimations.ContainsKey(filePath))
                                    AssetManager.VIPEEAnimations[filePath].Add(sprites);
                                else
                                    AssetManager.VIPEEAnimations.TryAdd(filePath, new() { sprites });
                            }
                        }
                    }
                    else
                    {
                        Recolors.LogWarning($"{Name} {eeName} folder doesn't exist");
                        Directory.CreateDirectory(eeFolder);
                    }
                }
                else
                {
                    var folder = Path.Combine(PackPath, "Custom");

                    if (Directory.Exists(folder))
                    {
                        foreach (var type in FileTypes)
                        {
                            foreach (var file in Directory.GetFiles(folder, $"*.{type}"))
                            {
                                var filePath = Path.Combine(folder, $"{file.SanitisePath()}.{type}");
                                var sprite = AssetManager.LoadDiskSprite(filePath.SanitisePath(), "Custom", Name, type);
                                filePath = filePath.SanitisePath(true);

                                if (sprite == null)
                                    continue;

                                if (name == "")
                                    RegIcons.TryAdd(filePath, sprite);
                                else if (name == "TT")
                                    TTIcons.TryAdd(filePath, sprite);
                                else if (name == "VIP")
                                    VIPIcons.TryAdd(filePath, sprite);
                            }
                        }
                    }
                    else
                    {
                        Recolors.LogWarning($"{Name} Custom folder doesn't exist");
                        Directory.CreateDirectory(folder);
                    }
                }
            }

            Debug();
        }
        catch (Exception e)
        {
            Recolors.LogError(e);
        }
    }

    // love ya pat
    public void LoadSpriteSheet(bool change)
    {
        try
        {
            var (rolesWithIndexDict, rolesWithIndex) = Utils.Filtered();

            foreach (var style in Styles)
            {
                if (MentionStyles.ContainsKey(style))
                    continue;

                var textures = new List<Texture2D>();
                var spritesDict = style switch
                {
                    "Traitor" => TTIcons,
                    "VIP" => VIPIcons,
                    "Custom" => CustomIcons,
                    _ => RegIcons,
                };
                var animationsDict = style switch
                {
                    "Traitor" => TTAnimations,
                    "VIP" => VIPAnimations,
                    "Custom" => new(),
                    _ => RegAnimations,
                };

                if (style == "Custom")
                    CustomIcons.ForEach((x, y) => animationsDict.Add(x, new() { y }));

                // now get all the sprites that we want to load
                foreach (var (role, roleInt) in rolesWithIndex)
                {
                    var actualRole = (Role)roleInt;

                    if (!spritesDict.TryGetValue(Utils.RoleName(actualRole), out var sprite))
                    {
                        if (animationsDict.TryGetValue(Utils.RoleName(actualRole), out var sprites))
                            sprite = sprites[0];
                        else
                        {
                            if (!RegIcons.TryGetValue(Utils.RoleName(actualRole), out sprite))
                                sprite = Service.Game.Roles.roleInfoLookup[actualRole].sprite;
                        }
                    }

                    sprite.texture.name = role;
                    textures.Add(sprite.texture);
                }

                // set spritecharacter name to "Role{number}" so that the game can find correct roles
                var asset = AssetManager.BuildGlyphs(textures.ToArray(), $"RoleIcons ({Name}, {style})", x => x.name = rolesWithIndexDict[(x.glyph as TMP_SpriteGlyph).sprite.name.ToLower()]);
                MentionStyles[style] = asset;
                Recolors.LogMessage($"{Name} {style} Sprite Asset loaded!");

                var assetPath = Path.Combine(PackPath, $"{Name.Replace(" ", "")}_{style}RoleIcons.png");

                if (File.Exists(assetPath))
                    File.Delete(assetPath);

                File.WriteAllBytes(assetPath, (asset.spriteSheet as Texture2D).Decompress().EncodeToPNG());
            }

            if (change)
            {
                AssetManager.ChangeSpriteSheets(Name);
                Recolors.LogMessage($"Changed to {Name} {Constants.CurrentStyle} Sprite Asset!");
            }
        }
        catch (Exception e)
        {
            Recolors.LogError(e);
        }
    }
}