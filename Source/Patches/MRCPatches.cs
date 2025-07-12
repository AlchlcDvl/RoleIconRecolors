using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using Cinematics.Players;
using Game.Characters;
using Game.Chat;
using Game.Chat.Decoders;
using Game.Simulation;
using Home.HomeScene;
using Home.Services;
using Home.Shared;
using Mentions;
using Mentions.Providers;
using Mentions.UI;
using Server.Shared.Cinematics;
using Server.Shared.Cinematics.Data;
using Server.Shared.Extensions;
using Server.Shared.Messages;
using Server.Shared.State;
using Server.Shared.State.Chat;
using Shared.Chat;

namespace FancyUI.Patches;

[HarmonyPatch(typeof(RoleCardPanel))]
public static class PatchRoleCard
{
    [HarmonyPatch(nameof(RoleCardPanel.UpdateTitle)), HarmonyPostfix]
    public static void UpdateTitlePostfix(RoleCardPanel __instance)
    {
        var component = __instance.GetComponent<GradientRoleColorController>();

        if (component != null)
            UObject.Destroy(component);

        __instance.gameObject.AddComponent<GradientRoleColorController>().Instance = __instance.rolecardBG;
        __instance.roleNameText.text = Pepper.GetMyRole().ToChangedDisplayString(Pepper.GetMyFaction(), Service.Game.Sim.simulation.observations.roleCardObservation.Data.modifier);
    }

    [HarmonyPatch(nameof(RoleCardPanel.GetSubAlignment)), HarmonyPrefix]
    public static bool Prefix(RoleCardPanel __instance, ref string __result)
    {
        if (!Fancy.GradientBuckets.Value)
            return true;
        var role = __instance.CurrentRole;
        var faction = __instance.CurrentFaction;
        var alignment = role.GetAlignment();
        var subAlignment = role.GetSubAlignment();
        var factionID = (int)faction;
        bool useNeutralGradient = false;
        var neutralGradient = Utils.CreateGradient(Fancy.NeutralStart.Value, Fancy.NeutralEnd.Value);
        var subAlignGradient = Utils.CreateGradient(Fancy.BucketStart.Value, Fancy.BucketEnd.Value);
        string displayString = "";
        if (role is Role.DEATH or Role.PESTILENCE or Role.WAR or Role.FAMINE)
        {
            __result = string.Empty;
            return false;
        }
        var acolyte = role switch
        {
            Role.BERSERKER => Role.WAR.ToColorizedNoLabel(faction),
            Role.BAKER => Role.FAMINE.ToColorizedNoLabel(faction),
            Role.PLAGUEBEARER => Role.PESTILENCE.ToColorizedNoLabel(faction),
            Role.SOULCOLLECTOR or Btos2Role.Warlock => Role.DEATH.ToColorizedNoLabel(faction),
            _ => string.Empty
        };
        if (!string.IsNullOrEmpty(acolyte))
        {
            __result = $"{acolyte} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_ACOLYTE"), subAlignGradient)}";
            return false;
        }
        if (factionID > 2 && factionID < 12 && factionID != 7 || factionID > 37 && factionID < 42)
        {
            useNeutralGradient = true;
            displayString = RoleAlignment.NEUTRAL.ToDisplayString();
        }
        else if (factionID == 13)
            displayString = Utils.GetString("FANCY_BUCKETS_CURSEDSOUL");
        else
            displayString = faction.ToDisplayString();
        var text = string.Empty;
        if (subAlignment.IsInvalid())
        {
            if (alignment.IsInvalid())
            {
                __result = string.Empty;
                return false;
            }
            __result = alignment.ToColorizedDisplayString();
            return false;
        }
        text = Utils.ApplyGradient(displayString, useNeutralGradient ? neutralGradient : faction.GetChangedGradient(role)) + " " + Utils.ApplyGradient(subAlignment.ToDisplayString(), subAlignGradient);
        __result = text;
        return false;
    }

    private static string ToChangedDisplayString(this Role role, FactionType faction, ROLE_MODIFIER modifier)
    {
        var roleName = Fancy.FactionalRoleNames.Value ? role.ToRoleFactionDisplayString(faction) : role.ToDisplayString();
        var gradientTt = faction.GetChangedGradient(role);
        var text = gradientTt != null ? Utils.ApplyGradient(roleName, gradientTt) : $"<color={faction.GetFactionColor()}>{roleName}</color>";

        switch (modifier)
        {
            case ROLE_MODIFIER.TRAITOR:
            {
                if (gradientTt != null)
                {
                    text += faction switch
                    {
                        FactionType.COVEN => $"\n<size=85%>{Utils.ApplyGradient($"({Fancy.CovenTraitorLabel.Value})", gradientTt)}</size>",
                        FactionType.APOCALYPSE => $"\n<size=85%>{Utils.ApplyGradient($"({Fancy.ApocTraitorLabel.Value})", gradientTt)}</size>",
                        (FactionType)43 => $"\n<size=85%>{Utils.ApplyGradient($"({Fancy.PandoraTraitorLabel.Value})", gradientTt)}</size>",
                        _ => $"\n<size=85%>{Utils.ApplyGradient($"({Fancy.CovenTraitorLabel.Value})", gradientTt)}</size>",
                    };
                }

                break;
            }
            case ROLE_MODIFIER.VIP:
            {
                text += $"\n<size=85%>{Utils.ApplyGradient($"({Fancy.VipLabel.Value})", gradientTt)}</size>";
                break;
            }
            case (ROLE_MODIFIER)10:
            {
                var gradient = Btos2Faction.Jackal.GetChangedGradient(role);
                var mod = Utils.GetGameType();
                var originalFaction = role.GetFactionType(mod);

                var label = originalFaction switch
                {
                    Btos2Faction.Town => Fancy.RecruitLabelTown.Value,
                    Btos2Faction.Coven => !Constants.IsPandora() ? Fancy.RecruitLabelCoven.Value : Fancy.RecruitLabelPandora.Value,
                    Btos2Faction.Apocalypse => !Constants.IsPandora() ? Fancy.RecruitLabelApocalypse.Value : Fancy.RecruitLabelPandora.Value,
                    Btos2Faction.SerialKiller => !Constants.IsCompliance() ? Fancy.RecruitLabelSerialKiller.Value : Fancy.RecruitLabelCompliance.Value,
                    Btos2Faction.Arsonist => !Constants.IsCompliance() ? Fancy.RecruitLabelArsonist.Value : Fancy.RecruitLabelCompliance.Value,
                    Btos2Faction.Werewolf => !Constants.IsCompliance() ? Fancy.RecruitLabelWerewolf.Value : Fancy.RecruitLabelCompliance.Value,
                    Btos2Faction.Shroud => !Constants.IsCompliance() ? Fancy.RecruitLabelShroud.Value : Fancy.RecruitLabelCompliance.Value,
                    Btos2Faction.CursedSoul => Fancy.RecruitLabelCursedSoul.Value,
                    _ => Btos2Role.Jackal.ToColorizedDisplayString()
                };

                text += $"\n<size=85%>{Utils.ApplyGradient($"({label})", gradient)}</size>";
                break;
            }
            default:
            {
                if ((Fancy.RoleCardFactionLabel.Value == FactionLabelOption.Mismatch && role.GetFactionType() != faction) || Fancy.RoleCardFactionLabel.Value == FactionLabelOption.Always ||
                    (Fancy.RoleCardFactionLabel.Value == FactionLabelOption.Conditional && !Utils.ConditionalCompliancePandora(role.GetFactionType(), faction)))
                {
                    var gradient2 = faction.GetChangedGradient(role);

                    if (gradient2 != null)
                        text += $"\n<size=85%>{Utils.ApplyGradient($"({faction.ToDisplayString()})", gradient2)}</size>";
                    else
                        text = $"{text}\n<size=85%><color={faction.GetFactionColor()}>({faction.ToDisplayString()})</color></size>";
                }

                break;
            }
        }

        return text;
    }

    [HarmonyPatch(nameof(RoleCardPanel.OnClickRoleDesc)), HarmonyPostfix]
    public static void OnClickRoleDescPostfix(RoleCardPanel __instance)
    {
        var text = __instance.roleDescText.text;
        var header = Utils.GetString("GUI_ROLECARDHEADER_FACTION");
        var index = text.IndexOf(header, StringComparison.Ordinal);

        if (index < 0)
            return;

        var start = text.IndexOf('\n', index) + 1;
        var end = text.IndexOf('\n', start);

        if (start <= 0 || end <= start)
            return;

        var faction = text[start..end].Trim();

        var factionName = faction.RemoveColorTags();

        var factionType = __instance.CurrentFaction;

        var gradient = factionType.GetChangedGradient(factionType == (FactionType)33 ? Btos2Role.Jackal : Role.NONE);
        var colored = Utils.ApplyGradient(factionName, gradient);

        __instance.roleDescText.text = text[..start] + colored + text[end..];
    }
}

