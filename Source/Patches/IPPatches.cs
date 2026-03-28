using System.Text.RegularExpressions;
using Game.Chat.Decoders;
using Game.Services;
using Game.Simulation;
using Home.Services;
using Home.Shared;
using Mentions;
using SalemModLoader;
using Server.Shared.Extensions;
using Server.Shared.Messages;
using Server.Shared.State.Chat;
using AbilityType = Game.Interface.TosAbilityPanelListItem.OverrideAbilityType;
using Game.Chat;
using UnityEngine;
using UnityEngine.UI;

namespace FancyUI.Patches;

[HarmonyPatch(typeof(RoleCardListItem), nameof(RoleCardListItem.SetData))]
public static class PatchRoleDeckBuilder
{
    public static void Postfix(RoleCardListItem __instance, Role role)
    {
        if (!Constants.EnableIcons())
            return;

        var icon = GetSprite(Utils.RoleName(role), Utils.FactionName(role.GetFactionType()), false);

        if (__instance.roleImage && icon.IsValid())
            __instance.roleImage.sprite = icon;

        var banned = __instance.bannedImageGO.GetComponent<Image>();
        var sprite = GetSprite("Banned");

        if (sprite.IsValid() && banned)
            banned.sprite = sprite;
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupRoleListItem))]
[HarmonyPatch(nameof(SpecialAbilityPopupRoleListItem.SetData), new Type[] { typeof(Sprite), typeof(Role), typeof(SpecialAbilityPopupEnchanter) })]
public static class Patch_SpecialAbilityPopupRoleListItem_Enchanter
{
	public static void Postfix(SpecialAbilityPopupRoleListItem __instance, Role role, Image ___roleIconImage)
	{
		if (!Constants.EnableIcons()) return;
		if (___roleIconImage == null) return;

		var icon = GetSprite(Utils.RoleName(role), Utils.FactionName(role.GetFactionType()), false);
		if (icon.IsValid())
		{
			___roleIconImage.sprite = icon;
			___roleIconImage.gameObject.SetActive(true);
		}
	}
}

[HarmonyPatch(typeof(SpecialAbilityPopupRoleListItem))]
[HarmonyPatch(nameof(SpecialAbilityPopupRoleListItem.SetData), new Type[] { typeof(Sprite), typeof(Role), typeof(SpecialAbilityPopupRitualist) })]
public static class Patch_SpecialAbilityPopupRoleListItem_Ritualist
{
	public static void Postfix(SpecialAbilityPopupRoleListItem __instance, Role role, Image ___roleIconImage)
	{
		if (!Constants.EnableIcons()) return;
		if (___roleIconImage == null) return;

		var icon = GetSprite(Utils.RoleName(role), Utils.FactionName(role.GetFactionType()), false);
		if (icon.IsValid())
		{
			___roleIconImage.sprite = icon;
			___roleIconImage.gameObject.SetActive(true);
		}
	}
}

[HarmonyPatch(typeof(SpecialAbilityPopupRoleListItem))]
[HarmonyPatch(nameof(SpecialAbilityPopupRoleListItem.SetData), new Type[] { typeof(Sprite), typeof(Role), typeof(LastWillPanel) })]
public static class Patch_SpecialAbilityPopupRoleListItem_LastWill
{
	public static void Postfix(SpecialAbilityPopupRoleListItem __instance, Role role, Image ___roleIconImage)
	{
		if (!Constants.EnableIcons()) return;
		if (___roleIconImage == null) return;

		var icon = GetSprite(Utils.RoleName(role), Utils.FactionName(role.GetFactionType()), false);
		if (icon.IsValid())
		{
			___roleIconImage.sprite = icon;
			___roleIconImage.gameObject.SetActive(true);
		}
	}
}

[HarmonyPatch(typeof(SpecialAbilityPopupFactionListItem))]
[HarmonyPatch(nameof(SpecialAbilityPopupRoleListItem.SetData), new Type[] { typeof(FactionType), typeof(SpecialAbilityPopupEnchanter) })]
public static class Patch_SpecialAbilityPopupFactionListItem_Enchanter
{
	public static void Postfix(SpecialAbilityPopupFactionListItem __instance, FactionType faction, Image ___factionIconImage)
	{
		if (!Constants.EnableIcons()) return;
		if (___factionIconImage == null) return;

		var icon = GetSprite(Utils.FactionName(faction), Utils.FactionName(faction), false);
		if (icon.IsValid())
		{
			___factionIconImage.sprite = icon;
			___factionIconImage.gameObject.SetActive(true);
		}
	}
}

[HarmonyPatch(typeof(SpecialAbilityPopupFactionListItem))]
[HarmonyPatch(nameof(SpecialAbilityPopupRoleListItem.SetData), new Type[] { typeof(Sprite), typeof(FactionType), typeof(LastWillPanel), typeof(bool) })]
public static class Patch_SpecialAbilityPopupFactionListItem_LastWill
{
	public static void Postfix(SpecialAbilityPopupFactionListItem __instance, FactionType faction, Image ___factionIconImage)
	{
		if (!Constants.EnableIcons()) return;
		if (___factionIconImage == null) return;

		var icon = GetSprite(Utils.FactionName(faction), Utils.FactionName(faction), false);
		if (icon.IsValid())
		{
			___factionIconImage.sprite = icon;
			___factionIconImage.gameObject.SetActive(true);
		}
	}
}
[HarmonyPatch(typeof(RoleMenuPopupFactionListItem), nameof(RoleMenuPopupFactionListItem.Set))]
public static class RoleMenuPopupFactionListItem_Set_IconPatch
{
    public static void Postfix(RoleMenuPopupFactionListItem __instance, FactionType overrideFaction)
    {
        if (!Constants.EnableIcons())
            return;

        if (!__instance.Icon)
            return;

        Sprite icon = null;

        switch (overrideFaction)
        {
            case FactionType.TOWN:
                icon = GetSprite("Town", Utils.FactionName(overrideFaction), false);
                break;

            case FactionType.COVEN:
                icon = GetSprite("Coven", Utils.FactionName(overrideFaction), false);
                break;

            case FactionType.SERIALKILLER:
                icon = GetSprite("SerialKiller", Utils.FactionName(overrideFaction), false);
                break;

            case FactionType.ARSONIST:
                icon = GetSprite("Arsonist", Utils.FactionName(overrideFaction), false);
                break;

            case FactionType.WEREWOLF:
                icon = GetSprite("Werewolf", Utils.FactionName(overrideFaction), false);
                break;

            case FactionType.SHROUD:
                icon = GetSprite("Shroud", Utils.FactionName(overrideFaction), false);
                break;

            case FactionType.APOCALYPSE:
                icon = GetSprite("Apocalypse", Utils.FactionName(overrideFaction), false);
                break;

            case FactionType.DOOMSAYER:
                icon = GetSprite("Doomsayer", Utils.FactionName(overrideFaction), false);
                break;

            case FactionType.EXECUTIONER:
                icon = GetSprite("Executioner", Utils.FactionName(overrideFaction), false);
                break;

            case FactionType.JESTER:
                icon = GetSprite("Jester", Utils.FactionName(overrideFaction), false);
                break;

            case FactionType.PIRATE:
                icon = GetSprite("Pirate", Utils.FactionName(overrideFaction), false);
                break;

            case FactionType.VAMPIRE:
                icon = GetSprite("Vampire", Utils.FactionName(overrideFaction), false);
                break;

            case FactionType.CURSED_SOUL:
                icon = GetSprite("CursedSoul", Utils.FactionName(overrideFaction), false);
                break;

            case Btos2Faction.Jackal:
                icon = GetSprite("Jackal", Utils.FactionName(overrideFaction), false);
                break;

            case Btos2Faction.Frogs:
            case Btos2Faction.Lions:
            case Btos2Faction.Hawks:
                icon = GetSprite("Teams", Utils.FactionName(overrideFaction), false);
                break;

            case Btos2Faction.Judge:
                icon = GetSprite("Judge", Utils.FactionName(overrideFaction), false);
                break;

            case Btos2Faction.Auditor:
                icon = GetSprite("Auditor", Utils.FactionName(overrideFaction), false);
                break;

            case Btos2Faction.Starspawn:
                icon = GetSprite("Starspawn", Utils.FactionName(overrideFaction), false);
                break;

            case Btos2Faction.Inquisitor:
                icon = GetSprite("Inquisitor", Utils.FactionName(overrideFaction), false);
                break;

            case Btos2Faction.Egotist:
                icon = GetSprite("Egotist", Utils.FactionName(overrideFaction), false);
                break;

            case Btos2Faction.Pandora:
                icon = GetSprite("PandorasBox", Utils.FactionName(overrideFaction), false);
                break;

            case Btos2Faction.Compliance:
                icon = GetSprite("CompliantKillers", Utils.FactionName(overrideFaction), false);
                break;

            default:
                icon = GetSprite("Hidden");
                break;
        }

        if (__instance.Icon && icon.IsValid())
            __instance.Icon.sprite = icon;
    }
}

[HarmonyPatch(typeof(RoleMenuPopupController))]
public static class RoleMenuPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(RoleMenuPopupController.InitializeSetRole1FactionInRoleListPanel))]
    public static void Postfix_SetRole1Faction(RoleMenuPopupController __instance)
    {
        if (__instance.m_index == -1)
            return;

        var icon = __instance.m_role.GetTMPSprite();
        icon = icon.Replace("RoleIcons\"", $"RoleIcons ({Utils.FactionName(__instance.m_faction, false)})\"");

        __instance.SetRole1FactionInRoleListLabel.text = __instance.l10n("GUI_ROLE_CARD_MENU_POPUP_SET_ROLE_FACTION").Replace("%role%", icon + __instance.m_role.ToColorizedDisplayString(__instance.m_faction));
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(RoleMenuPopupController.InitializeSetRole2FactionInRoleListPanel))]
    public static void Postfix_SetRole2Faction(RoleMenuPopupController __instance)
    {
        if (__instance.m_index == -1)
            return;

        var icon = __instance.m_role2.GetTMPSprite();
        icon = icon.Replace("RoleIcons\"", $"RoleIcons ({Utils.FactionName(__instance.m_faction2, false)})\"");

        __instance.SetRole2FactionInRoleListLabel.text = __instance.l10n("GUI_ROLE_CARD_MENU_POPUP_SET_ROLE_FACTION") .Replace("%role%", icon + __instance.m_role2.ToColorizedDisplayString(__instance.m_faction2));
    }
	
	[HarmonyPostfix]
	[HarmonyPatch(typeof(RoleMenuPopupController), nameof(RoleMenuPopupController.InitializeShowAddDualBucketToRoleListPanel))]
	public static void Postfix_AddDualBucket(RoleMenuPopupController __instance)
	{
		if (__instance.m_index == -1)
			return;

		var icon = __instance.m_role.GetTMPSprite();
		icon = icon.Replace("RoleIcons\"", $"RoleIcons ({Utils.FactionName(__instance.m_faction, false)})\"");

		__instance.AddDualBucketToHostRoleListLabel.text = __instance.l10n("GUI_ROLE_CARD_MENU_POPUP_SET_DUAL_BUCKET_IN_LIST").Replace("%role%", icon + __instance.m_role.ToColorizedDisplayString(__instance.m_faction));
	}
}
// [HarmonyPatch(typeof(RoleDeckListItem), nameof(RoleDeckListItem.SetData))]
// public static class PatchRoleListPanel2
// {
//     public static void Postfix(RoleDeckListItem __instance, RoleDeckSlot a_roleDeckSlot, bool a_isBan)
//     {
//         if (!Constants.EnableIcons())
//             return;

