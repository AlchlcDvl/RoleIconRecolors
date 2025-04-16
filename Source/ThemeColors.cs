namespace FancyUI;


public static class ThemeColors
{
public static Color GetWoodColor()
{
    try
    {
        return Pepper.GetMyFaction() switch
        {
            FactionType.TOWN => Fancy.TownUIThemeWood.Value.ToColor(),
            FactionType.COVEN => Fancy.CovenUIThemeWood.Value.ToColor(),
            FactionType.SERIALKILLER => Fancy.SerialKillerUIThemeWood.Value.ToColor(),
            FactionType.ARSONIST => Fancy.ArsonistUIThemeWood.Value.ToColor(),
            FactionType.WEREWOLF => Fancy.WerewolfUIThemeWood.Value.ToColor(),
            FactionType.SHROUD => Fancy.ShroudUIThemeWood.Value.ToColor(),
            FactionType.APOCALYPSE => Fancy.ApocalypseUIThemeWood.Value.ToColor(),
            FactionType.EXECUTIONER => Fancy.ExecutionerUIThemeWood.Value.ToColor(),
            FactionType.JESTER => Fancy.JesterUIThemeWood.Value.ToColor(),
            FactionType.PIRATE => Fancy.PirateUIThemeWood.Value.ToColor(),
            FactionType.DOOMSAYER => Fancy.DoomsayerUIThemeWood.Value.ToColor(),
            FactionType.VAMPIRE => Fancy.VampireUIThemeWood.Value.ToColor(),
            FactionType.CURSED_SOUL => Fancy.CursedSoulUIThemeWood.Value.ToColor(),

            (FactionType)33 => Fancy.JackalUIThemeWood.Value.ToColor(),   // Jackal
            (FactionType)34 => Fancy.FrogsUIThemeWood.Value.ToColor(),    // Frogs
            (FactionType)35 => Fancy.LionsUIThemeWood.Value.ToColor(),    // Lions
            (FactionType)36 => Fancy.HawksUIThemeWood.Value.ToColor(),    // Hawks

            (FactionType)38 => Fancy.JudgeUIThemeWood.Value.ToColor(),    // Judge
            (FactionType)39 => Fancy.AuditorUIThemeWood.Value.ToColor(),  // Auditor
            (FactionType)40 => Fancy.InquisitorUIThemeWood.Value.ToColor(), // Inquisitor
            (FactionType)41 => Fancy.StarspawnUIThemeWood.Value.ToColor(),  // Starspawn
            (FactionType)42 => Fancy.EgotistUIThemeWood.Value.ToColor(),    // Egotist
            (FactionType)43 => Fancy.PandoraUIThemeWood.Value.ToColor(),    // Pandora
            (FactionType)44 => Fancy.ComplianceUIThemeWood.Value.ToColor(), // Compliance

            _ => Fancy.MainUIThemeWood.Value.ToColor()
        };
    }
    catch 
    {
        return Fancy.MainUIThemeWood.Value.ToColor(); // default fallback
    }
}

public static Color GetPaperColor()
{
    try
    {
        return Pepper.GetMyFaction() switch
        {
            FactionType.TOWN => Fancy.TownUIThemePaper.Value.ToColor(),
            FactionType.COVEN => Fancy.CovenUIThemePaper.Value.ToColor(),
            FactionType.SERIALKILLER => Fancy.SerialKillerUIThemePaper.Value.ToColor(),
            FactionType.ARSONIST => Fancy.ArsonistUIThemePaper.Value.ToColor(),
            FactionType.WEREWOLF => Fancy.WerewolfUIThemePaper.Value.ToColor(),
            FactionType.SHROUD => Fancy.ShroudUIThemePaper.Value.ToColor(),
            FactionType.APOCALYPSE => Fancy.ApocalypseUIThemePaper.Value.ToColor(),
            FactionType.EXECUTIONER => Fancy.ExecutionerUIThemePaper.Value.ToColor(),
            FactionType.JESTER => Fancy.JesterUIThemePaper.Value.ToColor(),
            FactionType.PIRATE => Fancy.PirateUIThemePaper.Value.ToColor(),
            FactionType.DOOMSAYER => Fancy.DoomsayerUIThemePaper.Value.ToColor(),
            FactionType.VAMPIRE => Fancy.VampireUIThemePaper.Value.ToColor(),
            FactionType.CURSED_SOUL => Fancy.CursedSoulUIThemePaper.Value.ToColor(),

            (FactionType)33 => Fancy.JackalUIThemePaper.Value.ToColor(),   // Jackal
            (FactionType)34 => Fancy.FrogsUIThemePaper.Value.ToColor(),    // Frogs
            (FactionType)35 => Fancy.LionsUIThemePaper.Value.ToColor(),    // Lions
            (FactionType)36 => Fancy.HawksUIThemePaper.Value.ToColor(),    // Hawks

            (FactionType)38 => Fancy.JudgeUIThemePaper.Value.ToColor(),    // Judge
            (FactionType)39 => Fancy.AuditorUIThemePaper.Value.ToColor(),  // Auditor
            (FactionType)40 => Fancy.InquisitorUIThemePaper.Value.ToColor(), // Inquisitor
            (FactionType)41 => Fancy.StarspawnUIThemePaper.Value.ToColor(),  // Starspawn
            (FactionType)42 => Fancy.EgotistUIThemePaper.Value.ToColor(),    // Egotist
            (FactionType)43 => Fancy.PandoraUIThemePaper.Value.ToColor(),    // Pandora
            (FactionType)44 => Fancy.ComplianceUIThemePaper.Value.ToColor(), // Compliance

            _ => Fancy.MainUIThemePaper.Value.ToColor()
        };
    }
    catch
    {
        return Fancy.MainUIThemePaper.Value.ToColor(); 
    }
}

public static Color GetLeatherColor()
{
    try
    {
        return Pepper.GetMyFaction() switch
        {
            FactionType.TOWN => Fancy.TownUIThemeLeather.Value.ToColor(),
            FactionType.COVEN => Fancy.CovenUIThemeLeather.Value.ToColor(),
            FactionType.SERIALKILLER => Fancy.SerialKillerUIThemeLeather.Value.ToColor(),
            FactionType.ARSONIST => Fancy.ArsonistUIThemeLeather.Value.ToColor(),
            FactionType.WEREWOLF => Fancy.WerewolfUIThemeLeather.Value.ToColor(),
            FactionType.SHROUD => Fancy.ShroudUIThemeLeather.Value.ToColor(),
            FactionType.APOCALYPSE => Fancy.ApocalypseUIThemeLeather.Value.ToColor(),
            FactionType.EXECUTIONER => Fancy.ExecutionerUIThemeLeather.Value.ToColor(),
            FactionType.JESTER => Fancy.JesterUIThemeLeather.Value.ToColor(),
            FactionType.PIRATE => Fancy.PirateUIThemeLeather.Value.ToColor(),
            FactionType.DOOMSAYER => Fancy.DoomsayerUIThemeLeather.Value.ToColor(),
            FactionType.VAMPIRE => Fancy.VampireUIThemeLeather.Value.ToColor(),
            FactionType.CURSED_SOUL => Fancy.CursedSoulUIThemeLeather.Value.ToColor(),

            (FactionType)33 => Fancy.JackalUIThemeLeather.Value.ToColor(),
            (FactionType)34 => Fancy.FrogsUIThemeLeather.Value.ToColor(),
            (FactionType)35 => Fancy.LionsUIThemeLeather.Value.ToColor(),
            (FactionType)36 => Fancy.HawksUIThemeLeather.Value.ToColor(),

            (FactionType)38 => Fancy.JudgeUIThemeLeather.Value.ToColor(),
            (FactionType)39 => Fancy.AuditorUIThemeLeather.Value.ToColor(),
            (FactionType)40 => Fancy.InquisitorUIThemeLeather.Value.ToColor(),
            (FactionType)41 => Fancy.StarspawnUIThemeLeather.Value.ToColor(),
            (FactionType)42 => Fancy.EgotistUIThemeLeather.Value.ToColor(),
            (FactionType)43 => Fancy.PandoraUIThemeLeather.Value.ToColor(),
            (FactionType)44 => Fancy.ComplianceUIThemeLeather.Value.ToColor(),

            _ => Fancy.MainUIThemeLeather.Value.ToColor()
        };
    }
    catch
    {
        return Fancy.MainUIThemeLeather.Value.ToColor(); 
    }
}

public static Color GetMetalColor()
{
    try
    {
        return Pepper.GetMyFaction() switch
        {
            FactionType.TOWN => Fancy.TownUIThemeMetal.Value.ToColor(),
            FactionType.COVEN => Fancy.CovenUIThemeMetal.Value.ToColor(),
            FactionType.SERIALKILLER => Fancy.SerialKillerUIThemeMetal.Value.ToColor(),
            FactionType.ARSONIST => Fancy.ArsonistUIThemeMetal.Value.ToColor(),
            FactionType.WEREWOLF => Fancy.WerewolfUIThemeMetal.Value.ToColor(),
            FactionType.SHROUD => Fancy.ShroudUIThemeMetal.Value.ToColor(),
            FactionType.APOCALYPSE => Fancy.ApocalypseUIThemeMetal.Value.ToColor(),
            FactionType.EXECUTIONER => Fancy.ExecutionerUIThemeMetal.Value.ToColor(),
            FactionType.JESTER => Fancy.JesterUIThemeMetal.Value.ToColor(),
            FactionType.PIRATE => Fancy.PirateUIThemeMetal.Value.ToColor(),
            FactionType.DOOMSAYER => Fancy.DoomsayerUIThemeMetal.Value.ToColor(),
            FactionType.VAMPIRE => Fancy.VampireUIThemeMetal.Value.ToColor(),
            FactionType.CURSED_SOUL => Fancy.CursedSoulUIThemeMetal.Value.ToColor(),

            (FactionType)33 => Fancy.JackalUIThemeMetal.Value.ToColor(),
            (FactionType)34 => Fancy.FrogsUIThemeMetal.Value.ToColor(),
            (FactionType)35 => Fancy.LionsUIThemeMetal.Value.ToColor(),
            (FactionType)36 => Fancy.HawksUIThemeMetal.Value.ToColor(),

            (FactionType)38 => Fancy.JudgeUIThemeMetal.Value.ToColor(),
            (FactionType)39 => Fancy.AuditorUIThemeMetal.Value.ToColor(),
            (FactionType)40 => Fancy.InquisitorUIThemeMetal.Value.ToColor(),
            (FactionType)41 => Fancy.StarspawnUIThemeMetal.Value.ToColor(),
            (FactionType)42 => Fancy.EgotistUIThemeMetal.Value.ToColor(),
            (FactionType)43 => Fancy.PandoraUIThemeMetal.Value.ToColor(),
            (FactionType)44 => Fancy.ComplianceUIThemeMetal.Value.ToColor(),

            _ => Fancy.MainUIThemeMetal.Value.ToColor()
        };
    }
    catch
    {
        return Fancy.MainUIThemeMetal.Value.ToColor(); 
    }
}

public static Color GetFireColor()
{
    try
    {
        return Pepper.GetMyFaction() switch
        {
            FactionType.TOWN => Fancy.TownUIThemeFire.Value.ToColor(),
            FactionType.COVEN => Fancy.CovenUIThemeFire.Value.ToColor(),
            FactionType.SERIALKILLER => Fancy.SerialKillerUIThemeFire.Value.ToColor(),
            FactionType.ARSONIST => Fancy.ArsonistUIThemeFire.Value.ToColor(),
            FactionType.WEREWOLF => Fancy.WerewolfUIThemeFire.Value.ToColor(),
            FactionType.SHROUD => Fancy.ShroudUIThemeFire.Value.ToColor(),
            FactionType.APOCALYPSE => Fancy.ApocalypseUIThemeFire.Value.ToColor(),
            FactionType.EXECUTIONER => Fancy.ExecutionerUIThemeFire.Value.ToColor(),
            FactionType.JESTER => Fancy.JesterUIThemeFire.Value.ToColor(),
            FactionType.PIRATE => Fancy.PirateUIThemeFire.Value.ToColor(),
            FactionType.DOOMSAYER => Fancy.DoomsayerUIThemeFire.Value.ToColor(),
            FactionType.VAMPIRE => Fancy.VampireUIThemeFire.Value.ToColor(),
            FactionType.CURSED_SOUL => Fancy.CursedSoulUIThemeFire.Value.ToColor(),

            (FactionType)33 => Fancy.JackalUIThemeFire.Value.ToColor(),
            (FactionType)34 => Fancy.FrogsUIThemeFire.Value.ToColor(),
            (FactionType)35 => Fancy.LionsUIThemeFire.Value.ToColor(),
            (FactionType)36 => Fancy.HawksUIThemeFire.Value.ToColor(),

            (FactionType)38 => Fancy.JudgeUIThemeFire.Value.ToColor(),
            (FactionType)39 => Fancy.AuditorUIThemeFire.Value.ToColor(),
            (FactionType)40 => Fancy.InquisitorUIThemeFire.Value.ToColor(),
            (FactionType)41 => Fancy.StarspawnUIThemeFire.Value.ToColor(),
            (FactionType)42 => Fancy.EgotistUIThemeFire.Value.ToColor(),
            (FactionType)43 => Fancy.PandoraUIThemeFire.Value.ToColor(),
            (FactionType)44 => Fancy.ComplianceUIThemeFire.Value.ToColor(),

            _ => Fancy.MainUIThemeFire.Value.ToColor()
        };
    }
    catch
    {
        return Fancy.MainUIThemeFire.Value.ToColor(); 
    }
}

public static Color GetWaxColor()
{
    try
    {
        return Pepper.GetMyFaction() switch
        {
            FactionType.TOWN => Fancy.TownUIThemeWax.Value.ToColor(),
            FactionType.COVEN => Fancy.CovenUIThemeWax.Value.ToColor(),
            FactionType.SERIALKILLER => Fancy.SerialKillerUIThemeWax.Value.ToColor(),
            FactionType.ARSONIST => Fancy.ArsonistUIThemeWax.Value.ToColor(),
            FactionType.WEREWOLF => Fancy.WerewolfUIThemeWax.Value.ToColor(),
            FactionType.SHROUD => Fancy.ShroudUIThemeWax.Value.ToColor(),
            FactionType.APOCALYPSE => Fancy.ApocalypseUIThemeWax.Value.ToColor(),
            FactionType.EXECUTIONER => Fancy.ExecutionerUIThemeWax.Value.ToColor(),
            FactionType.JESTER => Fancy.JesterUIThemeWax.Value.ToColor(),
            FactionType.PIRATE => Fancy.PirateUIThemeWax.Value.ToColor(),
            FactionType.DOOMSAYER => Fancy.DoomsayerUIThemeWax.Value.ToColor(),
            FactionType.VAMPIRE => Fancy.VampireUIThemeWax.Value.ToColor(),
            FactionType.CURSED_SOUL => Fancy.CursedSoulUIThemeWax.Value.ToColor(),

            (FactionType)33 => Fancy.JackalUIThemeWax.Value.ToColor(),
            (FactionType)34 => Fancy.FrogsUIThemeWax.Value.ToColor(),
            (FactionType)35 => Fancy.LionsUIThemeWax.Value.ToColor(),
            (FactionType)36 => Fancy.HawksUIThemeWax.Value.ToColor(),

            (FactionType)38 => Fancy.JudgeUIThemeWax.Value.ToColor(),
            (FactionType)39 => Fancy.AuditorUIThemeWax.Value.ToColor(),
            (FactionType)40 => Fancy.InquisitorUIThemeWax.Value.ToColor(),
            (FactionType)41 => Fancy.StarspawnUIThemeWax.Value.ToColor(),
            (FactionType)42 => Fancy.EgotistUIThemeWax.Value.ToColor(),
            (FactionType)43 => Fancy.PandoraUIThemeWax.Value.ToColor(),
            (FactionType)44 => Fancy.ComplianceUIThemeWax.Value.ToColor(),

            _ => Fancy.MainUIThemeWax.Value.ToColor()
        };
    }
    catch
    {
        return Fancy.MainUIThemeWax.Value.ToColor(); 
    }
}


}
