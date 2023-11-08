using Game.Interface;
using Server.Shared.Extensions;
using Server.Shared.State;
using Home.GameBrowser;
using SalemModLoaderUI;
using Server.Shared.Info;
using Services;

namespace RecolorsWindows;

[HarmonyPatch(typeof(RoleCardListItem), nameof(RoleCardListItem.SetData))]
public static class PatchRoleDeckBuilder
{
    public static void Postfix(RoleCardListItem __instance, ref Role role)
    {
        if (!Constants.EnableIcons || role.IsModifierCard())
            return;

        var icon = AssetManager.GetSprite($"{Utils.RoleName(role)}", false);

        if (__instance.roleImage != null && icon != AssetManager.Blank)
            __instance.roleImage.sprite = icon;
    }
}

[HarmonyPatch(typeof(RoleDeckListItem), nameof(RoleDeckListItem.SetData))]
public static class PatchRoleListPanel
{
    public static void Postfix(RoleDeckListItem __instance, ref Role a_role, ref bool a_isBan)
    {
        if (!Constants.EnableIcons || a_role.IsModifierCard() || a_isBan)
            return;

        var icon = AssetManager.GetSprite($"{Utils.RoleName(a_role)}", false);

        if (__instance.roleImage != null && icon != AssetManager.Blank)
            __instance.roleImage.sprite = icon;
    }
}

[HarmonyPatch(typeof(GameBrowserRoleDeckListItem), nameof(GameBrowserRoleDeckListItem.SetData))]
public static class PatchBrowserRoleListPanel
{
    public static void Postfix(GameBrowserRoleDeckListItem __instance, ref Role a_role, ref bool a_isBan)
    {
        if (!Constants.EnableIcons || a_role.IsModifierCard() || a_isBan)
            return;

        var icon = AssetManager.GetSprite($"{Utils.RoleName(a_role)}", false);

        if (__instance.roleImage != null && icon != AssetManager.Blank)
            __instance.roleImage.sprite = icon;
    }
}

[HarmonyPatch(typeof(SalemModLoaderMainMenuController), nameof(SalemModLoaderMainMenuController.ClickMainButton))]
public static class PatchSMLSettings
{
    public static void Postfix() => AssetManager.TryLoadingSprites(Constants.CurrentPack);
}

[HarmonyPatch(typeof(RoleCardPanelBackground), nameof(RoleCardPanelBackground.SetRole))]
public class PatchRoleCards
{
    public static void Postfix(RoleCardPanelBackground __instance, ref Role role)
    {
        var panel = __instance.GetComponentInParent<RoleCardPanel>();
        panel.roleNameText.text = role.DisplayString(__instance.currentFaction);

        if (!Constants.EnableIcons)
            return;

        var index = 0;
        var name = Utils.RoleName(role);
        var sprite = role.IsTraitor(Pepper.GetMyFaction()) ? AssetManager.GetTTSprite(name) : AssetManager.GetSprite(name);

        if (sprite != AssetManager.Blank && panel.roleIcon != null)
            panel.roleIcon.sprite = sprite;

        var special = AssetManager.GetSprite($"{name}_Special");

        if (special != AssetManager.Blank && panel.specialAbilityPanel != null)
            panel.specialAbilityPanel.useButton.abilityIcon.sprite = special;

        var abilityname = $"{name}_Ability";
        var ability1 = AssetManager.GetSprite(abilityname);

        if (ability1 == AssetManager.Blank)
            abilityname += "_1";

        ability1 = AssetManager.GetSprite(abilityname);

        if (ability1 != AssetManager.Blank && panel.roleInfoButtons[index] != null)
        {
            panel.roleInfoButtons[index].abilityIcon.sprite = ability1;
            index++;
        }

        var ability2 = AssetManager.GetSprite($"{name}_Ability_2");

        if (ability2 != AssetManager.Blank && panel.roleInfoButtons[index] != null)
        {
            panel.roleInfoButtons[index].abilityIcon.sprite = ability2;
            index++;
        }

        var attribute = AssetManager.GetSprite($"Attributes_{Utils.FactionName(Pepper.GetMyFaction(), role)}");

        if (attribute != AssetManager.Blank && panel.roleInfoButtons[index] != null)
        {
            panel.roleInfoButtons[index].abilityIcon.sprite = attribute;
            index++;
        }

        var nommy = AssetManager.GetSprite("Necronomicon");

        if (nommy != AssetManager.Blank && Service.Game.Sim.info.roleCardObservation.Data.powerUp == POWER_UP_TYPE.NECRONOMICON && panel.roleInfoButtons[index] != null)
        {
            panel.roleInfoButtons[index].abilityIcon.sprite = nommy;
            index++;
        }
    }
}

