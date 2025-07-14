using Cinematics.Players;
using FlexMenu;
using Game.Characters;
using Game.DevMenu;
using Home.HomeScene;
using Home.LoginScene;
using Home.Services;
using Home.Shared;
using Mentions;
using Mentions.Providers;
using NewModLoading;
using SalemModLoader;
using SalemModLoaderUI;
using Server.Shared.Cinematics.Data;
using Server.Shared.Collections;
using Server.Shared.Extensions;
using Server.Shared.State;
using UnityEngine.EventSystems;
using UnityEngine.TextCore;

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

        var roleName = role.ToColorizedNoLabel(CurrentFaction);
        var roleIcon = showIcons ? (role.GetTMPSprite() + roleName) : roleName;

        roleIcon = roleIcon.Replace("RoleIcons\"", $"RoleIcons ({((role.GetFactionType() == CurrentFaction && Constants.CurrentStyle() == "Regular")
            ? "Regular"
            : Utils.FactionName(CurrentFaction, false))})\"");

        string roleRevealKey;

        if (role.IsHorseman())
            roleRevealKey = "FANCY_ROLE_REVEAL_ROLE_HORSEMAN";
        else if (Utils.StartsWithVowel(roleName))
            roleRevealKey = "FANCY_ROLE_REVEAL_ROLE_VOWEL";
        else
            roleRevealKey = "FANCY_ROLE_REVEAL_ROLE_CONSONANT";

        var roleText = __instance.l10n(roleRevealKey).Replace("%role%", roleIcon);
        __instance.roleTextPlayer.ShowText(roleText);

        // TODO: Faction Specific Role Blurbs
        var roleBlurb = Fancy.FactionalRoleBlurbs ? role.ToFactionalRoleBlurb(CurrentFaction) : role.GetRoleBlurb();
        __instance.roleBlurbTextPlayer.ShowText(roleBlurb);

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
        var achievementIds = Service.Game.Achievement.GetAllAchievementIds();
        var useColors = __instance._useColors;
        var gradient = useColors ? Utils.CreateGradient(Fancy.AchievementStart.Value, Fancy.AchievementEnd.Value) : null;

        var priority = 0;

        foreach (var achievementId in achievementIds)
        {
            var title = __instance.l10n($"GUI_ACHIEVEMENT_TITLE_{achievementId}");

            if (string.IsNullOrWhiteSpace(title))
                continue;

            var color = Fancy.AchievementStart.Value;
            var encodedText = $"[[~{achievementId}]]";
            var match = $"~{title}";

            var styledTitle = useColors
                ? $"<b>{Utils.ApplyGradient($"{title}", gradient)}</b>"
                : $"<b>{title}</b>";

            var richText = $"{__instance.styleTagOpen}{__instance.styleTagFont}<link=\"~{achievementId}\">{styledTitle}</link>{__instance.styleTagClose}";

            var mentionInfo = new MentionInfo
            {
                mentionInfoType = MentionInfo.MentionInfoType.ACHIEVEMENT,
                richText = richText,
                encodedText = encodedText,
                hashCode = richText.ToLowerInvariant().GetHashCode(),
                humanText = $"~{title.ToLowerInvariant()}"
            };

            __instance.MentionInfos.Add(mentionInfo);
            __instance.MentionTokens.Add(new MentionToken
            {
                mentionTokenType = MentionToken.MentionTokenType.ACHIEVEMENT,
                match = match,
                mentionInfo = mentionInfo,
                priority = priority++
            });
        }

        return false;
    }
}

