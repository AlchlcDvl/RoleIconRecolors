using Home.Services;
using SalemModLoader;
using Game.Services;
using Game.Simulation;
using Game.Characters;
using Game.Chat.Decoders;
using Server.Shared.Messages;
using Server.Shared.State.Chat;
using Home.LoginScene;
using Home.HomeScene;
using Mentions;
using Server.Shared.Extensions;
using Home.Shared;

namespace FancyUI.Patches;

[HarmonyPatch(typeof(RoleCardListItem), nameof(RoleCardListItem.SetData))]
public static class PatchRoleDeckBuilder
{
    public static void Postfix(RoleCardListItem __instance, ref Role role)
    {
        if (!Constants.EnableIcons())
            return;

        var icon = GetSprite(Utils.RoleName(role), Utils.FactionName(role.GetFactionType()), false);

        if (__instance.roleImage && icon.IsValid())
            __instance.roleImage.sprite = icon;

        var banned = __instance.bannedImageGO.GetComponent<Image>();
        var sprite = Fancy.Assets.GetSprite("Banned");

        if (sprite.IsValid() && banned)
            banned.sprite = sprite;
    }
}

[HarmonyPatch(typeof(RoleDeckListItem), nameof(RoleDeckListItem.SetData))]
public static class PatchRoleListPanel
{
    public static void Postfix(RoleDeckListItem __instance, ref Role a_role, ref bool a_isBan)
    {
        if (!Constants.EnableIcons())
            return;

        var icon = a_isBan ? Fancy.Assets.GetSprite("Banned") : GetSprite(Utils.RoleName(a_role), Utils.FactionName(a_role.GetFactionType()), false);

        if (__instance.roleImage && icon.IsValid())
            __instance.roleImage.sprite = icon;
    }
}

[HarmonyPatch(typeof(GameBrowserRoleDeckListItem), nameof(GameBrowserRoleDeckListItem.SetData))]
public static class PatchBrowserRoleListPanel
{
    public static void Postfix(GameBrowserRoleDeckListItem __instance, ref Role a_role, ref bool a_isBan)
    {
        if (!Constants.EnableIcons())
            return;

        var icon = a_isBan ? Fancy.Assets.GetSprite("Banned") : GetSprite(Utils.RoleName(a_role), Utils.FactionName(a_role.GetFactionType()), false);

        if (__instance.roleImage && icon.IsValid())
            __instance.roleImage.sprite = icon;
    }
}

[HarmonyPatch]
public static class PatchRoleCards
{
    [HarmonyPatch(typeof(RoleCardPanelBackground), nameof(RoleCardPanelBackground.SetRole))]
    public static void Postfix(RoleCardPanelBackground __instance, ref Role role)
    {
        if (Constants.EnableIcons())
        {
            var panel = __instance.GetComponentInParent<RoleCardPanel>();
            ChangeRoleCard(panel?.roleIcon, panel?.specialAbilityPanel?.useButton?.abilityIcon, panel?.roleInfoButtons, role, __instance.currentFaction);
        }
    }

    [HarmonyPatch(typeof(RoleCardPanelBackground), nameof(RoleCardPanelBackground.SetFaction))]
    public static void Postfix(RoleCardPanelBackground __instance, ref FactionType factionType)
    {
        if (Constants.EnableIcons())
        {
            var panel = __instance.GetComponentInParent<RoleCardPanel>();
            ChangeRoleCard(panel?.roleIcon, panel?.specialAbilityPanel?.useButton?.abilityIcon, panel?.roleInfoButtons, __instance.currentRole, factionType);
        }
    }

