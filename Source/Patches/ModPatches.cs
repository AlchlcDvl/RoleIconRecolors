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

[HarmonyPatch(typeof(SalemModLoaderMainMenuController), "Start")]
public static class AllowClosingFancyUI
{
    public static void Postfix(SalemModLoaderMainMenuController __instance) => __instance.transform.GetComponent<Button>("Button").onClick.AddListener(() =>
        UI.FancyUI.Instance?.gameObject?.Destroy());
}