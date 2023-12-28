namespace IconPacks.Patches;

public static class VanillaPatches
{
    public static void PatchRoleDeckBuilder(RoleCardListItem __instance, Role role)
    {
        if (!Constants.EnableIcons)
            return;

        Recolors.LogMessage("Patching RoleCardListItem.SetData");
        var icon = AssetManager.GetSprite($"{Utils.RoleName(role)}", false);

        if (__instance.roleImage && icon != AssetManager.Blank)
            __instance.roleImage.sprite = icon;
    }
}