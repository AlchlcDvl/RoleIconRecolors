using Game.Achievements;
using Home.Common.Dialog;
using Home.Interface;
using Home.Shared;
using Mentions.Providers;
using Mentions.UI;
using Game.Interface;

namespace FancyUI.Patches;

[HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.HandleOnMyIdentityChanged))]
public static class RoleCardPanelPatch
{
    public static bool Prefix(RoleCardPanel __instance, PlayerIdentityData playerIdentityData)
    {
        __instance.CurrentRole = playerIdentityData?.role ?? Pepper.GetMyRole();
        __instance.CurrentFaction = playerIdentityData?.faction ?? Pepper.GetMyFaction();
        __instance.SetRole(__instance.CurrentRole);
        __instance.ValidateSpecialAbilityPanel();
        __instance.ValidateBackgroundColor();
        __instance.ShowRoleDescIf0Quality();

        __instance.transform.GetChild(5).GetComponent<Image>().SetImageColor(ColorType.Wood);
        __instance.transform.GetChild(10).GetComponent<Image>().SetImageColor(ColorType.Wood);
        __instance.transform.GetChild(20).GetComponentInChildren<Image>().SetImageColor(ColorType.Metal);

        if (Fancy.SelectedUITheme.Value == UITheme.Faction)
        {
            var pooledChat = PooledChatViewSwitcherPatch.Cache;
            var chatInput = ChatInputControllerPatch.Cache;
            var dayNight = HudTimeChangePanelPatch.Cache;
            var abilityPanel = PatchAbilityPanel.Cache;
            var specialAbility = SpecialAbilityPanelPatch1.Cache;
            var specialAbilityPopup = SpecialAbilityPopupPanelPatch.Cache;
            var hudDock = HudDockPanelPatch.Cache;
            var rlgy = HudRoleListAndGraveyardControllerPatch.Cache;

            if (pooledChat)
                PooledChatViewSwitcherPatch.Postfix(pooledChat);

            if (chatInput)
                ChatInputControllerPatch.Postfix(chatInput);

            if (dayNight)
                HudTimeChangePanelPatch.Postfix(dayNight);

            if (abilityPanel)
                PatchAbilityPanel.Postfix(abilityPanel);

            if (specialAbility)
                SpecialAbilityPanelPatch1.Postfix(specialAbility);

            if (specialAbilityPopup)
                SpecialAbilityPopupPanelPatch.Postfix(specialAbilityPopup);

            if (hudDock)
                HudDockPanelPatch.Postfix(hudDock);

            if (rlgy)
                HudRoleListAndGraveyardControllerPatch.Postfix1(rlgy);
        }

        Utils.UpdateMaterials();
        return false;
    }
}

[HarmonyPatch(typeof(SpecialAbilityPanel), nameof(SpecialAbilityPanel.Start))]
public static class SpecialAbilityPanelPatch1
{
    public static SpecialAbilityPanel Cache;

    public static void Postfix(SpecialAbilityPanel __instance)
    {
        Cache = __instance;
        var og = __instance.transform.GetChild(1).GetComponent<Image>();
        Image copy;

        if (og.GetComponent<DummyBehaviour>())
        {
            copy = __instance.transform.GetChild(1).GetComponent<Image>();
            og = __instance.transform.GetChild(2).GetComponent<Image>();
        }
        else
        {
            copy = UObject.Instantiate(og, og.transform.parent);

            og.sprite = Fancy.Instance.Assets.GetSprite("RoleCard_Ability_Main_W");
            copy.sprite = Fancy.Instance.Assets.GetSprite("RoleCard_Ability_Main_M");

            copy.transform.SetSiblingIndex(1);
            copy.AddComponent<DummyBehaviour>();
        }

        og.SetImageColor(ColorType.Wood); // Main wood container
        copy.SetImageColor(ColorType.Metal); // The metal support

        try
        {
            __instance.useButton.transform.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Fire);
        } catch {}
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupPanel), nameof(SpecialAbilityPopupPanel.Start))]
public static class SpecialAbilityPopupPanelPatch
{
    public static SpecialAbilityPopupPanel Cache;

    public static void Postfix(SpecialAbilityPopupPanel __instance)
    {
        Cache = __instance;
        var og = __instance.transform.GetChild(0).GetComponent<Image>();
        Image copy;

        if (og.GetComponent<DummyBehaviour>())
        {
            copy = __instance.transform.GetChild(0).GetComponent<Image>();
            og = __instance.transform.GetChild(1).GetComponent<Image>();
        }
        else
        {
            copy = UObject.Instantiate(og, og.transform.parent);

            og.sprite = Fancy.Instance.Assets.GetSprite("RoleCard_Ability_Main_W");
            copy.sprite = Fancy.Instance.Assets.GetSprite("RoleCard_Ability_Main_M");

            copy.transform.SetSiblingIndex(0);
            copy.AddComponent<DummyBehaviour>();
        }

        og.SetImageColor(ColorType.Wood, false); // Main wood container
        copy.SetImageColor(ColorType.Metal, false); // The metal support
        Utils.UpdateMaterials(false, __instance.roleCardPanel.CurrentFaction);
    }
}

[HarmonyPatch(typeof(RoleCardPopupPanel), nameof(RoleCardPopupPanel.SetRoleAndFaction))]
public static class RoleCardPopupControllerPatch
{
    public static void Postfix(RoleCardPopupPanel __instance)
    {
        __instance.transform.GetChild(4).GetComponent<Image>().SetImageColor(ColorType.Wood, false); // Frame
        __instance.transform.GetChild(10).GetComponent<Image>().SetImageColor(ColorType.Wood, false); // Slot count for display
        __instance.transform.GetChild(13).GetComponent<Image>().SetImageColor(ColorType.Wood, false); // Slot count prefab 1
        __instance.transform.GetChild(14).GetComponent<Image>().SetImageColor(ColorType.Wood, false); // Slot count prefab 2
        __instance.transform.GetChild(15).GetComponent<Image>().SetImageColor(ColorType.Wood, false); // Slot count prefab 3
        __instance.transform.GetChild(16).GetComponent<Image>().SetImageColor(ColorType.Wood, false); // Slot count prefab 4
        __instance.transform.GetChild(17).GetComponent<Image>().SetImageColor(ColorType.Metal, false); // Closing screw
        Utils.UpdateMaterials(false, __instance.CurrentFaction);
    }
}

[HarmonyPatch(typeof(AchievementItem), nameof(AchievementItem.SetAchievement))]
public static class AchievementItemPatch
{
    public static void Postfix(AchievementItem __instance) => __instance.transform.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
}

