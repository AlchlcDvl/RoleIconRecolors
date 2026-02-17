using Server.Shared.Extensions;

namespace FancyUI;

public static class Gradients
{
    public static Gradient GetChangedGradient(this FactionType faction, Role role)
    {
        var mod = Utils.GetGameType();
        var middleKey = Utils.FactionName(faction, mod, stoned: true).ToUpper();
        var baseKey = Utils.FactionName(role.GetFactionType(mod), mod, stoned: true).ToUpper();
        var startKey = middleKey;

        switch (faction)
        {
            case FactionType.VAMPIRE:
                startKey = "VAMPIRE";
                break;
            case Btos2Faction.Jackal:
                startKey = "JACKAL";
                break;
        }
        string end;

        if (faction == Btos2Faction.Jackal && role != Btos2Role.Jackal && role != Role.NONE && role != Role.UNKNOWN)
        {
            end = Fancy.RecruitEndingColor.Value switch
            {
                OverrideEndType.FactionStart => Fancy.Colors.TryGetValue(baseKey, out var baseTuple) ? baseTuple.Start : Fancy.Colors["JACKAL"].End,
                OverrideEndType.FactionEnd => GetEndColor(baseKey, role),
				OverrideEndType.FactionBothColors => Utils.BlendHexColors(Fancy.Colors.TryGetValue(baseKey, out var bt) ? bt.Start : Fancy.Colors["JACKAL"].Start, GetEndColor(baseKey, role), Constants.RecruitEndRatio()),
				OverrideEndType.BlendFactionStart => Utils.BlendHexColors(Fancy.Colors["JACKAL"].End, Fancy.Colors.TryGetValue(baseKey, out var vs) ? vs.Start : Fancy.Colors["JACKAL"].End, Constants.RecruitEndRatio()),
				OverrideEndType.BlendFactionEnd => Utils.BlendHexColors(Fancy.Colors["JACKAL"].End, GetEndColor(baseKey, role), Constants.RecruitEndRatio()),
				OverrideEndType.BlendFactionBothColors => Utils.BlendHexColors(Fancy.Colors["JACKAL"].End, Utils.BlendHexColors(Fancy.Colors.TryGetValue(baseKey, out var vb) ? vb.Start : Fancy.Colors["JACKAL"].Start, GetEndColor(baseKey, role), Constants.RecruitEndRatio()), Constants.RecruitEndRatio()),
                _ => GetEndColor("JACKAL", role),
            };
        }
        else if (faction == FactionType.VAMPIRE && role != Role.VAMPIRE && role != Role.NONE && role != Role.UNKNOWN)
		{
			end = Fancy.ConvertEndingColor.Value switch
			{
				OverrideEndType.FactionStart => Fancy.Colors.TryGetValue(baseKey, out var baseTuple) ? baseTuple.Start : Fancy.Colors["VAMPIRE"].End,
				OverrideEndType.FactionEnd => GetEndColor(baseKey, role),
				OverrideEndType.FactionBothColors => Utils.BlendHexColors(Fancy.Colors.TryGetValue(baseKey, out var bt) ? bt.Start : Fancy.Colors["VAMPIRE"].Start, GetEndColor(baseKey, role), Constants.ConvertEndRatio()),
				OverrideEndType.BlendFactionStart => Utils.BlendHexColors(Fancy.Colors["VAMPIRE"].End, Fancy.Colors.TryGetValue(baseKey, out var vs) ? vs.Start : Fancy.Colors["VAMPIRE"].End, Constants.ConvertEndRatio()),
				OverrideEndType.BlendFactionEnd => Utils.BlendHexColors(Fancy.Colors["VAMPIRE"].End, GetEndColor(baseKey, role), Constants.ConvertEndRatio()),
				OverrideEndType.BlendFactionBothColors => Utils.BlendHexColors(Fancy.Colors["VAMPIRE"].End, Utils.BlendHexColors(Fancy.Colors.TryGetValue(baseKey, out var vb) ? vb.Start : Fancy.Colors["VAMPIRE"].Start, GetEndColor(baseKey, role), Constants.ConvertEndRatio()), Constants.ConvertEndRatio()),
				_ => GetEndColor("VAMPIRE", role),
			};
		}
        else
            end = GetEndColor(middleKey, role);

        string start;

        if (faction == Btos2Faction.Jackal && role != Btos2Role.Jackal && role != Role.NONE && role != Role.UNKNOWN)
        {
            start = Fancy.RecruitStartingColor.Value switch
            {
                OverrideStartType.FactionStart => Fancy.Colors.TryGetValue(baseKey, out var baseTuple) ? baseTuple.Start : Fancy.Colors["JACKAL"].Start,
                OverrideStartType.FactionEnd => GetEndColor(baseKey, role),
				OverrideStartType.FactionBothColors => Utils.BlendHexColors(Fancy.Colors.TryGetValue(baseKey, out var bt) ? bt.Start : Fancy.Colors["JACKAL"].Start, GetEndColor(baseKey, role), Constants.RecruitStartRatio()),
				OverrideStartType.BlendFactionStart => Utils.BlendHexColors(Fancy.Colors["JACKAL"].Start, Fancy.Colors.TryGetValue(baseKey, out var vs) ? vs.Start : Fancy.Colors["JACKAL"].Start, Constants.RecruitStartRatio()),
				OverrideStartType.BlendFactionEnd => Utils.BlendHexColors(Fancy.Colors["JACKAL"].Start, GetEndColor(baseKey, role), Constants.RecruitStartRatio()),
				OverrideStartType.BlendFactionBothColors => Utils.BlendHexColors(Fancy.Colors["JACKAL"].Start, Utils.BlendHexColors(Fancy.Colors.TryGetValue(baseKey, out var vb) ? vb.Start : Fancy.Colors["JACKAL"].Start, GetEndColor(baseKey, role), Constants.RecruitStartRatio()), Constants.RecruitStartRatio()),
                _ => Fancy.Colors["JACKAL"].Start,
            };
        }
        else if (faction == FactionType.VAMPIRE && role != Role.VAMPIRE && role != Role.NONE && role != Role.UNKNOWN)
		{
			start = Fancy.ConvertStartingColor.Value switch
			{
				OverrideStartType.FactionStart => Fancy.Colors.TryGetValue(baseKey, out var baseTuple) ? baseTuple.Start : Fancy.Colors["VAMPIRE"].Start,
				OverrideStartType.FactionEnd => GetEndColor(baseKey, role),
				OverrideStartType.FactionBothColors => Utils.BlendHexColors(Fancy.Colors.TryGetValue(baseKey, out var bt) ? bt.Start : Fancy.Colors["VAMPIRE"].Start, GetEndColor(baseKey, role), Constants.ConvertStartRatio()),
				OverrideStartType.BlendFactionStart => Utils.BlendHexColors(Fancy.Colors["VAMPIRE"].Start, Fancy.Colors.TryGetValue(baseKey, out var vs) ? vs.Start : Fancy.Colors["VAMPIRE"].Start, Constants.ConvertStartRatio()),
				OverrideStartType.BlendFactionEnd => Utils.BlendHexColors(Fancy.Colors["VAMPIRE"].Start, GetEndColor(baseKey, role), Constants.ConvertStartRatio()),
				OverrideStartType.BlendFactionBothColors => Utils.BlendHexColors(Fancy.Colors["VAMPIRE"].Start, Utils.BlendHexColors(Fancy.Colors.TryGetValue(baseKey, out var vb) ? vb.Start : Fancy.Colors["VAMPIRE"].Start, GetEndColor(baseKey, role), Constants.ConvertStartRatio()), Constants.ConvertStartRatio()),
				_ => Fancy.Colors["VAMPIRE"].Start,
			};
		}

        else
            start = Fancy.Colors[startKey].Start;

        // var start = Fancy.Colors[startKey].Start;
        var middle = Fancy.Colors[middleKey].Middle;

        return middle != null
            ? Utils.CreateGradient(start, middle, end)
            : Utils.CreateGradient(start, end ?? start);
    }