[HarmonyPatch(typeof(RoleCardPopupPanel))]
public static class RoleCardPopupPatches2
{
    [HarmonyPatch(nameof(RoleCardPopupPanel.OnClickRoleDesc))]
    public static void Postfix(RoleCardPopupPanel __instance)
    {
        var text = __instance.roleDescText.text;
        var header = Utils.GetString("GUI_ROLECARDHEADER_FACTION");
        var index = text.IndexOf(header, StringComparison.Ordinal);

        if (index < 0)
            return;

        var start = text.IndexOf('\n', index) + 1;
        var end = text.IndexOf('\n', start);

        if (start <= 0 || end <= start)
            return;

        var faction = text[start..end].Trim();

        var factionName = faction.RemoveColorTags();

        var factionType = __instance.CurrentFaction;

        var gradient = factionType.GetChangedGradient(factionType == (FactionType)33 ? Btos2Role.Jackal : Role.NONE);
        var colored = Utils.ApplyGradient(factionName, gradient);

        __instance.roleDescText.text = text[..start] + colored + text[end..];
    }

    [HarmonyPatch(nameof(RoleCardPopupPanel.GetSubAlignment)), HarmonyPrefix]
    public static bool Prefix(RoleCardPopupPanel __instance, ref string __result)
    {
        if (!Fancy.GradientBuckets.Value) 
            return true;
        var role = __instance.CurrentRole;
        var faction = __instance.CurrentFaction;
        var alignment = role.GetAlignment();
        var subAlignment = role.GetSubAlignment();
        var factionID = (int)faction;
        bool useNeutralGradient = false;
        var neutralGradient = Utils.CreateGradient(Fancy.NeutralStart.Value, Fancy.NeutralEnd.Value);
        var subAlignGradient = Utils.CreateGradient(Fancy.BucketStart.Value, Fancy.BucketEnd.Value);
        string displayString = "";
        if (role is Role.DEATH or Role.PESTILENCE or Role.WAR or Role.FAMINE)
        {
            __result = string.Empty;
            return false;
        }
        var acolyte = role switch
        {
            Role.BERSERKER => Role.WAR.ToColorizedNoLabel(faction),
            Role.BAKER => Role.FAMINE.ToColorizedNoLabel(faction),
            Role.PLAGUEBEARER => Role.PESTILENCE.ToColorizedNoLabel(faction),
            Role.SOULCOLLECTOR or Btos2Role.Warlock => Role.DEATH.ToColorizedNoLabel(faction),
            _ => string.Empty
        };
        if (!string.IsNullOrEmpty(acolyte))
        {
            __result = $"{acolyte} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_ACOLYTE"), subAlignGradient)}";
            return false;
        }
        if (factionID > 2 && factionID < 12 && factionID != 7 || factionID > 37 && factionID < 42)
        {
            useNeutralGradient = true;
            displayString = RoleAlignment.NEUTRAL.ToDisplayString();
        }
        else if (factionID == 13)
            displayString = Utils.GetString("FANCY_BUCKETS_CURSEDSOUL");
        else
            displayString = faction.ToDisplayString();
        var text = string.Empty;
        if (subAlignment.IsInvalid())
        {
            if (alignment.IsInvalid())
            {
                __result = string.Empty;
                return false;
            }
            __result = alignment.ToColorizedDisplayString();
            return false;
        }
        text = Utils.ApplyGradient(displayString, useNeutralGradient ? neutralGradient : faction.GetChangedGradient(role)) + " " + Utils.ApplyGradient(subAlignment.ToDisplayString(), subAlignGradient);
        __result = text;
        return false;
    }


    [HarmonyPatch(nameof(RoleCardPopupPanel.SetRole))]
    public static void Postfix(Role role, RoleCardPopupPanel __instance) => __instance.roleNameText.text = role.ToColorizedNoLabel(__instance.CurrentFaction);

    [HarmonyPatch(nameof(RoleCardPopupPanel.SetRoleAndFaction))]
    public static void Postfix(Role role, FactionType faction, RoleCardPopupPanel __instance) => __instance.roleNameText.text = role.ToColorizedNoLabel(faction);

    [HarmonyPatch(nameof(RoleCardPopupPanel.ShowAttackAndDefense)), HarmonyPrefix]
    public static bool Prefix(RoleCardPopupPanel __instance, RoleCardData data)
    {
        if (Service.Game.Sim.simulation.roleDeckBuilder.Data.modifierCards.Contains(Role.FEELIN_LUCKY) || Constants.IsBTOS2() && Service.Game.Sim.simulation.roleDeckBuilder.Data.modifierCards.Contains(Btos2Role.FeelinLucky))
            return true;
        int defense = -1;
        Role role = __instance.CurrentRole;
        int faction = (int)__instance.CurrentFaction;
        if (faction == 33)
            faction = (int)__instance.CurrentRole.GetFaction();
        if (faction > 0 && faction < 3 || faction > 42)
            defense = 0;
        if (faction > 2 && faction < 34 || faction == 33 && role == Btos2Role.Jackal || (role == Role.CULTIST || Constants.IsBTOS2() && role == Btos2Role.Cultist) && data.specialAbilityRemaining != 0 || faction == 40)
            defense = 1;
        if (Utils.IsHorseman(role) || data.defense == 3)
            defense = 3;
        if (faction > 37 && faction < 43 && faction != 40)
            defense = 4;
        if (faction == 33 && role != Btos2Role.Jackal || faction > 33 && faction < 37)
            defense = data.defense;
        float num = 0f;
        float num2 = 0f;
        if (data.attack == 1)
        {
            num = 0.33f;
        }
        else if (data.attack == 2)
        {
            num = 0.66f;
        }
        else if (data.attack == 3)
        {
            num = 1f;
        }
        if (defense == 1)
        {
            num2 = 0.33f;
        }
        else if (defense == 2 || defense == 4)
        {
            num2 = 0.66f;
        }
        else if (defense == 3)
        {
            num2 = 1f;
        }
        __instance.attackIcon.fillAmount = num;
        __instance.attackGlow.fillAmount = num;
        if (__instance.tabAtkFillImage)
        {
            __instance.tabAtkFillImage.fillAmount = num;
        }
        if (__instance.tabAtkGlowImage)
        {
            __instance.tabAtkGlowImage.fillAmount = num;
        }
        __instance.defenseIcon.fillAmount = num2;
        __instance.defenseGlow.fillAmount = num2;
        if (__instance.tabDefFillImage)
        {
            __instance.tabDefFillImage.fillAmount = num2;
        }
        if (__instance.tabDefGlowImage)
        {
            __instance.tabDefGlowImage.fillAmount = num2;
        }
        return false;
    }
}

[HarmonyPatch(typeof(TosAbilityPanelListItem), nameof(TosAbilityPanelListItem.SetKnownRole))]
public static class PlayerListPatch
{
    public static bool Prefix(Role role, FactionType faction, TosAbilityPanelListItem __instance)
    {
        __instance.playerRole = role;

        if (role is 0 or (Role)byte.MaxValue)
            return false;

        __instance.playerRole = role;
        var roleName = Fancy.FactionalRoleNames.Value ? role.ToRoleFactionDisplayString(faction) : role.ToDisplayString();
        var gradient = faction.GetChangedGradient(role);

        if (role is not (Role.STONED or Role.HIDDEN))
            __instance.playerRoleText.text = gradient != null ? Utils.ApplyGradient($"({roleName})", gradient) : $"<color={faction.GetFactionColor()}>({roleName})</color>";
        else
            __instance.playerRoleText.text = $"<color={role.GetFaction().GetFactionColor()}>({roleName})</color>";

        __instance.playerRoleText.gameObject.SetActive(true);
        __instance.playerRoleText.enableAutoSizing = false; // Remove when PlayerNotes+ fix is out

        return false;
    }
}

[HarmonyPatch(typeof(TosCharacterNametag), nameof(TosCharacterNametag.ColouredName))]
public static class TosCharacterNametagPatch
{
    public static void Postfix(string theName, FactionType factionType, Role role, ref string __result)
    {
        var gradient = factionType.GetChangedGradient(role);

        if (gradient == null || role is Role.STONED or Role.HIDDEN)
            return;

        var roleName = Fancy.FactionalRoleNames.Value ? role.ToRoleFactionDisplayString(factionType) : role.ToDisplayString();
        var gradientName = Utils.ApplyGradient(theName, gradient);
        var gradientRole = Utils.ApplyGradient($"({roleName})", gradient);
        __result = $"<size=36><sprite=\"{(Constants.IsBTOS2() ? "BTOS" : "")}RoleIcons\" name=\"Role{(int)role}\"></size>\n<size=24>{gradientName}</size>\n<size=18>{gradientRole}</size>";

        if (Constants.EnableIcons())
            __result = __result.Replace("RoleIcons\"", $"RoleIcons ({Utils.FactionName(factionType, false)})\"");
    }
}

[HarmonyPatch(typeof(HomeSceneController), nameof(HomeSceneController.HandleClickPlay))]
public static class FixStyles
{
    private static readonly Type StyleType = typeof(TMP_Style);
    private static readonly FieldInfo StylesField = AccessTools.Field(typeof(TMP_StyleSheet), "m_StyleList");
    private static readonly FieldInfo OpeningDefField = AccessTools.Field(StyleType, "m_OpeningDefinition");

