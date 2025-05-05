using Cinematics.Players;
using Game.Characters;
using Home.HomeScene;
using Home.Shared;
using Mentions;
using Server.Shared.Cinematics;
using Server.Shared.Cinematics.Data;
using Server.Shared.Extensions;

namespace FancyUI.Patches;

[HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.UpdateTitle))]
public static class PatchRoleCard
{
    public static void Postfix(RoleCardPanel __instance)
    {
        var component = __instance.GetComponent<GradientRoleColorController>();

        if (component != null)
            UObject.Destroy(component);

        __instance.gameObject.AddComponent<GradientRoleColorController>().Instance = __instance.rolecardBG;
        __instance.roleNameText.text = Pepper.GetMyRole().ToChangedDisplayString(Pepper.GetMyFaction(), Service.Game.Sim.simulation.observations.roleCardObservation.Data.modifier);
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
                        (FactionType)44 => $"\n<size=85%>{Utils.ApplyGradient($"({Fancy.PandoraTraitorLabel.Value})", gradientTt)}</size>",
                        _ => $"\n<size=85%>{Utils.ApplyGradient($"({Fancy.CovenTraitorLabel.Value})", gradientTt)}</size>",
                    };
                }

                break;
            }
            case ROLE_MODIFIER.VIP:
            {
                text += $"\n<size=85%>{Utils.ApplyGradient($"({Fancy.RecruitLabel.Value})", gradientTt)}</size>";
                break;
            }
            case (ROLE_MODIFIER)10:
            {
                var gradient = Btos2Faction.Jackal.GetChangedGradient(role);
                text += $"\n<size=85%>{Utils.ApplyGradient($"({Fancy.VIPLabel.Value})", gradient)}</size>";
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
}

[HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.OnClickRoleDesc))]
public static class RoleCardPanelPatches
{
    public static void Postfix(RoleCardPanel __instance)
    {
        var text = __instance.roleDescText.text;
        var header = Utils.GetString("GUI_ROLECARDHEADER_FACTION");
        var index = text.IndexOf(header);

        if (index < 0)
            return;

        var start = text.IndexOf('\n', index) + 1;
        var end = text.IndexOf('\n', start);

        if (start <= 0 || end <= start)
            return;

        var faction = text[start..end].Trim();

        var factionName = Utils.RemoveColorTags(faction);

        var factionType = __instance.CurrentFaction;

        var gradient = factionType.GetChangedGradient(Role.NONE);
        var colored = Utils.ApplyGradient(factionName, gradient);

        __instance.roleDescText.text = text[..start] + colored + text[end..];
    }
}

[HarmonyPatch(typeof(RoleCardPopupPanel), nameof(RoleCardPopupPanel.SetRole))]
public static class RoleCardPopupPatches
{
    public static void Postfix(Role role, RoleCardPopupPanel __instance) => __instance.roleNameText.text = role.ToColorizedDisplayString();
}

[HarmonyPatch(typeof(RoleCardPopupPanel), nameof(RoleCardPopupPanel.OnClickRoleDesc))]
public static class RoleCardPopupPatches2
{
    public static void Postfix(RoleCardPopupPanel __instance)
    {
        var text = __instance.roleDescText.text;
        var header = Utils.GetString("GUI_ROLECARDHEADER_FACTION");
        var index = text.IndexOf(header);

        if (index < 0)
            return;

        var start = text.IndexOf('\n', index) + 1;
        var end = text.IndexOf('\n', start);

        if (start <= 0 || end <= start)
            return;

        var faction = text[start..end].Trim();

        var factionName = Utils.RemoveColorTags(faction);

        var factionType = __instance.CurrentFaction;

        var gradient = factionType.GetChangedGradient(Role.NONE);
        var colored = Utils.ApplyGradient(factionName, gradient);

        __instance.roleDescText.text = text[..start] + colored + text[end..];
    }
}