[HarmonyPatch(typeof(TosAbilityPanelListItem), nameof(TosAbilityPanelListItem.OverrideIconAndText))]
public static class AbilityPanelStartPatch
{
    public static void Postfix(TosAbilityPanelListItem __instance, ref TosAbilityPanelListItem.OverrideAbilityType overrideType)
    {
        if (!Constants.EnableIcons || overrideType == TosAbilityPanelListItem.OverrideAbilityType.VOTING)
            return;

        switch (overrideType)
        {
            case TosAbilityPanelListItem.OverrideAbilityType.NECRO_ATTACK:
                var nommy = AssetManager.GetSprite("Necronomicon", false);

                if (nommy != AssetManager.Blank)
                {
                    if (Pepper.GetMyRole() == Role.ILLUSIONIST && __instance.choice2Sprite != null)
                    {
                        __instance.choice2Sprite.sprite = nommy;
                        var illu = AssetManager.GetSprite("Illusionist_Ability", Constants.PlayerPanelEasterEggs);

                        if (illu != AssetManager.Blank && __instance.choice1Sprite != null)
                            __instance.choice1Sprite.sprite = illu;
                    }
                    else if (__instance.choice1Sprite != null)
                    {
                        __instance.choice1Sprite.sprite = nommy;

                        if (Pepper.GetMyRole() == Role.WITCH)
                        {
                            var target = AssetManager.GetSprite("Witch_Ability_2", Constants.PlayerPanelEasterEggs);

                            if (target != AssetManager.Blank && __instance.choice2Sprite != null)
                                __instance.choice2Sprite.sprite = target;
                        }
                    }
                }

                break;

            case TosAbilityPanelListItem.OverrideAbilityType.POTIONMASTER_ATTACK:
                var attack = AssetManager.GetSprite("PotionMaster_Special", Constants.PlayerPanelEasterEggs);

                if (attack != AssetManager.Blank && __instance.choice1Sprite != null)
                    __instance.choice1Sprite.sprite = attack;

                break;

            case TosAbilityPanelListItem.OverrideAbilityType.POTIONMASTER_HEAL:
                var heal = AssetManager.GetSprite("PotionMaster_Ability_1", Constants.PlayerPanelEasterEggs);

                if (heal != AssetManager.Blank && __instance.choice1Sprite != null)
                    __instance.choice1Sprite.sprite = heal;

                break;

            case TosAbilityPanelListItem.OverrideAbilityType.POTIONMASTER_REVEAL:
                var reveal = AssetManager.GetSprite("PotionMaster_Ability_2", Constants.PlayerPanelEasterEggs);

                if (reveal != AssetManager.Blank && __instance.choice1Sprite != null)
                    __instance.choice1Sprite.sprite = reveal;

                break;

            case TosAbilityPanelListItem.OverrideAbilityType.POISONER_POISON:
                var poison = AssetManager.GetSprite("Poisoner_Special", Constants.PlayerPanelEasterEggs);

                if (poison != AssetManager.Blank && __instance.choice1Sprite != null)
                    __instance.choice1Sprite.sprite = poison;

                break;

            case TosAbilityPanelListItem.OverrideAbilityType.SHROUD:
                var shroud = AssetManager.GetSprite("Shroud_Special", Constants.PlayerPanelEasterEggs);

                if (shroud != AssetManager.Blank && __instance.choice1Sprite != null)
                    __instance.choice1Sprite.sprite = shroud;

                break;

            case TosAbilityPanelListItem.OverrideAbilityType.WEREWOLF_NON_FULL_MOON:
                var sniff = AssetManager.GetSprite("Werewolf_Ability_2", Constants.PlayerPanelEasterEggs);

                if (sniff != AssetManager.Blank && __instance.choice1Sprite != null)
                    __instance.choice1Sprite.sprite = sniff;

                break;

            default:
                var abilityName = $"{Utils.RoleName(Pepper.GetMyRole())}_Ability";
                var ability1 = AssetManager.GetSprite(abilityName, Constants.PlayerPanelEasterEggs);

                if (ability1 == AssetManager.Blank)
                    abilityName += "_1";

                ability1 = AssetManager.GetSprite(abilityName, Constants.PlayerPanelEasterEggs);

                if (ability1 != AssetManager.Blank && __instance.choice1Sprite != null)
                    __instance.choice1Sprite.sprite = ability1;

                var ability2 = AssetManager.GetSprite($"{Utils.RoleName(Pepper.GetMyRole())}_Ability_2", Constants.PlayerPanelEasterEggs);

                if (ability2 != AssetManager.Blank && __instance.choice2Sprite != null)
                    __instance.choice2Sprite.sprite = ability2;

                break;
        }
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupGenericListItem), nameof(SpecialAbilityPopupGenericListItem.SetData))]
public static class SpecialAbilityPanelPatch
{
    public static void Postfix(SpecialAbilityPopupGenericListItem __instance)
    {
        if (!Constants.EnableIcons)
            return;

        var special = AssetManager.GetSprite($"{Utils.RoleName(Pepper.GetMyRole())}_Special");

        if (special != AssetManager.Blank)
            __instance.choiceSprite.sprite = special;
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupRadialIcon), nameof(SpecialAbilityPopupRadialIcon.SetData))]
public static class PatchRitualistGuessMenu
{
    public static void Postfix(SpecialAbilityPopupRadialIcon __instance, ref Role a_role)
    {
        if (!Constants.EnableIcons)
            return;

        var icon = AssetManager.GetSprite($"{Utils.RoleName(a_role)}");

        if (__instance.roleIcon != null && icon != AssetManager.Blank)
            __instance.roleIcon.sprite = icon;
    }
}