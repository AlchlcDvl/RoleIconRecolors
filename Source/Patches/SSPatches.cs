// using Art;

// namespace FancyUI.Patches;

// [HarmonyPatch(typeof(RolecardBackground))]
// public static class SwapSilhouettesPatch
// {
//     [HarmonyPatch(nameof(RolecardBackground.SetRole))]
//     public static void Postfix(RolecardBackground __instance)
//     {
//         __instance.silhouette.gameObject.SetActive(false);
//         var animatorTransform = __instance.transform.FindRecursive("SilhouetteAnimRend");

//         if (!animatorTransform)
//         {
//             var animRend = UObject.Instantiate(__instance.silhouette.GetComponentInChildren<SpriteRenderer>(true), __instance.silhouette.transform.parent);
//             var anim = animRend.AddComponent<CustomRendAnimator>();
//             anim.SetAnim(Loading.Frames, Constants.AnimationDuration());
//         }
//     }
// }