[HarmonyPatch(typeof(TosAbilityPanelListItem), nameof(TosAbilityPanelListItem.SetKnownRole))]
public static class PlayerListPatch
{
    public static bool Prefix(Role role, FactionType faction, TosAbilityPanelListItem __instance)
    {
        __instance.playerRole = role;

        if (role is not (0 or (Role)byte.MaxValue))
        {
            __instance.playerRole = role;
            var roleName = Fancy.FactionalRoleNames.Value ? role.ToRoleFactionDisplayString(faction) : role.ToDisplayString();
            var gradient = faction.GetChangedGradient(role);

            if (role is not (Role.STONED or Role.HIDDEN))
                __instance.playerRoleText.text = gradient != null ? Utils.ApplyGradient($"({roleName})", gradient) : $"<color={faction.GetFactionColor()}>({roleName})</color>";
            else
                __instance.playerRoleText.text = $"<color={role.GetFaction().GetFactionColor()}>({roleName})</color>";

            __instance.playerRoleText.gameObject.SetActive(true);
            __instance.playerRoleText.enableAutoSizing = false; // Remove when PlayerNotes+ fix is out
        }

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
    private static readonly FieldInfo StylesField = AccessTools.Field(typeof(TMP_StyleSheet), "m_StyleList");
    private static readonly FieldInfo OpeningDefField = AccessTools.Field(typeof(TMP_Style), "m_OpeningDefinition");

    public static void Postfix()
    {
        if (StylesField.GetValue(TMP_Settings.defaultStyleSheet) is not List<TMP_Style> styles)
            return;

        SetStyle(styles, "TownColor", Fancy.Colors["TOWN"].Start);
        SetStyle(styles, "CovenColor", Fancy.Colors["COVEN"].Start);
        SetStyle(styles, "ApocalypseColor", Fancy.Colors["APOCALYPSE"].Start);
        SetStyle(styles, "SerialKillerColor", Fancy.Colors["SERIALKILLER"].Start);
        SetStyle(styles, "ArsonistColor", Fancy.Colors["ARSONIST"].Start);
        SetStyle(styles, "WerewolfColor", Fancy.Colors["WEREWOLF"].Start);
        SetStyle(styles, "ShroudColor", Fancy.Colors["SHROUD"].Start);
        SetStyle(styles, "ExecutionerColor", Fancy.Colors["EXECUTIONER"].Start);
        SetStyle(styles, "JesterColor", Fancy.Colors["JESTER"].Start);
        SetStyle(styles, "PirateColor", Fancy.Colors["PIRATE"].Start);
        SetStyle(styles, "DoomsayerColor", Fancy.Colors["DOOMSAYER"].Start);
        SetStyle(styles, "VampireColor", Fancy.Colors["VAMPIRE"].Start);
        SetStyle(styles, "CursedSoulColor", Fancy.Colors["CURSEDSOUL"].Start);
        SetStyle(styles, "NeutralColor", Fancy.Colors["STONED_HIDDEN"].Start);

        TMP_Settings.defaultStyleSheet.RefreshStyles();
    }

    private static void SetStyle(List<TMP_Style> styles, string styleName, string colorValue)
    {
        if (styles.TryFinding(s => s.name == styleName, out var style))
            OpeningDefField.SetValue(style, $"<color={colorValue}>");
    }
}

[HarmonyPatch(typeof(ClientRoleExtensions), nameof(ClientRoleExtensions.ToColorizedDisplayString), typeof(Role), typeof(FactionType))]
public static class AddChangedConversionTags
{
    public static void Postfix(ref string __result, Role role, FactionType factionType)
    {
        if (!role.IsResolved() && role is not (Role.FAMINE or Role.DEATH or Role.PESTILENCE or Role.WAR))
            return;

        var text = Fancy.FactionalRoleNames.Value ? role.ToRoleFactionDisplayString(factionType) : role.ToDisplayString();
        var gradient = factionType.GetChangedGradient(role);
        __result = gradient != null ? Utils.ApplyGradient(text, gradient) : $"<color={factionType.GetFactionColor()}>{text}</color>";
    }
}

[HarmonyPatch(typeof(MentionsProvider), nameof(MentionsProvider.DecodeSpeaker))]
public static class FancyChatExperimentalBTOS2
{
    private static readonly List<int> ExcludedIds = [50, 69, 70, 71];

