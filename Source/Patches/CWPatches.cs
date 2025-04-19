using Game.Achievements;

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

        if (Constants.GetMainUIThemeType() == UITheme.Faction)
        {
            var pooledChat = PooledChatViewSwitcherPatch.Cache;
            var chatInput = ChatInputControllerPatch.Cache;
            var dayNight = HudTimeChangePanelPatch.Cache;
            var abilityPanel = PatchAbilityPanel.Cache;
            var specialAbility = SpecialAbilityPanelPatch1.Cache;
            var specialAbilityPopup = SpecialAbilityPopupPanelPatch.Cache;
            var hudDock = HudDockPanelPatch.Cache;
            var rLGY = HudRoleListAndGraveyardControllerPatch.Cache;

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

            if (rLGY)
                HudRoleListAndGraveyardControllerPatch.Postfix1(rLGY);
        }

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

            og.sprite = Fancy.Assets.GetSprite("RoleCard_Ability_Main_W");
            copy.sprite = Fancy.Assets.GetSprite("RoleCard_Ability_Main_M");

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

            og.sprite = Fancy.Assets.GetSprite("RoleCard_Ability_Main_W");
            copy.sprite = Fancy.Assets.GetSprite("RoleCard_Ability_Main_M");

            copy.transform.SetSiblingIndex(0);
            copy.AddComponent<DummyBehaviour>();
        }

        og.SetImageColor(ColorType.Wood); // Main wood container
        copy.SetImageColor(ColorType.Metal); // The metal support
    }
}