    public static void Postfix()
    {
        if (StylesField.GetValue(TMP_Settings.defaultStyleSheet) is not List<TMP_Style> styles)
            return;

        styles.SetStyle("TownColor", Fancy.Colors["TOWN"].Start);
        styles.SetStyle("CovenColor", Fancy.Colors["COVEN"].Start);
        styles.SetStyle("ApocalypseColor", Fancy.Colors["APOCALYPSE"].Start);
        styles.SetStyle("SerialKillerColor", Fancy.Colors["SERIALKILLER"].Start);
        styles.SetStyle("ArsonistColor", Fancy.Colors["ARSONIST"].Start);
        styles.SetStyle("WerewolfColor", Fancy.Colors["WEREWOLF"].Start);
        styles.SetStyle("ShroudColor", Fancy.Colors["SHROUD"].Start);
        styles.SetStyle("ExecutionerColor", Fancy.Colors["EXECUTIONER"].Start);
        styles.SetStyle("JesterColor", Fancy.Colors["JESTER"].Start);
        styles.SetStyle("PirateColor", Fancy.Colors["PIRATE"].Start);
        styles.SetStyle("DoomsayerColor", Fancy.Colors["DOOMSAYER"].Start);
        // styles.SetStyle("VampireColor", Fancy.Colors["VAMPIRE"].Start);
        // styles.SetStyle("CursedSoulColor", Fancy.Colors["CURSEDSOUL"].Start);
        styles.SetStyle("NeutralColor", Fancy.NeutralStart.Value);

        TMP_Settings.defaultStyleSheet.RefreshStyles();
    }

    private static void SetStyle(this List<TMP_Style> styles, string styleName, string colorValue)
    {
        if (styles.TryFinding(s => s.name == styleName, out var style))
            OpeningDefField.SetValue(style, $"<color={colorValue}>");
        else
        {
            style = (TMP_Style)Activator.CreateInstance(StyleType, styleName, $"<color={colorValue}>", "</color>");
            styles.Add(style);
        }
    }
}

[HarmonyPatch(typeof(MentionsProvider))]
public static class FancyChatExperimentalBTOS2
{
    private static readonly List<int> ExcludedIds = [50, 69, 70, 71];

    [HarmonyPatch(nameof(MentionsProvider.DecodeSpeaker))]
    public static bool Prefix(MentionsProvider __instance, ref string __result, string encodedText, int position, bool isAlive)
    {
        var text = Service.Home.UserService.Settings.ChatNameColor switch // Might replace this with a customizable value (it overrides FancyChat)
        {
            1 => "B0B0B0",
            2 => "CC009E",
            _ => "FCCE3B",
        };
        var text2 = encodedText;

        if (!ExcludedIds.Contains(position))
        {
            var isRecruited = Service.Game.Sim.simulation.observations.playerEffects.Any(x => x.Data.effects.Contains((EffectType)100) && x.Data.playerPosition == position);
            // var isDisconnected = Service.Game.Sim.simulation.observations.playerEffects.Any(x => x.Data.effects.Contains(EffectType.DISCONNECTED));
            var gameName = Pepper.GetDiscussionPlayerByPosition(position).gameName;
            Gradient gradient = null;

            if (Utils.GetRoleAndFaction(position, out var playerInfo))
            {
                gradient = playerInfo.Item2.GetChangedGradient(playerInfo.Item1);

                if (gradient == null || isRecruited)
                    gradient = Btos2Faction.Jackal.GetChangedGradient(playerInfo.Item1);

                if (!isAlive && gradient != null && Fancy.DeadChatDesaturation.Value != -1)
                    gradient = Utils.Desaturate(gradient, Constants.DeadChatDesaturation());
            }

            if (gradient != null)
            {
                var text4 = Utils.ApplyGradient($"{gameName}:", gradient);
                var pattern = $"<color=#[0-9A-Fa-f]+>{Regex.Escape(gameName)}:";
                var regex = new Regex(pattern);
                text2 = regex.Replace(text2, text4);
            }
            else
                text2 = text2.Replace($"<color=#{text}>", $"<color=#{ColorUtility.ToHtmlStringRGB(Utils.GetPlayerRoleColor(position))}>");
        }

        text2 = text2.Replace("<color=#8C8C8C>", "<color=" + Fancy.DeadColor.Value + ">");

        __result = __instance.ProcessSpeakerName(text2, position, isAlive);
        return false;
    }

    [HarmonyPatch(nameof(MentionsProvider.ProcessSpeakerName))]
    public static void Postfix(string encodedText, int position, ref string __result)
    {
        if (Constants.IsBTOS2())
            return;

        __result = position switch
        {
            70 => $"<link=\"r57\"><sprite=\"BTOSRoleIcons\" name=\"Role57\"><indent=1.1em><b>{Utils.ApplyGradient(Fancy.CourtLabel.Value, Fancy.Colors["JUDGE"].Start, Fancy.Colors["JUDGE"].End)}:</b> </link>{encodedText.Replace("????: </color>", "").Replace("white", "#FFFF00")}",
            69 => encodedText.Replace("????:", $"<color=#{ColorUtility.ToHtmlStringRGB(Fancy.JuryColor.Value.ToColor())}>{Fancy.JuryLabel.Value}:</color>"),
            // I decided to remove the Seer icon from Jury messages for the scenario of which an Icon Pack's Seer icon does not fit Jury. An example is replacing Seer with TOS1 Medium.
            // 69 => encodedText.Replace("????:", $"<sprite=\"BTOSRoleIcons\" name=\"Role16\"> <color=#{ColorUtility.ToHtmlStringRGB(Fancy.JuryColor.Value.ToColor())}>{Fancy.JuryLabel.Value}:</color>"),

            71 => $"<link=\"r46\"><sprite=\"BTOSRoleIcons\" name=\"Role46\"><indent=1.1em><b>{Utils.ApplyGradient(Fancy.PirateLabel.Value, Fancy.Colors["PIRATE"].Start, Fancy.Colors["PIRATE"].End)}:</b> </link>{encodedText.Replace("????: </color>", "").Replace("white", "#ECC23E")}",
            _ => __result
        };
    }
}

[HarmonyPatch(typeof(FactionWinsCinematicPlayer), nameof(FactionWinsCinematicPlayer.Init))]
public static class PatchDefaultWinScreens
{
    private static readonly int State = Animator.StringToHash("State");

    public static bool Prefix(FactionWinsCinematicPlayer __instance, ICinematicData cinematicData)
    {
        __instance.elapsedDuration = 0f;
        Debug.Log($"FactionWinsCinematicPlayer current phase at start = {Pepper.GetGamePhase()}");
        __instance.cinematicData = cinematicData as FactionWinsCinematicData;
        __instance.totalDuration = CinematicFactionWinsTimes.GetWinTimeByFaction(__instance.cinematicData!.winningFaction);
        __instance.callbackTimers.Clear();
        var spawnedCharacters = Service.Game.Cast.GetSpawnedCharacters();

        if (spawnedCharacters == null)
        {
            Debug.LogError("spawnedPlayers is null in GetCrowd()");
            return false;
        }

        var positions = new HashSet<int>();
        __instance.cinematicData.entries.ForEach(e => positions.Add(e.position));
        spawnedCharacters.ForEach(c =>
        {
            if (positions.Contains(c.position))
                __instance.winningCharacters.Add(c);
            else
                c.characterSprite.SetColor(Color.clear);
        });
        var winningFaction = __instance.cinematicData.winningFaction;

        if (winningFaction == FactionType.TOWN)
        {
            Service.Home.AudioService.PlayMusic("Audio/Music/TownVictory.wav", false, AudioController.AudioChannel.Cinematic);
            __instance.evilProp.SetActive(false);
            __instance.goodProp.SetActive(true);
            __instance.m_Animator.SetInteger(State, 1);
        }
        else
        {
            Service.Home.AudioService.PlayMusic("Audio/Music/CovenVictory.wav", false, AudioController.AudioChannel.Cinematic);
            __instance.evilProp.SetActive(true);
            __instance.goodProp.SetActive(false);
            __instance.m_Animator.SetInteger(State, 2);
        }

        var text = Utils.GetString($"GUI_WINNERS_ARE_{(int)winningFaction}").RemoveColorTags();
        var gradient = winningFaction.GetChangedGradient(Role.NONE);

        if (gradient != null)
        {
            __instance.leftImage.color = Utils.GetFactionStartingColor(winningFaction);
            __instance.rightImage.color = Fancy.VerticalGradients.Value ? Utils.GetFactionStartingColor(winningFaction) : Utils.GetFactionEndingColor(winningFaction);
            __instance.textAnimatorPlayer.ShowText(Utils.ApplyGradient(text, gradient));
        }
        else
        {
            if (ColorUtility.TryParseHtmlString(winningFaction.GetFactionColor(), out var color))
            {
                __instance.leftImage.color = color;
                __instance.rightImage.color = color;
                __instance.glow.color = color;
            }

            __instance.text.color = color;
            __instance.textAnimatorPlayer.ShowText(text);
        }

        __instance.SetUpWinners(__instance.winningCharacters);
        return false;
    }
}

