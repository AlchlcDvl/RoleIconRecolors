using Game.Characters;
using Home.HomeScene;
using Home.Shared;
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

        public static string ToChangedDisplayString(this Role role, FactionType faction, ROLE_MODIFIER modifier)
        {
            var text = "";
            var roleName = "";
            if (Fancy.FactionalRoleNames.Value) { roleName = Utils.ToRoleFactionDisplayString(role, faction); }
            else { roleName = role.ToDisplayString(); }

            if (faction.GetChangedGradient(role) != null)
                text = AddChangedConversionTags.ApplyGradient(roleName, faction.GetChangedGradient(role));
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
                text = text + "\n<size=85%>" + AddChangedConversionTags.ApplyGradient($"({Fancy.TraitorLabel.Value})", gradientTT.Evaluate(0f), gradientTT.Evaluate(1f)) + "</size>";
            }
            else if (modifier == (ROLE_MODIFIER)10)
            {
                var gradient = ((FactionType)33).GetChangedGradient(role);
                text = text + "\n<size=85%>" + AddChangedConversionTags.ApplyGradient($"({Fancy.RecruitLabel.Value})", gradient.Evaluate(0f), gradient.Evaluate(1f)) + "</size>";
            }
            else if (RoleExtensions.GetFaction(role) != faction)
            {
                var gradient2 = faction.GetChangedGradient(role);

                if (gradient2 != null)
                {
                    if (faction == (FactionType)44)
                    {
                        text = text + "\n<size=85%>" + AddChangedConversionTags.ApplyThreeColorGradient("(" + faction.ToDisplayString() + ")", gradient2.Evaluate(0f), gradient2.Evaluate(0.5f),
                            gradient2.Evaluate(1f)) + "</size>";
                    }
                    else
                        text = text + "\n<size=85%>" + AddChangedConversionTags.ApplyGradient("(" + faction.ToDisplayString() + ")", gradient2.Evaluate(0f), gradient2.Evaluate(1f)) + "</size>";

                    if (modifier == (ROLE_MODIFIER)1)
                    {
                        text = text + "\n<size=85%>" + AddChangedConversionTags.ApplyGradient($"({Fancy.VIPLabel.Value})", gradientTT.Evaluate(0f), gradientTT.Evaluate(1f)) + "</size>";
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
                        __instance.playerRoleText.text = AddChangedConversionTags.ApplyThreeColorGradient("(" + roleName + ")", gradient.Evaluate(0f), gradient.Evaluate(0.5f),
                            gradient.Evaluate(1f));
                    }
                    else if (faction == (FactionType)33 && role != BetterTOS2.RolePlus.JACKAL)
                    {
                        Gradient jackalGradient = Btos2Faction.Jackal.GetChangedGradient(role);

                        __instance.playerRoleText.text = AddChangedConversionTags.ApplyGradient("(" + roleName + ")", jackalGradient.Evaluate(0f), jackalGradient.Evaluate(1f));
                    }
                    else
                        __instance.playerRoleText.text = AddChangedConversionTags.ApplyGradient("(" + roleName + ")", gradient.Evaluate(0f), gradient.Evaluate(1f));

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
                gradientName = AddChangedConversionTags.ApplyThreeColorGradient(theName, gradient.Evaluate(0f), gradient.Evaluate(0.5f), gradient.Evaluate(1f));
                gradientRole = AddChangedConversionTags.ApplyThreeColorGradient("(" + roleName + ")", gradient.Evaluate(0f), gradient.Evaluate(0.5f), gradient.Evaluate(1f));
            }
            else if (factionType == (FactionType)33 && role != Btos2Role.Jackal)
            {
                Gradient jackalGradient = Btos2Faction.Jackal.GetChangedGradient(role);
                gradientName = AddChangedConversionTags.ApplyGradient(theName, jackalGradient.Evaluate(0f), jackalGradient.Evaluate(1f));
                gradientRole = AddChangedConversionTags.ApplyGradient("(" + roleName + ")", gradient.Evaluate(0f), jackalGradient.Evaluate(1f));
            }
            else
            {
                gradientName = AddChangedConversionTags.ApplyGradient(theName, gradient.Evaluate(0f), gradient.Evaluate(1f));
                gradientRole = AddChangedConversionTags.ApplyGradient("(" + roleName + ")", gradient.Evaluate(0f), gradient.Evaluate(1f));
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
                string text;
                if (Fancy.FactionalRoleNames.Value) { text = Utils.ToRoleFactionDisplayString(role, factionType); }
                else { text = ClientRoleExtensions.ToDisplayString(role); }


                if (factionType.GetChangedGradient(role) != null)
                    newtext = AddChangedConversionTags.ApplyGradient(text, factionType.GetChangedGradient(role));
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
                if (factionType != FactionType.NONE) // TESTING PURPOSES
                {
                    if (factionType is not ((FactionType)33 or (FactionType)44))
                    {
                        if (factionType.GetChangedGradient(role) != null)
                        {
                            newtext += " " + AddChangedConversionTags.ApplyGradient("(" + factionType.ToDisplayString() + ")", factionType.GetChangedGradient(role).Evaluate(0f),
                                factionType.GetChangedGradient(role).Evaluate(1f));
                        }
                        else
                            newtext += " " + "<color=" + ClientRoleExtensions.GetFactionColor(factionType) + ">(" + factionType.ToDisplayString() + ")</color>";
                    }
                    else if (factionType == (FactionType)33)
                    {
                        newtext += " " + AddChangedConversionTags.ApplyGradient("(" + Fancy.RecruitLabel.Value + ")",
                            factionType.GetChangedGradient(role).Evaluate(0f), factionType.GetChangedGradient(role).Evaluate(1f));
                    }
                    else if (factionType == (FactionType)44)
                    {
                        newtext += " " + AddChangedConversionTags.ApplyThreeColorGradient("(" + factionType.ToDisplayString() + ")", factionType.GetChangedGradient(role).Evaluate(0f),
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

        SetStyle(styles, "TownColor", Fancy.TownStart.Value);
        SetStyle(styles, "CovenColor", Fancy.CovenStart.Value);
        SetStyle(styles, "ApocalypseColor", Fancy.ApocalypseStart.Value);
        SetStyle(styles, "SerialKillerColor", Fancy.SerialKillerStart.Value);
        SetStyle(styles, "ArsonistColor", Fancy.ArsonistStart.Value);
        SetStyle(styles, "WerewolfColor", Fancy.WerewolfStart.Value);
        SetStyle(styles, "ShroudColor", Fancy.ShroudStart.Value);
        SetStyle(styles, "ExecutionerColor", Fancy.ExecutionerStart.Value);
        SetStyle(styles, "JesterColor", Fancy.JesterStart.Value);
        SetStyle(styles, "PirateColor", Fancy.PirateStart.Value);
        SetStyle(styles, "DoomsayerColor", Fancy.DoomsayerStart.Value);
        SetStyle(styles, "VampireColor", Fancy.VampireStart.Value);
        SetStyle(styles, "CursedSoulColor", Fancy.CursedSoulStart.Value);
        SetStyle(styles, "NeutralColor", Fancy.Neutral.Value);

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
                __result = ApplyGradient(text, factionType.GetChangedGradient(role));
            else
                __result = $"<color={factionType.GetFactionColor()}>{text}</color>";
        }
    }

    public static string ApplyGradient(string text, Color color1, Color color2)
    {
        var gradient = new Gradient();
        gradient.SetKeys(
        [
            new(color1, 0f),
            new(color2, 1f)
        ],
        [
            new(1f, 0f),
            new(1f, 1f)
        ]);
        var text2 = "";

        for (var i = 0; i < text.Length; i++)
            text2 += $"<color={ToHexString(gradient.Evaluate((float)i / text.Length))}>{text[i]}</color>";

        return text2;
    }

    public static string ApplyThreeColorGradient(string text, Color color1, Color color2, Color color3)
    {
        var gradient = new Gradient();
        gradient.SetKeys(
        [
            new(color1, 0f),
            new(color2, 0.5f),
            new(color3, 1f)
        ],
        [
            new(1f, 0f),
            new(1f, 1f)
        ]);
        var text2 = "";

        for (var i = 0; i < text.Length; i++)
            text2 += $"<color={ToHexString(gradient.Evaluate((float)i / text.Length))}>{text[i]}</color>";

        return text2;
    }

    public static string ApplyGradient(string text, Gradient gradient)
    {
        var text2 = "";

        for (var i = 0; i < text.Length; i++)
            text2 += $"<color={ToHexString(gradient.Evaluate((float)i / text.Length))}>{text[i]}</color>";

        return text2;
    }

    public static string ToHexString(Color color)
    {
        Color32 color2 = color;
        return $"#{color2.r:X2}{color2.g:X2}{color2.b:X2}";
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
                            text3 = AddChangedConversionTags.ApplyThreeColorGradient(Pepper.GetDiscussionPlayerByPosition(position).gameName + ":", gradient.Evaluate(0f),
                                gradient.Evaluate(0.5f), gradient.Evaluate(1f));
                        }
                        else
                            text3 = AddChangedConversionTags.ApplyGradient(Pepper.GetDiscussionPlayerByPosition(position).gameName + ":", gradient.Evaluate(0f), gradient.Evaluate(1f));

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
                        var text4 = AddChangedConversionTags.ApplyGradient(Pepper.GetDiscussionPlayerByPosition(position).gameName + ":", gradient2.Evaluate(0f), gradient2.Evaluate(1f));
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
                    var text6 = AddChangedConversionTags.ApplyGradient(Pepper.GetDiscussionPlayerByPosition(position).gameName + ":", gradient3.Evaluate(0f), gradient3.Evaluate(1f));
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

// AS take a look at this, TOS2 freaks out when loading it
[HarmonyPatch(typeof(ClientRoleExtensions), nameof(ClientRoleExtensions.GetFactionColor))]
public static class SwapColor
{
    [HarmonyPostfix]
    public static void Swap(ref string __result, ref FactionType factionType)
    {
        if (Fancy.TownStart?.Value != null)
        {
            var faction = (int)factionType;
            __result = faction switch
            {
                1 => Fancy.TownStart.Value,
                2 => Fancy.CovenStart.Value,
                3 => Fancy.SerialKillerStart.Value,
                4 => Fancy.ArsonistStart.Value,
                5 => Fancy.WerewolfStart.Value,
                6 => Fancy.ShroudStart.Value,
                7 => Fancy.ApocalypseStart.Value,
                8 => Fancy.ExecutionerStart.Value,
                9 => Fancy.JesterStart.Value,
                10 => Fancy.PirateStart.Value,
                11 => Fancy.DoomsayerStart.Value,
                12 => Fancy.VampireStart.Value,
                13 => Fancy.CursedSoulStart.Value,
                33 => Fancy.JackalStart.Value,
                34 => Fancy.FrogsStart.Value,
                35 => Fancy.LionsStart.Value,
                36 => Fancy.HawksStart.Value,
                38 => Fancy.JudgeStart.Value,
                39 => Fancy.AuditorStart.Value,
                40 => Fancy.InquisitorStart.Value,
                41 => Fancy.StarspawnStart.Value,
                42 => Fancy.EgotistStart.Value,
                43 => Fancy.PandoraStart.Value,
                44 => Fancy.ComplianceStart.Value,
                250 => Fancy.Lovers.Value,
                _ => Fancy.StonedHidden.Value,
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
                    array[0] = new(Fancy.TownStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.TownMajor.Value.ToColor(), 1f);
                    goto setmajor;

                case FactionType.COVEN:
                    array[0] = new(Fancy.CovenStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.CovenMajor.Value.ToColor(), 1f);
                    goto setmajor;

                case FactionType.APOCALYPSE:
                    array[0] = new(Fancy.ApocalypseStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.ApocalypseMajor.Value.ToColor(), 1f);
                    goto setmajor;

                case FactionType.EXECUTIONER:
                    array[0] = new(Fancy.ExecutionerStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.ExecutionerEnd.Value.ToColor(), 1f);
                    goto setmajor;

                case FactionType.SERIALKILLER:
                    array[0] = new(Fancy.SerialKillerStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.SerialKillerMajor.Value.ToColor(), 1f);
                    goto setmajor;

                case FactionType.ARSONIST:
                    array[0] = new(Fancy.ArsonistStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.ArsonistMajor.Value.ToColor(), 1f);
                    goto setmajor;

                case FactionType.WEREWOLF:
                    array[0] = new(Fancy.WerewolfStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.WerewolfMajor.Value.ToColor(), 1f);
                    goto setmajor;

                case FactionType.SHROUD:
                    array[0] = new(Fancy.ShroudStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.ShroudMajor.Value.ToColor(), 1f);
                    goto setmajor;

                case FactionType.JESTER:
                    array[0] = new(Fancy.JesterStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.JesterEnd.Value.ToColor(), 1f);
                    goto setmajor;

                case (FactionType)40:
                    array[0] = new(Fancy.InquisitorStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.InquisitorEnd.Value.ToColor(), 1f);
                    goto setmajor;

                case FactionType.PIRATE:
                    array[0] = new(Fancy.PirateStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.PirateEnd.Value.ToColor(), 1f);
                    goto setmajor;

                case FactionType.DOOMSAYER:
                    array[0] = new(Fancy.DoomsayerStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.DoomsayerEnd.Value.ToColor(), 1f);
                    goto setmajor;

                case FactionType.VAMPIRE:
                    array[0] = new(Fancy.VampireStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.VampireMajor.Value.ToColor(), 1f);
                    goto setmajor;

                case FactionType.CURSED_SOUL:
                    array[0] = new(Fancy.CursedSoulStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.CursedSoulMajor.Value.ToColor(), 1f);
                    goto setmajor;

                case (FactionType)33:
                    switch (Fancy.RecruitEndingColor.Value)
                    {
                        case RecruitEndType.JackalEnd:
                            array[0] = new(Fancy.JackalStart.Value.ToColor(), 0f);
                            array[1] = new(Fancy.JackalMajor.Value.ToColor(), 1f);
                            goto setmajor;

                        case RecruitEndType.FactionStart:
                            array[0] = new(Fancy.JackalStart.Value.ToColor(), 0f);
                            switch (role.GetFaction())
                            {
                                case FactionType.TOWN:
                                    array[1] = new(Fancy.TownStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.COVEN:
                                    array[1] = new(Fancy.CovenStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.SERIALKILLER:
                                    array[1] = new(Fancy.SerialKillerStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.ARSONIST:
                                    array[1] = new(Fancy.ArsonistStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.WEREWOLF:
                                    array[1] = new(Fancy.WerewolfStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.SHROUD:
                                    array[1] = new(Fancy.ShroudStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.APOCALYPSE:
                                    array[1] = new(Fancy.ApocalypseStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.EXECUTIONER:
                                    array[1] = new(Fancy.ExecutionerStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.JESTER:
                                    array[1] = new(Fancy.JesterStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.PIRATE:
                                    array[1] = new(Fancy.PirateStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.DOOMSAYER:
                                    array[1] = new(Fancy.DoomsayerStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.VAMPIRE:
                                    array[1] = new(Fancy.VampireStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.CURSED_SOUL:
                                    array[1] = new(Fancy.CursedSoulStart.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)34:
                                    array[1] = new(Fancy.FrogsStart.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)35:
                                    array[1] = new(Fancy.LionsStart.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)36:
                                    array[1] = new(Fancy.HawksStart.Value.ToColor(), 1f);
                                    break;
                                 case (FactionType)38:
                                    array[1] = new(Fancy.JudgeStart.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)39:
                                    array[1] = new(Fancy.AuditorStart.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)40:
                                    array[1] = new(Fancy.InquisitorStart.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)41:
                                    array[1] = new(Fancy.StarspawnStart.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)42:
                                    array[1] = new(Fancy.EgotistStart.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)43:
                                    array[1] = new(Fancy.PandoraStart.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)44:
                                    array[1] = new(Fancy.ComplianceStart.Value.ToColor(), 1f);
                                    break;
                               default:
                                    array[1] = new(Fancy.JackalMajor.Value.ToColor(), 1f);
                                    break;
                            }
                            goto setmajor;

                        case RecruitEndType.FactionEnd: 
                            array[0] = new(Fancy.JackalStart.Value.ToColor(), 0f);
                            switch (role.GetFaction())
                            {
                                case FactionType.TOWN:
                                    array[1] = new(Fancy.TownMajor.Value.ToColor(), 1f);
                                    break;
                                case FactionType.COVEN:
                                    array[1] = new(Fancy.CovenMajor.Value.ToColor(), 1f);
                                    break;
                                case FactionType.SERIALKILLER:
                                    array[1] = new(Fancy.SerialKillerMajor.Value.ToColor(), 1f);
                                    break;
                                case FactionType.ARSONIST:
                                    array[1] = new(Fancy.ArsonistMajor.Value.ToColor(), 1f);
                                    break;
                                case FactionType.WEREWOLF:
                                    array[1] = new(Fancy.WerewolfMajor.Value.ToColor(), 1f);
                                    break;
                                case FactionType.SHROUD:
                                    array[1] = new(Fancy.ShroudMajor.Value.ToColor(), 1f);
                                    break;
                                case FactionType.APOCALYPSE:
                                    array[1] = new(Fancy.ApocalypseMajor.Value.ToColor(), 1f);
                                    break;
                                case FactionType.EXECUTIONER:
                                    array[1] = new(Fancy.ExecutionerStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.JESTER:
                                    array[1] = new(Fancy.JesterStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.PIRATE:
                                    array[1] = new(Fancy.PirateStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.DOOMSAYER:
                                    array[1] = new(Fancy.DoomsayerStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.VAMPIRE:
                                    array[1] = new(Fancy.VampireMajor.Value.ToColor(), 1f);
                                    break;
                                case FactionType.CURSED_SOUL:
                                    array[1] = new(Fancy.CursedSoulMajor.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)34:
                                    array[1] = new(Fancy.FrogsMajor.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)35:
                                    array[1] = new(Fancy.LionsMajor.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)36:
                                    array[1] = new(Fancy.HawksMajor.Value.ToColor(), 1f);
                                    break;
                                 case (FactionType)38:
                                    array[1] = new(Fancy.JudgeStart.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)39:
                                    array[1] = new(Fancy.AuditorStart.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)40:
                                    array[1] = new(Fancy.InquisitorStart.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)41:
                                    array[1] = new(Fancy.StarspawnStart.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)42:
                                    array[1] = new(Fancy.EgotistMajor.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)43:
                                    array[1] = new(Fancy.PandoraMajor.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)44:
                                    array[1] = new(Fancy.ComplianceMajor.Value.ToColor(), 1f);
                                    break;
                            }
                            goto setmajor;

                        default:
                            array[0] = new(Fancy.JackalStart.Value.ToColor(), 0f);
                            array[1] = new(Fancy.JackalMajor.Value.ToColor(), 1f);
                            goto setmajor;
                    }

                case (FactionType)38:
                    array[0] = new(Fancy.JudgeStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.JudgeEnd.Value.ToColor(), 1f);
                    goto setmajor;

                case (FactionType)39:
                    array[0] = new(Fancy.AuditorStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.AuditorEnd.Value.ToColor(), 1f);
                    goto setmajor;

                case (FactionType)41:
                    array[0] = new(Fancy.StarspawnStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.StarspawnEnd.Value.ToColor(), 1f);
                    goto setmajor;

                case (FactionType)42:
                    array[0] = new(Fancy.EgotistStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.EgotistMajor.Value.ToColor(), 1f);
                    goto setmajor;

                case (FactionType)43:
                    array[0] = new(Fancy.PandoraStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.PandoraMajor.Value.ToColor(), 1f);
                    goto setmajor;

                case (FactionType)34:
                    array[0] = new(Fancy.FrogsStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.FrogsMajor.Value.ToColor(), 1f);
                    goto setmajor;

                case (FactionType)35:
                    array[0] = new(Fancy.LionsStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.LionsMajor.Value.ToColor(), 1f);
                    goto setmajor;

                case (FactionType)36:
                    array[0] = new(Fancy.HawksStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.HawksMajor.Value.ToColor(), 1f);
                    goto setmajor;

                case (FactionType)44:
                    array =
                    [
                        new(Fancy.ComplianceStart.Value.ToColor(), 0f),
                        new(Fancy.ComplianceMiddle.Value.ToColor(), 0.5f),
                        new(Fancy.ComplianceMajor.Value.ToColor(), 1f)
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
                    array[0] = new(Fancy.TownStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.TownEnd.Value.ToColor(), 1f);
                    goto setgradient;

                case FactionType.COVEN:
                    array[0] = new(Fancy.CovenStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.CovenEnd.Value.ToColor(), 1f);
                    goto setgradient;

                case FactionType.APOCALYPSE:
                    array[0] = new(Fancy.ApocalypseStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.ApocalypseEnd.Value.ToColor(), 1f);
                    goto setgradient;

                case FactionType.EXECUTIONER:
                    array[0] = new(Fancy.ExecutionerStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.ExecutionerEnd.Value.ToColor(), 1f);
                    goto setgradient;

                case FactionType.SERIALKILLER:
                    array[0] = new(Fancy.SerialKillerStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.SerialKillerEnd.Value.ToColor(), 1f);
                    goto setgradient;

                case FactionType.ARSONIST:
                    array[0] = new(Fancy.ArsonistStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.ArsonistEnd.Value.ToColor(), 1f);
                    goto setgradient;

                case FactionType.WEREWOLF:
                    array[0] = new(Fancy.WerewolfStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.WerewolfEnd.Value.ToColor(), 1f);
                    goto setgradient;

                case FactionType.SHROUD:
                    array[0] = new(Fancy.ShroudStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.ShroudEnd.Value.ToColor(), 1f);
                    goto setgradient;

                case FactionType.JESTER:
                    array[0] = new(Fancy.JesterStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.JesterEnd.Value.ToColor(), 1f);
                    goto setgradient;

                case (FactionType)40:
                    array[0] = new(Fancy.InquisitorStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.InquisitorEnd.Value.ToColor(), 1f);
                    goto setgradient;

                case FactionType.PIRATE:
                    array[0] = new(Fancy.PirateStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.PirateEnd.Value.ToColor(), 1f);
                    goto setgradient;

                case FactionType.DOOMSAYER:
                    array[0] = new(Fancy.DoomsayerStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.DoomsayerEnd.Value.ToColor(), 1f);
                    goto setgradient;

                case FactionType.VAMPIRE:
                    array[0] = new(Fancy.VampireStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.VampireEnd.Value.ToColor(), 1f);
                    goto setgradient;

                case FactionType.CURSED_SOUL:
                    array[0] = new(Fancy.CursedSoulStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.CursedSoulEnd.Value.ToColor(), 1f);
                    goto setgradient;

                case (FactionType)33:
                    switch (Fancy.RecruitEndingColor.Value)
                    {
                        case RecruitEndType.JackalEnd:
                            array[0] = new(Fancy.JackalStart.Value.ToColor(), 0f);
                            array[1] = new(Fancy.JackalEnd.Value.ToColor(), 1f);
                            goto setgradient;

                        case RecruitEndType.FactionStart:
                            array[0] = new(Fancy.JackalStart.Value.ToColor(), 0f);
                            switch (role.GetFaction())
                            {
                                case FactionType.TOWN:
                                    array[1] = new(Fancy.TownStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.COVEN:
                                    array[1] = new(Fancy.CovenStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.SERIALKILLER:
                                    array[1] = new(Fancy.SerialKillerStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.ARSONIST:
                                    array[1] = new(Fancy.ArsonistStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.WEREWOLF:
                                    array[1] = new(Fancy.WerewolfStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.SHROUD:
                                    array[1] = new(Fancy.ShroudStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.APOCALYPSE:
                                    array[1] = new(Fancy.ApocalypseStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.EXECUTIONER:
                                    array[1] = new(Fancy.ExecutionerStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.JESTER:
                                    array[1] = new(Fancy.JesterStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.PIRATE:
                                    array[1] = new(Fancy.PirateStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.DOOMSAYER:
                                    array[1] = new(Fancy.DoomsayerStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.VAMPIRE:
                                    array[1] = new(Fancy.VampireStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.CURSED_SOUL:
                                    array[1] = new(Fancy.CursedSoulStart.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)34:
                                    array[1] = new(Fancy.FrogsStart.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)35:
                                    array[1] = new(Fancy.LionsStart.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)36:
                                    array[1] = new(Fancy.HawksStart.Value.ToColor(), 1f);
                                    break;
                                 case (FactionType)38:
                                    array[1] = new(Fancy.JudgeStart.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)39:
                                    array[1] = new(Fancy.AuditorStart.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)40:
                                    array[1] = new(Fancy.InquisitorStart.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)41:
                                    array[1] = new(Fancy.StarspawnStart.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)42:
                                    array[1] = new(Fancy.EgotistStart.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)43:
                                    array[1] = new(Fancy.PandoraStart.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)44:
                                    array[1] = new(Fancy.ComplianceStart.Value.ToColor(), 1f);
                                    break;
                               default:
                                    array[1] = new(Fancy.JackalEnd.Value.ToColor(), 1f);
                                    break;
                            }
                            goto setgradient;

                        case RecruitEndType.FactionEnd: 
                            array[0] = new(Fancy.JackalStart.Value.ToColor(), 0f);
                            switch (role.GetFaction())
                            {
                                case FactionType.TOWN:
                                    array[1] = new(Fancy.TownEnd.Value.ToColor(), 1f);
                                    break;
                                case FactionType.COVEN:
                                    array[1] = new(Fancy.CovenEnd.Value.ToColor(), 1f);
                                    break;
                                case FactionType.SERIALKILLER:
                                    array[1] = new(Fancy.SerialKillerEnd.Value.ToColor(), 1f);
                                    break;
                                case FactionType.ARSONIST:
                                    array[1] = new(Fancy.ArsonistEnd.Value.ToColor(), 1f);
                                    break;
                                case FactionType.WEREWOLF:
                                    array[1] = new(Fancy.WerewolfEnd.Value.ToColor(), 1f);
                                    break;
                                case FactionType.SHROUD:
                                    array[1] = new(Fancy.ShroudEnd.Value.ToColor(), 1f);
                                    break;
                                case FactionType.APOCALYPSE:
                                    array[1] = new(Fancy.ApocalypseEnd.Value.ToColor(), 1f);
                                    break;
                                case FactionType.EXECUTIONER:
                                    array[1] = new(Fancy.ExecutionerStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.JESTER:
                                    array[1] = new(Fancy.JesterStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.PIRATE:
                                    array[1] = new(Fancy.PirateStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.DOOMSAYER:
                                    array[1] = new(Fancy.DoomsayerStart.Value.ToColor(), 1f);
                                    break;
                                case FactionType.VAMPIRE:
                                    array[1] = new(Fancy.VampireEnd.Value.ToColor(), 1f);
                                    break;
                                case FactionType.CURSED_SOUL:
                                    array[1] = new(Fancy.CursedSoulEnd.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)34:
                                    array[1] = new(Fancy.FrogsEnd.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)35:
                                    array[1] = new(Fancy.LionsEnd.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)36:
                                    array[1] = new(Fancy.HawksEnd.Value.ToColor(), 1f);
                                    break;
                                 case (FactionType)38:
                                    array[1] = new(Fancy.JudgeStart.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)39:
                                    array[1] = new(Fancy.AuditorStart.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)40:
                                    array[1] = new(Fancy.InquisitorStart.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)41:
                                    array[1] = new(Fancy.StarspawnStart.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)42:
                                    array[1] = new(Fancy.EgotistEnd.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)43:
                                    array[1] = new(Fancy.PandoraEnd.Value.ToColor(), 1f);
                                    break;
                                case (FactionType)44:
                                    array[1] = new(Fancy.ComplianceEnd.Value.ToColor(), 1f);
                                    break;
                            }
                            goto setgradient;

                        default:
                            array[0] = new(Fancy.JackalStart.Value.ToColor(), 0f);
                            array[1] = new(Fancy.JackalEnd.Value.ToColor(), 1f);
                            goto setgradient;
                    }

                case (FactionType)38:
                    array[0] = new(Fancy.JudgeStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.JudgeEnd.Value.ToColor(), 1f);
                    goto setgradient;

                case (FactionType)39:
                    array[0] = new(Fancy.AuditorStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.AuditorEnd.Value.ToColor(), 1f);
                    goto setgradient;

                case (FactionType)41:
                    array[0] = new(Fancy.StarspawnStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.StarspawnEnd.Value.ToColor(), 1f);
                    goto setgradient;

                case (FactionType)42:
                    array[0] = new(Fancy.EgotistStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.EgotistEnd.Value.ToColor(), 1f);
                    goto setgradient;

                case (FactionType)43:
                    array[0] = new(Fancy.PandoraStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.PandoraEnd.Value.ToColor(), 1f);
                    goto setgradient;

                case (FactionType)34:
                    array[0] = new(Fancy.FrogsStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.FrogsEnd.Value.ToColor(), 1f);
                    goto setgradient;

                case (FactionType)35:
                    array[0] = new(Fancy.LionsStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.LionsEnd.Value.ToColor(), 1f);
                    goto setgradient;

                case (FactionType)36:
                    array[0] = new(Fancy.HawksStart.Value.ToColor(), 0f);
                    array[1] = new(Fancy.HawksEnd.Value.ToColor(), 1f);
                    goto setgradient;

                case (FactionType)44:
                    array =
                    [
                        new(Fancy.ComplianceStart.Value.ToColor(), 0f),
                        new(Fancy.ComplianceMiddle.Value.ToColor(), 0.5f),
                        new(Fancy.ComplianceEnd.Value.ToColor(), 1f)
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

public class GradientRoleColorController : MonoBehaviour
{
    public RoleCardPanelBackground __instance;
    private readonly float duration = 10f;
    private float value = 0f;

    public void Start() => StartCoroutine(ChangeValueOverTime(__instance.currentFaction, this.__instance.currentRole));

    public void OnDestroy() => StopCoroutine(ChangeValueOverTime(__instance.currentFaction, this.__instance.currentRole));

    private IEnumerator ChangeValueOverTime(FactionType faction, Role role)
    {
        var grad = faction.GetChangedGradient(role);

        if (grad == null)
        {
            Destroy(this);
            yield break;
        }

        for (;;)
        {
            for (var t = 0f; t < duration; t += Time.deltaTime)
            {
                value = Mathf.Lerp(0f, 1f, t / duration);
                __instance.rolecardBackgroundInstance.SetColor(grad.Evaluate(value));
                yield return new WaitForEndOfFrame();
            }

            for (var t2 = 0f; t2 < duration; t2 += Time.deltaTime)
            {
                value = Mathf.Lerp(1f, 0f, t2 / duration);
                __instance.rolecardBackgroundInstance.SetColor(grad.Evaluate(value));
                yield return new WaitForEndOfFrame();
            }
        }
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
                __result = "<link=\"r57\"><sprite=\"BTOSRoleIcons\" name=\"Role57\"><indent=1.1em><b>" + AddChangedConversionTags.ApplyGradient(Fancy.CourtLabel.Value, Fancy.JudgeStart.Value.ToColor(), Fancy.JudgeEnd.Value.ToColor()) + ":" +
                    "</b> </link>" + encodedText.Replace("????: </color>", "").Replace("white", "#FFFF00");
            }
            else if (position == 69)
                __result = encodedText.Replace("????:", $"<sprite=\"BTOSRoleIcons\" name=\"Role16\"> {Fancy.JuryLabel.Value}:");
            else if (position == 71)
            {
                __result = "<link=\"r46\"><sprite=\"BTOSRoleIcons\" name=\"Role46\"><indent=1.1em><b>" + AddChangedConversionTags.ApplyGradient(Fancy.PirateLabel.Value, Fancy.PirateStart.Value.ToColor(), Fancy.PirateEnd.Value.ToColor()) + ":</b> </link>" + encodedText.Replace("????: </color>", "").Replace("white", "#ECC23E");
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
                gradientText = AddChangedConversionTags.ApplyThreeColorGradient(text2, gradient.Evaluate(0f), gradient.Evaluate(0.5f), gradient.Evaluate(1f));
            else
                gradientText = AddChangedConversionTags.ApplyGradient(text2, gradient.Evaluate(0f), gradient.Evaluate(1f));

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

        if (winningFaction == FactionType.TOWN)
            Service.Home.AudioService.PlayMusic("Audio/Music/TownVictory.wav", false, AudioController.AudioChannel.Cinematic, true);
        else if (winningFaction is FactionType.COVEN or FactionType.NONE)
            Service.Home.AudioService.PlayMusic("Audio/Music/CovenVictory.wav", false, AudioController.AudioChannel.Cinematic, true);

        var text2 = __instance.l10n(string.Format("GUI_WINNERS_ARE_{0}", (int)winningFaction));
        string gradientText;

        if (winningFaction.GetChangedGradient(Role.NONE) != null)
        {
            Gradient gradient = winningFaction.GetChangedGradient(Role.NONE);

            if (winningFaction == (FactionType)44)
                gradientText = AddChangedConversionTags.ApplyThreeColorGradient(text2, gradient.Evaluate(0f), gradient.Evaluate(0.5f), gradient.Evaluate(1f));
            else
                gradientText = AddChangedConversionTags.ApplyGradient(text2, gradient.Evaluate(0f), gradient.Evaluate(1f));

            if (__instance.textAnimatorPlayer.gameObject.activeSelf)
                __instance.textAnimatorPlayer.ShowText(gradientText);
        }
        else if (__instance.textAnimatorPlayer.gameObject.activeSelf)
            __instance.textAnimatorPlayer.ShowText(text2);

        __instance.SetUpWinners();
        return;
    }
}


}


