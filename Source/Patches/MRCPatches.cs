using Game.Characters;
using Home.HomeScene;
using Home.Shared;
using Home.Utils;
using Server.Shared.Extensions;
using Game;
using HarmonyLib;
using SML;
using TMPro;
using Mentions;
using Cinematics.Players;
using Server.Shared.Cinematics.Data;
using Server.Shared.Cinematics;

namespace FancyUI.Patches

{
   /* [HarmonyPatch(typeof(ClientRoleExtensions), nameof(ClientRoleExtensions.ToColorizedDisplayString), typeof(Role), typeof(FactionType))]
    public static class ColorizeDisplayStringPatch
    {
        [HarmonyPostfix]
        public static void Result(ref string __result, ref Role role, ref FactionType factionType)
        {
            if (!Fancy.FactionalRoleNames.Value) return;

            if (RoleExtensions.IsResolved(role) || role is Role.FAMINE or Role.DEATH or Role.PESTILENCE or Role.WAR)
            {
                // me when I'm Canadian and refuse to use "colour"
                __result = Utils.GetColorizedRoleName(role, factionType);
            }
        }
    } */

    /* [HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.UpdateTitle))]
    public static class PatchRoleCard
    {
        public static void Postfix(RoleCardPanel __instance)
        {
            string roleName = string.Empty;

            if (Fancy.FactionalRoleNames.Value)
            {
                roleName = Utils.GetColorizedRoleName(Pepper.GetMyRole(), Pepper.GetMyFaction());
            }
            else
            {
                roleName = Pepper.GetMyRole().ToDisplayString();
            }

            __instance.roleNameText.text = roleName;
        }
    }
    [HarmonyPatch(typeof(RoleCardPopupPanel), nameof(RoleCardPopupPanel.SetRole))]
    public static class RoleCardPopupPatches
    {
        public static void Postfix(ref Role role, RoleCardPopupPanel __instance) => __instance.roleNameText.text = ClientRoleExtensions.ToColorizedDisplayString(role);
    } */

    [HarmonyPatch(typeof(RoleCardPanel), nameof(RoleCardPanel.UpdateTitle))]
    public static class PatchRoleCard
    {
        public static void Postfix(RoleCardPanel __instance)
        {
            var component = __instance.GetComponent<GradientRoleColorController>();

            if (component != null)
                UObject.Destroy(component);

            __instance.gameObject.AddComponent<GradientRoleColorController>().__instance = __instance.rolecardBG;
            __instance.roleNameText.text = Pepper.GetMyRole().ToChangedDisplayString(Pepper.GetMyFaction(), Service.Game.Sim.simulation.observations.roleCardObservation.Data.modifier);
        }

        private static bool ConditionalCompliancePandora(FactionType originalFaction, FactionType currentFaction)
        {
            if (currentFaction == (FactionType)43)
            {
                return originalFaction == FactionType.COVEN || originalFaction == FactionType.APOCALYPSE;
            }
            if (originalFaction == (FactionType)43)
            {
                return currentFaction == FactionType.COVEN || currentFaction == FactionType.APOCALYPSE;
            }
            if (currentFaction == (FactionType)44)
            {
                return originalFaction == FactionType.SERIALKILLER || originalFaction == FactionType.ARSONIST || originalFaction == FactionType.WEREWOLF || originalFaction == FactionType.SHROUD;
            }
            if (originalFaction == (FactionType)44)
            {
                return currentFaction == FactionType.SERIALKILLER || currentFaction == FactionType.ARSONIST || currentFaction == FactionType.WEREWOLF || currentFaction == FactionType.SHROUD;
            }
            return originalFaction == currentFaction;
        }

        public static string ToChangedDisplayString(this Role role, FactionType faction, ROLE_MODIFIER modifier)
        {
            var text = "";
            var roleName = "";
            if (Fancy.FactionalRoleNames.Value) { roleName = Utils.ToRoleFactionDisplayString(role, faction); }
            else { roleName = role.ToDisplayString(); }

            if (faction.GetChangedGradient(role) != null)
                text = Utils.SetGradient(roleName, faction.GetChangedGradient(role));
            else
            {
                text = string.Concat(
                [
                    "<color=",
                    ClientRoleExtensions.GetFactionColor(faction),
                    ">",
                    roleName,
                    "</color>"
                ]);
            }

            var gradientTT = faction.GetChangedGradient(role);

            if (modifier == (ROLE_MODIFIER)2 && gradientTT != null)
            {
                text = text + "\n<size=85%>" + Utils.ApplyGradient($"({Fancy.TraitorLabel.Value})", gradientTT.Evaluate(0f), gradientTT.Evaluate(1f)) + "</size>";
            }
            else if (modifier == (ROLE_MODIFIER)10)
            {
                var gradient = ((FactionType)33).GetChangedGradient(role);
                text = text + "\n<size=85%>" + Utils.ApplyGradient($"({Fancy.RecruitLabel.Value})", gradient.Evaluate(0f), gradient.Evaluate(1f)) + "</size>";
            }
            else if ((Fancy.RoleCardFactionLabel.Value == FactionLabelOption.Mismatch && RoleExtensions.GetFaction(role) != faction) || (Fancy.RoleCardFactionLabel.Value == FactionLabelOption.Always) || (Fancy.RoleCardFactionLabel.Value == FactionLabelOption.Conditional && !ConditionalCompliancePandora(RoleExtensions.GetFaction(role), faction)))
            {
                var gradient2 = faction.GetChangedGradient(role);

                if (gradient2 != null)
                {
                    if (faction == (FactionType)44)
                    {
                        text = text + "\n<size=85%>" + Utils.ApplyThreeColorGradient("(" + faction.ToDisplayString() + ")", gradient2.Evaluate(0f), gradient2.Evaluate(0.5f),
                            gradient2.Evaluate(1f)) + "</size>";
                    }
                    else
                        text = text + "\n<size=85%>" + Utils.ApplyGradient("(" + faction.ToDisplayString() + ")", gradient2.Evaluate(0f), gradient2.Evaluate(1f)) + "</size>";

                    if (modifier == (ROLE_MODIFIER)1)
                    {
                        text = text + "\n<size=85%>" + Utils.ApplyGradient($"({Fancy.VIPLabel.Value})", gradientTT.Evaluate(0f), gradientTT.Evaluate(1f)) + "</size>";
                    }

                }
                else
                {
                    text = string.Concat(
                    [
                        text,
                        "\n<size=85%><color=",
                        ClientRoleExtensions.GetFactionColor(faction),
                        ">(",
                        faction.ToDisplayString(),
                        ")</color></size>"
                    ]);
                }
            }

            return text;
        }
    }

    [HarmonyPatch(typeof(RoleCardPopupPanel), nameof(RoleCardPopupPanel.SetRole))]
    public static class RoleCardPopupPatches
    {
        public static void Postfix(ref Role role, RoleCardPopupPanel __instance) => __instance.roleNameText.text = ClientRoleExtensions.ToColorizedDisplayString(role);
    }

