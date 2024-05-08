using Cinematics.Players;
using Home.Services;
using SalemModLoader;
using Game.Services;
using Game.Simulation;
using Game.Characters;
using Game.Chat.Decoders;
using Server.Shared.Messages;
using Server.Shared.State.Chat;
using Home.Shared;
using Home.LoginScene;
using Home.HomeScene;

namespace IconPacks;

[HarmonyPatch(typeof(RoleCardListItem), nameof(RoleCardListItem.SetData))]
public static class PatchRoleDeckBuilder
{
    public static void Postfix(RoleCardListItem __instance, ref Role role)
    {
        if (!Constants.EnableIcons)
            return;

        var icon = AssetManager.GetSprite(Utils.RoleName(role), Utils.FactionName(role.GetFactionType()), false);

        if (__instance.roleImage && icon.IsValid())
            __instance.roleImage.sprite = icon;

        var banned = __instance.bannedImageGO.GetComponent<Image>();
        var sprite = AssetManager.GetSprite("Banned", false);

        if (sprite.IsValid() && banned)
            banned.sprite = sprite;
    }
}

[HarmonyPatch(typeof(RoleDeckListItem), nameof(RoleDeckListItem.SetData))]
public static class PatchRoleListPanel
{
    public static void Postfix(RoleDeckListItem __instance, ref Role a_role, ref bool a_isBan)
    {
        if (!Constants.EnableIcons)
            return;

        var icon = a_isBan ? AssetManager.GetSprite("Banned", false) : AssetManager.GetSprite(Utils.RoleName(a_role), Utils.FactionName(a_role.GetFactionType()), false);

        if (__instance.roleImage && icon.IsValid())
            __instance.roleImage.sprite = icon;
    }
}

[HarmonyPatch(typeof(GameBrowserRoleDeckListItem), nameof(GameBrowserRoleDeckListItem.SetData))]
public static class PatchBrowserRoleListPanel
{
    public static void Postfix(GameBrowserRoleDeckListItem __instance, ref Role a_role, ref bool a_isBan)
    {
        if (!Constants.EnableIcons)
            return;

        var icon = a_isBan ? AssetManager.GetSprite("Banned", false) : AssetManager.GetSprite(Utils.RoleName(a_role), Utils.FactionName(a_role.GetFactionType()), false);

        if (__instance.roleImage && icon.IsValid())
            __instance.roleImage.sprite = icon;
    }
}

[HarmonyPatch]
public static class PatchRoleCards
{
    [HarmonyPatch(typeof(RoleCardPanelBackground), nameof(RoleCardPanelBackground.SetRole))]
    public static class Patch1
    {
        public static void Postfix(RoleCardPanelBackground __instance, ref Role role)
        {
            if (Constants.EnableIcons)
            {
                var panel = __instance.GetComponentInParent<RoleCardPanel>();
                ChangeRoleCard(panel?.roleIcon, panel?.specialAbilityPanel?.useButton?.abilityIcon, panel?.roleInfoButtons, role, Pepper.GetMyFaction());
            }
        }
    }