[HarmonyPatch(typeof(HudDockPanel), nameof(HudDockPanel.Start))]
public static class HudDockPanelPatch
{
    public static HudDockPanel Cache;

    public static void Postfix(HudDockPanel __instance)
    {
        Cache = __instance;
        __instance.GetComponent<Image>().SetImageColor(ColorType.Wood);
    }
}

[HarmonyPatch(typeof(PooledChatViewSwitcher), nameof(PooledChatViewSwitcher.Start))]
public static class PooledChatViewSwitcherPatch
{
    public static PooledChatViewSwitcher Cache;

    public static void Postfix(PooledChatViewSwitcher __instance)
    {
        Cache = __instance;
        __instance.transform.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
        var parts = __instance.transform.GetChild(1).GetChild(1);
        parts.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
        parts.GetChild(1).GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
        parts.GetChild(2).GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
        var parts2 = parts.GetChild(5).GetChild(0);
        parts2.parent.GetComponent<Image>().SetImageColor(ColorType.Metal);
        parts2.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Metal);
        parts2.GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Metal);
        var nameplate = __instance.transform.GetChild(3).GetChild(0);
        nameplate.GetComponent<Image>().SetImageColor(ColorType.Metal);
        nameplate.Find("Name").GetComponent<TextMeshProUGUI>().SetGraphicColor(ColorType.Metal);
    }
}

[HarmonyPatch(typeof(ChatInputController), nameof(ChatInputController.SetChatState))]
public static class ChatInputControllerPatch
{
    public static ChatInputController Cache;

    public static ChatMentionsProvider mentionsProvider;

    public static void Postfix(ChatInputController __instance)
    {
        Cache = __instance;
        __instance.parchmentBackgroundImage.SetImageColor(ColorType.Paper);
        __instance.chatInputText.SetGraphicColor(ColorType.Paper);
        __instance.chatInput.textComponent.SetGraphicColor(ColorType.Paper);
        __instance.chatInput.placeholder.SetGraphicColor(ColorType.Paper);
        mentionsProvider = __instance.chatInput.mentionPanel.mentionsProvider as ChatMentionsProvider;
        try
        {
            __instance.parchmentBackgroundImage.transform.GetChild(2).GetChild(2).GetComponent<Image>().SetImageColor(ColorType.Wax);
        } catch {}
    }
}

[HarmonyPatch]
public static class LobbyInfoClassicPanelAndTimerPatch
{
    [HarmonyPatch(typeof(LobbyTimer), nameof(LobbyTimer.HandleOnLobbyDataChanged))]
    [HarmonyPatch(typeof(LobbyInfoClassicPanel), nameof(LobbyInfoClassicPanel.Start))]
    public static void Postfix(MonoBehaviour __instance) => __instance.GetComponent<Image>().SetImageColor(ColorType.Wood);
}

[HarmonyPatch(typeof(HudTimeChangePanel), nameof(HudTimeChangePanel.UpdateDayNightNumber))]
public static class HudTimeChangePanelPatch
{
    public static HudTimeChangePanel Cache;

    public static void Postfix(HudTimeChangePanel __instance)
    {
        Cache = __instance;
        __instance.transform.GetChild(0).GetChild(3).GetComponent<Image>().SetImageColor(ColorType.Wood);
        __instance.transform.GetChild(1).GetChild(3).GetComponent<Image>().SetImageColor(ColorType.Wood);
    }
}

[HarmonyPatch(typeof(LobbyGameModeChoicePanel), nameof(LobbyGameModeChoicePanel.Start))]
public static class LobbyGameModeChoicePanelPatch
{
    public static void Postfix(LobbyGameModeChoicePanel __instance) =>
        __instance.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(15).GetComponent<Image>().SetImageColor(ColorType.Wood);
}

[HarmonyPatch(typeof(TosAbilityPanel), nameof(TosAbilityPanel.Start))]
public static class PatchAbilityPanel
{
    public static TosAbilityPanel Cache;

    public static void Postfix(TosAbilityPanel __instance)
    {
        Cache = __instance;
        new[] { __instance.allFilterBtn, __instance.livingFilterBtn, __instance.targetFilterBtn, __instance.factionFilterBtn, __instance.graveyardFilterBtn }
            .Do(x => x.transform.GetAllComponents<Image>()
                .Do(y => y.SetImageColor(ColorType.Wax)));

        var parent = __instance.transform.GetChild(0);
        var leather = parent.Find("ListBacking").GetComponent<Image>()!;
        var corners = parent.Find("ListCorners")?.GetComponent<Image>()!;
        var paper = parent.Find("ListPaper")?.GetComponent<Image>()!;

        if (!corners)
        {
            corners = UObject.Instantiate(leather, parent);
            corners.transform.SetSiblingIndex(1);
            corners.name = "ListCorners";
            corners.sprite = Fancy.Instance.Assets.GetSprite("PlayerList_Main_M");
        }

        if (!paper)
        {
            paper = UObject.Instantiate(leather, parent);
            paper.transform.SetSiblingIndex(2);
            paper.name = "ListPaper";
            paper.sprite = Fancy.Instance.Assets.GetSprite("PlayerList_Main_P");
        }

        if (!leather.GetComponent<DummyBehaviour>())
        {
            leather.sprite = Fancy.Instance.Assets.GetSprite("PlayerList_Main_L");
            leather.AddComponent<DummyBehaviour>();
        }

        corners.SetImageColor(ColorType.Metal);
        leather.SetImageColor(ColorType.Leather);
        paper.SetImageColor(ColorType.Paper);

        parent.Find("MinimizeUIButton").GetComponentInChildren<Image>().SetImageColor(ColorType.Metal);
    }
}

[HarmonyPatch(typeof(LobbyInfoPanel), nameof(LobbyInfoPanel.Start))]
public static class PatchLobbyInfo
{
    public static void Postfix(LobbyInfoPanel __instance)
    {
        __instance.transform.GetComponent<Image>().SetImageColor(ColorType.Wood);
        __instance.transform.GetChild(0).GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
        __instance.publicPrivateButton.transform.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
        var lobbyName = __instance.lobbyDescriptionInput.transform;
        lobbyName.GetComponent<Image>().SetImageColor(ColorType.Wood);
        lobbyName.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Metal);
        lobbyName.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().color = Color.white;
        var rightSide = __instance.transform.GetChild(0).GetChild(1);
        rightSide.GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Wood);
        rightSide.GetChild(3).GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
        __instance.notificationPanel.GetComponent<Image>().SetImageColor(ColorType.Wood);
        __instance.splatIconGood.GetComponent<Image>().SetImageColor(ColorType.Wax);
        __instance.splatIconBad.GetComponent<Image>().SetImageColor(ColorType.Wax);
    }
}