    [HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.HandleOnMyIdentityChanged))]
    public static void Postfix(RoleCardPanel __instance, ref PlayerIdentityData playerIdentityData)
    {
        if (Constants.EnableIcons())
            ChangeRoleCard(__instance?.roleIcon, __instance?.specialAbilityPanel?.useButton?.abilityIcon, __instance?.roleInfoButtons, playerIdentityData.role, playerIdentityData.faction);
    }

    [HarmonyPatch(typeof(RoleCardPopupPanel), nameof(RoleCardPopupPanel.SetRoleAndFaction))]
    public static void Postfix(RoleCardPopupPanel __instance, ref Role role, ref FactionType faction)
    {
        if (Constants.EnableIcons())
            ChangeRoleCard(__instance?.roleIcon, __instance?.specialAbilityPanel?.useButton?.abilityIcon, __instance?.roleInfoButtons, role, faction, true);
    }

    private static void ChangeRoleCard(Image roleIcon, Image specialAbilityPanel, List<BaseAbilityButton> roleInfoButtons, Role role, FactionType factionType, bool isGuide = false)
    {
        roleInfoButtons ??= [];
        role = Constants.IsTransformed() ? Utils.GetTransformedVersion(role) : role;

        // this determines if the role in question is changed by dum's mod
        var isModifiedByTos1UI = Utils.ModifiedByToS1UI(role) && ModStates.IsLoaded("dum.oldui");
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
        else if (isModifiedByTos1UI)
        {
            if (special.IsValid() && roleInfoButtons.IsValid(index))
            {
                roleInfoButtons[index].abilityIcon.sprite = special;
                index++;
            }
            else if (Utils.Skippable(specialName))
                index++;

            isModifiedByTos1UI = false;
        }

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
        else if (isModifiedByTos1UI)
        {
            if (special.IsValid() && roleInfoButtons.IsValid(index))
            {
                roleInfoButtons[index].abilityIcon.sprite = special;
                index++;
            }
            else if (Utils.Skippable(specialName))
                index++;

            isModifiedByTos1UI = false;
        }

        var attributename = "Attributes_";
        var attribute = GetSprite(reg, attributename + name, faction);

        if (!attribute.IsValid())
            attribute = GetSprite(reg, attributename + (role.IsTransformedApoc() ? "Horsemen" : faction), faction);

        if (!attribute.IsValid() && reg)
            attribute = GetSprite(attributename + name, ogfaction);

        if (!attribute.IsValid() && reg)
            attribute = GetSprite(attributename + (role.IsTransformedApoc() ? "Horsemen" : faction), ogfaction);

        if (attribute.IsValid() && roleInfoButtons.IsValid(index))
        {
            roleInfoButtons[index].abilityIcon.sprite = attribute;
            index++;
        }
        else if (Utils.Skippable(attributename))
            index++;

        if (ogfaction != "Coven")
            return;

        var nommy = GetSprite("Necronomicon");

        if (nommy.IsValid() && (Constants.IsNecroActive() || isGuide) && roleInfoButtons.IsValid(index))
            roleInfoButtons[index].abilityIcon.sprite = nommy;
    }
}

