using Home.HomeScene;
using Home.LoginScene;
using SalemModLoaderUI;

namespace FancyUI.Patches;

[HarmonyPatch(typeof(HomeSceneController), nameof(HomeSceneController.Start))]
public static class CacheHomeSceneController
{
    public static HomeSceneController Controller { get; private set; }

    public static void Prefix(HomeSceneController __instance) => Controller = __instance;
}

[HarmonyPatch(typeof(LoginSceneController), nameof(LoginSceneController.Start))]
public static class HandlePacks
{
    public static void Prefix() => DownloaderUI.HandlePackData();
}

[HarmonyPatch(typeof(SalemModLoaderMainMenuController), "ClickMainButton")]
public static class AllowClosingFancyUI
{
    public static void Prefix() => UI.FancyUI.Instance?.gameObject?.Destroy();
}

// Next two patches were provided by Synapsium to re-add the Jailor overlay, thanks man!
[HarmonyPatch(typeof(JailorElementsController), nameof(JailorElementsController.HandleRoleAlteringEffects))]
public static class ReaddJailorOverlay
{
    public static BaseJailorOverlayController JailorOverlayPrefab { get; private set; }

    public static void Postfix(RoleAlteringEffectsState effectsState)
    {
        if (!JailorOverlayPrefab)
            JailorOverlayPrefab = GameObject.Find("Hud/JailorElementsUI(Clone)/MainPanel/JailorOverlay").GetComponent<BaseJailorOverlayController>();

        if (effectsState.bIsJailed && Constants.ShowOverlayWhenJailed())
            JailorOverlayPrefab.Show(); // Show overlay if you are being jailed and the setting is on

        if (effectsState.bIsJailing && Constants.ShowOverlayAsJailor())
            JailorOverlayPrefab.Show(); // Show overlay if you are jailing and the setting is on
    }
}

[HarmonyPatch(typeof(GlobalShaderColors), nameof(GlobalShaderColors.SetToDay))]
public static class RemoveJailorOverlay
{
    public static void Postfix()
    {
        if (ReaddJailorOverlay.JailorOverlayPrefab && ReaddJailorOverlay.JailorOverlayPrefab.IsShowing())
        {
            ReaddJailorOverlay.JailorOverlayPrefab.Close();

            try
            {
                Service.Home.AudioService.PlaySound("Audio/Sfx/JailOpenSound.wav");
            }
            catch (Exception exception)
            {
                Fancy.Instance.Error(exception);
            }
        }
    }
}