//         var a_role = a_roleDeckSlot?.Role1;

//         if (a_role == null)
//             return;

//         var role = a_role.Value;
//         var factionType = role.GetFactionType();

//         var icon = a_isBan
//             ? GetSprite("Banned")
//             : GetSprite(Utils.RoleName(role), Utils.FactionName(factionType), false);

//         if (__instance.roleImage && icon.IsValid())
//             __instance.roleImage.sprite = icon;
//     }
// }

[HarmonyPatch(typeof(RoleDeckListItem), nameof(RoleDeckListItem.SetData))]
public static class PatchRoleListPanel
{
    public static bool Prefix(RoleDeckListItem __instance, RoleDeckSlot a_roleDeckSlot, RoleDeckPanelController parent, bool a_isBan = false)
    {
        __instance.Reset();
        __instance._parentPanel = parent;
        __instance.background.SetActive(false);
        __instance.roleDeckSlot = a_roleDeckSlot;
        __instance.role = a_roleDeckSlot.Role1;
        __instance.role2 = a_roleDeckSlot.Role2;
        __instance.faction = a_roleDeckSlot.Faction1;
        __instance.faction2 = a_roleDeckSlot.Faction2;
        __instance.isBan = a_isBan;

        var role1 = __instance.role;
        var role2 = __instance.role2;
        var faction1 = __instance.faction;
        var faction2 = __instance.faction2;

        __instance.index = Service.Game.Sim.simulation.roleDeckBuilder.Data.roleDeckSlots
            .IndexOf(a_roleDeckSlot);

        __instance.roleName.gameObject.SetActive(true);
        __instance.roleImage.gameObject.SetActive(__instance.isBan);

        if (__instance.isBan)
        {
			var icon1 = role1.GetTMPSprite();
            icon1 = icon1.Replace("RoleIcons\"", $"RoleIcons (Regular)\"");

			__instance.roleName.text = $"<size=200%><voffset=-7>{icon1}</voffset></size>" + role1.ToColorizedNoLabel(faction1);
			__instance.roleImage.sprite = GetSprite("Blank");
        }
        else if (a_roleDeckSlot.IsDualBucket())
        {
            var icon1 = GetStyledIcon(role1, faction1);
            var icon2 = GetStyledIcon(role2, faction2);

            __instance.roleName.text = string.Concat(
                "<size=200%><voffset=-7>", icon1, "</voffset></size>", role1.ToColorizedShortenedDisplayString(faction1),
                " <color=#FFFFFF40>-</color> <size=200%><voffset=-7>", icon2, "</voffset></size>", role2.ToColorizedShortenedDisplayString(faction2)
            );
        }
        else
        {
            var icon1 = GetStyledIcon(role1, faction1);
            __instance.roleName.text = "<size=200%><voffset=-7>" + icon1 + "</voffset></size>" + role1.ToColorizedDisplayString(faction1);
        }

        __instance.roleImage.SetAllDirty();
        __instance.gameObject.SetActive(true);
        __instance.ValidateButtons();

        return false;
    }

    private static string GetStyledIcon(Role role, FactionType faction)
    {
        var icon = role.GetTMPSprite();
        return icon.Replace("RoleIcons\"", $"RoleIcons ({((role.GetFactionType() == faction && Constants.CurrentStyle() == "Regular")
            ? "Regular" : Utils.FactionName(faction, false))})\"");
    }
}
[HarmonyPatch(typeof(GameBrowserRoleDeckListItem), nameof(GameBrowserRoleDeckListItem.SetData))]
public static class PatchBrowserRoleListPanel
{
    public static bool Prefix(GameBrowserRoleDeckListItem __instance, RoleDeckSlot a_roleDeckSlot, bool a_isBan = false)
    {
        if (a_roleDeckSlot == null)
            return false;

        __instance.Reset();
        __instance.background.SetActive(false);
        __instance.roleDeckSlot = a_roleDeckSlot;
        __instance.isBan = a_isBan;

        var role1 = a_roleDeckSlot.Role1;
        var role2 = a_roleDeckSlot.Role2;
        var faction1 = a_roleDeckSlot.Faction1;
        var faction2 = a_roleDeckSlot.Faction2;

        __instance.roleName.gameObject.SetActive(true);
        __instance.roleImage.gameObject.SetActive(__instance.isBan);

        if (__instance.isBan)
        {
			var icon1 = role1.GetTMPSprite();
            icon1 = icon1.Replace("RoleIcons\"", $"RoleIcons (Regular)\"");
            __instance.roleName.text = $"<size=200%><voffset=-7>{icon1}</voffset></size>" + role1.ToColorizedNoLabel(faction1);
			__instance.roleImage.sprite = GetSprite("Blank");
        }
        else if (a_roleDeckSlot.IsDualBucket())
        {
            var icon1 = role1.GetTMPSprite();
            var icon2 = role2.GetTMPSprite();

            icon1 = icon1.Replace("RoleIcons\"", $"RoleIcons ({Utils.FactionName(faction1, false)})\"");
            icon2 = icon2.Replace("RoleIcons\"", $"RoleIcons ({Utils.FactionName(faction2, false)})\"");

            __instance.roleName.text = string.Concat(
                "<size=200%><voffset=-7>", icon1, "</voffset></size>",
                role1.ToColorizedShortenedDisplayString(faction1),
                " <color=#FFFFFF40>-</color> <size=200%><voffset=-7>", icon2, "</voffset></size>",
                role2.ToColorizedShortenedDisplayString(faction2)
            );
        }
        else
        {
            var icon1 = role1.GetTMPSprite();
            icon1 = icon1.Replace("RoleIcons\"", $"RoleIcons ({Utils.FactionName(faction1, false)})\"");

            __instance.roleName.text = "<size=200%><voffset=-7>" + icon1 + "</voffset></size>" +
                                       role1.ToColorizedDisplayString(faction1);
        }

        __instance.gameObject.SetActive(true);

        return false;
    }
}
[HarmonyPatch(typeof(RoleCardPanelBackground))]
public static class RoleCardFixesAndPatches
{
    [HarmonyPatch(nameof(RoleCardPanelBackground.Start))]
    public static bool Prefix(RoleCardPanelBackground __instance)
    {
        __instance.rolecardBackgroundInstance = UObject.Instantiate(__instance.rolecardBackgroundTemplate);
        Debug.Log($"RoleCardPanelBackground Created {__instance.rolecardBackgroundInstance.name}");
        
        __instance.deadStamp.SetActive(false);

        if (Service.Game.Sim.simulation != null)
        {
            Service.Game.Sim.simulation.myIdentity.OnChanged += __instance.HandleMyIdentityChanged;
            Service.Game.Sim.info.roleCardObservation.OnDataChanged += __instance.HandleRoleCardChanged;

            var myIdentity = Pepper.GetMyCurrentIdentity();
            __instance.SetRoleAndFaction(myIdentity.role, myIdentity.faction);
        }
        else
        {
            __instance.SetRoleAndFaction(Role.TAVERNKEEPER, FactionType.TOWN);
        }

        return false;
    }

    [HarmonyPatch(nameof(RoleCardPanelBackground.SetRoleAndFaction))]
    [HarmonyPatch(nameof(RoleCardPanelBackground.HandleMyIdentityChanged))]
    [HarmonyPatch(nameof(RoleCardPanelBackground.HandleRoleCardChanged))]
    public static void Postfix(RoleCardPanelBackground __instance)
    {
        if (!Constants.EnableIcons())
            return;

        var panel = __instance.GetComponentInParent<RoleCardPanel>();

        PatchRoleCards.ChangeRoleCard(
            panel?.roleIcon,
            panel?.specialAbilityPanel?.useButton?.abilityIcon,
            panel?.roleInfoButtons,
            __instance.currentRole,
            __instance.currentFaction
        );
    }
}
[HarmonyPatch(typeof(RoleCardPopupPanel))]
public static class PatchRoleCards
{
    [HarmonyPatch(nameof(RoleCardPopupPanel.SetRoleAndFaction))]
    public static void Postfix(RoleCardPopupPanel __instance, Role role, FactionType faction)
    {
        if (Constants.EnableIcons())
            ChangeRoleCard(__instance.roleIcon, __instance.specialAbilityPanel?.useButton?.abilityIcon, __instance.roleInfoButtons, role, faction, true);
    }

    [HarmonyPatch(nameof(RoleCardPopupPanel.ShowAttackAndDefense))]
    [HarmonyPriority(0)]
    public static void Postfix(RoleCardPopupPanel __instance, RoleCardData data)
    {
        var attack = GetSprite($"Attack{Utils.GetLevel(data.attack, true)}");
        var icon1 = __instance.transform.Find("AttackIcon").Find("Icon").GetComponent<Image>();

        if (icon1)
            icon1.sprite = attack.IsValid() ? attack : FancyAssetManager.Attack;

        var eth = __instance.CurrentFaction > Btos2Faction.Hawks && __instance.CurrentFaction < Btos2Faction.Pandora && __instance.CurrentFaction != Btos2Faction.Inquisitor;
        var defenseAmount = __instance.defenseGlow.fillAmount * 3.0303030303f;
        var defense = GetSprite($"Defense{Utils.GetLevel(eth ? 4 : (int)defenseAmount, false)}");
        var icon2 = __instance.transform.Find("DefenseIcon").Find("Icon").GetComponent<Image>();

        if (icon2)
            icon2.sprite = defense.IsValid() ? defense : (eth ? Ethereal : FancyAssetManager.Defense);
    }

    public static void ChangeRoleCard(Image roleIcon, Image specialAbilityPanel, List<BaseAbilityButton> roleInfoButtons, Role role, FactionType faction, bool isGuide = false)
    {
        roleInfoButtons ??= [];

        // Merged a CW patch here for optimisation purposes

        foreach (var button in roleInfoButtons)
            button.transform.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Metal); // Rings at the back

        Utils.UpdateMaterials(!isGuide, faction);

        role = Constants.IsTransformed() && !isGuide ? Utils.GetTransformedVersion(role) : role;
        var index = 0;
        var roleName = Utils.RoleName(role);
        var factionName = Utils.FactionName(faction);
        var ogfaction = Utils.FactionName(role.GetFactionType(), false);
        var reg = ogfaction != factionName;
        var sprite = GetSprite(reg, roleName, factionName);

