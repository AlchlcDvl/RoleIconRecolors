using Game.Interface;
using Server.Shared.State;

namespace Recolors;

public static class Patches
{
    [HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.SetRole))]
    public static class PatchRoleCards
    {
        public static void Postfix(RoleCardPanel __instance, ref Role role)
        {
            if (!Settings.EnableIcons)
                return;

            var spriteName = role.ToString().ToTitleCase();

            if (Recolors.Instance.RegIcons.ContainsKey(spriteName))
                __instance.roleIcon.sprite = AssetManager.GetSprite(spriteName);
        }
    }
}