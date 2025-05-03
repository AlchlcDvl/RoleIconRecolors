using Server.Shared.Extensions;

namespace FancyUI;

public static class GetChangedGradients
{
    public static Gradient GetChangedGradient(this FactionType faction, Role role)
    {
        var middleKey = (faction switch
        {
            FactionType.NONE or FactionType.UNKNOWN => "STONED_HIDDEN",
            _ => Utils.FactionName(faction),
        }).ToUpper();
        var baseFaction = role.GetFactionType();
        var baseKey = (baseFaction switch
        {
            FactionType.NONE or FactionType.UNKNOWN => "STONED_HIDDEN",
            _ => Utils.FactionName(baseFaction),
        }).ToUpper();
        var startKey = faction switch
        {
            Btos2Faction.Jackal => Fancy.RecruitEndingColor.Value switch
            {
                RecruitEndType.FactionStart => baseKey,
                _ => "JACKAL",
            },
            _ => middleKey,
        };
        var endKey = faction switch
        {
            Btos2Faction.Jackal => Fancy.RecruitEndingColor.Value switch
            {
                RecruitEndType.FactionEnd => baseKey,
                _ => "JACKAL",
            },
            _ => middleKey,
        };

        var start = Fancy.Colors[startKey].Start;
        var middle = Fancy.Colors[middleKey].Middle;

        var (_, endVal, majorVal, _, lethalVal) = Fancy.Colors[endKey];
        var isMajor = Fancy.MajorColors.Value && (role.GetSubAlignment() == SubAlignment.POWER || role is Role.FAMINE or Role.WAR or Role.PESTILENCE or Role.DEATH);
        var isLethal = Fancy.LethalColors.Value && (role.GetSubAlignment() == SubAlignment.KILLING || role == Role.BERSERKER || (role == Role.JAILOR && !Fancy.MajorColors.Value));
        var end = isMajor ? majorVal : (isLethal ? lethalVal : endVal);

        return middle != null ? Utils.CreateGradient(start, middle, end) : (end != null ? Utils.CreateGradient(start, end) : Utils.CreateGradient(start));
    }
}