		/* var obs = Service.Game?.Sim?.info?.roleCardObservation;
		var data = obs?.Data;
		bool ability1Available = data?.normalAbilityAvailable ?? true;
		bool ability2Available = data?.secondAbilityAvailable ?? true; */

        if (!sprite.IsValid() && reg)
            sprite = GetSprite(roleName, ogfaction);

        if (sprite.IsValid() && roleIcon)
            roleIcon.sprite = sprite;

        var specialName = $"{roleName}_Special";
        var special = GetSprite(reg, specialName, factionName);

        if (!special.IsValid() && reg)
            special = GetSprite(specialName, ogfaction);

        if (special.IsValid() && specialAbilityPanel)
            specialAbilityPanel.sprite = special;

        var abilityName = $"{roleName}_Ability";
        var ability1 = GetSprite(reg, abilityName, factionName);

        if (!ability1.IsValid())
            ability1 = GetSprite(reg, $"{abilityName}_1", factionName);

        if (!ability1.IsValid() && reg)
            ability1 = GetSprite(abilityName, ogfaction);

        if (!ability1.IsValid() && reg)
            ability1 = GetSprite($"{abilityName}_1", ogfaction);

        if (/*(ability1Available || isGuide) && */ability1.IsValid() && roleInfoButtons.IsValid(index))
        {
            roleInfoButtons[index].abilityIcon.sprite = ability1;
            index++;
        }
        else if (Utils.Skippable(abilityName) || Utils.Skippable($"{abilityName}_1"))
            index++;

        var abilityName2 = $"{roleName}_Ability_2";
        var ability2 = GetSprite(reg, abilityName2, factionName);

        if (!ability2.IsValid() && reg)
            ability2 = GetSprite(abilityName2, ogfaction);

        if (/*(ability2Available || isGuide) && */ability2.IsValid() && roleInfoButtons.IsValid(index)/* && !(role == Utils.GetWar() && !isGuide)*/)
        {
            roleInfoButtons[index].abilityIcon.sprite = ability2;
            index++;
        }
        else if (Utils.Skippable(abilityName2))
            index++;

        var attribute = GetSprite(reg, $"Attributes_{roleName}", factionName);

        if (!attribute.IsValid() && role.IsTransformedApoc())
            attribute = GetSprite(reg, "Attributes_Horsemen", factionName);

        if (!attribute.IsValid() && role.IsApocAcolyte())
            attribute = GetSprite(reg, "Attributes_Apocalypse", factionName);

        if (!attribute.IsValid() && role.IsCovenAligned())
            attribute = GetSprite(reg, "Attributes_Coven", factionName);

        if (!attribute.IsValid() && role.IsTownAligned())
            attribute = GetSprite(reg, "Attributes_Town", factionName);

        if (!attribute.IsValid())
            attribute = GetSprite(reg, "Attributes", factionName);

        if (reg && !attribute.IsValid())
        {
            attribute = GetSprite($"Attributes_{roleName}", ogfaction);

            if (!attribute.IsValid() && role.IsTransformedApoc())
                attribute = GetSprite("Attributes_Horsemen", ogfaction);

            if (!attribute.IsValid() && role.IsApocAcolyte())
                attribute = GetSprite("Attributes_Apocalypse", ogfaction);

            if (!attribute.IsValid() && role.IsCovenAligned())
                attribute = GetSprite(reg, "Attributes_Coven", ogfaction);

            if (!attribute.IsValid() && role.IsTownAligned())
                attribute = GetSprite(reg, "Attributes_Town", ogfaction);

            if (!attribute.IsValid())
                attribute = GetSprite("Attributes", ogfaction);
        }

        if (attribute.IsValid() && roleInfoButtons.IsValid(index))
            roleInfoButtons[index].abilityIcon.sprite = attribute;

        index++;
        var nommy = GetSprite(reg, $"Necronomicon_{roleName}", factionName);

        if (!nommy.IsValid())
            nommy = GetSprite(reg, "Necronomicon", factionName);

        if (reg && !nommy.IsValid())
        {
            nommy = GetSprite($"Necronomicon_{roleName}", ogfaction);

            if (!nommy.IsValid())
                nommy = GetSprite("Necronomicon", ogfaction);
        }

        if (nommy.IsValid() && (Constants.IsNecroActive() || isGuide) && roleInfoButtons.IsValid(index))
            roleInfoButtons[index].abilityIcon.sprite = nommy;
    }
}

[HarmonyPatch(typeof(TosAbilityPanelListItem), nameof(TosAbilityPanelListItem.OverrideIconAndText))]
public static class PatchAbilityPanelListItems
{
    public static void Postfix(TosAbilityPanelListItem __instance, AbilityType overrideType)
    {
        if (__instance.choice1Sprite)
            __instance.choice1Sprite.transform.parent.GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Wax);

        if (__instance.choice2Sprite)
            __instance.choice2Sprite.transform.parent.GetChild(1).GetComponent<Image>().SetImageColor(ColorType.Wax);

        __instance.playerName.SetGraphicColor(ColorType.Paper);
        __instance.playerNumber.SetGraphicColor(ColorType.Paper);

        if (!Constants.EnableIcons())
            return;

        var role = Pepper.GetMyRole();
        var faction = Utils.FactionName(Pepper.GetMyFaction());
        role = Constants.IsTransformed() ? Utils.GetTransformedVersion(role) : role;
        var ogfaction = Utils.FactionName(role.GetFactionType(), false);
        var name = Utils.RoleName(role);
        var reg = ogfaction != faction;
        var ee = Fancy.PlayerPanelEasterEggs.Value;