[HarmonyPatch(typeof(FactionWinsStandardCinematicPlayer), nameof(FactionWinsStandardCinematicPlayer.Init))]
public static class PatchCustomWinScreens
{
    public static bool Prefix(FactionWinsStandardCinematicPlayer __instance, ICinematicData cinematicData)
    {
        Debug.Log($"FactionWinsStandardCinematicPlayer current phase at end = {Pepper.GetGamePhase()}");
        __instance.elapsedDuration = 0f;
        __instance.cinematicData = cinematicData as FactionWinsCinematicData;
        __instance.totalDuration = CinematicFactionWinsTimes.GetWinTimeByFaction(__instance.cinematicData!.winningFaction);

        var winningFaction = __instance.cinematicData.winningFaction;
        PlayVictoryMusic(winningFaction);
        var text2 = Utils.GetString($"GUI_WINNERS_ARE_{(int)winningFaction}").RemoveColorTags();
        var gradient = winningFaction.GetChangedGradient(Role.NONE);

        if (gradient != null)
            text2 = Utils.ApplyGradient(text2, gradient);

        if (__instance.textAnimatorPlayer.gameObject.activeSelf)
            __instance.textAnimatorPlayer.ShowText(text2);

        foreach (var child in __instance.transform.GetComponentsInChildren<Transform>(true))
        {
            if (child.name is not ("Filigree_L" or "Filigree_R" or "Glow"))
                continue;

            if (child.TryGetComponent<Image>(out var image))
            {
                if (Fancy.VerticalGradients.Value)
                image.color = Utils.GetFactionStartingColor(winningFaction);
                else
                image.color = child.name == "Filigree_R" ? Utils.GetFactionEndingColor(winningFaction) : Utils.GetFactionStartingColor(winningFaction);
            }
                
        }

        __instance.SetUpWinners();
        return false;
    }

    private static void PlayVictoryMusic(FactionType winningFaction)
    {
        var musicPath = GetVictoryMusicPath(winningFaction);

        if (!NewModLoading.Utils.IsNullEmptyOrWhiteSpace(musicPath))
            Service.Home.AudioService.PlayMusic(musicPath, false, AudioController.AudioChannel.Cinematic);
    }

    private static string GetVictoryMusicPath(FactionType faction) => (Fancy.CinematicMap.TryGetValue(faction, out var setting) ? setting.Value : Fancy.GetCinematic(faction)) switch
    {
        CinematicType.TownWins => "Audio/Music/TownVictory.wav",
        CinematicType.CovenWins or CinematicType.FactionWins => "Audio/Music/CovenVictory.wav",
        _ => null
    };
}

[HarmonyPatch(typeof(ClientRoleExtensions))]
public static class ClientRoleExtensionsPatches
{
    [HarmonyPatch(nameof(ClientRoleExtensions.ToColorizedDisplayString), typeof(Role), typeof(FactionType))]
    public static void Postfix(ref string __result, Role role, FactionType factionType)
    {
        var gradient = factionType.GetChangedGradient(role);

        // Pretty sure this got removed from vanilla but I don't know for sure
        if (__result.Contains("<color=#B545FF>(Traitor)"))
        {
            __result = __result.Replace("<color=#B545FF>(Traitor)</color>", gradient != null
                ? $"{Utils.ApplyGradient($"({Fancy.CovenTraitorLabel.Value})", gradient)}"
                : $"<style=CovenColor>({Fancy.CovenTraitorLabel.Value})</style>");
        }

        if (!role.IsResolved() && role is not (Role.FAMINE or Role.DEATH or Role.PESTILENCE or Role.WAR))
            return;

        var text = Fancy.FactionalRoleNames.Value ? role.ToRoleFactionDisplayString(factionType) : role.ToDisplayString();
        var newText = gradient != null ? Utils.ApplyGradient(text, gradient) : $"<color={factionType.GetFactionColor()}>{text}</color>";
        var factionText = factionType.ToDisplayString();

        if (((Fancy.FactionNameNextToRole.Value == FactionLabelOption.Mismatch && role.GetFaction() != factionType) || (Fancy.FactionNameNextToRole.Value == FactionLabelOption.Always) ||
            (Fancy.FactionNameNextToRole.Value == FactionLabelOption.Conditional && !Utils.ConditionalCompliancePandora(role.GetFaction(), factionType))) && !Pepper.IsRoleRevealPhase())
        {
            if (factionType == Btos2Faction.Jackal)
            {
                var originalFaction = role.GetFactionType(Utils.GetGameType());
                factionText = originalFaction switch
                {
                    Btos2Faction.Town => Fancy.RecruitLabelTown.Value,
                    Btos2Faction.Coven => !Constants.IsPandora() ? Fancy.RecruitLabelCoven.Value : Fancy.RecruitLabelPandora.Value,
                    Btos2Faction.Apocalypse => !Constants.IsPandora() ? Fancy.RecruitLabelApocalypse.Value : Fancy.RecruitLabelPandora.Value,
                    Btos2Faction.SerialKiller => !Constants.IsCompliance() ? Fancy.RecruitLabelSerialKiller.Value : Fancy.RecruitLabelCompliance.Value,
                    Btos2Faction.Arsonist => !Constants.IsCompliance() ? Fancy.RecruitLabelArsonist.Value : Fancy.RecruitLabelCompliance.Value,
                    Btos2Faction.Werewolf => !Constants.IsCompliance() ? Fancy.RecruitLabelWerewolf.Value : Fancy.RecruitLabelCompliance.Value,
                    Btos2Faction.Shroud => !Constants.IsCompliance() ? Fancy.RecruitLabelShroud.Value : Fancy.RecruitLabelCompliance.Value,
                    Btos2Faction.CursedSoul => Fancy.RecruitLabelCursedSoul.Value,
                    _ => factionType.ToDisplayString()
                };
            }
            if (gradient != null)
                newText += $" {Utils.ApplyGradient($"({factionText})", gradient)}";
            else
                newText += $" <color={factionType.GetFactionColor()}>({factionText})</color>";
        }

        __result = newText;
    }

    [HarmonyPatch(nameof(ClientRoleExtensions.GetFactionColor))]
    public static bool Prefix(ref string __result, FactionType factionType)
    {
        __result = Fancy.Colors[Utils.FactionName(factionType, stoned: true).ToUpper()].Start;
        return false;
    }

    [HarmonyPatch(nameof(ClientRoleExtensions.ApplyFactionColor))]
    public static void Postfix(ref string __result, string text, FactionType factionType)
    {
        var factionColor = factionType.GetFactionColor();
        __result = "<color=" + factionColor + ">" + text + "</color>";
    }

