
using MediatR;

namespace OWP.EventPicking.Handlers;

public class EditEventPickingRequest : EventPickingRequest, IRequest<EventPickingResult>
{ }

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

        var tvList = (await repository.GetTVList()).ToList();

        foreach (var match in matchFiltered)
        {
            var leagueSettings = leagueSetting.Where(x => x.OWPLeagueId == match.OWPLeagueId).ToList();
            var flag = 0;
            foreach (var league in leagueSettings)
            {
                if (CheckMatchValidMappingWithLeagueManagement(match, league, tvList))
                {
                    flag += 1;
                    BuildMaxBet(match, league);
                    BuildSpreadHdp(match, league);
                    BuildSpreadOu(match, league);
                    BuildNoOfHdp(match, league);
                }
            }

            if (flag > 1)
            {
                match.MaxbetHdp = null;
            }
        }

        return new EventPickingResult
        {
            MatchesByDate = mainMatches.MatchesByDate,
            FilteredMatches = matchFiltered
        };
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
            ? league.MaxbetWK_NoOfHDP_Show
            : league.MaxbetWD_NoOfHDP_Show;
        match.IsReferenceLeagueManagement = true;
    }

    private bool CheckTvConditions(EventPickingModel match, LeagueSettingInfoSender leagueSetting, IList<Condition> tvList)
    {
        var tvContains = GetValueContains(tvList);
        var tvEqual = GetValueEquals(tvList);
        var checkTv = !leagueSetting.Condition.HasTv || tvContains.Any(x => x.Contains(match.TV)) || tvEqual.Any((x=>x.Equals(match.TV)));
        
        var checkIptv = false;
        if (leagueSetting.Condition.HasIpTv)
        {
            checkIptv = match.Iptv.Equals("IPTV");
        }
        return checkTv || checkIptv;
    }

    private bool CheckRoundOrDivisionConditions(string matchValues, IList<Condition> conditions)
    {
        var containsValues = GetValueContains(conditions);
        var equalsValues = GetValueEquals(conditions);

        var isEmptyBoth = conditions.Count == 0 && matchValues.Length == 0;
        var isContains = containsValues.Any(x => matchValues.Contains(x, StringComparison.CurrentCultureIgnoreCase));
        var isEquals = equalsValues.Any(x => x.Equals(matchValues, StringComparison.CurrentCultureIgnoreCase));
        return isEmptyBoth || isContains || isEquals;
    }

    private bool CheckMatchValidMappingWithLeagueManagement(EventPickingModel match, LeagueSettingInfoSender leagueSetting, IList<Condition> tvList)
    {
        var roundConditions = CheckRoundOrDivisionConditions(match.Round, leagueSetting.Condition.Round);
        var divisionConditions = CheckRoundOrDivisionConditions(match.Division, leagueSetting.Condition.Division);
        var tvConditions = CheckTvConditions(match, leagueSetting, tvList);

        return roundConditions && divisionConditions && tvConditions;
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