using TMPro;
using UnityEngine.TextCore;

namespace RecolorsWindows;

//Courtesy of pat, love ya mate
public class SpriteAssetBuilder
{
    public int GlyphWidth { get; set; }
    public int GlyphHeight { get; set; }
    public int GlyphsInRow { get; set; }
    public TMP_SpriteAsset Asset;

    public SpriteAssetBuilder(int glyphWidth, int glyphHeight, int glyphsInRow)
    {
        GlyphWidth = glyphWidth;
        GlyphHeight = glyphHeight;
        GlyphsInRow = glyphsInRow;
        Asset = ScriptableObject.CreateInstance<TMP_SpriteAsset>();
    }

    public void BuildGlyphs(Texture2D[] textures, string spriteAssetName, Action<TMP_SpriteCharacter> action)
    {
        var rowNumber = (textures.Length - 1) / GlyphsInRow + 1;
        var completeWidth = GlyphsInRow * GlyphWidth;
        var completeHeight = GlyphHeight * rowNumber;
        var image = new Texture2D(completeWidth, completeHeight) { name = spriteAssetName };

        var yIndex = rowNumber - 1;
        var xIndex = 0;

        Asset?.spriteGlyphTable.Clear();
        Asset?.spriteCharacterTable.Clear();

        uint index = 0;

        foreach (var texture in textures)
        {
            image.SetPixels(xIndex * GlyphWidth, yIndex * GlyphHeight, GlyphWidth, GlyphHeight, texture.GetPixels());

            BuildGlyph(texture, xIndex, yIndex, index, action);

            index++;
            xIndex++;

            if (xIndex >= GlyphsInRow)
            {
                xIndex = 0;
                yIndex--;
            }
        }

        image.Apply();

        Asset.name = spriteAssetName;
        Asset.material = UObject.Instantiate(TMP_Settings.defaultSpriteAsset.material);
        Asset.version = "1.1.0";
        Asset.material.mainTexture = image;
        Asset.spriteSheet = image;
        Asset.UpdateLookupTables();
    }

    internal Sprite BuildGlyph(Texture2D texture, int xIndex, int yIndex, uint index, Action<TMP_SpriteCharacter> action)
    {
        var sprite = Sprite.Create(texture, new(0, 0, texture.width, texture.height), new(0, 0), 100);
        sprite.name = texture.name;

        var glyph = new TMP_SpriteGlyph()
        {
            glyphRect = new GlyphRect()
            {
                width = GlyphWidth,
                height = GlyphHeight - 1,
                x = xIndex * GlyphWidth,
                y = yIndex * GlyphHeight
            },
            metrics = new GlyphMetrics()
            {
                width = GlyphWidth,
                height = GlyphHeight - 1,
                horizontalBearingY = GlyphHeight * 0.75f,
                horizontalBearingX = 0,
                horizontalAdvance = GlyphWidth
            },
            index = index,
            sprite = sprite,
        };

        var character = new TMP_SpriteCharacter(0, glyph)
        {
            name = texture.name,
            glyphIndex = index,
        };

        Asset?.spriteGlyphTable.Add(glyph);
        Asset?.spriteCharacterTable.Add(character);

        action(character);
        return sprite;
    }

    public static TMP_SpriteAsset GetGlyphs(TMP_SpriteAsset asset, int glyphsWidth, int glyphsHeight, int glyphsInRow, Action<Sprite> action = null)
    {
        // if we already have spritecharacters, just generate glyphs
        var rowNumber = (asset.spriteCharacterTable.Count - 1) / glyphsInRow + 1;

        var image = (Texture2D)asset.spriteSheet;
        asset.material = UObject.Instantiate(TMP_Settings.defaultSpriteAsset.material);
        asset.material.mainTexture = image;

        var yIndex = rowNumber - 1;
        var xIndex = 0;

        asset?.spriteGlyphTable.Clear();

        foreach (var character in asset.spriteCharacterTable)
        {
            var pixels = image.GetPixels(xIndex * glyphsWidth, yIndex * glyphsHeight, glyphsWidth, glyphsHeight);
            var texture = new Texture2D(glyphsWidth, glyphsHeight);
            texture.SetPixels(pixels);
            texture.Apply();

            var sprite = Sprite.Create(texture, new(0f, 0f, texture.width, texture.height), new(0f, 0f));

            texture.name = character.name;
            sprite.name = character.name;

            action?.Invoke(sprite);

            var glyph = new TMP_SpriteGlyph()
            {
                glyphRect = new GlyphRect()
                {
                    width = glyphsWidth,
                    height = glyphsHeight - 1,
                    x = xIndex * glyphsWidth,
                    y = yIndex * glyphsHeight
                },
                metrics = new GlyphMetrics()
                {
                    width = glyphsWidth,
                    height = glyphsHeight - 1,
                    horizontalBearingY = glyphsHeight * 0.75f,
                    horizontalBearingX = 0,
                    horizontalAdvance = glyphsWidth
                },
                index = character.glyphIndex,
                sprite = sprite,
            };

            character.glyph = glyph;
            asset.spriteGlyphTable.Add(glyph);

            xIndex++;

            if (xIndex >= glyphsInRow)
            {
                xIndex = 0;
                yIndex--;
            }
        }

        asset.UpdateLookupTables();
        return asset;
    }
}