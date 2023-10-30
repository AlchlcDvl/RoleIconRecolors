//using Game.Interface;

namespace RecolorsWindows.Patches;

public static class AbilityPanelPatches
{
    /*[HarmonyPatch(typeof(TosAbilityPanelListItem), nameof(TosAbilityPanelListItem.OverrideIconAndText))]
    public static class AbilityPanelStartPatch
    {
        public static void Postfix(TosAbilityPanelListItem __instance, ref TosAbilityPanelListItem.OverrideAbilityType overrideType)
        {
            if (!Constants.EnableIcons || overrideType == TosAbilityPanelListItem.OverrideAbilityType.VOTING)
                return;

            var data = __instance.uiRoleData.roleDataList.Find(d => d.role == Pepper.GetMyRole());

            if (data == null)
                return;

            if (data.abilityIcon != null)
            {
                var spriteName = $"{Utils.RoleName(Pepper.GetMyRole())}_Ability";
                var sprite = AssetManager.GetSprite(spriteName);

                if (sprite == Recolors.Instance.Blank)
                    spriteName += "_1";

                sprite = AssetManager.GetSprite(spriteName);

                if (sprite != Recolors.Instance.Blank)
                    __instance.choice1Sprite.sprite = sprite;
            }
        }
    }*/
}