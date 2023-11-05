using Game.Interface;
using Server.Shared.Extensions;
using Server.Shared.State;
using Home.GameBrowser;
using Home.Common.Settings;
using SalemModLoaderUI;

namespace RecolorsMac.Patches;

[HarmonyPatch(typeof(RoleCardListItem), nameof(RoleCardListItem.SetData))]
public static class PatchRoleDeckBuilder
{
    public static void Postfix(RoleCardListItem __instance, ref Role role)
    {
        if (!Constants.EnableIcons || role.IsModifierCard() || role.IsBucket())
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
        if (!Constants.EnableIcons || a_role.IsModifierCard() || a_role.IsBucket() || a_isBan)
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
        if (!Constants.EnableIcons || a_role.IsModifierCard() || a_role.IsBucket() || a_isBan)
            return;

        var icon = AssetManager.GetSprite($"{Utils.RoleName(a_role)}", false);

        if (__instance.roleImage != null && icon != AssetManager.Blank)
            __instance.roleImage.sprite = icon;
    }
}

[HarmonyPatch(typeof(SettingsController), nameof(SettingsController.PopulateListItems))]
public static class PatchSettingsGuidePanels
{
    public static void Postfix(SettingsController __instance)
    {
        if (!Constants.EnableIcons)
            return;

        foreach (var guidePanel in __instance._gameGuideListItems)
        {
            if (guidePanel.categoryItemBelongsTo != SettingsController.GameGuideCategory.ROLES)
                continue;

            var panel = guidePanel.rolecardPopup;
            var role = guidePanel.role;
            var index = 0;
            var name = Utils.RoleName(role);
            var sprite = AssetManager.GetSprite(name);

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

            if (ability1 != AssetManager.Blank && panel.roleInfoButtons.Count < index + 1)
            {
                panel.roleInfoButtons[index].abilityIcon.sprite = ability1;
                index++;
            }

            var ability2 = AssetManager.GetSprite($"{name}_Ability_2");

            if (ability2 != AssetManager.Blank && panel.roleInfoButtons.Count < index + 1)
            {
                panel.roleInfoButtons[index].abilityIcon.sprite = ability2;
                index++;
            }

            var attribute = AssetManager.GetSprite($"Attributes_{Utils.FactionName(role.GetFaction(), role)}");

            if (attribute != AssetManager.Blank && panel.roleInfoButtons.Count < index + 1)
            {
                panel.roleInfoButtons[index].abilityIcon.sprite = attribute;
                index++;
            }

            var nommy = AssetManager.GetSprite("Necronomicon");

            if (nommy != AssetManager.Blank && panel.roleInfoButtons.Count < index + 1)
            {
                panel.roleInfoButtons[index].abilityIcon.sprite = nommy;
                index++;
            }
        }
    }
}

[HarmonyPatch(typeof(SalemModLoaderMainMenuController), nameof(SalemModLoaderMainMenuController.ClickMainButton))]
public static class PatchSMLSettings
{
    public static void Postfix() => AssetManager.TryLoadingSprites(Constants.CurrentPack);
}