    [HarmonyPatch(nameof(ClientRoleExtensions.GetBucketDisplayString))]
    public static void Postfix(ref string __result, Role role)
    {
        if (!Fancy.GradientBuckets.Value)
            return;

        var town = FactionType.TOWN.GetChangedGradient(Role.ADMIRER);
        var townMajor = FactionType.TOWN.GetChangedGradient(Role.MAYOR);
        var townLethal = FactionType.TOWN.GetChangedGradient(Role.VIGILANTE);
        var coven = FactionType.COVEN.GetChangedGradient(Role.DREAMWEAVER);
        var covenMajor = FactionType.COVEN.GetChangedGradient(Role.COVENLEADER);
        var covenLethal = FactionType.COVEN.GetChangedGradient(Role.CONJURER);
        var apocalypse = FactionType.APOCALYPSE.GetChangedGradient(Role.PLAGUEBEARER);
        var pandora = Btos2Faction.Pandora.GetChangedGradient(Role.DREAMWEAVER);
        var pandoraMajor = Btos2Faction.Pandora.GetChangedGradient(Role.COVENLEADER);
        var pandoraLethal = Btos2Faction.Pandora.GetChangedGradient(Role.CONJURER);
        var compliance = Btos2Faction.Compliance.GetChangedGradient(Role.SERIALKILLER);
        var bucket = Utils.CreateGradient(Fancy.BucketStart.Value, Fancy.BucketEnd.Value);
        var neutral = Utils.CreateGradient(Fancy.NeutralStart.Value, Fancy.NeutralEnd.Value);

        if (Constants.IsBTOS2())
        {
            __result = role switch
            {
                Btos2Role.Any => Utils.GetString("BTOS_ROLEBUCKET_100"),
                Btos2Role.RandomTown => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_RANDOM"), bucket)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN"), town)}",
                Btos2Role.CommonTown => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COMMON"), bucket)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN"), town)}",
                Btos2Role.TownInvestigative => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN"), town)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_INVESTIGATIVE"), bucket)}",
                Btos2Role.TownProtective => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN"), town)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_PROTECTIVE"), bucket)}",
                Btos2Role.TownSupport => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN"), town)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_SUPPORT"), bucket)}",
                Btos2Role.TownKilling => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN"), townLethal)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_KILLING"), bucket)}",
                Btos2Role.TownGovernment => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN"), townMajor)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_GOVERNMENT"), bucket)}",
                Btos2Role.TownExecutive => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN"), townMajor)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_EXECUTIVE"), bucket)}",
                Btos2Role.RandomCoven => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_RANDOM"), bucket)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN"), coven)}",
                Btos2Role.CommonCoven => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COMMON"), bucket)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN"), coven)}",
                Btos2Role.CovenPower => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN"), covenMajor)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_POWER"), bucket)}",
                Btos2Role.CovenKilling => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN"), covenLethal)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_KILLING"), bucket)}",
                Btos2Role.CovenUtility => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN"), coven)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_UTILITY"), bucket)}",
                Btos2Role.CovenDeception => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN"), coven)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_DECEPTION"), bucket)}",
                Btos2Role.RandomApocalypse => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_RANDOM"), bucket)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_APOCALYPSE"), apocalypse)}",
                Btos2Role.NeutralKilling => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_NEUTRAL"), neutral)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_KILLING"), bucket)}",
                Btos2Role.NeutralEvil => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_NEUTRAL"), neutral)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_EVIL"), bucket)}",
                Btos2Role.NeutralPariah => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_NEUTRAL"), neutral)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_PARIAH"), bucket)}",
                Btos2Role.NeutralOutlier => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_NEUTRAL"), neutral)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_OUTLIER"), bucket)}",
                Btos2Role.RandomNeutral => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_RANDOM"), bucket)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_NEUTRAL"), neutral)}",
                _ => string.Empty,
            };
        }
        else
        {
            __result = role switch
            {
                Role.ANY => Utils.GetString("GUI_ROLE_LIST_BUCKET_ANY"),
                Role.RANDOM_TOWN => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_RANDOM"), bucket)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN"), town)}",
                Role.COMMON_TOWN => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COMMON"), bucket)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN"), town)}",
                Role.TOWN_INVESTIGATIVE => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN"), town)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_INVESTIGATIVE"), bucket)}",
                Role.TOWN_PROTECTIVE => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN"), town)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_PROTECTIVE"), bucket)}",
                Role.TOWN_SUPPORT => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN"), town)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_SUPPORT"), bucket)}",
                Role.TOWN_KILLING => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN"), townLethal)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_KILLING"), bucket)}",
                Role.TOWN_POWER => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN"), townMajor)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_POWER"), bucket)}",
                Role.RANDOM_COVEN => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_RANDOM"), bucket)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN"), coven)}",
                Role.COMMON_COVEN => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COMMON"), bucket)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN"), coven)}",
                Role.COVEN_POWER => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN"), covenMajor)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_POWER"), bucket)}",
                Role.COVEN_KILLING => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN"), covenLethal)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_KILLING"), bucket)}",
                Role.COVEN_UTILITY => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN"), coven)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_UTILITY"), bucket)}",
                Role.COVEN_DECEPTION => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN"), coven)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_DECEPTION"), bucket)}",
                Role.NEUTRAL_APOCALYPSE => !Fancy.ReplaceNAwithRA.Value
                    ? $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_NEUTRAL"), neutral)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_APOCALYPSE"), bucket)}"
                    : $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_RANDOM"), bucket)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_APOCALYPSE"), apocalypse)}",
                Role.NEUTRAL_KILLING => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_NEUTRAL"), neutral)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_KILLING"), bucket)}",
                Role.NEUTRAL_EVIL => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_NEUTRAL"), neutral)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_EVIL"), bucket)}",
                Role.RANDOM_NEUTRAL => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_RANDOM"), bucket)} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_NEUTRAL"), neutral)}",
                _ => string.Empty,
            };
        }
    }

[HarmonyPatch(nameof(ClientRoleExtensions.GetShortenedBucketDisplayString)), HarmonyPostfix]
public static void PostfixShortened(ref string __result, Role role)
{
    if (!Fancy.GradientBuckets.Value)
        return;

    var town = FactionType.TOWN.GetChangedGradient(Role.ADMIRER);
    var townMajor = FactionType.TOWN.GetChangedGradient(Role.MAYOR);
    var townLethal = FactionType.TOWN.GetChangedGradient(Role.VIGILANTE);
    var coven = FactionType.COVEN.GetChangedGradient(Role.DREAMWEAVER);
    var covenMajor = FactionType.COVEN.GetChangedGradient(Role.COVENLEADER);
    var covenLethal = FactionType.COVEN.GetChangedGradient(Role.CONJURER);
    var apocalypse = FactionType.APOCALYPSE.GetChangedGradient(Role.PLAGUEBEARER);
    var compliance = Btos2Faction.Compliance.GetChangedGradient(Role.SERIALKILLER);
    var bucket = Utils.CreateGradient(Fancy.BucketStart.Value, Fancy.BucketEnd.Value);
    var neutral = Utils.CreateGradient(Fancy.NeutralStart.Value, Fancy.NeutralEnd.Value);

    if (Constants.IsBTOS2())
    {
        __result = role switch
        {
            Btos2Role.Any => Utils.GetString("BTOS_ROLE_LIST_BUCKET_ANY_SHORT"),
            Btos2Role.RandomTown => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_RANDOM_SHORT"), bucket)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN_SHORT"), town)}",
            Btos2Role.CommonTown => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COMMON_SHORT"), bucket)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN_SHORT"), town)}",
            Btos2Role.TownInvestigative => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN_SHORT"), town)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_INVESTIGATIVE_SHORT"), bucket)}",
            Btos2Role.TownProtective => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN_SHORT"), town)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_PROTECTIVE_SHORT"), bucket)}",
            Btos2Role.TownSupport => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN_SHORT"), town)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_SUPPORT_SHORT"), bucket)}",
            Btos2Role.TownKilling => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN_SHORT"), townLethal)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_KILLING_SHORT"), bucket)}",
            Btos2Role.TownGovernment => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN_SHORT"), townMajor)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_GOVERNMENT_SHORT"), bucket)}",
            Btos2Role.TownExecutive => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN_SHORT"), townMajor)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_EXECUTIVE_SHORT"), bucket)}",
            Btos2Role.RandomCoven => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_RANDOM_SHORT"), bucket)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN_SHORT"), coven)}",
            Btos2Role.CommonCoven => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COMMON_SHORT"), bucket)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN_SHORT"), coven)}",
            Btos2Role.CovenPower => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN_SHORT"), covenMajor)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_POWER_SHORT"), bucket)}",
            Btos2Role.CovenKilling => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN_SHORT"), covenLethal)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_KILLING_SHORT"), bucket)}",
            Btos2Role.CovenUtility => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN_SHORT"), coven)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_UTILITY_SHORT"), bucket)}",
            Btos2Role.CovenDeception => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN_SHORT"), coven)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_DECEPTION_SHORT"), bucket)}",
            Btos2Role.RandomApocalypse => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_RANDOM_SHORT"), bucket)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_APOCALYPSE_SHORT"), apocalypse)}",
            Btos2Role.NeutralKilling => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_NEUTRAL_SHORT"), neutral)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_KILLING_SHORT"), bucket)}",
            Btos2Role.NeutralEvil => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_NEUTRAL_SHORT"), neutral)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_EVIL_SHORT"), bucket)}",
            Btos2Role.NeutralPariah => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_NEUTRAL_SHORT"), neutral)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_PARIAH_SHORT"), bucket)}",
            Btos2Role.NeutralOutlier => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_NEUTRAL_SHORT"), neutral)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_OUTLIER_SHORT"), bucket)}",
            Btos2Role.RandomNeutral => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_RANDOM_SHORT"), bucket)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_NEUTRAL_SHORT"), neutral)}",
            _ => string.Empty,
        };
    }
    else
    {
        __result = role switch
        {
            Role.ANY => Utils.GetString("GUI_ROLE_LIST_BUCKET_ANY_SHORT"),
            Role.RANDOM_TOWN => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_RANDOM_SHORT"), bucket)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN_SHORT"), town)}",
            Role.COMMON_TOWN => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COMMON_SHORT"), bucket)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN_SHORT"), town)}",
            Role.TOWN_INVESTIGATIVE => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN_SHORT"), town)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_INVESTIGATIVE_SHORT"), bucket)}",
            Role.TOWN_PROTECTIVE => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN_SHORT"), town)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_PROTECTIVE_SHORT"), bucket)}",
            Role.TOWN_SUPPORT => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN_SHORT"), town)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_SUPPORT_SHORT"), bucket)}",
            Role.TOWN_KILLING => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN_SHORT"), townLethal)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_KILLING_SHORT"), bucket)}",
            Role.TOWN_POWER => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN_SHORT"), townMajor)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_POWER_SHORT"), bucket)}",
            Role.RANDOM_COVEN => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_RANDOM_SHORT"), bucket)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN_SHORT"), coven)}",
            Role.COMMON_COVEN => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COMMON_SHORT"), bucket)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN_SHORT"), coven)}",
            Role.COVEN_POWER => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN_SHORT"), covenMajor)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_POWER_SHORT"), bucket)}",
            Role.COVEN_KILLING => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN_SHORT"), covenLethal)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_KILLING_SHORT"), bucket)}",
            Role.COVEN_UTILITY => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN_SHORT"), coven)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_UTILITY_SHORT"), bucket)}",
            Role.COVEN_DECEPTION => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN_SHORT"), coven)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_DECEPTION_SHORT"), bucket)}",
            Role.NEUTRAL_APOCALYPSE => !Fancy.ReplaceNAwithRA.Value
                ? $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_NEUTRAL_SHORT"), neutral)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_APOCALYPSE_SHORT"), bucket)}"
                : $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_RANDOM_SHORT"), bucket)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_APOCALYPSE_SHORT"), apocalypse)}",
            Role.NEUTRAL_KILLING => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_NEUTRAL_SHORT"), neutral)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_KILLING_SHORT"), bucket)}",
            Role.NEUTRAL_EVIL => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_NEUTRAL_SHORT"), neutral)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_EVIL_SHORT"), bucket)}",
            Role.RANDOM_NEUTRAL => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_RANDOM_SHORT"), bucket)}{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_NEUTRAL_SHORT"), neutral)}",
            _ => string.Empty,
        };
    }
}
    [HarmonyPatch(nameof(ClientRoleExtensions.ToColorizedShortenedDisplayString), typeof(Role), typeof(FactionType)), HarmonyPostfix]
    public static void ToColorizedShortenedDisplayStringPostfix(ref string __result, Role role, FactionType factionType)
    {
        var gradient = factionType.GetChangedGradient(role);

        if (!role.IsResolved() && role is not (Role.FAMINE or Role.DEATH or Role.PESTILENCE or Role.WAR))
            return;

        var text = role.ToShortenedDisplayString();

        __result = gradient != null
            ? Utils.ApplyGradient(text, gradient)
            : $"<color={factionType.GetFactionColor()}>{text}</color>";
    }

    [HarmonyPatch(nameof(ClientRoleExtensions.GetSecondFactionColor)), HarmonyPrefix]
    public static bool GetSecondFactionColorPrefix(ref string __result, FactionType factionType)
    {
        __result = Fancy.Colors[Utils.FactionName(factionType, stoned: true).ToUpper()].Start;
        return false;
    }
}

