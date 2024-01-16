using Game.Services;
using Home.Shared;
using Cinematics.Players;
using Home.Services;
using UnityEngine.UI;

namespace IconPacks;

[HarmonyPatch(typeof(RoleCardListItem), nameof(RoleCardListItem.SetData))]
public static class PatchRoleDeckBuilder
{
    public static void Postfix(RoleCardListItem __instance, ref Role role)
    {
        if (!Constants.EnableIcons)
            return;

        Recolors.LogMessage("Patching RoleCardListItem.SetData");
        var icon = AssetManager.GetSprite(Utils.RoleName(role), Utils.FactionName(role.GetFaction()), false);

        if (__instance.roleImage && icon != AssetManager.Blank)
            __instance.roleImage.sprite = icon;
    }
}

[HarmonyPatch(typeof(RoleDeckListItem), nameof(RoleDeckListItem.SetData))]
public static class PatchRoleListPanel
{
    public static void Postfix(RoleDeckListItem __instance, ref Role a_role, ref bool a_isBan)
    {
        if (!Constants.EnableIcons || a_isBan)
            return;

        Recolors.LogMessage("Patching RoleDeckListItem.SetData");
        var icon = AssetManager.GetSprite(Utils.RoleName(a_role), Utils.FactionName(a_role.GetFaction()), false);

        if (__instance.roleImage && icon != AssetManager.Blank)
            __instance.roleImage.sprite = icon;
    }
}

[HarmonyPatch(typeof(GameBrowserRoleDeckListItem), nameof(GameBrowserRoleDeckListItem.SetData))]
public static class PatchBrowserRoleListPanel
{
    public static void Postfix(GameBrowserRoleDeckListItem __instance, ref Role a_role, ref bool a_isBan)
    {
        if (!Constants.EnableIcons || a_isBan)
            return;

        Recolors.LogMessage("Patching GameBrowserRoleDeckListItem.SetData");
        var icon = AssetManager.GetSprite(Utils.RoleName(a_role), Utils.FactionName(a_role.GetFaction()), false);

        if (__instance.roleImage && icon != AssetManager.Blank)
            __instance.roleImage.sprite = icon;
    }
}

[HarmonyPatch(typeof(RoleCardPanelBackground), nameof(RoleCardPanelBackground.SetRole))]
public static class PatchRoleCards
{
    public static void Postfix(RoleCardPanelBackground __instance, ref Role role)
    {
        if (!Constants.EnableIcons)
            return;

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
        //this determines if the role in question is changed by dum's mod
        var isModifiedByTos1UI = Utils.ModifiedByToS1UI(role) && ModStates.IsLoaded("dum.oldui");
        var index = 0;
        var name = Utils.RoleName(role);
        var faction = Utils.FactionName(Pepper.GetMyFaction());
        var sprite = AssetManager.GetSprite(name, faction);

        if (sprite != AssetManager.Blank && panel.roleIcon)
            panel.roleIcon.sprite = sprite;

        var specialName = $"{name}_Special";
        var special = AssetManager.GetSprite(specialName, faction);

        if (special != AssetManager.Blank && panel.specialAbilityPanel && role != Role.NECROMANCER && !Constants.IsNecroActive)
            panel.specialAbilityPanel.useButton.abilityIcon.sprite = special;

        var abilityname = $"{name}_Ability";
        var ability1 = AssetManager.GetSprite(abilityname, faction);

        if (ability1 == AssetManager.Blank)
            ability1 = AssetManager.GetSprite(abilityname + "_1", faction);

        if (ability1 != AssetManager.Blank && panel.roleInfoButtons.Exists(index))
        {
            panel.roleInfoButtons[index].abilityIcon.sprite = ability1;
            index++;
        }
        else if (Utils.Skippable(abilityname) || Utils.Skippable(abilityname + "_1"))
            index++;
        else if (isModifiedByTos1UI)
        {
            if (special != AssetManager.Blank)
            {
                panel.roleInfoButtons[index].abilityIcon.sprite = special;
                index++;
            }
            else if (Utils.Skippable(specialName))
                index++;

            isModifiedByTos1UI = false;
        }

        var abilityname2 = $"{name}_Ability_2";
        var ability2 = AssetManager.GetSprite(abilityname2, faction);

        if (ability2 != AssetManager.Blank && panel.roleInfoButtons.Exists(index) && role != Role.WAR)
        {
            panel.roleInfoButtons[index].abilityIcon.sprite = ability2;
            index++;
        }
        else if (Utils.Skippable(abilityname2))
            index++;
        else if (isModifiedByTos1UI)
        {
            if (special != AssetManager.Blank)
            {
                panel.roleInfoButtons[index].abilityIcon.sprite = special;
                index++;
            }
            else if (Utils.Skippable(specialName))
                index++;

            isModifiedByTos1UI = false;
        }

        var attributename = role.IsTransformedApoc() ? "Attributes_Horsemen" : $"Attributes_{faction}";
        var attribute = AssetManager.GetSprite(attributename);

        if (attribute != AssetManager.Blank && panel.roleInfoButtons.Exists(index))
        {
            panel.roleInfoButtons[index].abilityIcon.sprite = attribute;
            index++;
        }
        else if (Utils.Skippable(attributename))
            index++;

        if (faction != "Coven")
            return;

        var nommy = AssetManager.GetSprite("Necronomicon");

        if (nommy != AssetManager.Blank && Constants.IsNecroActive && panel.roleInfoButtons.Exists(index))
            panel.roleInfoButtons[index].abilityIcon.sprite = nommy;
    }
}

