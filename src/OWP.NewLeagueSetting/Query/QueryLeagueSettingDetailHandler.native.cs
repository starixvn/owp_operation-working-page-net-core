
namespace OWP.NewLeagueSetting.Query;

public record QueryLeagueSettingDetailRequest(int leagueSettingId) : Query<LeagueSettingInfoSender>;

public class QueryLeagueSettingDetailHandler(Repository repository)
    : QueryHandler<QueryLeagueSettingDetailRequest, LeagueSettingInfoSender>
{
    public async Task<LeagueSettingInfoSender> Handle(QueryLeagueSettingDetailRequest request, CancellationToken cancellationToken)
        => await repository.QueryLeagueSettingDetail(request.leagueSettingId);

}