// This is needed because of build conflicts (ClientRoleExtensionsPatches.Postfix & ClientRoleExtensionsPatches.Postfix, for example) - Loonie
// Build conflicts my ass, scroll up - AD

// #{ColorUtility.ToHtmlStringRGB(Utils.GetPlayerRoleColor(position))}

[HarmonyPatch(typeof(SharedMentionsProvider))]
public static class KeywordMentionsPatches
{
    [HarmonyPatch(nameof(SharedMentionsProvider.BuildKeywordMentions))]
    public static void Postfix(SharedMentionsProvider __instance)
    {
        if (!__instance._useColors)
            return;

        var gradient = Utils.CreateGradient(Fancy.KeywordStart.Value, Fancy.KeywordEnd.Value);

        foreach (var mentionInfo in __instance.MentionInfos)
        {
            if (mentionInfo.mentionInfoType != MentionInfo.MentionInfoType.KEYWORD)
                continue;

            if (mentionInfo.richText.Contains("color=") || mentionInfo.richText.Contains("gradient="))
                continue;

            var raw = mentionInfo.humanText.TrimStart(':');

            if (string.IsNullOrWhiteSpace(raw))
                continue;

            var color = Fancy.KeywordStart.Value;
            var keyword = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(raw);
            var newText = Utils.ApplyGradient($"<b>{keyword}</b>", gradient);
            // var newText = $"<color={color}><b>{keyword}</b></color>";
            var encodedText = mentionInfo.encodedText;
            var keywordId = encodedText.TrimStart('[', ':').TrimEnd(']');

            mentionInfo.richText = $"{__instance.styleTagOpen}{__instance.styleTagFont}<link=\"k{keywordId}\">{newText}</link>{__instance.styleTagClose}";
            mentionInfo.hashCode = mentionInfo.richText.ToLowerInvariant().GetHashCode();
        }
    }

    [HarmonyPatch(nameof(SharedMentionsProvider.Build))]
    public static bool Prefix(SharedMentionsProvider __instance, RebuildMentionTypesFlag rebuildMentionTypesFlag)
    {
        var roles = rebuildMentionTypesFlag.HasFlag(RebuildMentionTypesFlag.ROLES);
        var keywords = rebuildMentionTypesFlag.HasFlag(RebuildMentionTypesFlag.KEYWORDS);
        var players = rebuildMentionTypesFlag.HasFlag(RebuildMentionTypesFlag.PLAYERS);
        var prefixes = rebuildMentionTypesFlag.HasFlag(RebuildMentionTypesFlag.PREFIXES);
        var emojis = rebuildMentionTypesFlag.HasFlag(RebuildMentionTypesFlag.EMOJIS);
        var achievements = rebuildMentionTypesFlag.HasFlag(RebuildMentionTypesFlag.ACHIEVEMENTS);

        __instance._useColors = Service.Home.UserService.Settings.MentionsUseColorsEnabled;
        __instance._playerEffects = Service.Home.UserService.Settings.MentionsPlayerEffects;
        __instance._roleEffects = Service.Home.UserService.Settings.MentionsRoleEffects;

        __instance.ClearMentions(roles, keywords, players, prefixes, emojis, achievements);

        if (roles)
        {
            if (!Constants.IsBTOS2())
                __instance.BuildRoleMentions();
            else
                BuildCustomRoleMentions(__instance);
            BuildFactionMentions(__instance);
        }

        if (keywords)
            BuildCustomKeywordMentions(__instance);

        if (players)
            __instance.BuildPlayerMentions();

        if (!Constants.IsBTOS2())
        {
            if (prefixes)
                __instance.BuildPrefixMentions();

            if (emojis)
                __instance.BuildEmojiMentions();

            if (achievements)
                __instance.BuildAchievementMentions();
        }

        return false;
    }

    private static void BuildCustomKeywordMentions(SharedMentionsProvider __instance)
    {
        var gradient = __instance._useColors ? Utils.CreateGradient(Fancy.KeywordStart.Value, Fancy.KeywordEnd.Value) : null;

        var keywordList = Service.Game.Keyword.keywordInfo
            .Select(k => (Keyword: k, Localized: Utils.GetString(k.KeywordKey)))
            .OrderBy(k => k.Localized, StringComparer.Ordinal)
            .ToList();

        var priority = 0;

        foreach (var (keyword, localizedText) in keywordList)
        {
            if (string.IsNullOrWhiteSpace(localizedText))
                continue;

            var encodedText = $"[[:{keyword.KeywordId}]]";

            var color = Fancy.KeywordStart.Value;
            var coloredText = __instance._useColors
                ? Utils.ApplyGradient($"<b>{localizedText}</b>", gradient)
                : $"<b>{localizedText}</b>";

            var richText = $"{__instance.styleTagOpen}{__instance.styleTagFont}<link=\"k{keyword.KeywordId}\">{coloredText}</link>{__instance.styleTagClose}";

            var mentionInfo = new MentionInfo
            {
                mentionInfoType = MentionInfo.MentionInfoType.KEYWORD,
                richText = richText,
                encodedText = encodedText,
                hashCode = richText.ToLowerInvariant().GetHashCode(),
                humanText = ":" + localizedText.ToLowerInvariant()
            };

            __instance.MentionInfos.Add(mentionInfo);
            __instance.MentionTokens.Add(new MentionToken
            {
                mentionTokenType = MentionToken.MentionTokenType.KEYWORD,
                match = ":" + localizedText,
                mentionInfo = mentionInfo,
                priority = priority++
            });
        }
    }