    /* [HarmonyPatch(typeof(TosAbilityPanelListItem), nameof(TosAbilityPanelListItem.SetKnownRole))]
    public static class PlayerListPatch
    {
        public static bool Prefix(ref Role role, ref FactionType faction, TosAbilityPanelListItem __instance)
        {
            __instance.playerRole = role;

            if (role is not (0 or (Role)byte.MaxValue))
            {
                string roleName;

                if (Fancy.FactionalRoleNames.Value)
                {
                    roleName = Utils.GetColorizedRoleName(role, faction, true);
                }
                else
                {
                    roleName = $"({role.ToDisplayString()})";
                }

                __instance.playerRoleText.enableAutoSizing = false; // Remove when PlayerNotes+ fix is out
                __instance.playerRoleText.text = roleName;
                __instance.playerRoleText.gameObject.SetActive(true);
            }

            return false;
        }
    } */

    [HarmonyPatch(typeof(TosAbilityPanelListItem), nameof(TosAbilityPanelListItem.SetKnownRole))]
    public static class PlayerListPatch
    {
        public static bool Prefix(ref Role role, ref FactionType faction, TosAbilityPanelListItem __instance)
        {
            __instance.playerRole = role;
            var roleName = "";
            if (Fancy.FactionalRoleNames.Value) { roleName = Utils.ToRoleFactionDisplayString(role, faction); }
            else { roleName = role.ToDisplayString(); }

            var factionName = faction.ToDisplayString();

            if (role is not (0 or (Role)byte.MaxValue))
            {
                var gradient = faction.GetChangedGradient(role);

                if (gradient != null && role is not ((Role)254 or (Role)241))
                {
                    if (faction is ((FactionType)44) and not ((FactionType)33))
                    {
                        __instance.playerRoleText.text = Utils.ApplyThreeColorGradient("(" + roleName + ")", gradient.Evaluate(0f), gradient.Evaluate(0.5f),
                            gradient.Evaluate(1f));
                    }
                    else if (faction == (FactionType)33 && role != BetterTOS2.RolePlus.JACKAL)
                    {
                        Gradient jackalGradient = Btos2Faction.Jackal.GetChangedGradient(role);

                        __instance.playerRoleText.text = Utils.ApplyGradient("(" + roleName + ")", jackalGradient.Evaluate(0f), jackalGradient.Evaluate(1f));
                    }
                    else
                        __instance.playerRoleText.text = Utils.ApplyGradient("(" + roleName + ")", gradient.Evaluate(0f), gradient.Evaluate(1f));

                }
                else if (role is not ((Role)254 or (Role)241))
                {
                    __instance.playerRoleText.text = string.Concat(
                    [
                        "<color=",
                        ClientRoleExtensions.GetFactionColor(faction),
                        ">(",
                        roleName,
                        ")</color>"
                    ]);
                }
                else
                {
                    __instance.playerRoleText.text = string.Concat(
                    [
                        "<color=",
                        ClientRoleExtensions.GetFactionColor(RoleExtensions.GetFaction(role)),
                        ">(",
                        roleName,
                        ")</color>"
                    ]);
                }

                __instance.playerRoleText.gameObject.SetActive(true);
            }

            return false;
        }
    }

   /* [HarmonyPatch(typeof(TosCharacterNametag), nameof(TosCharacterNametag.ColouredName))]
    public static class TosCharacterNametagPatch
    {
        public static void Postfix(FactionType factionType, ref string __result, ref string theName, ref Role role)
        {
            if (Fancy.FactionalRoleNames.Value)
            {
                var nameText = Utils.GetColorizedText(theName, factionType);
                var roleText = Utils.GetColorizedRoleName(role, factionType, true);

                if (nameText == theName) // Stoned and Hidden are not checked here because gradients can be given to them via PlayerNotes+
                {
                    var color = factionType.GetFactionColor();
                    nameText = $"<color={color}>{theName}</color>";
                }

                __result = $"<size=36><sprite=\"RoleIcons\" name=\"Role{(int)role}\"></size>\n<size=24>{nameText}</size>\n<size=18>{roleText}</size>";
            }

            if (Constants.EnableIcons())
            {
                __result = __result.Replace("RoleIcons\"", $"RoleIcons ({Utils.FactionName(factionType, false)})\"");
            }

            if (Constants.IsBTOS2())
            {
                __result = __result.Replace("\"RoleIcons", "\"BTOSRoleIcons");
            }
        }
    } */

[HarmonyPatch(typeof(TosCharacterNametag), nameof(TosCharacterNametag.ColouredName))]
public static class TosCharacterNametagPatch
{
    public static void Postfix(TosCharacterNametag __instance, ref string theName, ref FactionType factionType, ref Role role, ref string __result)
    {
            var roleName = "";
            if (Fancy.FactionalRoleNames.Value) { roleName = Utils.ToRoleFactionDisplayString(role, factionType); }
            else { roleName = role.ToDisplayString(); }


        if (factionType.GetChangedGradient(role) != null && role is not (Role.STONED or Role.HIDDEN))
        {
            var gradient = factionType.GetChangedGradient(role);
            var gradientName = "";
            var gradientRole = "";

            if (factionType is ((FactionType)44) and not ((FactionType)33))
            {
                gradientName = Utils.ApplyThreeColorGradient(theName, gradient.Evaluate(0f), gradient.Evaluate(0.5f), gradient.Evaluate(1f));
                gradientRole = Utils.ApplyThreeColorGradient("(" + roleName + ")", gradient.Evaluate(0f), gradient.Evaluate(0.5f), gradient.Evaluate(1f));
            }
            else if (factionType == (FactionType)33 && role != Btos2Role.Jackal)
            {
                Gradient jackalGradient = Btos2Faction.Jackal.GetChangedGradient(role);
                gradientName = Utils.ApplyGradient(theName, jackalGradient.Evaluate(0f), jackalGradient.Evaluate(1f));
                gradientRole = Utils.ApplyGradient("(" + roleName + ")", gradient.Evaluate(0f), jackalGradient.Evaluate(1f));
            }
            else
            {
                gradientName = Utils.ApplyGradient(theName, gradient.Evaluate(0f), gradient.Evaluate(1f));
                gradientRole = Utils.ApplyGradient("(" + roleName + ")", gradient.Evaluate(0f), gradient.Evaluate(1f));
            }

            if (Constants.IsBTOS2())
                __result = $"<size=36><sprite=\"BTOSRoleIcons\" name=\"Role{(int)role}\"></size>\n<size=24>{gradientName}</size>\n<size=18>{gradientRole}</size>";
            else
                __result = $"<size=36><sprite=\"RoleIcons\" name=\"Role{(int)role}\"></size>\n<size=24>{gradientName}</size>\n<size=18>{gradientRole}</size>";

            if (Constants.EnableIcons())
            {
                __result = __result.Replace("RoleIcons\"", $"RoleIcons ({Utils.FactionName(factionType, false)})\"");
            }

        }
    }
}

