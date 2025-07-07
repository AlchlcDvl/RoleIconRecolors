using Server.Shared.Extensions;

namespace FancyUI;

public static class Gradients
{
    public static Gradient GetChangedGradient(this FactionType faction, Role role)
    {
        var mod = Utils.GetGameType();
        var middleKey = Utils.FactionName(faction, mod, stoned: true).ToUpper();
        var baseKey = Utils.FactionName(role.GetFactionType(mod), mod, stoned: true).ToUpper();
        var startKey = faction == Btos2Faction.Jackal ? "JACKAL" : middleKey;
        string end;

        if (faction == Btos2Faction.Jackal && role != Btos2Role.Jackal)
        {
            end = Fancy.RecruitEndingColor.Value switch
            {
                RecruitEndType.FactionStart => Fancy.Colors.TryGetValue(baseKey, out var baseTuple) ? baseTuple.Start : Fancy.Colors["JACKAL"].End,
                RecruitEndType.FactionEnd => GetEndColor(baseKey, role),
                _ => GetEndColor("JACKAL", role),
            };
        }
        else
            end = GetEndColor(middleKey, role);

        var start = Fancy.Colors[startKey].Start;
        var middle = Fancy.Colors[middleKey].Middle;

        return middle != null
            ? Utils.CreateGradient(start, middle, end)
            : Utils.CreateGradient(start, end ?? start);
    }

    private static string GetEndColor(string key, Role role)
    {
        var (_, endVal, majorVal, _, lethalVal) = Fancy.Colors[key];

        var isMajor = Fancy.MajorColors.Value &&
            (role.GetSubAlignment() == SubAlignment.POWER || (Constants.IsBTOS2() && role.GetSubAlignment() is (SubAlignment)37 or (SubAlignment)38) || role is Role.FAMINE or Role.WAR or
            Role.PESTILENCE or Role.DEATH || (Constants.IsBTOS2() ? (role is Btos2Role.Cultist or Btos2Role.Catalyst) : (role is Role.CULTIST or Role.CATALYST)));

        var isLethal = Fancy.LethalColors.Value &&
            (role.GetSubAlignment() == SubAlignment.KILLING ||
            role == Role.BERSERKER ||
            (role == Role.JAILOR && !Fancy.MajorColors.Value));

        return isMajor && majorVal != null
            ? majorVal
            : isLethal && lethalVal != null
                ? lethalVal
                : endVal;
    }

    public static string GetVerticalGradientKey(Gradient gradient)
    {
        foreach (KeyValuePair<Gradient, string> kvp in verticalGradients)
            if (kvp.Key.Equals(gradient))
                return kvp.Value;
        string name = "FancyGradient" + (verticalGradients.Count + 1);
        int hashCode = TMP_TextUtilities.GetHashCode(name);
        Color color1 = gradient.Evaluate(0f);
        Color color2 = gradient.Evaluate(1f);
        TMP_ColorGradient colorGradient = new(color1, color1, color2, color2);
        colorGradient.name = name;
        colorGradient.colorMode = ColorMode.VerticalGradient;
        MaterialReferenceManager.AddColorGradientPreset(hashCode, colorGradient);
        verticalGradients.Add(gradient, name);
        return name;
    }

    public static Dictionary<Gradient, string> verticalGradients = new Dictionary<Gradient, string>();
}