[HarmonyPatch(typeof(DialogController), nameof(DialogController.Start))]
public static class PatchDialogBox
{
    public static void Postfix(DialogController __instance)
    {
        __instance.background.GetChild(0).GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
        __instance.closeButtonClusterGO.transform.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Metal);
        __instance.closeButtonClusterGO.transform.GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Metal);
        __instance.inputConfirmClusterGO.transform.GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Metal);
        // Cannot change the color of the buttons to Wax because of the Animator resetting its Material (sadly, turning off the Animator will stop its animations if you didn't know)
    }
}

// [HarmonyPatch(typeof(SettingsController), nameof(SettingsController.Start))]
// public static class PatchSettingsController
// {
//     public static void Postfix(SettingsController __instance)
//     {
//         var modal = __instance.transform.GetChild(0).GetChild(1);
//         var tabs = modal.GetChild(0);
//         var frame = modal.GetChild(1);

//         for (var i = 0; i < 5; i++)
//         {
//             var tempTab = tabs.GetChild(i);
//             tempTab.GetComponent<Image>().SetImageColor(ColorType.Wood);
//             tempTab.GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Wood);
//         }

//         frame.GetComponent<Image>().SetImageColor(ColorType.Wood);
//         frame.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
//         frame.GetChild(1).GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Metal);
//         var bottomButtons = frame.GetChild(2).GetChild(0);
//         bottomButtons.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
//         bottomButtons.GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Wood);
//         bottomButtons.GetChild(3).GetComponent<Image>().SetImageColor(ColorType.Wood);
//         bottomButtons.GetChild(4).GetComponent<Image>().SetImageColor(ColorType.Wood);

//         // Settings Game Panel
//         var settingsGamePanel = __instance.settingsViewPanel.GetComponent<SettingsGamePanel>();

//         for (var i = 0; i < 4; i++)
//             settingsGamePanel.transform.GetChild(i).GetComponent<Image>().SetImageColor(ColorType.Wood);

//         var firstChild = settingsGamePanel.transform.GetChild(0);

//         for (var i = 1; i < 7; i++)
//         {
//             var tempHolder = firstChild.GetChild(i);
//             tempHolder.GetComponent<Image>().SetImageColor(ColorType.Metal);
//             var tempSetting = tempHolder.GetChild(1);
//             tempSetting.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Metal);
//             tempSetting.GetChild(2).GetComponent<Image>().SetImageColor(ColorType.Metal);
//         }

//         settingsGamePanel.sliderCinematics.transform.GetComponent<Image>().SetImageColor(ColorType.Metal);
//         var firstChild1 = settingsGamePanel.sliderCinematics.transform.GetChild(1);
//         firstChild1.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Paper);
//         firstChild1.GetChild(1).GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Paper);
//         settingsGamePanel.sliderMusic.transform.GetComponent<Image>().SetImageColor(ColorType.Metal);
//         var firstChild2 = settingsGamePanel.sliderMusic.transform.GetChild(1);
//         firstChild2.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Paper);
//         firstChild2.GetChild(1).GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Paper);
//         settingsGamePanel.sliderEffects.transform.GetComponent<Image>().SetImageColor(ColorType.Metal);
//         var firstChild3 = settingsGamePanel.sliderEffects.transform.GetChild(1);
//         firstChild3.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Paper);
//         firstChild3.GetChild(1).GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Paper);
//         var secondChild = settingsGamePanel.transform.GetChild(2);

//         for (var i = 1; i < 7; i++)
//         {
//             var tempHolder = secondChild.GetChild(i);
//             tempHolder.GetComponent<Image>().SetImageColor(ColorType.Metal);

//             if (i >= 3)
//                 continue;

//             var tempSetting = tempHolder.GetChild(1);
//             tempSetting.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Metal);
//             tempSetting.GetChild(2).GetComponent<Image>().SetImageColor(ColorType.Metal);
//         }

//         settingsGamePanel.transform.GetChild(3).GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Metal);

//         // Chat Game Panel
//         var chatGamePanel = frame.GetChild(5).GetComponent<ChatGamePanel>();
//         chatGamePanel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
//         var chatOptions = chatGamePanel.transform.GetChild(1);
//         var secondChatChild = chatOptions.GetChild(1);
//         chatOptions.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
//         secondChatChild.GetComponent<Image>().SetImageColor(ColorType.Wood);
//         var firstChatChild = chatOptions.GetChild(0);

//         for (var i = 1; i < 6; i++)
//         {
//             var tempHolder = firstChatChild.GetChild(i);
//             tempHolder.GetComponent<Image>().SetImageColor(ColorType.Metal);
//             var tempSetting = tempHolder.GetChild(1);
//             tempSetting.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Metal);
//             tempSetting.GetChild(2).GetComponent<Image>().SetImageColor(ColorType.Metal);
//         }

//         for (var i = 1; i < 4; i++)
//         {
//             var tempHolder = secondChatChild.GetChild(i);
//             tempHolder.GetComponent<Image>().SetImageColor(ColorType.Metal);
//             var tempSetting = tempHolder.GetChild(1);
//             tempSetting.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Metal);
//             tempSetting.GetChild(2).GetComponent<Image>().SetImageColor(ColorType.Metal);
//         }

//         // Binding Game Panel
//         var bindingGamePanel = __instance.bindingsPanel;
//         bindingGamePanel.transform.GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Wood);
//         var bindingChild = bindingGamePanel.transform.GetChild(2);
//         bindingChild.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
//         var bindingLoopChild = bindingGamePanel.transform.GetChild(2).GetChild(0).GetChild(0);

//         for (var i = 0; i < 27; i++)
//             bindingLoopChild.GetChild(i).GetComponent<Image>().SetImageColor(ColorType.Metal);

//         // Game Guide Panel
//         var gameGuidePanel = frame.GetChild(7).GetComponent<GameGuidePanel>();
//         var subBar = gameGuidePanel.transform.GetChild(0).GetChild(0);
//         subBar.GetComponent<Image>().SetImageColor(ColorType.Wood);

//         for (var i = 0; i < 3; i++)
//             subBar.GetChild(i).GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Wood);