    public static void BuildCustomRoleMentions(SharedMentionsProvider __instance)
    {
        var list = Service.Game.Roles.roleInfos
            .Where(item => !item.role.IsModifierCard() && item.role != Role.NONE)
            .OrderBy(item => item.role.ToDisplayString(), StringComparer.Ordinal)
            .ToList();

        var priority = 0;

        foreach (var item in list)
        {
            var role = (int)item.role;
            var display = item.role.ToDisplayString();
            var shortName = item.shortRoleName.Length > 0 ? item.shortRoleName : display;

            var encodedText = $"[[#{role}]]";

            var sprite = (__instance._roleEffects == 1) ? $"<sprite=\"BTOSRoleIcons\" name=\"Role{role}\">" : "";
            var name = __instance._useColors ? item.role.ToColorizedDisplayString() : display;

            var richText = $"{__instance.styleTagOpen}{__instance.styleTagFont}<link=\"r{role}\">{sprite}<b>{name}</b></link>{__instance.styleTagClose}";

            var mentionInfo = new MentionInfo
            {
                mentionInfoType = MentionInfo.MentionInfoType.ROLE,
                richText = richText,
                encodedText = encodedText,
                hashCode = richText.ToLowerInvariant().GetHashCode(),
                humanText = "#" + display.ToLowerInvariant()
            };

            __instance.MentionInfos.Add(mentionInfo);
            __instance.MentionTokens.Add(new MentionToken
            {
                mentionTokenType = MentionToken.MentionTokenType.ROLE,
                match = "#" + shortName,
                mentionInfo = mentionInfo,
                priority = priority++
            });
        }
    }
    private static void BuildFactionMentions(SharedMentionsProvider __instance)
    {
        Dictionary<FactionType, Role> dict;
        if (!Constants.IsBTOS2())
            dict = new()
            {
                { FactionType.TOWN, Role.RANDOM_TOWN },
                { FactionType.COVEN, Role.RANDOM_COVEN },
                { FactionType.SERIALKILLER, Role.SERIALKILLER },
                { FactionType.ARSONIST, Role.ARSONIST },
                { FactionType.WEREWOLF, Role.WEREWOLF },
                { FactionType.SHROUD, Role.SHROUD },
                { FactionType.APOCALYPSE, Role.NEUTRAL_APOCALYPSE },
                { FactionType.EXECUTIONER, Role.EXECUTIONER },
                { FactionType.JESTER, Role.JESTER },
                { FactionType.PIRATE, Role.PIRATE },
                { FactionType.DOOMSAYER, Role.DOOMSAYER },
                { FactionType.VAMPIRE, Role.VAMPIRE },
                { FactionType.CURSED_SOUL, Role.CURSED_SOUL }
            };

        else
            dict = new()
            {
                { FactionType.TOWN, Btos2Role.RandomTown },
                { FactionType.COVEN, Btos2Role.RandomCoven },
                { FactionType.SERIALKILLER, Btos2Role.SerialKiller },
                { FactionType.ARSONIST, Btos2Role.Arsonist },
                { FactionType.WEREWOLF, Btos2Role.Werewolf },
                { FactionType.SHROUD, Btos2Role.Shroud },
                { FactionType.APOCALYPSE, Btos2Role.RandomApocalypse },
                { FactionType.EXECUTIONER, Btos2Role.Executioner },
                { FactionType.JESTER, Btos2Role.Jester },
                { FactionType.PIRATE, Btos2Role.Pirate },
                { FactionType.DOOMSAYER, Btos2Role.Doomsayer },
                { FactionType.VAMPIRE, Btos2Role.Vampire },
                { FactionType.CURSED_SOUL, Btos2Role.CursedSoul },
                { Btos2Faction.Jackal, Btos2Role.Jackal },
                { Btos2Faction.Frogs, Btos2Role.Teams },
                { Btos2Faction.Lions, Btos2Role.Teams },
                { Btos2Faction.Hawks, Btos2Role.Teams },
                { Btos2Faction.Judge, Btos2Role.Judge },
                { Btos2Faction.Auditor, Btos2Role.Auditor },
                { Btos2Faction.Inquisitor, Btos2Role.Inquisitor },
                { Btos2Faction.Starspawn, Btos2Role.Starspawn },
                { Btos2Faction.Egotist, Btos2Role.Egotist },
                { Btos2Faction.Pandora, Btos2Role.PandorasBox },
                { Btos2Faction.Compliance, Btos2Role.CompliantKillers }
            };
        var shortNames = new Dictionary<FactionType, string>
        {
            { FactionType.COVEN, "TT" },
            { FactionType.SERIALKILLER, "SK" },
            { FactionType.WEREWOLF, "WW" },
            { FactionType.APOCALYPSE, "ATT" },
            { FactionType.VAMPIRE, "CONVERTED" },
            { FactionType.CURSED_SOUL, "CS" },
            { Btos2Faction.Jackal, "RECRUITED" },
            { Btos2Faction.Frogs, "BLUE" },
            { Btos2Faction.Lions, "YELLOW" },
            { Btos2Faction.Hawks, "RED" },
            { Btos2Faction.Starspawn, "SS" },
            { Btos2Faction.Egotist, "EGOTOWNIE" },
            { Btos2Faction.Pandora, "PTT" },
            { Btos2Faction.Compliance, "COMKILLERS" }
        };
        var priority = 0;

        foreach (var kvp in dict)
        {
            var item = kvp.Key;
            var roleIcon = kvp.Value.GetTMPSprite();
            roleIcon = roleIcon.Replace("RoleIcons\"", $"RoleIcons ({((kvp.Value.GetFactionType() == item && Constants.CurrentStyle() == "Regular")
            ? "Regular"
            : Utils.FactionName(item, false))})\"");
            var faction = (int)item;
            var display = item.ToDisplayString();
            var shortName = shortNames.ContainsKey(item) ? shortNames.GetValue(item) : display;
            var encodedText = $"{faction}";
            var name = __instance._useColors ? Utils.ApplyGradient(item.ToDisplayString(), item.GetChangedGradient(Constants.IsBTOS2() ? Btos2Role.Jackal : Role.DREAMWEAVER)) : display;
            name = __instance._roleEffects == 1 ? roleIcon + name : name;

            var richText = $"{__instance.styleTagOpen}{__instance.styleTagFont}<b>{name}</b>{__instance.styleTagClose}";

            var mentionInfo = new MentionInfo
            {
                mentionInfoType = (MentionInfo.MentionInfoType)10,
                richText = richText,
                encodedText = encodedText,
                hashCode = richText.ToLowerInvariant().GetHashCode(),
                humanText = "$" + display.ToLowerInvariant()
            };

            __instance.MentionInfos.Add(mentionInfo);
            __instance.MentionTokens.Add(new MentionToken
            {
                mentionTokenType = (MentionToken.MentionTokenType)10,
                match = "$" + display,
                mentionInfo = mentionInfo,
                priority = priority++
            });
            if (shortName != display)
                __instance.MentionTokens.Add(new MentionToken
                {
                    mentionTokenType = (MentionToken.MentionTokenType)10,
                    match = "$" + shortName,
                    mentionInfo = mentionInfo,
                    priority = priority++
                });
            if (item == FactionType.COVEN)
                __instance.MentionTokens.Add(new MentionToken
                {
                    mentionTokenType = (MentionToken.MentionTokenType)10,
                    match = "$CTT",
                    mentionInfo = mentionInfo,
                    priority = priority++
                });
            if (faction > 33 && faction < 37)
                __instance.MentionTokens.Add(new MentionToken
                {
                    mentionTokenType = (MentionToken.MentionTokenType)10,
                    match = "$TEAMS",
                    mentionInfo = mentionInfo,
                    priority = priority++
                });
        }
    }

    [HarmonyPatch(nameof(SharedMentionsProvider.PreparePlayerMentions))]
    public static bool Prefix(SharedMentionsProvider __instance, DiscussionPlayerObservation player, int skinId, int i, MentionInfo.MentionInfoType mentionInfoType,
        MentionToken.MentionTokenType mentionTokenType)
    {
        if (Constants.BetterMentionsExists() || !Pepper.IsLobbyOrPickNamesOrGamePhase())
            return true;

        var text = string.IsNullOrWhiteSpace(player.Data.gameName)
            ? player.Data.accountName
            : player.Data.gameName;
        var match = $"@{i + 1}";
        var match2 = "@" + text;
        var encodedText = $"[[@{i + 1}]]";
        var text2 = (mentionTokenType == MentionToken.MentionTokenType.ACCOUNT) ? "a" : string.Empty;
        var gradient = Utils.CreateGradient(Fancy.MentionStart.Value, Fancy.MentionEnd.Value);

        var color = Fancy.MentionStart.Value;
        // var faction = Pepper.GetDiscussionPlayerFactionIfKnown(i);
        // if (faction is not FactionType.NONE and not FactionType.UNKNOWN)
        // {
        //     var roleColor = Utils.GetPlayerRoleColor(i);
        //     var roleColorString = ColorUtility.ToHtmlStringRGB(roleColor);
        //     if (string.IsNullOrEmpty(roleColorString))
        //         roleColorString = "FFCE3B";
        //     color = $"#{roleColorString}";
        // }
        if (string.IsNullOrEmpty(color))
            color = "#FFCE3B";

        var text3 = __instance._useColors
            ? Utils.ApplyGradient($"<b>{text}</b>", gradient)
            : $"<b>{text}</b>";

        var text4 = __instance._playerEffects == 2
            ? $"<sprite=\"PlayerNumbers\" name=\"PlayerNumbers_{player.Data.position + 1}\">"
            : (__instance._playerEffects == 1
                ? $"<sprite=\"Cast\" name=\"Skin{skinId}\">"
                : string.Empty);

        var text5 = $"{__instance.styleTagOpen}{__instance.styleTagFont}<link=\"{text2}{player.Data.position}\">{text4}{text3}</link>{__instance.styleTagClose}";

        var mentionInfo = new MentionInfo
        {
            mentionInfoType = mentionInfoType,
            richText = text5,
            encodedText = encodedText,
            hashCode = text5.ToLower().GetHashCode(),
            humanText = "@" + text.ToLower()
        };

        __instance.MentionInfos.Add(mentionInfo);
        __instance.MentionTokens.Add(new MentionToken
        {
            mentionTokenType = mentionTokenType,
            match = match,
            mentionInfo = mentionInfo,
            priority = i
        });
        __instance.MentionTokens.Add(new MentionToken
        {
            mentionTokenType = mentionTokenType,
            match = match2,
            mentionInfo = mentionInfo,
            priority = i
        });

        return false;
    }

    [HarmonyPatch(nameof(SharedMentionsProvider.ClearMentions))]
    public static void Postfix(SharedMentionsProvider __instance, ref bool rebuildRoles, ref bool rebuildKeywords, ref bool rebuildPlayers, ref bool rebuildPrefixes, ref bool rebuildEmojis, ref bool rebuildAchievements)
    {
        if (rebuildRoles)
        {
            __instance.MentionTokens.RemoveAll((MentionToken m) => m.mentionTokenType == (MentionToken.MentionTokenType)10);
            __instance.MentionInfos.RemoveAll((MentionInfo m) => m.mentionInfoType == (MentionInfo.MentionInfoType)10);
        }
    }
}