    [HarmonyPatch(typeof(ClientRoleExtensions), nameof(ClientRoleExtensions.ToColorizedDisplayString), typeof(Role), typeof(FactionType))]
    public static class AddTTAndGradients
    {
        [HarmonyPostfix]
        public static void Result(ref string __result, ref Role role, ref FactionType factionType)
        {
            var newtext = "";

            if (__result.Contains("<color=#B545FF>(Traitor)"))
                __result = __result.Replace("<color=#B545FF>(Traitor)</color>", "<style=CovenColor>(Traitor)</style>");

            if (RoleExtensions.IsResolved(role) || role is Role.FAMINE or Role.DEATH or Role.PESTILENCE or Role.WAR)
            {
                var text = "";
                text = ClientRoleExtensions.ToDisplayString(role);
                
                if (Fancy.FactionalRoleNames.Value) { text = Utils.ToRoleFactionDisplayString(role, factionType); }
                else { text = ClientRoleExtensions.ToDisplayString(role); }


                if (factionType.GetChangedGradient(role) != null)
                    newtext = Utils.SetGradient(text, factionType.GetChangedGradient(role));
                else
                {
                    newtext = string.Concat(
                    [
                        "<color=",
                        ClientRoleExtensions.GetFactionColor(factionType),
                        ">",
                        text,
                        "</color>"
                    ]);
                }

                // DOES NOT WORK, SOMEONE LOOK AT THIS
                // if (RoleExtensions.GetFaction(role) != factionType && factionType != FactionType.NONE && Fancy.FactionNameNextToRole.Value)
                // if (factionType != FactionType.NONE) // TESTING PURPOSES
                {
                    if (factionType is not ((FactionType)33 or (FactionType)44))
                    {
                        if (factionType.GetChangedGradient(role) != null)
                        {
                            newtext += " " + Utils.ApplyGradient("(" + factionType.ToDisplayString() + ")", factionType.GetChangedGradient(role).Evaluate(0f),
                                factionType.GetChangedGradient(role).Evaluate(1f));
                        }
                        else
                            newtext += " " + "<color=" + ClientRoleExtensions.GetFactionColor(factionType) + ">(" + factionType.ToDisplayString() + ")</color>";
                    }
                    else if (factionType == (FactionType)33)
                    {
                        newtext += " " + Utils.ApplyGradient("(" + Fancy.RecruitLabel.Value + ")",
                            factionType.GetChangedGradient(role).Evaluate(0f), factionType.GetChangedGradient(role).Evaluate(1f));
                    }
                    else if (factionType == (FactionType)44)
                    {
                        newtext += " " + Utils.ApplyThreeColorGradient("(" + factionType.ToDisplayString() + ")", factionType.GetChangedGradient(role).Evaluate(0f),
                            factionType.GetChangedGradient(role).Evaluate(0.5f), factionType.GetChangedGradient(role).Evaluate(1f));
                    }

                } 

                __result = newtext;
            }
        }
    }

    [HarmonyPatch(typeof(HomeSceneController), nameof(HomeSceneController.HandleClickPlay))]
    public static class FixStyles
    {
        [HarmonyPostfix]
        public static void RefreshStyles()
        {
            var defaultStyleSheet = TMP_Settings.defaultStyleSheet;

            FieldInfo stylesField = AccessTools.Field(typeof(TMP_StyleSheet), "m_StyleList");
            if (stylesField == null)
            {
                return;
            }

            var styles = stylesField.GetValue(defaultStyleSheet) as List<TMP_Style>;
            if (styles == null)
            {
                return;
            }

            SetStyle(styles, "TownColor", Fancy.Colors["TOWN"].start);
            SetStyle(styles, "CovenColor", Fancy.Colors["COVEN"].start);
            SetStyle(styles, "ApocalypseColor", Fancy.Colors["APOCALYPSE"].start);
            SetStyle(styles, "SerialKillerColor", Fancy.Colors["SERIALKILLER"].start);
            SetStyle(styles, "ArsonistColor", Fancy.Colors["ARSONIST"].start);
            SetStyle(styles, "WerewolfColor", Fancy.Colors["WEREWOLF"].start);
            SetStyle(styles, "ShroudColor", Fancy.Colors["SHROUD"].start);
            SetStyle(styles, "ExecutionerColor", Fancy.Colors["EXECUTIONER"].start);
            SetStyle(styles, "JesterColor", Fancy.Colors["JESTER"].start);
            SetStyle(styles, "PirateColor", Fancy.Colors["PIRATE"].start);
            SetStyle(styles, "DoomsayerColor", Fancy.Colors["DOOMSAYER"].start);
            SetStyle(styles, "VampireColor", Fancy.Colors["VAMPIRE"].start);
            SetStyle(styles, "CursedSoulColor", Fancy.Colors["CURSEDSOUL"].start);
            SetStyle(styles, "NeutralColor", Fancy.Colors["NEUTRAL"].start);

            defaultStyleSheet.RefreshStyles();
        }

        private static void SetStyle(List<TMP_Style> styles, string styleName, string colorValue)
        {
            var style = styles.Find(s => s.name == styleName);
            if (style == null) return;

            var openingDefField = typeof(TMP_Style).GetField("m_OpeningDefinition", BindingFlags.Instance | BindingFlags.NonPublic);
            if (openingDefField != null)
            {
                openingDefField.SetValue(style, $"<color={colorValue}>");
            }
        }   
    }

[HarmonyPatch(typeof(ClientRoleExtensions), nameof(ClientRoleExtensions.ToColorizedDisplayString), typeof(Role), typeof(FactionType))]
public static class AddChangedConversionTags
{
    public static void Postfix(ref string __result, ref Role role, ref FactionType factionType)
    {
        if (role.IsResolved() || role is Role.FAMINE or Role.DEATH or Role.PESTILENCE or Role.WAR)
        {
            var text = "";
            if (Fancy.FactionalRoleNames.Value) { text = Utils.ToRoleFactionDisplayString(role, factionType); }
            else { text = role.ToDisplayString(); }


            if (factionType.GetChangedGradient(role) != null)
                __result = Utils.SetGradient(text, factionType.GetChangedGradient(role));
            else
                __result = $"<color={factionType.GetFactionColor()}>{text}</color>";
        }
    }
}

[HarmonyPatch(typeof(MentionsProvider), nameof(MentionsProvider.DecodeSpeaker))]
public static class FancyChatExperimentalBTOS2
{
    public static List<int> ExcludedIds = [50, 69, 70, 71];