[HarmonyPatch(typeof(ClientRoleExtensions), nameof(ClientRoleExtensions.ToColorizedDisplayString), [typeof(Role), typeof(FactionType)])]
public static class ModifierFactionPatch
{
    public static bool Prefix(ref string __result, Role role, FactionType factionType)
    {
        if (role.IsModifierCard() && Fancy.ModifierFactions.Value)
        {
            FactionType modifierFaction = FactionType.NONE;
            if (Constants.IsBTOS2())
                modifierFaction = (role) switch
                {
                    Btos2Role.Vip => Btos2Faction.Town,
                    Btos2Role.CovenTownTraitor => Btos2Faction.Coven,
                    Btos2Role.GhostTown => Btos2Faction.Shroud,
                    Btos2Role.PerfectTown => Btos2Faction.Town,
                    Btos2Role.AnonVoting => Btos2Faction.Judge,
                    Btos2Role.OneTrial => Btos2Faction.Executioner,
                    Btos2Role.ApocTownTraitor => Btos2Faction.Apocalypse,
                    Btos2Role.NecroPass => Btos2Faction.Coven,
                    Btos2Role.Egotist => Btos2Faction.Egotist,
                    Btos2Role.SpeakingSpirits => Btos2Faction.CursedSoul,
                    Btos2Role.CompliantKillers => Btos2Faction.Compliance,
                    Btos2Role.PandorasBox => Btos2Faction.Pandora,
                    Btos2Role.CovenVip => Btos2Faction.Coven,
                    Btos2Role.Lovers => Btos2Faction.Lovers,
                    Btos2Role.FeelinLucky => Btos2Faction.Jester,
                    Btos2Role.AllOutliers => Btos2Faction.CursedSoul,
                    _ => FactionType.NONE
                };
            else
                modifierFaction = (role) switch
                {
                    Role.VIP => FactionType.TOWN,
                    Role.TOWN_TRAITOR => FactionType.COVEN,
                    Role.GHOST_TOWN => FactionType.SHROUD,
                    Role.NO_TOWN_HANGED => FactionType.TOWN,
                    Role.ONE_TRIAL_PER_DAY => FactionType.EXECUTIONER,
                    Role.FOUR_HORSEMEN => FactionType.APOCALYPSE,
                    Role.ALL_OUTLIERS => FactionType.CURSED_SOUL,
                    Role.ELECTION => FactionType.TOWN,
                    Role.FEELIN_LUCKY => FactionType.JESTER,
                    _ => FactionType.NONE
                };
            if (modifierFaction != FactionType.NONE)
            {
                __result = Utils.ApplyGradient(role.ToDisplayString(), Gradients.GetChangedGradient(modifierFaction, role));
                return false;
            }
        }
        return true;
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

[HarmonyPatch(typeof(SpecialAbilityPopupGenericDualTargetListItem), nameof(SpecialAbilityPopupGenericDualTargetListItem.SetData))]
public static class PatchSpecialAbilityPopupGenericDualTargetListItem
{
    public static bool Prefix(
        SpecialAbilityPopupGenericDualTargetListItem __instance, int position, string player_name, Sprite headshot, bool hasChoice1, bool hasChoice2, UIRoleData
        data, Role role, SpecialAbilityPopupGenericDualTarget parent)
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
            roleText = Fancy.FactionalRoleNames.Value
                ? Utils.GetRoleName(role2, factionType, true)
                : $"({role2.ToDisplayString()})";

        var text = $"{player_name} {Utils.ApplyGradient(roleText, gradient)}";
        __instance.playerName.SetText(text);

        __instance.playerHeadshot.sprite = headshot;
        __instance.characterPosition = position;
        __instance.playerNumber.text = $"{__instance.characterPosition + 1}.";

        var uiRoleDataInstance = data.roleDataList.Find(d => d.role == myRole);
        if (uiRoleDataInstance != null)
        {
            __instance.choiceText.text = __instance.l10n($"GUI_ROLE_SPECIAL_ABILITY_VERB_{(int)myRole}");
            __instance.choiceSprite.sprite = uiRoleDataInstance.specialAbilityIcon;
            __instance.choice2Text.text = __instance.l10n($"GUI_ROLE_SPECIAL_ABILITY_VERB_{(int)myRole}");
            __instance.choice2Sprite.sprite = uiRoleDataInstance.specialAbilityIcon;
        }

        __instance.choiceButton.gameObject.SetActive(hasChoice1 && !__instance.selected2);
        __instance.choice2Button.gameObject.SetActive(hasChoice2 && !__instance.selected1);

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
[HarmonyPatch(typeof(TMP_Dropdown))]
public static class DropdownPatch
{
    [HarmonyPatch(nameof(TMP_Dropdown.Show))]
    public static void Postfix(TMP_Dropdown __instance)
    {
        var parent = __instance.GetComponentInParent<DropdownSetting>();

        if (parent)
            parent.Arrow.sprite = Fancy.Instance.Assets.GetSprite("DropDown_ArrowUp");
    }

    [HarmonyPatch("ImmediateDestroyDropdownList")]
    public static void Prefix(TMP_Dropdown __instance)
    {
        var parent = __instance.GetComponentInParent<DropdownSetting>();

        if (parent)
            parent.Arrow.sprite = Fancy.Instance.Assets.GetSprite("DropDown_ArrowDown");
    }
}

[HarmonyPatch(typeof(FactionWinsStandardCinematicPlayer), nameof(FactionWinsStandardCinematicPlayer.SetUpWinners))]
public static class FactionWinsStandardCinematicPlayer_SetUpWinners_Patch
{
    private static readonly int[] AllowedSilhouettes =
    [
        1,2,3,4,5,6,7,8,9,10,
        11,12,13,14,15,16,17,18,19,20,
        21,22,23,24,25,26,27,28,29,30,
        31,32,33,34,35,36,37,38,39,40,
        41,42,43,44,45,46,47,48,49,50,
        51,52,53,54,55,56,57,58,59,60,
        240, 250, 251, 252, 253
    ];
    private static readonly int[] AllowedSilhouettesBTOS =
    [
        ..AllowedSilhouettes,
        61,62,63,64
    ];

    public static void Postfix(FactionWinsStandardCinematicPlayer __instance)
    {
        var gamePhase = Service.Game.Sim.info.gameInfo.Data.gamePhase;

        if (gamePhase != GamePhase.LOBBY || __instance.silhouetteWrappers == null || __instance.characterWrappers == null || __instance.cinematicData.entries == null)
            return;

        foreach (var wrapper in __instance.silhouetteWrappers.Where(x => x))
        {
            var silhouetteId = 0;

            if (Fancy.SelectTestingRole.Value == Role.NONE)
                silhouetteId = Constants.IsBTOS2() ? AllowedSilhouettesBTOS.Random() : AllowedSilhouettes.Random();
            else
                silhouetteId = (int)Fancy.SelectTestingRole.Value;

            wrapper.SwapWithSilhouette(silhouetteId, true);
        }

        foreach (var wrapper in __instance.characterWrappers.Where(x => x))
            wrapper.SwapWithCharacter(URandom.RandomRangeInt(0, 86), true, 0);
    }
}

[HarmonyPatch(typeof(HomeAudioService), nameof(HomeAudioService.PlayMusic))]
public static class CommonCurtisL
{
    public static void Prefix(ref string sound)
    {
        if (sound == "TribunalIntro" && Fancy.DisableBTOSTribunal.Value)
            sound = "Audio/Sfx/CinematicSFX/TribunalCinematic.wav";
    }
}

[HarmonyPatch(typeof(PickFactionContext), nameof(PickFactionContext.Initialize))]
public static class AddBTOSFactionsToDevMenu
{
    public static void Postfix(PickFactionContext __instance)
    {
        if (!Constants.IsBTOS2())
            return;

        for (byte i = 33; i < 45; i++)
            AddCustomFactionEntry(__instance, i);

        AddCustomFactionEntry(__instance, 250);
    }

    private static void AddCustomFactionEntry(PickFactionContext context, byte id)
    {
        var factionType = (FactionType)id;
        var factionBox = new Box<FactionType>(factionType);

        var label = $"<color={factionType.GetFactionColor()}>Faction {id}</color>";

        var entry = new FlexMenuEntry()
            .SetLabel(label)
            .SetAction(() => context.HandleClickFaction(factionBox.Value));

        context.AddEntry(entry);
    }
}

[HarmonyPatch(typeof(PickRoleContext), nameof(PickRoleContext.Initialize))]
public static class AddBTOS2RolesToDevMenu
{
    public static void Postfix(PickRoleContext __instance)
    {
        if (Constants.IsBTOS2())
        {
            for (var i = (byte)Role.ROLE_COUNT; i < 65; i++)
                AddCustomRoleEntry(__instance, i);
        }

        foreach (var i in new byte[] { 240, 241, 250, 251, 252, 253, 254 })
            AddCustomRoleEntry(__instance, i);
    }

    private static void AddCustomRoleEntry(PickRoleContext context, byte roleId)
    {
        var role = (Role)roleId;
        var roleBox = new Box<Role>(role);
        var label = role.ToColorizedDisplayString();
        var entry = new FlexMenuEntry()
            .SetLabel(label)
            .SetAction(() => context.HandleClickRole(roleBox.Value));

        context.AddEntry(entry);
    }
}

[HarmonyPatch(typeof(RoleDeckBuilder), nameof(RoleDeckBuilder.GetSortedRoleSlots))]
public static class PandoraAndComplianceRoleSlotsPatch
{
    [HarmonyPriority(Priority.Last)]
    public static void Postfix(RoleDeckBuilder __instance, ref List<RoleDeckSlot> __result)
    {
        if (!Constants.IsBTOS2())
            return;

        var list = new List<RoleDeckSlot>();
        var alignments = new[]
        {
            RoleAlignment.TOWN,
            Btos2RoleAlignment.Pandora,
            Btos2RoleAlignment.Compliance,
            RoleAlignment.COVEN,
            Btos2RoleAlignment.Apocalypse,
            RoleAlignment.NEUTRAL
        };
        var isPandora = Constants.IsPandora();
        var isCompliance = Constants.IsCompliance();

        foreach (var alignment in alignments)
        {
            if (isPandora && (alignment == RoleAlignment.COVEN || alignment == Btos2RoleAlignment.Apocalypse))
                continue;

            var currentAlignmentIsPandora = alignment == Btos2RoleAlignment.Pandora && isPandora;
            var currentAlignmentIsCompliance = alignment == Btos2RoleAlignment.Compliance && isCompliance;

            list.AddRange(__instance.GetPredicateRoleSlots(r => r switch
            {
                { Role1: var role1 } when role1.IsResolved() && !r.Role2.IsResolved() => SingleRolePredicate(r),
                { Role1: var role1, Role2: var role2 } when role1.IsResolved() && role2.IsResolved() => TwoRolePredicate(r),
                { Role1: var role1, Role2: var role2 } when role1.IsBucket() && role2.IsResolved() => TwoRolePredicate(r),
                { Role1: var role1, Role2: var role2 } when role1.IsBucket() && role2.IsBucket() => TwoRolePredicate(r),
                { Role1: var role1 } when r.IsBucket() && role1.GetRoleBucket().subAlignment != SubAlignment.ANY => SingleRolePredicate(r),
                { Role1: var role1 } when r.IsBucket() && role1.GetRoleBucket().subAlignment == SubAlignment.ANY => SingleRolePredicate(r),
                _ => false
            }));

            if (currentAlignmentIsCompliance)
                list.AddRange(__instance.GetPredicateRoleSlots(r => r.Role1 == Btos2Role.NeutralKilling));

            bool SingleRolePredicate(RoleDeckSlot r) =>
                !IsNeutralExclusion(r, alignment, isCompliance) &&
                MatchesAlignmentConditions(r.Role1, alignment, currentAlignmentIsPandora, currentAlignmentIsCompliance);

            bool TwoRolePredicate(RoleDeckSlot r) =>
                !IsNeutralExclusion(r, alignment, isCompliance) &&
                MatchesAlignmentConditions(r.Role1, alignment, currentAlignmentIsPandora, currentAlignmentIsCompliance) &&
                MatchesAlignmentConditions(r.Role2, alignment, currentAlignmentIsPandora, currentAlignmentIsCompliance);
        }

        list.AddRange(__instance.GetPredicateRoleSlots(r => r.Role1.IsResolved() && r.Role2.IsResolved() && FinalCombinedRolePredicate(r)));
        list.AddRange(__instance.GetPredicateRoleSlots(r => r.Role1.IsBucket() && r.Role2.IsResolved() && FinalCombinedRolePredicate(r)));
        list.AddRange(__instance.GetPredicateRoleSlots(r => r.Role1.IsBucket() && r.Role2.IsBucket() && FinalCombinedRolePredicate(r)));
        list.AddRange(__instance.GetPredicateRoleSlots(r => r.IsBucket() && r.Role1.GetAlignment() == RoleAlignment.ANY));

        __result = list;

        bool MatchesAlignmentConditions(Role role, RoleAlignment currentAlignment, bool isPandoraMode, bool isComplianceMode)
        {
            return role.GetAlignment() == currentAlignment ||
                    (isPandoraMode && (role.GetAlignment() == RoleAlignment.COVEN || role.GetAlignment() == Btos2RoleAlignment.Apocalypse)) ||
                    (isComplianceMode && role.IsNeutralKilling());
        }

        bool IsNeutralExclusion(RoleDeckSlot slot, RoleAlignment currentAlignment, bool isComplianceMode)
        {
            return currentAlignment == RoleAlignment.NEUTRAL && isComplianceMode &&
                    (slot.Role1.IsNeutralKilling() || slot.Role2.IsNeutralKilling());
        }

        bool FinalCombinedRolePredicate(RoleDeckSlot r) =>
            r.Role1.GetAlignment() != r.Role2.GetAlignment() &&
            (!isPandora || !((r.Role1.GetAlignment() == RoleAlignment.COVEN && r.Role2.GetAlignment() == Btos2RoleAlignment.Apocalypse) ||
                (r.Role2.GetAlignment() == RoleAlignment.COVEN && r.Role1.GetAlignment() == Btos2RoleAlignment.Apocalypse))) &&
            (!isCompliance || !(r.Role1.IsNeutralKilling() && r.Role2.IsNeutralKilling()));
    }
}

[HarmonyPatch(typeof(RoleDeckSlot), nameof(RoleDeckSlot.GetRoleAlignment))]
public static class PandoraAndComplianceDeckSlotPatch
{
    public static bool Prefix(RoleDeckSlot __instance, ref RoleAlignment __result)
    {
        if (!Constants.IsBTOS2())
            return true;

        var role1Alignment = __instance.Role1.GetAlignment();
        var role2Alignment = __instance.Role2.GetAlignment();

        if (Constants.IsPandora())
        {
            if (role1Alignment is RoleAlignment.COVEN or Btos2RoleAlignment.Apocalypse)
                role1Alignment = Btos2RoleAlignment.Pandora;

            if (role2Alignment is RoleAlignment.COVEN or Btos2RoleAlignment.Apocalypse)
                role2Alignment = Btos2RoleAlignment.Pandora;
        }

        if (Constants.IsCompliance())
        {
            if (__instance.Role1.IsNeutralKilling() || __instance.Role1 == Btos2Role.NeutralKilling)
                role1Alignment = Btos2RoleAlignment.Compliance;

            if (__instance.Role2.IsNeutralKilling() || __instance.Role1 == Btos2Role.NeutralKilling)
                role2Alignment = Btos2RoleAlignment.Compliance;
        }

        __result = !__instance.IsDualBucket() || role1Alignment == role2Alignment ? role1Alignment : RoleAlignment.ANY;
        return false;
    }
}

[HarmonyPatch(typeof(RoleDeckListItem), nameof(RoleDeckListItem.SetData))]
public static class PandoraAndComplianceListItemPatch
{
    public static bool Prefix(RoleDeckListItem __instance, RoleDeckSlot a_roleDeckSlot, RoleDeckPanelController parent, bool a_isBan)
    {
        if (!Constants.IsBTOS2())
            return true;

        __instance.Reset();
        __instance._parentPanel = parent;
        __instance.background.SetActive(false);
        __instance.roleDeckSlot = a_roleDeckSlot;
        __instance.role = a_roleDeckSlot.Role1;
        __instance.role2 = a_roleDeckSlot.Role2;
        __instance.isBan = a_isBan;
        Debug.Log(string.Format("Setting Deck Slot {0} + {1}", __instance.role, __instance.role2));
        var factionType1 = __instance.role.GetFaction();
        var factionType2 = __instance.role2.GetFaction();
        var isPandora = Constants.IsPandora();
        var isCompliance = Constants.IsCompliance();

        if (isPandora && factionType1 is FactionType.COVEN or FactionType.APOCALYPSE)
            factionType1 = Btos2Faction.Pandora;

        if (isPandora && factionType2 is FactionType.COVEN or FactionType.APOCALYPSE)
            factionType2 = Btos2Faction.Pandora;

        if (isCompliance && __instance.role.IsNeutralKilling())
            factionType1 = Btos2Faction.Compliance;

        if (isCompliance && __instance.role2.IsNeutralKilling())
            factionType2 = Btos2Faction.Compliance;

        if (__instance.role2 == Role.NONE)
            __instance.roleName.text = __instance.role.ToColorizedDisplayString(factionType1) ?? "";
        else
        {
            __instance.roleName.text = __instance.role.ToColorizedShortenedDisplayString(factionType1) + " <color=#FFFFFF40>-</color> " +
                __instance.role2.ToColorizedShortenedDisplayString(factionType2);
        }

        __instance.roleName.gameObject.SetActive(true);
        __instance.roleImage.gameObject.SetActive(true);

        if (__instance.isBan)
            __instance.roleImage.sprite = __instance.bannedSprite;
        else if (__instance.role.IsModifierCard())
            __instance.roleImage.sprite = __instance.uiRoleData.hostOptionsDataList.Find(d => d.role == __instance.role).roleIcon;
        else if (__instance.role.IsBucket())
            __instance.roleImage.sprite = __instance.uiRoleData.roleBucketDataList.Find(d => d.role == __instance.role).roleIcon;
        else
            __instance.roleImage.sprite = __instance.uiRoleData.roleDataList.Find(d => d.role == __instance.role).roleIcon;

        __instance.roleImage.SetAllDirty();
        __instance.gameObject.SetActive(true);
        __instance.ValidateButtons();
        return false;
    }
}

// THIS GOD FOR SAKEN PATCH REFUSES TO DO ANYTHING, SOMEONE FIX THIS SHIT

/* HEY LOONIE
NEXT TIME YOU WANT A FUCKING MULTI-LINE COMMENT
DO IT LIKE THIS PLEASE
- synapsium synapperson XIV */

[HarmonyPatch(typeof(TribunalCinematicPlayer), nameof(TribunalCinematicPlayer.CommonSetup))]
public static class MarshalCinematicFixes
{
    public static void Postfix(TribunalCinematicPlayer __instance)
    {
        if (!Constants.IsBTOS2())
            return;

        var playerPosition = __instance.roleRevealCinematic.playerPosition;
        var characterByPosition = Service.Game.Cast.GetCharacterByPosition(playerPosition);

        if (characterByPosition)
            characterByPosition.characterSprite.SetColor(Color.clear);

        var playerName = Service.Game.Cast.GetPlayerName(playerPosition, false);
        var text = (__instance.roleRevealCinematic.hasRevealed ? Utils.GetString("FANCY_MARSHAL_CINEMATIC_2") : Utils.GetString("FANCY_MARSHAL_CINEMATIC_BTOS")).Replace("%name%", playerName);
        // __instance.silhouetteWrapper.SwapWithSilhouette(56, true);
        __instance.textAnimatorPlayer.ShowText(text);
    }
}