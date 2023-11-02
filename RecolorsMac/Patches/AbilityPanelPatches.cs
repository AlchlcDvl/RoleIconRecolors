using Game.Interface;
using Server.Shared.State;

namespace RecolorsMac.Patches;

public static class AbilityPanelPatches
{
    [HarmonyPatch(typeof(TosAbilityPanelListItem), nameof(TosAbilityPanelListItem.OverrideIconAndText))]
    public static class AbilityPanelStartPatch
    {
        public static void Postfix(TosAbilityPanelListItem __instance, ref TosAbilityPanelListItem.OverrideAbilityType overrideType)
        {
            if (!Constants.EnableIcons || overrideType == TosAbilityPanelListItem.OverrideAbilityType.VOTING)
                return;

            switch (overrideType)
            {
                case TosAbilityPanelListItem.OverrideAbilityType.NECRO_ATTACK:
                    var nommy = AssetManager.GetSprite("Necronomicon", false);

                    if (nommy != Recolors.Blank)
                    {
                        if (Pepper.GetMyRole() == Role.ILLUSIONIST && __instance.choice2Sprite != null)
                            __instance.choice2Sprite.sprite = nommy;
                        else if (__instance.choice1Sprite != null)
                            __instance.choice1Sprite.sprite = nommy;
                    }

                    break;

                case TosAbilityPanelListItem.OverrideAbilityType.POTIONMASTER_ATTACK:
                    var attack = AssetManager.GetSprite("PotionMaster_Special", false);

                    if (attack != Recolors.Blank && __instance.choice1Sprite != null)
                        __instance.choice1Sprite.sprite = attack;

                    break;

                case TosAbilityPanelListItem.OverrideAbilityType.POTIONMASTER_HEAL:
                    var heal = AssetManager.GetSprite("PotionMaster_Ability1", false);

                    if (heal != Recolors.Blank && __instance.choice1Sprite != null)
                        __instance.choice1Sprite.sprite = heal;

                    break;

                case TosAbilityPanelListItem.OverrideAbilityType.POTIONMASTER_REVEAL:
                    var reveal = AssetManager.GetSprite("PotionMaster_Ability2", false);

                    if (reveal != Recolors.Blank && __instance.choice1Sprite != null)
                        __instance.choice1Sprite.sprite = reveal;

                    break;

                case TosAbilityPanelListItem.OverrideAbilityType.POISONER_POISON:
                    var poison = AssetManager.GetSprite("Poisoner_Special", false);

                    if (poison != Recolors.Blank && __instance.choice1Sprite != null)
                        __instance.choice1Sprite.sprite = poison;

                    break;

                case TosAbilityPanelListItem.OverrideAbilityType.SHROUD:
                    var shroud = AssetManager.GetSprite("Shroud_Special", false);

                    if (shroud != Recolors.Blank && __instance.choice1Sprite != null)
                        __instance.choice1Sprite.sprite = shroud;

                    break;

                case TosAbilityPanelListItem.OverrideAbilityType.WEREWOLF_NON_FULL_MOON:
                    var sniff = AssetManager.GetSprite("Werewolf_Ability2", false);

                    if (sniff != Recolors.Blank && __instance.choice1Sprite != null)
                        __instance.choice1Sprite.sprite = sniff;

                    break;

                default:
                    var abilityName = $"{Utils.RoleName(Pepper.GetMyRole())}_Ability";
                    var ability1 = AssetManager.GetSprite(abilityName, false);

                    if (ability1 == Recolors.Blank)
                        abilityName += "_1";

                    ability1 = AssetManager.GetSprite(abilityName, false);

                    if (ability1 != Recolors.Blank && __instance.choice1Sprite != null)
                        __instance.choice1Sprite.sprite = ability1;

                    var ability2 = AssetManager.GetSprite($"{Utils.RoleName(Pepper.GetMyRole())}_Ability2", false);

                    if (ability2 != Recolors.Blank && __instance.choice2Sprite != null)
                        __instance.choice2Sprite.sprite = ability2;

                    break;
            }
        }
    }
}