    private static string GetEndColor(string key, Role role)
    {
        var (_, endVal, majorVal, _, lethalVal, horsemanVal) = Fancy.Colors[key];

		var isPower = role.GetSubAlignment() is SubAlignment.POWER or (SubAlignment)37;
		var isTownGovernment = Constants.IsBTOS2() ? role.GetSubAlignment() == (SubAlignment)38 : role is Role.MAYOR or Role.MONARCH;
		var isCultist = Constants.IsBTOS2() ? role is Btos2Role.Cultist : role is Role.CULTIST;
		var isCatalyst = Constants.IsBTOS2() ? role is Btos2Role.Catalyst : role is Role.CATALYST;

		var isMajor = Fancy.MajorColors.Value && ((isPower && !(Fancy.ExcludeTownGovernmentRoles.Value && isTownGovernment)) || (isCultist && !Fancy.ExcludeCultist.Value) || (isCatalyst && !Fancy.ExcludeCatalyst.Value));

        /* var isMajor = Fancy.MajorColors.Value &&
            (role.GetSubAlignment() == SubAlignment.POWER || (Constants.IsBTOS2() && role.GetSubAlignment() is (SubAlignment)37 or (SubAlignment)38)
            || (Constants.IsBTOS2() ? (role is Btos2Role.Cultist or Btos2Role.Catalyst) : (role is Role.CULTIST or Role.CATALYST)));*/

        var isHorseman = Fancy.HorsemenColors.Value &&
            (role is Role.FAMINE or Role.WAR or
            Role.PESTILENCE or Role.DEATH);

        var isLethal = Fancy.LethalColors.Value &&
            (role.GetSubAlignment() == SubAlignment.KILLING ||
            role == Role.BERSERKER ||
            (role == Role.JAILOR && !Fancy.MajorColors.Value));

        return isMajor && majorVal != null
            ? majorVal
            : isHorseman && horsemanVal != null
            ? horsemanVal
            : isLethal && lethalVal != null
                ? lethalVal
                : endVal;
    }

    public static string GetVerticalGradientKey(Gradient gradient)
    {
        foreach (var kvp in verticalGradients)
            if (kvp.Key.Equals(gradient))
                return kvp.Value;
        var name = "FancyGradient" + (verticalGradients.Count + 1);
        var hashCode = TMP_TextUtilities.GetHashCode(name);
        var color1 = gradient.Evaluate(0f);
        var color2 = gradient.Evaluate(1f);
        TMP_ColorGradient colorGradient = ScriptableObject.CreateInstance<TMP_ColorGradient>();
        colorGradient.name = name;
        colorGradient.colorMode = ColorMode.VerticalGradient;
        colorGradient.topLeft = color1;
        colorGradient.topRight = color1;
        colorGradient.bottomLeft = color2;
        colorGradient.bottomRight = color2;
        MaterialReferenceManager.AddColorGradientPreset(hashCode, colorGradient);
        verticalGradients.Add(gradient, name);
        return name;
    }

    public static Dictionary<Gradient, string> verticalGradients = [];
}