[HarmonyPatch(typeof(TosAbilityPanelListItem), nameof(TosAbilityPanelListItem.OverrideIconAndText))]
public static class PatchAbilityPanel
{
    public static void Postfix(TosAbilityPanelListItem __instance, ref TosAbilityPanelListItem.OverrideAbilityType overrideType)
    {
        if (!Constants.EnableIcons() || overrideType == TosAbilityPanelListItem.OverrideAbilityType.VOTING)
            return;

        var role = Pepper.GetMyRole();
        var faction = Utils.FactionName(Pepper.GetMyFaction());
        role = Constants.IsTransformed() ? Utils.GetTransformedVersion(role) : role;
        var ogfaction = Utils.FactionName(role.GetFactionType(), false);
        var name = Utils.RoleName(role);
        var reg = ogfaction != faction;

        switch (overrideType)
        {
            case TosAbilityPanelListItem.OverrideAbilityType.NECRO_ATTACK:
                var nommy = GetSprite("Necronomicon", Constants.PlayerPanelEasterEggs());

                if (nommy.IsValid() && __instance.choice1Sprite && role != Role.ILLUSIONIST)
                    __instance.choice1Sprite.sprite = nommy;

                if (role == Role.ILLUSIONIST && __instance.choice2Sprite)
                {
                    __instance.choice2Sprite.sprite = nommy;
                    var illu = GetSprite(reg, "Illusionist_Ability", faction, Constants.PlayerPanelEasterEggs());

                    if (!illu.IsValid() && reg)
                        illu = GetSprite("Illusionist_Ability", ogfaction, Constants.PlayerPanelEasterEggs());

                    if (illu.IsValid() && __instance.choice1Sprite)
                        __instance.choice1Sprite.sprite = illu;
                }
                else if (role == Role.WITCH)
                {
                    var target = GetSprite(reg, "Witch_Ability_2", faction, Constants.PlayerPanelEasterEggs());

                    if (!target.IsValid() && reg)
                        target = GetSprite("Witch_Ability_2", ogfaction, Constants.PlayerPanelEasterEggs());

                    if (target.IsValid() && __instance.choice2Sprite)
                        __instance.choice2Sprite.sprite = target;
                }

                break;

            case TosAbilityPanelListItem.OverrideAbilityType.POTIONMASTER_ATTACK or TosAbilityPanelListItem.OverrideAbilityType.POISONER_POISON or
                TosAbilityPanelListItem.OverrideAbilityType.SHROUD or TosAbilityPanelListItem.OverrideAbilityType.INVESTIGATOR:
                var special = GetSprite(reg, $"{name}_Special", faction, Constants.PlayerPanelEasterEggs());

                if (!special.IsValid() && reg)
                    special = GetSprite($"{name}_Special", ogfaction, Constants.PlayerPanelEasterEggs());

                if (special.IsValid() && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = special;

                break;

            case TosAbilityPanelListItem.OverrideAbilityType.POTIONMASTER_HEAL:
                var ab1 = GetSprite(reg, $"{name}_Ability_1", faction, Constants.PlayerPanelEasterEggs());

                if (!ab1.IsValid() && reg)
                    ab1 = GetSprite($"{name}_Ability_1", ogfaction, Constants.PlayerPanelEasterEggs());

                if (ab1.IsValid() && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = ab1;

                break;

            case TosAbilityPanelListItem.OverrideAbilityType.POTIONMASTER_REVEAL or TosAbilityPanelListItem.OverrideAbilityType.WEREWOLF_NON_FULL_MOON:
                var ab2 = GetSprite(reg, $"{name}_Ability_2", faction, Constants.PlayerPanelEasterEggs());

                if (!ab2.IsValid() && reg)
                    ab2 = GetSprite($"{name}_Ability_2", ogfaction, Constants.PlayerPanelEasterEggs());

                if (ab2.IsValid() && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = ab2;

                break;

            default:
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

                var ability2 = GetSprite(reg, $"{name}_Ability_2", faction, Constants.PlayerPanelEasterEggs());

                if (!ability1.IsValid() && reg)
                    ability1 = GetSprite($"{name}_Ability_2", ogfaction, Constants.PlayerPanelEasterEggs());

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
    public static void Postfix(SpecialAbilityPopupRadialIcon __instance, ref Role a_role)
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

    private static readonly List<string> Assets = [ "Cast", "LobbyIcons", "MiscIcons", "PlayerNumbers", "RoleIcons", "SalemTmpIcons", "TrialReportIcons" ];

    public static bool Prefix(HomeInterfaceService __instance)
    {
        Assets.ForEach(key =>
        {
            Debug.Log($"HomeInterfaceService:: Add Sprite Asset {key}");
            var asset = __instance.LoadResource<TMP_SpriteAsset>($"TmpSpriteAssets/{key}.asset");

            if (key == "RoleIcons")
                RoleIcons = asset;
            else if (key == "PlayerNumbers")
                Numbers = asset;

            if (key is "RoleIcons" or "PlayerNumbers")
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
    public static void Postfix(RoleCardPanel __instance, ref RoleCardData data)
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
    public static void Postfix(RoleCardPopupPanel __instance, ref RoleCardData data)
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
        if (!Constants.EnableIcons())
            return;

        var killRecord = Service.Game.Sim.simulation.killRecords.Data.Find(k => k.playerId == __instance.m_discussionPlayerState.position);

        if (killRecord == null)
            return;

        var ogfaction = __instance.m_role.GetFactionType();
        var faction = killRecord.playerFaction;
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
        if (!Constants.EnableIcons() || !Pepper.IsGamePhasePlay())
            return;

        var killRecord = Service.Game.Sim.simulation.killRecords.Data.Find(k => k.playerId == __instance.m_discussionPlayerState.position);

        if (killRecord != null)
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

        if (sprite.IsValid())
        {
            if (__instance.RoleIconImage)
                __instance.RoleIconImage.sprite = sprite;

            if (__instance.HeaderRoleIconImage)
                __instance.HeaderRoleIconImage.sprite = sprite;
        }
    }
}

[HarmonyPatch(typeof(PlayerEffectsService), nameof(PlayerEffectsService.GetEffect))]
public static class PlayerEffectsServicePatch
{
    private static readonly Dictionary<EffectType, Sprite> EffectSprites = [];

    public static void Postfix(ref EffectType effectType, ref PlayerEffectInfo __result)
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
    public static void Postfix(ref Role role, ref FactionType factionType, ref string __result)
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
public static class GetVIPRoleIconAndNameInlineStringPatch
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
    public static void Postfix(ref FactionType factionType, ref string __result)
    {
        if (Constants.EnableIcons())
            __result = __result.Replace("RoleIcons\"", $"RoleIcons ({Utils.FactionName(factionType, false)})\"");
    }
}

[HarmonyPatch(typeof(BaseDecoder), nameof(BaseDecoder.Decode))]
[HarmonyPatch(typeof(BaseDecoder), nameof(BaseDecoder.Encode))]
public static class FixDecodingAndEncoding
{
    public static void Postfix(ref ChatLogMessage chatLogMessage, ref string __result)
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
public static class PMBakerMenuPatch
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
        var sprite3 = GetSprite(reg, $"{name}_Special", faction);

        if (!sprite1.IsValid() && reg)
            sprite1 = GetSprite($"{name}_Ability_1", ogfaction);

        if (!sprite2.IsValid() && reg)
            sprite2 = GetSprite($"{name}_Ability_2", ogfaction);

        if (!sprite3.IsValid() && reg)
            sprite3 = GetSprite($"{name}_Special", ogfaction);

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

[HarmonyPatch(typeof(LoginSceneController), nameof(LoginSceneController.Start))]
public static class HandlePacks
{
    public static void Prefix() => IconPacksUI.HandlePackData();
}

[HarmonyPatch(typeof(HomeSceneController), nameof(HomeSceneController.Start))]
public static class CacheHomeSceneController
{
    public static HomeSceneController Controller { get; private set; }

    public static void Prefix(HomeSceneController __instance) => Controller = __instance;
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
                return BTOS2_2 ?? BTOS2_1;
            else if (str.Contains("RoleIcons"))
                return Vanilla1 ?? CacheDefaults.RoleIcons;
            else if (str == "PlayerNumbers")
                return Vanilla2 ?? CacheDefaults.Numbers;
            else
                return oldSpriteAssetRequest(index, str);
        };
    }

    private static bool Request(string str, out TMP_SpriteAsset asset)
    {
        asset = null;

        if (!Constants.EnableIcons())
            return false;

        if (IconPacks.TryGetValue(Constants.CurrentPack(), out var pack))
        {
            if (str.Contains("RoleIcons"))
            {
                var mod = ModType.Vanilla;

                if (str.Contains("BTOS"))
                    mod = ModType.BTOS2;

                var deconstructed = Constants.CurrentStyle();

                if (str.Contains("(") && !str.Contains("Blank"))
                    deconstructed = str.Replace("RoleIcons (", "").Replace(")", "").Replace("BTOS", "").Replace(" name=", "");

                if (!pack.Assets.TryGetValue(mod, out var assets))
                    Fancy.Instance.Warning($"Unable to find {Constants.CurrentPack()} assets for {mod}");
                else
                {
                    if (!assets.MentionStyles.TryGetValue(deconstructed, out asset) || !asset)
                        Fancy.Instance.Warning($"{Constants.CurrentPack()} {mod} Mention Style {deconstructed} was null or missing");

                    if (!asset && deconstructed != "Regular")
                    {
                        if (!assets.MentionStyles.TryGetValue("Regular", out asset) || !asset)
                            Fancy.Instance.Warning($"{Constants.CurrentPack()} {mod} Mention Style Regular was null or missing");
                    }

                    if (!asset && deconstructed != "Factionless")
                    {
                        if (!assets.MentionStyles.TryGetValue("Factionless", out asset) || !asset)
                            Fancy.Instance.Warning($"{Constants.CurrentPack()} {mod} Mention Style Factionless was null or missing");
                    }
                }

                asset ??= mod switch
                {
                    ModType.BTOS2 => BTOS2_2 ?? BTOS2_1,
                    _ => Vanilla1 ?? CacheDefaults.RoleIcons
                };
            }
            else if (str == "PlayerNumbers")
            {
                if (!pack.PlayerNumbers && Constants.CustomNumbers())
                    Fancy.Instance.Warning($"{Constants.CurrentPack()} PlayerNumber was null");

                asset = pack.PlayerNumbers ?? Vanilla2 ?? CacheDefaults.Numbers;
                return Constants.CustomNumbers() && asset;
            }

            return (str.Contains("RoleIcons") || str == "PlayerNumbers") && asset;
        }
        else
        {
            Fancy.Instance.Warning($"{Constants.CurrentPack()} doesn't have an icon pack");
            return false;
        }
    }
}

[HarmonyPatch(typeof(GameModifierPopupController), nameof(GameModifierPopupController.Show)), HarmonyPriority(Priority.VeryLow)]
public static class ChangeGameModifierPopup
{
    public static void Postfix(GameModifierPopupController __instance)
    {
        var sprite = GetSprite(Utils.RoleName(__instance.CurrentRole));

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
        if (Utils.GetRoleAndFaction(position, out var tuple))
            __result = __result.Replace("RoleIcons\"", $"RoleIcons ({Utils.FactionName(tuple.Item2)})\"");
        else if (position is 69 or 70 or 71)
            __result = __result.Replace("RoleIcons\"", "RoleIcons (Regular)\"");
    }
}

[HarmonyPatch(typeof(MentionsProvider), nameof(MentionsProvider.ProcessEncodedText), typeof(string), typeof(List<string>))]
public static class OverwriteDecodedText
{
    public static bool Prefix(MentionsProvider __instance, ref string encodedText, ref List<string> mentions, ref string __result)
    {
        foreach (var mention in mentions)
        {
            var mentionInfo = __instance.MentionInfos.FirstOrDefault(m => m.encodedText == mention);

            if (mentionInfo != null)
                encodedText = encodedText.Replace(mention, mentionInfo.richText);
            else
            {
                var match = MentionsProvider.RoleRegex.Match(mention);

                if (match.Success)
                {
                    var role = (Role)int.Parse(match.Groups["R"].Value);

                    if (!role.IsModifierCard() && role != Role.NONE)
                    {
                        var factionType = (FactionType)int.Parse(match.Groups["F"].Value);
                        var text = __instance._useColors ? role.ToColorizedDisplayString(factionType) :
                            ((role.GetFaction() == factionType) ? role.ToDisplayString() :
                            (role.ToDisplayString() + " (" + factionType.ToDisplayString() + ")"));
                        var text2 = __instance._roleEffects ? $"<sprite=\"RoleIcons ({Utils.FactionName(factionType)})\" name=\"Role{(int)role}\">" : string.Empty;
                        var text3 = $"{__instance.styleTagOpen}{__instance.styleTagFont}<link=\"r{(int)role},{(int)factionType}\">{text2}<b>{text}</b></link>{__instance.styleTagClose}";
                        encodedText = encodedText.Replace(mention, text3);
                    }
                }
            }
        }

        if (Constants.IsBTOS2())
            encodedText = encodedText.Replace("RoleIcons", "BTOSRoleIcons");

        __result = encodedText;
        return false;
    }
}