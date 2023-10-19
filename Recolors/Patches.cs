using Game.Interface;
using Server.Shared.State;
using Server.Shared.Info;
using Services;

namespace Recolors;

public static class Patches
{
    private static int ButtonIndex;
    private static string RoleName(Role role) => role switch
    {
        Role.ADMIRER => "Admirer",
        Role.AMNESIAC => "Amnesiac",
        Role.BODYGUARD => "Bodyguard",
        Role.CLERIC => "Cleric",
        Role.CORONER => "Coroner",
        Role.CRUSADER => "Crusader",
        Role.DEPUTY => "Deputy",
        Role.INVESTIGATOR => "Investigator",
        Role.JAILOR => "Jailor",
        Role.LOOKOUT => "Lookout",
        Role.MAYOR => "Mayor",
        Role.MONARCH => "Monarch",
        Role.PROSECUTOR => "Prosecutor",
        Role.PSYCHIC => "Psychic",
        Role.RETRIBUTIONIST => "Retributionist",
        Role.SEER => "Seer",
        Role.SHERIFF => "Sheriff",
        Role.SPY => "Spy",
        Role.TAVERNKEEPER => "TavernKeeper",
        Role.TRACKER => "Tracker",
        Role.TRAPPER => "Trapper",
        Role.TRICKSTER => "Trickster",
        Role.VETERAN => "Veteran",
        Role.VIGILANTE => "Vigilante",
        Role.CONJURER => "Conjurer",
        Role.COVENLEADER => "CovenLeader",
        Role.DREAMWEAVER => "Dreamweaver",
        Role.ENCHANTER => "Enchanter",
        Role.HEXMASTER => "HexMaster",
        Role.ILLUSIONIST => "Illusionist",
        Role.JINX => "Jinx",
        Role.MEDUSA => "Medusa",
        Role.NECROMANCER => "Necromancer",
        Role.POISONER => "Poisoner",
        Role.POTIONMASTER => "PotionMaster",
        Role.RITUALIST => "Ritualist",
        Role.VOODOOMASTER => "VoodooMaster",
        Role.WILDLING => "Wilding",
        Role.WITCH => "Witch",
        Role.ARSONIST => "Arsonist",
        Role.BAKER => "Baker",
        Role.BERSERKER => "Berserker",
        Role.DOOMSAYER => "Doomsayer",
        Role.EXECUTIONER => "Executioner",
        Role.JESTER => "Jester",
        Role.PIRATE => "Pirate",
        Role.PLAGUEBEARER => "Plaguebearer",
        Role.SERIALKILLER => "SerialKiller",
        Role.SHROUD => "Shroud",
        Role.SOULCOLLECTOR => "SoulCollector",
        Role.WEREWOLF => "Werewolf",
        Role.FAMINE => "Famine",
        Role.WAR => "War",
        Role.PESTILENCE => "Pestilence",
        Role.DEATH => "Death",
        Role.STONED => "Stoned",
        _ => "Blank"
    };
    private static string FactionName(Role role) => role switch
    {
        Role.ADMIRER or Role.AMNESIAC or Role.BODYGUARD or Role.CLERIC or Role.CORONER or Role.CRUSADER or Role.DEPUTY or Role.INVESTIGATOR or Role.JAILOR or Role.LOOKOUT or Role.MAYOR or
            Role.MONARCH or Role.PROSECUTOR or Role.PSYCHIC or Role.RETRIBUTIONIST or Role.SEER or Role.SHERIFF or Role.SPY or Role.TAVERNKEEPER or Role.TRACKER or Role.TRAPPER or
            Role.TRICKSTER or Role.VETERAN or Role.VIGILANTE => "Town",
        Role.CONJURER or Role.COVENLEADER or Role.DREAMWEAVER or Role.ENCHANTER or Role.HEXMASTER or Role.ILLUSIONIST or Role.JINX or Role.MEDUSA or Role.NECROMANCER or Role.POISONER or
            Role.POTIONMASTER or Role.RITUALIST or Role.VOODOOMASTER or Role.WILDLING or Role.WITCH => "Coven",
        Role.ARSONIST => "Arsonist",
        Role.DOOMSAYER => "Doomsayer",
        Role.EXECUTIONER => "Executioner",
        Role.JESTER => "Jester",
        Role.PIRATE => "Pirate",
        Role.SERIALKILLER => "SerialKiller",
        Role.SHROUD => "Shroud",
        Role.WEREWOLF => "Werewolf",
        Role.BAKER or Role.BERSERKER or Role.PLAGUEBEARER or Role.SOULCOLLECTOR or Role.FAMINE or Role.WAR or Role.PESTILENCE or Role.DEATH => "Horsemen",
        _ => "Blank"
    };

