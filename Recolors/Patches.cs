using Game.Interface;
using Server.Shared.Extensions;
using Server.Shared.State;

namespace Recolors;

public static class Patches
{
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
        Role.CURSED_SOUL => "CursedSoul",
        Role.VAMPIRE => "Vampire",
        Role.STONED => "Stoned",
        _ => "Blank"
    };
    private static string FactionName(FactionType role) => role switch
    {
        FactionType.TOWN => "Town",
        FactionType.COVEN => "Coven",
        FactionType.SERIALKILLER => "SerialKiller",
        FactionType.ARSONIST => "Arsonist",
        FactionType.WEREWOLF => "Werewolf",
        FactionType.SHROUD => "Shroud",
        FactionType.APOCALYPSE => "Horsemen",
        FactionType.EXECUTIONER => "Executioner",
        FactionType.JESTER => "Jester",
        FactionType.PIRATE => "Pirate",
        FactionType.DOOMSAYER => "Doomsayer",
        FactionType.VAMPIRE => "Vampire",
        FactionType.CURSED_SOUL => "CursedSoul",
        _ => "Blank"
    };

    [HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.Update))]
    public static class PatchRoleCardIcons1
    {
        public static void Postfix(RoleCardPanel __instance)
        {
            if (!Settings.EnableIcons)
                return;

            var role = RoleName(Pepper.GetMyCurrentIdentity().role);
            var faction = FactionName(Pepper.GetMyCurrentIdentity().role.GetFaction());
            var sprite = AssetManager.GetSprite(role);

            if (sprite != Recolors.Instance.Blank)
                __instance.roleIcon.sprite = sprite;

            var index = 0;
            var spriteName = $"{role}_Ability";
            var sprite1 = AssetManager.GetSprite(spriteName);

            if (sprite1 == Recolors.Instance.Blank)
                spriteName += "_1";

            sprite1 = AssetManager.GetSprite(spriteName);

            if (sprite1 != Recolors.Instance.Blank)
            {
                __instance.roleInfoButtons[index].abilityIcon.sprite = sprite1;
                index++;
            }

            var sprite2 = AssetManager.GetSprite($"{role}_Ability_2");

            if (sprite2 == Recolors.Instance.Blank)
                sprite2 = AssetManager.GetSprite($"Attributes_{faction}");

            if (sprite2 != Recolors.Instance.Blank)
            {
                __instance.roleInfoButtons[index].abilityIcon.sprite = sprite2;
                index++;
            }

            var sprite3 = AssetManager.GetSprite($"{role}_Special");

            if (sprite3 != Recolors.Instance.Blank)
                __instance.specialAbilityPanel.useButton.abilityIcon.sprite = sprite3;
        }
    }

    /*[HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.DetermineFrameAndSlots_DestroyRoleInfoSlots))]
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
    }*/

    /*[HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.DetermineFrameAndSlots_AttributeIcon))]
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
    }*/
}