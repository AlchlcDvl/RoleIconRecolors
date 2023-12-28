namespace IconPacks.Patches;

public static class PatchHandler
{
    public static void PatchRoleDeckBuilder(RoleCardListItem __instance, Role role, GameType type)
    {
        if (!Constants.EnableIcons)
            return;

        Recolors.LogMessage("Patching RoleCardListItem.SetData");

        if (type == GameType.Vanilla)
            VanillaPatches.PatchRoleDeckBuilder(__instance, role);
    }
}