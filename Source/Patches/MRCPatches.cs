using System.Text.RegularExpressions;
using Cinematics.Players;
using Game.Characters;
using Home.HomeScene;
using Home.Shared;
using Mentions;
using Server.Shared.Cinematics;
using Server.Shared.Cinematics.Data;
using Server.Shared.Extensions;

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
                text += $"\n<size=85%>{Utils.ApplyGradient($"({Fancy.VipLabel.Value})", gradientTt)}</size>";
                break;
            }
            case (ROLE_MODIFIER)10:
            {
                var gradient = Btos2Faction.Jackal.GetChangedGradient(role);
                text += $"\n<size=85%>{Utils.ApplyGradient($"({Fancy.RecruitLabel.Value})", gradient)}</size>";
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

        var factionName = Utils.RemoveColorTags(faction);

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

        var factionName = Utils.RemoveColorTags(faction);

        var factionType = __instance.CurrentFaction;

        var gradient = factionType.GetChangedGradient(factionType == (FactionType)33 ? Btos2Role.Jackal : Role.NONE);
        var colored = Utils.ApplyGradient(factionName, gradient);

        __instance.roleDescText.text = text[..start] + colored + text[end..];
    }

    [HarmonyPatch(nameof(RoleCardPopupPanel.SetRole))]
    public static void Postfix(Role role, RoleCardPopupPanel __instance) => __instance.roleNameText.text = role.ToColorizedDisplayString();
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
                var pattern = $@"<color=#[0-9A-Fa-f]+>{Regex.Escape(gameName)}:";
                var regex = new Regex(pattern);
                text2 = regex.Replace(text2, text4);
            }
            else
                text2 = text2.Replace($"<color=#{text}>", $"<color=#{ColorUtility.ToHtmlStringRGB(Utils.GetPlayerRoleColor(position))}>");
        }

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
            69 => encodedText.Replace("????:", $"<sprite=\"BTOSRoleIcons\" name=\"Role16\"> <color=#{ColorUtility.ToHtmlStringRGB(Fancy.JuryColor.Value.ToColor())}>{Fancy.JuryLabel.Value}:</color>"),

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

