using Cinematics.Players;
using Home.HomeScene;
using Home.LoginScene;
using Home.Shared;
using Mentions;
using Home.Services;
using Mentions.Providers;
using SalemModLoaderUI;
using Server.Shared.Cinematics.Data;
using UnityEngine.EventSystems;
using Server.Shared.Extensions;

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

        if ((effectsState.bIsJailed && Fancy.ShowOverlayWhenJailed.Value) || (effectsState.bIsJailing && Fancy.ShowOverlayAsJailor.Value))
            JailorOverlayPrefab.Show(); // Show overlay if you are jailing or being jailed while the respective setting is on
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
/* [HarmonyPatch(typeof(FactionWinsCinematicPlayer), nameof(FactionWinsCinematicPlayer.Init))]
public static class PatchDefaultWinScreens
{
    private static readonly int State = Animator.StringToHash("State");

    public static bool Prefix(FactionWinsCinematicPlayer __instance, ICinematicData cinematicData)
    {
        __instance.elapsedDuration = 0f;
        Debug.Log($"FactionWinsCinematicPlayer current phase at start = {Pepper.GetGamePhase()}");
        __instance.cinematicData = cinematicData as FactionWinsCinematicData;
        __instance.totalDuration = CinematicFactionWinsTimes.GetWinTimeByFaction(__instance.cinematicData!.winningFaction);
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
            Service.Home.AudioService.PlayMusic("Audio/Music/TownVictory.wav", false, AudioController.AudioChannel.Cinematic);
            __instance.evilProp.SetActive(false);
            __instance.goodProp.SetActive(true);
            __instance.m_Animator.SetInteger(State, 1);
        }
        else
        {
            Service.Home.AudioService.PlayMusic("Audio/Music/CovenVictory.wav", false, AudioController.AudioChannel.Cinematic);
            __instance.evilProp.SetActive(true);
            __instance.goodProp.SetActive(false);
            __instance.m_Animator.SetInteger(State, 2);
        }

        // Define the colors for each faction
        // Default for generic start, Default for the generic middle (not used), Placeholder for generic end color
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
        var text = $"GUI_WINNERS_ARE_{(int)winningFaction}";
        var text2 = __instance.l10n(text);
        var gradientText = winningFaction == (FactionType)44 ? Utils.ApplyGradient(text2, startColor, middleColor, endColor) : Utils.ApplyGradient(text2, startColor, endColor);

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
} */

[HarmonyPatch(typeof(RoleRevealCinematicPlayer), nameof(RoleRevealCinematicPlayer.SetRole))]
public static class RoleRevealCinematicPlayerPatch
{
    private static FactionType CurrentFaction;

    [HarmonyPatch(nameof(RoleRevealCinematicPlayer.SetRole))]
    public static bool Prefix(RoleRevealCinematicPlayer __instance, Role role)
    {
        if (role == Role.NONE)
            return true;

        var showIcons = Fancy.IconsInRoleReveal.Value;
        var skinIcon = showIcons
            ? $"<sprite=\"Cast\" name=\"Skin{__instance.roleRevealCinematic.skinId}\">{Service.Game.Cast.GetSkinName(__instance.roleRevealCinematic.skinId)}"
            : Service.Game.Cast.GetSkinName(__instance.roleRevealCinematic.skinId);
        var skinText = __instance.l10n("CINE_ROLE_REVEAL_SKIN").Replace("%skin%", skinIcon);
        __instance.skinTextPlayer.ShowText(skinText);
        __instance.totalDuration = Tuning.ROLE_REVEAL_TIME;
        __instance.silhouetteWrapper.gameObject.SetActive(true);
        __instance.silhouetteWrapper.SwapWithSilhouette((int)role);
        var roleIcon = showIcons ? (role.GetTMPSprite() + role.ToColorizedDisplayString(CurrentFaction)) : role.ToColorizedDisplayString(CurrentFaction);
        roleIcon = roleIcon.Replace("RoleIcons\"", $"RoleIcons ({((role.GetFactionType() == CurrentFaction && Constants.CurrentStyle() == "Regular")
            ? "Regular"
            : Utils.FactionName(CurrentFaction, false))})\"");
        var roleText = __instance.l10n("CINE_ROLE_REVEAL_ROLE").Replace("%role%", roleIcon);
        __instance.roleTextPlayer.ShowText(roleText);

        if (Pepper.GetCurrentGameType() == GameType.Ranked)
            __instance.playableDirector.Resume();

        return false;
    }

    [HarmonyPatch(nameof(RoleRevealCinematicPlayer.HandleOnMyIdentityChanged))]
    public static void Prefix(PlayerIdentityData playerIdentity) => CurrentFaction = playerIdentity.faction;
}