        switch (overrideType)
        {
            case AbilityType.NECRO_ATTACK:
            {
                var nommy = GetSprite("Necronomicon", faction, ee);

                if (nommy.IsValid() && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = nommy;

                switch (role)
                {
                    case Role.WITCH:
                    {
                        var target = GetSprite(reg, "Witch_Ability_2", faction, ee);

                        if (!target.IsValid() && reg)
                            target = GetSprite("Witch_Ability_2", ogfaction, ee);

                        if (target.IsValid() && __instance.choice2Sprite)
                            __instance.choice2Sprite.sprite = target;

                        break;
                    }
                }

                break;
            }
            case AbilityType.VOTING:
            {
                var ab1 = GetSprite(reg, $"Vote", faction, ee);

                if (ab1.IsValid() && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = ab1;

                break;
            }
            case AbilityType.POISONER_POISON or AbilityType.SHROUD or AbilityType.PIRATE or (AbilityType)30 or (AbilityType)32 or (AbilityType)33:
            {
                var special = GetSprite(reg, $"{name}_Special", faction, ee);

                if (!special.IsValid() && reg)
                    special = GetSprite($"{name}_Special", ogfaction, ee);

                if (special.IsValid() && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = special;

                break;
            }
            case AbilityType.POTIONMASTER_ATTACK:
            {
                switch (role)
                {
                    case Role.POTIONMASTER:
                    {
                        var special = GetSprite(reg, $"Necronomicon", faction, ee);

                        if (!special.IsValid() && reg)
                            special = GetSprite($"Necronomicon", ogfaction, ee);

                        if (special.IsValid() && __instance.choice1Sprite)
                            __instance.choice1Sprite.sprite = special;

                        break;

                    }
                    case Role.BAKER:
                    {
						var special = GetSprite(reg, $"{name}_Ability_3", faction, ee);

						if (!special.IsValid() && reg)
							special = GetSprite($"{name}_Ability_3", ogfaction, ee);

						if (special.IsValid() && __instance.choice2Sprite)
							__instance.choice2Sprite.sprite = special;

						break;
                    }
                    default:
                    {
                        var special = GetSprite(reg, $"{name}_Ability_3", faction, ee);

                        if (!special.IsValid() && reg)
                            special = GetSprite($"{name}_Ability_3", ogfaction, ee);

                        if (special.IsValid() && __instance.choice1Sprite)
                            __instance.choice1Sprite.sprite = special;

                        break;
                    }
                }

                break;
            }
            case AbilityType.POTIONMASTER_HEAL:
            {
                switch (role)
                {
                    case Role.POTIONMASTER:
                    {
                        var ab2 = GetSprite(reg, $"{name}_Ability_2", faction, ee);

                        if (!ab2.IsValid() && reg)
                            ab2 = GetSprite($"{name}_Ability_2", ogfaction, ee);

                        if (ab2.IsValid() && __instance.choice1Sprite)
                            __instance.choice1Sprite.sprite = ab2;

                        break;

                    }
                    case Role.BAKER:
                    {
						var ab2 = GetSprite(reg, $"{name}_Ability_2", faction, ee);

						if (!ab2.IsValid() && reg)
							ab2 = GetSprite($"{name}_Ability_2", ogfaction, ee);

						if (ab2.IsValid() && __instance.choice2Sprite)
							__instance.choice2Sprite.sprite = ab2;

						break;
                    }
                    default:
                    {
                        var ab2 = GetSprite(reg, $"{name}_Ability_2", faction, ee);

                        if (!ab2.IsValid() && reg)
                            ab2 = GetSprite($"{name}_Ability_2", ogfaction, ee);

                        if (ab2.IsValid() && __instance.choice1Sprite)
                            __instance.choice1Sprite.sprite = ab2;

                        break;
                    }
                }

                break;
            }
            case AbilityType.POTIONMASTER_REVEAL:
            {
                switch (role)
                {
                    case Role.POTIONMASTER:
                    {
                        var ab = GetSprite(reg, $"{name}_Ability_1", faction, ee);

                        if (!ab.IsValid() && reg)
                            ab = GetSprite($"{name}_Ability_1", ogfaction, ee);

                        if (ab.IsValid() && __instance.choice1Sprite)
                            __instance.choice1Sprite.sprite = ab;

                        break;

                    }
                    case Role.BAKER:
                    {
						var ab = GetSprite(reg, $"{name}_Ability_1", faction, ee);

						if (!ab.IsValid() && reg)
							ab = GetSprite($"{name}_Ability_1", ogfaction, ee);

						if (ab.IsValid() && __instance.choice2Sprite)
							__instance.choice2Sprite.sprite = ab;

						break;
                    }
                    default:
                    {
                        var ab = GetSprite(reg, $"{name}_Ability_1", faction, ee);

                        if (!ab.IsValid() && reg)
                            ab = GetSprite($"{name}_Ability_1", ogfaction, ee);

                        if (ab.IsValid() && __instance.choice1Sprite)
                            __instance.choice1Sprite.sprite = ab;

                        break;
                    }
                }

                break;
            }
            case AbilityType.VOODOOMASTER_SILENCE:
            {
                var ab = GetSprite(reg, $"{name}_Ability_1", faction, ee);

                if (!ab.IsValid() && reg)
                    ab = GetSprite($"{name}_Ability_1", ogfaction, ee);

                if (ab.IsValid() && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = ab;

                break;
            }
            case AbilityType.VOODOOMASTER_DEAFEN:
            {
                var ab = GetSprite(reg, $"{name}_Ability_2", faction, ee);

                if (!ab.IsValid() && reg)
                    ab = GetSprite($"{name}_Ability_2", ogfaction, ee);

                if (ab.IsValid() && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = ab;

                break;
            }
            case AbilityType.VOODOOMASTER_BLIND:
            {
                var ab = GetSprite(reg, $"{name}_Ability_3", faction, ee);

                if (!ab.IsValid() && reg)
                    ab = GetSprite($"{name}_Ability_3", ogfaction, ee);

                if (ab.IsValid() && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = ab;

                break;
            }
            case AbilityType.WEREWOLF_NON_FULL_MOON:
            {
                var ab2 = GetSprite(reg, $"{name}_Ability_2", faction, ee);

                if (!ab2.IsValid() && reg)
                    ab2 = GetSprite($"{name}_Ability_2", ogfaction, ee);

                if (ab2.IsValid() && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = ab2;

                break;
            }
            default:
			{
				if (role == Role.JAILOR && Pepper.GetCurrentPlayPhase() == PlayPhase.NIGHT)
				{
					var target = GetSprite(reg, "Jailor_Ability", faction, ee);

					if (!target.IsValid() && reg)
						target = GetSprite("Jailor_Ability_1", faction, ee);

					if (!target.IsValid() && reg)
						target = GetSprite("Jailor_Ability", ogfaction, ee);

					if (!target.IsValid() && reg)
						target = GetSprite("Jailor_Ability_1", ogfaction, ee);

					if (target.IsValid() && __instance.choice2Sprite)
						__instance.choice2Sprite.sprite = target;
				}
				else
				{
					var abilityName = $"{name}_Ability";
					var ability1 = GetSprite(reg, abilityName, faction, ee);

					if (!ability1.IsValid())
						ability1 = GetSprite(reg, $"{abilityName}_1", faction, ee);

					if (reg)
					{
						if (!ability1.IsValid())
							ability1 = GetSprite(abilityName, ogfaction, ee);

						if (!ability1.IsValid())
							ability1 = GetSprite($"{abilityName}_1", ogfaction, ee);
					}

					if (ability1.IsValid() && __instance.choice1Sprite)
						__instance.choice1Sprite.sprite = ability1;

					var ability2 = GetSprite(reg, $"{abilityName}_2", faction, ee);

					if (!ability2.IsValid() && reg)
						ability2 = GetSprite($"{abilityName}_2", ogfaction, ee);

					if (ability2.IsValid() && __instance.choice2Sprite)
						__instance.choice2Sprite.sprite = ability2;
				}

				break;
			}

        }
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupGenericListItem), nameof(SpecialAbilityPopupGenericListItem.SetData))]
public static class SpecialAbilityPopupGenericListItemPatch
{
    // ReSharper disable once InconsistentNaming
    public static bool Prefix(
        SpecialAbilityPopupGenericListItem __instance,
        int position,
        string player_name,
        Sprite headshot,
        bool hasChoice1,
        bool hasChoice2,
        UIRoleData data)
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
        {
            roleText = Fancy.FactionalRoleNames.Value
                ? Utils.GetRoleName(role, factionType, true)
                : $"({role.ToDisplayString()})";
        }

        __instance.playerName.SetText(
            $"{player_name} {Utils.ApplyGradient(roleText, gradient)}");

        __instance.playerHeadshot.sprite = headshot;
        __instance.characterPosition = position;
        __instance.playerNumber.text = $"{position + 1}.";

        var myRole = Pepper.GetMyRole();
        var uiRoleDataInstance = data.roleDataList.Find(d => d.role == myRole);

        if (uiRoleDataInstance != null)
        {
            __instance.choiceText.text =
                __instance.l10n($"GUI_ROLE_SPECIAL_ABILITY_VERB_{(int)uiRoleDataInstance.role}");

            __instance.choice2Text.text =
                __instance.l10n($"GUI_ROLE_ABILITY2_VERB_{(int)uiRoleDataInstance.role}");

            var faction = Utils.FactionName(Pepper.GetMyFaction());
            var ogFaction = Utils.FactionName(myRole.GetFactionType(), false);
            var reg = faction != ogFaction;

            // Special ability icon
            if (__instance.choiceSprite)
            {
                if (Constants.EnableIcons())
                {
                    var name = $"{Utils.RoleName(myRole)}_Special";
                    var sprite = GetSprite(reg, name, faction);

                    if (!sprite.IsValid() && reg)
                        sprite = GetSprite(name, ogFaction);

                    __instance.choiceSprite.sprite = sprite.IsValid()
                        ? sprite
                        : uiRoleDataInstance.specialAbilityIcon;
                }
                else
                {
                    __instance.choiceSprite.sprite =
                        uiRoleDataInstance.specialAbilityIcon;
                }

                __instance.choiceSprite.enabled = false;
                __instance.choiceSprite.enabled = true;
            }

            // Ability 2 icon
            if (__instance.choice2Sprite)
            {
                if (Constants.EnableIcons())
                {
                    var name = $"{Utils.RoleName(myRole)}_Ability_2";
                    var sprite = GetSprite(reg, name, faction);

                    if (!sprite.IsValid() && reg)
                        sprite = GetSprite(name, ogFaction);

                    __instance.choice2Sprite.sprite = sprite.IsValid()
                        ? sprite
                        : uiRoleDataInstance.abilityIcon2;
                }
                else
                {
                    __instance.choice2Sprite.sprite =
                        uiRoleDataInstance.abilityIcon2;
                }

                __instance.choice2Sprite.enabled = false;
                __instance.choice2Sprite.enabled = true;
            }
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


[HarmonyPatch(typeof(SpecialAbilityPopupRadialIcon), nameof(SpecialAbilityPopupRadialIcon.SetData))]
public static class PatchRitualistGuessMenu
{
    public static void Postfix(SpecialAbilityPopupRadialIcon __instance, Role a_role)
    {
        if (!Constants.EnableIcons())
            return;

        var icon = GetSprite(Utils.RoleName(a_role), Utils.FactionName(a_role.GetFactionType()), false);

        if (__instance.roleIcon && icon.IsValid())
            __instance.roleIcon.sprite = icon;
    }
}

[HarmonyPatch(typeof(HomeInterfaceService), nameof(HomeInterfaceService.Init))]
public static class CacheDefaults
{
    public static bool ServiceExists { get; private set; }

    public static TMP_SpriteAsset RoleIcons { get; private set; }
    public static TMP_SpriteAsset Numbers { get; private set; }
    public static TMP_SpriteAsset Emojis { get; private set; }

    private static readonly List<string> Assets = [ "Cast", "LobbyIcons", "MiscIcons", "PlayerNumbers", "RoleIcons", "SalemTmpIcons", "TrialReportIcons", "Emojis" ];

    public static bool Prefix(HomeInterfaceService __instance)
    {
        Assets.ForEach(key =>
        {
            Debug.Log($"HomeInterfaceService:: Add Sprite Asset {key}");
            var asset = __instance.LoadResource<TMP_SpriteAsset>($"TmpSpriteAssets/{key}.asset");

            switch (key)
            {
                case "RoleIcons":
                {
                    RoleIcons = asset;
                    break;
                }
                case "PlayerNumbers":
                {
                    Numbers = asset;
                    break;
                }
                case "Emojis":
                {
                    Emojis = asset;
                    break;
                }
            }

            if (key is "RoleIcons" or "PlayerNumbers" or "Emojis")
                Utils.DumpSprite(asset.spriteSheet as Texture2D, key, Path.Combine(IPPath, "Vanilla"), true);
            else
                MaterialReferenceManager.AddSpriteAsset(asset);
        });

        SetScrollSprites();
        ServiceExists = true;
        __instance.isReady_ = true;
        return false;
    }
}

[HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.ShowAttackAndDefense))]
public static class PatchAttackDefense
{
    public static void Postfix(RoleCardPanel __instance, RoleCardData data)
    {
        if (!Constants.EnableIcons())
            return;

        var attack = GetSprite($"Attack{Utils.GetLevel(data.attack, true)}");
        var icon1 = __instance.transform.Find("AttackIcon").Find("Icon").GetComponent<Image>();

        if (icon1)
            icon1.sprite = attack.IsValid() ? attack : FancyAssetManager.Attack;

        var eth = __instance.myData.IsEthereal() || __instance.CurrentFaction == Btos2Faction.Egotist;
        var defense = GetSprite($"Defense{Utils.GetLevel(eth ? 4 : data.defense, false)}");
        var icon2 = __instance.transform.Find("DefenseIcon").Find("Icon").GetComponent<Image>();

        if (icon2)
            icon2.sprite = defense.IsValid() ? defense : (eth ? Ethereal : FancyAssetManager.Defense);
    }
}

[HarmonyPatch(typeof(PlayerPopupController))]
public static class PlayerPopupControllerPatch
{
    [HarmonyPatch(nameof(PlayerPopupController.SetRoleIcon)), HarmonyPostfix]
    public static void SetRoleIconPostfix(PlayerPopupController __instance)
    {
        if (!Constants.EnableIcons() || !Service.Game.Sim.simulation.killRecords.Data.TryFinding(k => k.playerId == __instance.m_discussionPlayerState.position, out var killRecord))
            return;

        var ogfaction = __instance.m_role.GetFactionType();
        var faction = killRecord!.playerFaction;
        var reg = ogfaction != faction;
        var name = Utils.RoleName(__instance.m_role);
        var sprite = GetSprite(reg, name, Utils.FactionName(faction));

        if (reg && !sprite.IsValid())
            sprite = GetSprite(name, Utils.FactionName(ogfaction, false));

        if (sprite.IsValid() && __instance.RoleIcon)
            __instance.RoleIcon.sprite = sprite;
    }

    [HarmonyPatch(nameof(PlayerPopupController.InitializeRolePanel)), HarmonyPostfix]
    public static void InitializeRolePanelPostfix(PlayerPopupController __instance)
    {
        if (!Constants.EnableIcons() || !Pepper.IsGamePhasePlay())
            return;

        Tuple<Role, FactionType> tuple;
        var ogfaction = __instance.m_role.GetFactionType();

        if (Service.Game.Sim.simulation.killRecords.Data.TryFinding(k => k.playerId == __instance.m_discussionPlayerState.position, out var killRecord))
            tuple = new(killRecord!.playerRole, killRecord.playerFaction);
        else if (!Utils.GetRoleAndFaction(__instance.m_discussionPlayerState.position, out tuple))
            tuple = new(__instance.m_role, ogfaction);

        if (__instance.m_discussionPlayerState.position == Pepper.GetMyPosition())
            tuple = new(Pepper.GetMyRole(), Pepper.GetMyFaction());

        if (tuple.Item2 == FactionType.NONE)
            tuple = new(tuple.Item1, ogfaction);

        var faction = tuple.Item2;
        var reg = ogfaction != faction;
        var name = Utils.RoleName(__instance.m_role);
        var sprite = GetSprite(reg, name, Utils.FactionName(faction));

        if (reg && !sprite.IsValid())
            sprite = GetSprite(name, Utils.FactionName(ogfaction, false));

        if (sprite.IsValid() && __instance.RoleIcon)
            __instance.RoleIcon.sprite = sprite;

        __instance.RoleLabel.SetText(Service.Game.Sim.simulation.GetRoleNameLinkString(tuple.Item1, tuple.Item2));
    }
}

[HarmonyPatch(typeof(RoleMenuPopupController), nameof(RoleMenuPopupController.SetRoleIconAndLabels))]
public static class RoleMenuPopupControllerPatch
{
    public static void Postfix(RoleMenuPopupController __instance)
    {
        if (!Constants.EnableIcons())
            return;

        var sprite = GetSprite(Utils.RoleName(__instance.m_role), Utils.FactionName(__instance.m_role.GetFactionType()));

        if (!sprite.IsValid())
            return;

        if (__instance.RoleIconImage)
            __instance.RoleIconImage.sprite = sprite;

        if (__instance.HeaderRoleIconImage)
            __instance.HeaderRoleIconImage.sprite = sprite;
    }
}

[HarmonyPatch(typeof(PlayerEffectsService), nameof(PlayerEffectsService.GetEffect))]
public static class PlayerEffectsServicePatch
{
    private static readonly Dictionary<EffectType, Sprite> EffectSprites = [];

    public static void Postfix(EffectType effectType, PlayerEffectInfo __result)
    {
        if (!EffectSprites.ContainsKey(effectType))
            EffectSprites[effectType] = __result.sprite;

        if (!Constants.EnableIcons())
        {
            __result.sprite = EffectSprites[effectType];
            return;
        }

		var effect = Utils.EffectName(effectType);
        var ee = Fancy.PlayerPanelEasterEggs.Value;
        var sprite = GetSprite($"{effect}_Effect", ee);

        if (!sprite.IsValid())
            sprite = GetSprite(effect, ee);

        __result.sprite = sprite.IsValid() ? sprite : EffectSprites[effectType];
    }
}

[HarmonyPatch(typeof(GameSimulation))]
public static class GetInlinedStringsPatches
{
    [HarmonyPatch(typeof(GameSimulation), nameof(GameSimulation.GetRoleIconAndNameInlineString))]
    public static void Postfix(Role role, FactionType factionType, ref string __result)
    {
        if (Constants.EnableIcons())
        {
            __result = __result.Replace("RoleIcons\"", $"RoleIcons ({(role.GetFactionType() == factionType && Constants.CurrentStyle() == "Regular" ? "Regular" : Utils.FactionName(factionType, false))})\"");
        }
    }

    // [HarmonyPatch(nameof(GameSimulation.GetTownTraitorRoleIconAndNameInlineString)), HarmonyPostfix]
    // public static void GetTownTraitorRoleIconAndNameInlineStringPostfix(ref string __result)
    // {
        // if (Constants.EnableIcons())
            // __result = __result.Replace("RoleIcons\"", "RoleIcons (Coven)\"");
    // }

    // [HarmonyPatch(nameof(GameSimulation.GetVIPRoleIconAndNameInlineString)), HarmonyPostfix]
    // public static void GetVIPRoleIconAndNameInlineStringPostfix(ref string __result)
    // {
        // if (Constants.EnableIcons())
            // __result = $"{__result.Replace("RoleIcons\"", "RoleIcons (VIP)\"")} <sprite=\"RoleIcons\" name=\"Role201\">";
    // }
}

[HarmonyPatch]
public static class FixDecodingAndEncoding
{
    public static IEnumerable<MethodBase> TargetMethods()
    {
        var baseType = typeof(BaseDecoder);

        foreach (var type in AccessTools.GetTypesFromAssembly(baseType.Assembly).Where(baseType.IsAssignableFrom))
        {
            var method = AccessTools.Method(type, "Decode");

            if (method != null)
                yield return method;
        }
    }

    public static void Postfix(ChatLogMessage chatLogMessage, ref string __result)
    {
        if (!Constants.EnableIcons() || chatLogMessage.chatLogEntry is not ChatLogChatMessageEntry entry)
            return;

        var faction = "Regular";

        if (Utils.GetRoleAndFaction(entry.speakerId, out var tuple))
            faction = Utils.FactionName(tuple.Item2, false);

        var myFact = Pepper.GetMyFaction();

        if (myFact == tuple.Item2 && Constants.CurrentStyle() == "Regular")
            faction = "Regular";
        else if (entry.speakerId == Pepper.GetMyPosition())
            faction = Utils.FactionName(myFact, false);

        __result = __result.Replace("RoleIcons\"", $"RoleIcons ({faction})\"");
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupPotionMaster), nameof(SpecialAbilityPopupPotionMaster.Start)), HarmonyPriority(Priority.Low)]
public static class PmBakerMenuPatch
{
    public static void Postfix(SpecialAbilityPopupPotionMaster __instance)
    {
        if (!Constants.EnableIcons())
            return;

        var role = Pepper.GetMyRole();
        var name = Utils.RoleName(role);
        var faction = Utils.FactionName(Pepper.GetMyFaction());
        var ogfaction = Utils.FactionName(role.GetFactionType(), false);
        var reg = ogfaction != faction;

        var sprite1 = GetSprite(reg, $"{name}_Ability_1", faction);
        var sprite2 = GetSprite(reg, $"{name}_Ability_2", faction);
        var sprite3 = GetSprite(reg, $"{name}_Ability_3", faction);

        if (!sprite1.IsValid() && reg)
            sprite1 = GetSprite($"{name}_Ability_1", ogfaction);

        if (!sprite2.IsValid() && reg)
            sprite2 = GetSprite($"{name}_Ability_2", ogfaction);

        if (!sprite3.IsValid() && reg)
            sprite3 = GetSprite($"{name}_Ability_3", ogfaction);

        var image1 = __instance.transform.Find("Background/ShieldPotion").GetComponentInChildren<Image>();
        var image2 = __instance.transform.Find("Background/RevealPotion").GetComponentInChildren<Image>();
        var image3 = __instance.transform.Find("Background/KillPotion").GetComponentInChildren<Image>();

        if (sprite1.IsValid() && image1)
            image1.sprite = sprite1;

        if (sprite2.IsValid() && image2)
            image2.sprite = sprite2;

        if (sprite3.IsValid() && image3)
            image3.sprite = sprite3;
    }
}

[HarmonyPatch(typeof(DownloadContributorTags), nameof(DownloadContributorTags.AddTMPSprites)), HarmonyPriority(Priority.VeryLow)]
public static class ReplaceTMPSpritesPatch
{
    private static Func<int, string, TMP_SpriteAsset> OldSpriteAssetRequest;
    private static readonly Dictionary<string, TMP_SpriteAsset> CachedAssets = [];

    public static void ClearCache() => CachedAssets.Clear();

    public static void Postfix()
    {
        OldSpriteAssetRequest = Traverse.Create<TMP_Text>().Field<Func<int, string, TMP_SpriteAsset>>("OnSpriteAssetRequest").Value;
        TMP_Text.OnSpriteAssetRequest += Request;;
    }

    private static TMP_SpriteAsset Request(int index, string str)
    {
        if (CachedAssets.TryGetValue(str, out var result))
            return result;

        var requestSuccess = false;

        try
        {
            if (Request(str, out result))
                requestSuccess = true;
        }
        catch (Exception e)
        {
            RunDiagnostics(e);
        }

        if (!requestSuccess)
        {
            if (str.Contains("BTOSRoleIcons"))
                result = BTOS22 ?? BTOS21;
            else if (str.Contains("RoleIcons"))
                result = Vanilla1 ?? CacheDefaults.RoleIcons;
            else
            {
                result = str switch
                {
                    "PlayerNumbers" => Vanilla2 ?? CacheDefaults.Numbers,
                    "Emojis" => Vanilla3 ?? CacheDefaults.Emojis,
                    _ => OldSpriteAssetRequest(index, str)
                };
            }
        }

        CachedAssets[str] = result;
        return result;
    }

    private static bool Request(string str, out TMP_SpriteAsset asset)
    {
        asset = null;

        if (!Constants.EnableIcons())
            return false;

        var packName = Fancy.SelectedIconPack.Value;

        if (!IconPacks.TryGetValue(packName, out var pack))
        {
            Fancy.Instance.Warning($"{packName} doesn't have an icon pack");
            return false;
        }

        if (str.Contains("RoleIcons"))
        {
            var mod = GameModType.Vanilla;

            if (str.Contains("BTOS") || Constants.IsBTOS2() || Utils.FindCasualQueue())
                mod = GameModType.BTOS2;

            var deconstructed = Constants.CurrentStyle();
            var defaultSprite = mod switch
            {
                GameModType.BTOS2 => BTOS22 ?? BTOS21,
                _ => Vanilla1 ?? CacheDefaults.RoleIcons
            };

            if (str.Contains("("))
                deconstructed = str.Split('(', ')').Select(x => x.Trim()).Last(x => !NewModLoading.Utils.IsNullEmptyOrWhiteSpace(x));

            if (deconstructed is "None" or "Blank" || NewModLoading.Utils.IsNullEmptyOrWhiteSpace(deconstructed))
                deconstructed = "Regular";

            if (!pack.Assets.TryGetValue(mod, out var assets))
                Fancy.Instance.Warning($"Unable to find {packName} assets for {mod}");
            else if (deconstructed == "Vanilla")
                asset = defaultSprite;
            else
            {
                if (!assets.MentionStyles.TryGetValue(deconstructed, out asset) || !asset)
                    Fancy.Instance.Warning($"{packName} {mod} Mention Style {deconstructed} was null or missing");

                if (!asset && deconstructed != "Regular")
                {
                    if (!assets.MentionStyles.TryGetValue("Regular", out asset) || !asset)
                        Fancy.Instance.Warning($"{packName} {mod} Mention Style Regular was null or missing");
                }

                if (!asset && deconstructed != "Factionless")
                {
                    if (!assets.MentionStyles.TryGetValue("Factionless", out asset) || !asset)
                        Fancy.Instance.Warning($"{packName} {mod} Mention Style Factionless was null or missing");
                }
            }

            asset ??= defaultSprite;
        }
        else switch (str)
        {
            case "PlayerNumbers":
            {
                if (!pack.PlayerNumbers && Constants.CustomNumbers())
                    Fancy.Instance.Warning($"{packName} PlayerNumber was null");

                asset = pack.PlayerNumbers ?? Vanilla2 ?? CacheDefaults.Numbers;
                return Constants.CustomNumbers() && asset;
            }
            case "Emojis":
            {
                if (!pack.Emojis)
                    Fancy.Instance.Warning($"{packName} Emoji was null");

                asset = pack.Emojis ?? Vanilla3 ?? CacheDefaults.Emojis;
                return asset;
            }
        }

        return (str.Contains("RoleIcons") || str is "PlayerNumbers" or "Emojis") && asset;
    }
}

[HarmonyPatch(typeof(GameModifierPopupController), nameof(GameModifierPopupController.Show)), HarmonyPriority(Priority.VeryLow)]
public static class ChangeGameModifierPopup
{
    public static void Postfix(GameModifierPopupController __instance)
    {
        if (!Constants.EnableIcons())
            return;

        var sprite = GetSprite(Utils.RoleName(__instance.CurrentRole), Utils.FactionName(__instance.CurrentRole.GetFactionType()));

        if (sprite.IsValid() && __instance.roleIcon)
            __instance.roleIcon.sprite = sprite;
    }
}

[HarmonyPatch(typeof(NecroPassingVoteEntry))]
public static class NecroPassPatches
{
    [HarmonyPatch(nameof(NecroPassingVoteEntry.RefreshData)), HarmonyPostfix]
    public static void RefreshDataPatch(NecroPassingVoteEntry __instance)
    {
        if (!Utils.GetRoleAndFaction(__instance.Position, out var tuple))
            return;

        if (Pepper.GetMyPosition() == __instance.Position)
            tuple = new(Pepper.GetMyRole(), Pepper.GetMyFaction());

        var role = tuple.Item1;
        var faction = tuple.Item2;

        if (Constants.EnableIcons())
        {
            var nommy = GetSprite("Necronomicon");

            if (__instance.BookIcon && nommy.IsValid())
                __instance.BookIcon.sprite = nommy;

            var ogfaction = role.GetFactionType();
            var reg = ogfaction != faction;
            var roleName = Utils.RoleName(role);
            var sprite = GetSprite(reg, roleName, Utils.FactionName(faction));

            if (!sprite.IsValid() && reg)
                sprite = GetSprite(roleName, Utils.FactionName(ogfaction));

            if (sprite.IsValid() && __instance.RoleIcon)
                __instance.RoleIcon.sprite = sprite;
        }

        if (__instance.NameAndRole)
		{
			var discussionPlayer = Service.Game.Sim.info.GetDiscussionPlayer(__instance.Position);
			if (discussionPlayer == null)
				return;

			var playerName = discussionPlayer.gameName;
			var roleText = role.ToFactionalDisplayString(faction);
			var colored = role.ToColorizedNoLabel(faction);

			switch (Fancy.NecroPassingFormat.Value)
			{
				case NecroPassingFormatOption.Vanilla:
					__instance.NameAndRole.text =
						$"{playerName}\n<size=24>({roleText})</size>";
					break;

				case NecroPassingFormatOption.Classic:
					__instance.NameAndRole.text =
						$"{colored}\n{playerName}";
					break;

				case NecroPassingFormatOption.InvertedClassic:
					__instance.NameAndRole.text =
						$"{playerName}\n{colored}";
					break;

				default:
					__instance.NameAndRole.text =
						$"{playerName}\n<size=24>({roleText})</size>";
					break;
			}
		}

    }


    [HarmonyPatch(nameof(NecroPassingVoteEntry.UpdateVoteState)), HarmonyPostfix]
    public static void UpdateVoteStatePatch(NecroPassingVoteEntry __instance)
    {
        if (!Constants.EnableIcons())
            return;

        for (var i = 0; i < __instance.m_votes.Count; i++)
        {
            var id = __instance.m_votes[i];

            if (!Utils.GetRoleAndFaction(id, out var tuple))
                continue;

            if (Pepper.GetMyPosition() == id)
                tuple = new(Pepper.GetMyRole(), Pepper.GetMyFaction());

            var bust = __instance.m_roleBusts[i];
            var ogfaction = tuple.Item1.GetFactionType();
            var reg = ogfaction != tuple.Item2;
            var role = Utils.RoleName(tuple.Item1);
            var sprite = GetSprite(reg, role, Utils.FactionName(tuple.Item2));

            if (!sprite.IsValid() && reg)
                sprite = GetSprite(role, Utils.FactionName(ogfaction));

            if (sprite.IsValid() && bust)
                bust.sprite = sprite;
        }
    }
}

[HarmonyPatch(typeof(MentionsProvider))]
public static class MentionsProviderPatches
{
    [HarmonyPatch(nameof(MentionsProvider.ProcessAdvancedRoleMention)), HarmonyPriority(Priority.Low)]
    public static bool Prefix(MentionsProvider __instance, Match roleMatch, string encodedText, string mention, ref string __result)
    {
        if (!Constants.EnableIcons())
            return true;

        if (!int.TryParse(roleMatch.Groups["R"].Value, out var result))
        {
            __result = encodedText;
            return false;
        }

        if (!int.TryParse(roleMatch.Groups["F"].Value, out var result2))
        {
            __result = encodedText;
            return false;
        }

        var role = (Role)result;
        var factionType = (FactionType)result2;
        var text = __instance._useColors ? role.ToColorizedDisplayString(factionType) : role.ToRoleFactionDisplayString(factionType);
        var roleIconsString = Constants.IsBTOS2() ? "BTOSRoleIcons" : "RoleIcons";
        var text2 = __instance._roleEffects ? $"<sprite=\"{roleIconsString} ({Utils.FactionName(factionType)})\" name=\"Role{(int)role}\">" : string.Empty;
        var text3 = $"{__instance.styleTagOpen}{__instance.styleTagFont}<link=\"r{(int)role},{(int)factionType}\">{text2}<b>{text}</b></link>{__instance.styleTagClose}";
        var item = new MentionInfo
        {
            encodedText = mention,
            richText = text3,
            mentionInfoType = MentionInfo.MentionInfoType.ROLE,
            hashCode = text3.ToLower().GetHashCode()
        };

        if (!__instance._textualMentionInfos.Contains(item))
            __instance._textualMentionInfos.Add(item);

        if (!__instance.MentionInfos.Contains(item))
            __instance.MentionInfos.Add(item);

        encodedText = encodedText.ReplaceIcons();

        __result = encodedText.Replace(mention, text3);
        return false;
    }

    [HarmonyPatch(nameof(MentionsProvider.ProcessSpeakerName)), HarmonyPriority(Priority.VeryLow)]
    public static void Postfix(int position, ref string __result)
    {
        if (!Constants.EnableIcons())
            return;

        if (Utils.GetRoleAndFaction(position, out var tuple))
            __result = __result.Replace("RoleIcons\"", $"RoleIcons ({Utils.FactionName(tuple.Item2)})\"");
        else if (position is 69 or 70 or 71)
            __result = __result.Replace("RoleIcons\"", "RoleIcons (Regular)\"");
    }

    [HarmonyPatch(nameof(MentionsProvider.Start))] // Achievement and faction mentions
    // ReSharper disable once InconsistentNaming
    public static void Prefix(ref HashSet<char> ___ExpansionTokens)
    {
        ___ExpansionTokens.Add('~');
        ___ExpansionTokens.Add('$');
    }

    public static Regex FactionalRoleRegex = new Regex(@"<style=(Mention|MentionMono)>(.*?)</style(?=>\$)", RegexOptions.RightToLeft);
    public static Regex EncodedRoleRegex = new Regex(@"\[\[#\d+");

    [HarmonyPatch(nameof(MentionsProvider.ExpandCandidate), typeof(MentionInfo))]
    public static bool Prefix(MentionsProvider __instance, ref MentionInfo candidate)
    {
        if (candidate.mentionInfoType != (MentionInfo.MentionInfoType)10)
            return true;

        var fullText = __instance._matchInfo.fullText;
        var trimmed = __instance._matchInfo.stringPosition == fullText.Length ? fullText : fullText.Remove(__instance._matchInfo.stringPosition);
        var match = FactionalRoleRegex.Match(trimmed);

        if (!match.Success)
        {
            __instance._candidates.Clear();
            return false;
        }

        var value = match.Value + ">";
        var encodedValue = __instance.EncodeText(value);
        var encodedMatch = EncodedRoleRegex.Match(encodedValue);

        if (!encodedMatch.Success)
        {
            __instance._candidates.Clear();
            return false;
        }

        encodedValue = encodedMatch.Value + "," + candidate.encodedText + "]]";
        var finalValue = __instance.DecodeText(encodedValue);

        if (!__instance._matchInfo.endsWithSubmissionCharacter && !__instance._matchInfo.followedBySubmissionCharacter && !__instance._matchInfo.endsWithPunctuationCharacter && !__instance._matchInfo.followedByPunctuationCharacter)
            finalValue += " ";

        __instance._matchInfo.fullText = fullText.Replace(value + __instance._matchInfo.matchString, finalValue);
        __instance._matchInfo.stringPosition = __instance._matchInfo.stringPosition
                                                + (finalValue.Length - (value + __instance._matchInfo.matchString).Length)
                                                + ((__instance._matchInfo.followedBySubmissionCharacter || __instance._matchInfo.endsWithSubmissionCharacter || __instance._matchInfo.followedByPunctuationCharacter || __instance._matchInfo.endsWithPunctuationCharacter) ? 1 : 0);
        __instance._candidates.Clear();
        return false;
    }
}

// This whole class is a mess but DO NOT TOUCH
[HarmonyPatch(typeof(HeaderAnnouncements), nameof(HeaderAnnouncements.ShowHeaderMessage))]
public static class MakeProperFactionChecksInHeaderAnnouncement
{
    public static bool Prefix(HeaderAnnouncements __instance, TrialData trialData, ref IEnumerator __result)
    {
        __result = trialData.trialPhase is TrialPhase.EXECUTION_REVEAL or TrialPhase.HANGMAN_EXECUTION_REVEAL
            ? FixMessage(__instance, trialData)
            : ShowHeaderMessageOriginal(__instance, trialData);
        return false;
    }

    // I touched it!
    private static IEnumerator FixMessage(HeaderAnnouncements __instance, TrialData trialData)
    {
        __instance.Clear();

        if (!Service.Game.Sim.simulation.killRecords.Data.TryFinding(k => k.playerId == trialData.defendantPosition, out var killRecord) || killRecord == null ||
            killRecord.killedByReasons.Count < 1)
        {
            yield break;
        }

        var role = killRecord.playerRole;
        var faction = killRecord.playerFaction;

        var roleText = role.ToColorizedDisplayString(faction);
        var roleIconsString = Constants.IsBTOS2() ? "BTOSRoleIcons" : "RoleIcons";
        var icon = $"<sprite=\"{roleIconsString} ({(Constants.CurrentStyle() == "Regular" && role.GetFactionType() == faction ? "Regular" : Utils.FactionName(faction, false))})\" name=\"Role{(int)role}\">";
        var display = icon + roleText;

        var l10nKey = Fancy.YouAreARole ? Utils.GetHangingMessage2(role, faction) : "GUI_GAME_WHO_DIED_AND_HOW_1_2";
        var formattedLine = __instance.l10n(l10nKey).Replace("%role%", display);

        var name = Service.Game.Sim.simulation.GetDisplayName(trialData.defendantPosition).ToWhiteNameString();
        formattedLine = formattedLine.Replace("%name%", name);
        formattedLine = formattedLine.ReplaceIcons();

        __instance.AddLine(formattedLine);
    }

    [HarmonyReversePatch]
    private static IEnumerator ShowHeaderMessageOriginal(HeaderAnnouncements instance, TrialData trialData) => throw new NotImplementedException();
}

[HarmonyPatch(typeof(WhoDiedAndHowPanel), nameof(WhoDiedAndHowPanel.HandleSubphaseRole))]
public static class MakeProperFactionChecksInWdah1
{
    public static bool Prefix(WhoDiedAndHowPanel __instance)
    {
        Debug.Log("WhoDiedAndHowPanel:: HandleSubphaseRole");

        __instance.deathNotePanel.canvasGroup.DisableRenderingAndInteraction();

        if (!Service.Game.Sim.simulation.killRecords.Data.TryFinding(k => k.playerId == __instance.currentPlayerNumber, out var killRecord) || killRecord!.killedByReasons.Count < 1)
            return false;

        var roleIconsString = Constants.IsBTOS2() ? "BTOSRoleIcons" : "RoleIcons";
        var roleText = $"<sprite=\"{roleIconsString}\" name=\"Role{(int)killRecord.playerRole}\">{killRecord.playerRole.ToColorizedDisplayString(killRecord.playerFaction)}";

        if (Constants.EnableIcons())
            roleText = roleText.Replace("RoleIcons\"", $"RoleIcons ({Utils.FactionName(killRecord.playerFaction)})\"");

        string newLine;
        if (killRecord.playerRole is Role.NONE or Role.UNKNOWN)
        {
            newLine = Utils.GetString("GUI_GAME_WHO_DIED_VICTIM_ROLE_UNKNOWN");
        }
        else
        {
            if (killRecord.playerRole == Role.STONED)
            {
                newLine = Utils.GetString("GUI_GAME_WHO_DIED_VICTIM_ROLE_STONED");
            }

            newLine = (killRecord.playerRole != Role.HIDDEN) ? Utils.GetString("GUI_GAME_WHO_DIED_VICTIM_ROLE_KNOWN") : Utils.GetString("GUI_GAME_WHO_DIED_VICTIM_ROLE_HIDDEN");
            newLine = newLine.Replace("%role%", killRecord.playerRole.GetTMPSprite() + killRecord.playerRole.ToColorizedDisplayString(killRecord.playerFaction));
        }

        var text = Fancy.YouAreARole ? Utils.GetString(Utils.GetWdahMessage(killRecord.playerRole, killRecord.playerFaction))
            .Replace("%role%", roleText)
            .ReplaceIcons() : newLine.ReplaceIcons();

        __instance.AddLine(text, 1f);
        return false;
    }
}

[HarmonyPatch(typeof(WhoDiedAndHowPanel), nameof(WhoDiedAndHowPanel.HandleSubphaseWhoDied))]
public static class MakeProperFactionChecksInWdah2
{
    public static bool Prefix(WhoDiedAndHowPanel __instance, float phaseTime)
    {
        // if (!Constants.EnableIcons()) return true;

        Debug.Log($"HandleSubphaseWhoDied phaseTime = {phaseTime}");

        if (__instance.tombstonePanel != null)
            __instance.tombstonePanel.Clear();
        else
            Debug.LogWarning("WhoDiedAndHowPanel.HandleSubphaseWhoDiedAndHow: tombstonePanel was null.");

        if (__instance.lines != null)
            __instance.lines.Clear();
        else
            Debug.LogWarning("WhoDiedAndHowPanel.HandleSubphaseWhoDiedAndHow: lines list was null.");

        var playerName = Service.Game.Cast.GetPlayerName(__instance.currentPlayerNumber);
        var text = __instance.l10n("GUI_GAME_WHO_DIED_AND_HOW_START").Replace("%name%", playerName);

        if (Service.Game.Sim.simulation.killRecords.Data.TryFinding(k => k.playerId == __instance.currentPlayerNumber, out var killRecord) && killRecord!.killedByReasons.First()
            .IsDaytimeKillReason())
        {
            text = __instance.l10n("GUI_GAME_WHO_DIED_AND_HOW_DAYKILL_START").Replace("%name%", playerName);
        }

        __instance.AddLine(text, Tuning.REVEAL_TIME_PER_ADDL_KILLED_BY_REASON);

        if (killRecord == null || killRecord.killedByReasons.Count < 1)
            __instance.AddLine(__instance.l10n("GUI_GAME_KILLED_BY_REASON_0"), Tuning.REVEAL_TIME_PER_ADDL_KILLED_BY_REASON);
		else
		{
			var isBTOS2 = Constants.IsBTOS2();
			var loc = Service.Home.LocalizationService;

			for (var i = 0; i < killRecord.killedByReasons.Count; i++)
			{
				var killedByReason = killRecord.killedByReasons[i];
				var also = i == 0 ? "" : "_ALSO";
				var reasonId = (int)killedByReason;

				var fancy = $"FANCY_GUI_GAME{also}_KILLED_BY_REASON_{reasonId}";
				var normal = $"GUI_GAME{also}_KILLED_BY_REASON_{reasonId}";

				// var text2 = __instance.l10n($"GUI_GAME{also}_KILLED_BY_REASON_{(int)killedByReason}");
				var text2 = Utils.TryGetString(fancy, normal, false);

				text2 = text2.Replace("RoleIcons\"", "RoleIcons (Regular)\"");

				if (isBTOS2)
					text2 = text2.Replace("\"RoleIcons", "\"BTOSRoleIcons").Replace("106\"", "107\"");
				else
					text2 = text2.Replace("\"BTOSRoleIcons", "\"RoleIcons").Replace("107\"", "106\"");

				__instance.AddLine(text2, Tuning.REVEAL_TIME_PER_ADDL_KILLED_BY_REASON);
			}
		}
        Debug.Log("WhoDiedAndHowPanel:: HandleSubphaseWhoDied");
        return false;
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupOracleListItem), nameof(SpecialAbilityPopupOracleListItem.SetData))]
public static class PatchOracleMenu
{
    public static void Postfix(SpecialAbilityPopupOracleListItem __instance, Role a_role)
    {
        if (!Constants.EnableIcons())
            return;

        var sprite = GetSprite(Utils.RoleName(a_role), Utils.FactionName(a_role.GetFactionType()));

        if (sprite.IsValid() && __instance.roleIcon)
            __instance.roleIcon.sprite = sprite;
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupNecromancerRetributionist), nameof(SpecialAbilityPopupNecromancerRetributionist.Start))]
public static class PatchNecroRetMenu
{
    public static void Postfix(SpecialAbilityPopupNecromancerRetributionist __instance)
    {
        if (!Constants.EnableIcons())
            return;

        var sprite = GetSprite(Utils.RoleName(Pepper.GetMyRole()), Utils.FactionName(Pepper.GetMyFaction()));

        if (sprite.IsValid() && __instance.roleIcon)
            __instance.roleIcon.sprite = sprite;
    }
}


[HarmonyPatch(typeof(SpecialAbilityPopupNecromancerRetributionistListItem), nameof(SpecialAbilityPopupNecromancerRetributionistListItem.SetData))]
public static class PatchNecroRetMenuItem
{
    public static void Postfix(SpecialAbilityPopupNecromancerRetributionistListItem __instance, Role role)
    {
        if (!Constants.EnableIcons())
            return;

        var myRole      = Pepper.GetMyRole();
        var myRoleName  = Utils.RoleName(myRole);
        var myFaction   = Utils.FactionName(Pepper.GetMyFaction());
        var myOgFaction = Utils.FactionName(myRole.GetFactionType(), false);

        var specialName = $"{myRoleName}_Special";
        var special =
            GetSprite(specialName, myFaction, false) ??
            GetSprite(specialName, myOgFaction, false);

        if (special.IsValid() && __instance.choiceSprite)
            __instance.choiceSprite.sprite = special;

        var targetRoleName = Utils.RoleName(role);
        var targetFaction  = Utils.FactionName(role.GetFactionType(), false);

        RoleCardData roleCardData = SharedRoleData.GetRoleCardData(role);

        string abilitySuffix;
		string abilityText;

        if (roleCardData != null)
        {
            if (roleCardData.normalAbilityAvailable)
			{
				abilitySuffix = "Ability";
				abilityText = Utils.GetString($"{Utils.GetKeyPrefix()}_ROLE_ABILITY1_VERB_{(int)role}");
			}
            else if (roleCardData.secondAbilityAvailable)
			{
				abilitySuffix = "Ability_2";
				abilityText = Utils.GetString($"{Utils.GetKeyPrefix()}_ROLE_ABILITY2_VERB_{(int)role}");
			}
            else if (roleCardData.specialAbilityAvailable)
			{
				var btos = Constants.IsBTOS2() ? string.Empty : "_";
				abilitySuffix = "Special";
				abilityText = Utils.GetString($"{Utils.GetKeyPrefix()}_ROLE_SPECIAL{btos}ABILITY_VERB_{(int)role}");
			}
            else
			{
				abilitySuffix = "Ability";
				abilityText = Utils.GetString($"{Utils.GetKeyPrefix()}_ROLE_ABILITY1_VERB_{(int)role}");
			}
        }
        else
        {
            abilitySuffix = "Ability";
			abilityText = Utils.GetString($"{Utils.GetKeyPrefix()}_ROLE_ABILITY1_VERB_{(int)role}");
        }

        var abilityName = $"{targetRoleName}_{abilitySuffix}";

        var abilitySprite =
            GetSprite(abilityName, targetFaction, false) ??
            GetSprite($"{abilityName}_1", targetFaction, false);

        if (abilitySprite.IsValid() && __instance.choice2Sprite)
            __instance.choice2Sprite.sprite = abilitySprite;
		
		if (__instance.choice2Text)
			__instance.choice2Text.text = abilityText;
		
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupGenericDualTarget), nameof(SpecialAbilityPopupGenericDualTarget.Start))]
public static class PatchDualAbilityMenu
{
    public static void Postfix(SpecialAbilityPopupGenericDualTarget __instance)
    {
        if (!Constants.EnableIcons())
            return;

        var sprite = GetSprite(Utils.RoleName(Pepper.GetMyRole()), Utils.FactionName(Pepper.GetMyFaction()));

        if (sprite.IsValid() && __instance.roleIcon)
            __instance.roleIcon.sprite = sprite;
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupGenericDualTargetListItem), nameof(SpecialAbilityPopupGenericDualTargetListItem.SetData))]
public static class PatchDualAbilityMenuItem
{
    public static void Postfix(SpecialAbilityPopupGenericDualTargetListItem __instance, int position)
    {
        if (!Constants.EnableIcons())
            return;

        var role        = Pepper.GetMyRole();
        var roleName    = Utils.RoleName(role);
        var faction     = Utils.FactionName(Pepper.GetMyFaction());
        var ogFaction   = Utils.FactionName(role.GetFactionType(), false);

        var baseName = $"{roleName}_Special";

        var sprite = GetSprite(baseName, faction, false);


        if (!sprite.IsValid())
            sprite = GetSprite(baseName, ogFaction, false);

        if (!sprite.IsValid())
            return;

        if (__instance.choiceSprite)
            __instance.choiceSprite.sprite = sprite;

        if (__instance.choice2Sprite)
            __instance.choice2Sprite.sprite = sprite;
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupDayConfirmListItem), nameof(SpecialAbilityPopupDayConfirmListItem.SetData))]
public static class PatchDayConfirmAbilityMenuItem
{
    public static void Postfix(SpecialAbilityPopupDayConfirmListItem __instance, int position, string player_name, Sprite headshot, bool hasChoice1, UIRoleData data)
    {
        if (!Constants.EnableIcons())
            return;

        var role      = Pepper.GetMyRole();
        var roleName  = Utils.RoleName(role);
        var faction   = Utils.FactionName(Pepper.GetMyFaction());
        var ogFaction = Utils.FactionName(role.GetFactionType(), false);

        var baseName = $"{roleName}_Special";

        var sprite = GetSprite(baseName, faction, false);

        if (!sprite.IsValid())
            sprite = GetSprite(baseName, ogFaction, false);

        if (!sprite.IsValid())
            return;

        if (__instance.choiceSprite)
            __instance.choiceSprite.sprite = sprite;
    }
}
[HarmonyPatch(typeof(PirateProgressLevelElement), nameof(PirateProgressLevelElement.SetData), typeof(Role), typeof(Role), typeof(Role), typeof(bool), typeof(bool), typeof(bool))]
public static class PatchPirateMenu
{
    public static void Postfix(PirateProgressLevelElement __instance, Role role1, Role role2, Role role3)
    {
        if (!Constants.EnableIcons())
            return;

        var role = Utils.RoleName(role1);
        var faction = Utils.FactionName(role1.GetFactionType());
        var sprite = GetSprite(role, faction);

        if (sprite.IsValid() && __instance.Role1)
            __instance.Role1.sprite = sprite;

        role = Utils.RoleName(role2);
        faction = Utils.FactionName(role2.GetFactionType());
        sprite = GetSprite(role, faction);

        if (sprite.IsValid() && __instance.Role2)
            __instance.Role2.sprite = sprite;

        role = Utils.RoleName(role3);
        faction = Utils.FactionName(role3.GetFactionType());
        sprite = GetSprite(role, faction);

        if (sprite.IsValid() && __instance.Role3)
            __instance.Role3.sprite = sprite;
    }
}

