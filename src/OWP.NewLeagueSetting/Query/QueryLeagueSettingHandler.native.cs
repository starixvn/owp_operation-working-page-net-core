namespace OWP.NewLeagueSetting.Query;

public class QueryLeagueSettingRequest: LeagueParamRequest, IRequest<IEnumerable<LeagueSettingInfoSender>>;

public class QueryLeagueSettingHandler(Repository repository) 
    : IRequestHandler<QueryLeagueSettingRequest, IEnumerable<LeagueSettingInfoSender>>
{
    
    public async Task<IEnumerable<LeagueSettingInfoSender>> Handle(QueryLeagueSettingRequest request, CancellationToken cancellationToken)
      => await repository.QueryLeagueSetting(request);
}