    [HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.HandleOnMyIdentityChanged))]
    public static class Patch2
    {
        public static void Postfix(RoleCardPanel __instance, ref PlayerIdentityData playerIdentityData)
        {
            if (Constants.EnableIcons)
            {
                ChangeRoleCard(__instance?.roleIcon, __instance?.specialAbilityPanel?.useButton?.abilityIcon, __instance?.roleInfoButtons, playerIdentityData.role,
                    playerIdentityData.faction);
            }
        }
    }

    [HarmonyPatch(typeof(RoleCardPopupPanel), nameof(RoleCardPopupPanel.SetRole))]
    public static class Patch3
    {
        public static void Postfix(RoleCardPopupPanel __instance, ref Role role)
        {
            if (Constants.EnableIcons)
                ChangeRoleCard(__instance?.roleIcon, __instance?.specialAbilityPanel?.useButton?.abilityIcon, __instance?.roleInfoButtons, role, role.GetFactionType(), true);
        }
    }

    private static void ChangeRoleCard(Image roleIcon, Image specialAbilityPanel, List<BaseAbilityButton> roleInfoButtons, Role role, FactionType factionType, bool isGuide = false)
    {
        roleInfoButtons ??= [];
        role = Constants.IsTransformed ? Utils.GetTransformedVersion(role) : role;

        //this determines if the role in question is changed by dum's mod
        var isModifiedByTos1UI = Utils.ModifiedByToS1UI(role) && ModStates.IsLoaded("dum.oldui");
        var index = 0;
        var name = Utils.RoleName(role);
        var faction = Utils.FactionName(factionType);
        var ogfaction = Utils.FactionName(role.GetFactionType());
        var reg = ogfaction != faction;
        var sprite = AssetManager.GetSprite(reg, name, faction);

        if (!sprite.IsValid() && reg)
            sprite = AssetManager.GetSprite(name, ogfaction);

        if (sprite.IsValid() && roleIcon)
            roleIcon.sprite = sprite;

        var specialName = $"{name}_Special";
        var special = AssetManager.GetSprite(reg, specialName, faction);

        if (!special.IsValid() && reg)
            special = AssetManager.GetSprite(specialName, ogfaction);

        if (special.IsValid() && specialAbilityPanel)
            specialAbilityPanel.sprite = special;

        var abilityname = $"{name}_Ability";
        var ability1 = AssetManager.GetSprite(reg, abilityname, faction);

        if (!ability1.IsValid())
            ability1 = AssetManager.GetSprite(reg, abilityname + "_1", faction);

        if (!ability1.IsValid() && reg)
            ability1 = AssetManager.GetSprite(abilityname, ogfaction);

        if (!ability1.IsValid() && reg)
            ability1 = AssetManager.GetSprite(abilityname + "_1", ogfaction);

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
        var ability2 = AssetManager.GetSprite(reg, abilityname2, faction);

        if (!ability2.IsValid() && reg)
            ability2 = AssetManager.GetSprite(abilityname2, ogfaction);

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
        var attribute = AssetManager.GetSprite(reg, attributename + name, faction);

        if (!attribute.IsValid())
            attribute = AssetManager.GetSprite(reg, attributename + (role.IsTransformedApoc() ? "Horsemen" : faction), faction);

        if (!attribute.IsValid() && reg)
            attribute = AssetManager.GetSprite(attributename + name, ogfaction);

        if (!attribute.IsValid() && reg)
            attribute = AssetManager.GetSprite(attributename + (role.IsTransformedApoc() ? "Horsemen" : faction), ogfaction);

        if (attribute.IsValid() && roleInfoButtons.IsValid(index))
        {
            roleInfoButtons[index].abilityIcon.sprite = attribute;
            index++;
        }
        else if (Utils.Skippable(attributename))
            index++;

        if (ogfaction != "Coven")
            return;

        var nommy = AssetManager.GetSprite("Necronomicon");

        if (nommy.IsValid() && (Constants.IsNecroActive || isGuide) && roleInfoButtons.IsValid(index))
            roleInfoButtons[index].abilityIcon.sprite = nommy;
    }
}