[HarmonyPatch(typeof(Tos2GameBrowserListItem), nameof(Tos2GameBrowserListItem.Initialize))]
public static class Tos2GameBrowserListItemPatch
{
    public static void Postfix(Tos2GameBrowserListItem __instance)
    {
        if (!Constants.EnableIcons())
            return;

        var town = __instance.transform.GetComponent<Image>("Icon_Townie");
        var coven = __instance.transform.GetComponent<Image>("Icon_Traitor");
        var neut = __instance.transform.GetComponent<Image>("Icon_Neutral");
        var any = __instance.transform.GetComponent<Image>("Icon_Any");
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
    }
}

// [HarmonyPatch(typeof(HudDockItem))]
// [HarmonyPatch(nameof(HudDockItem.Start))]
// [HarmonyPatch(nameof(HudDockItem.OnEnable))]
// [HarmonyPatch(nameof(HudDockItem.HandleOnGameInfoChanged))]
// public static class DockItemIconPatches
// {
    // [HarmonyPostfix]
    // public static void Postfix(HudDockItem __instance)
    // {
        // if (!Constants.EnableIcons())
            // return;

        // Utils.ApplyDockItemIcon(__instance);
    // }
// }

// [HarmonyPatch(typeof(HudDockPanel), nameof(HudDockPanel.RefreshNavigation))]
// public static class HudDockPanel_RefreshNavigation_Patch
// {
    // [HarmonyPostfix]
    // public static void Postfix(HudDockPanel __instance)
    // {
        // if (!Constants.EnableIcons())
            // return;

        // if (__instance == null)
            // return;

        // var items = __instance.GetComponentsInChildren<HudDockItem>(true);
        // if (items == null || items.Length == 0)
            // return;

        // foreach (var item in items)
        // {
            // Utils.ApplyDockItemIcon(item);
        // }
    // }
