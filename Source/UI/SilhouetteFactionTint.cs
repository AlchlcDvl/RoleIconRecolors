using Game.Characters;
using Home.Shared;

namespace FancyUI.UI;

// Vanilla silhouettes are baked in one navy hue, so a plain tint can't reach every faction color. Each renderer's
// texture is swapped (through a MaterialPropertyBlock) for a desaturated copy before tinting.
public static class SilhouetteFactionTint
{
    private static readonly Dictionary<int, Texture2D> GrayscaleTextureCache = [];
    private static readonly HashSet<int> GrayscaleTextureIds = [];
    private static readonly MaterialPropertyBlock PropertyBlock = new();

    internal static readonly Dictionary<int, Material> OriginalRendererMaterials = [];

    // Glow/light props use a different shader than the body and must keep their real material.
    internal static readonly HashSet<int> ExcludedRendererIds = [];

    // Silhouettes where the shader-majority guess for glow props is wrong; tint everything on these.
    private static readonly HashSet<int> ShaderHeuristicExemptSilhouetteIds = 
	[
		(int)Role.CLERIC, 
		(int)Role.MAYOR, 
		(int)Role.SHERIFF, 
		(int)Role.TAVERNKEEPER
	];

    // These are not Silhouettes.
    public static readonly HashSet<int> ExcludedSilhouetteIds =
    [
        (int)Role.HANGMAN,
        (int)Role.FAMINE,
        (int)Role.WAR,
        (int)Role.PESTILENCE,
        (int)Role.DEATH
    ];

    // Change to "_BaseMap" if the grayscale texture doesn't stick.
    private const string TexturePropertyName = "_MainTex";

    public static bool IsExcluded(int silhouetteId) => ExcludedSilhouetteIds.Contains(silhouetteId);

    private static bool TryGetFactionTint(FactionType faction, out Color tint)
    {
        try
        {
            var key = Utils.FactionName(faction, stoned: true).ToUpper();
            var choice = Fancy.SilhouetteColorOptions.TryGetValue(key, out var option) ? option.Value : SilhouetteColorType.Mixed;

            var color = choice switch
            {
                SilhouetteColorType.Primary => Utils.GetFactionStartingColor(faction),
                SilhouetteColorType.Secondary => Utils.GetFactionEndingColor(faction),
                _ => Utils.GetFactionMiddleColor(faction)
            };

            var factor = Mathf.Clamp(Fancy.SilhouetteBrightness.Value, 0f, 300f) / 100f;

            tint = new Color(color.r * factor, color.g * factor, color.b * factor, color.a).linear;
            return true;
        }
        catch
        {
            tint = Color.white;
            return false;
        }
    }

    private static void SnapshotAndClassifyRenderers(Silhouette silhouette)
    {
        var renderers = silhouette.GetComponentsInChildren<SpriteRenderer>();

        if (renderers.Length == 0)
            return;

        var shaderCounts = new Dictionary<Shader, int>();

        foreach (var r in renderers)
        {
            if (!r.sharedMaterial || !r.sharedMaterial.shader)
                continue;

            var id = r.GetInstanceID();

            if (!OriginalRendererMaterials.ContainsKey(id))
                OriginalRendererMaterials[id] = r.sharedMaterial;

            var shader = r.sharedMaterial.shader;
            shaderCounts.TryGetValue(shader, out var count);
            shaderCounts[shader] = count + 1;
        }

        if (shaderCounts.Count <= 1)
            return;

        Shader bodyShader = null;
        var best = -1;

        foreach (var kv in shaderCounts)
        {
            if (kv.Value > best)
            {
                best = kv.Value;
                bodyShader = kv.Key;
            }
        }

        foreach (var r in renderers)
        {
            if (r.sharedMaterial && r.sharedMaterial.shader && r.sharedMaterial.shader != bodyShader)
                ExcludedRendererIds.Add(r.GetInstanceID());
        }
    }

    // Blit + ReadPixels, since the source atlas is probably not readable.
    private static Texture2D GetOrCreateGrayscaleTexture(Texture source)
    {
        var key = source.GetInstanceID();

        if (GrayscaleTextureIds.Contains(key))
            return source as Texture2D;

        if (GrayscaleTextureCache.TryGetValue(key, out var cached) && cached)
            return cached;

        var rt = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGB32);
        var previous = RenderTexture.active;
        var readable = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);

        try
        {
            Graphics.Blit(source, rt);
            RenderTexture.active = rt;
            readable.ReadPixels(new(0f, 0f, source.width, source.height), 0, 0);
            readable.Apply();
        }
        finally
        {
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(rt);
        }

        var pixels = readable.GetPixels32();

        for (var i = 0; i < pixels.Length; i++)
        {
            var p = pixels[i];
            var luminance = (byte)Mathf.RoundToInt((0.2126f * p.r) + (0.7152f * p.g) + (0.0722f * p.b));
            pixels[i] = new(luminance, luminance, luminance, p.a);
        }

        readable.SetPixels32(pixels);
        readable.Apply();

        GrayscaleTextureCache[key] = readable;
        GrayscaleTextureIds.Add(readable.GetInstanceID());
        return readable;
    }

    public static void ApplyGrayscaleTexture(SpriteRenderer renderer)
    {
        if (!renderer || !renderer.sprite || !renderer.sprite.texture)
            return;

        var grayscale = GetOrCreateGrayscaleTexture(renderer.sprite.texture);
        renderer.GetPropertyBlock(PropertyBlock);
        PropertyBlock.SetTexture(TexturePropertyName, grayscale);
        renderer.SetPropertyBlock(PropertyBlock);
    }

    public static void Apply(Silhouette silhouette, FactionType faction)
    {
        if (!Fancy.ColoredSilhouettes.Value || !silhouette || IsExcluded(silhouette.silhouetteId))
            return;

        var component = silhouette.GetComponent<CharacterSpriteMaterial>();

        if (!component || !TryGetFactionTint(faction, out var tint))
            return;

        // Must run before createUniqueMaterialInstance is set, while renderers still hold their real materials.
        if (!ShaderHeuristicExemptSilhouetteIds.Contains(silhouette.silhouetteId))
            SnapshotAndClassifyRenderers(silhouette);

        component.createUniqueMaterialInstance = true;

        if (!silhouette.GetComponent<SilhouetteGrayscaleMaintainer>())
            silhouette.gameObject.AddComponent<SilhouetteGrayscaleMaintainer>();

        component.SetColor(tint);
    }
}

// SpriteRenderer restores its original texture whenever the sprite changes, so the grayscale texture is re-applied every LateUpdate.
public class SilhouetteGrayscaleMaintainer : MonoBehaviour
{
    private SpriteRenderer[] renderers;

    public void Awake() => renderers = GetComponentsInChildren<SpriteRenderer>();

    public void LateUpdate()
    {
        if (renderers == null || renderers.Length == 0)
            renderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (var renderer in renderers)
        {
            if (!renderer)
                continue;

            var id = renderer.GetInstanceID();

            if (SilhouetteFactionTint.ExcludedRendererIds.Contains(id))
            {
                if (SilhouetteFactionTint.OriginalRendererMaterials.TryGetValue(id, out var original) && original && renderer.sharedMaterial != original)
                    renderer.sharedMaterial = original;

                continue;
            }

            SilhouetteFactionTint.ApplyGrayscaleTexture(renderer);
        }
    }
}
