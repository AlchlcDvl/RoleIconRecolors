using Art;
using NewModLoading;
using UnityEngine.U2D.Animation;

namespace FancyUI.Patches;

[HarmonyPatch(typeof(RolecardBackground), nameof(RolecardBackground.SetRole))]
public static class SwapSilhouettesPatches
{
    private static SpriteRenderer prefab;
    private static SpriteRenderer Prefab
    {
        get
        {
            if (prefab)
                return prefab;

            // This is a bit of a hack, but it works. We need to get the prefab from the game, but we don't want to use the one in the game.
            try
            {
                var sil = UObject.Instantiate(Service.Game.Cast.GetSilhouette(42), null).DontDestroy(); // 42 = Berserker, the one that actually works lmao
                sil.transform.SetLayerRecursive(16, [Layers.RimLightingLayer]);
                sil.gameObject.SetActive(false);
                prefab = UObject.Instantiate(sil.GetComponentInChildren<SpriteRenderer>(true), null).DontDestroy();
                sil.gameObject.Destroy();
                return prefab;
            }
            catch
            {
                return prefab = null; // Retry as many times as needed
            }
        }
    }

    public static void Postfix(RolecardBackground __instance)
    {
        if (!Constants.EnableSwaps())
            return;

        var animatorTransform = __instance.silhouette.transform.parent.Find("SilhouetteAnimRend");
        var anim = animatorTransform?.GetComponent<CustomRendAnimator>();

        if (!anim)
        {
            var animRend = UObject.Instantiate(Prefab, __instance.silhouette.transform.parent);
            animRend.name = "SilhouetteAnimRend";
            animRend.GetComponent<SpriteSkin>().Destroy();
            animRend.transform.localPosition = new(-0.4f, 6.6f, 0f);
            animRend.transform.localScale = new(0.65f, 0.65f, 1f);

            var shadow = UObject.Instantiate(__instance.silhouette.transform.GetComponent<SpriteRenderer>("Shadow"), animRend.transform);
            shadow.name = "Shadow";
            shadow.transform.localPosition = new(-0.8936f, -8.9f, 0f);
            shadow.transform.localScale = new(6f, 2f, 1f);

            anim = animRend.EnsureComponent<CustomRendAnimator>();
        }

        anim!.SetAnim(Loading.Frames, Fancy.AnimationDuration.Value);
        anim.gameObject.SetActive(true);
        __instance.silhouette.gameObject.SetActive(false);
    }
}