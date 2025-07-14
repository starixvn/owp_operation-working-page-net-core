namespace OWP.EventPicking.Api.Services;

public interface LeagueSettingForClubFriendlyBuilder
{
    Task Build(IList<EventPickingModel> mainEvents, CancellationToken cancellationToken);
}
public class LeagueSettingForClubFriendlyBuilderImp: LeagueSettingForClubFriendlyBuilder
{
    private readonly IList<string> FiveMajorLeague = new List<string>()
    {
        "*ENGLISH PREMIER LEAGUE",
        "*GERMANY-BUNDESLIGA I",
        "*ITALY SERIE A",
        "*SPAIN PRIMERA LALIGA",
        "FRANCE LIGUE 1"
    };

    public async Task Build(IList<EventPickingModel> mainEvents, CancellationToken cancellationToken)
    {
        var events= GetMatchesClubFriendlyIgnore5MajorLeague(mainEvents);
        await BuildLeagueSetting(events, cancellationToken);
    }

    private IList<EventPickingModel> GetMatchesClubFriendlyIgnore5MajorLeague(
        IList<EventPickingModel> matches)
    => matches.Where(x => x.IsClubFriendlyLeague
        && !(FiveMajorLeague.Contains(x.HomeMainLeague) && FiveMajorLeague.Contains(x.AwayMainLeague)))
       .ToList();
    

    private async Task BuildLeagueSetting(IList<EventPickingModel> events, CancellationToken cancellationToken)
    {
        // TODO
    }
}
