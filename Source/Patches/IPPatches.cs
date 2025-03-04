using Home.Services;
using SalemModLoader;
using Game.Services;
using Game.Simulation;
using Game.Characters;
using Game.Chat.Decoders;
using Server.Shared.Messages;
using Server.Shared.State.Chat;
using Mentions;
using Server.Shared.Extensions;
using Home.Shared;
using System.Text.RegularExpressions;
using AbilityType = Game.Interface.TosAbilityPanelListItem.OverrideAbilityType;
using Cinematics.Players;

namespace FancyUI.Patches;

[HarmonyPatch(typeof(RoleCardListItem), nameof(RoleCardListItem.SetData))]
public static class PatchRoleDeckBuilder
{
    public static void Postfix(RoleCardListItem __instance, Role role)
    {
        if (!Constants.EnableIcons())
            return;

        var icon = GetSprite(Utils.RoleName(role), Utils.FactionName(role.GetFactionType()), false);

        if (__instance.roleImage && icon.IsValid())
            __instance.roleImage.sprite = icon;

        var banned = __instance.bannedImageGO.GetComponent<Image>();
        var sprite = GetSprite("Banned");

        if (sprite.IsValid() && banned)
            banned.sprite = sprite;
    }
}

[HarmonyPatch(typeof(RoleDeckListItem), nameof(RoleDeckListItem.SetData))]
public static class PatchRoleListPanel
{
    public static void Postfix(RoleDeckListItem __instance, Role a_role, bool a_isBan)
    {
        if (!Constants.EnableIcons())
            return;

        var icon = a_isBan ? GetSprite("Banned") : GetSprite(Utils.RoleName(a_role), Utils.FactionName(a_role.GetFactionType()), false);

        if (__instance.roleImage && icon.IsValid())
            __instance.roleImage.sprite = icon;
    }
}

[HarmonyPatch(typeof(GameBrowserRoleDeckListItem), nameof(GameBrowserRoleDeckListItem.SetData))]
public static class PatchBrowserRoleListPanel
{
    public static void Postfix(GameBrowserRoleDeckListItem __instance, Role a_role, bool a_isBan)
    {
        if (!Constants.EnableIcons())
            return;

        var icon = a_isBan ? GetSprite("Banned") : GetSprite(Utils.RoleName(a_role), Utils.FactionName(a_role.GetFactionType()), false);

        if (__instance.roleImage && icon.IsValid())
            __instance.roleImage.sprite = icon;
    }
}

[HarmonyPatch]
public static class PatchRoleCards
{
    [HarmonyPatch(typeof(RoleCardPanelBackground), nameof(RoleCardPanelBackground.SetRole))]
    public static void Postfix(RoleCardPanelBackground __instance, Role role)
    {
        if (Constants.EnableIcons())
        {
            var panel = __instance.GetComponentInParent<RoleCardPanel>();
            ChangeRoleCard(panel?.roleIcon, panel?.specialAbilityPanel?.useButton?.abilityIcon, panel?.roleInfoButtons, role, __instance.currentFaction);
        }

        // Merged a CW patch here for optimisation purposes
        if (Constants.EnableCustomUI())
        {
            foreach (var button in __instance.GetComponentInParent<RoleCardPanel>().roleInfoButtons)
                button.transform.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Metal); // Rings at the back
        }
    }

    [HarmonyPatch(typeof(RoleCardPanelBackground), nameof(RoleCardPanelBackground.SetFaction))]
    public static void Postfix(RoleCardPanelBackground __instance, FactionType factionType)
    {
        if (Constants.EnableIcons())
        {
            var panel = __instance.GetComponentInParent<RoleCardPanel>();
            ChangeRoleCard(panel?.roleIcon, panel?.specialAbilityPanel?.useButton?.abilityIcon, panel?.roleInfoButtons, __instance.currentRole, factionType);
        }

        // Merged a CW patch here for optimisation purposes
        if (Constants.EnableCustomUI())
        {
            foreach (var button in __instance.GetComponentInParent<RoleCardPanel>().roleInfoButtons)
                button.transform.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Metal); // Rings at the back
        }
    }

    [HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.HandleOnMyIdentityChanged))]
    public static void Postfix(RoleCardPanel __instance, PlayerIdentityData playerIdentityData)
    {
        if (Constants.EnableIcons())
            ChangeRoleCard(__instance?.roleIcon, __instance?.specialAbilityPanel?.useButton?.abilityIcon, __instance?.roleInfoButtons, playerIdentityData.role, playerIdentityData.faction);

        // Merged a CW patch here for optimisation purposes
        if (!Constants.EnableCustomUI())
            return;

        foreach (var button in __instance!.roleInfoButtons)
            button.transform.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Metal); // Rings at the back
    }

    [HarmonyPatch(typeof(RoleCardPopupPanel), nameof(RoleCardPopupPanel.SetRoleAndFaction))]
    public static void Postfix(RoleCardPopupPanel __instance, Role role, FactionType faction)
    {
        if (Constants.EnableIcons())
            ChangeRoleCard(__instance?.roleIcon, __instance?.specialAbilityPanel?.useButton?.abilityIcon, __instance?.roleInfoButtons, role, faction, true);

        // Merged a CW patch here for optimisation purposes
        if (!Constants.EnableCustomUI())
            return;

        foreach (var button in __instance!.roleInfoButtons)
            button.transform.GetChild(0).GetComponent<Image>().SetImageColor(ColorType.Metal); // Rings at the back
    }

    private static void ChangeRoleCard(Image roleIcon, Image specialAbilityPanel, List<BaseAbilityButton> roleInfoButtons, Role role, FactionType factionType, bool isGuide = false)
    {
        roleInfoButtons ??= [];
        role = Constants.IsTransformed() ? Utils.GetTransformedVersion(role) : role;
        var index = 0;
        var name = Utils.RoleName(role);
        var faction = Utils.FactionName(factionType);
        var ogfaction = Utils.FactionName(role.GetFactionType(), false);
        var reg = ogfaction != faction;
        var sprite = GetSprite(reg, name, faction);

        if (!sprite.IsValid() && reg)
            sprite = GetSprite(name, ogfaction);

        if (sprite.IsValid() && roleIcon)
            roleIcon.sprite = sprite;

        var specialName = $"{name}_Special";
        var special = GetSprite(reg, specialName, faction);

        if (!special.IsValid() && reg)
            special = GetSprite(specialName, ogfaction);

        if (special.IsValid() && specialAbilityPanel)
            specialAbilityPanel.sprite = special;

        var abilityname = $"{name}_Ability";
        var ability1 = GetSprite(reg, abilityname, faction);

        if (!ability1.IsValid())
            ability1 = GetSprite(reg, abilityname + "_1", faction);

        if (!ability1.IsValid() && reg)
            ability1 = GetSprite(abilityname, ogfaction);

        if (!ability1.IsValid() && reg)
            ability1 = GetSprite(abilityname + "_1", ogfaction);

        if (ability1.IsValid() && roleInfoButtons.IsValid(index))
        {
            roleInfoButtons[index].abilityIcon.sprite = ability1;
            index++;
        }
        else if (Utils.Skippable(abilityname) || Utils.Skippable(abilityname + "_1"))
            index++;

        var abilityname2 = $"{name}_Ability_2";
        var ability2 = GetSprite(reg, abilityname2, faction);

        if (!ability2.IsValid() && reg)
            ability2 = GetSprite(abilityname2, ogfaction);

        if (ability2.IsValid() && roleInfoButtons.IsValid(index) && !(role == Utils.GetWar() && !isGuide))
        {
            roleInfoButtons[index].abilityIcon.sprite = ability2;
            index++;
        }
        else if (Utils.Skippable(abilityname2))
            index++;

        var attribute = GetSprite(reg, $"Attributes_{name}", faction);

        if (!attribute.IsValid() && role.IsTransformedApoc())
            attribute = GetSprite(reg, "Attributes_Horsemen", faction);

        if (!attribute.IsValid())
            attribute = GetSprite(reg, "Attributes", faction);

        if (reg && !attribute.IsValid())
        {
            attribute = GetSprite($"Attributes_{name}", ogfaction);

            if (!attribute.IsValid() && role.IsTransformedApoc())
                attribute = GetSprite("Attributes_Horsemen", ogfaction);

            if (!attribute.IsValid())
                attribute = GetSprite("Attributes", ogfaction);
        }

        if (attribute.IsValid() && roleInfoButtons.IsValid(index))
            roleInfoButtons[index].abilityIcon.sprite = attribute;

        if (ogfaction != "Coven")
            return;

        index++;
        var nommy = GetSprite(reg, $"Necronomicon_{name}", faction);

        if (!nommy.IsValid())
            nommy = GetSprite(reg, "Necronomicon", faction);

        if (reg && !nommy.IsValid())
        {
            nommy = GetSprite($"Necronomicon_{name}", ogfaction);

            if (!nommy.IsValid())
                nommy = GetSprite("Necronomicon", ogfaction);
        }

        if (nommy.IsValid() && (Constants.IsNecroActive() || isGuide) && roleInfoButtons.IsValid(index))
            roleInfoButtons[index].abilityIcon.sprite = nommy;
    }
}

