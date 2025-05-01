using Server.Shared.Extensions;

namespace FancyUI;

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

                    default:
                        array[0] = new(Fancy.Colors["STONED_HIDDEN"].start.ToColor(), 0f);
                        array[1] = new(Fancy.Colors["STONED_HIDDEN"].start.ToColor(), 1f);
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
                        array[0] = new(Fancy.Colors["TOWN"].start.ToColor(), 0f);
                        array[1] = new(Fancy.Colors["TOWN"].lethal.ToColor(), 1f);
                        goto setlethal;

                    case FactionType.COVEN:
                        array[0] = new(Fancy.Colors["COVEN"].start.ToColor(), 0f);
                        array[1] = new(Fancy.Colors["COVEN"].lethal.ToColor(), 1f);
                        goto setlethal;

                    case FactionType.APOCALYPSE:
                        array[0] = new(Fancy.Colors["APOCALYPSE"].start.ToColor(), 0f);
                        array[1] = new(Fancy.Colors["APOCALYPSE"].lethal.ToColor(), 1f);
                        goto setlethal;

                    case FactionType.SERIALKILLER:
                        array[0] = new(Fancy.Colors["SERIALKILLER"].start.ToColor(), 0f);
                        array[1] = new(Fancy.Colors["SERIALKILLER"].end.ToColor(), 1f);
                        goto setlethal;

                    case FactionType.ARSONIST:
                        array[0] = new(Fancy.Colors["ARSONIST"].start.ToColor(), 0f);
                        array[1] = new(Fancy.Colors["ARSONIST"].end.ToColor(), 1f);
                        goto setlethal;

                    case FactionType.WEREWOLF:
                        array[0] = new(Fancy.Colors["WEREWOLF"].start.ToColor(), 0f);
                        array[1] = new(Fancy.Colors["WEREWOLF"].end.ToColor(), 1f);
                        goto setlethal;

                    case FactionType.SHROUD:
                        array[0] = new(Fancy.Colors["SHROUD"].start.ToColor(), 0f);
                        array[1] = new(Fancy.Colors["SHROUD"].end.ToColor(), 1f);
                        goto setlethal;

                    case FactionType.EXECUTIONER:
                        array[0] = new(Fancy.Colors["EXECUTIONER"].start.ToColor(), 0f);
                        array[1] = new(Fancy.Colors["EXECUTIONER"].end.ToColor(), 1f);
                        goto setlethal;

                    case FactionType.JESTER:
                        array[0] = new(Fancy.Colors["JESTER"].start.ToColor(), 0f);
                        array[1] = new(Fancy.Colors["JESTER"].end.ToColor(), 1f);
                        goto setlethal;

                    case (FactionType)40:
                        array[0] = new(Fancy.Colors["INQUISITOR"].start.ToColor(), 0f);
                        array[1] = new(Fancy.Colors["INQUISITOR"].end.ToColor(), 1f);
                        goto setlethal;

                    case FactionType.PIRATE:
                        array[0] = new(Fancy.Colors["PIRATE"].start.ToColor(), 0f);
                        array[1] = new(Fancy.Colors["PIRATE"].end.ToColor(), 1f);
                        goto setlethal;

                    case FactionType.DOOMSAYER:
                        array[0] = new(Fancy.Colors["DOOMSAYER"].start.ToColor(), 0f);
                        array[1] = new(Fancy.Colors["DOOMSAYER"].end.ToColor(), 1f);
                        goto setlethal;

                    case FactionType.VAMPIRE:
                        array[0] = new(Fancy.Colors["VAMPIRE"].start.ToColor(), 0f);
                        array[1] = new(Fancy.Colors["VAMPIRE"].lethal.ToColor(), 1f);
                        goto setlethal;

                    case FactionType.CURSED_SOUL:
                        array[0] = new(Fancy.Colors["CURSEDSOUL"].start.ToColor(), 0f);
                        array[1] = new(Fancy.Colors["CURSEDSOUL"].lethal.ToColor(), 1f);
                        goto setlethal;

                    case (FactionType)33:
                        switch (Fancy.RecruitEndingColor.Value)
                        {
                            case RecruitEndType.JackalEnd:
                                array[0] = new(Fancy.Colors["JACKAL"].start.ToColor(), 0f);
                                array[1] = new(Fancy.Colors["JACKAL"].lethal.ToColor(), 1f);
                                goto setlethal;

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
                                        array[1] = new(Fancy.Colors["JACKAL"].lethal.ToColor(), 1f);
                                        break;
                                }
                                goto setlethal;

                            case RecruitEndType.FactionEnd: 
                                array[0] = new(Fancy.Colors["JACKAL"].start.ToColor(), 1f);
                                switch (role.GetFaction())
                                {
                                    case FactionType.TOWN:
                                        array[1] = new(Fancy.Colors["TOWN"].lethal.ToColor(), 1f);
                                        break;
                                    case FactionType.COVEN:
                                        array[1] = new(Fancy.Colors["COVEN"].lethal.ToColor(), 1f);
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
                                        array[1] = new(Fancy.Colors["APOCALYPSE"].lethal.ToColor(), 1f);
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
                                        array[1] = new(Fancy.Colors["VAMPIRE"].lethal.ToColor(), 1f);
                                        break;
                                    case FactionType.CURSED_SOUL:
                                        array[1] = new(Fancy.Colors["CURSEDSOUL"].lethal.ToColor(), 1f);
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
                                        array[1] = new(Fancy.Colors["JACKAL"].lethal.ToColor(), 1f);
                                        break;
                                }
                                goto setlethal;

                            default:
                                array[0] = new(Fancy.Colors["JACKAL"].start.ToColor(), 0f);
                                array[1] = new(Fancy.Colors["JACKAL"].lethal.ToColor(), 1f);
                                goto setlethal;
                        }

                    case (FactionType)38:
                        array[0] = new(Fancy.Colors["JUDGE"].start.ToColor(), 0f);
                        array[1] = new(Fancy.Colors["JUDGE"].end.ToColor(), 1f);
                        goto setlethal;

                    case (FactionType)39:
                        array[0] = new(Fancy.Colors["AUDITOR"].start.ToColor(), 0f);
                        array[1] = new(Fancy.Colors["AUDITOR"].end.ToColor(), 1f);
                        goto setlethal;

                    case (FactionType)41:
                        array[0] = new(Fancy.Colors["STARSPAWN"].start.ToColor(), 0f);
                        array[1] = new(Fancy.Colors["STARSPAWN"].end.ToColor(), 1f);
                        goto setlethal;

                    case (FactionType)42:
                        array[0] = new(Fancy.Colors["EGOTIST"].start.ToColor(), 0f);
                        array[1] = new(Fancy.Colors["EGOTIST"].lethal.ToColor(), 1f);
                        goto setlethal;

                    case (FactionType)43:
                        array[0] = new(Fancy.Colors["PANDORA"].start.ToColor(), 0f);
                        array[1] = new(Fancy.Colors["PANDORA"].lethal.ToColor(), 1f);
                        goto setlethal;

                    case (FactionType)34:
                        array[0] = new(Fancy.Colors["FROGS"].start.ToColor(), 0f);
                        array[1] = new(Fancy.Colors["FROGS"].lethal.ToColor(), 1f);
                        goto setlethal;

                    case (FactionType)35:
                        array[0] = new(Fancy.Colors["LIONS"].start.ToColor(), 0f);
                        array[1] = new(Fancy.Colors["LIONS"].lethal.ToColor(), 1f);
                        goto setlethal;

                    case (FactionType)36:
                        array[0] = new(Fancy.Colors["HAWKS"].start.ToColor(), 0f);
                        array[1] = new(Fancy.Colors["HAWKS"].lethal.ToColor(), 1f);
                        goto setlethal;

                    case (FactionType)44:
                        array =
                        [
                            new(Fancy.Colors["COMPLIANCE"].start.ToColor(), 0f),
                            new(Fancy.Colors["COMPLIANCE"].middle.ToColor(), 0.5f),
                            new(Fancy.Colors["COMPLIANCE"].end.ToColor(), 1f)
                        ];
                        goto setlethal;

                    default:
                        array[0] = new(Fancy.Colors["STONED_HIDDEN"].start.ToColor(), 0f);
                        array[1] = new(Fancy.Colors["STONED_HIDDEN"].start.ToColor(), 1f);
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

                    default:
                        array[0] = new(Fancy.Colors["STONED_HIDDEN"].start.ToColor(), 0f);
                        array[1] = new(Fancy.Colors["STONED_HIDDEN"].start.ToColor(), 1f);
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