[HarmonyPatch(typeof(SharedMentionsProvider), nameof(SharedMentionsProvider.BuildAchievementMentions))]
public static class AchievementMentionsPatch
{
    public static bool Prefix(SharedMentionsProvider __instance)
    {
        var allAchievementIds = Service.Game.Achievement.GetAllAchievementIds();
        var num = 0;

        foreach (var achievementId in allAchievementIds)
        {
            var gradient = Utils.CreateGradient(Fancy.AchievementStart.Value, Fancy.AchievementEnd.Value);
            var title = __instance.l10n($"GUI_ACHIEVEMENT_TITLE_{achievementId}");
            var match = $"~{title}";
            var encodedText = $"[[~{achievementId}]]";

            var styledTitle = __instance._useColors
                ? $"<b>{Utils.ApplyGradient($"{title}", gradient)}</b>"
                : $"<b>{title}</b>";

            var richText = $"{__instance.styleTagOpen}{__instance.styleTagFont}<link=\"~{achievementId}\">{styledTitle}</link>{__instance.styleTagClose}";

            var mentionInfo = new MentionInfo
            {
                mentionInfoType = MentionInfo.MentionInfoType.ACHIEVEMENT,
                richText = richText,
                encodedText = encodedText,
                hashCode = richText.ToLower().GetHashCode(),
                humanText = $"~{title.ToLower()}"
            };

            __instance.MentionInfos.Add(mentionInfo);

            __instance.MentionTokens.Add(new()
            {
                mentionTokenType = MentionToken.MentionTokenType.ACHIEVEMENT,
                match = match,
                mentionInfo = mentionInfo,
                priority = num
            });

            num++;
        }

        return false; // Skip the original method
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupGenericListItem), nameof(SpecialAbilityPopupGenericListItem.SetData))]
public static class SpecialAbilityPopupGenericListItemPatch
{
    // ReSharper disable once InconsistentNaming
    public static bool Prefix(SpecialAbilityPopupGenericListItem __instance, int position, string player_name, Sprite headshot, bool hasChoice1, bool hasChoice2, UIRoleData data)
    {
        var role = Role.NONE;
        var factionType = FactionType.NONE;

        if (Utils.GetRoleAndFaction(position, out var tuple))
        {
            role = tuple.Item1;
            factionType = tuple.Item2;
        }

        var roleText = "";
        var gradient = factionType.GetChangedGradient(role);

        if (role != Role.NONE)
            roleText = Fancy.FactionalRoleNames.Value ? Utils.GetRoleName(role, factionType, true) : $"({role.ToDisplayString()})";

        var text = $"{player_name} {Utils.ApplyGradient(roleText, gradient)}";
        __instance.playerName.SetText(text);
        __instance.playerHeadshot.sprite = headshot;
        __instance.characterPosition = position;
        __instance.playerNumber.text = $"{__instance.characterPosition + 1}.";
        var myRole = Pepper.GetMyRole();
        var uiRoleDataInstance = data.roleDataList.Find(d => d.role == myRole);

        if (uiRoleDataInstance != null)
        {
            __instance.choiceText.text = __instance.l10n($"GUI_ROLE_SPECIAL_ABILITY_VERB_{(int)uiRoleDataInstance.role}");
            __instance.choiceSprite.sprite = uiRoleDataInstance.specialAbilityIcon;
            __instance.choice2Text.text = __instance.l10n($"GUI_ROLE_ABILITY2_VERB_{(int)uiRoleDataInstance.role}");
            __instance.choice2Sprite.sprite = uiRoleDataInstance.abilityIcon2;
        }

        __instance.choiceButton.gameObject.SetActive(hasChoice1);
        __instance.choice2Button.gameObject.SetActive(hasChoice2);

        if (!hasChoice1)
        {
            __instance.selected1 = false;
            __instance.choiceButton.Deselect();
        }

        if (!hasChoice2)
        {
            __instance.selected2 = false;
            __instance.choice2Button.Deselect();
        }

        return false;
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupDayConfirmListItem), nameof(SpecialAbilityPopupDayConfirmListItem.SetData))]
public static class SpecialAbilityPopupDayConfirmListItemPatch
{
    // ReSharper disable once InconsistentNaming
    public static bool Prefix(SpecialAbilityPopupDayConfirmListItem __instance, int position, string player_name, Sprite headshot, bool hasChoice1, UIRoleData data)
    {
        var role = Role.NONE;
        var factionType = FactionType.NONE;

        if (Utils.GetRoleAndFaction(position, out var tuple))
        {
            role = tuple.Item1;
            factionType = tuple.Item2;
        }

        var roleText = "";
        var gradient = factionType.GetChangedGradient(role);

        if (role != Role.NONE)
            roleText = Fancy.FactionalRoleNames.Value ? Utils.GetRoleName(role, factionType, true) : $"({role.ToDisplayString()})";

        var text = $"{player_name} {Utils.ApplyGradient(roleText, gradient)}";
        __instance.playerName.SetText(text);
        __instance.playerHeadshot.sprite = headshot;
        __instance.characterPosition = position;
        __instance.playerNumber.text = $"{__instance.characterPosition + 1}.";
        var myRole = Pepper.GetMyRole();
        var uiRoleDataInstance = data.roleDataList.Find(d => d.role == myRole);

        if (uiRoleDataInstance != null)
        {
            __instance.choiceText.text = __instance.l10n(uiRoleDataInstance.specialAbilityVerb);
            __instance.choiceSprite.sprite = uiRoleDataInstance.specialAbilityIcon;
        }

        __instance.choiceButton.gameObject.SetActive(hasChoice1);
        __instance.selected = false;
        return false;
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupNecromancerRetributionistListItem), nameof(SpecialAbilityPopupNecromancerRetributionistListItem.SetData))]
public static class SpecialAbilityPopupNecromancerRetributionistListItemPatch
{
    // ReSharper disable once InconsistentNaming
    public static bool Prefix(SpecialAbilityPopupNecromancerRetributionistListItem __instance, int position, string player_name, Sprite headshot, bool hasChoice1, bool hasChoice2, UIRoleData
        data, Role role, SpecialAbilityPopupNecromancerRetributionist parent)
    {
        __instance.parent = parent;
        var myRole = Pepper.GetMyRole();
        var role2 = Role.NONE;
        var factionType = FactionType.NONE;

        if (Utils.GetRoleAndFaction(position, out var tuple))
        {
            role2 = tuple.Item1;
            factionType = tuple.Item2;
        }

        var roleText = "";
        var gradient = factionType.GetChangedGradient(role2);

        if (role2 != Role.NONE)
            roleText = Fancy.FactionalRoleNames.Value ? Utils.GetRoleName(role2, factionType, true) : $"({role2.ToDisplayString()})";

        var text = $"{player_name} {Utils.ApplyGradient(roleText, gradient)}";
        __instance.playerName.SetText(text);
        __instance.playerHeadshot.sprite = headshot;
        __instance.characterPosition = position;
        __instance.playerNumber.text = $"{__instance.characterPosition + 1}.";
        var uiRoleDataInstance = data.roleDataList.Find(d => d.role == myRole);
        var uiRoleDataInstance2 = data.roleDataList.Find(d => d.role == role);

        if (uiRoleDataInstance != null)
        {
            __instance.choiceText.text = __instance.l10n($"GUI_ROLE_SPECIAL_ABILITY_VERB_{(int)myRole}");
            __instance.choiceSprite.sprite = uiRoleDataInstance.specialAbilityIcon;
        }

        if (uiRoleDataInstance2 != null && role != Role.NONE)
        {
            __instance.choice2Text.text = __instance.GetAbilityVerb(uiRoleDataInstance2.role);
            __instance.choice2Sprite.sprite = uiRoleDataInstance2.role is Role.DEPUTY or Role.CONJURER ? uiRoleDataInstance2.specialAbilityIcon : uiRoleDataInstance2.abilityIcon;
        }

        __instance.choiceButton.gameObject.SetActive(hasChoice1);
        __instance.choice2Button.gameObject.SetActive(hasChoice2);

        if (!hasChoice1)
        {
            __instance.selected1 = false;
            __instance.choiceButton.Deselect();
        }

        if (!hasChoice2)
        {
            __instance.selected2 = false;
            __instance.choice2Button.Deselect();
        }

        if (EventSystem.current.currentSelectedGameObject != __instance.gpSelectable.gameObject)
            __instance.GPSelectExit();
        else
            __instance.GPSelectEnter();

        return false;
    }
}

