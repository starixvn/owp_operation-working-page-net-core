namespace OWP.EventPicking.Api.Services;
using OWP.EventPicking.Api.Models;

public interface MappingRuleSyncMBLeagueFriendlyService
{
    public void ProcessLeagueSettingsForClubFriendlyMatch(
        LeagueSettingInfoSender selectedHomeLeagueSetting, 
        LeagueSettingInfoSender selectedAwayLeagueSetting, 
        EventPickingModel match);
}

public class MappingRuleSyncMBLeagueFriendlyServiceImp : MappingRuleSyncMBLeagueFriendlyService
{

    public void ProcessLeagueSettingsForClubFriendlyMatch(
        LeagueSettingInfoSender selectedHomeLeagueSetting, 
        LeagueSettingInfoSender selectedAwayLeagueSetting, 
        EventPickingModel match)
    {
        var homeMaxbet = GetMaxbet(selectedHomeLeagueSetting, match);
        var homeSpreadHdp = selectedHomeLeagueSetting.Spread_HDP_Value;
        var homeSpreadOu = selectedHomeLeagueSetting.Spread_OU_Value;

        var awayMaxbet = GetMaxbet(selectedAwayLeagueSetting, match);
        var awaySpreadHdp = selectedAwayLeagueSetting.Spread_HDP_Value;
        var awaySpreadOu = selectedAwayLeagueSetting.Spread_OU_Value;

        var (finalMaxbet, finalSpreadHdp, finalSpreadOu) = CalculateFinalMBAndSpread(homeMaxbet!.Value, awayMaxbet!.Value, homeSpreadHdp, awaySpreadHdp, homeSpreadOu, awaySpreadOu);

        if (finalMaxbet > 0)
        {
            match.MaxbetHdp = finalMaxbet;
            match.MaxbetOu = finalMaxbet;
            match.SpreadHdp = finalSpreadHdp;
            match.SpreadOu = finalSpreadOu;
            match.IsReferenceLeagueManagement = true;
        }
    }
    public (decimal finalMaxbet, decimal? finalSpreadHdp, decimal? finalSpreadOu) CalculateFinalMBAndSpread(
        decimal homeMaxbet, decimal awayMaxbet, decimal? homeSpreadHdp, decimal? awaySpreadHdp, decimal? homeSpreadOu, decimal? awaySpreadOu)
    {
        // Rule 1: Home & Away MB >= 25K
        if (homeMaxbet >= 25000 && awayMaxbet >= 25000)
        {
            return (15000, 0.10m, 0.12m);
        }

        // Rule 2: Home MB = Away MB = xK, where x ∈ [13K-24K]
        if (homeMaxbet == awayMaxbet && homeMaxbet >= 13000 && homeMaxbet <= 24000)
        {
            return (12000, 0.10m, 0.12m);
        }

        // Rule 3 & 4: Home MB = Away MB = 12K AND Home & Away Spread <= 10%-12%
        if (homeMaxbet == awayMaxbet && homeMaxbet == 12000)
        {
            var spreadValues = new[] { homeSpreadHdp, homeSpreadOu, awaySpreadHdp, awaySpreadOu };
            var nonNullSpreads = spreadValues.Where(s => s.HasValue).Select(s => s.Value).ToList();
            var averageSpread = nonNullSpreads.Count > 0 ? nonNullSpreads.Average() : 0;

            if (averageSpread <= 0.12m)
            {
                return (12000, 0.10m, 0.12m);
            }
            else
            {
                return (6000, 0.16m, 0.18m);
            }
        }

        // Rule 5: Home MB = Away MB = yK, where y ∈ [6K-11K]
        if (homeMaxbet == awayMaxbet && homeMaxbet >= 6000 && homeMaxbet <= 11000)
        {
            return (6000, 0.16m, 0.18m);
        }

        // Rule 6: Home & Away MB <= 5K AND MB Home ≠ MB Away
        if (homeMaxbet <= 5000 && awayMaxbet <= 5000 && homeMaxbet != awayMaxbet)
        {
            var higherMaxbet = Math.Max(homeMaxbet, awayMaxbet);
            var selectedSpreadHdp = homeSpreadHdp;
            var selectedSpreadOu = homeSpreadOu;
            if (higherMaxbet == awayMaxbet)
            {
                selectedSpreadHdp = awaySpreadHdp;
                selectedSpreadOu = awaySpreadOu;
            }

            if (((selectedSpreadHdp ?? 0) + (selectedSpreadOu ?? 0)) / 2 < 0.17m)
            {
                selectedSpreadHdp = 0.16m;
                selectedSpreadOu = 0.18m;
            }

            return (higherMaxbet, selectedSpreadHdp, selectedSpreadOu);
        }

        // Rule 7: Home MB = Away MB = zK, where z ∈ [2K-5K]
        if (homeMaxbet == awayMaxbet && homeMaxbet >= 2000 && homeMaxbet <= 5000)
        {
            var (lowerSpreadHdp, lowerSpreadOu) = CompareAverageSpread(homeSpreadHdp, homeSpreadOu, awaySpreadHdp, awaySpreadOu, useHigher: false);

            return (homeMaxbet, lowerSpreadHdp, lowerSpreadOu);
        }

        // Rule 8 & 9: When MB Home ≠ MB Away. Use the higher MB to evaluate.
        if (homeMaxbet != awayMaxbet)
        {
            var higherMaxbet = Math.Max(homeMaxbet, awayMaxbet);

            // Rule 8: Higher MB >= 12K
            if (higherMaxbet >= 12000)
            {
                return (12000, 0.10m, 0.12m);
            }

            // Rule 9: Higher MB < 12K
            else
            {
                return (6000, 0.16m, 0.18m);
            }
        }

        return (0, 0, 0);
    }

    

    private decimal? GetMaxbet(LeagueSettingInfoSender leagueSetting, EventPickingModel match)
    {
        if (IsWeekend(match.EventDateTime))
        {
            return leagueSetting.MaxbetWK_HDP_Value;
        }
        else
        {
            return leagueSetting.MaxbetWD_HDP_Value;
        }
    }

    private static bool IsWeekend(DateTime eventDateTime)
    {
        var eventDate = DateTime.SpecifyKind(eventDateTime, DateTimeKind.Utc).AddHours(-12);
        var isWeekend = eventDate.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
        return isWeekend;
    }

    private static (decimal? spreadHdp, decimal? spreadOu) CompareAverageSpread(decimal? homeSpreadHdp, decimal? homeSpreadOu, decimal? awaySpreadHdp, decimal? awaySpreadOu, bool useHigher = true)
    {
        var homeValues = new[] { homeSpreadHdp, homeSpreadOu }.Where(v => v.HasValue).Select(v => v.Value).ToList();
        var awayValues = new[] { awaySpreadHdp, awaySpreadOu }.Where(v => v.HasValue).Select(v => v.Value).ToList();

        var homeAverageSpread = homeValues.Count > 0 ? homeValues.Average() : (decimal?)null;
        var awayAverageSpread = awayValues.Count > 0 ? awayValues.Average() : (decimal?)null;

        if (homeAverageSpread == null)
        {
            return (awaySpreadHdp, awaySpreadOu);
        }
        if (awayAverageSpread == null)
        {
            return (homeSpreadHdp, homeSpreadOu);
        }

        bool shouldUseHome = useHigher ?
            homeAverageSpread > awayAverageSpread :
            homeAverageSpread < awayAverageSpread;

        if (shouldUseHome)
        {
            return (homeSpreadHdp, homeSpreadOu);
        }
        else
        {
            return (awaySpreadHdp, awaySpreadOu);
        }
    }
}
