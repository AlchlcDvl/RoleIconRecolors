using Game.Interface;
using Server.Shared.Extensions;
using Server.Shared.State;
using Server.Shared.Info;
using Services;

namespace RecolorsMac.Patches;

public static class RoleCardPatches
{
    private static int Index;

    [HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.SetRole))]
    public static class PatchRoleCardIcon
    {
        public static void Postfix(RoleCardPanel __instance)
        {
            if (!Constants.EnableIcons || __instance.myData.roleIcon == null)
                return;

            var role = Pepper.GetMyRole();
            var name = Utils.RoleName(role);
            var sprite = role.IsTraitor(Pepper.GetMyFaction()) ? AssetManager.GetTTSprite(name) : AssetManager.GetSprite(name);

            if (sprite != AssetManager.Blank && __instance.roleIcon != null)
                __instance.roleIcon.sprite = sprite;
        }
    }

    [HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.DetermineFrameAndSlots))]
    public static class ResetIndexPatch
    {
        public static void Prefix() => Index = 0;
    }

    [HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.DetermineFrameAndSlots_AbilityIcon))]
    public static class PatchRoleCardAbilityIcons1
    {
        public static void Postfix(RoleCardPanel __instance)
        {
            if (!Constants.EnableIcons || __instance.myData.abilityIcon == null)
                return;

            var spriteName = $"{Utils.RoleName(Pepper.GetMyRole())}_Ability";
            var sprite = AssetManager.GetSprite(spriteName);

            if (sprite == AssetManager.Blank)
                spriteName += "_1";

            sprite = AssetManager.GetSprite(spriteName);

            if (sprite != AssetManager.Blank && __instance.roleInfoButtons[Index] != null)
            {
                __instance.roleInfoButtons[Index].abilityIcon.sprite = sprite;
                Index++;
            }
        }
    }

    [HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.DetermineFrameAndSlots_AbilityIcon2))]
    public static class PatchRoleCardAbilityIcons2
    {
        public static void Postfix(RoleCardPanel __instance)
        {
            if (!Constants.EnableIcons || __instance.myData.abilityIcon2 == null)
                return;

            var sprite = AssetManager.GetSprite($"{Utils.RoleName(Pepper.GetMyRole())}_Ability_2");

            if (sprite != AssetManager.Blank && __instance.roleInfoButtons[Index] != null)
            {
                __instance.roleInfoButtons[Index].abilityIcon.sprite = sprite;
                Index++;
            }
        }
    }

    [HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.DetermineFrameAndSlots_AttributeIcon))]
    public static class PatchRoleCardAttributeIcon
    {
        public static void Postfix(RoleCardPanel __instance)
        {
            if (!Constants.EnableIcons || __instance.myData.attributeIcon == null)
                return;

            var sprite = AssetManager.GetSprite($"Attributes_{Utils.FactionName(Pepper.GetMyFaction())}");

            if (sprite != AssetManager.Blank && __instance.roleInfoButtons[Index] != null)
            {
                __instance.roleInfoButtons[Index].abilityIcon.sprite = sprite;
                Index++;
            }
        }
    }

    [HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.DetermineFrameAndSlots_Necro))]
    public static class PatchRoleCardNecronomicon
    {
        public static void Postfix(RoleCardPanel __instance)
        {
            if (!Constants.EnableIcons || Service.Game.Sim.info.roleCardObservation.Data.powerUp != POWER_UP_TYPE.NECRONOMICON)
                return;

            var sprite = AssetManager.GetSprite("Necronomicon");

            if (sprite != AssetManager.Blank && __instance.roleInfoButtons[Index] != null)
            {
                __instance.roleInfoButtons[Index].abilityIcon.sprite = sprite;
                Index++;
            }
        }
    }

    [HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.ValidateSpecialAbilityPanel))]
    public static class PatchRoleCardSpecialAbilityIcon
    {
        public static void Postfix(RoleCardPanel __instance)
        {
            if (!Constants.EnableIcons || __instance.myData.specialAbilityIcon == null)
                return;

            var sprite = AssetManager.GetSprite($"{Utils.RoleName(Pepper.GetMyRole())}_Special");

            if (sprite != AssetManager.Blank && __instance.specialAbilityPanel.useButton.abilityIcon != null)
                __instance.specialAbilityPanel.useButton.abilityIcon.sprite = sprite;
        }
    }

    [HarmonyPatch(typeof(RoleCardPanelBackground), nameof(RoleCardPanelBackground.SetRole))]
    public class FixRoleChanges
    {
        public static void Postfix(RoleCardPanelBackground __instance, ref Role role)
        {
            var panel = __instance.GetComponentInParent<RoleCardPanel>();
            panel.roleNameText.text = role.DisplayString(__instance.currentFaction);

            if (!Constants.EnableIcons)
                return;

            Index = 0;
            var name = Utils.RoleName(role);
            var sprite = role.IsTraitor(Pepper.GetMyFaction()) ? AssetManager.GetTTSprite(name) : AssetManager.GetSprite(name);

            if (sprite != AssetManager.Blank && panel.roleIcon != null)
                panel.roleIcon.sprite = sprite;

            var special = AssetManager.GetSprite($"{name}_Special");

            if (special != AssetManager.Blank && panel.specialAbilityPanel.useButton.abilityIcon != null)
                panel.specialAbilityPanel.useButton.abilityIcon.sprite = special;

            var abilityname = $"{name}_Ability";
            var ability1 = AssetManager.GetSprite(abilityname);

            if (ability1 == AssetManager.Blank)
                abilityname += "_1";

            ability1 = AssetManager.GetSprite(abilityname);

            if (ability1 != AssetManager.Blank && panel.roleInfoButtons[Index] != null)
            {
                panel.roleInfoButtons[Index].abilityIcon.sprite = ability1;
                Index++;
            }

            var ability2 = AssetManager.GetSprite($"{name}_Ability_2");

            if (ability2 != AssetManager.Blank && panel.roleInfoButtons[Index] != null)
            {
                panel.roleInfoButtons[Index].abilityIcon.sprite = ability2;
                Index++;
            }

            var attribute = AssetManager.GetSprite($"Attributes_{Utils.FactionName(Pepper.GetMyFaction(), role)}");

            if (attribute != AssetManager.Blank && panel.roleInfoButtons[Index] != null)
            {
                panel.roleInfoButtons[Index].abilityIcon.sprite = attribute;
                Index++;
            }

            var nommy = AssetManager.GetSprite("Necronomicon");

            if (nommy != AssetManager.Blank && Service.Game.Sim.info.roleCardObservation.Data.powerUp == POWER_UP_TYPE.NECRONOMICON && panel.roleInfoButtons[Index] != null)
            {
                panel.roleInfoButtons[Index].abilityIcon.sprite = nommy;
                Index++;
            }
        }
    }
}