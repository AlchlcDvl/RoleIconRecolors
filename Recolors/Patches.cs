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

            switch (role)
            {
                case Role.ADMIRER:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Admirer");
                    break;
                case Role.AMNESIAC:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Amnesiac");
                    break;
                case Role.BODYGUARD:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Bodyguard");
                    break;
                case Role.CLERIC:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Cleric");
                    break;
                case Role.CORONER:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Coroner");
                    break;
                case Role.CRUSADER:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Crusader");
                    break;
                case Role.DEPUTY:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Deputy");
                    break;
                case Role.INVESTIGATOR:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Investigator");
                    break;
                case Role.JAILOR:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Jailor");
                    break;
                case Role.LOOKOUT:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Lookout");
                    break;
                case Role.MAYOR:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Mayor");
                    break;
                case Role.MONARCH:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Monarch");
                    break;
                case Role.PROSECUTOR:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Prosecutor");
                    break;
                case Role.PSYCHIC:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Psychic");
                    break;
                case Role.RETRIBUTIONIST:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Retributionist");
                    break;
                case Role.SEER:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Seer");
                    break;
                case Role.SHERIFF:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Sheriff");
                    break;
                case Role.SPY:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Spy");
                    break;
                case Role.TAVERNKEEPER:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("TavernKeeper");
                    break;
                case Role.TRACKER:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Tracker");
                    break;
                case Role.TRAPPER:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Trapper");
                    break;
                case Role.TRICKSTER:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Trickster");
                    break;
                case Role.VETERAN:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Veteran");
                    break;
                case Role.VIGILANTE:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Vigilante");
                    break;
                case Role.CONJURER:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Conjurer");
                    break;
                case Role.COVENLEADER:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("CovenLeader");
                    break;
                case Role.DREAMWEAVER:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Dreamweaver");
                    break;
                case Role.ENCHANTER:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Enchanter");
                    break;
                case Role.HEXMASTER:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("HexMaster");
                    break;
                case Role.ILLUSIONIST:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Illusionist");
                    break;
                case Role.JINX:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Jinx");
                    break;
                case Role.MEDUSA:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Medusa");
                    break;
                case Role.NECROMANCER:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Necromancer");
                    break;
                case Role.POISONER:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Poisoner");
                    break;
                case Role.POTIONMASTER:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("PotionMaster");
                    break;
                case Role.RITUALIST:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Ritualist");
                    break;
                case Role.VOODOOMASTER:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("VoodooMaster");
                    break;
                case Role.WILDLING:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("WIlding");
                    break;
                case Role.WITCH:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Witch");
                    break;
                case Role.ARSONIST:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Arsonist");
                    break;
                case Role.BAKER:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Baker");
                    break;
                case Role.BERSERKER:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Berserker");
                    break;
                case Role.DOOMSAYER:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Doomsayer");
                    break;
                case Role.EXECUTIONER:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Executioner");
                    break;
                case Role.JESTER:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Jester");
                    break;
                case Role.PIRATE:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Pirate");
                    break;
                case Role.PLAGUEBEARER:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Plaguebearer");
                    break;
                case Role.SERIALKILLER:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("SerialKiller");
                    break;
                case Role.SHROUD:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Shroud");
                    break;
                case Role.SOULCOLLECTOR:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("SoulCollector");
                    break;
                case Role.WEREWOLF:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Werewolf");
                    break;
                case Role.VAMPIRE:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Vampire");
                    break;
                case Role.WAR:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("War");
                    break;
                case Role.FAMINE:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Famine");
                    break;
                case Role.PESTILENCE:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Pestilence");
                    break;
                case Role.DEATH:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Death");
                    break;
                case Role.STONED:
                    __instance.roleIcon.sprite = AssetManager.GetSprite("Stoned");
                    break;
            }
        }
    }
}