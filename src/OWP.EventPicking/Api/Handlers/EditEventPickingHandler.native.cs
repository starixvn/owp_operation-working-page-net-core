using OWP.EventPicking.Api.Middleware.Models;
using OWP.EventPicking.Api.Models;
using OWP.EventPicking.Api.Services;

namespace OWP.EventPicking.Api.Handlers;

public class EditEventPickingRequest : EventPickingRequest, IRequest<EventPickingResult>;

public class EditEventPickingHandler : IRequestHandler<EditEventPickingRequest, EventPickingResult>
{
    private readonly QueryEventPickingService service;

    private readonly Repository repository;

    public EditEventPickingHandler(QueryEventPickingService service, Repository repository)
    {
        this.service = service;
        this.repository = repository;
    }

    public async Task<EventPickingResult> Handle(EditEventPickingRequest request, CancellationToken cancellationToken)
    {
        var mainMatches = await service.Execute(request);

        var matchFiltered = mainMatches.FilteredMatches;

        var leagueSetting = (await repository.QueryLeagueSetting()).ToList();

        foreach (var match in matchFiltered)
        {
            var leagueSettingByLeagueId = leagueSetting.Where(x => x.OWPLeagueId == match.OWPLeagueId).ToList();

            if (leagueSettingByLeagueId.Count == 1)
            {
                SuggestionValue(match, leagueSettingByLeagueId.First());
                continue;
            }

            var leagueIsMappingWithCondition = leagueSettingByLeagueId
                .Where(league => CheckMatchValidMappingWithLeagueManagement(match, league)).ToList();

            if (leagueIsMappingWithCondition.Count == 1)
            {
                SuggestionValue(match, leagueIsMappingWithCondition.First());
            }
        }

        return new EventPickingResult
        {
            MatchesByDate = mainMatches.MatchesByDate,
            FilteredMatches = matchFiltered
        };
    }

    private void SuggestionValue(EventPickingModel match, LeagueSettingInfoSender league)
    {
        BuildLstNote(match, league);
        BuildMaxBet(match, league);
        BuildSpreadHdp(match, league);
        BuildSpreadOu(match, league);
        BuildNoOfHdp(match, league);
    }

    private void BuildLstNote(EventPickingModel match, LeagueSettingInfoSender league)
    {
        if (match.LstNote != null) return;
        match.LstNote = league.LstNote;
        match.IsReferenceLeagueManagement = true;
    }

    private void BuildMaxBet(EventPickingModel match, LeagueSettingInfoSender league)
    {
        if (match.MaxbetHdp != null) return;
        match.MaxbetHdp = IsWeekend(match.EventDateTime) ? league.MaxbetWK_HDP_Value : league.MaxbetWD_HDP_Value;
        match.IsReferenceLeagueManagement = true;
    }

    private void BuildSpreadHdp(EventPickingModel match, LeagueSettingInfoSender league)
    {
        if (match.SpreadHdp != null) return;
        match.SpreadHdp = league.Spread_HDP_Value;
        match.IsReferenceLeagueManagement = true;
    }
    private void BuildSpreadOu(EventPickingModel match, LeagueSettingInfoSender league)
    {
        if (match.SpreadOu != null) return;
        match.SpreadOu = league.Spread_OU_Value;
        match.IsReferenceLeagueManagement = true;
    }
    private void BuildNoOfHdp(EventPickingModel match, LeagueSettingInfoSender league)
    {
        if (match.NoOfHDP != null) return;
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