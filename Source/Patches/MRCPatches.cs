using Home.Shared;
using Server.Shared.Extensions;

namespace FancyUI.Patches

/*
* TO DO: Add patches for the following:
  * Reanimate Menu
  * Special Ability Panel
*/

{
[HarmonyPatch(typeof(ClientRoleExtensions), nameof(ClientRoleExtensions.ToColorizedDisplayString), typeof(Role), typeof(FactionType))]
public static class ColorizeDisplayStringPatch
{
    [HarmonyPostfix]
    public static void Result(ref string __result, ref Role role, ref FactionType factionType)
    {
        if (!Fancy.FactionalRoleNames.Value) return;

        if (RoleExtensions.IsResolved(role) || role is Role.FAMINE or Role.DEATH or Role.PESTILENCE or Role.WAR)
        {
            var roleName = Utils.ToRoleFactionDisplayString(role, factionType);
            
            try
            {
                var gradient = BetterTOS2.GetGradients.GetGradient(factionType); 

                if (gradient != null)
                {
                    __result = BetterTOS2.AddNewConversionTags.ApplyGradient(roleName, gradient.Evaluate(0f), gradient.Evaluate(1f));
                }
                else
                {
                    var color = ClientRoleExtensions.GetFactionColor(factionType);
                    __result = $"<color={color}>{roleName}</color>";
                }
            }
            catch
            {
                var color = ClientRoleExtensions.GetFactionColor(factionType);
                __result = $"<color={color}>{roleName}</color>";
            }
        }
    }
}

[HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.UpdateTitle))]
public static class PatchRoleCard
{
    public static void Postfix(RoleCardPanel __instance)
    {
        string roleName = string.Empty;

        if (Fancy.FactionalRoleNames.Value)
        {
            try
            {
                var gradient = BetterTOS2.GetGradients.GetGradient(Pepper.GetMyFaction());
                roleName = Utils.ToRoleFactionDisplayString(Pepper.GetMyRole(), Pepper.GetMyFaction());

                if (gradient != null)
                {
                    roleName = BetterTOS2.AddNewConversionTags.ApplyGradient(roleName, gradient.Evaluate(0f), gradient.Evaluate(1f));
                }
                else
                {
                    var color = Pepper.GetMyFaction().GetFactionColor();
                    roleName = $"<color={color}>{roleName}</color>";
                }
            }
            catch
            {
                var color = Pepper.GetMyFaction().GetFactionColor();
                roleName = $"<color={color}>{roleName}</color>";
            }
        }
        else
        {
            roleName = Pepper.GetMyRole().ToDisplayString();
        }

        __instance.roleNameText.text = roleName;
    }
}
    [HarmonyPatch(typeof(RoleCardPopupPanel), nameof(RoleCardPopupPanel.SetRole))]
    public static class RoleCardPopupPatches
    {
        public static void Postfix(ref Role role, RoleCardPopupPanel __instance) => __instance.roleNameText.text = ClientRoleExtensions.ToColorizedDisplayString(role);
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
            {
                try
                {
                    var gradient = BetterTOS2.GetGradients.GetGradient(faction);
                    roleName = Utils.ToRoleFactionDisplayString(role, faction);

                    if (gradient != null)
                    {
                        roleName = BetterTOS2.AddNewConversionTags.ApplyGradient($"({roleName})", gradient.Evaluate(0f), gradient.Evaluate(1f));
                    }
                    else
                    {
                        var color = faction.GetFactionColor();
                        roleName = $"<color={color}>({roleName})</color>";
                    }
                }
                catch
                {
                    var color = faction.GetFactionColor();
                    roleName = $"<color={color}>({role.ToDisplayString()})</color>";
                }
            }
            else
            {
                roleName = $"({role.ToDisplayString()})";
            }

            __instance.playerRoleText.text = roleName;
            __instance.playerRoleText.gameObject.SetActive(true);
        }

        return false;
    }
}
}