[HarmonyPatch(typeof(TosAbilityPanelListItem), nameof(TosAbilityPanelListItem.OverrideIconAndText))]
public static class PatchAbilityPanel
{
    public static void Postfix(TosAbilityPanelListItem __instance, AbilityType overrideType)
    {
        if (!Constants.EnableIcons() || overrideType == AbilityType.VOTING)
            return;

        var role = Pepper.GetMyRole();
        var faction = Utils.FactionName(Pepper.GetMyFaction());
        role = Constants.IsTransformed() ? Utils.GetTransformedVersion(role) : role;
        var ogfaction = Utils.FactionName(role.GetFactionType(), false);
        var name = Utils.RoleName(role);
        var reg = ogfaction != faction;

        switch (overrideType)
        {
            case AbilityType.NECRO_ATTACK:
            {
                var nommy = GetSprite("Necronomicon", Constants.PlayerPanelEasterEggs());

                if (nommy.IsValid() && __instance.choice1Sprite && role != Role.ILLUSIONIST)
                    __instance.choice1Sprite.sprite = nommy;

                switch (role)
                {
                    case Role.ILLUSIONIST when __instance.choice2Sprite:
                    {
                        __instance.choice2Sprite.sprite = nommy;
                        var illu = GetSprite(reg, "Illusionist_Ability", faction, Constants.PlayerPanelEasterEggs());

                        if (!illu.IsValid() && reg)
                            illu = GetSprite("Illusionist_Ability", ogfaction, Constants.PlayerPanelEasterEggs());

                        if (illu.IsValid() && __instance.choice1Sprite)
                            __instance.choice1Sprite.sprite = illu;

                        break;
                    }
                    case Role.WITCH:
                    {
                        var target = GetSprite(reg, "Witch_Ability_2", faction, Constants.PlayerPanelEasterEggs());

                        if (!target.IsValid() && reg)
                            target = GetSprite("Witch_Ability_2", ogfaction, Constants.PlayerPanelEasterEggs());

                        if (target.IsValid() && __instance.choice2Sprite)
                            __instance.choice2Sprite.sprite = target;

                        break;
                    }
                }

                break;
            }
            case AbilityType.POISONER_POISON or AbilityType.SHROUD or AbilityType.INVESTIGATOR or AbilityType.PIRATE:
            {
                var special = GetSprite(reg, $"{name}_Special", faction, Constants.PlayerPanelEasterEggs());

                if (!special.IsValid() && reg)
                    special = GetSprite($"{name}_Special", ogfaction, Constants.PlayerPanelEasterEggs());

                if (special.IsValid() && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = special;

                break;
            }
            case AbilityType.POTIONMASTER_ATTACK:
            {
                var special = GetSprite(reg, $"{name}_Ability_3", faction, Constants.PlayerPanelEasterEggs());

                if (!special.IsValid() && reg)
                    special = GetSprite($"{name}_Ability_3", ogfaction, Constants.PlayerPanelEasterEggs());

                if (special.IsValid() && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = special;

                break;
            }
            case AbilityType.POTIONMASTER_HEAL:
            {
                var ab1 = GetSprite(reg, $"{name}_Ability_1", faction, Constants.PlayerPanelEasterEggs());

                if (!ab1.IsValid() && reg)
                    ab1 = GetSprite($"{name}_Ability_1", ogfaction, Constants.PlayerPanelEasterEggs());

                if (ab1.IsValid() && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = ab1;

                break;
            }
            case AbilityType.POTIONMASTER_REVEAL or AbilityType.WEREWOLF_NON_FULL_MOON:
            {
                var ab2 = GetSprite(reg, $"{name}_Ability_2", faction, Constants.PlayerPanelEasterEggs());

                if (!ab2.IsValid() && reg)
                    ab2 = GetSprite($"{name}_Ability_2", ogfaction, Constants.PlayerPanelEasterEggs());

                if (ab2.IsValid() && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = ab2;

                break;
            }
            default:
            {
                var abilityName = $"{name}_Ability";
                var ability1 = GetSprite(reg, abilityName, faction, Constants.PlayerPanelEasterEggs());

                if (!ability1.IsValid())
                    ability1 = GetSprite(reg, abilityName + "_1", faction, Constants.PlayerPanelEasterEggs());

                if (!ability1.IsValid() && reg)
                    ability1 = GetSprite(abilityName, ogfaction, Constants.PlayerPanelEasterEggs());

                if (!ability1.IsValid() && reg)
                    ability1 = GetSprite(abilityName + "_1", ogfaction, Constants.PlayerPanelEasterEggs());

                if (ability1.IsValid() && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = ability1;

                var ability2 = GetSprite(reg, abilityName + "_2", faction, Constants.PlayerPanelEasterEggs());

                if (!ability2.IsValid() && reg)
                    ability2 = GetSprite(abilityName + "_2", ogfaction, Constants.PlayerPanelEasterEggs());

                if (ability2.IsValid() && __instance.choice2Sprite)
                    __instance.choice2Sprite.sprite = ability2;

                break;
            }
        }
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupGenericListItem), nameof(SpecialAbilityPopupGenericListItem.SetData))]
public static class SpecialAbilityPanelPatch
{
    public static void Postfix(SpecialAbilityPopupGenericListItem __instance)
    {
        if (!Constants.EnableIcons())
            return;

        var role = Pepper.GetMyRole();
        var name = $"{Utils.RoleName(role)}_Special";
        var faction = Utils.FactionName(Pepper.GetMyFaction());
        var ogfaction = Utils.FactionName(role.GetFactionType(), false);
        var reg = faction != ogfaction;
        var special = GetSprite(reg, name, faction);

        if (!special.IsValid() && reg)
            special = GetSprite(name, ogfaction);

        if (special.IsValid() && __instance.choiceSprite)
            __instance.choiceSprite.sprite = special;
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupRadialIcon), nameof(SpecialAbilityPopupRadialIcon.SetData))]
public static class PatchRitualistGuessMenu
{
    public static void Postfix(SpecialAbilityPopupRadialIcon __instance, Role a_role)
    {
        if (!Constants.EnableIcons())
            return;

        var icon = GetSprite(Utils.RoleName(a_role), Utils.FactionName(a_role.GetFactionType()), false);

        if (__instance.roleIcon && icon.IsValid())
            __instance.roleIcon.sprite = icon;
    }
}

[HarmonyPatch(typeof(HomeInterfaceService), nameof(HomeInterfaceService.Init))]
public static class CacheDefaults
{
    public static bool ServiceExists { get; private set; }

    public static TMP_SpriteAsset RoleIcons { get; private set; }
    public static TMP_SpriteAsset Numbers { get; private set; }
    public static TMP_SpriteAsset Emojis { get; private set; }

    private static readonly List<string> Assets = [ "Cast", "LobbyIcons", "MiscIcons", "PlayerNumbers", "RoleIcons", "SalemTmpIcons", "TrialReportIcons", "Emojis" ];

    public static bool Prefix(HomeInterfaceService __instance)
    {
        Assets.ForEach(key =>
        {
            Debug.Log($"HomeInterfaceService:: Add Sprite Asset {key}");
            var asset = __instance.LoadResource<TMP_SpriteAsset>($"TmpSpriteAssets/{key}.asset");

            switch (key)
            {
                case "RoleIcons":
                {
                    RoleIcons = asset;
                    break;
                }
                case "PlayerNumbers":
                {
                    Numbers = asset;
                    break;
                }
                case "Emojis":
                {
                    Emojis = asset;
                    break;
                }
            }

            if (key is "RoleIcons" or "PlayerNumbers" or "Emojis")
                Utils.DumpSprite(asset.spriteSheet as Texture2D, key, Path.Combine(IPPath, "Vanilla"), true);
            else
                MaterialReferenceManager.AddSpriteAsset(asset);
        });

        SetScrollSprites();
        ServiceExists = true;
        __instance.isReady_ = true;
        return false;
    }
}

[HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.ShowAttackAndDefense))]
public static class PatchAttackDefense
{
    public static void Postfix(RoleCardPanel __instance, RoleCardData data)
    {
        if (!Constants.EnableIcons())
            return;

        var attack = GetSprite($"Attack{Utils.GetLevel(data.attack, true)}");
        var icon1 = __instance.transform.Find("AttackIcon").Find("Icon").GetComponent<Image>();

        if (icon1)
            icon1.sprite = attack.IsValid() ? attack : FancyAssetManager.Attack;

        var eth = __instance.myData.IsEthereal();
        var defense = GetSprite($"Defense{Utils.GetLevel(eth ? 4 : data.defense, false)}");
        var icon2 = __instance.transform.Find("DefenseIcon").Find("Icon").GetComponent<Image>();

        if (icon2)
            icon2.sprite = defense.IsValid() ? defense : (eth ? Ethereal : FancyAssetManager.Defense);
    }
}

[HarmonyPatch(typeof(RoleCardPopupPanel), nameof(RoleCardPopupPanel.ShowAttackAndDefense))]
public static class PatchAttackDefensePopup
{
    public static void Postfix(RoleCardPopupPanel __instance, RoleCardData data)
    {
        if (!Constants.EnableIcons())
            return;

        var attack = GetSprite($"Attack{Utils.GetLevel(data.attack, true)}");
        var icon1 = __instance.transform.Find("AttackIcon").Find("Icon").GetComponent<Image>();

        if (icon1)
            icon1.sprite = attack.IsValid() ? attack : FancyAssetManager.Attack;

        var eth = __instance.myData.IsEthereal();
        var defense = GetSprite($"Defense{Utils.GetLevel(eth ? 4 : data.defense, false)}");
        var icon2 = __instance.transform.Find("DefenseIcon").Find("Icon").GetComponent<Image>();

        if (icon2)
            icon2.sprite = defense.IsValid() ? defense : (eth ? Ethereal : FancyAssetManager.Defense);
    }
}

[HarmonyPatch(typeof(PlayerPopupController), nameof(PlayerPopupController.SetRoleIcon))]
public static class PlayerPopupControllerPatch
{
    public static void Postfix(PlayerPopupController __instance)
    {
        if (!Constants.EnableIcons() || !Service.Game.Sim.simulation.killRecords.Data.TryFinding(k => k.playerId == __instance.m_discussionPlayerState.position, out var killRecord))
            return;

        var ogfaction = __instance.m_role.GetFactionType();
        var faction = killRecord!.playerFaction;
        var reg = ogfaction != faction;
        var name = Utils.RoleName(__instance.m_role);
        var sprite = GetSprite(reg, name, Utils.FactionName(faction));

        if (reg && !sprite.IsValid())
            sprite = GetSprite(name, Utils.FactionName(ogfaction, false));

        if (sprite.IsValid() && __instance.RoleIcon)
            __instance.RoleIcon.sprite = sprite;
    }
}

[HarmonyPatch(typeof(PlayerPopupController), nameof(PlayerPopupController.InitializeRolePanel))]
public static class InitialiseRolePanel
{
    public static void Postfix(PlayerPopupController __instance)
    {
        if (!Constants.EnableIcons() || !Pepper.IsGamePhasePlay() || !Service.Game.Sim.simulation.killRecords.Data.Any(k => k.playerId == __instance.m_discussionPlayerState.position))
            return;

        var ogfaction = __instance.m_role.GetFactionType();

        if (!Utils.GetRoleAndFaction(__instance.m_discussionPlayerState.position, out var tuple))
            tuple = new(__instance.m_role, ogfaction);

        if (__instance.m_discussionPlayerState.position == Pepper.GetMyPosition())
            tuple = new(Pepper.GetMyRole(), Pepper.GetMyFaction());

        var faction = tuple.Item2;
        var reg = ogfaction != faction;
        var name = Utils.RoleName(__instance.m_role);
        var sprite = GetSprite(reg, name, Utils.FactionName(faction));

        if (reg && !sprite.IsValid())
            sprite = GetSprite(name, Utils.FactionName(ogfaction, false));

        if (sprite.IsValid() && __instance.RoleIcon)
            __instance.RoleIcon.sprite = sprite;

        __instance.RoleLabel.SetText(Service.Game.Sim.simulation.GetRoleNameLinkString(tuple.Item1, tuple.Item2));
    }
}

[HarmonyPatch(typeof(RoleMenuPopupController), nameof(RoleMenuPopupController.SetRoleIconAndLabels))]
public static class RoleMenuPopupControllerPatch
{
    public static void Postfix(RoleMenuPopupController __instance)
    {
        if (!Constants.EnableIcons())
            return;

        var sprite = GetSprite(Utils.RoleName(__instance.m_role), Utils.FactionName(__instance.m_role.GetFactionType()));

        if (!sprite.IsValid())
            return;

        if (__instance.RoleIconImage)
            __instance.RoleIconImage.sprite = sprite;

        if (__instance.HeaderRoleIconImage)
            __instance.HeaderRoleIconImage.sprite = sprite;
    }
}

[HarmonyPatch(typeof(PlayerEffectsService), nameof(PlayerEffectsService.GetEffect))]
public static class PlayerEffectsServicePatch
{
    private static readonly Dictionary<EffectType, Sprite> EffectSprites = [];