[HarmonyPatch(typeof(TosAbilityPanelListItem), nameof(TosAbilityPanelListItem.OverrideIconAndText))]
public static class PatchAbilityPanel
{
    public static void Postfix(TosAbilityPanelListItem __instance, ref TosAbilityPanelListItem.OverrideAbilityType overrideType)
    {
        if (!Constants.EnableIcons || overrideType == TosAbilityPanelListItem.OverrideAbilityType.VOTING)
            return;

        Recolors.LogMessage("Patching TosAbilityPanelListItem.OverrideIconAndText");
        var role = Pepper.GetMyRole();
        var faction = Pepper.GetMyFaction();

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

        switch (overrideType)
        {
            case TosAbilityPanelListItem.OverrideAbilityType.NECRO_ATTACK:
                var nommy = AssetManager.GetSprite("Necronomicon", Constants.PlayerPanelEasterEggs);

                if (nommy != AssetManager.Blank)
                {
                    if (role == Role.ILLUSIONIST && __instance.choice2Sprite)
                    {
                        __instance.choice2Sprite.sprite = nommy;
                        var illu = AssetManager.GetSprite("Illusionist_Ability", Utils.FactionName(faction), Constants.PlayerPanelEasterEggs);

                        if (illu != AssetManager.Blank && __instance.choice1Sprite)
                            __instance.choice1Sprite.sprite = illu;
                    }
                    else if (__instance.choice1Sprite)
                    {
                        __instance.choice1Sprite.sprite = nommy;

                        if (role == Role.WITCH)
                        {
                            var target = AssetManager.GetSprite("Witch_Ability_2", Utils.FactionName(faction), Constants.PlayerPanelEasterEggs);

                            if (target != AssetManager.Blank && __instance.choice2Sprite)
                                __instance.choice2Sprite.sprite = target;
                        }
                    }
                }

                break;

            case TosAbilityPanelListItem.OverrideAbilityType.POTIONMASTER_ATTACK or TosAbilityPanelListItem.OverrideAbilityType.POISONER_POISON or TosAbilityPanelListItem.OverrideAbilityType.SHROUD:
                var special = AssetManager.GetSprite($"{Utils.RoleName(role)}_Special", Utils.FactionName(faction), Constants.PlayerPanelEasterEggs);

                if (special != AssetManager.Blank && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = special;

                break;

            case TosAbilityPanelListItem.OverrideAbilityType.POTIONMASTER_HEAL:
                var heal = AssetManager.GetSprite($"{Utils.RoleName(role)}_Ability_1", Utils.FactionName(faction), Constants.PlayerPanelEasterEggs);

                if (heal != AssetManager.Blank && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = heal;

                break;

            case TosAbilityPanelListItem.OverrideAbilityType.POTIONMASTER_REVEAL:
                var reveal = AssetManager.GetSprite($"{Utils.RoleName(role)}_Ability_2", Utils.FactionName(faction), Constants.PlayerPanelEasterEggs);

                if (reveal != AssetManager.Blank && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = reveal;

                break;

            case TosAbilityPanelListItem.OverrideAbilityType.WEREWOLF_NON_FULL_MOON:
                var sniff = AssetManager.GetSprite($"{Utils.RoleName(role)}_Ability_2", Utils.FactionName(faction), Constants.PlayerPanelEasterEggs);

                if (sniff != AssetManager.Blank && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = sniff;

                break;

            default:
                var abilityName = $"{Utils.RoleName(role)}_Ability";
                var ability1 = AssetManager.GetSprite(abilityName, Utils.FactionName(faction), Constants.PlayerPanelEasterEggs);

                if (ability1 == AssetManager.Blank)
                    ability1 = AssetManager.GetSprite(abilityName + "_1", Utils.FactionName(faction), Constants.PlayerPanelEasterEggs);

                if (ability1 != AssetManager.Blank && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = ability1;

                var ability2 = AssetManager.GetSprite($"{Utils.RoleName(role)}_Ability_2", Utils.FactionName(faction), Constants.PlayerPanelEasterEggs);

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
        if (!Constants.EnableIcons)
            return;

        Recolors.LogMessage("Patching SpecialAbilityPopupGenericListItem.SetData");
        var special = AssetManager.GetSprite($"{Utils.RoleName(Pepper.GetMyRole())}_Special", Utils.FactionName(Pepper.GetMyFaction()));

        if (special != AssetManager.Blank)
            __instance.choiceSprite.sprite = special;
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupRadialIcon), nameof(SpecialAbilityPopupRadialIcon.SetData))]
public static class PatchRitualistGuessMenu
{
    public static void Postfix(SpecialAbilityPopupRadialIcon __instance, ref Role a_role)
    {
        if (!Constants.EnableIcons)
            return;

        Recolors.LogMessage("Patching SpecialAbilityPopupRadialIcon.SetData");
        var icon = AssetManager.GetSprite(Utils.RoleName(a_role), Utils.FactionName(a_role.GetFaction()), false);

        if (__instance.roleIcon != null && icon != AssetManager.Blank)
            __instance.roleIcon.sprite = icon;
    }
}

[HarmonyPatch(typeof(RoleService), nameof(RoleService.Init))]
[HarmonyPriority(Priority.Low)]
public static class PatchRoleService
{
    public static bool ServiceExists;

    public static void Postfix()
    {
        Recolors.LogMessage("Patching RoleService.Init");

        if (AssetManager.IconPacks.TryGetValue(Constants.CurrentPack, out var pack))
            pack.LoadSpriteSheets(true);

        AssetManager.LoadVanillaSpriteSheet(!Constants.EnableIcons);
        ServiceExists = true;
    }
}

[HarmonyPatch(typeof(ApplicationController), nameof(ApplicationController.QuitGame))]
public static class ExitGamePatch
{
    public static void Prefix()
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
        if (!Constants.EnableIcons)
            return;

        Recolors.LogMessage("Patching RoleCardPopupPanel.SetRole");
        var index = 0;
        var name = Utils.RoleName(role);
        var faction = Utils.FactionName(role.GetFaction());
        var sprite = AssetManager.GetSprite(name, faction);
        //this determines if the role in question is changed by dum's mod
        var isModifiedByTos1UI = Utils.ModifiedByToS1UI(role) && ModStates.IsLoaded("dum.oldui");

        if (sprite != AssetManager.Blank && __instance.roleIcon)
            __instance.roleIcon.sprite = sprite;

        var specialName = $"{name}_Special";
        var special = AssetManager.GetSprite(specialName, faction);

        if (special != AssetManager.Blank && __instance.specialAbilityPanel && role != Role.NECROMANCER)
            __instance.specialAbilityPanel.useButton.abilityIcon.sprite = special;

        var abilityname = $"{name}_Ability";
        var ability1 = AssetManager.GetSprite(abilityname, faction);

        if (ability1 == AssetManager.Blank)
            ability1 = AssetManager.GetSprite(abilityname + "_1", faction);

        if (ability1 != AssetManager.Blank && __instance.roleInfoButtons.Exists(index))
        {
            __instance.roleInfoButtons[index].abilityIcon.sprite = ability1;
            index++;
        }
        else if (Utils.Skippable(abilityname) || Utils.Skippable(abilityname + "_1"))
            index++;
        else if (isModifiedByTos1UI)
        {
            if (special != AssetManager.Blank && __instance.roleInfoButtons.Exists(index))
            {
                __instance.roleInfoButtons[index].abilityIcon.sprite = special;
                index++;
            }
            else if (Utils.Skippable(specialName))
                index++;

            isModifiedByTos1UI = false;
        }

        var abilityname2 = $"{name}_Ability_2";
        var ability2 = AssetManager.GetSprite(abilityname2, faction);

        if (ability2 != AssetManager.Blank && __instance.roleInfoButtons.Exists(index))
        {
            __instance.roleInfoButtons[index].abilityIcon.sprite = ability2;
            index++;
        }
        else if (Utils.Skippable(abilityname2))
            index++;
        else if (isModifiedByTos1UI)
        {
            if (special != AssetManager.Blank && __instance.roleInfoButtons.Exists(index))
            {
                __instance.roleInfoButtons[index].abilityIcon.sprite = special;
                index++;
            }
            else if (Utils.Skippable(specialName))
                index++;

            isModifiedByTos1UI = false;
        }

        var attributename = role.IsTransformedApoc() ? "Attributes_Horsemen" : $"Attributes_{faction}";
        var attribute = AssetManager.GetSprite(attributename);

        if (attribute != AssetManager.Blank && __instance.roleInfoButtons.Exists(index))
        {
            __instance.roleInfoButtons[index].abilityIcon.sprite = attribute;
            index++;
        }
        else if (Utils.Skippable(attributename))
            index++;

        if (faction != "Coven")
            return;

        var nommy = AssetManager.GetSprite("Necronomicon");

        if (nommy != AssetManager.Blank && __instance.roleInfoButtons.Exists(index))
            __instance.roleInfoButtons[index].abilityIcon.sprite = nommy;
    }
}

[HarmonyPatch(typeof(DoomsayerLeavesCinematicPlayer), nameof(DoomsayerLeavesCinematicPlayer.Init))]
public static class PatchDoomsayerLeaving
{
    public static void Postfix(DoomsayerLeavesCinematicPlayer __instance)
    {
        if (!Constants.EnableIcons)
            return;

        Recolors.LogMessage("Patching DoomsayerLeavesCinematicPlayer.Init");

        var role1 = __instance.doomsayerLeavesCinematicData.roles[0];
        var role2 = __instance.doomsayerLeavesCinematicData.roles[1];
        var role3 = __instance.doomsayerLeavesCinematicData.roles[2];

        var sprite1 = AssetManager.GetSprite(Utils.RoleName(role1), Utils.FactionName(role1.GetFaction()));
        var sprite2 = AssetManager.GetSprite(Utils.RoleName(role2), Utils.FactionName(role2.GetFaction()));
        var sprite3 = AssetManager.GetSprite(Utils.RoleName(role3), Utils.FactionName(role3.GetFaction()));

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
    public static int Hash1;
    public static int Hash2;
    public static TMP_SpriteAsset Cache1;
    public static TMP_SpriteAsset Cache2;
    private static readonly string[] Assets = new[] { "Cast", "LobbyIcons", "MiscIcons", "PlayerNumbers", "RoleIcons", "SalemTmpIcons", "TrialReportIcons" };

    public static bool Prefix(HomeInterfaceService __instance)
    {
        Recolors.LogMessage("Patching HomeInterfaceService.Init");
        Assets.ForEach(key =>
        {
            Debug.Log($"HomeInterfaceService:: Add Sprite Asset {key}");
            var asset = __instance.LoadResource<TMP_SpriteAsset>($"TmpSpriteAssets/{key}.asset");
            MaterialReferenceManager.AddSpriteAsset(asset);

            if (key == "RoleIcons")
            {
                Hash1 = asset.hashCode;
                Cache1 = asset;
            }
            else if (key == "PlayerNumbers")
            {
                Hash2 = asset.hashCode;
                Cache2 = asset;
            }

            if (key is "RoleIcons" or "PlayerNumbers")
                Utils.DumpSprite(asset.spriteSheet as Texture2D, $"{key}.png", AssetManager.VanillaPath);
        });
        __instance.isReady_ = true;
        return false;
    }
}

[HarmonyPatch(typeof(HomeScrollService), nameof(HomeScrollService.Init))]
[HarmonyPriority(Priority.Low)]
public static class PatchScrolls
{
    public static bool ServiceExists;

    public static void Postfix()
    {
        if (!Constants.EnableIcons)
            return;

        Recolors.LogMessage("Patching HomeScrollService.Init");
        AssetManager.SetScrollSprites();
        ServiceExists = true;
    }
}

[HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.ShowAttackAndDefense))]
public static class PatchAttackDefense
{
    public static void Postfix(RoleCardPanel __instance, ref RoleCardData data)
    {
        if (!Constants.EnableIcons)
            return;

        Recolors.LogMessage("Patching RoleCardPanel.ShowAttackAndDefense");
        var attack = AssetManager.GetSprite($"Attack{Utils.GetLevel(data.attack, true)}");
        var icon1 = __instance.transform.Find("AttackIcon").Find("Icon").GetComponent<Image>();

        if (icon1)
        {
            AssetManager.Attack ??= icon1.sprite;
            icon1.sprite = attack != AssetManager.Blank ? attack : AssetManager.Attack;
        }

        var defense = AssetManager.GetSprite($"Defense{Utils.GetLevel(data.defense, false)}");
        var icon2 = __instance.transform.Find("DefenseIcon").Find("Icon").GetComponent<Image>();

        if (icon2)
        {
            AssetManager.Defense ??= icon2.sprite;
            icon2.sprite = defense != AssetManager.Blank ? defense : AssetManager.Defense;
        }
    }
}

[HarmonyPatch(typeof(RoleCardPopupPanel), nameof(RoleCardPopupPanel.ShowAttackAndDefense))]
public static class PatchAttackDefensePopup
{
    public static void Postfix(RoleCardPopupPanel __instance, ref RoleCardData data)
    {
        if (!Constants.EnableIcons)
            return;

        Recolors.LogMessage("Patching RoleCardPopupPanel.ShowAttackAndDefense");
        var attack = AssetManager.GetSprite($"Attack{Utils.GetLevel(data.attack, true)}");
        var icon1 = __instance.transform.Find("AttackIcon").Find("Icon").GetComponent<Image>();

        if (icon1)
        {
            AssetManager.Attack ??= icon1.sprite;
            icon1.sprite = attack != AssetManager.Blank ? attack : AssetManager.Attack;
        }

        var defense = AssetManager.GetSprite($"Defense{Utils.GetLevel(data.defense, false)}");
        var icon2 = __instance.transform.Find("DefenseIcon").Find("Icon").GetComponent<Image>();

        if (icon2)
        {
            AssetManager.Defense ??= icon2.sprite;
            icon2.sprite = defense != AssetManager.Blank ? defense : AssetManager.Defense;
        }
    }
}

[HarmonyPatch(typeof(PlayerPopupController), nameof(PlayerPopupController.SetRoleIcon))]
public static class PlayerPopupControllerPatch
{
    public static void Postfix(PlayerPopupController __instance)
    {
        if (!Constants.EnableIcons)
            return;

        Recolors.LogMessage("Patching PlayerPopupController.SetRoleIcon");
        var sprite = AssetManager.GetSprite(Utils.RoleName(__instance.m_role), Utils.FactionName(__instance.m_role.GetFaction()));

        if (sprite != AssetManager.Blank && __instance.RoleIcon)
            __instance.RoleIcon.sprite = sprite;
    }
}

[HarmonyPatch(typeof(RoleMenuPopupController), nameof(RoleMenuPopupController.SetRoleIconAndLabels))]
public static class RoleMenuPopupControllerPatch
{
    public static void Postfix(RoleMenuPopupController __instance)
    {
        if (!Constants.EnableIcons)
            return;

        Recolors.LogMessage("Patching RoleMenuPopupController.SetRoleIconAndLabels");
        var sprite = AssetManager.GetSprite(Utils.RoleName(__instance.m_role), Utils.FactionName(__instance.m_role.GetFaction()));

        if (sprite != AssetManager.Blank && __instance.RoleIconImage && __instance.HeaderRoleIconImage)
        {
            __instance.RoleIconImage.sprite = sprite;
            __instance.HeaderRoleIconImage.sprite = sprite;
        }
    }
}