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
    BuildMBForClubFriendlyMajorLeagueBuilder buildMBForClubFriendlyMajorLeagueBuilder,
    MappingRuleSyncMBLeagueFriendlyService mappingRuleSyncMBLeagueFriendlyService) : IRequestHandler<EditEventPickingRequest, EventPickingResult>
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
        var tvList = (await repository.GetTVList()).ToList();

        var clubFriendlyMatches = matchFiltered.Where(x => x.IsClubFriendlyLeague).ToList();
        var mainLeague = await pullMainLeagueService.GetMainLeague(clubFriendlyMatches, cancellationToken);
        mainLeagueForEventMapper.Execute(matchFiltered, mainLeague);
        foreach (var match in matchFiltered)
        {
            if (match.IsClubFriendlyLeague)
            {
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
            mappingRuleSyncMBLeagueFriendlyService.ProcessLeagueSettingsForClubFriendlyMatch(selectedHomeLeagueSetting, selectedAwayLeagueSetting, match);
        }

        CalculateNoHDPClubFriendLy(match);

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

    private static bool CheckRoundConditions(string matchValues, IList<Condition> conditions)
    {
        var isContains = CheckConditionContain(matchValues, conditions);
        var isEquals = CheckConditionEqual(matchValues, conditions);
        return !conditions.Any() || isContains || isEquals;
    }

    private static bool CheckDivisionConditions(string matchValues, IList<Condition> conditions)
    {
        var isContains = CheckConditionContain(matchValues, conditions);
        var isEquals = CheckConditionEqual(matchValues, conditions);
        var isPairWiseContains = CheckConditionPairWiseContain(matchValues, conditions);
        return !conditions.Any() || isContains || isEquals || isPairWiseContains;
    }
    private static bool CheckConditionPairWiseContain(string matchValues, IList<Condition> conditions)
    {
        var containsValues = GetValuePairWiseContains(conditions);
        var homeAwayDivision = matchValues.Split("-");
        if (homeAwayDivision.Length != 2)
        {
            return false;
        }

        var homeDivision= homeAwayDivision[0];
        var awayDivision= homeAwayDivision[1];

        return containsValues.Any(c =>
        {
            var homeAwayCondition = c.Split(",");
            if (homeAwayCondition.Length != 2)
            {
                return false;
            }
            var firstConditionValue = homeAwayCondition[0].Trim();
            var secondConditionValue = homeAwayCondition[1].Trim();

            return (homeDivision.Contains(firstConditionValue) &&
                   awayDivision.Contains(secondConditionValue)) 
                   ||
                   (homeDivision.Contains(secondConditionValue) &&
                   awayDivision.Contains(firstConditionValue));
        });
    }

    private static bool CheckConditionContain(string matchValues, IList<Condition> conditions)
    {
        var containsValues = GetValueContains(conditions);

       return containsValues.Any(x => matchValues.Contains(x, StringComparison.CurrentCultureIgnoreCase)) && !string.IsNullOrEmpty(matchValues);
    }

    private static bool CheckConditionEqual(string matchValues, IList<Condition> conditions)
    {
        var equalsValues = GetValueEquals(conditions);

        return equalsValues.Any(x => x.Equals(matchValues, StringComparison.CurrentCultureIgnoreCase));
    }

    private static bool CheckMatchValidMappingWithLeagueManagement(EventPickingModel match, LeagueSettingInfoSender leagueSetting)
    {
        var roundConditions = CheckRoundConditions(match.Round, leagueSetting.Condition.Round);
        var divisionConditions = CheckDivisionConditions(match.Division, leagueSetting.Condition.Division);
        return roundConditions && divisionConditions;
    }

    private static IList<string> GetValuePairWiseContains(IList<Condition> conditions)
        => conditions.Where(x => x.Type == "Pairwise Contains").Select(x => x.Value).ToList();


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

    private static (bool hasTv, bool hasIpTv) DetermineTvIptvStatus(EventPickingModel match, List<Condition> tvList)
    {
        var hasValidTv = !string.IsNullOrWhiteSpace(match.TV) &&
                        !match.TV.Contains("Cancel", StringComparison.OrdinalIgnoreCase) &&
                        tvList.Any(tvCondition => IsTvConditionMatch(match.TV, tvCondition));

        var hasValidIptv = !string.IsNullOrWhiteSpace(match.Iptv) &&
                          !match.Iptv.Contains("Cancel", StringComparison.OrdinalIgnoreCase) &&
                          match.Iptv.Contains("IPTV", StringComparison.OrdinalIgnoreCase);

        var hasValidChannel = !string.IsNullOrWhiteSpace(match.Channel) &&
                             !match.Channel.Contains("Cancel", StringComparison.OrdinalIgnoreCase);

        if (hasValidTv)
        {
            return (true, false);
        }
        else if (hasValidIptv || hasValidChannel)
        {
            return (false, true);
        }
        else
        {
            return (false, false);
        }
    }

    private static bool IsTvConditionMatch(string tvField, Condition tvCondition)
    {
        if (string.IsNullOrWhiteSpace(tvField) || string.IsNullOrWhiteSpace(tvCondition.Value))
            return false;

        return tvCondition.Type switch
        {
            "Contains" => tvField.Contains(tvCondition.Value, StringComparison.OrdinalIgnoreCase),
            "Equals" => tvField.Equals(tvCondition.Value, StringComparison.OrdinalIgnoreCase),
            _ => false
        };
    }
}