    public static void Postfix(EffectType effectType, PlayerEffectInfo __result)
    {
        if (!EffectSprites.ContainsKey(effectType))
            EffectSprites[effectType] = __result.sprite;

        if (!Constants.EnableIcons())
        {
            __result.sprite = EffectSprites[effectType];
            return;
        }

        var effect = Utils.EffectName(effectType);
        var sprite = GetSprite(effect + "_Effect", Constants.PlayerPanelEasterEggs());

        if (!sprite.IsValid())
            sprite = GetSprite(effect, Constants.PlayerPanelEasterEggs());

        __result.sprite = sprite.IsValid() ? sprite : EffectSprites[effectType];
    }
}

[HarmonyPatch(typeof(GameSimulation), nameof(GameSimulation.GetRoleIconAndNameInlineString))]
public static class GetRoleIconAndNameInlineStringPatch
{
    public static void Postfix(Role role, FactionType factionType, ref string __result)
    {
        if (Constants.EnableIcons())
        {
            __result = __result.Replace("RoleIcons\"", $"RoleIcons ({(role.GetFactionType() == factionType && Constants.CurrentStyle() == "Regular" ? "Regular" :
                Utils.FactionName(factionType, false))})\"");
        }
    }
}

[HarmonyPatch(typeof(GameSimulation), nameof(GameSimulation.GetTownTraitorRoleIconAndNameInlineString))]
public static class GetTownTraitorRoleIconAndNameInlineStringPatch
{
    public static void Postfix(ref string __result)
    {
        if (Constants.EnableIcons())
            __result = __result.Replace("RoleIcons\"", "RoleIcons (Coven)\"");
    }
}