[HarmonyPatch(typeof(ClientRoleExtensions))]
public static class ClientRoleExtensionsPatches
{
    [HarmonyPatch(nameof(ClientRoleExtensions.ToColorizedDisplayString), typeof(Role), typeof(FactionType))]
    public static void Postfix(ref string __result, Role role, FactionType factionType)
    {
        var gradient = factionType.GetChangedGradient(role);

        if (__result.Contains("<color=#B545FF>(Traitor)"))
        {
            __result = __result.Replace("<color=#B545FF>(Traitor)</color>", gradient != null
                ? $"{Utils.ApplyGradient($"({Fancy.CovenTraitorLabel.Value})", gradient)}"
                : $"<style=CovenColor>({Fancy.CovenTraitorLabel.Value})</style>");
        }

        if (__result.Contains("<color=#06E00C>(VIP)"))
        {
            __result = __result.Replace("<color=#06E00C>(VIP)</color>", gradient != null
                ? $"{Utils.ApplyGradient($"({Fancy.VipLabel.Value})", gradient)}"
                : $"<style=TownColor>({Fancy.VipLabel.Value})</style>");
        }

        if (!role.IsResolved() && role is not (Role.FAMINE or Role.DEATH or Role.PESTILENCE or Role.WAR))
            return;

        var text = Fancy.FactionalRoleNames.Value ? role.ToRoleFactionDisplayString(factionType) : role.ToDisplayString();
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

    [HarmonyPatch(nameof(ClientRoleExtensions.GetFactionColor))]
    public static bool Prefix(ref string __result, FactionType factionType)
    {
        __result = Fancy.Colors[Utils.FactionName(factionType, stoned: true).ToUpper()].Start;
        return false;
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

        if (Constants.IsBTOS2())
        {
            __result = role switch
            {
                Btos2Role.TrueAny => Utils.GetString("BTOS_ROLEBUCKET_100"),
                Btos2Role.Any => Utils.GetString("BTOS_ROLEBUCKET_101"),
                Btos2Role.RandomTown => $"{Utils.GetString("FANCY_BUCKETS_RANDOM")} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN"), town)}",
                Btos2Role.CommonTown => $"{Utils.GetString("FANCY_BUCKETS_COMMON")} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN"), town)}",
                Btos2Role.TownInvestigative => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN"), town)} {Utils.GetString("FANCY_BUCKETS_INVESTIGATIVE")}",
                Btos2Role.TownProtective => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN"), town)} {Utils.GetString("FANCY_BUCKETS_PROTECTIVE")}",
                Btos2Role.TownSupport => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN"), town)} {Utils.GetString("FANCY_BUCKETS_SUPPORT")}",
                Btos2Role.TownKilling => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN"), townLethal)} {Utils.GetString("FANCY_BUCKETS_KILLING")}",
                Btos2Role.TownPower => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN"), townMajor)} {Utils.GetString("FANCY_BUCKETS_POWER")}",
                Btos2Role.RandomCoven => !Constants.IsPandora()
                    ? $"{Utils.GetString("FANCY_BUCKETS_RANDOM")} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN"), coven)}"
                    : $"{Utils.GetString("FANCY_BUCKETS_RANDOM")} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN"), pandora)}",
                Btos2Role.CommonCoven => !Constants.IsPandora()
                    ? $"{Utils.GetString("FANCY_BUCKETS_COMMON")} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN"), coven)}"
                    : $"{Utils.GetString("FANCY_BUCKETS_COMMON")} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN"), pandora)}",
                Btos2Role.CovenPower => !Constants.IsPandora()
                    ? $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN"), covenMajor)} {Utils.GetString("FANCY_BUCKETS_POWER")}"
                    : $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN"), pandoraMajor)} {Utils.GetString("FANCY_BUCKETS_POWER")}",
                Btos2Role.CovenKilling => !Constants.IsPandora()
                    ? $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN"), covenLethal)} {Utils.GetString("FANCY_BUCKETS_KILLING")}"
                    : $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN"), pandoraLethal)} {Utils.GetString("FANCY_BUCKETS_KILLING")}",
                Btos2Role.CovenUtility => !Constants.IsPandora()
                    ? $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN"), coven)} {Utils.GetString("FANCY_BUCKETS_UTILITY")}"
                    : $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN"), pandora)} {Utils.GetString("FANCY_BUCKETS_UTILITY")}",
                Btos2Role.CovenDeception => !Constants.IsPandora()
                    ? $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN"), coven)} {Utils.GetString("FANCY_BUCKETS_DECEPTION")}"
                    : $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN"), pandora)} {Utils.GetString("FANCY_BUCKETS_DECEPTION")}",
                Btos2Role.RandomApocalypse => !Constants.IsPandora()
                    ? $"{Utils.GetString("FANCY_BUCKETS_RANDOM")} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_APOCALYPSE_BTOS"), apocalypse)}"
                    : $"{Utils.GetString("FANCY_BUCKETS_RANDOM")} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_APOCALYPSE_BTOS"), pandora)}",
                Btos2Role.NeutralKilling => !Constants.IsCompliance()
                    ? $"{Utils.GetString("FANCY_BUCKETS_NEUTRAL")} {Utils.GetString("FANCY_BUCKETS_KILLING")}"
                    : $"{Utils.GetString("FANCY_BUCKETS_RANDOM")} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COMPLIANCE"), compliance)}",
                Btos2Role.NeutralEvil => $"{Utils.GetString("FANCY_BUCKETS_NEUTRAL")} {Utils.GetString("FANCY_BUCKETS_EVIL")}",
                Btos2Role.NeutralPariah => $"{Utils.GetString("FANCY_BUCKETS_NEUTRAL")} {Utils.GetString("FANCY_BUCKETS_PARIAH")}",
                Btos2Role.NeutralSpecial => $"{Utils.GetString("FANCY_BUCKETS_NEUTRAL")} {Utils.GetString("FANCY_BUCKETS_SPECIAL")}",
                Btos2Role.RandomNeutral => $"{Utils.GetString("FANCY_BUCKETS_RANDOM")} {Utils.GetString("FANCY_BUCKETS_NEUTRAL")}",
                Btos2Role.CommonNeutral => $"{Utils.GetString("FANCY_BUCKETS_COMMON")} {Utils.GetString("FANCY_BUCKETS_NEUTRAL")}",
                _ => string.Empty,
            };
        }
        else
        {
            __result = role switch
            {
                Role.ANY => Utils.GetString("GUI_ROLE_LIST_BUCKET_ANY"),
                Role.RANDOM_TOWN => $"{Utils.GetString("FANCY_BUCKETS_RANDOM")} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN"), town)}",
                Role.COMMON_TOWN => $"{Utils.GetString("FANCY_BUCKETS_COMMON")} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN"), town)}",
                Role.TOWN_INVESTIGATIVE => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN"), town)} {Utils.GetString("FANCY_BUCKETS_INVESTIGATIVE")}",
                Role.TOWN_PROTECTIVE => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN"), town)} {Utils.GetString("FANCY_BUCKETS_PROTECTIVE")}",
                Role.TOWN_SUPPORT => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN"), town)} {Utils.GetString("FANCY_BUCKETS_SUPPORT")}",
                Role.TOWN_KILLING => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN"), townLethal)} {Utils.GetString("FANCY_BUCKETS_KILLING")}",
                Role.TOWN_POWER => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_TOWN"), townMajor)} {Utils.GetString("FANCY_BUCKETS_POWER")}",
                Role.RANDOM_COVEN => $"{Utils.GetString("FANCY_BUCKETS_RANDOM")} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN"), coven)}",
                Role.COMMON_COVEN => $"{Utils.GetString("FANCY_BUCKETS_COMMON")} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN"), coven)}",
                Role.COVEN_POWER => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN"), covenMajor)} {Utils.GetString("FANCY_BUCKETS_POWER")}",
                Role.COVEN_KILLING => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN"), covenLethal)} {Utils.GetString("FANCY_BUCKETS_KILLING")}",
                Role.COVEN_UTILITY => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN"), coven)} {Utils.GetString("FANCY_BUCKETS_UTILITY")}",
                Role.COVEN_DECEPTION => $"{Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_COVEN"), coven)} {Utils.GetString("FANCY_BUCKETS_DECEPTION")}",
                Role.NEUTRAL_APOCALYPSE => !Fancy.ReplaceNAwithRA.Value
                    ? $"{Utils.GetString("FANCY_BUCKETS_NEUTRAL")} {Utils.GetString("FANCY_BUCKETS_APOCALYPSE_VANILLA")}"
                    : $"{Utils.GetString("FANCY_BUCKETS_RANDOM")} {Utils.ApplyGradient(Utils.GetString("FANCY_BUCKETS_APOCALYPSE_BTOS"), apocalypse)}",
                Role.NEUTRAL_KILLING => $"{Utils.GetString("FANCY_BUCKETS_NEUTRAL")} {Utils.GetString("FANCY_BUCKETS_KILLING")}",
                Role.NEUTRAL_EVIL => $"{Utils.GetString("FANCY_BUCKETS_NEUTRAL")} {Utils.GetString("FANCY_BUCKETS_EVIL")}",
                Role.RANDOM_NEUTRAL => $"{Utils.GetString("FANCY_BUCKETS_RANDOM")} {Utils.GetString("FANCY_BUCKETS_NEUTRAL")}",
                _ => string.Empty,
            };
        }
    }
}