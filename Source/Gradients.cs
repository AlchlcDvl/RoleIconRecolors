using Server.Shared.Extensions;

namespace FancyUI;

public static class Gradients
{
    public static Gradient GetChangedGradient(this FactionType faction, Role role, bool isTesting = false)
    {
        var mod = Utils.GetGameType();
        var middleKey = isTesting ? Utils.GetFactionTestingKey(faction) : Utils.GetFactionKey(faction);
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
        if (VerticalGradients.TryGetValue(gradient, out var value))
            return value;

        var name = "FancyGradient" + (VerticalGradients.Count + 1);
        var hashCode = TMP_TextUtilities.GetHashCode(name);
        var color1 = gradient.Evaluate(0f);
        var color2 = gradient.Evaluate(1f);
        var colorGradient = ScriptableObject.CreateInstance<TMP_ColorGradient>();
        colorGradient.name = name;
        colorGradient.colorMode = ColorMode.VerticalGradient;
        colorGradient.topLeft = color1;
        colorGradient.topRight = color1;
        colorGradient.bottomLeft = color2;
        colorGradient.bottomRight = color2;
        MaterialReferenceManager.AddColorGradientPreset(hashCode, colorGradient);
        VerticalGradients.Add(gradient, name);
        return name;
    }

    private static readonly Dictionary<Gradient, string> VerticalGradients = [];
}