    [HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.SetRole))]
    public static class PatchRoleCardIcons
    {
        public static void Prefix(RoleCardPanel __instance) => ButtonIndex = __instance.infoButtonsShowing;

        public static void Postfix(RoleCardPanel __instance)
        {
            if (!Settings.EnableIcons)
                return;

            var sprite = AssetManager.GetSprite(RoleName(__instance.myData.role));

            if (sprite != Recolors.Instance.Blank)
                __instance.roleIcon.sprite = sprite;
        }
    }

    [HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.DetermineFrameAndSlots_DestroyRoleInfoSlots))]
    public static class PatchSlots
    {
        public static void Prefix() => ButtonIndex = 0;
    }

    [HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.DetermineFrameAndSlots_AbilityIcon))]
    public static class PatchRoleCardAbilityIcons1
    {
        public static void Postfix(RoleCardPanel __instance)
        {
            if (!Settings.EnableIcons || __instance.myData.abilityIcon == null)
                return;

            var spriteName = RoleName(__instance.myData.role) + "_Ability";
            var sprite = AssetManager.GetSprite(spriteName);

            if (sprite == Recolors.Instance.Blank)
                spriteName += "_1";

            sprite = AssetManager.GetSprite(spriteName);

            if (sprite != Recolors.Instance.Blank)
            {
                __instance.roleInfoButtons[ButtonIndex].abilityIcon.sprite = sprite;
                ButtonIndex++;
            }
        }
    }

    [HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.DetermineFrameAndSlots_AbilityIcon2))]
    public static class PatchRoleCardAbilityIcons2
    {
        public static void Postfix(RoleCardPanel __instance)
        {
            if (!Settings.EnableIcons || __instance.myData.abilityIcon2 == null)
                return;

            var sprite = AssetManager.GetSprite(RoleName(__instance.myData.role) + "_Ability_2");

            if (sprite != Recolors.Instance.Blank)
            {
                __instance.roleInfoButtons[ButtonIndex].abilityIcon.sprite = sprite;
                ButtonIndex++;
            }
        }
    }

    [HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.DetermineFrameAndSlots_AttributeIcon))]
    public static class PatchRoleCardAttributeIcon
    {
        public static void Postfix(RoleCardPanel __instance)
        {
            if (!Settings.EnableIcons || __instance.myData.attributeIcon == null)
                return;

            var sprite = AssetManager.GetSprite("Attribute_" + FactionName(__instance.myData.role));

            if (sprite != Recolors.Instance.Blank)
            {
                __instance.roleInfoButtons[ButtonIndex].abilityIcon.sprite = sprite;
                ButtonIndex++;
            }
        }
    }

    [HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.DetermineFrameAndSlots_Necro))]
    public static class PatchRoleCardNecronomicon
    {
        public static void Postfix(RoleCardPanel __instance)
        {
            if (!Settings.EnableIcons || Service.Game.Sim.info.roleCardObservation.Data.powerUp != POWER_UP_TYPE.NECRONOMICON)
                return;

            var sprite = AssetManager.GetSprite("Necronomicon");

            if (sprite != Recolors.Instance.Blank)
            {
                __instance.roleInfoButtons[ButtonIndex].abilityIcon.sprite = sprite;
                ButtonIndex++;
            }
        }
    }

    [HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.ValidateSpecialAbilityPanel))]
    public static class PatchRoleCardSpecialAbilityIcon
    {
        public static void Postfix(RoleCardPanel __instance)
        {
            if (!Settings.EnableIcons || __instance.myData.specialAbilityIcon == null)
                return;

            var sprite = AssetManager.GetSprite(RoleName(__instance.myData.role) + "_Special");

            if (sprite != Recolors.Instance.Blank)
                __instance.specialAbilityPanel.useButton.abilityIcon.sprite = sprite;
        }
    }
}