//         for (var i = 1; i < 5; i++)
//         {
//             var tempHolder = gameGuidePanel.transform.GetChild(i);
//             tempHolder.GetComponent<Image>().SetImageColor(ColorType.Wood);
//             tempHolder.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Metal);

//             if (i == 1)
//                 tempHolder.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Metal);
//         }

//         // Social Game Panel
//         var socialGamePanel = frame.GetChild(8).GetComponent<SocialGamePanel>();
//         var twitch = socialGamePanel.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0);
//         twitch.GetComponent<Image>().SetImageColor(ColorType.Wood);
//         var firstTwitch = twitch.GetChild(0);
//         firstTwitch.GetComponent<Image>().SetImageColor(ColorType.Metal);
//         var firstTwitchFirst = firstTwitch.GetChild(1);
//         firstTwitchFirst.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Metal);
//         firstTwitchFirst.GetChild(2).GetComponent<Image>().SetImageColor(ColorType.Metal);
//         twitch.GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Metal);

//         for (var i = 2; i < 5; i++)
//         {
//             var tempHolder = twitch.GetChild(i);
//             tempHolder.GetComponent<Image>().SetImageColor(ColorType.Metal);
//             tempHolder.GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Metal);
//             tempHolder.GetChild(2).GetComponent<Image>().SetImageColor(ColorType.Wax);
//         }

//         var links = socialGamePanel.transform.GetChild(1).GetChild(0).GetChild(0);
//         links.GetComponent<Image>().SetImageColor(ColorType.Wood);

//         for (var i = 1; i < 7; i++)
//             links.GetChild(i).GetComponent<Image>().SetImageColor(ColorType.Metal);
//     }
// }

[HarmonyPatch(typeof(GameModeChoicePanelController), nameof(GameModeChoicePanelController.Start))]
public static class PatchGameModeChoices
{
    public static void Postfix(GameModeChoicePanelController __instance)
    {
        if (__instance.transform.GetComponent<DummyBehaviour>())
            return;

        __instance.transform.AddComponent<DummyBehaviour>();
        __instance.transform.GetChild(__instance.transform.childCount - 1).GetComponent<Image>().SetImageColor(ColorType.Wood);

        for (var i = 0; i < __instance.transform.childCount - 1; i++)
            __instance.transform.GetChild(i).GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Metal);

        var lobbyDescription = __instance.transform.parent.GetChild(2);
        lobbyDescription.GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Wood);
        // Cannot change the color of the Join button to Wax because of the Animator resetting its Material
    }
}

[HarmonyPatch(typeof(SafeAreaController), nameof(SafeAreaController.OnEnable))]
public static class PatchHomeMenu
{
    [HarmonyPriority(Priority.Last)]
    public static void Postfix(SafeAreaController __instance)
    {
        var leftButtons = __instance.transform.Find("LeftButtons");
        leftButtons.GetChild(leftButtons.childCount - 1).GetComponent<Image>().SetImageColor(ColorType.Wood);

        for (var i = 1; i < leftButtons.childCount - 1; i++)
            leftButtons.GetChild(i).GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Metal);

        foreach (var trans in __instance.transform.GetComponentsInChildren<Transform>(true))
        {
            if (trans.name == "GameModeChoiceElementsUI(Clone)" && !trans.GetChild(0).GetChild(0).GetComponent<DummyBehaviour>())
                PatchGameModeChoices.Postfix(trans.GetChild(0).GetChild(0).GetComponent<GameModeChoicePanelController>());
        }
    }
}

[HarmonyPatch(typeof(HomeBannerController), nameof(HomeBannerController.Start))]
public static class PatchHomeBanner
{
    public static void Postfix(HomeBannerController __instance)
    {
        __instance.usernameText.transform.parent.GetComponent<Image>().SetImageColor(ColorType.Paper);
        __instance.usernameText.SetGraphicColor(ColorType.Paper);
        __instance.townPointText.SetGraphicColor(ColorType.Paper);
    }
}

[HarmonyPatch(typeof(HomeFeaturedItemElementsPanel), nameof(HomeFeaturedItemElementsPanel.Start))]
public static class PatchFeaturedItem
{
    public static void Postfix(HomeFeaturedItemElementsPanel __instance)
    {
        var canvas = __instance.panelCanvas.transform.GetChild(0).GetChild(0);
        canvas.GetComponent<Image>().SetImageColor(ColorType.Wood);
        var titlePlate = canvas.GetChild(2).GetChild(0);
        titlePlate.GetComponent<Image>().SetImageColor(ColorType.Metal);
        titlePlate.GetChild(0).GetComponent<TextMeshProUGUI>().SetGraphicColor(ColorType.Metal);
        __instance.featuredItem.transform.GetChild(5).GetComponent<Image>().SetImageColor(ColorType.Wax);
    }
}

[HarmonyPatch(typeof(HomeDailyDealElementsPanel), nameof(HomeDailyDealElementsPanel.Start))]
public static class PatchDailyDeal
{
    public static void Postfix(HomeDailyDealElementsPanel __instance)
    {
        var canvas = __instance.panelCanvas.transform.GetChild(1).GetChild(0);
        canvas.GetComponent<Image>().SetImageColor(ColorType.Wood);
        canvas.GetChild(5).GetComponent<Image>().SetImageColor(ColorType.Metal);
        canvas.GetChild(5).GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Metal);
        var titlePlate = canvas.GetChild(4).GetChild(0);
        titlePlate.GetComponent<Image>().SetImageColor(ColorType.Metal);
        titlePlate.GetChild(0).GetComponent<TextMeshProUGUI>().SetGraphicColor(ColorType.Metal);
        canvas.GetChild(2).GetChild(0).GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Wax);
    }
}

[HarmonyPatch(typeof(RoleMenuPopupElementsPanel), nameof(RoleMenuPopupElementsPanel.Start))]
public static class PatchRoleMenuPopup
{
    public static void Postfix(RoleMenuPopupElementsPanel __instance)
    {
        var template = __instance.RoleMenuPopupTemplate;
        template.transform.GetChild(0).GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
        template.transform.GetChild(1).GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Metal);

        foreach (var button in template.buttons)
            button.transform.GetComponent<Image>().SetImageColor(ColorType.Metal);
    }
}

[HarmonyPatch(typeof(KeywordMenuPopupElementsPanel), nameof(KeywordMenuPopupElementsPanel.Start))]
public static class PatchKeywordMenuPopup
{
    public static void Postfix(KeywordMenuPopupElementsPanel __instance)
    {
        var template = __instance.KeywordMenuPopupTemplate;
        template.transform.GetChild(0).GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
        template.transform.GetChild(1).GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Metal);
        template.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Metal);
        template.transform.GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Metal);
    }
}

