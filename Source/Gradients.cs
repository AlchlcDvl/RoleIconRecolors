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

        if (faction == Btos2Faction.Jackal)
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
            (role.GetSubAlignment() == SubAlignment.POWER ||
            role is Role.FAMINE or Role.WAR or Role.PESTILENCE or Role.DEATH);

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
}