[HarmonyPatch(typeof(Pepper), nameof(Pepper.GetMyFaction))]
public static class FixMyFaction
{
    public static bool Prefix(ref FactionType __result)
    {
        try
        {
            if (Leo.IsHomeScene())
                __result = FactionType.NONE;
            else
                __result = Pepper.IsLobbyPhase() ? FactionType.NONE : Service.Game.Sim.simulation.myIdentity.Data.faction;
        }
        catch
        {
            __result = FactionType.NONE; // Because the base game is allergic to null checks
        }

        return false;
    }
}

[HarmonyPatch(typeof(FactionWinsCinematicData), nameof(FactionWinsCinematicData.SetFaction))]
public static class VictoryCinematicSwapperPatch
{
    public static bool Prefix(FactionType factionType, FactionWinsCinematicData __instance)
    {
        __instance.winningFaction_ = factionType;
        __instance.cinematicType_ = Fancy.CinematicMap.TryGetValue(factionType, out var cinematic) ? cinematic.Value : CinematicType.FactionWins;
        return false;
    }
}

// Did the guys at TMP never think of devs trying to modify the arrow button??? seriously??? the drop-downs look so damn off with the arrow not updating up or down with respect to its selection
[HarmonyPatch(typeof(TMP_Dropdown), nameof(TMP_Dropdown.Show))]
public static class DropdownPatch
{
    public static void Postfix(TMP_Dropdown __instance)
    {
        var parent = __instance.GetComponentInParent<DropdownSetting>();

        if (parent)
            parent.Arrow.sprite = Fancy.Instance.Assets.GetSprite("DropDown_ArrowUp");
    }
}

