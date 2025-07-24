using System.Threading.Tasks;
using NPOI.SS.Formula.Functions;
using OWP.EventPicking.Api.Middleware.Models;
using OWP.EventPicking.Api.Models;
using OWP.EventPicking.Api.Services;

namespace OWP.EventPicking.Api.Handlers;

public class EditEventPickingRequest : EventPickingRequest, IRequest<EventPickingResult>;

public class EditEventPickingHandler(
    QueryEventPickingService service,
    Repository repository,
    PullMainLeagueService pullMainLeagueService,
    MapMainLeagueForEventService mainLeagueForEventMapper,
    BuildMBForClubFriendlyMajorLeagueBuilder buildMBForClubFriendlyMajorLeagueBuilder) : IRequestHandler<EditEventPickingRequest, EventPickingResult>
{

    private readonly IList<string> FiveMajorLeague = new List<string>()
    {
        "*ENGLISH PREMIER LEAGUE",
        "*GERMANY-BUNDESLIGA I",
        "*ITALY SERIE A",
        "*SPAIN PRIMERA LALIGA",
        "FRANCE LIGUE 1"
    };

    public async Task<EventPickingResult> Handle(EditEventPickingRequest request, CancellationToken cancellationToken)
    {
        var mainMatches = await service.Execute(request, new List<short>());

        var matchFiltered = mainMatches.FilteredMatches;

        var leagueSetting = (await repository.QueryLeagueSetting()).ToList();
        

        var clubFriendlyMatches = matchFiltered.Where(x => x.IsClubFriendlyLeague).ToList();
        var mainLeague = new List<main_league_info>();
        //var mainLeague = await pullMainLeagueService.GetMainLeague(clubFriendlyMatches, cancellationToken);
        //var mainLeague = new List<main_league_info>
        //{
        //    new() { TeamName = "FK Arsenal Dzyarzhynsk", MainLeagueName = "BELARUS PREMIER LEAGUE" },
        //    new() { TeamName = "Widzew Lodz (N)", MainLeagueName = "POLAND EKSTRAKLASA" },
        //    new() { TeamName = "FK Spartak Subotica (N)", MainLeagueName = "SERBIA SUPER LEAGUE" },
        //    new() { TeamName = "FC Magdeburg", MainLeagueName = "GERMANY BUNDESLIGA 2" },
        //    new() { TeamName = "Slovan Duslo Sala", MainLeagueName = "SLOVAKIA LIGA 3" },
        //    new() { TeamName = "FC Neftekhimik Nizhnekamsk", MainLeagueName = "RUSSIA FIRST LEAGUE" },
        //    new() { TeamName = "Piast Gliwice", MainLeagueName = "POLAND EKSTRAKLASA" },
        //    new() { TeamName = "FK Novi Pazar", MainLeagueName = "SERBIA SUPER LEAGUE" },
        //    new() { TeamName = "Zurich", MainLeagueName = "SWITZERLAND SUPER LEAGUE" },
        //    new() { TeamName = "ViOn Zlate Moravce", MainLeagueName = "SLOVAKIA 2ND DIVISION" },
        //    new() { TeamName = "Dungannon Swifts", MainLeagueName = "NORTHERN IRELAND PREMIERSHIP" },
        //    new() { TeamName = "Wellington Phoenix FC", MainLeagueName = "AUSTRALIA A-LEAGUE" },
        //    new() { TeamName = "Kickers Offenbach", MainLeagueName = "GERMANY REGIONAL LEAGUE SOUTHWEST" },
        //    new() { TeamName = "Loughgall", MainLeagueName = "NORTHERN IRELAND PREMIERSHIP DEVELOPMENT LEAGUE" },
        //    new() { TeamName = "Wrexham", MainLeagueName = "ENGLISH LEAGUE CHAMPIONSHIP" },
        //    new() { TeamName = "SV Wehen Wiesbaden", MainLeagueName = "GERMANY 3RD LIGA" },
        //    new() { TeamName = "SV Donau Klagenfurt", MainLeagueName = "RUSSIA FIRST LEAGUE" },
        //    new() { TeamName = "Basel", MainLeagueName = "POLAND EKSTRAKLASA" },
        //    new() { TeamName = "VfB Eichstatt", MainLeagueName = "SERBIA SUPER LEAGUE" },
        //    new() { TeamName = "Besiktas", MainLeagueName = "SWITZERLAND SUPER LEAGUE" },
        //    new() { TeamName = "RFC Liege", MainLeagueName = "SLOVAKIA 2ND DIVISION" },
        //    new() { TeamName = "CD Platense Municipal Zacatecoluca", MainLeagueName = "*ENGLISH PREMIER LEAGUE" },
        //    new() { TeamName = "SC Launsdorf", MainLeagueName = "*ENGLISH PREMIER LEAGUE" },
        //    new() { TeamName = "FC Rapperswil-Jona", MainLeagueName = "*GERMANY-BUNDESLIGA I" },
        //    new() { TeamName = "FC Pipinsried", MainLeagueName = "*ITALY SERIE A" },
        //    new() { TeamName = "SKN St. Polten", MainLeagueName = "*SPAIN PRIMERA LALIGA" },
        //    new() { TeamName = "Royal Aywaille FC", MainLeagueName = "FRANCE LIGUE 1" },
        //    new() { TeamName = "Ballyclare Comrades", MainLeagueName = "*ITALY SERIE A" },
        //    new() { TeamName = "Crusaders FC", MainLeagueName = "*SPAIN PRIMERA LALIGA" },
        //    new() { TeamName = "Zorya Lugansk (N)", MainLeagueName = "*ITALY SERIE A" },
        //    new() { TeamName = "FK Sarajevo", MainLeagueName = "*GERMANY-BUNDESLIGA I" },
        //    new() { TeamName = "NK Bravo (N)", MainLeagueName = "*GERMANY-BUNDESLIGA I" },
        //    new() { TeamName = "HNK Rijeka", MainLeagueName = "*GERMANY-BUNDESLIGA I" },
        //    new() { TeamName = "Orlando Pirates", MainLeagueName = "*GERMANY-BUNDESLIGA I" },
        //    new() { TeamName = "Pafos FC", MainLeagueName = "*ITALY SERIE A" },
        //    new() { TeamName = "Hapoel Rishon LeZion", MainLeagueName = "*ENGLISH PREMIER LEAGUE" },
        //    new() { TeamName = "FK Graficar Beograd (N)", MainLeagueName = "*ENGLISH PREMIER LEAGUE" },
        //    new() { TeamName = "Auckland FC", MainLeagueName = "*GERMANY-BUNDESLIGA I" },
        //    new() { TeamName = "Adelaide Blue Eagles", MainLeagueName = "*ITALY SERIE A" },
        //    new() { TeamName = "Bruk-Bet Termalica Nieciecza (N)", MainLeagueName = "*SPAIN PRIMERA LALIGA" },
        //    new() { TeamName = "Maccabi Herzliya", MainLeagueName = "RUSSIA FIRST LEAGUE" },
        //    new() { TeamName = "OFK Petrovac", MainLeagueName = "*GERMANY-BUNDESLIGA I" },
        //    new() { TeamName = "West Coast Rangers", MainLeagueName = "*ITALY SERIE A" },
        //    new() { TeamName = "Adelaide United", MainLeagueName = "*SPAIN PRIMERA LALIGA" },
        //    new() { TeamName = "Neftchi Baku", MainLeagueName = "FRANCE LIGUE 1" },


        //    new() { TeamName = "Arsenal", MainLeagueName = "*ENGLISH PREMIER LEAGUE" },
        //    new() { TeamName = "Newcastle United", MainLeagueName = "*ENGLISH PREMIER LEAGUE" },
        //    new() { TeamName = "Clitheroe FC", MainLeagueName = "*ENGLISH PREMIER LEAGUE" },
        //    new() { TeamName = "Accrington Stanley", MainLeagueName = "*ENGLISH PREMIER LEAGUE" },
        //    new() { TeamName = "Asteras Tripolis", MainLeagueName = "*ENGLISH PREMIER LEAGUE" },
        //    new() { TeamName = "Neftchi Baku", MainLeagueName = "*ENGLISH PREMIER LEAGUE" },
        //    new() { TeamName = "FC 08 Homburg", MainLeagueName = "*ENGLISH PREMIER LEAGUE" },
        //    new() { TeamName = "SV Elversberg", MainLeagueName = "*ENGLISH PREMIER LEAGUE" },
        //    new() { TeamName = "Montedio Yamagata", MainLeagueName = "JAPAN J-LEAGUE DIVISION 2" },
        //    new() { TeamName = "Reims", MainLeagueName = "FRANCE LIGUE 2" },
        //    new() { TeamName = "Vissel Kobe", MainLeagueName = "JAPAN J-LEAGUE DIVISION 1" },
        //    new() { TeamName = "Middelfart Boldklub", MainLeagueName = "JAPAN J-LEAGUE DIVISION 2" },
        //    new() { TeamName = "Naesby BK", MainLeagueName = "FRANCE LIGUE 2" }
        //};
        mainLeagueForEventMapper.Execute(matchFiltered, mainLeague);
        foreach (var match in matchFiltered)
        {
            if (match.IsClubFriendlyLeague)
            {
                var tvList = (await repository.GetTVList()).ToList();
                ProcessLeagueSettingsForClubFriendlyMatch(leagueSetting, match, tvList);
            }
            else
            {
                var leagueSettingByLeagueId = leagueSetting.Where(x => x.OWPLeagueId == match.OWPLeagueId).ToList();
                ProcessLeagueSettingsForMatch(leagueSettingByLeagueId, match);
            }


        }

        return new EventPickingResult
        {
            MatchesByDate = mainMatches.MatchesByDate,
            FilteredMatches = matchFiltered
        };
    }

    private void ProcessLeagueSettingsForClubFriendlyMatch(List<LeagueSettingInfoSender> leagueSetting, EventPickingModel match, List<Condition> tvList)
    {
        if (match.IsSync || match.ChildMatchId is not null) return;
        if (FiveMajorLeague.Contains(match.HomeMainLeague) && FiveMajorLeague.Contains(match.AwayMainLeague))
        {
            buildMBForClubFriendlyMajorLeagueBuilder.Build(match, tvList);
        }
        else
        {
            ProcessLeagueSettingsForClubFriendlyNotMajorLeagueMatch(leagueSetting, match);
        }
    }

    private void ProcessLeagueSettingsForClubFriendlyNotMajorLeagueMatch(List<LeagueSettingInfoSender> leagueSetting, EventPickingModel match)
    {
        var homeLeagueSettingByLeagueId = leagueSetting.Where(x => x.League.Trim() == match.HomeMainLeague && GetMaxbet(x, match) != null).ToList();
        var awayLeagueSettingByLeagueId = leagueSetting.Where(x => x.League.Trim() == match.AwayMainLeague && GetMaxbet(x, match) != null).ToList();
        LeagueSettingInfoSender selectedHomeLeagueSetting = null;
        LeagueSettingInfoSender selectedAwayLeagueSetting = null;
        if (homeLeagueSettingByLeagueId.Count > 0)
        {
            selectedHomeLeagueSetting = ProcessLeagueSettingsForMatch(homeLeagueSettingByLeagueId, match);
        }
        if (awayLeagueSettingByLeagueId.Count > 0)
        {
            selectedAwayLeagueSetting = ProcessLeagueSettingsForMatch(awayLeagueSettingByLeagueId, match);
        }

        if (selectedHomeLeagueSetting != null && selectedAwayLeagueSetting != null)
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
                match.SpreadHdp = finalSpreadHdp;
                match.SpreadOu = finalSpreadOu;
                match.IsReferenceLeagueManagement = true;
                CalculateNoHDPClubFriendLy(match);
            }
        }

    }

    private void CalculateNoHDPClubFriendLy(EventPickingModel match)
    {
        var maxbetHdp = match.MaxbetHdp;

        var noOfHDP = maxbetHdp switch
        {
            >= 2000 and <= 4000 => "2 lines",
            >= 5000 and <= 8000 => string.Empty,
            >= 10000 and <= 60000 => IsPeakHour(match.EventDateTime) ? string.Empty : "3+3 Lines",
            _ => match.NoOfHDP
        };

        match.NoOfHDP = noOfHDP;
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
    private LeagueSettingInfoSender ProcessLeagueSettingsForMatch(List<LeagueSettingInfoSender> leagueSettingByLeagueId, EventPickingModel match)
    {
        switch (leagueSettingByLeagueId.Count)
        {
            case 0:
                match.IsShowCheckButton = false;
                return null;
            case 1:
                SuggestionValue(match, leagueSettingByLeagueId.First());
                return leagueSettingByLeagueId.First();
            default:
                return HandleMappingForMultipleSettings(match, leagueSettingByLeagueId);
        }
    }

    private LeagueSettingInfoSender HandleMappingForMultipleSettings(EventPickingModel match, IList<LeagueSettingInfoSender> leagueSettingByLeagueId)
    {
        var leagueIsMappingWithCondition = leagueSettingByLeagueId
            .Where(league => CheckMatchValidMappingWithLeagueManagement(match, league)).ToList();

        if (leagueIsMappingWithCondition.Count == 1)
        {
            SuggestionValue(match, leagueIsMappingWithCondition.First());
            return leagueIsMappingWithCondition.First();
        }
        return leagueIsMappingWithCondition.FirstOrDefault();

    }


    private void SuggestionValue(EventPickingModel match, LeagueSettingInfoSender league)
    {
        if (match.IsSync || match.ChildMatchId is not null) return;
        BuildRemark2(match, league);
        BuildLstNote(match, league);
        BuildMaxBet(match, league);
        BuildSpreadHdp(match, league);
        BuildSpreadOu(match, league);
        BuildNoOfHdp(match, league);
    }
    private void BuildRemark2(EventPickingModel match, LeagueSettingInfoSender league)
    {
        match.Remark2LeagueManagement = league.Remark2;
        match.IsReferenceLeagueManagement = true;
    }

    private void BuildLstNote(EventPickingModel match, LeagueSettingInfoSender league)
    {
        match.LstNote = league.LstNote;
        match.IsReferenceLeagueManagement = true;
    }

    private void BuildMaxBet(EventPickingModel match, LeagueSettingInfoSender league)
    {
        match.MaxbetHdp = IsWeekend(match.EventDateTime) ? league.MaxbetWK_HDP_Value : league.MaxbetWD_HDP_Value;
        match.IsReferenceLeagueManagement = true;
    }

    private void BuildSpreadHdp(EventPickingModel match, LeagueSettingInfoSender league)
    {
        match.SpreadHdp = league.Spread_HDP_Value;
        match.IsReferenceLeagueManagement = true;
    }
    private void BuildSpreadOu(EventPickingModel match, LeagueSettingInfoSender league)
    {
        match.SpreadOu = league.Spread_OU_Value;
        match.IsReferenceLeagueManagement = true;
    }
    private void BuildNoOfHdp(EventPickingModel match, LeagueSettingInfoSender league)
    {
        match.NoOfHDP = IsWeekend(match.EventDateTime)
            ? league.WK_NoOfHDP_Value
            : league.WD_NoOfHDP_Value;
        match.IsReferenceLeagueManagement = true;
    }

    private bool CheckRoundOrDivisionConditions(string matchValues, IList<Condition> conditions)
    {
        var containsValues = GetValueContains(conditions);
        var equalsValues = GetValueEquals(conditions);

        var isConditionsEmpty = conditions.Count == 0;
        var isContains = containsValues.Any(x => matchValues.Contains(x, StringComparison.CurrentCultureIgnoreCase)) && !string.IsNullOrEmpty(matchValues);
        var isEquals = equalsValues.Any(x => x.Equals(matchValues, StringComparison.CurrentCultureIgnoreCase));
        return isConditionsEmpty || isContains || isEquals;
    }

    private bool CheckMatchValidMappingWithLeagueManagement(EventPickingModel match, LeagueSettingInfoSender leagueSetting)
    {
        var roundConditions = CheckRoundOrDivisionConditions(match.Round, leagueSetting.Condition.Round);
        var divisionConditions = CheckRoundOrDivisionConditions(match.Division, leagueSetting.Condition.Division);

        return roundConditions && divisionConditions;
    }

    private static IList<string> GetValueContains(IList<Condition> conditions)
        => conditions.Where(x => x.Type == "Contains").Select(x => x.Value).ToList();

    private static IList<string> GetValueEquals(IList<Condition> conditions)
        => conditions.Where(x => x.Type == "Equals").Select(x => x.Value).ToList();

    private static bool IsWeekend(DateTime eventDateTime)
    {
        var eventDate = DateTime.SpecifyKind(eventDateTime, DateTimeKind.Utc).AddHours(-12);
        var isWeekend = eventDate.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
        return isWeekend;
    }

    private static bool IsPeakHour(DateTime eventDateTime)
    {
        var time = eventDateTime.TimeOfDay;
        return IsWeekend(eventDateTime)
            ? time >= TimeSpan.FromHours(20) || time <= TimeSpan.FromHours(4.5)
            : time >= TimeSpan.FromHours(2) && time <= TimeSpan.FromHours(4.5);
    }

    private static (decimal finalMaxbet, decimal? finalSpreadHdp, decimal? finalSpreadOu) CalculateFinalMBAndSpread(
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

            // Rule 8: Higher MB > 12K
            if (higherMaxbet > 12000)
            {
                return (12000, 0.10m, 0.12m);
            }

            // Rule 9: Higher MB <= 12K
            else
            {
                return (6000, 0.16m, 0.18m);
            }
        }

        return (0, 0, 0);
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