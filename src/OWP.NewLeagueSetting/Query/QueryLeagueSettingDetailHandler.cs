
using OWP.NewLeagueSetting.Data;

namespace OWP.NewLeagueSetting.Query;

public record QueryLeagueSettingDetailRequest(int leagueSettingId) : IRequest<LeagueSettingInfoSender>;

public class QueryLeagueSettingDetailHandler : IRequestHandler<QueryLeagueSettingDetailRequest, LeagueSettingInfoSender>
{
    private readonly Repository repository;

    public QueryLeagueSettingDetailHandler(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<LeagueSettingInfoSender> Handle(QueryLeagueSettingDetailRequest request, CancellationToken cancellationToken)
        => await repository.QueryLeagueSettingDetail(request.leagueSettingId);

}