// Why were these patches nested??
[HarmonyPatch(typeof(Home.Utils.StringUtils), nameof(Home.Utils.StringUtils.ReplaceRoleTagWithRoleText))]
public static class ReplaceRoleTagWithRoleTextPatch
{
    public static bool Prefix(ref string __result, string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            Debug.LogWarning("ReplaceRoleTagWithRoleText String is null!");
            __result = string.Empty;
            return false;
        }

        var modified = false;

        // Handle %name_role
        var index = str.IndexOf("%name_role");

        while (index > -1)
        {
            var endIndex = str.IndexOf("%", index + 1);

            if (endIndex == -1)
                break;

            var fullTag = str.Substring(index, endIndex - index + 1);
            var idStart = index + 10;

            if (int.TryParse(str[idStart..endIndex], out var roleId))
            {
                var role = (Role)roleId;
                var colorized = role.ToColorizedDisplayString();
                str = str.Replace(fullTag, colorized);
                modified = true;
            }

            index = str.IndexOf("%name_role", index + 1);
        }

        // Handle %name_faction
        index = str.IndexOf("%name_faction");

        while (index > -1)
        {
            var endIndex = str.IndexOf("%", index + 1);

            if (endIndex == -1)
                break;

            var fullTag = str.Substring(index, endIndex - index + 1);
            var idStart = index + 13;

            if (int.TryParse(str[idStart..endIndex], out var factionId))
            {
                var faction = (FactionType)factionId;
                var colorized = Utils.ApplyGradient(faction.ToDisplayString(), faction.GetChangedGradient(Role.DREAMWEAVER));
                str = str.Replace(fullTag, colorized);
                modified = true;
            }

            index = str.IndexOf("%name_faction", index + 1);
        }

        if (modified)
            __result = str;

        return !modified;
    }
}


[HarmonyPatch(typeof(HomeLocalizationService), nameof(HomeLocalizationService.GetLocalizedString))]
public static class LocalizationManagerPatches
{
    public static void Postfix(ref string __result)
    {
        __result = Utils.RemoveVanillaGradientStyleTags(__result);
    }
}

[HarmonyPatch(typeof(GraveyardItem), nameof(GraveyardItem.SetPlayerPicAndName))]
public static class GraveyardItem_SetPlayerPicAndName_Patch
{
    public static void Postfix(GraveyardItem __instance)
    {
        var killRecord = __instance._mKillRecord;
        var faction = killRecord.playerFaction;
        var role = killRecord.playerRole;
        var player = (int)killRecord.playerId;
        var icon = role.GetTMPSprite();
        icon = icon.Replace("RoleIcons\"", $"RoleIcons ({((role.GetFactionType() == faction && Constants.CurrentStyle() == "Regular")
            ? "Regular"
            : Utils.FactionName(faction, false))})\"");

        var name = Service.Game.Sim.simulation.GetPlayerInlineLinkString(player, 1.1) ?? "";

        var text = icon + role.ToColorizedNoLabel(faction);

        var combinedText = $"{name} ({text})";
        __instance.playerAndRoleLabel.SetText(combinedText);
    }
}

[HarmonyPatch(typeof(GameSimulation))]
public static class GameSimPatches
{
    [HarmonyPatch(nameof(GameSimulation.GetVIPText)), HarmonyPostfix]
    public static void VIPTextPostfix(ref string __result)
    {
        var gradient = FactionType.TOWN.GetChangedGradient(Role.ADMIRER);
        __result = $" {Utils.ApplyGradient($"({Fancy.VipLabel.Value})", gradient)}";
    }
}

[HarmonyPatch(typeof(WhoDiedDecoder), nameof(WhoDiedDecoder.Encode))]
public static class WdahChatPatch
{
    public static bool Prefix(ChatLogMessage chatLogMessage, MentionPanel mentionPanel, GamePhase chatPhase, UIController uiController, List<HudChatStyle> chatStyles, ChatWindowType chatWindowType, ref string __result)
    {
        if (chatLogMessage?.chatLogEntry is ChatLogWhoDiedEntry entry && entry.killRecord != null)
        {
            var killRecord = entry.killRecord;
            var playerName = Utils.BuildPlayerTag(killRecord);

            if (entry.subphase == WhoDiedAndHowSubphase.WhoDied)
            {
                __result = killRecord.isDay
                    ? uiController.l10n("GUI_XDIED_TODAY").Replace("%name%", playerName)
                    : uiController.l10n("GUI_XDIED_LAST_NIGHT").Replace("%name%", playerName);
            }
            else if (entry.subphase == WhoDiedAndHowSubphase.Role)
            {
                var roleName = Utils.BuildRoleText(killRecord);
                var key = Utils.GetHangingMessage(killRecord.playerRole, killRecord.playerFaction);

                __result = uiController.l10n(key).Replace("%name%", playerName).Replace("%role%", roleName);
            }
            else
            {
                Debug.LogWarning($"Invalid SubPhase {entry.subphase} in WhoDiedDecoder");
                __result = string.Empty;
            }
            return false;
        }

        Debug.LogWarning("Unable to encode invalid ChatLogWhoDiedEntry.");
        __result = string.Empty;
        return false;
    }
}

[HarmonyPatch(typeof(WhoDiedRoleDecoder), nameof(WhoDiedRoleDecoder.Encode))]
public static class WdahChatPatch2
{
    public static bool Prefix(ChatLogMessage chatLogMessage, MentionPanel mentionPanel, GamePhase chatPhase, UIController uiController, List<HudChatStyle> chatStyles, ChatWindowType chatWindowType, ref string __result)
    {
        if (chatLogMessage?.chatLogEntry is ChatLogWhoDiedEntry entry && entry.killRecord != null)
            {
                var killRecord = entry.killRecord;
                var playerName = Utils.BuildPlayerTag(killRecord);
                var roleName = Utils.BuildRoleText(killRecord);
                var key = Utils.GetHangingMessage(killRecord.playerRole, killRecord.playerFaction);

                __result = uiController.l10n(key).Replace("%name%", playerName).Replace("%role%", roleName);

                return false;
            }

        Debug.LogWarning("Unable to encode invalid ChatLogWhoDiedEntry.");
        __result = string.Empty;
        return false;
    }
}

[HarmonyPatch(typeof(HudGameMessagePoolItem), nameof(HudGameMessagePoolItem.Validate))]
public static class LeaveTownFactionPatch
{
    public static void Postfix(HudGameMessagePoolItem __instance)
    {
        ChatLogGameMessageEntry chatLogGameMessageEntry = __instance._chatLogMessage.chatLogEntry as ChatLogGameMessageEntry;
        if (chatLogGameMessageEntry.messageId == GameFeedbackMessage.LEFT_TOWN)
            leaveTownItems.Add(__instance);
    }
    public static void FixLeaveTownMessages(KillRecord killRecord)
    {
        foreach (HudGameMessagePoolItem __instance in leaveTownItems)
        {
            ChatLogGameMessageEntry chatLogGameMessageEntry = __instance._chatLogMessage.chatLogEntry as ChatLogGameMessageEntry;
            if (killRecord.playerId != chatLogGameMessageEntry.playerNumber1)
                continue;
            string text = __instance.l10n(Constants.IsBTOS2() ? "BTOS_GAME_304" : "GAME_304");
            text = text.Replace("%name%", string.Format("[[@{0}]]", chatLogGameMessageEntry.playerNumber1 + 1));
            Tuple<Role, FactionType> tuple = new(killRecord.playerRole, killRecord.playerFaction);
            text = text.Replace("%role%", string.Format("[[#{0},{1}]]", (int)tuple.Item1, (int)tuple.Item2));
            text = __instance.mentionPanel.mentionsProvider.DecodeText(text);
            __instance.textField.SetText(text);
            string style = __instance.l10nStyle(Constants.IsBTOS2() ? "BTOS_GAME_304" : "GAME_304", "");
            string text2 = __instance.l10nStyle(text, "");
            if (!string.IsNullOrEmpty(text2))
            {
                __instance.SetChatStyle(text2);
            }
            else
            {
                __instance.ChatColor = __instance._chatColor;
            }
            __instance.SetBounds(text);
        }

        leaveTownItems.Clear();
    }

    public static List<HudGameMessagePoolItem> leaveTownItems = new();
}

[HarmonyPatch(typeof(HudGraveyardPanel), nameof(HudGraveyardPanel.CreateGraveyardItem))]
public static class LeaveTownFactionPatch2
{
    public static void Postfix(KillRecord killRecord)
    {
        if (killRecord.killedByReasons.Contains(KilledByReason.LEAVING_TOWN))
            LeaveTownFactionPatch.FixLeaveTownMessages(killRecord);
    }
}