[HarmonyPatch(typeof(RoleCardPopupPanel), nameof(RoleCardPopupPanel.SetRoleAndFaction))]
public static class RoleCardPopupControllerPatch
{
    public static void Postfix(RoleCardPopupPanel __instance)
    {
        __instance.transform.GetChild(4).GetComponent<Image>().SetImageColor(ColorType.Wood); // Frame
        __instance.transform.GetChild(10).GetComponent<Image>().SetImageColor(ColorType.Wood); // Slot count for display
        __instance.transform.GetChild(13).GetComponent<Image>().SetImageColor(ColorType.Wood); // Slot count prefab 1
        __instance.transform.GetChild(14).GetComponent<Image>().SetImageColor(ColorType.Wood); // Slot count prefab 2
        __instance.transform.GetChild(15).GetComponent<Image>().SetImageColor(ColorType.Wood); // Slot count prefab 3
        __instance.transform.GetChild(16).GetComponent<Image>().SetImageColor(ColorType.Wood); // Slot count prefab 4
        __instance.transform.GetChild(17).GetComponent<Image>().SetImageColor(ColorType.Metal); // Closing screw
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

    public static void Postfix(ChatInputController __instance)
    {
        Cache = __instance;
        __instance.parchmentBackgroundImage.SetImageColor(ColorType.Paper);
        __instance.chatInputText.SetGraphicColor(ColorType.Paper);
        __instance.chatInput.textComponent.SetGraphicColor(ColorType.Paper);
        __instance.chatInput.placeholder.SetGraphicColor(ColorType.Paper);

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
    public static void Postfix(LobbyGameModeChoicePanel __instance) => __instance.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(15).GetComponent<Image>().SetImageColor(ColorType.Wood);
}

[HarmonyPatch(typeof(TosAbilityPanel), nameof(TosAbilityPanel.Start))]
public static class PatchAbilityPanel
{
    public static TosAbilityPanel Cache;

    public static void Postfix(TosAbilityPanel __instance)
    {
        Cache = __instance;
        new[] { __instance.allFilterBtn, __instance.livingFilterBtn, __instance.targetFilterBtn, __instance.factionFilterBtn, __instance.graveyardFilterBtn }
            .ForEach(x => x.transform.GetAllComponents<Image>()
                .ForEach(y => y.SetImageColor(ColorType.Wax)));

        var parent = __instance.transform.GetChild(0);
        var leather = parent.Find("ListBacking").GetComponent<Image>()!;
        var corners = parent.Find("ListCorners")?.GetComponent<Image>()!;
        var paper = parent.Find("ListPaper")?.GetComponent<Image>()!;

        if (!corners)
        {
            corners = UObject.Instantiate(leather, parent);
            corners.transform.SetSiblingIndex(1);
            corners.name = "ListCorners";
            corners.sprite = Fancy.Assets.GetSprite("PlayerList_Main_M");
        }

        if (!paper)
        {
            paper = UObject.Instantiate(leather, parent);
            paper.transform.SetSiblingIndex(2);
            paper.name = "ListPaper";
            paper.sprite = Fancy.Assets.GetSprite("PlayerList_Main_P");
        }

        if (!leather.GetComponent<DummyBehaviour>())
        {
            leather.sprite = Fancy.Assets.GetSprite("PlayerList_Main_L");
            leather.AddComponent<DummyBehaviour>();
        }

        corners.SetImageColor(ColorType.Metal);
        leather.SetImageColor(ColorType.Leather);
        paper.SetImageColor(ColorType.Paper);

        parent.Find("MinimizeUIButton").GetComponentInChildren<Image>().SetImageColor(ColorType.Metal);
    }
}

// HudRoleListPanel - Roles = 3, Bans = 4

[HarmonyPatch(typeof(HudRoleListAndGraveyardController), nameof(HudRoleListAndGraveyardController.Start))]
public static class HudRoleListAndGraveyardControllerPatch
{
    public static HudRoleListAndGraveyardController Cache;

    private static Image OpenWood;
    private static Image RoleListWood;
    private static Image GraveyardWood;

    [HarmonyPatch(nameof(HudRoleListAndGraveyardController.Start)), HarmonyPostfix]
    public static void Postfix1(HudRoleListAndGraveyardController __instance)
    {
        Cache = __instance;

        var template1 = __instance.GetComponent<Image>();
        template1.SetImageColor(ColorType.Wood);
        OpenWood = __instance.transform.parent.Find("OpenWood")?.GetComponent<Image>();

        if (!OpenWood)
        {
            template1.sprite = Fancy.Assets.GetSprite("RoleList_Open_M");
            OpenWood = UObject.Instantiate(template1, __instance.transform.parent);
            OpenWood.transform.SetSiblingIndex(template1.transform.GetSiblingIndex());
            OpenWood.transform.ClearChildren();
            OpenWood.GetComponent<HudRoleListAndGraveyardController>().Destroy();
            OpenWood.GetComponent<HorizontalLayoutGroup>().Destroy();
            OpenWood.name = "OpenWood";
            OpenWood.sprite = Fancy.Assets.GetSprite("RoleList_Open_W");
        }

        template1.SetImageColor(ColorType.Metal);
        OpenWood.SetImageColor(ColorType.Wood);

        var roleList = __instance.roleListPanelBackground;
        roleList.transform.GetComponent<Image>("RolesButton").SetImageColor(ColorType.Wax);
        roleList.transform.GetComponent<Image>("BansButton").SetImageColor(ColorType.Wax);
        roleList.transform.GetComponent<Image>("Sprite").SetImageColor(ColorType.Metal);
        // roleList.transform.GetComponent<TextMeshProUGUI>("RoleListTitle").SetGraphicColor(ColorType.Metal);
        roleList.transform.Find("RoleListTitle").gameObject.SetActive(false);
        RoleListWood = roleList.transform.parent.Find("RoleListWood")?.GetComponent<Image>();

        if (!RoleListWood)
        {
            roleList.sprite = Fancy.Assets.GetSprite("RoleList_Closed_M");
            RoleListWood = UObject.Instantiate(roleList, __instance.transform.parent);
            RoleListWood.transform.SetSiblingIndex(roleList.transform.GetSiblingIndex());
            RoleListWood.transform.ClearChildren();
            RoleListWood.name = "RoleListWood";
            RoleListWood.sprite = Fancy.Assets.GetSprite("RoleList_Closed_W");
        }

        roleList.SetImageColor(ColorType.Metal);
        RoleListWood.SetImageColor(ColorType.Wood);

        var graveyard = __instance.graveyardPanelBackground;
        graveyard.transform.GetComponent<Image>("Sprite").SetImageColor(ColorType.Metal);
        // graveyard.GetComponentsInChildren<TextMeshProUGUI>(true).FirstOrDefault(t => t.name == "RoleListTitle").SetGraphicColor(ColorType.Metal);
        graveyard.GetComponentsInChildren<TextMeshProUGUI>(true).FirstOrDefault(t => t.name == "RoleListTitle")?.gameObject.SetActive(false);

        if (!GraveyardWood)
        {
            graveyard.sprite = Fancy.Assets.GetSprite("Graveyard_M");
            GraveyardWood = UObject.Instantiate(graveyard, __instance.transform.parent);
            GraveyardWood.transform.SetSiblingIndex(graveyard.transform.GetSiblingIndex());
            GraveyardWood.transform.ClearChildren();
            GraveyardWood.name = "GraveyardWood";
            GraveyardWood.sprite = Fancy.Assets.GetSprite("Graveyard_W");
        }

        graveyard.SetImageColor(ColorType.Metal);
        GraveyardWood.SetImageColor(ColorType.Wood);

    }

    [HarmonyPatch(nameof(HudRoleListAndGraveyardController.ValidateVisibility)), HarmonyPostfix]
    public static void Postfix2(HudRoleListAndGraveyardController __instance)
    {
        if (OpenWood)
            OpenWood.enabled = __instance.GetComponent<Image>().enabled;

        if (RoleListWood)
            RoleListWood.enabled = __instance.roleListPanelBackground.enabled;

        if (GraveyardWood)
            GraveyardWood.enabled = __instance.graveyardPanelBackground.enabled;
    }
}

// [HarmonyPatch(typeof(SafeAreaController), nameof(SafeAreaController.OnEnable))]
// public static class SafeAreaController_OnEnable
// {
//     public static void Postfix(SafeAreaController __instance)
//     {
//         if (Pepper.IsLobbyOrGamePhase())
//             return;
//
//         var parts = __instance.transform.GetChild(12);
//         parts.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Wood); // Wood frame at the back
//         parts.GetChild(13).GetComponent<Image>().SetImageColor(ColorType.Wood); // Wood frame in the front
//     }
// }