    public static bool Prefix(MentionsProvider __instance, ref string __result, string encodedText, int position, bool isAlive)
    {
        var text = Service.Home.UserService.Settings.ChatNameColor switch // Might replace this with a customizable value (it overrides FancyChat)
        {
            1 => "B0B0B0",
            2 => "CC009E",
            _ => "FCCE3B",
        };
        var text2 = encodedText;

        if (!ExcludedIds.Contains(position) && isAlive)
        {
            var isRecruited = Service.Game.Sim.simulation.observations.playerEffects.Any(x => x.Data.effects.Contains((EffectType)100) && x.Data.playerPosition == position);
            var gameName = Pepper.GetDiscussionPlayerByPosition(position).gameName;
            Gradient gradient = null;

            if (Utils.GetRoleAndFaction(position, out var playerInfo))
            {
                gradient = playerInfo.Item2.GetChangedGradient(playerInfo.Item1);

                if (gradient == null || isRecruited)
                    gradient = Btos2Faction.Jackal.GetChangedGradient(playerInfo.Item1);
            }

            if (gradient != null)
            {
                var text4 = Utils.ApplyGradient($"{gameName}:", gradient);
                text2 = text2.Replace($"<color=#{ColorUtility.ToHtmlStringRGB(Pepper.GetDiscussionPlayerRoleColor(position))}>{gameName}", text4);
            }
            else
                text2 = text2.Replace($"<color=#{text}>", $"<color=#{ColorUtility.ToHtmlStringRGB(Utils.GetPlayerRoleColor(position))}>");
        }

        __result = __instance.ProcessSpeakerName(text2, position, isAlive);
        return false;
    }
}

[HarmonyPatch(typeof(ClientRoleExtensions), nameof(ClientRoleExtensions.GetFactionColor))]
public static class SwapColor
{
    public static bool Prefix(ref string __result, FactionType factionType)
    {
        __result = Fancy.Colors[Utils.FactionName(factionType, stoned: true).ToUpper()].Start;
        return false;
    }
}

[HarmonyPatch(typeof(MentionsProvider), nameof(MentionsProvider.ProcessSpeakerName))]
public static class PatchJudge
{
    public static void Postfix(string encodedText, int position, ref string __result)
    {
        if (Constants.IsBTOS2())
            return;

        __result = position switch
        {
            70 => $"<link=\"r57\"><sprite=\"BTOSRoleIcons\" name=\"Role57\"><indent=1.1em><b>{Utils.ApplyGradient(Fancy.CourtLabel.Value, Fancy.Colors["JUDGE"].Start, Fancy.Colors["JUDGE"].End)}:</b> </link>{encodedText.Replace("????: </color>", "").Replace("white", "#FFFF00")}",
            69 => encodedText.Replace("????:", $"<sprite=\"BTOSRoleIcons\" name=\"Role16\"> {Fancy.JuryLabel.Value}:"),
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

        var text = __instance.l10n($"GUI_WINNERS_ARE_{(int)winningFaction}");
        var gradient = winningFaction.GetChangedGradient(Role.NONE);

        if (gradient != null)
        {
            __instance.leftImage.color = Utils.GetFactionStartingColor(winningFaction);
            __instance.rightImage.color = Utils.GetFactionEndingColor(winningFaction);
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
        var text2 = __instance.l10n($"GUI_WINNERS_ARE_{(int)winningFaction}");
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
                image.color = child.name == "Filigree_R" ? Utils.GetFactionEndingColor(winningFaction) : Utils.GetFactionStartingColor(winningFaction);
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

    private static string GetVictoryMusicPath(FactionType faction)
    {
        return (Fancy.CinematicMap.TryGetValue(faction, out var setting) ? setting.Value : Fancy.GetCinematic(faction)) switch
        {
            CinematicType.TownWins => "Audio/Music/TownVictory.wav",
            CinematicType.CovenWins or CinematicType.FactionWins => "Audio/Music/CovenVictory.wav",
            _ => null
        };
    }
}

[HarmonyPatch(typeof(ClientRoleExtensions), nameof(ClientRoleExtensions.ToColorizedDisplayString), typeof(Role), typeof(FactionType))]
public static class AddTtAndGradients
{
    public static void Postfix(ref string __result, Role role, FactionType factionType)
    {
        if (__result.Contains("<color=#B545FF>(Traitor)"))
            __result = __result.Replace("<color=#B545FF>(Traitor)</color>", $"<style=CovenColor>({Fancy.CovenTraitorLabel.Value})</style>");

        if (!role.IsResolved() && role is not (Role.FAMINE or Role.DEATH or Role.PESTILENCE or Role.WAR))
            return;

        var text = Fancy.FactionalRoleNames.Value ? role.ToRoleFactionDisplayString(factionType) : role.ToDisplayString();
        var gradient = factionType.GetChangedGradient(role);
        var newText = gradient != null ? Utils.ApplyGradient(text, gradient) : $"<color={factionType.GetFactionColor()}>{text}</color>";

        if (((Fancy.FactionNameNextToRole.Value == FactionLabelOption.Mismatch && role.GetFaction() != factionType) || (Fancy.FactionNameNextToRole.Value == FactionLabelOption.Always) ||
            (Fancy.FactionNameNextToRole.Value == FactionLabelOption.Conditional && !Utils.ConditionalCompliancePandora(role.GetFaction(), factionType))) && !Pepper.IsRoleRevealPhase())
        {
            if (gradient != null)
                newText += $" {Utils.ApplyGradient($"({factionType.ToDisplayString()})", gradient)}";
            else
                newText += $" <color={factionType.GetFactionColor()}>({factionType.ToDisplayString()})</color>";
        }

        __result = newText;
    }
}