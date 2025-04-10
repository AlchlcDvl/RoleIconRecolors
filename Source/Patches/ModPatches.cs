using Home.HomeScene;
using Home.LoginScene;
using SalemModLoaderUI;
using Home.Shared;
using Server.Shared.Extensions;
using Cinematics.Players;
using Server.Shared.Cinematics.Data;
using Server.Shared.Cinematics;


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

// The next two patches were provided by Synapsium to re-add the Jailor overlay, thanks man!
[HarmonyPatch(typeof(JailorElementsController), nameof(JailorElementsController.HandleRoleAlteringEffects))]
public static class ReAddJailorOverlay
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
        if (!ReAddJailorOverlay.JailorOverlayPrefab || !ReAddJailorOverlay.JailorOverlayPrefab.IsShowing())
            return;

        ReAddJailorOverlay.JailorOverlayPrefab.Close();

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

// This patches the default win screens (used by modded factions).
[HarmonyPatch(typeof(FactionWinsCinematicPlayer), nameof(FactionWinsCinematicPlayer.Init))]
public static class PatchDefaultWinScreens
{
    public static bool Prefix(FactionWinsCinematicPlayer __instance, ICinematicData cinematicData)
    {
        __instance.elapsedDuration = 0f;
        Debug.Log(string.Format("FactionWinsCinematicPlayer current phase at start = {0}", Pepper.GetGamePhase()));
        __instance.cinematicData = cinematicData as FactionWinsCinematicData;
        __instance.totalDuration = CinematicFactionWinsTimes.GetWinTimeByFaction(__instance.cinematicData.winningFaction);
        __instance.callbackTimers.Clear();
        var spawnedCharacters = Service.Game.Cast.GetSpawnedCharacters();

        if (spawnedCharacters == null)
        {
            Debug.LogError("spawnedPlayers is null in GetCrowd()");
            return false;
        }

        var positions = new HashSet<int>();
        __instance.cinematicData.entries.ForEach(e => positions.Add(e.position));
        spawnedCharacters.ForEach(c =>
        {
            if (positions.Contains(c.position))
                __instance.winningCharacters.Add(c);
            else
                c.characterSprite.SetColor(Color.clear);
        });
        var winningFaction = __instance.cinematicData.winningFaction;

        // Audio and prop changes based on winning faction
        if (winningFaction == FactionType.TOWN)
        {
            Service.Home.AudioService.PlayMusic("Audio/Music/TownVictory.wav", false, AudioController.AudioChannel.Cinematic, true);
            __instance.evilProp.SetActive(false);
            __instance.goodProp.SetActive(true);
            __instance.m_Animator.SetInteger("State", 1);
        }
        else
        {
            Service.Home.AudioService.PlayMusic("Audio/Music/CovenVictory.wav", false, AudioController.AudioChannel.Cinematic, true);
            __instance.evilProp.SetActive(true);
            __instance.goodProp.SetActive(false);
            __instance.m_Animator.SetInteger("State", 2);
        }

        // Define the colors for each faction
        // Default for generic start, Default for generic middle (not used), Placeholder for generic end color
        var (startColor, middleColor, endColor) = winningFaction switch
        {
            (FactionType)43 => (new Color32(181, 69, 255, 255), (Color32)Color.clear, new Color32(255, 0, 78, 255)), // #B545FF (Pandora Start), #FF004E (Pandora End)
            (FactionType)44 => (new Color32(45, 68, 181, 255), new Color32(174, 27, 30, 255), new Color32(252, 159, 50, 255)), // #2D44B5 (Compliance Start), #AE1B1E (Compliance Middle), #FC9F32 (Compliance End)
            (FactionType)42 => (new Color32(53, 159, 63, 255), (Color32)Color.clear, new Color32(63, 53, 159, 255)), // #359f3f (Egotist Start), #3f359f (Egotist End)
            (FactionType)33 => (new Color32(64, 64, 64, 255), (Color32)Color.clear, new Color32(208, 208, 208, 255)), // #404040 (Jackal/Recruit Start), #D0D0D0 (Jackal/Recruit End)
            FactionType.JESTER => (new Color32(245, 166, 212, 255), (Color32)Color.clear, new Color32(245, 166, 212, 255)), // #F5A6D4 (Jester Start), #F5A6D4 (Jester End)
            FactionType.DOOMSAYER => (new Color32(0, 204, 153, 255), (Color32)Color.clear, new Color32(0, 204, 153, 255)), // #00CC99 (Doomsayer Start), #00CC99 (Doomsayer End)
            FactionType.PIRATE => (new Color32(236, 194, 62, 255), (Color32)Color.clear, new Color32(236, 194, 62, 255)), // #ECC23E (Pirate Start), #ECC23E (Pirate End)
            FactionType.EXECUTIONER => (new Color32(148, 151, 151, 255), (Color32)Color.clear, new Color32(148, 151, 151, 255)), // #949797 (Executioner Start), #949797 (Executioner End)
            (FactionType)40 => (new Color32(130, 18, 82, 255), (Color32)Color.clear, new Color32(130, 18, 82, 255)), // #821252 (Inquisitor Start), #821252 (Inquisitor End)
            (FactionType)39 => (new Color32(174, 186, 135, 255), (Color32)Color.clear, new Color32(232, 252, 197, 255)), // #AEBA87 (Auditor Start), #E8FCC5 (Auditor End)
            (FactionType)38 => (new Color32(199, 115, 100, 255), (Color32)Color.clear, new Color32(201, 61, 80, 255)), // #C77364 (Judge Start), #C93D50 (Judge End)
            (FactionType)41 => (new Color32(252, 231, 154, 255), (Color32)Color.clear, new Color32(153, 156, 255, 255)), // #FCE79A (Starspawn Start), #999CFF (Starspawn End)
            (FactionType)34 => (new Color32(30, 73, 207, 255), (Color32)Color.clear, new Color32(30, 73, 207, 255)), // #1e49cf (Frogs Start), #1e49cf (Frogs End)
            (FactionType)35 => (new Color32(255, 195, 79, 255), (Color32)Color.clear, new Color32(255, 195, 79, 255)), // #ffc34f (Lions Start), #ffc34f (Lions End)
            (FactionType)36 => (new Color32(168, 21, 56, 255), (Color32)Color.clear, new Color32(168, 21, 56, 255)), // #a81538 (Hawks Start), #a81538 (Hawks End)
            (FactionType)37 => (new Color32(230, 149, 106, 255), (Color32)Color.clear, new Color32(230, 149, 106, 255)), // #E6956A (Cannibal Start), #E6956A (Cannibal End)
            (FactionType)250 => (new Color32(254, 166, 250, 255), (Color32)Color.clear, new Color32(254, 166, 250, 255)), // #FEA6FA (Lovers Start), #FEA6FA (Lovers End)
            FactionType.CURSED_SOUL => (new Color32(117, 0, 175, 255), (Color32)Color.clear, new Color32(117, 0, 175, 255)), // #7500AF (Cursed Soul Start), #7500AF (Cursed Soul End)
            _ => (new Color32(156, 154, 154, 255), (Color32)Color.clear, new Color32(156, 154, 154, 255)) // #9C9A9A (Default Start), #9C9A9A (Default End)
        };

        __instance.leftImage.color = startColor;
        __instance.rightImage.color = endColor;
        var text = string.Format("GUI_WINNERS_ARE_{0}", (int)winningFaction);
        var text2 = __instance.l10n(text);
        string gradientText;

        if (winningFaction == (FactionType)44)
            gradientText = Utils.ApplyGradient(text2, startColor, middleColor, endColor);
        else
            gradientText = Utils.ApplyGradient(text2, startColor, endColor);

        if (__instance.textAnimatorPlayer.gameObject.activeSelf)
            __instance.textAnimatorPlayer.ShowText(gradientText);
        else
        {
            // Fallback to default faction color
            if (ColorUtility.TryParseHtmlString(winningFaction.GetFactionColor(), out var color))
            {
                __instance.leftImage.color = color;
                __instance.rightImage.color = color;
                __instance.glow.color = color;
            }

            __instance.text.color = color;
            __instance.textAnimatorPlayer.ShowText(text2);
        }

        // Set up winners on the cinematic screen
        __instance.SetUpWinners(__instance.winningCharacters);
        return false;
    }
}