[HarmonyPatch(typeof(KeywordTooltipElementsPanel), nameof(KeywordTooltipElementsPanel.Start))]
public static class PatchKeywordTooltip
{
    public static void Postfix(KeywordTooltipElementsPanel __instance) => __instance.KeywordTooltipTemplate.transform.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
}

[HarmonyPatch(typeof(PlayerPopupElementsPanel), nameof(PlayerPopupElementsPanel.Start))]
public static class PatchPlayerPopup
{
    public static void Postfix(PlayerPopupElementsPanel __instance)
    {
        var template = __instance.PlayerPopupTemplate;
        template.transform.GetChild(2).GetComponent<Image>().SetImageColor(ColorType.Wood);
        template.transform.GetChild(3).GetComponent<Image>().SetImageColor(ColorType.Metal);
        var buttons = template.transform.GetChild(1).GetChild(0);

        for (var i = 0; i < 15; i++)
        {
            var tempHolder = buttons.GetChild(i);

            switch (i)
            {
                case 2:
                {
                    tempHolder.GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Wax);
                    break;
                }
                case 3:
                {
                    tempHolder.GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Metal);
                    break;
                }
                default:
                {
                    var tempButton = tempHolder.GetChild(1).GetChild(0);
                    tempButton.GetComponent<Image>().SetImageColor(ColorType.Metal);

                    if (i != 6)
                        tempButton.GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Wax);

                    if (i == 12)
                    {
                        tempButton = tempHolder.GetChild(2).GetChild(0);
                        tempButton.GetComponent<Image>().SetImageColor(ColorType.Metal);
                        tempButton.GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Wax);
                    }

                    break;
                }
            }
        }
    }
}

[HarmonyPatch(typeof(MentionPanel), nameof(MentionPanel.Start))]
public static class PatchMentionPanel
{
    public static void Postfix(MentionPanel __instance)
    {
        var root = __instance.transform;

        if (root.childCount == 0)
            return;

        var first = root.GetChild(0);

        if (first.childCount > 2)
        {
            var image = first.GetChild(2).GetComponent<Image>();
            image?.SetImageColor(ColorType.Wood);
        }

        if (first.childCount <= 0)
            return;

        var second = first.GetChild(0);

        if (second.childCount <= 0)
            return;

        var third = second.GetChild(0);

        if (third.childCount <= 0)
            return;

        var template = third.GetChild(0);
        template.GetComponent<Image>()?.SetImageColor(ColorType.Metal);

        if (template.childCount > 1)
            template.GetChild(1).GetComponent<Image>()?.SetImageColor(ColorType.Metal);
    }
}

// [HarmonyPatch(typeof(CustomizationPanel), nameof(CustomizationPanel.Start))]
// public static class PatchCustomizationPanel
// {
//     public static void Postfix(CustomizationPanel __instance)
//     {
//         // Main Panel
//         foreach (var tab in __instance.tabAnims)
//             tab.transform.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);

//         var mainPanel = __instance.transform.GetChild(1);
//         mainPanel.GetChild(1).GetChild(1).GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
//         mainPanel.GetChild(2).GetComponent<Image>().SetImageColor(ColorType.Wood);
//         var title = mainPanel.GetChild(4);
//         title.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Metal);
//         title.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().SetGraphicColor(ColorType.Metal);
//         title.GetChild(1).GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Metal);
//         title.GetChild(1).GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Metal);

//         // Tome
//         var tome = __instance.transform.GetChild(0);
//         tome.GetComponent<Image>().SetImageColor(ColorType.Wood);
//         tome.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
//         tome.GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Metal);

//         for (var i = 2; i < 5; i++)
//             tome.GetChild(i).GetComponent<TextMeshProUGUI>().SetGraphicColor(ColorType.Metal);

//         var buyContainer = tome.GetChild(5);
//         buyContainer.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wax);
//         buyContainer.GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Wax);
//         tome.GetChild(7).GetComponent<Image>().SetImageColor(ColorType.Wood);
//     }
// }

// [HarmonyPatch(typeof(RoleSelectionPanel), nameof(RoleSelectionPanel.Start))]
// public static class PatchRoleSelectionPanel
// {
//     public static void Postfix(RoleSelectionPanel __instance)
//     {
//         try
//         {
//             foreach (var tab in __instance.tabInstances)
//             {
//                 tab.transform.GetComponent<Image>().SetImageColor(ColorType.Wood);
//                 tab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetGraphicColor(ColorType.Wood);
//             }

//             __instance.transform.GetChild(1).GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
//             __instance.helpTipDecks.transform.GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Metal);
//             __instance.helpTipDecks.transform.GetChild(2).GetComponent<Image>().SetImageColor(ColorType.Metal);
//             __instance.helpTipRoles.transform.GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Metal);
//             __instance.helpTipRoles.transform.GetChild(2).GetComponent<Image>().SetImageColor(ColorType.Metal);
//             var roles = __instance.transform.GetChild(4);
//             roles.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood);
//             roles.GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Wood);
//             var top = roles.GetChild(5);
//             top.GetComponent<Image>().SetImageColor(ColorType.Wood);
//             var title = top.GetChild(0).GetChild(0);
//             title.GetComponent<Image>().SetImageColor(ColorType.Metal);
//             title.GetChild(0).GetComponent<TextMeshProUGUI>().SetGraphicColor(ColorType.Metal);
//             title.GetChild(1).GetComponent<TextMeshProUGUI>().SetGraphicColor(ColorType.Metal);
//             var template = __instance.cardListItemTemplate;
//             var roleTitle = template.transform.GetChild(1);
//             roleTitle.GetComponent<Image>().SetImageColor(ColorType.Metal);
//             roleTitle.GetChild(0).GetComponent<TextMeshProUGUI>().SetGraphicColor(ColorType.Metal);
//             template.transform.GetChild(3).GetComponent<Image>().SetImageColor(ColorType.Wax);

//             foreach (var cardListItem in __instance.roleCardListItems)
//             {
//                 roleTitle = cardListItem.transform.GetChild(1);
//                 roleTitle.GetComponent<Image>().SetImageColor(ColorType.Metal);
//                 roleTitle.GetChild(0).GetComponent<TextMeshProUGUI>().SetGraphicColor(ColorType.Metal);
//                 cardListItem.transform.GetChild(3).GetComponent<Image>().SetImageColor(ColorType.Wax);
//             }

