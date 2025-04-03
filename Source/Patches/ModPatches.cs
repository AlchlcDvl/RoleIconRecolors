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

// This patches the unique win screens.
/* [HarmonyPatch(typeof(FactionWinsStandardCinematicPlayer), nameof(FactionWinsStandardCinematicPlayer.Init))]
public static class PatchCustomWinScreens
{
    public static void Postfix(FactionWinsStandardCinematicPlayer __instance, ref ICinematicData cinematicData)
    {
        Debug.Log(string.Format("FactionWinsStandardCinematicPlayer current phase at end = {0}", Pepper.GetGamePhase()));
        __instance.elapsedDuration = 0f;
        __instance.cinematicData = cinematicData as FactionWinsCinematicData;
        var num = CinematicFactionWinsTimes.GetWinTimeByFaction(__instance.cinematicData.winningFaction);
        __instance.totalDuration = num;

        if (Pepper.IsResultsPhase())
            num += 0.2f;

        var winningFaction = __instance.cinematicData.winningFaction;

        if (winningFaction == FactionType.TOWN)
            Service.Home.AudioService.PlayMusic("Audio/Music/TownVictory.wav", false, AudioController.AudioChannel.Cinematic, true);
        else if (winningFaction is FactionType.COVEN or FactionType.NONE)
            Service.Home.AudioService.PlayMusic("Audio/Music/CovenVictory.wav", false, AudioController.AudioChannel.Cinematic, true);

        var text2 = __instance.l10n(string.Format("GUI_WINNERS_ARE_{0}", (int)winningFaction));
        string gradientText;

        // Define the colors for each faction
        Color startColor = Color.black;    // Default for generic start
        Color middleColor = Color.clear;   // Default for generic middle (not used)
        Color endColor = Color.white;      // Placeholder for generic end color

        if (winningFaction == (FactionType)43)
        {
            startColor = new Color(0.710f, 0.333f, 1f);      // #B545FF (Pandora Start)
            endColor = new Color(1f, 0f, 0.309f);             // #FF004E (Pandora End)
        }
        else if (winningFaction == (FactionType)44)
        {
            startColor = new Color(0.179f, 0.267f, 0.710f);   // #2D44B5 (Compliance Start)
            middleColor = new Color(0.682f, 0.105f, 0.125f);  // #AE1B1E (Compliance Middle)
            endColor = new Color(0.988f, 0.623f, 0.196f);     // #FC9F32 (Compliance End)
        }
        else if (winningFaction == (FactionType)42)
        {
            startColor = new Color(0.212f, 0.624f, 0.247f);   // #359f3f (Egotist Start)
            endColor = new Color(0.247f, 0.208f, 0.624f);     // #3f359f (Egotist End)
        }
        else if (winningFaction == (FactionType)33)
        {
            startColor = new Color(0.251f, 0.251f, 0.251f);  // #404040 (Jackal/Recruit Start)
            endColor = new Color(0.823f, 0.823f, 0.823f);    // #D0D0D0 (Jackal/Recruit End)
        }
        else if (winningFaction == FactionType.JESTER)
        {
            startColor = new Color(0.961f, 0.651f, 0.832f);   // #F5A6D4 (Jester Start)
            endColor = new Color(0.961f, 0.651f, 0.832f);     // #F5A6D4 (Jester End)
        }
        else if (winningFaction == FactionType.DOOMSAYER)
        {
            startColor = new Color(0f, 0.8f, 0.6f);           // #00CC99 (Doomsayer Start)
            endColor = new Color(0f, 0.8f, 0.6f);             // #00CC99 (Doomsayer End)
        }
        else if (winningFaction == FactionType.PIRATE)
        {
            startColor = new Color(0.929f, 0.761f, 0.247f);   // #ECC23E (Pirate Start)
            endColor = new Color(0.929f, 0.761f, 0.247f);     // #ECC23E (Pirate End)
        }
        else if (winningFaction == FactionType.EXECUTIONER)
        {
            startColor = new Color(0.580f, 0.596f, 0.596f);   // #949797 (Executioner Start)
            endColor = new Color(0.580f, 0.596f, 0.596f);     // #949797 (Executioner End)
        }
        else if (winningFaction == (FactionType)40)
        {
            startColor = new Color(0.510f, 0.071f, 0.322f);   // #821252 (Inquisitor Start)
            endColor = new Color(0.510f, 0.071f, 0.322f);     // #821252 (Inquisitor End)
        }
        else if (winningFaction == (FactionType)39)
        {
            startColor = new Color(0.678f, 0.729f, 0.529f);   // #AEBA87 (Auditor Start)
            endColor = new Color(0.909f, 0.988f, 0.769f);     // #E8FCC5 (Auditor End)
        }
        else if (winningFaction == (FactionType)38)
        {
            startColor = new Color(0.78f, 0.451f, 0.392f);    // #C77364 (Judge Start)
            endColor = new Color(0.792f, 0.241f, 0.314f);     // #C93D50 (Judge End)
        }
        else if (winningFaction == (FactionType)41)
        {
            startColor = new Color(0.988f, 0.906f, 0.604f);   // #FCE79A (Starspawn Start)
            endColor = new Color(0.6f, 0.612f, 1f);           // #999CFF (Starspawn End)
        }
        else if (winningFaction == (FactionType)34)
        {
            startColor = new Color(0.118f, 0.286f, 0.811f);   // #1e49cf (Frogs Start)
            endColor = new Color(0.118f, 0.286f, 0.811f);     // #1e49cf (Frogs End)
        }
        else if (winningFaction == (FactionType)35)
        {
            startColor = new Color(0.996f, 0.765f, 0.310f);   // #ffc34f (Lions Start)
            endColor = new Color(0.996f, 0.765f, 0.310f);     // #ffc34f (Lions End)
        }
        else if (winningFaction == (FactionType)36)
        {
            startColor = new Color(0.659f, 0.078f, 0.220f);   // #a81538 (Hawks Start)
            endColor = new Color(0.659f, 0.078f, 0.220f);     // #a81538 (Hawks End)
        }
        else if (winningFaction == (FactionType)37)
        {
            startColor = new Color(0.922f, 0.584f, 0.416f);   // #E6956A (Cannibal Start)
            endColor = new Color(0.922f, 0.584f, 0.416f);     // #E6956A (Cannibal End)
        }
        else if (winningFaction == (FactionType)250)
        {
            startColor = new Color(0.996f, 0.651f, 0.980f);   // #FEA6FA (Lovers Start)
            endColor = new Color(0.996f, 0.651f, 0.980f);     // #FEA6FA (Lovers End)
        }
        else if (winningFaction == FactionType.CURSED_SOUL)
        {
            startColor = new Color(0.459f, 0.000f, 0.686f);   // #7500AF (Cursed Soul Start)
            endColor = new Color(0.459f, 0.000f, 0.686f);     // #7500AF (Cursed Soul End)
        }
        else 
        {
            startColor = new Color(0.615f, 0.604f, 0.604f);   // #9C9A9A (Default Start)
            endColor = new Color(0.615f, 0.604f, 0.604f);     // #9C9A9A (Default End)
        }

        if (winningFaction == (FactionType)44)
            gradientText = AddChangedConversionTags.ApplyThreeColorGradient(text2, startColor, middleColor, endColor);
        else
            gradientText = AddChangedConversionTags.ApplyGradient(text2, startColor, endColor);

        if (__instance.textAnimatorPlayer.gameObject.activeSelf)
            __instance.textAnimatorPlayer.ShowText(gradientText);

        __instance.SetUpWinners();
        return;
    }
} */

