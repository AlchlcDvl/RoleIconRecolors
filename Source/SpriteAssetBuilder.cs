namespace RecolorsPlatformless;

//Courtesy of pat, love ya mate
public static class SpriteAssetBuilder
{
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