// }

// [HarmonyPatch(typeof(HudDockPanel), nameof(HudDockPanel.RefreshNavigation))]
// public static class HudDockPanel_RefreshNavigation_Patch
// {
    // [HarmonyPostfix]
    // public static void Postfix(HudDockPanel __instance)
    // {
        // if (!Constants.EnableIcons())
            // return;

        // foreach (var item in __instance.GetComponentsInChildren<HudDockItem>(true))
        // {
            // Utils.ApplyDockItemIcon(item);
        // }
    // }
// }

// Role bucket header
[HarmonyPatch(typeof(RoleListPopupController), nameof(RoleListPopupController.Show))]
public static class RoleListHeaderIcon
{
    public static bool Prefix(RoleListPopupController __instance, Role role, Role role2, FactionType faction, FactionType faction2)
    {
        __instance.gameObject.SetActive(true);

        if (role2 == Role.NONE)
        {
            var icon1 = role.GetTMPSprite();
            icon1 = icon1.Replace("RoleIcons\"", $"RoleIcons ({Utils.FactionName(faction, false)})\"");

            __instance.RoleNameLabel.text = icon1 + role.ToColorizedDisplayString(faction);
        }
        else
        {
            var icon1 = role.GetTMPSprite();
            var icon2 = role2.GetTMPSprite();
            icon1 = icon1.Replace("RoleIcons\"", $"RoleIcons ({Utils.FactionName(faction, false)})\"");
            icon2 = icon2.Replace("RoleIcons\"", $"RoleIcons ({Utils.FactionName(faction2, false)})\"");

            __instance.RoleNameLabel.text = string.Concat(
                icon1,
                role.ToColorizedShortenedDisplayString(faction),
                " <color=#808080>-</color> ",
                icon2,
                role2.ToColorizedShortenedDisplayString(faction2)
            );
        }

        Role[] roles = [role, role2];
        FactionType[] factions = [faction, faction2];
        __instance.randomRoleListController.BuildList(roles, factions, null);

        Service.Game.Interface.PopupController.DefaultRoleListPopupAnchor = __instance.DefaultRoleListPopupAnchor;
        Service.Game.Interface.PopupController.IsRoleListPopupOpen = true;
        Service.Game.Interface.PopupController.UpdateRoleListPopupAnchor();

        return false;
    }
}
[HarmonyPatch(typeof(RoleListItem))]
public static class RoleListItemIcon
{
    [HarmonyPatch(nameof(RoleListItem.RefreshLabel))]
    [HarmonyPostfix]
    public static void RefreshLabel_Postfix(RoleListItem __instance)
    {
        ApplyCustomLabel(__instance);
    }