[HarmonyPatch(typeof(GameSimulation), nameof(GameSimulation.GetVIPRoleIconAndNameInlineString))]
public static class GetVipRoleIconAndNameInlineStringPatch
{
    public static void Postfix(ref string __result)
    {
        if (Constants.EnableIcons())
            __result = __result.Replace("RoleIcons\"", "RoleIcons (VIP)\"") + " <sprite=\"RoleIcons\" name=\"Role201\">";
    }
}

[HarmonyPatch(typeof(TosCharacterNametag), nameof(TosCharacterNametag.ColouredName))]
public static class TosCharacterNametagPatch
{
    public static void Postfix(FactionType factionType, ref string __result)
    {
        if (Constants.EnableIcons())
            __result = __result.Replace("RoleIcons\"", $"RoleIcons ({Utils.FactionName(factionType, false)})\"");
    }
}

[HarmonyPatch(typeof(BaseDecoder))]
public static class FixDecodingAndEncoding
{
    [HarmonyPatch(nameof(BaseDecoder.Decode))]
    [HarmonyPatch(nameof(BaseDecoder.Encode))]
    public static void Postfix(ChatLogMessage chatLogMessage, ref string __result)
    {
        if (Constants.EnableIcons() && chatLogMessage.chatLogEntry is ChatLogChatMessageEntry entry)
        {
            var faction = "Regular";

            if (Utils.GetRoleAndFaction(entry.speakerId, out var tuple))
                faction = Utils.FactionName(tuple.Item2, false);

            var myFact = Pepper.GetMyFaction();

            if (myFact == tuple.Item2 && Constants.CurrentStyle() == "Regular")
                faction = "Regular";
            else if (entry.speakerId == Pepper.GetMyPosition())
                faction = Utils.FactionName(myFact, false);

            __result = __result.Replace("RoleIcons\"", $"RoleIcons ({faction})\"");
        }
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupPotionMaster), nameof(SpecialAbilityPopupPotionMaster.Start)), HarmonyPriority(Priority.Low)]
public static class PmBakerMenuPatch
{
    public static void Postfix(SpecialAbilityPopupPotionMaster __instance)
    {
        if (!Constants.EnableIcons())
            return;

        var role = Pepper.GetMyRole();
        var name = Utils.RoleName(role);
        var faction = Utils.FactionName(Pepper.GetMyFaction());
        var ogfaction = Utils.FactionName(role.GetFactionType(), false);
        var reg = ogfaction != faction;

        var sprite1 = GetSprite(reg, $"{name}_Ability_1", faction);
        var sprite2 = GetSprite(reg, $"{name}_Ability_2", faction);
        var sprite3 = GetSprite(reg, $"{name}_Ability_3", faction);

        if (!sprite1.IsValid() && reg)
            sprite1 = GetSprite($"{name}_Ability_1", ogfaction);

        if (!sprite2.IsValid() && reg)
            sprite2 = GetSprite($"{name}_Ability_2", ogfaction);

        if (!sprite3.IsValid() && reg)
            sprite3 = GetSprite($"{name}_Ability_3", ogfaction);

        var image1 = __instance.transform.Find("Background/ShieldPotion").GetComponentInChildren<Image>();
        var image2 = __instance.transform.Find("Background/RevealPotion").GetComponentInChildren<Image>();
        var image3 = __instance.transform.Find("Background/KillPotion").GetComponentInChildren<Image>();

        if (sprite1.IsValid() && image1)
            image1.sprite = sprite1;

        if (sprite2.IsValid() && image2)
            image2.sprite = sprite2;

        if (sprite3.IsValid() && image3)
            image3.sprite = sprite3;
    }
}