//             __instance.transform.GetChild(5).GetComponent<Image>().SetImageColor(ColorType.Metal);
//         }
//         catch (Exception e)
//         {
//             Debug.LogWarning(e);
//         }
//     }
// }

[HarmonyPatch(typeof(HudRoleListAndGraveyardController))]
public static class HudRoleListAndGraveyardControllerPatch
{
    public static HudRoleListAndGraveyardController Cache;

    private static Image OpenWood;
    private static Image RoleListWood;
    private static Image GraveyardWood;
    private static Image PanelImage;

    [HarmonyPatch(nameof(HudRoleListAndGraveyardController.Start)), HarmonyPostfix]
    public static void Postfix1(HudRoleListAndGraveyardController __instance)
    {
        Cache = __instance;

        PanelImage = __instance.GetComponent<Image>();
        PanelImage.SetImageColor(ColorType.Wood);
        OpenWood = __instance.transform.parent.Find("OpenWood")?.GetComponent<Image>();

        if (!OpenWood)
        {
            PanelImage.sprite = Fancy.Instance.Assets.GetSprite("RoleList_Open_M");
            OpenWood = UObject.Instantiate(PanelImage, __instance.transform.parent);
            OpenWood.transform.SetSiblingIndex(PanelImage.transform.GetSiblingIndex());
            OpenWood.transform.ClearChildren();
            OpenWood.GetComponent<HudRoleListAndGraveyardController>().Destroy();
            OpenWood.GetComponent<HorizontalLayoutGroup>().Destroy();
            OpenWood.name = "OpenWood";
            OpenWood.sprite = Fancy.Instance.Assets.GetSprite("RoleList_Open_W");
        }

        PanelImage.SetImageColor(ColorType.Metal);
        OpenWood.SetImageColor(ColorType.Wood);

        var roleList = __instance.roleListPanelBackground;
        roleList.transform.GetComponent<Image>("RolesButton").SetImageColor(ColorType.Wax);
        roleList.transform.GetComponent<Image>("BansButton").SetImageColor(ColorType.Wax);
        roleList.transform.GetComponent<Image>("Sprite").SetImageColor(ColorType.Metal);
        roleList.transform.GetComponent<TextMeshProUGUI>("RoleListTitle").SetGraphicColor(ColorType.Metal);
        RoleListWood = roleList.transform.parent.Find("RoleListWood")?.GetComponent<Image>();

        if (!RoleListWood)
        {
            roleList.sprite = Fancy.Instance.Assets.GetSprite("RoleList_Closed_M");
            RoleListWood = UObject.Instantiate(roleList, __instance.transform.parent);
            RoleListWood.transform.SetSiblingIndex(roleList.transform.GetSiblingIndex());
            RoleListWood.transform.ClearChildren();
            RoleListWood.name = "RoleListWood";
            RoleListWood.sprite = Fancy.Instance.Assets.GetSprite("RoleList_Closed_W");
        }

        roleList.SetImageColor(ColorType.Metal);
        RoleListWood.SetImageColor(ColorType.Wood);

        var graveyard = __instance.graveyardPanelBackground;
        graveyard.transform.GetComponent<Image>("Sprite").SetImageColor(ColorType.Metal);

        var graveyardListTitle = graveyard.transform.GetComponent<TextMeshProUGUI>("RoleListTitle")!;
        graveyardListTitle.enableVertexGradient = false;
        graveyardListTitle.SetGraphicColor(ColorType.Metal);

        GraveyardWood = graveyard.transform.parent.Find("GraveyardWood")?.GetComponent<Image>();

        if (!GraveyardWood)
        {
            graveyard.sprite = Fancy.Instance.Assets.GetSprite("Graveyard_M");
            GraveyardWood = UObject.Instantiate(graveyard, __instance.transform.parent);
            GraveyardWood.transform.SetSiblingIndex(graveyard.transform.GetSiblingIndex());
            GraveyardWood.transform.ClearChildren();
            GraveyardWood.name = "GraveyardWood";
            GraveyardWood.sprite = Fancy.Instance.Assets.GetSprite("Graveyard_W");
        }

        graveyard.SetImageColor(ColorType.Metal);
        GraveyardWood.SetImageColor(ColorType.Wood);
    }

    [HarmonyPatch(nameof(HudRoleListAndGraveyardController.ValidateVisibility)), HarmonyPostfix]
    public static void Postfix2(HudRoleListAndGraveyardController __instance)
    {
        if (OpenWood)
            OpenWood.enabled = PanelImage.enabled;

        if (RoleListWood)
            RoleListWood.enabled = __instance.roleListPanelBackground.enabled;

        if (GraveyardWood)
            GraveyardWood.enabled = __instance.graveyardPanelBackground.enabled;
    }
}

[HarmonyPatch(typeof(Tos2GameBrowserController), nameof(Tos2GameBrowserController.Start))]
public static class Tos2GameBrowserControllerPatch
{
    public static void Postfix(Tos2GameBrowserController __instance)
    {
        // The buttons at the bottom have the animator component that resets the material, rip
        __instance.transform.GetComponent<Image>("Refresh").SetImageColor(ColorType.Metal);
        __instance.transform.GetComponent<Image>("Frame").SetImageColor(ColorType.Wood);
        var nameplate = __instance.transform.GetComponent<Image>("TopGold")!;
        nameplate.SetImageColor(ColorType.Metal);
        nameplate.GetComponentInChildren<TextMeshProUGUI>().SetGraphicColor(ColorType.Metal);
    }
}

[HarmonyPatch(typeof(RoleDeckPanelController))]
public static class RoleDeckPanelControllerPatch
{
    private static RectTransform MetalTransform;
    private static RectTransform PaperTransform;
    public static bool Pandora;
    public static bool Compliance;

