
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
        // TODO
        var eventPickingMatches = await service.Execute(request);
        return eventPickingMatches;
        /* var unKnowMaxbetMatches = eventPickingMatches.MatchesByDate.ToList().Where(x => x.MaxbetHdp == null).ToList();

        var leagueSetting = (await repository.QueryLeagueSetting()).ToList();

        foreach (var match in unKnowMaxbetMatches)
        {
            var leagueSettings = leagueSetting.Where(x => x.OWPLeagueId == match.OWPLeagueId).ToList();

            if (leagueSettings.Any())
            {
                foreach (var league in leagueSettings)
                {
                    var roundContains = league.Condition.Round.Where(x => x.Type == "Contains").Select(x=> x.Value).ToList();
                    var roundEquals = league.Condition.Round.Where(x => x.Type != "Contains").Select(x => x.Value).ToList();
                    var con1 = league.Condition.Round.Count == 0 && match.Round.Length == 0;
                    var con2 = roundContains.Any(x => match.Round.Contains(x)) ||
                               roundEquals.Any(x => x.Equals(match.Round));
                    var divisionContains = league.Condition.Division.Where(x => x.Type == "Contains").Select(x => x.Value).ToList();
                    var divisionEquals = league.Condition.Division.Where(x => x.Type != "Contains").Select(x => x.Value).ToList();
                    var con3 = league.Condition.Division.Count == 0 && match.Division.Length == 0;
                    var con4 = divisionContains.Any(x => match.Round.Contains(x)) ||
                               divisionEquals.Any(x => x.Equals(match.Round));
                    if ((con1 && con3) || (con2 && con4))
                    {
                        match.MaxbetHdp = IsWeekend(match.EventDateTime ) ? league.MaxbetWK_HDP_value : league.MaxbetWD_HDP_value;
                    }

                }
            }
        }

        var test = unKnowMaxbetMatches.Where(x => x.MaxbetHdp != null);
        return await service.Execute(request); */
    }

    private static bool IsWeekend(DateTime eventDateTime)
    {
        var eventDate = DateTime.SpecifyKind(eventDateTime, DateTimeKind.Utc).AddHours(-4);
        var isWeekend = eventDate.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
        return isWeekend;
    }


}