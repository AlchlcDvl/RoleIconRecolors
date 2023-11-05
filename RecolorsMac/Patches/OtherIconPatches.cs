using Game.Interface;
using Server.Shared.Extensions;
using Server.Shared.State;

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