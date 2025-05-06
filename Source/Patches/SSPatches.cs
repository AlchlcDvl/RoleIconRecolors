// using Art;
// using UnityEngine.U2D.Animation;

// namespace FancyUI.Patches;

// [HarmonyPatch(typeof(RolecardBackground))]
// public static class SwapSilhouettesPatch
// {
//     [HarmonyPatch(nameof(RolecardBackground.SetRole))]
//     public static void Postfix(RolecardBackground __instance)
//     {
//         var animatorTransform = __instance.silhouette.transform.parent.FindRecursive("SilhouetteAnimRend");
//         var anim = animatorTransform?.GetComponent<CustomRendAnimator>();

//         if (!anim)
//         {
//             var animRend = UObject.Instantiate(__instance.silhouette.GetComponentInChildren<SpriteRenderer>(true), __instance.silhouette.transform.parent);
//             animRend.name = "SilhouetteAnimRend";
//             animRend.GetComponent<SpriteSkin>().Destroy();
//             animRend.transform.localPosition = new(-0.4f, 6.6f, 0f);
//             animRend.transform.localScale = new(0.65f, 0.65f, 1f);
//             anim = animRend.AddComponent<CustomRendAnimator>();

//             var shadow = UObject.Instantiate(__instance.silhouette.transform.GetComponent<SpriteRenderer>("Shadow"), animRend.transform);
//             shadow.name = "Shadow";
//             shadow.transform.localPosition = new(-0.8936f, -8.9f, 0f);
//             shadow.transform.localScale = new(6f, 2f, 1f);
//         }

//         anim.SetAnim(Loading.Frames, Fancy.AnimationDuration.Value);
//         __instance.silhouette.gameObject.SetActive(false);
//     }
// }

// Above code works, below is supposed to "fix" the above code but it doesn't lol

// [HarmonyPatch(typeof(RolecardBackground))]
// public static class SwapSilhouettesPatches
// {
//     private static SpriteRenderer Prefab;

//     [HarmonyPatch(nameof(RolecardBackground.Start))]
//     public static void Postfix()
//     {
//         Fancy.Instance.Error("HI");
//         Prefab ??= Service.Game.Cast.GetSilhouette(42).GetComponentInChildren<SpriteRenderer>(true);
//     }

//     [HarmonyPatch(nameof(RolecardBackground.SetRole))]
//     public static void Postfix(RolecardBackground __instance)
//     {
//         Fancy.Instance.Error("HI");
//         var animatorTransform = __instance.silhouette.transform.parent.Find("SilhouetteAnimRend");
//         CustomRendAnimator anim;

//         if (!animatorTransform)
//         {
//             var animRend = UObject.Instantiate(Prefab, __instance.silhouette.transform.parent);
//             animRend.name = "SilhouetteAnimRend";
//             animRend.GetComponent<SpriteSkin>().Destroy();
//             animRend.transform.localPosition = new(-0.4f, 6.6f, 0f);
//             animRend.transform.localScale = new(0.65f, 0.65f, 1f);
//             animRend.enabled = true;

//             var shadow = UObject.Instantiate(__instance.silhouette.transform.GetComponent<SpriteRenderer>("Shadow"), animRend.transform);
//             shadow.name = "Shadow";
//             shadow.transform.localPosition = new(-0.8936f, -8.9f, 0f);
//             shadow.transform.localScale = new(6f, 2f, 1f);

//             anim = animRend.EnsureComponent<CustomRendAnimator>();
//         }
//         else
//             anim = animatorTransform.EnsureComponent<CustomRendAnimator>();

//         anim.SetAnim(Loading.Frames, Fancy.AnimationDuration.Value);
//         __instance.silhouette.gameObject.SetActive(false);
//     }
// }