[HarmonyPatch(typeof(TosAbilityPanelListItem), nameof(TosAbilityPanelListItem.OverrideIconAndText))]
public static class PatchAbilityPanel
{
    public static void Postfix(TosAbilityPanelListItem __instance, ref TosAbilityPanelListItem.OverrideAbilityType overrideType)
    {
        if (!Constants.EnableIcons || overrideType == TosAbilityPanelListItem.OverrideAbilityType.VOTING)
            return;

        var role = Pepper.GetMyRole();
        var faction = Utils.FactionName(Pepper.GetMyFaction());
        role = Constants.IsTransformed ? Utils.GetTransformedVersion(role) : role;
        var ogfaction = Utils.FactionName(role.GetFactionType());
        var name = Utils.RoleName(role);
        var reg = ogfaction != faction;

        switch (overrideType)
        {
            case TosAbilityPanelListItem.OverrideAbilityType.NECRO_ATTACK:
                var nommy = AssetManager.GetSprite("Necronomicon", Constants.PlayerPanelEasterEggs);

                if (nommy.IsValid() && __instance.choice1Sprite && role != Role.ILLUSIONIST)
                    __instance.choice1Sprite.sprite = nommy;

                if (role == Role.ILLUSIONIST && __instance.choice2Sprite)
                {
                    __instance.choice2Sprite.sprite = nommy;
                    var illu = AssetManager.GetSprite(reg, "Illusionist_Ability", faction, Constants.PlayerPanelEasterEggs);

                    if (!illu.IsValid() && reg)
                        illu = AssetManager.GetSprite("Illusionist_Ability", ogfaction, Constants.PlayerPanelEasterEggs);

                    if (illu.IsValid() && __instance.choice1Sprite)
                        __instance.choice1Sprite.sprite = illu;
                }
                else if (role == Role.WITCH)
                {
                    var target = AssetManager.GetSprite(reg, "Witch_Ability_2", faction, Constants.PlayerPanelEasterEggs);

                    if (!target.IsValid() && reg)
                        target = AssetManager.GetSprite("Witch_Ability_2", ogfaction, Constants.PlayerPanelEasterEggs);

                    if (target.IsValid() && __instance.choice2Sprite)
                        __instance.choice2Sprite.sprite = target;
                }

                break;

            case TosAbilityPanelListItem.OverrideAbilityType.POTIONMASTER_ATTACK:
            case TosAbilityPanelListItem.OverrideAbilityType.POISONER_POISON:
            case TosAbilityPanelListItem.OverrideAbilityType.SHROUD:
            case TosAbilityPanelListItem.OverrideAbilityType.INVESTIGATOR:
                var special = AssetManager.GetSprite(reg, $"{name}_Special", faction, Constants.PlayerPanelEasterEggs);

                if (!special.IsValid() && reg)
                    special = AssetManager.GetSprite($"{name}_Special", ogfaction, Constants.PlayerPanelEasterEggs);

                if (special.IsValid() && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = special;

                break;

            case TosAbilityPanelListItem.OverrideAbilityType.POTIONMASTER_HEAL:
                var ab1 = AssetManager.GetSprite(reg, $"{name}_Ability_1", faction, Constants.PlayerPanelEasterEggs);

                if (!ab1.IsValid() && reg)
                    ab1 = AssetManager.GetSprite($"{name}_Ability_1", ogfaction, Constants.PlayerPanelEasterEggs);

                if (ab1.IsValid() && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = ab1;

                break;

            case TosAbilityPanelListItem.OverrideAbilityType.POTIONMASTER_REVEAL:
            case TosAbilityPanelListItem.OverrideAbilityType.WEREWOLF_NON_FULL_MOON:
                var ab2 = AssetManager.GetSprite(reg, $"{name}_Ability_2", faction, Constants.PlayerPanelEasterEggs);

                if (!ab2.IsValid() && reg)
                    ab2 = AssetManager.GetSprite($"{name}_Ability_2", ogfaction, Constants.PlayerPanelEasterEggs);

                if (ab2.IsValid() && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = ab2;

                break;

            default:
                var abilityName = $"{name}_Ability";
                var ability1 = AssetManager.GetSprite(reg, abilityName, faction, Constants.PlayerPanelEasterEggs);

                if (!ability1.IsValid())
                    ability1 = AssetManager.GetSprite(reg, abilityName + "_1", faction, Constants.PlayerPanelEasterEggs);

                if (!ability1.IsValid() && reg)
                    ability1 = AssetManager.GetSprite(abilityName, ogfaction, Constants.PlayerPanelEasterEggs);

                if (!ability1.IsValid() && reg)
                    ability1 = AssetManager.GetSprite(abilityName + "_1", ogfaction, Constants.PlayerPanelEasterEggs);

                if (ability1.IsValid() && __instance.choice1Sprite)
                    __instance.choice1Sprite.sprite = ability1;

                var ability2 = AssetManager.GetSprite(reg, $"{name}_Ability_2", faction, Constants.PlayerPanelEasterEggs);

                if (!ability1.IsValid() && reg)
                    ability1 = AssetManager.GetSprite($"{name}_Ability_2", ogfaction, Constants.PlayerPanelEasterEggs);

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

        var role = Pepper.GetMyRole();
        var name = $"{Utils.RoleName(role)}_Special";
        var faction = Utils.FactionName(Pepper.GetMyFaction());
        var ogfaction = Utils.FactionName(role.GetFactionType());
        var reg = faction != ogfaction;
        var special = AssetManager.GetSprite(reg, name, faction);

        if (!special.IsValid() && reg)
            special = AssetManager.GetSprite(name, ogfaction);

        if (special.IsValid() && __instance.choiceSprite)
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

        var icon = AssetManager.GetSprite(Utils.RoleName(a_role), Utils.FactionName(a_role.GetFactionType()), false);

        if (__instance.roleIcon && icon.IsValid())
            __instance.roleIcon.sprite = icon;
    }
}

[HarmonyPatch(typeof(DoomsayerLeavesCinematicPlayer), nameof(DoomsayerLeavesCinematicPlayer.Init))]
public static class PatchDoomsayerLeaving
{
    public static void Postfix(DoomsayerLeavesCinematicPlayer __instance)
    {
        if (!Constants.EnableIcons || Constants.IsBTOS2)
            return;

        var role1 = __instance.doomsayerLeavesCinematicData.roles[0];
        var role2 = __instance.doomsayerLeavesCinematicData.roles[1];
        var role3 = __instance.doomsayerLeavesCinematicData.roles[2];

        var sprite1 = AssetManager.GetSprite(Utils.RoleName(role1), Utils.FactionName(role1.GetFactionType()));
        var sprite2 = AssetManager.GetSprite(Utils.RoleName(role2), Utils.FactionName(role2.GetFactionType()));
        var sprite3 = AssetManager.GetSprite(Utils.RoleName(role3), Utils.FactionName(role3.GetFactionType()));

        if (sprite1.IsValid() && __instance.otherCharacterRole1)
            __instance.otherCharacterRole1.sprite = sprite1;

        if (sprite2.IsValid() && __instance.otherCharacterRole2)
            __instance.otherCharacterRole2.sprite = sprite2;

        if (sprite3.IsValid() && __instance.otherCharacterRole3)
            __instance.otherCharacterRole3.sprite = sprite3;
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
                Utils.DumpSprite(asset.spriteSheet as Texture2D, key, Path.Combine(AssetManager.ModPath, "Vanilla"), true);
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

        var attack = AssetManager.GetSprite($"Attack{Utils.GetLevel(data.attack, true)}");
        var icon1 = __instance.transform.Find("AttackIcon").Find("Icon").GetComponent<Image>();

        if (icon1)
            icon1.sprite = attack.IsValid() ? attack : AssetManager.Attack;

        var eth = __instance.myData.IsEthereal();
        var defense = AssetManager.GetSprite($"Defense{Utils.GetLevel(eth ? 4 : data.defense, false)}");
        var icon2 = __instance.transform.Find("DefenseIcon").Find("Icon").GetComponent<Image>();

        if (icon2)
            icon2.sprite = defense.IsValid() ? defense : (eth ? AssetManager.Ethereal : AssetManager.Defense);
    }
}

[HarmonyPatch(typeof(RoleCardPopupPanel), nameof(RoleCardPopupPanel.ShowAttackAndDefense))]
public static class PatchAttackDefensePopup
{
    public static void Postfix(RoleCardPopupPanel __instance, ref RoleCardData data)
    {
        if (!Constants.EnableIcons)
            return;

        var attack = AssetManager.GetSprite($"Attack{Utils.GetLevel(data.attack, true)}");
        var icon1 = __instance.transform.Find("AttackIcon").Find("Icon").GetComponent<Image>();

        if (icon1)
            icon1.sprite = attack.IsValid() ? attack : AssetManager.Attack;

        var eth = __instance.myData.IsEthereal();
        var defense = AssetManager.GetSprite($"Defense{Utils.GetLevel(eth ? 4 : data.defense, false)}");
        var icon2 = __instance.transform.Find("DefenseIcon").Find("Icon").GetComponent<Image>();

        if (icon2)
            icon2.sprite = defense.IsValid() ? defense : (eth ? AssetManager.Ethereal : AssetManager.Defense);
    }
}

[HarmonyPatch(typeof(PlayerPopupController), nameof(PlayerPopupController.SetRoleIcon))]
public static class PlayerPopupControllerPatch
{
    public static void Postfix(PlayerPopupController __instance)
    {
        if (!Constants.EnableIcons)
            return;

        var killRecord = Service.Game.Sim.simulation.killRecords.Data.Find(k => k.playerId == __instance.m_discussionPlayerState.position);

        if (killRecord == null)
            return;

        var ogfaction = __instance.m_role.GetFactionType();
        var faction = killRecord.playerFaction;
        var reg = ogfaction != faction;
        var name = Utils.RoleName(__instance.m_role);
        var sprite = AssetManager.GetSprite(reg, name, Utils.FactionName(faction));

        if (reg && !sprite.IsValid())
            sprite = AssetManager.GetSprite(name, Utils.FactionName(ogfaction));

        if (sprite.IsValid() && __instance.RoleIcon)
            __instance.RoleIcon.sprite = sprite;
    }
}

[HarmonyPatch(typeof(PlayerPopupController), nameof(PlayerPopupController.InitializeRolePanel))]
public static class InitialiseRolePanel
{
    public static void Postfix(PlayerPopupController __instance)
    {
        if (!Constants.EnableIcons || !Pepper.IsGamePhasePlay())
            return;

        var killRecord = Service.Game.Sim.simulation.killRecords.Data.Find(k => k.playerId == __instance.m_discussionPlayerState.position);

        if (killRecord != null)
            return;

        var ogfaction = __instance.m_role.GetFactionType();

        if (!Service.Game.Sim.simulation.knownRolesAndFactions.Data.TryGetValue(__instance.m_discussionPlayerState.position, out var tuple))
            tuple = new(__instance.m_role, ogfaction);

        if (__instance.m_discussionPlayerState.position == Pepper.GetMyPosition())
            tuple = new(Pepper.GetMyRole(), Pepper.GetMyFaction());

        var faction = tuple.Item2;
        var reg = ogfaction != faction;
        var name = Utils.RoleName(__instance.m_role);
        var sprite = AssetManager.GetSprite(reg, name, Utils.FactionName(faction));

        if (reg && !sprite.IsValid())
            sprite = AssetManager.GetSprite(name, Utils.FactionName(ogfaction));

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
        if (!Constants.EnableIcons)
            return;

        var sprite = AssetManager.GetSprite(Utils.RoleName(__instance.m_role), Utils.FactionName(__instance.m_role.GetFactionType()));

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

        if (!Constants.EnableIcons)
        {
            __result.sprite = EffectSprites[effectType];
            return;
        }

        var effect = Utils.EffectName(effectType);
        var sprite = AssetManager.GetSprite(effect + "_Effect", Constants.PlayerPanelEasterEggs);

        if (!sprite.IsValid())
            sprite = AssetManager.GetSprite(effect, Constants.PlayerPanelEasterEggs);

        __result.sprite = sprite.IsValid() ? sprite : EffectSprites[effectType];
    }
}

[HarmonyPatch(typeof(GameSimulation), nameof(GameSimulation.GetRoleIconAndNameInlineString))]
public static class GetRoleIconAndNameInlineStringPatch
{
    public static void Postfix(ref FactionType factionType, ref string __result)
    {
        if (Constants.EnableIcons)
            __result = __result.Replace("RoleIcons\"", $"RoleIcons ({Utils.FactionName(factionType)})\"");
    }
}

[HarmonyPatch(typeof(GameSimulation), nameof(GameSimulation.GetTownTraitorRoleIconAndNameInlineString))]
public static class GetTownTraitorRoleIconAndNameInlineStringPatch
{
    public static void Postfix(ref string __result)
    {
        if (Constants.EnableIcons)
            __result = __result.Replace("RoleIcons\"", "RoleIcons (Coven)\"");
    }
}

[HarmonyPatch(typeof(GameSimulation), nameof(GameSimulation.GetVIPRoleIconAndNameInlineString))]
public static class GetVIPRoleIconAndNameInlineStringPatch
{
    public static void Postfix(ref string __result)
    {
        if (Constants.EnableIcons)
            __result = __result.Replace("RoleIcons\"", "RoleIcons (VIP)\"") + " <sprite=\"RoleIcons (VIP)\" name=\"Role201\">";
    }
}

[HarmonyPatch(typeof(TosCharacterNametag), nameof(TosCharacterNametag.ColouredName))]
public static class TosCharacterNametagPatch
{
    public static void Postfix(TosCharacterNametag __instance, ref FactionType factionType, ref string __result)
    {
        if (!Constants.EnableIcons)
            return;

        if (__instance.tosCharacter.position == Pepper.GetMyPosition())
            factionType = Pepper.GetMyFaction();

        __result = __result.Replace("RoleIcons\"", $"RoleIcons ({Utils.FactionName(factionType)})\"");
    }
}

[HarmonyPatch(typeof(BaseDecoder), nameof(BaseDecoder.Decode))]
[HarmonyPatch(typeof(BaseDecoder), nameof(BaseDecoder.Encode))]
public static class FixDecodingAndEncoding
{
    public static void Postfix(ref ChatLogMessage chatLogMessage, ref string __result)
    {
        if (Constants.EnableIcons && chatLogMessage.chatLogEntry is ChatLogChatMessageEntry entry)
        {
            var faction = "Regular";

            if (Service.Game.Sim.simulation.knownRolesAndFactions.Data.TryGetValue(entry.speakerId, out var tuple))
                faction = Utils.FactionName(tuple.Item2);

            if (entry.speakerId == Pepper.GetMyPosition())
                faction = Utils.FactionName(Pepper.GetMyFaction());

            __result = __result.Replace("RoleIcons\"", $"RoleIcons ({faction})\"");
        }
    }
}

[HarmonyPatch(typeof(SpecialAbilityPopupPotionMaster), nameof(SpecialAbilityPopupPotionMaster.Start))]
[HarmonyPriority(Priority.Low)]
public static class PMBakerMenuPatch
{
    public static void Postfix(SpecialAbilityPopupPotionMaster __instance)
    {
        if (!Constants.EnableIcons)
            return;

        var role = Pepper.GetMyRole();
        var name = Utils.RoleName(role);
        var faction = Utils.FactionName(Pepper.GetMyFaction());
        var ogfaction = Utils.FactionName(role.GetFactionType());
        var reg = ogfaction != faction;

        var sprite1 = AssetManager.GetSprite(reg, $"{name}_Ability_1", faction);
        var sprite2 = AssetManager.GetSprite(reg, $"{name}_Ability_2", faction);
        var sprite3 = AssetManager.GetSprite(reg, $"{name}_Special", faction);

        if (!sprite1.IsValid() && reg)
            sprite1 = AssetManager.GetSprite($"{name}_Ability_1", ogfaction);

        if (!sprite2.IsValid() && reg)
            sprite2 = AssetManager.GetSprite($"{name}_Ability_2", ogfaction);

        if (!sprite3.IsValid() && reg)
            sprite3 = AssetManager.GetSprite($"{name}_Special", ogfaction);

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

[HarmonyPatch(typeof(PlayerPopupController), nameof(PlayerPopupController.SetRoleName))]
[HarmonyPriority(Priority.Low)]
public static class RemoveTextIconFromPlayerPopupBecauseWhyIsItThere
{
    public static void Postfix(PlayerPopupController __instance)
    {
        var killRecord = Service.Game.Sim.simulation.killRecords.Data.Find(k => k.playerId == __instance.m_discussionPlayerState.position);
        var text = __instance.m_role.ToColorizedDisplayString() ?? "";

        if (killRecord != null)
        {
            text = Service.Game.Sim.simulation.GetRoleNameLinkString(killRecord.playerRole, killRecord.playerFaction) ?? "";

            if ((int)__instance.m_hiddenRole is not (241 or 0))
                text = __instance.m_hiddenRole.ToColorizedDisplayString() + " (" + text + ")";
        }

        __instance.RoleLabel.SetText(text);
    }
}

[HarmonyPatch(typeof(LoginSceneController), nameof(LoginSceneController.Start))]
public static class HandlePacks
{
    public static void Prefix() => DownloadController.HandlePackData();
}

[HarmonyPatch(typeof(HomeSceneController), nameof(HomeSceneController.Start))]
public static class CacheHomeSceneController
{
    public static HomeSceneController Controller { get; private set; }

    public static void Prefix(HomeSceneController __instance) => Controller = __instance;
}

[HarmonyPatch(typeof(DownloadContributorTags), nameof(DownloadContributorTags.AddTMPSprites))]
[HarmonyPriority(Priority.VeryLow)]
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
                AssetManager.RunDiagnostics(e);
            }

            if (str.Contains("BTOSRoleIcons"))
                return AssetManager.BTOS2_2 ?? AssetManager.BTOS2_1;
            else if (str.Contains("RoleIcons"))
                return AssetManager.Vanilla1 ?? CacheDefaults.RoleIcons;
            else if (str == "PlayerNumbers")
                return AssetManager.Vanilla2 ?? CacheDefaults.Numbers;
            else
                return oldSpriteAssetRequest(index, str);
        };
    }

    private static bool Request(string str, out TMP_SpriteAsset asset)
    {
        asset = null;

        if (!Constants.EnableIcons)
            return false;

        if (AssetManager.IconPacks.TryGetValue(Constants.CurrentPack, out var pack))
        {
            if (str.Contains("RoleIcons"))
            {
                var mod = ModType.Vanilla;

                if (str.Contains("BTOS"))
                    mod = ModType.BTOS2;

                var deconstructed = Constants.CurrentStyle;

                if (str.Contains("(") && !str.Contains("Blank"))
                    deconstructed = str.Replace("RoleIcons (", "").Replace(")", "").Replace("BTOS", "");

                var defaultSprite = mod switch
                {
                    ModType.BTOS2 => AssetManager.BTOS2_2 ?? AssetManager.BTOS2_1,
                    _ => AssetManager.Vanilla1 ?? CacheDefaults.RoleIcons
                };

                if (!pack.Assets.TryGetValue(mod, out var assets))
                    Logging.LogWarning($"Unable to find {Constants.CurrentPack} assets for {mod}");

                if (!assets.MentionStyles.TryGetValue(deconstructed, out asset))
                    Logging.LogWarning($"{Constants.CurrentPack} {mod} Mention Style {deconstructed} was null or missing");

                if (!asset && deconstructed != "Regular")
                {
                    if (!assets.MentionStyles.TryGetValue("Regular", out asset))
                        Logging.LogWarning($"{Constants.CurrentPack} {mod} Mention Style Regular was null or missing");
                }

                asset ??= defaultSprite;
            }
            else if (str == "PlayerNumbers")
            {
                if (!pack.PlayerNumbers && Constants.CustomNumbers)
                    Logging.LogWarning($"{Constants.CurrentPack} PlayerNumber was null");

                asset = pack.PlayerNumbers ?? AssetManager.Vanilla2 ?? CacheDefaults.Numbers;
                return Constants.CustomNumbers && asset;
            }

            return (str.Contains("RoleIcons") || str == "PlayerNumbers") && asset;
        }
        else
        {
            Logging.LogWarning($"{Constants.CurrentPack} doesn't have an icon pack");
            return false;
        }
    }
}

[HarmonyPatch(typeof(GameModifierPopupController), nameof(GameModifierPopupController.Show))]
[HarmonyPriority(Priority.VeryLow)]
public static class ChangeGameModifierPopup
{
    public static void Postfix(GameModifierPopupController __instance)
    {
        var sprite = AssetManager.GetSprite(Utils.RoleName(__instance.CurrentRole));

        if (sprite.IsValid() && __instance.roleIcon)
            __instance.roleIcon.sprite = sprite;
    }
}