// This patches the default win screens (used by modded factions).
[HarmonyPatch(typeof(FactionWinsCinematicPlayer), nameof(FactionWinsCinematicPlayer.Init))]
public static class PatchDefaultWinScreens
{
    public static void Postfix(FactionWinsCinematicPlayer __instance, ref ICinematicData cinematicData)
    {
        __instance.elapsedDuration = 0f;
        Debug.Log(string.Format("FactionWinsCinematicPlayer current phase at start = {0}", Pepper.GetGamePhase()));
        __instance.cinematicData = cinematicData as FactionWinsCinematicData;
        var winTimeByFaction = CinematicFactionWinsTimes.GetWinTimeByFaction(__instance.cinematicData.winningFaction);
        __instance.totalDuration = winTimeByFaction;
        __instance.callbackTimers.Clear();
        var spawnedCharacters = Service.Game.Cast.GetSpawnedCharacters();

        if (spawnedCharacters == null)
        {
            Debug.LogError("spawnedPlayers is null in GetCrowd()");
            return;
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

        var text = string.Format("GUI_WINNERS_ARE_{0}", (int)winningFaction);
        var text2 = __instance.l10n(text);
        string gradientText;

        // Define the colors for each faction
        Color startColor = Color.black;    // Default for generic start
        Color middleColor = Color.clear;   // Default for generic middle (not used)
        Color endColor = Color.white;      // Placeholder for generic end color

        if (winningFaction == (FactionType)43)
        {
            startColor = new Color(0.710f, 0.333f, 1f);      // #B545FF (Pandora Start)
            endColor = new Color(1f, 0f, 0.309f);             // #FF004E (Pandora End)
        }
        else if (winningFaction == (FactionType)44)
        {
            startColor = new Color(0.179f, 0.267f, 0.710f);   // #2D44B5 (Compliance Start)
            middleColor = new Color(0.682f, 0.105f, 0.125f);  // #AE1B1E (Compliance Middle)
            endColor = new Color(0.988f, 0.623f, 0.196f);     // #FC9F32 (Compliance End)
        }
        else if (winningFaction == (FactionType)42)
        {
            startColor = new Color(0.212f, 0.624f, 0.247f);   // #359f3f (Egotist Start)
            endColor = new Color(0.247f, 0.208f, 0.624f);     // #3f359f (Egotist End)
        }
        else if (winningFaction == (FactionType)33)
        {
            startColor = new Color(0.251f, 0.251f, 0.251f);  // #404040 (Jackal/Recruit Start)
            endColor = new Color(0.823f, 0.823f, 0.823f);    // #D0D0D0 (Jackal/Recruit End)
        }
        else if (winningFaction == FactionType.JESTER)
        {
            startColor = new Color(0.961f, 0.651f, 0.832f);   // #F5A6D4 (Jester Start)
            endColor = new Color(0.961f, 0.651f, 0.832f);     // #F5A6D4 (Jester End)
        }
        else if (winningFaction == FactionType.DOOMSAYER)
        {
            startColor = new Color(0f, 0.8f, 0.6f);           // #00CC99 (Doomsayer Start)
            endColor = new Color(0f, 0.8f, 0.6f);             // #00CC99 (Doomsayer End)
        }
        else if (winningFaction == FactionType.PIRATE)
        {
            startColor = new Color(0.929f, 0.761f, 0.247f);   // #ECC23E (Pirate Start)
            endColor = new Color(0.929f, 0.761f, 0.247f);     // #ECC23E (Pirate End)
        }
        else if (winningFaction == FactionType.EXECUTIONER)
        {
            startColor = new Color(0.580f, 0.596f, 0.596f);   // #949797 (Executioner Start)
            endColor = new Color(0.580f, 0.596f, 0.596f);     // #949797 (Executioner End)
        }
        else if (winningFaction == (FactionType)40)
        {
            startColor = new Color(0.510f, 0.071f, 0.322f);   // #821252 (Inquisitor Start)
            endColor = new Color(0.510f, 0.071f, 0.322f);     // #821252 (Inquisitor End)
        }
        else if (winningFaction == (FactionType)39)
        {
            startColor = new Color(0.678f, 0.729f, 0.529f);   // #AEBA87 (Auditor Start)
            endColor = new Color(0.909f, 0.988f, 0.769f);     // #E8FCC5 (Auditor End)
        }
        else if (winningFaction == (FactionType)38)
        {
            startColor = new Color(0.78f, 0.451f, 0.392f);    // #C77364 (Judge Start)
            endColor = new Color(0.792f, 0.241f, 0.314f);     // #C93D50 (Judge End)
        }
        else if (winningFaction == (FactionType)41)
        {
            startColor = new Color(0.988f, 0.906f, 0.604f);   // #FCE79A (Starspawn Start)
            endColor = new Color(0.6f, 0.612f, 1f);           // #999CFF (Starspawn End)
        }
        else if (winningFaction == (FactionType)34)
        {
            startColor = new Color(0.118f, 0.286f, 0.811f);   // #1e49cf (Frogs Start)
            endColor = new Color(0.118f, 0.286f, 0.811f);     // #1e49cf (Frogs End)
        }
        else if (winningFaction == (FactionType)35)
        {
            startColor = new Color(0.996f, 0.765f, 0.310f);   // #ffc34f (Lions Start)
            endColor = new Color(0.996f, 0.765f, 0.310f);     // #ffc34f (Lions End)
        }
        else if (winningFaction == (FactionType)36)
        {
            startColor = new Color(0.659f, 0.078f, 0.220f);   // #a81538 (Hawks Start)
            endColor = new Color(0.659f, 0.078f, 0.220f);     // #a81538 (Hawks End)
        }
        else if (winningFaction == (FactionType)37)
        {
            startColor = new Color(0.922f, 0.584f, 0.416f);   // #E6956A (Cannibal Start)
            endColor = new Color(0.922f, 0.584f, 0.416f);     // #E6956A (Cannibal End)
        }
        else if (winningFaction == (FactionType)250)
        {
            startColor = new Color(0.996f, 0.651f, 0.980f);   // #FEA6FA (Lovers Start)
            endColor = new Color(0.996f, 0.651f, 0.980f);     // #FEA6FA (Lovers End)
        }
        else if (winningFaction == FactionType.CURSED_SOUL)
        {
            startColor = new Color(0.459f, 0.000f, 0.686f);   // #7500AF (Cursed Soul Start)
            endColor = new Color(0.459f, 0.000f, 0.686f);     // #7500AF (Cursed Soul End)
        }
        else 
        {
            startColor = new Color(0.615f, 0.604f, 0.604f);   // #9C9A9A (Default Start)
            endColor = new Color(0.615f, 0.604f, 0.604f);     // #9C9A9A (Default End)
        }

        __instance.leftImage.color = startColor;
        __instance.rightImage.color = endColor;

        if (winningFaction == (FactionType)44)
            gradientText = AddChangedConversionTags.ApplyThreeColorGradient(text2, startColor, middleColor, endColor);
        else
            gradientText = AddChangedConversionTags.ApplyGradient(text2, startColor, endColor);

        if (__instance.textAnimatorPlayer.gameObject.activeSelf)
            __instance.textAnimatorPlayer.ShowText(gradientText);
        else
        {
            // Fallback to default faction color
            if (ColorUtility.TryParseHtmlString(winningFaction.GetFactionColor(), out Color color))
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
        return;
    }
}


[HarmonyPatch(typeof(ClientRoleExtensions), nameof(ClientRoleExtensions.ToColorizedDisplayString), typeof(Role), typeof(FactionType))]
public static class AddChangedConversionTags
{
    public static string ApplyGradient(string text, Color color1, Color color2)
    {
        var gradient = new Gradient();
        gradient.SetKeys(
        [
            new(color1, 0f),
            new(color2, 1f)
        ],
        [
            new(1f, 0f),
            new(1f, 1f)
        ]);
        var text2 = "";

        for (var i = 0; i < text.Length; i++)
            text2 += $"<color={ToHexString(gradient.Evaluate((float)i / text.Length))}>{text[i]}</color>";

        return text2;
    }

    public static string ApplyThreeColorGradient(string text, Color color1, Color color2, Color color3)
    {
        var gradient = new Gradient();
        gradient.SetKeys(
        [
            new(color1, 0f),
            new(color2, 0.5f),
            new(color3, 1f)
        ],
        [
            new(1f, 0f),
            new(1f, 1f)
        ]);
        var text2 = "";

        for (var i = 0; i < text.Length; i++)
            text2 += $"<color={ToHexString(gradient.Evaluate((float)i / text.Length))}>{text[i]}</color>";

        return text2;
    }

    public static string ApplyGradient(string text, Gradient gradient)
    {
        var text2 = "";

        for (var i = 0; i < text.Length; i++)
            text2 += $"<color={ToHexString(gradient.Evaluate((float)i / text.Length))}>{text[i]}</color>";

        return text2;
    }

    public static string ToHexString(Color color)
    {
        Color32 color2 = color;
        return $"#{color2.r:X2}{color2.g:X2}{color2.b:X2}";
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