[HarmonyPatch(typeof(DownloadContributorTags), nameof(DownloadContributorTags.AddTMPSprites)), HarmonyPriority(Priority.VeryLow)]
public static class ReplaceTMPSpritesPatch
{
    public static void Postfix()
    {
        var oldSpriteAssetRequest = Traverse.Create<TMP_Text>().Field<Func<int, string, TMP_SpriteAsset>>("OnSpriteAssetRequest").Value;
        TMP_Text.OnSpriteAssetRequest += (index, str) =>
        {
            try
            {
                if (Request(str, out var result))
                    return result;
            }
            catch (Exception e)
            {
                RunDiagnostics(e);
            }

            if (str.Contains("BTOSRoleIcons"))
                return BTOS22 ?? BTOS21;

            if (str.Contains("RoleIcons"))
                return Vanilla1 ?? CacheDefaults.RoleIcons;

            return str switch
            {
                "PlayerNumbers" => Vanilla2 ?? CacheDefaults.Numbers,
                "Emojis" => Vanilla3 ?? CacheDefaults.Emojis,
                _ => oldSpriteAssetRequest(index, str)
            };
        };
    }

    private static bool Request(string str, out TMP_SpriteAsset asset)
    {
        asset = null;

        if (!Constants.EnableIcons())
            return false;

        var packName = Constants.CurrentPack();

        if (IconPacks.TryGetValue(packName, out var pack))
        {
            if (str.Contains("RoleIcons"))
            {
                var mod = ModType.Vanilla;

                if (str.Contains("BTOS") || Constants.IsBTOS2() || Utils.FindCasualQueue())
                    mod = ModType.BTOS2;

                var deconstructed = Constants.CurrentStyle();
                var defaultSprite = mod switch
                {
                    ModType.BTOS2 => BTOS22 ?? BTOS21,
                    _ => Vanilla1 ?? CacheDefaults.RoleIcons
                };

                if (str.Contains("("))
                    deconstructed = str.Split('(', ')').Select(x => x.Trim()).Last(x => !StringUtils.IsNullEmptyOrWhiteSpace(x));

                if (deconstructed is "None" or "Blank" || StringUtils.IsNullEmptyOrWhiteSpace(deconstructed))
                    deconstructed = "Regular";

                if (!pack.Assets.TryGetValue(mod, out var assets))
                    Fancy.Instance.Warning($"Unable to find {packName} assets for {mod}");
                else if (deconstructed == "Vanilla")
                    asset = defaultSprite;
                else
                {
                    if (!assets.MentionStyles.TryGetValue(deconstructed, out asset) || !asset)
                        Fancy.Instance.Warning($"{packName} {mod} Mention Style {deconstructed} was null or missing");

                    if (!asset && deconstructed != "Regular")
                    {
                        if (!assets.MentionStyles.TryGetValue("Regular", out asset) || !asset)
                            Fancy.Instance.Warning($"{packName} {mod} Mention Style Regular was null or missing");
                    }

                    if (!asset && deconstructed != "Factionless")
                    {
                        if (!assets.MentionStyles.TryGetValue("Factionless", out asset) || !asset)
                            Fancy.Instance.Warning($"{packName} {mod} Mention Style Factionless was null or missing");
                    }
                }

                asset ??= defaultSprite;
            }
            else if (str == "PlayerNumbers")
            {
                if (!pack.PlayerNumbers && Constants.CustomNumbers())
                    Fancy.Instance.Warning($"{packName} PlayerNumber was null");

                asset = pack.PlayerNumbers ?? Vanilla2 ?? CacheDefaults.Numbers;
                return Constants.CustomNumbers() && asset;
            }
            else if (str == "Emojis")
            {
                if (!pack.Emojis)
                    Fancy.Instance.Warning($"{packName} Emoji was null");

                asset = pack.Emojis ?? Vanilla3 ?? CacheDefaults.Emojis;
                return asset;
            }

            return (str.Contains("RoleIcons") || str is "PlayerNumbers" or "Emojis") && asset;
        }

        Fancy.Instance.Warning($"{packName} doesn't have an icon pack");
        return false;
    }
}