    public static bool Prefix(MentionsProvider __instance, ref string __result, string encodedText, int position, bool isAlive)
    {
        var text = Service.Home.UserService.Settings.ChatNameColor switch
        {
            1 => "B0B0B0",
            2 => "CC009E",
            _ => "FCCE3B",
        };
        var text2 = encodedText;

        if (!ExcludedIds.Contains(position))
        {
            if (isAlive)
            {
                var isRecruited = Service.Game.Sim.simulation.observations.playerEffects.Any(x => x.Data.effects.Contains((EffectType)100) && x.Data.playerPosition == position);

                if (Utils.GetRoleInfo(position, out var playerInfo))
                {
                    if (playerInfo.Item2.GetChangedGradient(playerInfo.Item1) != null)
                    {
                        var gradient = playerInfo.Item2.GetChangedGradient(playerInfo.Item1);

                        if (isRecruited)
                            gradient = ((FactionType)33).GetChangedGradient(playerInfo.Item1);

                        var text3 = "";

                        if (playerInfo.Item2 == ((FactionType)44))
                        {
                            text3 = Utils.ApplyThreeColorGradient(Pepper.GetDiscussionPlayerByPosition(position).gameName + ":", gradient.Evaluate(0f),
                                gradient.Evaluate(0.5f), gradient.Evaluate(1f));
                        }
                        else
                            text3 = Utils.ApplyGradient(Pepper.GetDiscussionPlayerByPosition(position).gameName + ":", gradient.Evaluate(0f), gradient.Evaluate(1f));

                        text2 = text2.Replace(string.Concat(
                        [
                            "<color=#",
                            ColorUtility.ToHtmlStringRGB(Pepper.GetDiscussionPlayerRoleColor(position)),
                            ">",
                            Pepper.GetDiscussionPlayerByPosition(position).gameName,
                            ":"
                        ]), text3);
                    }
                    else if (isRecruited)
                    {
                        var gradient2 = ((FactionType)33).GetChangedGradient(playerInfo.Item1);
                        var text4 = Utils.ApplyGradient(Pepper.GetDiscussionPlayerByPosition(position).gameName + ":", gradient2.Evaluate(0f), gradient2.Evaluate(1f));
                        text2 = text2.Replace(string.Concat(
                        [
                            "<color=#",
                            ColorUtility.ToHtmlStringRGB(Pepper.GetDiscussionPlayerRoleColor(position)),
                            ">",
                            Pepper.GetDiscussionPlayerByPosition(position).gameName,
                            ":"
                        ]), text4);
                    }
                    else 
                    {
                        var text5 = ColorUtility.ToHtmlStringRGB(Utils.GetPlayerRoleColor(position));
                        text2 = text2.Replace("<color=#" + text + ">", "<color=#" + text5 + ">");
                    }
                }
                else if (isRecruited)
                {
                    var gradient3 = ((FactionType)33).GetChangedGradient(playerInfo.Item1);
                    var text6 = Utils.ApplyGradient(Pepper.GetDiscussionPlayerByPosition(position).gameName + ":", gradient3.Evaluate(0f), gradient3.Evaluate(1f));
                    text2 = text2.Replace(string.Concat(
                    [
                        "<color=#",
                        text,
                        ">",
                        Pepper.GetDiscussionPlayerByPosition(position).gameName,
                        ":"
                    ]), text6);
                }
            }
        }

        __result = __instance.ProcessSpeakerName(text2, position, isAlive);
        return false;
    }

}

[HarmonyPatch(typeof(ClientRoleExtensions), nameof(ClientRoleExtensions.GetFactionColor))]
public static class SwapColor
{
    [HarmonyPostfix]
    public static void Swap(ref string __result, ref FactionType factionType)
    {
        if (Fancy.Colors != null)
        {
            var faction = (int)factionType;
            __result = faction switch
            {
                1 => Fancy.Colors["TOWN"].start,
                2 => Fancy.Colors["COVEN"].start,
                3 => Fancy.Colors["SERIALKILLER"].start,
                4 => Fancy.Colors["ARSONIST"].start,
                5 => Fancy.Colors["WEREWOLF"].start,
                6 => Fancy.Colors["SHROUD"].start,
                7 => Fancy.Colors["APOCALYPSE"].start,
                8 => Fancy.Colors["EXECUTIONER"].start,
                9 => Fancy.Colors["JESTER"].start,
                10 => Fancy.Colors["PIRATE"].start,
                11 => Fancy.Colors["DOOMSAYER"].start,
                12 => Fancy.Colors["VAMPIRE"].start,
                13 => Fancy.Colors["CURSEDSOUL"].start,
                33 => Fancy.Colors["JACKAL"].start,
                34 => Fancy.Colors["FROGS"].start,
                35 => Fancy.Colors["LIONS"].start,
                36 => Fancy.Colors["HAWKS"].start,
                38 => Fancy.Colors["JUDGE"].start,
                39 => Fancy.Colors["AUDITOR"].start,
                40 => Fancy.Colors["INQUISITOR"].start,
                41 => Fancy.Colors["STARSPAWN"].start,
                42 => Fancy.Colors["EGOTIST"].start,
                43 => Fancy.Colors["PANDORA"].start,
                44 => Fancy.Colors["COMPLIANCE"].start,
                250 => Fancy.Colors["LOVERS"].start,
                _ => Fancy.Colors["STONED_HIDDEN"].start,
            };
        }
    }
}



public static class GetChangedGradients
{
    public static Gradient GetChangedGradient(this FactionType faction, Role role)
    {
        var gradient = new Gradient();
        var array = new GradientColorKey[2];
        var array2 = new GradientAlphaKey[2];

        Gradient result;
        if (Fancy.MajorColors.Value && (role.GetSubAlignment() == SubAlignment.POWER || role == Role.FAMINE || role == Role.WAR || role == Role.PESTILENCE || role == Role.DEATH))
        {
            switch (faction)
            {
                case FactionType.TOWN:
                    array[0] = new(Fancy.Colors["TOWN"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["TOWN"].major.ToColor(), 1f);
                    goto setmajor;

                case FactionType.COVEN:
                    array[0] = new(Fancy.Colors["COVEN"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["COVEN"].major.ToColor(), 1f);
                    goto setmajor;

                case FactionType.APOCALYPSE:
                    array[0] = new(Fancy.Colors["APOCALYPSE"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["APOCALYPSE"].major.ToColor(), 1f);
                    goto setmajor;

                case FactionType.SERIALKILLER:
                    array[0] = new(Fancy.Colors["SERIALKILLER"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["SERIALKILLER"].major.ToColor(), 1f);
                    goto setmajor;

                case FactionType.ARSONIST:
                    array[0] = new(Fancy.Colors["ARSONIST"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["ARSONIST"].major.ToColor(), 1f);
                    goto setmajor;

                case FactionType.WEREWOLF:
                    array[0] = new(Fancy.Colors["WEREWOLF"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["WEREWOLF"].major.ToColor(), 1f);
                    goto setmajor;

                case FactionType.SHROUD:
                    array[0] = new(Fancy.Colors["SHROUD"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["SHROUD"].major.ToColor(), 1f);
                    goto setmajor;

                case FactionType.EXECUTIONER:
                    array[0] = new(Fancy.Colors["EXECUTIONER"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["EXECUTIONER"].end.ToColor(), 1f);
                    goto setmajor;

                case FactionType.JESTER:
                    array[0] = new(Fancy.Colors["JESTER"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["JESTER"].end.ToColor(), 1f);
                    goto setmajor;

                case (FactionType)40:
                    array[0] = new(Fancy.Colors["INQUISITOR"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["INQUISITOR"].end.ToColor(), 1f);
                    goto setmajor;

                case FactionType.PIRATE:
                    array[0] = new(Fancy.Colors["PIRATE"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["PIRATE"].end.ToColor(), 1f);
                    goto setmajor;

                case FactionType.DOOMSAYER:
                    array[0] = new(Fancy.Colors["DOOMSAYER"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["DOOMSAYER"].end.ToColor(), 1f);
                    goto setmajor;

                case FactionType.VAMPIRE:
                    array[0] = new(Fancy.Colors["VAMPIRE"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["VAMPIRE"].major.ToColor(), 1f);
                    goto setmajor;

                case FactionType.CURSED_SOUL:
                    array[0] = new(Fancy.Colors["CURSEDSOUL"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["CURSEDSOUL"].major.ToColor(), 1f);
                    goto setmajor;

                case (FactionType)33:
                    switch (Fancy.RecruitEndingColor.Value)
                    {
                        case RecruitEndType.JackalEnd:
                            array[0] = new(Fancy.Colors["JACKAL"].start.ToColor(), 0f);
                            array[1] = new(Fancy.Colors["JACKAL"].major.ToColor(), 1f);
                            goto setmajor;

                        case RecruitEndType.FactionStart:
                            array[0] = new(Fancy.Colors["JACKAL"].start.ToColor(), 0f);
                            switch (role.GetFaction())
                            {
                                case FactionType.TOWN:
                                    array[1] = new(Fancy.Colors["TOWN"].start.ToColor(), 1f);
                                    break;
                                case FactionType.COVEN:
                                    array[1] = new(Fancy.Colors["COVEN"].start.ToColor(), 1f);
                                    break;
                                case FactionType.SERIALKILLER:
                                    array[1] = new(Fancy.Colors["SERIALKILLER"].start.ToColor(), 1f);
                                    break;
                                case FactionType.ARSONIST:
                                    array[1] = new(Fancy.Colors["ARSONIST"].start.ToColor(), 1f);
                                    break;
                                case FactionType.WEREWOLF:
                                    array[1] = new(Fancy.Colors["WEREWOLF"].start.ToColor(), 1f);
                                    break;
                                case FactionType.SHROUD:
                                    array[1] = new(Fancy.Colors["SHROUD"].start.ToColor(), 1f);
                                    break;
                                case FactionType.APOCALYPSE:
                                    array[1] = new(Fancy.Colors["APOCALYPSE"].start.ToColor(), 1f);
                                    break;
                                case FactionType.EXECUTIONER:
                                    array[1] = new(Fancy.Colors["EXECUTIONER"].start.ToColor(), 1f);
                                    break;
                                case FactionType.JESTER:
                                    array[1] = new(Fancy.Colors["JESTER"].start.ToColor(), 1f);
                                    break;
                                case FactionType.PIRATE:
                                    array[1] = new(Fancy.Colors["PIRATE"].start.ToColor(), 1f);
                                    break;
                                case FactionType.DOOMSAYER:
                                    array[1] = new(Fancy.Colors["DOOMSAYER"].start.ToColor(), 1f);
                                    break;
                                case FactionType.VAMPIRE:
                                    array[1] = new(Fancy.Colors["VAMPIRE"].start.ToColor(), 1f);
                                    break;
                                case FactionType.CURSED_SOUL:
                                    array[1] = new(Fancy.Colors["CURSEDSOUL"].start.ToColor(), 1f);
                                    break;
                                 case (FactionType)38:
                                    array[1] = new(Fancy.Colors["JUDGE"].start.ToColor(), 1f);
                                    break;
                                case (FactionType)39:
                                    array[1] = new(Fancy.Colors["AUDITOR"].start.ToColor(), 1f);
                                    break;
                                case (FactionType)40:
                                    array[1] = new(Fancy.Colors["INQUISITOR"].start.ToColor(), 1f);
                                    break;
                                case (FactionType)41:
                                    array[1] = new(Fancy.Colors["STARSPAWN"].start.ToColor(), 1f);
                                    break;
                               default:
                                    array[1] = new(Fancy.Colors["JACKAL"].major.ToColor(), 1f);
                                    break;
                            }
                            goto setmajor;

                        case RecruitEndType.FactionEnd: 
                            array[0] = new(Fancy.Colors["JACKAL"].start.ToColor(), 1f);
                            switch (role.GetFaction())
                            {
                                case FactionType.TOWN:
                                    array[1] = new(Fancy.Colors["TOWN"].major.ToColor(), 1f);
                                    break;
                                case FactionType.COVEN:
                                    array[1] = new(Fancy.Colors["COVEN"].major.ToColor(), 1f);
                                    break;
                                case FactionType.SERIALKILLER:
                                    array[1] = new(Fancy.Colors["SERIALKILLER"].major.ToColor(), 1f);
                                    break;
                                case FactionType.ARSONIST:
                                    array[1] = new(Fancy.Colors["ARSONIST"].major.ToColor(), 1f);
                                    break;
                                case FactionType.WEREWOLF:
                                    array[1] = new(Fancy.Colors["WEREWOLF"].major.ToColor(), 1f);
                                    break;
                                case FactionType.SHROUD:
                                    array[1] = new(Fancy.Colors["SHROUD"].major.ToColor(), 1f);
                                    break;
                                case FactionType.APOCALYPSE:
                                    array[1] = new(Fancy.Colors["APOCALYPSE"].major.ToColor(), 1f);
                                    break;
                                case FactionType.EXECUTIONER:
                                    array[1] = new(Fancy.Colors["EXECUTIONER"].end.ToColor(), 1f);
                                    break;
                                case FactionType.JESTER:
                                    array[1] = new(Fancy.Colors["JESTER"].end.ToColor(), 1f);
                                    break;
                                case FactionType.PIRATE:
                                    array[1] = new(Fancy.Colors["PIRATE"].end.ToColor(), 1f);
                                    break;
                                case FactionType.DOOMSAYER:
                                    array[1] = new(Fancy.Colors["DOOMSAYER"].end.ToColor(), 1f);
                                    break;
                                case FactionType.VAMPIRE:
                                    array[1] = new(Fancy.Colors["VAMPIRE"].major.ToColor(), 1f);
                                    break;
                                case FactionType.CURSED_SOUL:
                                    array[1] = new(Fancy.Colors["CURSEDSOUL"].major.ToColor(), 1f);
                                    break;
                                 case (FactionType)38:
                                    array[1] = new(Fancy.Colors["JUDGE"].end.ToColor(), 1f);
                                    break;
                                case (FactionType)39:
                                    array[1] = new(Fancy.Colors["AUDITOR"].end.ToColor(), 1f);
                                    break;
                                case (FactionType)40:
                                    array[1] = new(Fancy.Colors["INQUISITOR"].end.ToColor(), 1f);
                                    break;
                                case (FactionType)41:
                                    array[1] = new(Fancy.Colors["STARSPAWN"].end.ToColor(), 1f);
                                    break;
                               default:
                                    array[1] = new(Fancy.Colors["JACKAL"].major.ToColor(), 1f);
                                    break;
                            }
                            goto setmajor;

                        default:
                            array[0] = new(Fancy.Colors["JACKAL"].start.ToColor(), 0f);
                            array[1] = new(Fancy.Colors["JACKAL"].major.ToColor(), 1f);
                            goto setmajor;
                    }

                case (FactionType)38:
                    array[0] = new(Fancy.Colors["JUDGE"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["JUDGE"].end.ToColor(), 1f);
                    goto setmajor;

                case (FactionType)39:
                    array[0] = new(Fancy.Colors["AUDITOR"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["AUDITOR"].end.ToColor(), 1f);
                    goto setmajor;

                case (FactionType)41:
                    array[0] = new(Fancy.Colors["STARSPAWN"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["STARSPAWN"].end.ToColor(), 1f);
                    goto setmajor;

                case (FactionType)42:
                    array[0] = new(Fancy.Colors["EGOTIST"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["EGOTIST"].major.ToColor(), 1f);
                    goto setmajor;

                case (FactionType)43:
                    array[0] = new(Fancy.Colors["PANDORA"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["PANDORA"].major.ToColor(), 1f);
                    goto setmajor;

                case (FactionType)34:
                    array[0] = new(Fancy.Colors["FROGS"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["FROGS"].major.ToColor(), 1f);
                    goto setmajor;

                case (FactionType)35:
                    array[0] = new(Fancy.Colors["LIONS"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["LIONS"].major.ToColor(), 1f);
                    goto setmajor;

                case (FactionType)36:
                    array[0] = new(Fancy.Colors["HAWKS"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["HAWKS"].major.ToColor(), 1f);
                    goto setmajor;

                case (FactionType)44:
                    array =
                    [
                        new(Fancy.Colors["COMPLIANCE"].start.ToColor(), 0f),
                        new(Fancy.Colors["COMPLIANCE"].middle.ToColor(), 0.5f),
                        new(Fancy.Colors["COMPLIANCE"].major.ToColor(), 1f)
                    ];
                    goto setmajor;

            }
            return null;


            setmajor:
                array2[0] = new(1f, 0f);
                array2[1] = new(1f, 1f);
                gradient.SetKeys(array, array2);
				result = gradient;
        }
        else
        {
            switch (faction)
            {
                case FactionType.TOWN:
                    array[0] = new(Fancy.Colors["TOWN"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["TOWN"].end.ToColor(), 1f);
                    goto setgradient;

                case FactionType.COVEN:
                    array[0] = new(Fancy.Colors["COVEN"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["COVEN"].end.ToColor(), 1f);
                    goto setgradient;

                case FactionType.APOCALYPSE:
                    array[0] = new(Fancy.Colors["APOCALYPSE"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["APOCALYPSE"].end.ToColor(), 1f);
                    goto setgradient;

                case FactionType.SERIALKILLER:
                    array[0] = new(Fancy.Colors["SERIALKILLER"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["SERIALKILLER"].end.ToColor(), 1f);
                    goto setgradient;

                case FactionType.ARSONIST:
                    array[0] = new(Fancy.Colors["ARSONIST"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["ARSONIST"].end.ToColor(), 1f);
                    goto setgradient;

                case FactionType.WEREWOLF:
                    array[0] = new(Fancy.Colors["WEREWOLF"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["WEREWOLF"].end.ToColor(), 1f);
                    goto setgradient;

                case FactionType.SHROUD:
                    array[0] = new(Fancy.Colors["SHROUD"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["SHROUD"].end.ToColor(), 1f);
                    goto setgradient;

                case FactionType.EXECUTIONER:
                    array[0] = new(Fancy.Colors["EXECUTIONER"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["EXECUTIONER"].end.ToColor(), 1f);
                    goto setgradient;

                case FactionType.JESTER:
                    array[0] = new(Fancy.Colors["JESTER"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["JESTER"].end.ToColor(), 1f);
                    goto setgradient;

                case (FactionType)40:
                    array[0] = new(Fancy.Colors["INQUISITOR"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["INQUISITOR"].end.ToColor(), 1f);
                    goto setgradient;

                case FactionType.PIRATE:
                    array[0] = new(Fancy.Colors["PIRATE"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["PIRATE"].end.ToColor(), 1f);
                    goto setgradient;

                case FactionType.DOOMSAYER:
                    array[0] = new(Fancy.Colors["DOOMSAYER"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["DOOMSAYER"].end.ToColor(), 1f);
                    goto setgradient;

                case FactionType.VAMPIRE:
                    array[0] = new(Fancy.Colors["VAMPIRE"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["VAMPIRE"].end.ToColor(), 1f);
                    goto setgradient;

                case FactionType.CURSED_SOUL:
                    array[0] = new(Fancy.Colors["CURSEDSOUL"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["CURSEDSOUL"].end.ToColor(), 1f);
                    goto setgradient;

                case (FactionType)33:
                    switch (Fancy.RecruitEndingColor.Value)
                    {
                        case RecruitEndType.JackalEnd:
                            array[0] = new(Fancy.Colors["JACKAL"].start.ToColor(), 0f);
                            array[1] = new(Fancy.Colors["JACKAL"].end.ToColor(), 1f);
                            goto setgradient;

                        case RecruitEndType.FactionStart:
                            array[0] = new(Fancy.Colors["JACKAL"].start.ToColor(), 0f);
                            switch (role.GetFaction())
                            {
                                case FactionType.TOWN:
                                    array[1] = new(Fancy.Colors["TOWN"].start.ToColor(), 1f);
                                    break;
                                case FactionType.COVEN:
                                    array[1] = new(Fancy.Colors["COVEN"].start.ToColor(), 1f);
                                    break;
                                case FactionType.SERIALKILLER:
                                    array[1] = new(Fancy.Colors["SERIALKILLER"].start.ToColor(), 1f);
                                    break;
                                case FactionType.ARSONIST:
                                    array[1] = new(Fancy.Colors["ARSONIST"].start.ToColor(), 1f);
                                    break;
                                case FactionType.WEREWOLF:
                                    array[1] = new(Fancy.Colors["WEREWOLF"].start.ToColor(), 1f);
                                    break;
                                case FactionType.SHROUD:
                                    array[1] = new(Fancy.Colors["SHROUD"].start.ToColor(), 1f);
                                    break;
                                case FactionType.APOCALYPSE:
                                    array[1] = new(Fancy.Colors["APOCALYPSE"].start.ToColor(), 1f);
                                    break;
                                case FactionType.EXECUTIONER:
                                    array[1] = new(Fancy.Colors["EXECUTIONER"].start.ToColor(), 1f);
                                    break;
                                case FactionType.JESTER:
                                    array[1] = new(Fancy.Colors["JESTER"].start.ToColor(), 1f);
                                    break;
                                case FactionType.PIRATE:
                                    array[1] = new(Fancy.Colors["PIRATE"].start.ToColor(), 1f);
                                    break;
                                case FactionType.DOOMSAYER:
                                    array[1] = new(Fancy.Colors["DOOMSAYER"].start.ToColor(), 1f);
                                    break;
                                case FactionType.VAMPIRE:
                                    array[1] = new(Fancy.Colors["VAMPIRE"].start.ToColor(), 1f);
                                    break;
                                case FactionType.CURSED_SOUL:
                                    array[1] = new(Fancy.Colors["CURSEDSOUL"].start.ToColor(), 1f);
                                    break;
                                 case (FactionType)38:
                                    array[1] = new(Fancy.Colors["JUDGE"].start.ToColor(), 1f);
                                    break;
                                case (FactionType)39:
                                    array[1] = new(Fancy.Colors["AUDITOR"].start.ToColor(), 1f);
                                    break;
                                case (FactionType)40:
                                    array[1] = new(Fancy.Colors["INQUISITOR"].start.ToColor(), 1f);
                                    break;
                                case (FactionType)41:
                                    array[1] = new(Fancy.Colors["STARSPAWN"].start.ToColor(), 1f);
                                    break;
                               default:
                                    array[1] = new(Fancy.Colors["JACKAL"].end.ToColor(), 1f);
                                    break;
                            }
                            goto setgradient;

                        case RecruitEndType.FactionEnd: 
                            array[0] = new(Fancy.Colors["JACKAL"].start.ToColor(), 1f);
                            switch (role.GetFaction())
                            {
                                case FactionType.TOWN:
                                    array[1] = new(Fancy.Colors["TOWN"].end.ToColor(), 1f);
                                    break;
                                case FactionType.COVEN:
                                    array[1] = new(Fancy.Colors["COVEN"].end.ToColor(), 1f);
                                    break;
                                case FactionType.SERIALKILLER:
                                    array[1] = new(Fancy.Colors["SERIALKILLER"].end.ToColor(), 1f);
                                    break;
                                case FactionType.ARSONIST:
                                    array[1] = new(Fancy.Colors["ARSONIST"].end.ToColor(), 1f);
                                    break;
                                case FactionType.WEREWOLF:
                                    array[1] = new(Fancy.Colors["WEREWOLF"].end.ToColor(), 1f);
                                    break;
                                case FactionType.SHROUD:
                                    array[1] = new(Fancy.Colors["SHROUD"].end.ToColor(), 1f);
                                    break;
                                case FactionType.APOCALYPSE:
                                    array[1] = new(Fancy.Colors["APOCALYPSE"].end.ToColor(), 1f);
                                    break;
                                case FactionType.EXECUTIONER:
                                    array[1] = new(Fancy.Colors["EXECUTIONER"].end.ToColor(), 1f);
                                    break;
                                case FactionType.JESTER:
                                    array[1] = new(Fancy.Colors["JESTER"].end.ToColor(), 1f);
                                    break;
                                case FactionType.PIRATE:
                                    array[1] = new(Fancy.Colors["PIRATE"].end.ToColor(), 1f);
                                    break;
                                case FactionType.DOOMSAYER:
                                    array[1] = new(Fancy.Colors["DOOMSAYER"].end.ToColor(), 1f);
                                    break;
                                case FactionType.VAMPIRE:
                                    array[1] = new(Fancy.Colors["VAMPIRE"].end.ToColor(), 1f);
                                    break;
                                case FactionType.CURSED_SOUL:
                                    array[1] = new(Fancy.Colors["CURSEDSOUL"].end.ToColor(), 1f);
                                    break;
                                 case (FactionType)38:
                                    array[1] = new(Fancy.Colors["JUDGE"].end.ToColor(), 1f);
                                    break;
                                case (FactionType)39:
                                    array[1] = new(Fancy.Colors["AUDITOR"].end.ToColor(), 1f);
                                    break;
                                case (FactionType)40:
                                    array[1] = new(Fancy.Colors["INQUISITOR"].end.ToColor(), 1f);
                                    break;
                                case (FactionType)41:
                                    array[1] = new(Fancy.Colors["STARSPAWN"].end.ToColor(), 1f);
                                    break;
                               default:
                                    array[1] = new(Fancy.Colors["JACKAL"].end.ToColor(), 1f);
                                    break;
                            }
                            goto setgradient;

                        default:
                            array[0] = new(Fancy.Colors["JACKAL"].start.ToColor(), 0f);
                            array[1] = new(Fancy.Colors["JACKAL"].end.ToColor(), 1f);
                            goto setgradient;
                    }

                case (FactionType)38:
                    array[0] = new(Fancy.Colors["JUDGE"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["JUDGE"].end.ToColor(), 1f);
                    goto setgradient;

                case (FactionType)39:
                    array[0] = new(Fancy.Colors["AUDITOR"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["AUDITOR"].end.ToColor(), 1f);
                    goto setgradient;

                case (FactionType)41:
                    array[0] = new(Fancy.Colors["STARSPAWN"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["STARSPAWN"].end.ToColor(), 1f);
                    goto setgradient;

                case (FactionType)42:
                    array[0] = new(Fancy.Colors["EGOTIST"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["EGOTIST"].end.ToColor(), 1f);
                    goto setgradient;

                case (FactionType)43:
                    array[0] = new(Fancy.Colors["PANDORA"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["PANDORA"].end.ToColor(), 1f);
                    goto setgradient;

                case (FactionType)34:
                    array[0] = new(Fancy.Colors["FROGS"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["FROGS"].end.ToColor(), 1f);
                    goto setgradient;

                case (FactionType)35:
                    array[0] = new(Fancy.Colors["LIONS"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["LIONS"].end.ToColor(), 1f);
                    goto setgradient;

                case (FactionType)36:
                    array[0] = new(Fancy.Colors["HAWKS"].start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["HAWKS"].end.ToColor(), 1f);
                    goto setgradient;

                case (FactionType)44:
                    array =
                    [
                        new(Fancy.Colors["COMPLIANCE"].start.ToColor(), 0f),
                        new(Fancy.Colors["COMPLIANCE"].middle.ToColor(), 0.5f),
                        new(Fancy.Colors["COMPLIANCE"].end.ToColor(), 1f)
                    ];
                    goto setgradient;

            }
            return null;


            setgradient:
                array2[0] = new(1f, 0f);
                array2[1] = new(1f, 1f);
                gradient.SetKeys(array, array2);
				result = gradient;

        }

        return result;
    }
}

[HarmonyPatch(typeof(MentionsProvider), nameof(MentionsProvider.ProcessSpeakerName))]
public static class PatchJudge
{
    public static void Postfix(string encodedText, int position, ref string __result)
    {
        if (Constants.IsBTOS2())
        {
            if (position == 70)
            {
                __result = "<link=\"r57\"><sprite=\"BTOSRoleIcons\" name=\"Role57\"><indent=1.1em><b>" + Utils.ApplyGradient(Fancy.CourtLabel.Value, Fancy.Colors["JUDGE"].start.ToColor(), Fancy.Colors["JUDGE"].end.ToColor()) + ":" +
                    "</b> </link>" + encodedText.Replace("????: </color>", "").Replace("white", "#FFFF00");
            }
            else if (position == 69)
                __result = encodedText.Replace("????:", $"<sprite=\"BTOSRoleIcons\" name=\"Role16\"> {Fancy.JuryLabel.Value}:");
            else if (position == 71)
            {
                __result = "<link=\"r46\"><sprite=\"BTOSRoleIcons\" name=\"Role46\"><indent=1.1em><b>" + Utils.ApplyGradient(Fancy.PirateLabel.Value, Fancy.Colors["PIRATE"].start.ToColor(), Fancy.Colors["PIRATE"].end.ToColor()) + ":</b> </link>" + encodedText.Replace("????: </color>", "").Replace("white", "#ECC23E");
            }
        }
    }
}

[HarmonyPatch(typeof(FactionWinsCinematicPlayer), nameof(FactionWinsCinematicPlayer.Init))]
public static class PatchDefaultWinScreens
{
    public static void Postfix(FactionWinsCinematicPlayer __instance, ref ICinematicData cinematicData)
    {
        __instance.elapsedDuration = 0f;
        Debug.Log(string.Format("FactionWinsCinematicPlayer current phase at start = {0}", Pepper.GetGamePhase()));
        __instance.cinematicData = cinematicData as FactionWinsCinematicData;
        var winTimeByFaction = CinematicFactionWinsTimes.GetWinTimeByFaction(__instance.cinematicData.winningFaction);
        __instance.totalDuration = winTimeByFaction;
        __instance.callbackTimers.Clear();
        var spawnedCharacters = Service.Game.Cast.GetSpawnedCharacters();

        if (spawnedCharacters == null)
        {
            Debug.LogError("spawnedPlayers is null in GetCrowd()");
            return;
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
            Service.Home.AudioService.PlayMusic("Audio/Music/TownVictory.wav", false, AudioController.AudioChannel.Cinematic, true);
            __instance.evilProp.SetActive(false);
            __instance.goodProp.SetActive(true);
            __instance.m_Animator.SetInteger("State", 1);
        }
        else
        {
            Service.Home.AudioService.PlayMusic("Audio/Music/CovenVictory.wav", false, AudioController.AudioChannel.Cinematic, true);
            __instance.evilProp.SetActive(true);
            __instance.goodProp.SetActive(false);
            __instance.m_Animator.SetInteger("State", 2);
        }

        var text = string.Format("GUI_WINNERS_ARE_{0}", (int)winningFaction);
        var text2 = __instance.l10n(text);
        string gradientText;

        if (winningFaction.GetChangedGradient(Role.NONE) != null)
        {
            var gradient = winningFaction.GetChangedGradient(Role.NONE);
            __instance.leftImage.color = Utils.GetFactionStartingColor(winningFaction);
            __instance.rightImage.color = Utils.GetFactionEndingColor(winningFaction);

            if (winningFaction == (FactionType)44)
                gradientText = Utils.ApplyThreeColorGradient(text2, gradient.Evaluate(0f), gradient.Evaluate(0.5f), gradient.Evaluate(1f));
            else
                gradientText = Utils.ApplyGradient(text2, gradient.Evaluate(0f), gradient.Evaluate(1f));

            __instance.textAnimatorPlayer.ShowText(gradientText);
        }
        else
        {
            if (ColorUtility.TryParseHtmlString(winningFaction.GetFactionColor(), out Color color))
            {
                __instance.leftImage.color = color;
                __instance.rightImage.color = color;
                __instance.glow.color = color;
            }

            __instance.text.color = color;
            __instance.textAnimatorPlayer.ShowText(text2);
        }

        __instance.SetUpWinners(__instance.winningCharacters);
        return;
    }
}

[HarmonyPatch(typeof(FactionWinsStandardCinematicPlayer), nameof(FactionWinsStandardCinematicPlayer.Init))]
public static class PatchCustomWinScreens
{
    public static void Postfix(FactionWinsStandardCinematicPlayer __instance, ref ICinematicData cinematicData)
    {
        Debug.Log(string.Format("FactionWinsStandardCinematicPlayer current phase at end = {0}", Pepper.GetGamePhase()));
        __instance.elapsedDuration = 0f;
        __instance.cinematicData = cinematicData as FactionWinsCinematicData;
        var num = CinematicFactionWinsTimes.GetWinTimeByFaction(__instance.cinematicData.winningFaction);
        __instance.totalDuration = num;

        if (Pepper.IsResultsPhase())
            num += 0.2f;

        var winningFaction = __instance.cinematicData.winningFaction;

        PlayVictoryMusic(winningFaction);

        var text2 = __instance.l10n(string.Format("GUI_WINNERS_ARE_{0}", (int)winningFaction));
        string gradientText;

        if (winningFaction.GetChangedGradient(Role.NONE) != null)
        {
            Gradient gradient = winningFaction.GetChangedGradient(Role.NONE);

            if (winningFaction == (FactionType)44)
                gradientText = Utils.ApplyThreeColorGradient(text2, gradient.Evaluate(0f), gradient.Evaluate(0.5f), gradient.Evaluate(1f));
            else
                gradientText = Utils.ApplyGradient(text2, gradient.Evaluate(0f), gradient.Evaluate(1f));

            if (__instance.textAnimatorPlayer.gameObject.activeSelf)
                __instance.textAnimatorPlayer.ShowText(gradientText);
        }
        else if (__instance.textAnimatorPlayer.gameObject.activeSelf)
            __instance.textAnimatorPlayer.ShowText(text2);

        __instance.SetUpWinners();
        return;
    }

    private static void PlayVictoryMusic(FactionType winningFaction)
    {
        string musicPath = GetVictoryMusicPath(winningFaction);
        if (!string.IsNullOrEmpty(musicPath))
        {
            Service.Home.AudioService.PlayMusic(musicPath, false, AudioController.AudioChannel.Cinematic, true);
        }
    }

    private static string GetVictoryMusicPath(FactionType faction)
    {
        CinematicType? cinematicType = faction switch
        {
            FactionType.NONE => CinematicType.CovenWins,
            FactionType.TOWN => Fancy.TownCinematic.Value,
            FactionType.COVEN => Fancy.CovenCinematic.Value,
            FactionType.SERIALKILLER => Fancy.SerialKillerCinematic.Value,
            FactionType.ARSONIST => Fancy.ArsonistCinematic.Value,
            FactionType.WEREWOLF => Fancy.WerewolfCinematic.Value,
            FactionType.SHROUD => Fancy.ShroudCinematic.Value,
            FactionType.APOCALYPSE => Fancy.ApocalypseCinematic.Value,
            FactionType.VAMPIRE => Fancy.VampireCinematic.Value,
            (FactionType)33 => Fancy.JackalCinematic.Value,
            (FactionType)34 => Fancy.FrogsCinematic.Value,
            (FactionType)35 => Fancy.LionsCinematic.Value,
            (FactionType)36 => Fancy.HawksCinematic.Value,
            (FactionType)43 => Fancy.PandoraCinematic.Value,
            (FactionType)44 => Fancy.ComplianceCinematic.Value,
            (FactionType)250 => Fancy.LoversCinematic.Value,
            _ => null,
        };

        if (cinematicType == CinematicType.TownWins)
            return "Audio/Music/TownVictory.wav";
        if (cinematicType == CinematicType.CovenWins || cinematicType == CinematicType.FactionWins)
            return "Audio/Music/CovenVictory.wav";

        return null;
    }

}



}