[HarmonyPatch(typeof(RoleRevealCinematicPlayer), nameof(RoleRevealCinematicPlayer.SetRole))]
public static class RoleRevealCinematicPlayerPatch
{
    public static bool Prefix(RoleRevealCinematicPlayer __instance, ref Role role)
    {
        if (!Constants.IconsInRoleReveal() || role == Role.NONE)
            return true;

        var newValue = $"<sprite=\"Cast\" name=\"Skin{__instance.roleRevealCinematic.skinId}\">{Service.Game.Cast.GetSkinName(__instance.roleRevealCinematic.skinId)}";
        var text = __instance.l10n("CINE_ROLE_REVEAL_SKIN").Replace("%skin%", newValue);
        __instance.skinTextPlayer.ShowText(text);

        __instance.totalDuration = Tuning.ROLE_REVEAL_TIME;
        __instance.silhouetteWrapper.gameObject.SetActive(true);
        __instance.silhouetteWrapper.SwapWithSilhouette((int)role);

        var newValue2 = role.GetTMPSprite() + role.ToColorizedDisplayString();
        var text2 = __instance.l10n("CINE_ROLE_REVEAL_ROLE").Replace("%role%", newValue2);
        __instance.roleTextPlayer.ShowText(text2);

        if (Pepper.GetCurrentGameType() == GameType.Ranked)
            __instance.playableDirector.Resume();

        return false;
    }
}