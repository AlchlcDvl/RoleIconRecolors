using Game.Characters;
using Home.Shared;
using Server.Shared.Extensions;

namespace FancyUI.Patches;

/*
* TO DO: Add patches for the following:
* Reanimate Menu
* Special Ability Panel
*/

[HarmonyPatch(typeof(ClientRoleExtensions), nameof(ClientRoleExtensions.ToColorizedDisplayString), typeof(Role), typeof(FactionType))]
public static class ColorizeDisplayStringPatch
{
    public static void Postfix(ref string __result, ref Role role, ref FactionType factionType)
    {
        if (!Fancy.FactionalRoleNames.Value)
            return;

        if (role.IsResolved() || role is Role.FAMINE or Role.DEATH or Role.PESTILENCE or Role.WAR)
        {
            // me when I'm Canadian and refuse to use "colour"
            __result = Utils.GetColorizedRoleName(role, factionType);
        }
    }
}

[HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.UpdateTitle))]
public static class PatchRoleCard
{
    public static void Postfix(RoleCardPanel __instance)
    {
        string roleName;

        if (Fancy.FactionalRoleNames.Value)
            roleName = Utils.GetColorizedRoleName(Pepper.GetMyRole(), Pepper.GetMyFaction());
        else
            roleName = Pepper.GetMyRole().ToDisplayString();

        __instance.roleNameText.text = roleName;
    }
}

[HarmonyPatch(typeof(RoleCardPopupPanel), nameof(RoleCardPopupPanel.SetRole))]
public static class RoleCardPopupPatches
{
    public static void Postfix(ref Role role, RoleCardPopupPanel __instance) => __instance.roleNameText.text = role.ToColorizedDisplayString();
}

[HarmonyPatch(typeof(TosAbilityPanelListItem), nameof(TosAbilityPanelListItem.SetKnownRole))]
public static class PlayerListPatch
{
    public static bool Prefix(ref Role role, ref FactionType faction, TosAbilityPanelListItem __instance)
    {
        __instance.playerRole = role;

        if (role is not (0 or (Role)byte.MaxValue))
        {
            string roleName;

            if (Fancy.FactionalRoleNames.Value)
                roleName = Utils.GetColorizedRoleName(role, faction, true);
            else
                roleName = $"({role.ToDisplayString()})";

            __instance.playerRoleText.enableAutoSizing = false; // Remove when PlayerNotes+ fix is out
            __instance.playerRoleText.text = roleName;
            __instance.playerRoleText.gameObject.SetActive(true);
        }

        return false;
    }
}

[HarmonyPatch(typeof(TosCharacterNametag), nameof(TosCharacterNametag.ColouredName))]
public static class TosCharacterNametagPatch
{
    public static void Postfix(FactionType factionType, ref string __result, ref string theName, ref Role role)
    {
        if (Fancy.FactionalRoleNames.Value)
        {
            var nameText = Utils.GetColorizedText(theName, factionType);
            var roleText = Utils.GetColorizedRoleName(role, factionType, true);

            if (nameText == theName) // Stoned and Hidden are not checked here because gradients can be given to them via PlayerNotes+
                nameText = $"<color={factionType.GetFactionColor()}>{theName}</color>";

            __result = $"<size=36><sprite=\"RoleIcons\" name=\"Role{(int)role}\"></size>\n<size=24>{nameText}</size>\n<size=18>{roleText}</size>";
        }

        if (Constants.EnableIcons())
            __result = __result.Replace("RoleIcons\"", $"RoleIcons ({Utils.FactionName(factionType, false)})\"");

        if (Constants.IsBTOS2())
            __result = __result.Replace("\"RoleIcons", "\"BTOSRoleIcons");
    }
}