    [HarmonyPatch(nameof(RoleDeckPanelController.Start)), HarmonyPostfix]
    public static void StartPostfix(RoleDeckPanelController __instance)
    {
        var baseLeather = __instance.deckView.GetComponent<Image>();
        baseLeather.type = Image.Type.Sliced;
        baseLeather.pixelsPerUnitMultiplier = 2.5f;
        Image metal;
        Image paper;

        if (baseLeather.GetComponent<DummyBehaviour>())
        {
            metal = __instance.transform.GetComponent<Image>("DeckMetal");
            paper = __instance.transform.GetComponent<Image>("DeckPaper");
        }
        else
        {
            metal = UObject.Instantiate(baseLeather, baseLeather.transform.parent);
            metal.transform.ClearChildren();
            metal.transform.SetSiblingIndex(0);
            metal.sprite = Fancy.Instance.Assets.GetSprite("RoleList_M");
            metal.name = "DeckMetal";
            metal.type = Image.Type.Sliced;
            metal.pixelsPerUnitMultiplier = 2.5f;
            MetalTransform = metal.GetComponent<RectTransform>();

            paper = UObject.Instantiate(baseLeather, baseLeather.transform.parent);
            paper.transform.ClearChildren();
            paper.transform.SetSiblingIndex(1);
            paper.sprite = Fancy.Instance.Assets.GetSprite("RoleList_P");
            paper.name = "DeckPaper";
            paper.type = Image.Type.Sliced;
            paper.pixelsPerUnitMultiplier = 2.5f;
            PaperTransform = paper.GetComponent<RectTransform>();

            baseLeather.sprite = Fancy.Instance.Assets.GetSprite("RoleList_L");
            baseLeather.AddComponent<DummyBehaviour>();
        }

        baseLeather.SetImageColor(ColorType.Leather);
        metal.SetImageColor(ColorType.Metal);
        paper.SetImageColor(ColorType.Paper);
        __instance.transform.GetComponent<Image>("RolesIcon").SetImageColor(ColorType.Paper);
        __instance.transform.GetComponent<Image>("PlayersIcon").SetImageColor(ColorType.Wax);
        __instance.transform.GetComponent<Image>("RoleTypeSeparator").SetImageColor(ColorType.Leather);
        __instance.transform.GetComponent<TextMeshProUGUI>("LobbyInstructionsText").SetGraphicColor(ColorType.Paper);
        __instance.splatIcon.GetComponent<Image>().SetImageColor(ColorType.Wax);

        if (!Constants.EnableIcons())
            return;

        var town = __instance.transform.GetComponent<Image>("TownIcon");
        var coven = __instance.transform.GetComponent<Image>("CovenIcon");
        var neut = __instance.transform.GetComponent<Image>("NeutralIcon");
        var any = __instance.transform.GetComponent<Image>("AnyIcon");
        var modifier = __instance.transform.GetComponent<Image>("ModifierIcon");
        var sprite = GetSprite("Town");

        if (sprite.IsValid() && town)
            town.sprite = sprite;

        sprite = GetSprite("Coven");

        if (sprite.IsValid() && coven)
            coven.sprite = sprite;

        sprite = GetSprite("Neutral");

        if (sprite.IsValid() && neut)
            neut.sprite = sprite;

        sprite = GetSprite("Any");

        if (sprite.IsValid() && any)
            any.sprite = sprite;

        sprite = GetSprite("Modifier");

        if (sprite.IsValid() && modifier)
            modifier.sprite = sprite;

        Pandora = Constants.IsPandora();
        Compliance = Constants.IsCompliance();
        __instance.AdjustSizeBasedOnRolesAdded();
    }

    [HarmonyPatch(nameof(RoleDeckPanelController.AdjustSizeBasedOnRolesAdded)), HarmonyPostfix]
    public static void AdjustSizeBasedOnRolesAddedPostfix(RoleDeckPanelController __instance)
    {
        if (__instance.roleDeckListItems.Count < 10 && !Fancy.TallRoleDeck.Value)
            return;

        var component = __instance.deckView.GetComponent<RectTransform>();
        var viewport = __instance.deckView.transform.GetChild(3).GetChild(0) as RectTransform;
        var bitchAssMask = viewport.GetComponent<RectMask2D>();
        var num3 = 0f;
        var ySize = __instance.startTop;
        var lastActiveDeckItem = __instance.deckListItemTemplate;

        for (var i = __instance.roleDeckListItems.Count - 1; !lastActiveDeckItem.isActiveAndEnabled && i > -1; i--)
            lastActiveDeckItem = __instance.roleDeckListItems[i];

        if (Fancy.TallRoleDeck.Value)
            ySize = 720f;
        else if (-lastActiveDeckItem.transform.localPosition.y + 50 > viewport.rect.yMax)
        {
            for (var num = 1; ySize + viewport.rect.yMax < -lastActiveDeckItem.transform.localPosition.y + 50 && ySize < 720f; num++)
            {
                var num2 = num * 40;
                num3 = num * 0.04f;
                ySize = Mathf.Min(__instance.startTop + num2, 720f);
            }
        }

        component.offsetMax = new Vector2(component.offsetMax.x, ySize);
        bitchAssMask.padding = new Vector4(0f, -ySize, 0f, 0f);

        __instance.scaler.matchWidthOrHeight = Fancy.TallRoleDeck.Value ? 1f : 0.5f + num3;

        if (MetalTransform && PaperTransform)
            MetalTransform.offsetMax = PaperTransform.offsetMax = component.offsetMax;
    }
}

[HarmonyPatch(typeof(GameBrowserRoleDeck), nameof(GameBrowserRoleDeck.Start))]
public static class GameBrowserRoleDeckPatch
{
    public static void Postfix(GameBrowserRoleDeck __instance)
    {
        var baseLeather = __instance.transform.GetComponent<Image>("DeckView")!;
        Image metal;
        Image paper;

        if (baseLeather.GetComponent<DummyBehaviour>())
        {
            metal = __instance.transform.GetComponent<Image>("DeckMetal");
            paper = __instance.transform.GetComponent<Image>("DeckPaper");
        }
        else
        {
            metal = UObject.Instantiate(baseLeather, baseLeather.transform.parent);
            metal.GetComponent<ScrollRect>().Destroy();
            metal.transform.ClearChildren();
            metal.transform.SetSiblingIndex(0);
            metal.sprite = Fancy.Instance.Assets.GetSprite("RoleList_M");
            metal.name = "DeckMetal";

            paper = UObject.Instantiate(baseLeather, baseLeather.transform.parent);
            paper.GetComponent<ScrollRect>().Destroy();
            paper.transform.ClearChildren();
            paper.transform.SetSiblingIndex(1);
            paper.sprite = Fancy.Instance.Assets.GetSprite("RoleList_P");
            paper.name = "DeckPaper";

            baseLeather.sprite = Fancy.Instance.Assets.GetSprite("RoleList_L");
            baseLeather.AddComponent<DummyBehaviour>();
        }

        baseLeather.SetImageColor(ColorType.Leather);
        metal.SetImageColor(ColorType.Metal);
        paper.SetImageColor(ColorType.Paper);
    }
}

