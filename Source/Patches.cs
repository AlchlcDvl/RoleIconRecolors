using Game.Services;
using Home.Shared;
using Cinematics.Players;
using Home.Services;
using UnityEngine.UI;

namespace RecolorsPlatformless;

[HarmonyPatch(typeof(RoleCardListItem), nameof(RoleCardListItem.SetData))]
public static class PatchRoleDeckBuilder
{
    public static void Postfix(RoleCardListItem __instance, ref Role role)
    {
        Recolors.LogMessage("Patching RoleCardListItem.SetData");

        if (!Constants.EnableIcons)
            return;

        var icon = AssetManager.GetSprite($"{Utils.RoleName(role)}", false);

        if (__instance.roleImage && icon != AssetManager.Blank)
            __instance.roleImage.sprite = icon;
    }
}

[HarmonyPatch(typeof(RoleDeckListItem), nameof(RoleDeckListItem.SetData))]
public static class PatchRoleListPanel
{
    public static void Postfix(RoleDeckListItem __instance, ref Role a_role, ref bool a_isBan)
    {
        Recolors.LogMessage("Patching RoleDeckListItem.SetData");

        if (!Constants.EnableIcons || a_isBan)
            return;

        var icon = AssetManager.GetSprite($"{Utils.RoleName(a_role)}", false);

        if (__instance.roleImage && icon != AssetManager.Blank)
            __instance.roleImage.sprite = icon;
    }
}

[HarmonyPatch(typeof(GameBrowserRoleDeckListItem), nameof(GameBrowserRoleDeckListItem.SetData))]
public static class PatchBrowserRoleListPanel
{
    public static void Postfix(GameBrowserRoleDeckListItem __instance, ref Role a_role, ref bool a_isBan)
    {
        Recolors.LogMessage("Patching GameBrowserRoleDeckListItem.SetData");

        if (!Constants.EnableIcons || a_isBan)
            return;

        var icon = AssetManager.GetSprite($"{Utils.RoleName(a_role)}", false);

        if (__instance.roleImage && icon != AssetManager.Blank)
            __instance.roleImage.sprite = icon;
    }
}

[HarmonyPatch(typeof(RoleCardPanelBackground), nameof(RoleCardPanelBackground.SetRole))]
public static class PatchRoleCards
{
    public static void Postfix(RoleCardPanelBackground __instance, ref Role role)
    {
        Recolors.LogMessage("Patching RoleCardPanelBackground.SetRole");

        if (Constants.IsTransformed)
        {
            role = role switch
            {
                Role.BAKER => Role.FAMINE,
                Role.BERSERKER => Role.WAR,
                Role.SOULCOLLECTOR => Role.DEATH,
                Role.PLAGUEBEARER => Role.PESTILENCE,
                _ => role
            };
        }

        var panel = __instance.GetComponentInParent<RoleCardPanel>();
        panel.roleNameText.text = role.DisplayString(__instance.currentFaction);

        if (!Constants.EnableIcons)
            return;

        var index = 0;
        var name = Utils.RoleName(role);
        var sprite = AssetManager.GetSprite(name);

        if (sprite != AssetManager.Blank && panel.roleIcon)
            panel.roleIcon.sprite = sprite;

        var special = AssetManager.GetSprite($"{name}_Special");

        if (special != AssetManager.Blank && panel.specialAbilityPanel && !(role == Role.NECROMANCER && !Constants.IsNecroActive))
            panel.specialAbilityPanel.useButton.abilityIcon.sprite = special;

        var abilityname = $"{name}_Ability";
        var ability1 = AssetManager.GetSprite(abilityname);

        if (ability1 == AssetManager.Blank)
        {
            abilityname += "_1";
            ability1 = AssetManager.GetSprite(abilityname);
        }

        if (ability1 != AssetManager.Blank && panel.roleInfoButtons.Exists(index))
        {
            panel.roleInfoButtons[index].abilityIcon.sprite = ability1;
            index++;
        }
        else if (AssetManager.Skippable(abilityname) || AssetManager.Skippable(abilityname.Replace("_1", "")))
            index++;

        var abilityname2 = $"{name}_Ability_2";
        var ability2 = AssetManager.GetSprite(abilityname2);

        if (ability2 != AssetManager.Blank && panel.roleInfoButtons.Exists(index) && role != Role.WAR)
        {
            panel.roleInfoButtons[index].abilityIcon.sprite = ability2;
            index++;
        }
        else if (AssetManager.Skippable(abilityname2))
            index++;

        var attributename = $"Attributes_{Utils.FactionName(Pepper.GetMyFaction(), role)}";
        var attribute = AssetManager.GetSprite(attributename);

        if (attribute != AssetManager.Blank && panel.roleInfoButtons.Exists(index))
        {
            panel.roleInfoButtons[index].abilityIcon.sprite = attribute;
            index++;
        }
        else if (AssetManager.Skippable(attributename))
            index++;

        var nommy = AssetManager.GetSprite("Necronomicon");

        if (nommy != AssetManager.Blank && Constants.IsNecroActive && panel.roleInfoButtons.Exists(index))
            panel.roleInfoButtons[index].abilityIcon.sprite = nommy;
    }
}