    [HarmonyPatch(nameof(RoleListItem.SetRole))]
    [HarmonyPostfix]
    public static void SetRole_Postfix(RoleListItem __instance)
    {
        ApplyCustomLabel(__instance);
    }

    private static void ApplyCustomLabel(RoleListItem __instance)
    {
        if (__instance.role == Role.NONE)
            return;

        string roleText;


        if (__instance.role2 == Role.NONE)
        {
			var role1Sprite = __instance.role.GetTMPSprite();
			var role1Text = __instance.role.ToColorizedDisplayString(__instance.faction);
			role1Sprite = role1Sprite.Replace("RoleIcons", $"RoleIcons ({Utils.FactionName(__instance.faction, false)})");
            roleText = role1Sprite + role1Text;
        }
        else
        {
			var role1Sprite = __instance.role.GetTMPSprite();
			var role1Text = __instance.role.ToColorizedShortenedDisplayString(__instance.faction);
            var role2Sprite = __instance.role2.GetTMPSprite();
			role1Sprite = role1Sprite.Replace("RoleIcons", $"RoleIcons ({Utils.FactionName(__instance.faction, false)})");
			role2Sprite = role2Sprite.Replace("RoleIcons", $"RoleIcons ({Utils.FactionName(__instance.faction2, false)})");
			
            var role2Text = __instance.role2.ToColorizedShortenedDisplayString(__instance.faction2);

            roleText = $"{role1Sprite}{role1Text} <color=#FFFFFF40>-</color> {role2Sprite}{role2Text}";
        }

        __instance.roleLabel.SetText(roleText);

        __instance.ValidateCrossOut();
    }
}
[HarmonyPatch(typeof(RandomRoleListItemController))]
public static class RoleListPopupUpdate
{
    private static FactionType GetEffectiveFaction(RoleData roleData)
    {
        if (roleData.factionType == FactionType.NONE)
            return roleData.role.GetFactionType();
        return roleData.factionType;
    }

