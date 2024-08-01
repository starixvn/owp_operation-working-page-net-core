
using OWP.CQRS;

namespace OWP.NewLeagueSetting.Query;

public record QueryLeagueSettingDetailRequest(int leagueSettingId) : Query<LeagueSettingInfoSender>;

public class QueryLeagueSettingDetailHandler : QueryHandler<QueryLeagueSettingDetailRequest, LeagueSettingInfoSender>
{
    private readonly Repository repository;

    public QueryLeagueSettingDetailHandler(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<LeagueSettingInfoSender> Handle(QueryLeagueSettingDetailRequest request, CancellationToken cancellationToken)
        => await repository.QueryLeagueSettingDetail(request.leagueSettingId);

}