[HarmonyPatch(typeof(TosAbilityPanelListItem), nameof(TosAbilityPanelListItem.OverrideIconAndText))]
public static class AbilityPanelStartPatch
{
    public static void Postfix(TosAbilityPanelListItem __instance, ref TosAbilityPanelListItem.OverrideAbilityType overrideType)
    {
        Recolors.LogMessage("Patching TosAbilityPanelListItem.OverrideIconAndText");

        if (!Constants.EnableIcons || overrideType == TosAbilityPanelListItem.OverrideAbilityType.VOTING)
            return;

        switch (overrideType)
        {
            case TosAbilityPanelListItem.OverrideAbilityType.NECRO_ATTACK:
                var nommy = AssetManager.GetSprite("Necronomicon", Constants.PlayerPanelEasterEggs);

                if (nommy != AssetManager.Blank)
                {
                    if (Pepper.GetMyRole() == Role.ILLUSIONIST && __instance.choice2Sprite)
                    {
                        __instance.choice2Sprite.sprite = nommy;
                        var illu = AssetManager.GetSprite("Illusionist_Ability", Constants.PlayerPanelEasterEggs);

                        if (illu != AssetManager.Blank && __instance.choice1Sprite)
                            __instance.choice1Sprite.sprite = illu;
                    }
                    else if (__instance.choice1Sprite)
                    {
                        __instance.choice1Sprite.sprite = nommy;

                        if (Pepper.GetMyRole() == Role.WITCH)
                        {
                            var target = AssetManager.GetSprite("Witch_Ability_2", Constants.PlayerPanelEasterEggs);

                            if (target != AssetManager.Blank && __instance.choice2Sprite)
                                __instance.choice2Sprite.sprite = target;
                        }
                    }
                }

                break;

            case TosAbilityPanelListItem.OverrideAbilityType.POTIONMASTER_ATTACK:
                var attack = AssetManager.GetSprite("PotionMaster_Special", Constants.PlayerPanelEasterEggs);

                if (attack != AssetManager.Blank && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = attack;

                break;

            case TosAbilityPanelListItem.OverrideAbilityType.POTIONMASTER_HEAL:
                var heal = AssetManager.GetSprite("PotionMaster_Ability_1", Constants.PlayerPanelEasterEggs);

                if (heal != AssetManager.Blank && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = heal;

                break;

            case TosAbilityPanelListItem.OverrideAbilityType.POTIONMASTER_REVEAL:
                var reveal = AssetManager.GetSprite("PotionMaster_Ability_2", Constants.PlayerPanelEasterEggs);

                if (reveal != AssetManager.Blank && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = reveal;

                break;

            case TosAbilityPanelListItem.OverrideAbilityType.POISONER_POISON:
                var poison = AssetManager.GetSprite("Poisoner_Special", Constants.PlayerPanelEasterEggs);

                if (poison != AssetManager.Blank && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = poison;

                break;

            case TosAbilityPanelListItem.OverrideAbilityType.SHROUD:
                var shroud = AssetManager.GetSprite("Shroud_Special", Constants.PlayerPanelEasterEggs);

                if (shroud != AssetManager.Blank && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = shroud;

                break;

            case TosAbilityPanelListItem.OverrideAbilityType.WEREWOLF_NON_FULL_MOON:
                var sniff = AssetManager.GetSprite("Werewolf_Ability_2", Constants.PlayerPanelEasterEggs);

                if (sniff != AssetManager.Blank && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = sniff;

                break;

            default:
                var role = Pepper.GetMyRole();

                if (Constants.IsTransformed)
                {
                    role = role switch
                    {
                        Role.BAKER => Role.FAMINE,
                        Role.BERSERKER => Role.WAR,
                        Role.SOULCOLLECTOR => Role.DEATH,
                        Role.PLAGUEBEARER => Role.PESTILENCE,
                        _ => role
                    };
                }

                var abilityName = $"{Utils.RoleName(role)}_Ability";
                var ability1 = AssetManager.GetSprite(abilityName, Constants.PlayerPanelEasterEggs);

                if (ability1 == AssetManager.Blank)
                    abilityName += "_1";

                ability1 = AssetManager.GetSprite(abilityName, Constants.PlayerPanelEasterEggs);

                if (ability1 != AssetManager.Blank && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = ability1;

                var ability2 = AssetManager.GetSprite($"{Utils.RoleName(role)}_Ability_2", Constants.PlayerPanelEasterEggs);

                if (ability2 != AssetManager.Blank && __instance.choice2Sprite)
                    __instance.choice2Sprite.sprite = ability2;

                break;
        }
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupGenericListItem), nameof(SpecialAbilityPopupGenericListItem.SetData))]
public static class SpecialAbilityPanelPatch
{
    public static void Postfix(SpecialAbilityPopupGenericListItem __instance)
    {
        Recolors.LogMessage("Patching SpecialAbilityPopupGenericListItem.SetData");

        if (!Constants.EnableIcons)
            return;

        var special = AssetManager.GetSprite($"{Utils.RoleName(Pepper.GetMyRole())}_Special");

        if (special != AssetManager.Blank)
            __instance.choiceSprite.sprite = special;
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupRadialIcon), nameof(SpecialAbilityPopupRadialIcon.SetData))]
public static class PatchRitualistGuessMenu
{
    public static void Postfix(SpecialAbilityPopupRadialIcon __instance, ref Role a_role)
    {
        Recolors.LogMessage("Patching SpecialAbilityPopupRadialIcon.SetData");

        if (!Constants.EnableIcons)
            return;

        var icon = AssetManager.GetSprite($"{Utils.RoleName(a_role)}", false);

        if (__instance.roleIcon != null && icon != AssetManager.Blank)
            __instance.roleIcon.sprite = icon;
    }
}

[HarmonyPatch(typeof(RoleService), nameof(RoleService.Init))]
public static class PatchRoleService
{
    public static bool ServiceExists = false;

    public static void Postfix()
    {
        Recolors.LogMessage("Patching RoleService.Init");

        if (AssetManager.IconPacks.TryGetValue(Constants.CurrentPack, out var pack))
            pack.LoadSpriteSheet(true);

        ServiceExists = true;
    }
}

[HarmonyPatch(typeof(ApplicationController), nameof(ApplicationController.QuitGame))]
public static class ExitGamePatch
{
    public static void Postfix()
    {
        Recolors.LogMessage("Patching ApplicationController.QuitGame");
        Utils.SaveLogs();
    }
}

[HarmonyPatch(typeof(RoleCardPopupPanel), nameof(RoleCardPopupPanel.SetRole))]
public static class PatchGuideRoleCards
{
    public static void Postfix(RoleCardPopupPanel __instance, ref Role role)
    {
        Recolors.LogMessage("Patching RoleCardPopupPanel.SetRole");

        if (!Constants.EnableIcons)
            return;

        var index = 0;
        var name = Utils.RoleName(role);
        var sprite = AssetManager.GetSprite(name, false);

        if (sprite != AssetManager.Blank && __instance.roleIcon)
            __instance.roleIcon.sprite = sprite;

        var special = AssetManager.GetSprite($"{name}_Special");

        if (special != AssetManager.Blank && __instance.specialAbilityPanel && role != Role.NECROMANCER)
            __instance.specialAbilityPanel.useButton.abilityIcon.sprite = special;

        var abilityname = $"{name}_Ability";
        var ability1 = AssetManager.GetSprite(abilityname, false);

        if (ability1 == AssetManager.Blank)
        {
            abilityname += "_1";
            ability1 = AssetManager.GetSprite(abilityname, false);
        }

        if (ability1 != AssetManager.Blank && __instance.roleInfoButtons.Exists(index))
        {
            __instance.roleInfoButtons[index].abilityIcon.sprite = ability1;
            index++;
        }
        else if (AssetManager.Skippable(abilityname) || AssetManager.Skippable(abilityname + "_1") || AssetManager.Skippable(abilityname.Replace("_1", "")))
            index++;

        var abilityname2 = $"{name}_Ability_2";
        var ability2 = AssetManager.GetSprite(abilityname2, false);

        if (ability2 != AssetManager.Blank && __instance.roleInfoButtons.Exists(index))
        {
            __instance.roleInfoButtons[index].abilityIcon.sprite = ability2;
            index++;
        }
        else if (AssetManager.Skippable(abilityname2))
            index++;

        var attributename = $"Attributes_{Utils.FactionName(role.GetFaction(), role)}";
        var attribute = AssetManager.GetSprite(attributename, false);

        if (attribute != AssetManager.Blank && __instance.roleInfoButtons.Exists(index))
        {
            __instance.roleInfoButtons[index].abilityIcon.sprite = attribute;
            index++;
        }
        else if (AssetManager.Skippable(attributename))
            index++;

        var nommy = AssetManager.GetSprite("Necronomicon", false);

        if (nommy != AssetManager.Blank && __instance.roleInfoButtons.Exists(index))
            __instance.roleInfoButtons[index].abilityIcon.sprite = nommy;
    }
}

[HarmonyPatch(typeof(DoomsayerLeavesCinematicPlayer), nameof(DoomsayerLeavesCinematicPlayer.Init))]
public static class PatchDoomsayerLeaving
{
    public static void Postfix(DoomsayerLeavesCinematicPlayer __instance)
    {
        Recolors.LogMessage("Patching DoomsayerLeavesCinematicPlayer.Init");

        if (!Constants.EnableIcons)
            return;

        var role1 = __instance.doomsayerLeavesCinematicData.roles[0];
        var role2 = __instance.doomsayerLeavesCinematicData.roles[1];
        var role3 = __instance.doomsayerLeavesCinematicData.roles[2];

        var sprite1 = AssetManager.GetSprite(Utils.RoleName(role1));
        var sprite2 = AssetManager.GetSprite(Utils.RoleName(role2));
        var sprite3 = AssetManager.GetSprite(Utils.RoleName(role3));

        if (sprite1 != AssetManager.Blank)
            __instance.otherCharacterRole1.sprite = sprite1;

        if (sprite2 != AssetManager.Blank)
            __instance.otherCharacterRole2.sprite = sprite2;

        if (sprite3 != AssetManager.Blank)
            __instance.otherCharacterRole3.sprite = sprite3;
    }
}

[HarmonyPatch(typeof(HomeInterfaceService), nameof(HomeInterfaceService.Init))]
public static class CacheDefaultSpriteSheet
{
    public static int Cache;
    public static TMP_SpriteAsset VanillaSheet;
    private static readonly string[] Assets = new[] { "Cast", "LobbyIcons", "MiscIcons", "PlayerNumbers", "RoleIcons", "SalemTmpIcons", "TrialReportIcons" };

    public static bool Prefix(HomeInterfaceService __instance)
    {
        Recolors.LogMessage("Patching HomeInterfaceService.Init");
        Assets.ForEach(key =>
        {
            Debug.Log("HomeInterfaceService:: Add Sprite Asset " + key);
            var asset = __instance.LoadResource<TMP_SpriteAsset>($"TmpSpriteAssets/{key}.asset");
            MaterialReferenceManager.AddSpriteAsset(asset);

            if (key.Contains("RoleIcons"))
            {
                Cache = asset.hashCode;
                VanillaSheet = asset;
            }
        });
        __instance.isReady_ = true;
        return false;
    }
}

[HarmonyPatch(typeof(HomeScrollService), nameof(HomeScrollService.Init))]
public static class PatchScrolls
{
    public static bool ServiceExists = false;

    public static void Postfix(HomeScrollService __instance)
    {
        Recolors.LogMessage("Patching HomeScrollService.Init");

        if (!Constants.EnableIcons)
            return;

        AssetManager.SetScrollSprites(__instance);
        ServiceExists = true;
    }
}

[HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.ShowAttackAndDefense))]
public static class PatchAttackDefense
{
    public static void Postfix(RoleCardPanel __instance, ref RoleCardData data)
    {
        Recolors.LogMessage("Patching RoleCardPanel.ShowAttackAndDefense");

        if (!Constants.EnableIcons)
            return;

        var attack = AssetManager.GetSprite($"Attack{Utils.GetLevel(data.attack, true)}");

        if (attack != AssetManager.Blank)
            __instance.transform.Find("AttackIcon").Find("Icon").GetComponent<Image>().sprite = attack;

        var defense = AssetManager.GetSprite($"Defense{Utils.GetLevel(data.defense, false)}");

        if (defense != AssetManager.Blank)
            __instance.transform.Find("DefenseIcon").Find("Icon").GetComponent<Image>().sprite = defense;
    }
}

[HarmonyPatch(typeof(RoleCardPopupPanel), nameof(RoleCardPopupPanel.ShowAttackAndDefense))]
public static class PatchAttackDefensePopup
{
    public static void Postfix(RoleCardPopupPanel __instance, ref RoleCardData data)
    {
        Recolors.LogMessage("Patching RoleCardPopupPanel.ShowAttackAndDefense");

        if (!Constants.EnableIcons)
            return;

        var attack = AssetManager.GetSprite($"Attack{Utils.GetLevel(data.attack, true)}", false);
        var icon1 = __instance.transform.Find("AttackIcon").Find("Icon").GetComponent<Image>();

        if (attack != AssetManager.Blank && icon1)
            icon1.sprite = attack;

        var defense = AssetManager.GetSprite($"Defense{Utils.GetLevel(data.defense, false)}", false);
        var icon2 = __instance.transform.Find("DefenseIcon").Find("Icon").GetComponent<Image>();

        if (defense != AssetManager.Blank && icon2)
            icon2.sprite = defense;
    }
}

[HarmonyPatch(typeof(TosAbilityPanelListItem), nameof(TosAbilityPanelListItem.SetKnownRole))]
public static class FixApocNaming
{
    public static void Postfix(TosAbilityPanelListItem __instance, ref Role role)
    {
        Recolors.LogMessage("Patching TosAbilityPanelListItem.SetKnownRole");

        if (role != Pepper.GetMyRole() || !role.IsApoc())
            return;

        if (Constants.IsTransformed)
        {
            role = role switch
            {
                Role.BAKER => Role.FAMINE,
                Role.BERSERKER => Role.WAR,
                Role.SOULCOLLECTOR => Role.DEATH,
                Role.PLAGUEBEARER => Role.PESTILENCE,
                _ => role
            };
        }

        ColorUtility.TryParseHtmlString(role.GetFaction().GetFactionColor(), out var color);
        __instance.playerRoleText.color = color;
        __instance.playerRoleText.text = $"({role.ToDisplayString()})";
    }
}