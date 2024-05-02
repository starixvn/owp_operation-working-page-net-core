
using OWP.NewLeagueSetting.Data;

namespace OWP.NewLeagueSetting.Query;

public record QueryLeagueSettingRequest(string league) : IRequest<IEnumerable<LeagueSettingInfoSender>>;

public class QueryLeagueSettingHandler : IRequestHandler<QueryLeagueSettingRequest, IEnumerable<LeagueSettingInfoSender>>
{
    private readonly Repository repository;

    public QueryLeagueSettingHandler(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<IEnumerable<LeagueSettingInfoSender>> Handle(QueryLeagueSettingRequest request, CancellationToken cancellationToken)
      => await repository.QueryLeagueSetting(request.league);

}
