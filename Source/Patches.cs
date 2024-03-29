using Cinematics.Players;
using Home.Services;
using UnityEngine.UI;
using SalemModLoader;
using Server.Shared.Extensions;

namespace IconPacks;

[HarmonyPatch(typeof(RoleCardListItem), nameof(RoleCardListItem.SetData))]
public static class PatchRoleDeckBuilder
{
    public static void Postfix(RoleCardListItem __instance, ref Role role)
    {
        if (!Constants.EnableIcons)
            return;

        Logging.LogMessage("Patching RoleCardListItem.SetData");
        var icon = AssetManager.GetSprite(Utils.RoleName(role), Utils.FactionName(role.GetFaction()), false);

        if (__instance.roleImage && icon.IsValid())
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

        Logging.LogMessage("Patching RoleDeckListItem.SetData");
        var icon = AssetManager.GetSprite(Utils.RoleName(a_role), Utils.FactionName(a_role.GetFaction()), false);

        if (__instance.roleImage && icon.IsValid())
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

        Logging.LogMessage("Patching GameBrowserRoleDeckListItem.SetData");
        var icon = AssetManager.GetSprite(Utils.RoleName(a_role), Utils.FactionName(a_role.GetFaction()), false);

        if (__instance.roleImage && icon.IsValid())
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

        Logging.LogMessage("Patching RoleCardPanelBackground.SetRole");

        if (Constants.IsTransformed)
        {
            if (Constants.IsBTOS2)
            {
                role = role switch
                {
                    RolePlus.BAKER => RolePlus.FAMINE,
                    RolePlus.BERSERKER => RolePlus.WAR,
                    RolePlus.SOUL_COLLECTOR => RolePlus.DEATH,
                    RolePlus.PLAGUEBEARER => RolePlus.PESTILENCE,
                    _ => role
                };
            }
            else
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
        }

        var panel = __instance.GetComponentInParent<RoleCardPanel>();
        //this determines if the role in question is changed by dum's mod
        var isModifiedByTos1UI = Utils.ModifiedByToS1UI(role) && ModStates.IsLoaded("dum.oldui");
        var index = 0;
        var name = Utils.RoleName(role);
        var faction = Utils.FactionName(Pepper.GetMyFaction());
        var sprite = AssetManager.GetSprite(name, faction);

        if (sprite.IsValid() && panel.roleIcon)
            panel.roleIcon.sprite = sprite;

        var specialName = $"{name}_Special";
        var special = AssetManager.GetSprite(specialName, faction);

        if (special.IsValid() && panel.specialAbilityPanel && !(role == (Constants.IsBTOS2 ? RolePlus.NECROMANCER : Role.NECROMANCER) && !Constants.IsNecroActive))
            panel.specialAbilityPanel.useButton.abilityIcon.sprite = special;

        var abilityname = $"{name}_Ability";
        var ability1 = AssetManager.GetSprite(abilityname, faction);

        if (!ability1.IsValid())
            ability1 = AssetManager.GetSprite(abilityname + "_1", faction);

        if (ability1.IsValid() && panel.roleInfoButtons.Exists(index))
        {
            panel.roleInfoButtons[index].abilityIcon.sprite = ability1;
            index++;
        }
        else if (Utils.Skippable(abilityname) || Utils.Skippable(abilityname + "_1"))
            index++;
        else if (isModifiedByTos1UI)
        {
            if (special.IsValid())
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

        if (ability2.IsValid() && panel.roleInfoButtons.Exists(index) && role != (Constants.IsBTOS2 ? RolePlus.WAR : Role.WAR))
        {
            panel.roleInfoButtons[index].abilityIcon.sprite = ability2;
            index++;
        }
        else if (Utils.Skippable(abilityname2))
            index++;
        else if (isModifiedByTos1UI)
        {
            if (special.IsValid())
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

        if (attribute.IsValid() && panel.roleInfoButtons.Exists(index))
        {
            panel.roleInfoButtons[index].abilityIcon.sprite = attribute;
            index++;
        }
        else if (Utils.Skippable(attributename))
            index++;

        if (faction != "Coven")
            return;

        var nommy = AssetManager.GetSprite("Necronomicon");

        if (nommy.IsValid() && Constants.IsNecroActive && panel.roleInfoButtons.Exists(index))
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

        Logging.LogMessage("Patching TosAbilityPanelListItem.OverrideIconAndText");
        var role = Pepper.GetMyRole();
        var faction = Pepper.GetMyFaction();

        if (Constants.IsTransformed)
        {
            if (Constants.IsBTOS2)
            {
                role = role switch
                {
                    RolePlus.BAKER => RolePlus.FAMINE,
                    RolePlus.BERSERKER => RolePlus.WAR,
                    RolePlus.SOUL_COLLECTOR => RolePlus.DEATH,
                    RolePlus.PLAGUEBEARER => RolePlus.PESTILENCE,
                    _ => role
                };
            }
            else
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
        }

        switch (overrideType)
        {
            case TosAbilityPanelListItem.OverrideAbilityType.NECRO_ATTACK:
                var nommy = AssetManager.GetSprite("Necronomicon", Constants.PlayerPanelEasterEggs);

                if (nommy.IsValid())
                {
                    if (role == Role.ILLUSIONIST && __instance.choice2Sprite)
                    {
                        __instance.choice2Sprite.sprite = nommy;
                        var illu = AssetManager.GetSprite("Illusionist_Ability", Utils.FactionName(faction), Constants.PlayerPanelEasterEggs);

                        if (illu.IsValid() && __instance.choice1Sprite)
                            __instance.choice1Sprite.sprite = illu;
                    }
                    else if (__instance.choice1Sprite)
                    {
                        __instance.choice1Sprite.sprite = nommy;

                        if (role == Role.WITCH)
                        {
                            var target = AssetManager.GetSprite("Witch_Ability_2", Utils.FactionName(faction), Constants.PlayerPanelEasterEggs);

                            if (target.IsValid() && __instance.choice2Sprite)
                                __instance.choice2Sprite.sprite = target;
                        }
                    }
                }

                break;

            case TosAbilityPanelListItem.OverrideAbilityType.POTIONMASTER_ATTACK:
            case TosAbilityPanelListItem.OverrideAbilityType.POISONER_POISON:
            case TosAbilityPanelListItem.OverrideAbilityType.SHROUD:
            case TosAbilityPanelListItem.OverrideAbilityType.INVESTIGATOR:
                var special = AssetManager.GetSprite($"{Utils.RoleName(role)}_Special", Utils.FactionName(faction), Constants.PlayerPanelEasterEggs);

                if (special.IsValid() && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = special;

                break;

            case TosAbilityPanelListItem.OverrideAbilityType.POTIONMASTER_HEAL:
                var ab1 = AssetManager.GetSprite($"{Utils.RoleName(role)}_Ability_1", Utils.FactionName(faction), Constants.PlayerPanelEasterEggs);

                if (ab1.IsValid() && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = ab1;

                break;

            case TosAbilityPanelListItem.OverrideAbilityType.POTIONMASTER_REVEAL:
            case TosAbilityPanelListItem.OverrideAbilityType.WEREWOLF_NON_FULL_MOON:
                var ab2 = AssetManager.GetSprite($"{Utils.RoleName(role)}_Ability_2", Utils.FactionName(faction), Constants.PlayerPanelEasterEggs);

                if (ab2.IsValid() && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = ab2;

                break;

            default:
                var abilityName = $"{Utils.RoleName(role)}_Ability";
                var ability1 = AssetManager.GetSprite(abilityName, Utils.FactionName(faction), Constants.PlayerPanelEasterEggs);

                if (!ability1.IsValid())
                    ability1 = AssetManager.GetSprite(abilityName + "_1", Utils.FactionName(faction), Constants.PlayerPanelEasterEggs);

                if (ability1.IsValid() && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = ability1;

                var ability2 = AssetManager.GetSprite($"{Utils.RoleName(role)}_Ability_2", Utils.FactionName(faction), Constants.PlayerPanelEasterEggs);

                if (ability2.IsValid() && __instance.choice2Sprite)
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

        Logging.LogMessage("Patching SpecialAbilityPopupGenericListItem.SetData");
        var special = AssetManager.GetSprite($"{Utils.RoleName(Pepper.GetMyRole())}_Special", Utils.FactionName(Pepper.GetMyFaction()));

        if (special.IsValid())
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

        Logging.LogMessage("Patching SpecialAbilityPopupRadialIcon.SetData");
        var icon = AssetManager.GetSprite(Utils.RoleName(a_role), Utils.FactionName(a_role.GetFaction()), false);

        if (__instance.roleIcon && icon.IsValid())
            __instance.roleIcon.sprite = icon;
    }
}

[HarmonyPatch(typeof(RoleCardPopupPanel), nameof(RoleCardPopupPanel.SetRole))]
public static class PatchGuideRoleCards
{
    public static void Postfix(RoleCardPopupPanel __instance, ref Role role)
    {
        if (!Constants.EnableIcons)
            return;

        Logging.LogMessage("Patching RoleCardPopupPanel.SetRole");
        var index = 0;
        var name = Utils.RoleName(role);
        var faction = Utils.FactionName(role.GetFaction());
        var sprite = AssetManager.GetSprite(name, faction);
        //this determines if the role in question is changed by dum's mod
        var isModifiedByTos1UI = Utils.ModifiedByToS1UI(role) && ModStates.IsLoaded("dum.oldui");

        if (sprite.IsValid() && __instance.roleIcon)
            __instance.roleIcon.sprite = sprite;

        var specialName = $"{name}_Special";
        var special = AssetManager.GetSprite(specialName, faction);

        if (special.IsValid() && __instance.specialAbilityPanel && role != Role.NECROMANCER)
            __instance.specialAbilityPanel.useButton.abilityIcon.sprite = special;

        var abilityname = $"{name}_Ability";
        var ability1 = AssetManager.GetSprite(abilityname, faction);

        if (!ability1.IsValid())
            ability1 = AssetManager.GetSprite(abilityname + "_1", faction);

        if (ability1.IsValid() && __instance.roleInfoButtons.Exists(index))
        {
            __instance.roleInfoButtons[index].abilityIcon.sprite = ability1;
            index++;
        }
        else if (Utils.Skippable(abilityname) || Utils.Skippable(abilityname + "_1"))
            index++;
        else if (isModifiedByTos1UI)
        {
            if (special.IsValid() && __instance.roleInfoButtons.Exists(index))
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

        if (ability2.IsValid() && __instance.roleInfoButtons.Exists(index))
        {
            __instance.roleInfoButtons[index].abilityIcon.sprite = ability2;
            index++;
        }
        else if (Utils.Skippable(abilityname2))
            index++;
        else if (isModifiedByTos1UI)
        {
            if (special.IsValid() && __instance.roleInfoButtons.Exists(index))
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

        if (attribute.IsValid() && __instance.roleInfoButtons.Exists(index))
        {
            __instance.roleInfoButtons[index].abilityIcon.sprite = attribute;
            index++;
        }
        else if (Utils.Skippable(attributename))
            index++;

        if (faction != "Coven")
            return;

        var nommy = AssetManager.GetSprite("Necronomicon");

        if (nommy.IsValid() && __instance.roleInfoButtons.Exists(index))
            __instance.roleInfoButtons[index].abilityIcon.sprite = nommy;
    }
}

[HarmonyPatch(typeof(DoomsayerLeavesCinematicPlayer), nameof(DoomsayerLeavesCinematicPlayer.Init))]
public static class PatchDoomsayerLeaving
{
    public static void Postfix(DoomsayerLeavesCinematicPlayer __instance)
    {
        if (!Constants.EnableIcons || Constants.IsBTOS2)
            return;

        Logging.LogMessage("Patching DoomsayerLeavesCinematicPlayer.Init");

        var role1 = __instance.doomsayerLeavesCinematicData.roles[0];
        var role2 = __instance.doomsayerLeavesCinematicData.roles[1];
        var role3 = __instance.doomsayerLeavesCinematicData.roles[2];

        var sprite1 = AssetManager.GetSprite(Utils.RoleName(role1), Utils.FactionName(role1.GetFaction()));
        var sprite2 = AssetManager.GetSprite(Utils.RoleName(role2), Utils.FactionName(role2.GetFaction()));
        var sprite3 = AssetManager.GetSprite(Utils.RoleName(role3), Utils.FactionName(role3.GetFaction()));

        if (sprite1.IsValid())
            __instance.otherCharacterRole1.sprite = sprite1;

        if (sprite2.IsValid())
            __instance.otherCharacterRole2.sprite = sprite2;

        if (sprite3.IsValid())
            __instance.otherCharacterRole3.sprite = sprite3;
    }
}

[HarmonyPatch(typeof(HomeInterfaceService), nameof(HomeInterfaceService.Init))]
public static class CacheDefaultSpriteSheet
{
    public static bool ServiceExists;

    public static TMP_SpriteAsset Cache1;
    public static TMP_SpriteAsset Cache2;

    private static readonly List<string> Assets = [ "Cast", "LobbyIcons", "MiscIcons", "PlayerNumbers", "RoleIcons", "SalemTmpIcons", "TrialReportIcons" ];

    public static bool Prefix(HomeInterfaceService __instance)
    {
        Logging.LogMessage("Patching HomeInterfaceService.Init");
        Assets.ForEach(key =>
        {
            Debug.Log($"HomeInterfaceService:: Add Sprite Asset {key}");
            var asset = __instance.LoadResource<TMP_SpriteAsset>($"TmpSpriteAssets/{key}.asset");

            if (key == "RoleIcons")
                Cache1 = asset;
            else if (key == "PlayerNumbers")
                Cache2 = asset;

            if (key is "RoleIcons" or "PlayerNumbers")
            {
                Utils.DumpSprite(asset.spriteSheet as Texture2D, key);

                if (key == "PlayerNumbers")
                    MaterialReferenceManager.AddSpriteAsset(asset);
            }
            else
                MaterialReferenceManager.AddSpriteAsset(asset);
        });

        AssetManager.SetScrollSprites();
        ServiceExists = true;
        __instance.isReady_ = true;
        return false;
    }
}

[HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.ShowAttackAndDefense))]
public static class PatchAttackDefense
{
    public static void Postfix(RoleCardPanel __instance, ref RoleCardData data)
    {
        if (!Constants.EnableIcons)
            return;

        Logging.LogMessage("Patching RoleCardPanel.ShowAttackAndDefense");
        var attack = AssetManager.GetSprite($"Attack{Utils.GetLevel(data.attack, true)}");
        var icon1 = __instance.transform.Find("AttackIcon").Find("Icon").GetComponent<Image>();

        if (icon1)
        {
            AssetManager.Attack ??= icon1.sprite;
            icon1.sprite = attack.IsValid() ? attack : AssetManager.Attack;
        }

        var defLevel = Constants.IsBTOS2 ? (__instance.myData.IsEthereal() ? 4 : data.defense) : data.defense;
        var defense = AssetManager.GetSprite($"Defense{Utils.GetLevel(defLevel, false)}");
        var icon2 = __instance.transform.Find("DefenseIcon").Find("Icon").GetComponent<Image>();

        if (icon2)
        {
            if (Constants.IsBTOS2 && __instance.myData.IsEthereal())
                AssetManager.Ethereal ??= icon2.sprite;
            else
                AssetManager.Defense ??= icon2.sprite;

            icon2.sprite = defense.IsValid() ? defense : AssetManager.Defense;
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

        Logging.LogMessage("Patching RoleCardPopupPanel.ShowAttackAndDefense");
        var attack = AssetManager.GetSprite($"Attack{Utils.GetLevel(data.attack, true)}");
        var icon1 = __instance.transform.Find("AttackIcon").Find("Icon").GetComponent<Image>();

        if (icon1)
        {
            AssetManager.Attack ??= icon1.sprite;
            icon1.sprite = attack.IsValid() ? attack : AssetManager.Attack;
        }

        var defLevel = Constants.IsBTOS2 ? (__instance.myData.IsEthereal() ? 4 : data.defense) : data.defense;
        var defense = AssetManager.GetSprite($"Defense{Utils.GetLevel(defLevel, false)}");
        var icon2 = __instance.transform.Find("DefenseIcon").Find("Icon").GetComponent<Image>();

        if (icon2)
        {
            if (Constants.IsBTOS2 && __instance.myData.IsEthereal())
                AssetManager.Ethereal ??= icon2.sprite;
            else
                AssetManager.Defense ??= icon2.sprite;

            icon2.sprite = defense.IsValid() ? defense : AssetManager.Defense;
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

        Logging.LogMessage("Patching PlayerPopupController.SetRoleIcon");
        var sprite = AssetManager.GetSprite(Utils.RoleName(__instance.m_role), Utils.FactionName(__instance.m_role.GetFaction()));

        if (sprite.IsValid() && __instance.RoleIcon)
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

        Logging.LogMessage("Patching RoleMenuPopupController.SetRoleIconAndLabels");
        var sprite = AssetManager.GetSprite(Utils.RoleName(__instance.m_role), Utils.FactionName(__instance.m_role.GetFaction()));

        if (sprite.IsValid() && __instance.RoleIconImage && __instance.HeaderRoleIconImage)
        {
            __instance.RoleIconImage.sprite = sprite;
            __instance.HeaderRoleIconImage.sprite = sprite;
        }
    }
}

[HarmonyPatch(typeof(DownloadContributorTags), nameof(DownloadContributorTags.AddTMPSprites))]
[HarmonyPriority(Priority.Low)]
public static class ApplicationControllerPatch
{
    public static void Postfix()
    {
        var oldSpriteAssetRequest = Traverse.Create<TMP_Text>().Field<Func<int, string, TMP_SpriteAsset>>("OnSpriteAssetRequest").Value;
        TMP_Text.OnSpriteAssetRequest += (_, str) =>
        {
            try
            {
                if (AssetManager.IconPacks.TryGetValue(Constants.CurrentPack, out var pack))
                {
                    if (str == "BTOSRoleIcons")
                        return (pack.BTOS2MentionStyles.TryGetValue(Constants.CurrentStyle, out var style) ? style : AssetManager.BTOS2Asset) ?? AssetManager.BTOS2Asset;
                    else if (str == "RoleIcons")
                    {
                        return ((pack.MentionStyles.TryGetValue(Constants.CurrentStyle, out var style) ? style : AssetManager.VanillaAsset) ?? AssetManager.VanillaAsset) ??
                            CacheDefaultSpriteSheet.Cache1;
                    }
                    else
                        return oldSpriteAssetRequest(_, str);
                }
                else if (str == "BTOSRoleIcons")
                    return AssetManager.BTOS2Asset;
                else if (str == "RoleIcons")
                    return AssetManager.VanillaAsset ?? CacheDefaultSpriteSheet.Cache1;
                /*else if (str == "PlayerNumbers")
                    return CacheDefaultSpriteSheet.Cache2;*/
                else
                    return oldSpriteAssetRequest(_, str);
            }
            catch (Exception e)
            {
                AssetManager.RunDiagnostics(e);

                if (str == "BTOSRoleIcons")
                    return AssetManager.BTOS2Asset;
                else if (str == "RoleIcons")
                    return AssetManager.VanillaAsset ?? CacheDefaultSpriteSheet.Cache1;
                /*else if (str == "PlayerNumbers")
                    return CacheDefaultSpriteSheet.Cache2;*/
                else
                    return oldSpriteAssetRequest(_, str);
            }
        };
    }
}