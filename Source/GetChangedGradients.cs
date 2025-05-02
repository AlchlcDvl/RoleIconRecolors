using Server.Shared.Extensions;

namespace FancyUI;

// TODO: Get back to this
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
                    array[0] = new(Fancy.Colors["TOWN"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["TOWN"].Major.ToColor(), 1f);
                    goto setmajor;

                case FactionType.COVEN:
                    array[0] = new(Fancy.Colors["COVEN"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["COVEN"].Major.ToColor(), 1f);
                    goto setmajor;

                case FactionType.APOCALYPSE:
                    array[0] = new(Fancy.Colors["APOCALYPSE"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["APOCALYPSE"].Major.ToColor(), 1f);
                    goto setmajor;

                case FactionType.SERIALKILLER:
                    array[0] = new(Fancy.Colors["SERIALKILLER"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["SERIALKILLER"].Major.ToColor(), 1f);
                    goto setmajor;

                case FactionType.ARSONIST:
                    array[0] = new(Fancy.Colors["ARSONIST"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["ARSONIST"].Major.ToColor(), 1f);
                    goto setmajor;

                case FactionType.WEREWOLF:
                    array[0] = new(Fancy.Colors["WEREWOLF"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["WEREWOLF"].Major.ToColor(), 1f);
                    goto setmajor;

                case FactionType.SHROUD:
                    array[0] = new(Fancy.Colors["SHROUD"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["SHROUD"].Major.ToColor(), 1f);
                    goto setmajor;

                case FactionType.EXECUTIONER:
                    array[0] = new(Fancy.Colors["EXECUTIONER"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["EXECUTIONER"].End.ToColor(), 1f);
                    goto setmajor;

                case FactionType.JESTER:
                    array[0] = new(Fancy.Colors["JESTER"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["JESTER"].End.ToColor(), 1f);
                    goto setmajor;

                case (FactionType)40:
                    array[0] = new(Fancy.Colors["INQUISITOR"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["INQUISITOR"].End.ToColor(), 1f);
                    goto setmajor;

                case FactionType.PIRATE:
                    array[0] = new(Fancy.Colors["PIRATE"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["PIRATE"].End.ToColor(), 1f);
                    goto setmajor;

                case FactionType.DOOMSAYER:
                    array[0] = new(Fancy.Colors["DOOMSAYER"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["DOOMSAYER"].End.ToColor(), 1f);
                    goto setmajor;

                case FactionType.VAMPIRE:
                    array[0] = new(Fancy.Colors["VAMPIRE"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["VAMPIRE"].Major.ToColor(), 1f);
                    goto setmajor;

                case FactionType.CURSED_SOUL:
                    array[0] = new(Fancy.Colors["CURSEDSOUL"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["CURSEDSOUL"].Major.ToColor(), 1f);
                    goto setmajor;

                case (FactionType)33:
                    switch (Fancy.RecruitEndingColor.Value)
                    {
                        case RecruitEndType.JackalEnd:
                            array[0] = new(Fancy.Colors["JACKAL"].Start.ToColor(), 0f);
                            array[1] = new(Fancy.Colors["JACKAL"].Major.ToColor(), 1f);
                            goto setmajor;

                        case RecruitEndType.FactionStart:
                            array[0] = new(Fancy.Colors["JACKAL"].Start.ToColor(), 0f);
                            switch (role.GetFaction())
                            {
                                case FactionType.TOWN:
                                    array[1] = new(Fancy.Colors["TOWN"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.COVEN:
                                    array[1] = new(Fancy.Colors["COVEN"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.SERIALKILLER:
                                    array[1] = new(Fancy.Colors["SERIALKILLER"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.ARSONIST:
                                    array[1] = new(Fancy.Colors["ARSONIST"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.WEREWOLF:
                                    array[1] = new(Fancy.Colors["WEREWOLF"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.SHROUD:
                                    array[1] = new(Fancy.Colors["SHROUD"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.APOCALYPSE:
                                    array[1] = new(Fancy.Colors["APOCALYPSE"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.EXECUTIONER:
                                    array[1] = new(Fancy.Colors["EXECUTIONER"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.JESTER:
                                    array[1] = new(Fancy.Colors["JESTER"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.PIRATE:
                                    array[1] = new(Fancy.Colors["PIRATE"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.DOOMSAYER:
                                    array[1] = new(Fancy.Colors["DOOMSAYER"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.VAMPIRE:
                                    array[1] = new(Fancy.Colors["VAMPIRE"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.CURSED_SOUL:
                                    array[1] = new(Fancy.Colors["CURSEDSOUL"].Start.ToColor(), 1f);
                                    break;
                                case (FactionType)38:
                                    array[1] = new(Fancy.Colors["JUDGE"].Start.ToColor(), 1f);
                                    break;
                                case (FactionType)39:
                                    array[1] = new(Fancy.Colors["AUDITOR"].Start.ToColor(), 1f);
                                    break;
                                case (FactionType)40:
                                    array[1] = new(Fancy.Colors["INQUISITOR"].Start.ToColor(), 1f);
                                    break;
                                case (FactionType)41:
                                    array[1] = new(Fancy.Colors["STARSPAWN"].Start.ToColor(), 1f);
                                    break;
                                default:
                                    array[1] = new(Fancy.Colors["JACKAL"].Major.ToColor(), 1f);
                                    break;
                            }
                            goto setmajor;

                        case RecruitEndType.FactionEnd:
                            array[0] = new(Fancy.Colors["JACKAL"].Start.ToColor(), 1f);
                            switch (role.GetFaction())
                            {
                                case FactionType.TOWN:
                                    array[1] = new(Fancy.Colors["TOWN"].Major.ToColor(), 1f);
                                    break;
                                case FactionType.COVEN:
                                    array[1] = new(Fancy.Colors["COVEN"].Major.ToColor(), 1f);
                                    break;
                                case FactionType.SERIALKILLER:
                                    array[1] = new(Fancy.Colors["SERIALKILLER"].Major.ToColor(), 1f);
                                    break;
                                case FactionType.ARSONIST:
                                    array[1] = new(Fancy.Colors["ARSONIST"].Major.ToColor(), 1f);
                                    break;
                                case FactionType.WEREWOLF:
                                    array[1] = new(Fancy.Colors["WEREWOLF"].Major.ToColor(), 1f);
                                    break;
                                case FactionType.SHROUD:
                                    array[1] = new(Fancy.Colors["SHROUD"].Major.ToColor(), 1f);
                                    break;
                                case FactionType.APOCALYPSE:
                                    array[1] = new(Fancy.Colors["APOCALYPSE"].Major.ToColor(), 1f);
                                    break;
                                case FactionType.EXECUTIONER:
                                    array[1] = new(Fancy.Colors["EXECUTIONER"].End.ToColor(), 1f);
                                    break;
                                case FactionType.JESTER:
                                    array[1] = new(Fancy.Colors["JESTER"].End.ToColor(), 1f);
                                    break;
                                case FactionType.PIRATE:
                                    array[1] = new(Fancy.Colors["PIRATE"].End.ToColor(), 1f);
                                    break;
                                case FactionType.DOOMSAYER:
                                    array[1] = new(Fancy.Colors["DOOMSAYER"].End.ToColor(), 1f);
                                    break;
                                case FactionType.VAMPIRE:
                                    array[1] = new(Fancy.Colors["VAMPIRE"].Major.ToColor(), 1f);
                                    break;
                                case FactionType.CURSED_SOUL:
                                    array[1] = new(Fancy.Colors["CURSEDSOUL"].Major.ToColor(), 1f);
                                    break;
                                case (FactionType)38:
                                    array[1] = new(Fancy.Colors["JUDGE"].End.ToColor(), 1f);
                                    break;
                                case (FactionType)39:
                                    array[1] = new(Fancy.Colors["AUDITOR"].End.ToColor(), 1f);
                                    break;
                                case (FactionType)40:
                                    array[1] = new(Fancy.Colors["INQUISITOR"].End.ToColor(), 1f);
                                    break;
                                case (FactionType)41:
                                    array[1] = new(Fancy.Colors["STARSPAWN"].End.ToColor(), 1f);
                                    break;
                                default:
                                    array[1] = new(Fancy.Colors["JACKAL"].Major.ToColor(), 1f);
                                    break;
                            }
                            goto setmajor;

                        default:
                            array[0] = new(Fancy.Colors["JACKAL"].Start.ToColor(), 0f);
                            array[1] = new(Fancy.Colors["JACKAL"].Major.ToColor(), 1f);
                            goto setmajor;
                    }

                case (FactionType)38:
                    array[0] = new(Fancy.Colors["JUDGE"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["JUDGE"].End.ToColor(), 1f);
                    goto setmajor;

                case (FactionType)39:
                    array[0] = new(Fancy.Colors["AUDITOR"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["AUDITOR"].End.ToColor(), 1f);
                    goto setmajor;

                case (FactionType)41:
                    array[0] = new(Fancy.Colors["STARSPAWN"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["STARSPAWN"].End.ToColor(), 1f);
                    goto setmajor;

                case (FactionType)42:
                    array[0] = new(Fancy.Colors["EGOTIST"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["EGOTIST"].Major.ToColor(), 1f);
                    goto setmajor;

                case (FactionType)43:
                    array[0] = new(Fancy.Colors["PANDORA"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["PANDORA"].Major.ToColor(), 1f);
                    goto setmajor;

                case (FactionType)34:
                    array[0] = new(Fancy.Colors["FROGS"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["FROGS"].Major.ToColor(), 1f);
                    goto setmajor;

                case (FactionType)35:
                    array[0] = new(Fancy.Colors["LIONS"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["LIONS"].Major.ToColor(), 1f);
                    goto setmajor;

                case (FactionType)36:
                    array[0] = new(Fancy.Colors["HAWKS"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["HAWKS"].Major.ToColor(), 1f);
                    goto setmajor;

                case (FactionType)44:
                    array =
                    [
                        new(Fancy.Colors["COMPLIANCE"].Start.ToColor(), 0f),
                        new(Fancy.Colors["COMPLIANCE"].Middle.ToColor(), 0.5f),
                        new(Fancy.Colors["COMPLIANCE"].Major.ToColor(), 1f)
                    ];
                    goto setmajor;

                default:
                    array[0] = new(Fancy.Colors["STONED_HIDDEN"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["STONED_HIDDEN"].Start.ToColor(), 1f);
                    goto setmajor;
            }

setmajor:
            array2[0] = new(1f, 0f);
            array2[1] = new(1f, 1f);
            gradient.SetKeys(array, array2);
            result = gradient;
        }
        else if (Fancy.LethalColors.Value && (role.GetSubAlignment() == SubAlignment.KILLING || role == Role.BERSERKER || role == Role.JAILOR && !Fancy.MajorColors.Value))
        {
            switch (faction)
            {
                case FactionType.TOWN:
                    array[0] = new(Fancy.Colors["TOWN"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["TOWN"].Lethal.ToColor(), 1f);
                    goto setlethal;

                case FactionType.COVEN:
                    array[0] = new(Fancy.Colors["COVEN"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["COVEN"].Lethal.ToColor(), 1f);
                    goto setlethal;

                case FactionType.APOCALYPSE:
                    array[0] = new(Fancy.Colors["APOCALYPSE"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["APOCALYPSE"].Lethal.ToColor(), 1f);
                    goto setlethal;

                case FactionType.SERIALKILLER:
                    array[0] = new(Fancy.Colors["SERIALKILLER"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["SERIALKILLER"].End.ToColor(), 1f);
                    goto setlethal;

                case FactionType.ARSONIST:
                    array[0] = new(Fancy.Colors["ARSONIST"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["ARSONIST"].End.ToColor(), 1f);
                    goto setlethal;

                case FactionType.WEREWOLF:
                    array[0] = new(Fancy.Colors["WEREWOLF"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["WEREWOLF"].End.ToColor(), 1f);
                    goto setlethal;

                case FactionType.SHROUD:
                    array[0] = new(Fancy.Colors["SHROUD"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["SHROUD"].End.ToColor(), 1f);
                    goto setlethal;

                case FactionType.EXECUTIONER:
                    array[0] = new(Fancy.Colors["EXECUTIONER"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["EXECUTIONER"].End.ToColor(), 1f);
                    goto setlethal;

                case FactionType.JESTER:
                    array[0] = new(Fancy.Colors["JESTER"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["JESTER"].End.ToColor(), 1f);
                    goto setlethal;

                case (FactionType)40:
                    array[0] = new(Fancy.Colors["INQUISITOR"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["INQUISITOR"].End.ToColor(), 1f);
                    goto setlethal;

                case FactionType.PIRATE:
                    array[0] = new(Fancy.Colors["PIRATE"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["PIRATE"].End.ToColor(), 1f);
                    goto setlethal;

                case FactionType.DOOMSAYER:
                    array[0] = new(Fancy.Colors["DOOMSAYER"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["DOOMSAYER"].End.ToColor(), 1f);
                    goto setlethal;

                case FactionType.VAMPIRE:
                    array[0] = new(Fancy.Colors["VAMPIRE"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["VAMPIRE"].Lethal.ToColor(), 1f);
                    goto setlethal;

                case FactionType.CURSED_SOUL:
                    array[0] = new(Fancy.Colors["CURSEDSOUL"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["CURSEDSOUL"].Lethal.ToColor(), 1f);
                    goto setlethal;

                case (FactionType)33:
                    switch (Fancy.RecruitEndingColor.Value)
                    {
                        case RecruitEndType.JackalEnd:
                            array[0] = new(Fancy.Colors["JACKAL"].Start.ToColor(), 0f);
                            array[1] = new(Fancy.Colors["JACKAL"].Lethal.ToColor(), 1f);
                            goto setlethal;

                        case RecruitEndType.FactionStart:
                            array[0] = new(Fancy.Colors["JACKAL"].Start.ToColor(), 0f);
                            switch (role.GetFaction())
                            {
                                case FactionType.TOWN:
                                    array[1] = new(Fancy.Colors["TOWN"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.COVEN:
                                    array[1] = new(Fancy.Colors["COVEN"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.SERIALKILLER:
                                    array[1] = new(Fancy.Colors["SERIALKILLER"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.ARSONIST:
                                    array[1] = new(Fancy.Colors["ARSONIST"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.WEREWOLF:
                                    array[1] = new(Fancy.Colors["WEREWOLF"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.SHROUD:
                                    array[1] = new(Fancy.Colors["SHROUD"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.APOCALYPSE:
                                    array[1] = new(Fancy.Colors["APOCALYPSE"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.EXECUTIONER:
                                    array[1] = new(Fancy.Colors["EXECUTIONER"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.JESTER:
                                    array[1] = new(Fancy.Colors["JESTER"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.PIRATE:
                                    array[1] = new(Fancy.Colors["PIRATE"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.DOOMSAYER:
                                    array[1] = new(Fancy.Colors["DOOMSAYER"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.VAMPIRE:
                                    array[1] = new(Fancy.Colors["VAMPIRE"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.CURSED_SOUL:
                                    array[1] = new(Fancy.Colors["CURSEDSOUL"].Start.ToColor(), 1f);
                                    break;
                                case (FactionType)38:
                                    array[1] = new(Fancy.Colors["JUDGE"].Start.ToColor(), 1f);
                                    break;
                                case (FactionType)39:
                                    array[1] = new(Fancy.Colors["AUDITOR"].Start.ToColor(), 1f);
                                    break;
                                case (FactionType)40:
                                    array[1] = new(Fancy.Colors["INQUISITOR"].Start.ToColor(), 1f);
                                    break;
                                case (FactionType)41:
                                    array[1] = new(Fancy.Colors["STARSPAWN"].Start.ToColor(), 1f);
                                    break;
                                default:
                                    array[1] = new(Fancy.Colors["JACKAL"].Lethal.ToColor(), 1f);
                                    break;
                            }
                            goto setlethal;

                        case RecruitEndType.FactionEnd:
                            array[0] = new(Fancy.Colors["JACKAL"].Start.ToColor(), 1f);
                            switch (role.GetFaction())
                            {
                                case FactionType.TOWN:
                                    array[1] = new(Fancy.Colors["TOWN"].Lethal.ToColor(), 1f);
                                    break;
                                case FactionType.COVEN:
                                    array[1] = new(Fancy.Colors["COVEN"].Lethal.ToColor(), 1f);
                                    break;
                                case FactionType.SERIALKILLER:
                                    array[1] = new(Fancy.Colors["SERIALKILLER"].End.ToColor(), 1f);
                                    break;
                                case FactionType.ARSONIST:
                                    array[1] = new(Fancy.Colors["ARSONIST"].End.ToColor(), 1f);
                                    break;
                                case FactionType.WEREWOLF:
                                    array[1] = new(Fancy.Colors["WEREWOLF"].End.ToColor(), 1f);
                                    break;
                                case FactionType.SHROUD:
                                    array[1] = new(Fancy.Colors["SHROUD"].End.ToColor(), 1f);
                                    break;
                                case FactionType.APOCALYPSE:
                                    array[1] = new(Fancy.Colors["APOCALYPSE"].Lethal.ToColor(), 1f);
                                    break;
                                case FactionType.EXECUTIONER:
                                    array[1] = new(Fancy.Colors["EXECUTIONER"].End.ToColor(), 1f);
                                    break;
                                case FactionType.JESTER:
                                    array[1] = new(Fancy.Colors["JESTER"].End.ToColor(), 1f);
                                    break;
                                case FactionType.PIRATE:
                                    array[1] = new(Fancy.Colors["PIRATE"].End.ToColor(), 1f);
                                    break;
                                case FactionType.DOOMSAYER:
                                    array[1] = new(Fancy.Colors["DOOMSAYER"].End.ToColor(), 1f);
                                    break;
                                case FactionType.VAMPIRE:
                                    array[1] = new(Fancy.Colors["VAMPIRE"].Lethal.ToColor(), 1f);
                                    break;
                                case FactionType.CURSED_SOUL:
                                    array[1] = new(Fancy.Colors["CURSEDSOUL"].Lethal.ToColor(), 1f);
                                    break;
                                case (FactionType)38:
                                    array[1] = new(Fancy.Colors["JUDGE"].End.ToColor(), 1f);
                                    break;
                                case (FactionType)39:
                                    array[1] = new(Fancy.Colors["AUDITOR"].End.ToColor(), 1f);
                                    break;
                                case (FactionType)40:
                                    array[1] = new(Fancy.Colors["INQUISITOR"].End.ToColor(), 1f);
                                    break;
                                case (FactionType)41:
                                    array[1] = new(Fancy.Colors["STARSPAWN"].End.ToColor(), 1f);
                                    break;
                                default:
                                    array[1] = new(Fancy.Colors["JACKAL"].Lethal.ToColor(), 1f);
                                    break;
                            }
                            goto setlethal;

                        default:
                            array[0] = new(Fancy.Colors["JACKAL"].Start.ToColor(), 0f);
                            array[1] = new(Fancy.Colors["JACKAL"].Lethal.ToColor(), 1f);
                            goto setlethal;
                    }

                case (FactionType)38:
                    array[0] = new(Fancy.Colors["JUDGE"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["JUDGE"].End.ToColor(), 1f);
                    goto setlethal;

                case (FactionType)39:
                    array[0] = new(Fancy.Colors["AUDITOR"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["AUDITOR"].End.ToColor(), 1f);
                    goto setlethal;

                case (FactionType)41:
                    array[0] = new(Fancy.Colors["STARSPAWN"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["STARSPAWN"].End.ToColor(), 1f);
                    goto setlethal;

                case (FactionType)42:
                    array[0] = new(Fancy.Colors["EGOTIST"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["EGOTIST"].Lethal.ToColor(), 1f);
                    goto setlethal;

                case (FactionType)43:
                    array[0] = new(Fancy.Colors["PANDORA"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["PANDORA"].Lethal.ToColor(), 1f);
                    goto setlethal;

                case (FactionType)34:
                    array[0] = new(Fancy.Colors["FROGS"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["FROGS"].Lethal.ToColor(), 1f);
                    goto setlethal;

                case (FactionType)35:
                    array[0] = new(Fancy.Colors["LIONS"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["LIONS"].Lethal.ToColor(), 1f);
                    goto setlethal;

                case (FactionType)36:
                    array[0] = new(Fancy.Colors["HAWKS"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["HAWKS"].Lethal.ToColor(), 1f);
                    goto setlethal;

                case (FactionType)44:
                    array =
                    [
                        new(Fancy.Colors["COMPLIANCE"].Start.ToColor(), 0f),
                        new(Fancy.Colors["COMPLIANCE"].Middle.ToColor(), 0.5f),
                        new(Fancy.Colors["COMPLIANCE"].End.ToColor(), 1f)
                    ];
                    goto setlethal;

                default:
                    array[0] = new(Fancy.Colors["STONED_HIDDEN"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["STONED_HIDDEN"].Start.ToColor(), 1f);
                    goto setlethal;
            }

setlethal:
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
                    array[0] = new(Fancy.Colors["TOWN"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["TOWN"].End.ToColor(), 1f);
                    goto setgradient;

                case FactionType.COVEN:
                    array[0] = new(Fancy.Colors["COVEN"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["COVEN"].End.ToColor(), 1f);
                    goto setgradient;

                case FactionType.APOCALYPSE:
                    array[0] = new(Fancy.Colors["APOCALYPSE"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["APOCALYPSE"].End.ToColor(), 1f);
                    goto setgradient;

                case FactionType.SERIALKILLER:
                    array[0] = new(Fancy.Colors["SERIALKILLER"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["SERIALKILLER"].End.ToColor(), 1f);
                    goto setgradient;

                case FactionType.ARSONIST:
                    array[0] = new(Fancy.Colors["ARSONIST"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["ARSONIST"].End.ToColor(), 1f);
                    goto setgradient;

                case FactionType.WEREWOLF:
                    array[0] = new(Fancy.Colors["WEREWOLF"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["WEREWOLF"].End.ToColor(), 1f);
                    goto setgradient;

                case FactionType.SHROUD:
                    array[0] = new(Fancy.Colors["SHROUD"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["SHROUD"].End.ToColor(), 1f);
                    goto setgradient;

                case FactionType.EXECUTIONER:
                    array[0] = new(Fancy.Colors["EXECUTIONER"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["EXECUTIONER"].End.ToColor(), 1f);
                    goto setgradient;

                case FactionType.JESTER:
                    array[0] = new(Fancy.Colors["JESTER"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["JESTER"].End.ToColor(), 1f);
                    goto setgradient;

                case (FactionType)40:
                    array[0] = new(Fancy.Colors["INQUISITOR"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["INQUISITOR"].End.ToColor(), 1f);
                    goto setgradient;

                case FactionType.PIRATE:
                    array[0] = new(Fancy.Colors["PIRATE"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["PIRATE"].End.ToColor(), 1f);
                    goto setgradient;

                case FactionType.DOOMSAYER:
                    array[0] = new(Fancy.Colors["DOOMSAYER"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["DOOMSAYER"].End.ToColor(), 1f);
                    goto setgradient;

                case FactionType.VAMPIRE:
                    array[0] = new(Fancy.Colors["VAMPIRE"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["VAMPIRE"].End.ToColor(), 1f);
                    goto setgradient;

                case FactionType.CURSED_SOUL:
                    array[0] = new(Fancy.Colors["CURSEDSOUL"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["CURSEDSOUL"].End.ToColor(), 1f);
                    goto setgradient;

                case (FactionType)33:
                    switch (Fancy.RecruitEndingColor.Value)
                    {
                        case RecruitEndType.JackalEnd:
                            array[0] = new(Fancy.Colors["JACKAL"].Start.ToColor(), 0f);
                            array[1] = new(Fancy.Colors["JACKAL"].End.ToColor(), 1f);
                            goto setgradient;

                        case RecruitEndType.FactionStart:
                            array[0] = new(Fancy.Colors["JACKAL"].Start.ToColor(), 0f);
                            switch (role.GetFaction())
                            {
                                case FactionType.TOWN:
                                    array[1] = new(Fancy.Colors["TOWN"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.COVEN:
                                    array[1] = new(Fancy.Colors["COVEN"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.SERIALKILLER:
                                    array[1] = new(Fancy.Colors["SERIALKILLER"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.ARSONIST:
                                    array[1] = new(Fancy.Colors["ARSONIST"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.WEREWOLF:
                                    array[1] = new(Fancy.Colors["WEREWOLF"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.SHROUD:
                                    array[1] = new(Fancy.Colors["SHROUD"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.APOCALYPSE:
                                    array[1] = new(Fancy.Colors["APOCALYPSE"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.EXECUTIONER:
                                    array[1] = new(Fancy.Colors["EXECUTIONER"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.JESTER:
                                    array[1] = new(Fancy.Colors["JESTER"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.PIRATE:
                                    array[1] = new(Fancy.Colors["PIRATE"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.DOOMSAYER:
                                    array[1] = new(Fancy.Colors["DOOMSAYER"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.VAMPIRE:
                                    array[1] = new(Fancy.Colors["VAMPIRE"].Start.ToColor(), 1f);
                                    break;
                                case FactionType.CURSED_SOUL:
                                    array[1] = new(Fancy.Colors["CURSEDSOUL"].Start.ToColor(), 1f);
                                    break;
                                case (FactionType)38:
                                    array[1] = new(Fancy.Colors["JUDGE"].Start.ToColor(), 1f);
                                    break;
                                case (FactionType)39:
                                    array[1] = new(Fancy.Colors["AUDITOR"].Start.ToColor(), 1f);
                                    break;
                                case (FactionType)40:
                                    array[1] = new(Fancy.Colors["INQUISITOR"].Start.ToColor(), 1f);
                                    break;
                                case (FactionType)41:
                                    array[1] = new(Fancy.Colors["STARSPAWN"].Start.ToColor(), 1f);
                                    break;
                                default:
                                    array[1] = new(Fancy.Colors["JACKAL"].End.ToColor(), 1f);
                                    break;
                            }
                            goto setgradient;

                        case RecruitEndType.FactionEnd:
                            array[0] = new(Fancy.Colors["JACKAL"].Start.ToColor(), 1f);
                            switch (role.GetFaction())
                            {
                                case FactionType.TOWN:
                                    array[1] = new(Fancy.Colors["TOWN"].End.ToColor(), 1f);
                                    break;
                                case FactionType.COVEN:
                                    array[1] = new(Fancy.Colors["COVEN"].End.ToColor(), 1f);
                                    break;
                                case FactionType.SERIALKILLER:
                                    array[1] = new(Fancy.Colors["SERIALKILLER"].End.ToColor(), 1f);
                                    break;
                                case FactionType.ARSONIST:
                                    array[1] = new(Fancy.Colors["ARSONIST"].End.ToColor(), 1f);
                                    break;
                                case FactionType.WEREWOLF:
                                    array[1] = new(Fancy.Colors["WEREWOLF"].End.ToColor(), 1f);
                                    break;
                                case FactionType.SHROUD:
                                    array[1] = new(Fancy.Colors["SHROUD"].End.ToColor(), 1f);
                                    break;
                                case FactionType.APOCALYPSE:
                                    array[1] = new(Fancy.Colors["APOCALYPSE"].End.ToColor(), 1f);
                                    break;
                                case FactionType.EXECUTIONER:
                                    array[1] = new(Fancy.Colors["EXECUTIONER"].End.ToColor(), 1f);
                                    break;
                                case FactionType.JESTER:
                                    array[1] = new(Fancy.Colors["JESTER"].End.ToColor(), 1f);
                                    break;
                                case FactionType.PIRATE:
                                    array[1] = new(Fancy.Colors["PIRATE"].End.ToColor(), 1f);
                                    break;
                                case FactionType.DOOMSAYER:
                                    array[1] = new(Fancy.Colors["DOOMSAYER"].End.ToColor(), 1f);
                                    break;
                                case FactionType.VAMPIRE:
                                    array[1] = new(Fancy.Colors["VAMPIRE"].End.ToColor(), 1f);
                                    break;
                                case FactionType.CURSED_SOUL:
                                    array[1] = new(Fancy.Colors["CURSEDSOUL"].End.ToColor(), 1f);
                                    break;
                                case (FactionType)38:
                                    array[1] = new(Fancy.Colors["JUDGE"].End.ToColor(), 1f);
                                    break;
                                case (FactionType)39:
                                    array[1] = new(Fancy.Colors["AUDITOR"].End.ToColor(), 1f);
                                    break;
                                case (FactionType)40:
                                    array[1] = new(Fancy.Colors["INQUISITOR"].End.ToColor(), 1f);
                                    break;
                                case (FactionType)41:
                                    array[1] = new(Fancy.Colors["STARSPAWN"].End.ToColor(), 1f);
                                    break;
                                default:
                                    array[1] = new(Fancy.Colors["JACKAL"].End.ToColor(), 1f);
                                    break;
                            }
                            goto setgradient;

                        default:
                            array[0] = new(Fancy.Colors["JACKAL"].Start.ToColor(), 0f);
                            array[1] = new(Fancy.Colors["JACKAL"].End.ToColor(), 1f);
                            goto setgradient;
                    }

                case (FactionType)38:
                    array[0] = new(Fancy.Colors["JUDGE"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["JUDGE"].End.ToColor(), 1f);
                    goto setgradient;

                case (FactionType)39:
                    array[0] = new(Fancy.Colors["AUDITOR"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["AUDITOR"].End.ToColor(), 1f);
                    goto setgradient;

                case (FactionType)41:
                    array[0] = new(Fancy.Colors["STARSPAWN"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["STARSPAWN"].End.ToColor(), 1f);
                    goto setgradient;

                case (FactionType)42:
                    array[0] = new(Fancy.Colors["EGOTIST"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["EGOTIST"].End.ToColor(), 1f);
                    goto setgradient;

                case (FactionType)43:
                    array[0] = new(Fancy.Colors["PANDORA"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["PANDORA"].End.ToColor(), 1f);
                    goto setgradient;

                case (FactionType)34:
                    array[0] = new(Fancy.Colors["FROGS"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["FROGS"].End.ToColor(), 1f);
                    goto setgradient;

                case (FactionType)35:
                    array[0] = new(Fancy.Colors["LIONS"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["LIONS"].End.ToColor(), 1f);
                    goto setgradient;

                case (FactionType)36:
                    array[0] = new(Fancy.Colors["HAWKS"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["HAWKS"].End.ToColor(), 1f);
                    goto setgradient;

                case (FactionType)44:
                    array =
                    [
                        new(Fancy.Colors["COMPLIANCE"].Start.ToColor(), 0f),
                        new(Fancy.Colors["COMPLIANCE"].Middle.ToColor(), 0.5f),
                        new(Fancy.Colors["COMPLIANCE"].End.ToColor(), 1f)
                    ];
                    goto setgradient;

                default:
                    array[0] = new(Fancy.Colors["STONED_HIDDEN"].Start.ToColor(), 0f);
                    array[1] = new(Fancy.Colors["STONED_HIDDEN"].Start.ToColor(), 1f);
                    goto setgradient;

            }

setgradient:
            array2[0] = new(1f, 0f);
            array2[1] = new(1f, 1f);
            gradient.SetKeys(array, array2);
            result = gradient;

        }
        return result;
    }
}