[HarmonyPatch(typeof(GameModifierPopupController), nameof(GameModifierPopupController.Show)), HarmonyPriority(Priority.VeryLow)]
public static class ChangeGameModifierPopup
{
    public static void Postfix(GameModifierPopupController __instance)
    {
        if (!Constants.EnableIcons())
            return;

        var sprite = GetSprite(Utils.RoleName(__instance.CurrentRole), Utils.FactionName(__instance.CurrentRole.GetFactionType()));

        if (sprite.IsValid() && __instance.roleIcon)
            __instance.roleIcon.sprite = sprite;
    }
}

[HarmonyPatch(typeof(NecroPassingVoteEntry))]
public static class NecroPassPatches
{
    [HarmonyPatch(nameof(NecroPassingVoteEntry.RefreshData)), HarmonyPostfix]
    public static void RefreshDataPatch(NecroPassingVoteEntry __instance)
    {
        if (!Constants.EnableIcons())
            return;

        var nommy = GetSprite("Necronomicon");

        if (__instance.BookIcon && nommy.IsValid())
            __instance.BookIcon.sprite = nommy;

        if (!Utils.GetRoleAndFaction(__instance.Position, out var tuple))
            return;

        if (Pepper.GetMyPosition() == __instance.Position)
            tuple = new(Pepper.GetMyRole(), Pepper.GetMyFaction());

        var ogfaction = tuple.Item1.GetFactionType();
        var reg = ogfaction != tuple.Item2;
        var role = Utils.RoleName(tuple.Item1);
        var sprite = GetSprite(reg, role, Utils.FactionName(tuple.Item2));

        if (!sprite.IsValid() && reg)
            sprite = GetSprite(role, Utils.FactionName(ogfaction));

        if (sprite.IsValid() && __instance.RoleIcon)
            __instance.RoleIcon.sprite = sprite;
    }

    [HarmonyPatch(nameof(NecroPassingVoteEntry.UpdateVoteState)), HarmonyPostfix]
    public static void UpdateVoteStatePatch(NecroPassingVoteEntry __instance)
    {
        if (!Constants.EnableIcons())
            return;

        for (var i = 0; i < __instance.m_votes.Count; i++)
        {
            var id = __instance.m_votes[i];

            if (!Utils.GetRoleAndFaction(id, out var tuple))
                continue;

            if (Pepper.GetMyPosition() == id)
                tuple = new(Pepper.GetMyRole(), Pepper.GetMyFaction());

            var bust = __instance.m_roleBusts[i];
            var ogfaction = tuple.Item1.GetFactionType();
            var reg = ogfaction != tuple.Item2;
            var role = Utils.RoleName(tuple.Item1);
            var sprite = GetSprite(reg, role, Utils.FactionName(tuple.Item2));

            if (!sprite.IsValid() && reg)
                sprite = GetSprite(role, Utils.FactionName(ogfaction));

            if (sprite.IsValid() && bust)
                bust.sprite = sprite;
        }
    }
}

[HarmonyPatch(typeof(MentionsProvider), nameof(MentionsProvider.ProcessSpeakerName)), HarmonyPriority(Priority.VeryLow)]
public static class FixSpeakerIcons
{
    public static void Postfix(int position, ref string __result)
    {
        if (!Constants.EnableIcons())
            return;

        if (Utils.GetRoleAndFaction(position, out var tuple))
            __result = __result.Replace("RoleIcons\"", $"RoleIcons ({Utils.FactionName(tuple.Item2)})\"");
        else if (position is 69 or 70 or 71)
            __result = __result.Replace("RoleIcons\"", "RoleIcons (Regular)\"");
    }
}

