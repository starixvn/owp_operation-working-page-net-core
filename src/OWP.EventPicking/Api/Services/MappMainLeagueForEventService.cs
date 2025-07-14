namespace OWP.EventPicking.Api.Services;
public interface MapMainLeagueForEventService
{
    public void Execute(IList<EventPickingModel> eventPicking, IList<main_league_info> mainLeagueInfo);
}

public class MapMainLeagueForEventServiceImp: MapMainLeagueForEventService
{
    public void Execute(IList<EventPickingModel> eventPicking, IList<main_league_info> mainLeagueInfo)
    {
        foreach (var @event in eventPicking)
        {
            var (homeMainLeague, awayMainLeague) = GetMainLeague(@event.HomeName, @event.AwayName, mainLeagueInfo);
            @event.HomeMainLeague = homeMainLeague;
            @event.AwayMainLeague = awayMainLeague;
        }
    }

    private static (string homeMainLeague, string awayMainLeague) GetMainLeague(string homeName, string awayName, IList<main_league_info> mainLeagueInfo)
    {
        return (mainLeagueInfo.FirstOrDefault(x => x.teamName == homeName)?.mainLeague,
            mainLeagueInfo.FirstOrDefault(x => x.teamName == awayName)?.mainLeague);
    }
}