[HarmonyPatch(typeof(FactionWinsStandardCinematicPlayer), nameof(FactionWinsStandardCinematicPlayer.SetUpWinners))]
public static class FactionWinsStandardCinematicPlayer_SetUpWinners_Patch
{
    public static void Postfix(FactionWinsStandardCinematicPlayer __instance)
    {
        if (Service.Game.Sim.info.gameInfo.Data.gamePhase != GamePhase.LOBBY || __instance.silhouetteWrappers == null || __instance.cinematicData.entries == null)
            return;

        int[] allowedSilhouettes =
        [
            1,2,3,4,5,6,7,8,9,10,
            11,12,13,14,15,16,17,18,19,20,
            21,22,23,24,25,26,27,28,29,30,
            31,32,33,34,35,36,37,38,39,40,
            41,42,43,44,45,46,47,48,49,50,
            51,52,53,54,55,56,
            240, 250, 251, 252, 253
        ];

        int[] allowedSilhouettesBTOS =
        [
            1,2,3,4,5,6,7,8,9,10,
            11,12,13,14,15,16,17,18,19,20,
            21,22,23,24,25,26,27,28,29,30,
            31,32,33,34,35,36,37,38,39,40,
            41,42,43,44,45,46,47,48,49,50,
            51,52,53,54,55,56,57,58,59,60,
            61,62,
            240, 250, 251, 252, 253
        ];

        foreach (var wrapper in __instance.silhouetteWrappers.Where(x => x))
        {
            var silhouetteId = 0;

            if (Fancy.SelectTestingRole.Value == Role.NONE && !Constants.IsBTOS2())
                silhouetteId = allowedSilhouettes.Random();

            if (Fancy.SelectTestingRole.Value == Role.NONE && Constants.IsBTOS2())
                silhouetteId = allowedSilhouettesBTOS.Random();

            if (Fancy.SelectTestingRole.Value != Role.NONE)
                silhouetteId = (int)Fancy.SelectTestingRole.Value;

            wrapper.SwapWithSilhouette(silhouetteId, true);
        }
    }
}

[HarmonyPatch(typeof(HomeAudioService))]
public static class CommonCurtisL
{
    [HarmonyPatch("PlayMusic")]
    [HarmonyPrefix]
    public static void OverrideMusic(ref string sound, ref bool loop)
    {
        if (sound == "TribunalIntro" && Fancy.DisableBTOSTribunal.Value)
        {
            sound = "Audio/Sfx/CinematicSFX/TribunalCinematic.wav";
        }
    }

}