[HarmonyPatch(typeof(MentionsProvider), nameof(MentionsProvider.ProcessAdvancedRoleMention)), HarmonyPriority(Priority.Low)]
public static class OverwriteDecodedText
{
    public static bool Prefix(MentionsProvider __instance, Match roleMatch, ref string encodedText, ref string mention, ref string __result)
    {
        if (!Constants.EnableIcons())
            return true;

        if (!int.TryParse(roleMatch.Groups["R"].Value, out var result))
        {
            __result = encodedText;
            return false;
        }

        if (!int.TryParse(roleMatch.Groups["F"].Value, out var result2))
        {
            __result = encodedText;
            return false;
        }

        var role = (Role)result;
        var factionType = (FactionType)result2;
        var text = __instance._useColors ? role.ToColorizedDisplayString(factionType) : role.ToFactionString(factionType);
        var text2 = __instance._roleEffects ? $"<sprite=\"RoleIcons ({Utils.FactionName(factionType)})\" name=\"Role{(int)role}\">" : "";
        var text3 = $"{__instance.styleTagOpen}{__instance.styleTagFont}<link=\"r{(int)role},{(int)factionType}\">{text2}<b>{text}</b></link>{__instance.styleTagClose}";
        var item = new MentionInfo()
        {
            encodedText = mention,
            richText = text3,
            mentionInfoType = MentionInfo.MentionInfoType.ROLE,
            hashCode = text3.ToLower().GetHashCode()
        };

        if (!__instance._textualMentionInfos.Contains(item))
            __instance._textualMentionInfos.Add(item);

        if (!__instance.MentionInfos.Contains(item))
            __instance.MentionInfos.Add(item);

        if (Constants.IsBTOS2())
            encodedText = encodedText.Replace("\"RoleIcons", "\"BTOSRoleIcons");

        __result = encodedText.Replace(mention, text3);
        return false;
    }
}

// This whole class is a mess but DO NOT TOUCH
[HarmonyPatch(typeof(HeaderAnnouncements), nameof(HeaderAnnouncements.ShowHeaderMessage))]
public static class MakeProperFactionChecksInHeaderAnnouncement
{
    public static bool Prefix(HeaderAnnouncements __instance, TrialData trialData, ref IEnumerator __result)
    {
        if (!Constants.EnableIcons())
            return true;

        __result = trialData.trialPhase is TrialPhase.EXECUTION_REVEAL or TrialPhase.HANGMAN_EXECUTION_REVEAL
            ? FixMessage(__instance, trialData)
            : ShowHeaderMessageOriginal(__instance, trialData);
        return false;
    }

    private static IEnumerator FixMessage(HeaderAnnouncements __instance, TrialData trialData)
    {
        __instance.Clear();

        if (!Service.Game.Sim.simulation.killRecords.Data.TryFinding(k => k.playerId == trialData.defendantPosition, out var killRecord) || killRecord == null ||
            killRecord.killedByReasons.Count < 1)
        {
            yield break;
        }

        var text = __instance.l10n("GUI_GAME_WHO_DIED_AND_HOW_1_2")
            .Replace("%role%", $"<sprite=\"RoleIcons ({Utils.FactionName(killRecord.playerFaction)})\" name=\"Role{(int)killRecord.playerRole}\">" +
                killRecord.playerRole.ToColorizedDisplayString(killRecord.playerFaction))
            .Replace("%name%", Service.Game.Sim.simulation.GetDisplayName(trialData.defendantPosition).ToWhiteNameString());

        if (Constants.IsBTOS2())
            text = text.Replace("\"RoleIcons", "\"BTOSRoleIcons");

        __instance.AddLine(text);
    }

    [HarmonyReversePatch]
    private static IEnumerator ShowHeaderMessageOriginal(HeaderAnnouncements instance, TrialData trialData) => throw new NotImplementedException();
}

[HarmonyPatch(typeof(WhoDiedAndHowPanel), nameof(WhoDiedAndHowPanel.HandleSubphaseRole))]
public static class MakeProperFactionChecksInWdah1
{
    public static bool Prefix(WhoDiedAndHowPanel __instance)
    {
        if (!Constants.EnableIcons())
            return true;

        Debug.Log("WhoDiedAndHowPanel:: HandleSubphaseRole");
        __instance.deathNotePanel.canvasGroup.DisableRenderingAndInteraction();

        if (!Service.Game.Sim.simulation.killRecords.Data.TryFinding(k => k.playerId == __instance.currentPlayerNumber, out var killRecord) || killRecord!.killedByReasons.Count < 1)
            return false;

        var text = __instance.l10n(killRecord.playerRole switch
        {
            Role.STONED => "GUI_GAME_WHO_DIED_VICTIM_ROLE_STONED",
            Role.HIDDEN => "GUI_GAME_WHO_DIED_VICTIM_ROLE_HIDDEN",
            Role.NONE or Role.UNKNOWN => "GUI_GAME_WHO_DIED_VICTIM_ROLE_UNKNOWN",
            _ => "GUI_GAME_WHO_DIED_VICTIM_ROLE_KNOWN"
        })
        .Replace("%role%", $"<sprite=\"RoleIcons ({Utils.FactionName(killRecord.playerFaction)})\" name=\"Role{(int)killRecord.playerRole}\">" +
            killRecord.playerRole.ToColorizedDisplayString(killRecord.playerFaction));

        if (Constants.IsBTOS2())
            text = text.Replace("\"RoleIcons", "\"BTOSRoleIcons");

        __instance.AddLine(text, 1f);
        return false;
    }
}