    private static string BuildRoleText(RoleData roleData)
    {
        var factionType = GetEffectiveFaction(roleData);
		var icon = roleData.role.GetTMPSprite();
		icon = icon.Replace("RoleIcons\"", $"RoleIcons ({Utils.FactionName(factionType, false)})\"");

        var roleText = icon + roleData.role.ToColorizedDisplayString(factionType);
        return roleText;
    }

    [HarmonyPatch(nameof(RandomRoleListItemController.HandleOnRoleDeckBuilderChanged))]
    public static bool Prefix(RandomRoleListItemController __instance, RoleDeckBuilder roleDeckBuilder)
    {
        var banPanel = __instance.BannedPanelGO;
        var roleData = __instance.m_roleData;
        var roleName = __instance.RoleNameLabel;

        if (roleDeckBuilder.bannedRoles.Contains(roleData.role))
            roleData.factionType = FactionType.NONE;

        var roleText = BuildRoleText(roleData);

        if (roleDeckBuilder.bannedRoles.Contains(roleData.role))
        {
            if (!banPanel.activeSelf)
            {
                roleName.SetText("<color=#8C8C8C>" + roleText + "</color>");
                banPanel.SetActive(true);
            }
        }
        else
        {
            roleName.SetText(roleText);
            banPanel.SetActive(false);
        }

        return false;
    }

    [HarmonyPatch(nameof(RandomRoleListItemController.OnDataSet))]
    public static bool Prefix(RandomRoleListItemController __instance, RoleData roleData)
    {
        __instance.m_roleData = roleData;

        var banPanel = __instance.BannedPanelGO;
        var roleName = __instance.RoleNameLabel;

        var deckBuilder = Service.Game.Sim.simulation.roleDeckBuilder.Data;

        if (deckBuilder.bannedRoles.Contains(roleData.role))
            roleData.factionType = FactionType.NONE;

        var roleText = BuildRoleText(roleData);

        if (deckBuilder.bannedRoles.Contains(roleData.role))
        {
            roleName.SetText("<color=#8C8C8C>" + roleData.role.ToDisplayString() + "</color>");
            banPanel.SetActive(true);
        }
        else
        {
            roleName.SetText(roleText);
            banPanel.SetActive(false);
        }

        return false;
    }
}