[HarmonyPatch(typeof(NavigationController), nameof(NavigationController.ShowSceneWithMapSelection))]
public static class NavigationControllerPatch
{
    public static void Postfix(string baseSceneName)
    {
        try
        {
            Utils.UpdateMaterials(skipFactionCheck: baseSceneName is not (SceneNames.GAME or SceneNames.GAME_BASE));
        }
        catch { }
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupNecromancerRetributionistListItem), nameof(SpecialAbilityPopupNecromancerRetributionistListItem.SetData))]
public static class ReanimateMenuListPatch
{
    public static void Postfix(
        SpecialAbilityPopupNecromancerRetributionistListItem __instance,
        bool hasChoice1,
        bool hasChoice2)
    {
        if (hasChoice1 && __instance.choiceButton)
        {
            foreach (var img in __instance.choiceButton.GetComponentsInChildren<Image>(true))
            {
                if (img == __instance.choiceSprite)
                    continue;

                img.SetImageColor(ColorType.Wax);
            }
        }

        if (hasChoice2 && __instance.choice2Button)
        {
            foreach (var img in __instance.choice2Button.GetComponentsInChildren<Image>(true))
            {
                if (img == __instance.choice2Sprite)
                    continue;

                img.SetImageColor(ColorType.Wax);
                img.color *= 0.75f;
            }
        }
    }
}

[HarmonyPatch( typeof(SpecialAbilityPopupNecromancerRetributionist), nameof(SpecialAbilityPopupNecromancerRetributionist.Start))]
public static class ReanimateMenuPatches
{
    static void Postfix(SpecialAbilityPopupNecromancerRetributionist __instance)
    {
        var background = __instance.transform.Find("Background");
        if (background == null)
            return;

        var frame = background.Find("Frame");
        if (frame != null)
        {
            var frameImg = frame.GetComponent<Image>();
            if (frameImg != null)
                frameImg.SetImageColor(ColorType.Wood);
        }

        var close = background.Find("CloseButton");
        if (close != null)
        {
            foreach (var img in close.GetComponentsInChildren<Image>(true))
                img.SetImageColor(ColorType.Metal);
        }
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupDayConfirm), nameof(SpecialAbilityPopupDayConfirm.Start))]
public static class DayConfirmPatch
{
    static void Postfix(SpecialAbilityPopupDayConfirm __instance)
    {
        var root = __instance.transform;

        var woodFrame = root.Find("Background/Frame/Frame");
        if (woodFrame != null)
        {
            var img = woodFrame.GetComponent<Image>();
            if (img != null)
                img.SetImageColor(ColorType.Wood);
        }

        var closeButton = root.Find("Background/CloseButton");
        if (closeButton != null)
        {
            foreach (var img in closeButton.GetComponentsInChildren<Image>(true))
                img.SetImageColor(ColorType.Metal);
        }

        var confirmPopup = root.Find("ConfirmPopup");
        if (confirmPopup != null)
        {
            foreach (var img in confirmPopup.GetComponentsInChildren<Image>(true))
            {
                img.SetImageColor(ColorType.Wood);
            }

            var confirmButton = confirmPopup.Find("ConfirmButton");
            if (confirmButton != null)
            {
                foreach (var img in confirmButton.GetComponentsInChildren<Image>(true))
                    img.SetImageColor(ColorType.Wax);
            }

            var cancelButton = confirmPopup.Find("CancelButton");
            if (cancelButton != null)
            {
                foreach (var img in cancelButton.GetComponentsInChildren<Image>(true))
                    img.SetImageColor(ColorType.Wax);
            }
        }
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupDayConfirmListItem), nameof(SpecialAbilityPopupDayConfirmListItem.SetData))]
public static class DayConfirmListPatch
{
    static void Postfix(SpecialAbilityPopupDayConfirmListItem __instance)
    {
        var backing = __instance.transform
            .Find("AbilityButton/Backing");

        if (backing == null)
            return;

        var img = backing.GetComponent<Image>();
        if (img == null)
            return;

		img.SetImageColor(ColorType.Wax);
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupGeneric), nameof(SpecialAbilityPopupGeneric.Start))]
public static class GenericSpecialAbilityPatch
{
    static void Postfix(SpecialAbilityPopupGeneric __instance)
    {
        var root = __instance.transform;

        var frame = root.Find("Background/Frame");
        if (frame != null)
        {
            var img = frame.GetComponent<Image>();
            if (img != null)
                img.SetImageColor(ColorType.Wood);
        }

        var closeButton = root.Find("Background/CloseButton");
		if (closeButton != null)
		{
			foreach (var img in closeButton.GetComponentsInChildren<Image>(true))
			{
				img.SetImageColor(ColorType.Metal);
			}
		}

    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupGenericListItem), nameof(SpecialAbilityPopupGenericListItem.SetData))]
public static class GenericSpecialAbilityListPatch
{
    static void Postfix(SpecialAbilityPopupGenericListItem __instance)
    {
        var backings = __instance.transform
            .Find("AbilityButtons");

        if (backings == null)
            return;

        foreach (Transform child in backings)
        {
            if (child.name != "AbilityButton")
                continue;

            var backing = child.Find("Backing");
            if (backing == null)
                continue;

            var img = backing.GetComponent<Image>();
            if (img != null)
                img.SetImageColor(ColorType.Wax);
        }
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupGenericDualTarget), nameof(SpecialAbilityPopupGenericDualTarget.Start))]
public static class DualTargetAbilityPatch
{
    static void Postfix(SpecialAbilityPopupGenericDualTarget __instance)
    {
        var root = __instance.transform;
        var frame = root.Find("Background/Frame");
        if (frame != null)
        {
            var img = frame.GetComponent<Image>();
            if (img != null)
                img.SetImageColor(ColorType.Wood);
        }

        var closeButton = root.Find("Background/CloseButton");
        if (closeButton != null)
        {
            foreach (var img in closeButton.GetComponentsInChildren<Image>(true))
                img.SetImageColor(ColorType.Metal);
        }
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupGenericDualTargetListItem), nameof(SpecialAbilityPopupGenericDualTargetListItem.SetData))]
public static class DualTargetAbilityListPatch
{
    static void Postfix(SpecialAbilityPopupGenericDualTargetListItem __instance)
    {
        var abilityButtons = __instance.transform
            .Find("AbilityButtons");

        if (abilityButtons == null)
            return;

        foreach (Transform btn in abilityButtons)
        {
            if (btn.name != "AbilityButton" || btn.name != "AbilityButton2")
                continue;

            var backing = btn.Find("Backing");
            if (backing == null)
                continue;

            var img = backing.GetComponent<Image>();
            if (img != null)
                img.SetImageColor(ColorType.Wax);
        }
    }
}