[HarmonyPatch(typeof(WhoDiedAndHowPanel), nameof(WhoDiedAndHowPanel.HandleSubphaseWhoDied))]
public static class MakeProperFactionChecksInWdah2
{
    public static bool Prefix(WhoDiedAndHowPanel __instance, float phaseTime)
    {
        if (!Constants.EnableIcons())
            return true;

        Debug.Log("HandleSubphaseWhoDied phaseTime = " + phaseTime);

        if (__instance.tombstonePanel != null)
            __instance.tombstonePanel.Clear();
        else
            Debug.LogWarning("WhoDiedAndHowPanel.HandleSubphaseWhoDiedAndHow: tombstonePanel was null.");

        if (__instance.lines != null)
            __instance.lines.Clear();
        else
            Debug.LogWarning("WhoDiedAndHowPanel.HandleSubphaseWhoDiedAndHow: lines list was null.");

        var playerName = Service.Game.Cast.GetPlayerName(__instance.currentPlayerNumber);
        var text = __instance.l10n("GUI_GAME_WHO_DIED_AND_HOW_START").Replace("%name%", playerName);

        if (Service.Game.Sim.simulation.killRecords.Data.TryFinding(k => k.playerId == __instance.currentPlayerNumber, out var killRecord) && killRecord!.killedByReasons.First()
            .IsDaytimeKillReason())
        {
            text = __instance.l10n("GUI_GAME_WHO_DIED_AND_HOW_DAYKILL_START").Replace("%name%", playerName);
        }

        __instance.AddLine(text, Tuning.REVEAL_TIME_PER_ADDL_KILLED_BY_REASON);

        if (killRecord == null || killRecord.killedByReasons.Count < 1)
            __instance.AddLine(__instance.l10n("GUI_GAME_KILLED_BY_REASON_0"), Tuning.REVEAL_TIME_PER_ADDL_KILLED_BY_REASON);
        else
        {
            for (var i = 0; i < killRecord.killedByReasons.Count; i++)
            {
                var killedByReason = killRecord.killedByReasons[i];
                var text2 = __instance.l10n($"GUI_GAME{(i == 0 ? "" : "_ALSO")}_KILLED_BY_REASON_{(int)killedByReason}").Replace("RoleIcons\"", "RoleIcons (Regular)\"");

                if (Constants.IsBTOS2())
                    text2 = text2.Replace("\"RoleIcons", "\"BTOSRoleIcons").Replace("106\"", "109\"");

                __instance.AddLine(text2, Tuning.REVEAL_TIME_PER_ADDL_KILLED_BY_REASON);
            }
        }

        Debug.Log("WhoDiedAndHowPanel:: HandleSubphaseWhoDied");
        return false;
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupOracleListItem), nameof(SpecialAbilityPopupOracleListItem.SetData))]
public static class PatchOracleMenu
{
    public static void Postfix(SpecialAbilityPopupOracleListItem __instance, Role a_role)
    {
        if (!Constants.EnableIcons())
            return;

        var sprite = GetSprite(Utils.RoleName(a_role), Utils.FactionName(a_role.GetFactionType()));

        if (sprite.IsValid() && __instance.roleIcon)
            __instance.roleIcon.sprite = sprite;
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupNecromancerRetributionist), nameof(SpecialAbilityPopupNecromancerRetributionist.Start))]
public static class PatchNecroRetMenu
{
    public static void Postfix(SpecialAbilityPopupNecromancerRetributionist __instance)
    {
        if (!Constants.EnableIcons())
            return;

        var sprite = GetSprite(Utils.RoleName(Pepper.GetMyRole()), Utils.FactionName(Pepper.GetMyFaction()));

        if (sprite.IsValid() && __instance.roleIcon)
            __instance.roleIcon.sprite = sprite;
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupNecromancerRetributionistListItem), nameof(SpecialAbilityPopupNecromancerRetributionistListItem.SetData))]
public static class PatchNecroRetMenuItem
{
    public static void Postfix(SpecialAbilityPopupNecromancerRetributionistListItem __instance, int position)
    {
        if (!Constants.EnableIcons() || !Utils.GetRoleAndFaction(position, out var tuple))
            return;

        var role = Utils.RoleName(tuple.Item1);
        var faction = Utils.FactionName(tuple.Item2);
        var sprite = GetSprite(role, faction);

        if (sprite.IsValid() && __instance.choiceSprite)
            __instance.choiceSprite.sprite = sprite;

        sprite = GetSprite($"{role}_{(role is "Deputy" or "Conjurer" ? "Special" : "Ability")}", faction, false);

        if (!sprite.IsValid())
            sprite = GetSprite($"{role}_Ability_1", faction, false);

        if (sprite.IsValid() && __instance.choice2Sprite)
            __instance.choice2Sprite.sprite = sprite;
    }
}

[HarmonyPatch(typeof(PirateProgressLevelElement), nameof(PirateProgressLevelElement.SetData), typeof(Role), typeof(Role), typeof(Role), typeof(bool), typeof(bool), typeof(bool))]
public static class PatchPirateMenu
{
    public static void Postfix(PirateProgressLevelElement __instance, Role role1, Role role2, Role role3)
    {
        if (!Constants.EnableIcons())
            return;

        var role = Utils.RoleName(role1);
        var faction = Utils.FactionName(role1.GetFactionType());
        var sprite = GetSprite(role, faction);

        if (sprite.IsValid() && __instance.Role1)
            __instance.Role1.sprite = sprite;

        role = Utils.RoleName(role2);
        faction = Utils.FactionName(role2.GetFactionType());
        sprite = GetSprite(role, faction);

        if (sprite.IsValid() && __instance.Role2)
            __instance.Role2.sprite = sprite;

        role = Utils.RoleName(role3);
        faction = Utils.FactionName(role3.GetFactionType());
        sprite = GetSprite(role, faction);

        if (sprite.IsValid() && __instance.Role3)
            __instance.Role3.sprite = sprite;
    }
}

[HarmonyPatch(typeof(RoleRevealCinematicPlayer), nameof(RoleRevealCinematicPlayer.SetRole))]
public static class RoleRevealCinematicPlayerPatch
{
    public static bool Prefix(RoleRevealCinematicPlayer __instance, ref Role role)
    {
        if (!Constants.IconsInRoleReveal() || role == Role.NONE)
            return true;

        var newValue = $"<sprite=\"Cast\" name=\"Skin{__instance.roleRevealCinematic.skinId}\">{Service.Game.Cast.GetSkinName(__instance.roleRevealCinematic.skinId)}";
        var text = __instance.l10n("CINE_ROLE_REVEAL_SKIN").Replace("%skin%", newValue);
        __instance.skinTextPlayer.ShowText(text);

        __instance.totalDuration = Tuning.ROLE_REVEAL_TIME;
        __instance.silhouetteWrapper.gameObject.SetActive(true);
        __instance.silhouetteWrapper.SwapWithSilhouette((int)role, true);

        var newValue2 = role.GetTMPSprite() + role.ToColorizedDisplayString();
        var text2 = __instance.l10n("CINE_ROLE_REVEAL_ROLE").Replace("%role%", newValue2);
        __instance.roleTextPlayer.ShowText(text2);

        if (Pepper.GetCurrentGameType() == GameType.Ranked)
            __instance.playableDirector.Resume();

        return false;
    }
}