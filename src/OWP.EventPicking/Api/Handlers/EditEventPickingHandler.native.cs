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
    LeagueSettingForClubFriendlyBuilder leagueSettingForClubFriendly) : IRequestHandler<EditEventPickingRequest, EventPickingResult>
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
        //var mainLeague = await pullMainLeagueService.GetMainLeague(matchFiltered, cancellationToken);
        var mainLeague = new List<main_league_info>
        {
            new() { teamName = "Slaven Koprivnica", mainLeague = "WORLD CUP 2026 ASIA QUALIFIERS" },
            new() { teamName = "ETO FC Gyor", mainLeague = "WORLD CUP 2026 ASIA QUALIFIERS" },
            new() { teamName = "NK Varazdin", mainLeague = "VIETNAM V LEAGUE 2" },
            new() { teamName = "Zorya Lugansk", mainLeague = "VIETNAM V LEAGUE 1" },
            new() { teamName = "FC Oleksandria", mainLeague = "USA USSF DIVISION 2 PRO LEAGUE" },
            new() { teamName = "Slaven Koprivnica", mainLeague = "US OPEN CUP" }
        };
        mainLeagueForEventMapper.Execute(matchFiltered, mainLeague);

        foreach (var match in matchFiltered)
        {
            if (match.IsClubFriendlyLeague &&
            !(FiveMajorLeague.Contains(match.HomeMainLeague) && FiveMajorLeague.Contains(match.AwayMainLeague)))
            {
                var homeLeagueSettingByLeagueId = leagueSetting.Where(x => x.League == match.HomeMainLeague).ToList();
                var awayLeagueSettingByLeagueId = leagueSetting.Where(x => x.League == match.AwayMainLeague).ToList();
                if (homeLeagueSettingByLeagueId.Count > 0)
                {
                    var homeSetting = ProcessLeagueSettingsForMatch(homeLeagueSettingByLeagueId, match);
                }
                if (awayLeagueSettingByLeagueId.Count > 0)
                {
                    var awaySetting = ProcessLeagueSettingsForMatch(awayLeagueSettingByLeagueId, match);
                }
            }
            else
            {
                var leagueSettingByLeagueId = leagueSetting.Where(x => x.OWPLeagueId == match.OWPLeagueId).ToList();
                ProcessLeagueSettingsForMatch(leagueSettingByLeagueId, match);
            }


        }

        leagueSettingForClubFriendly.Build(matchFiltered, leagueSetting, cancellationToken);
        return new EventPickingResult
        {
            MatchesByDate = mainMatches.MatchesByDate,
            FilteredMatches = matchFiltered
        };
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
        else
        {
            return leagueSettingByLeagueId.First();
        }

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
        var eventDate = DateTime.SpecifyKind(eventDateTime, DateTimeKind.Utc).AddHours(-4);
        var isWeekend = eventDate.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
        return isWeekend;
    }

}