namespace OWP.NewLeagueSetting.Query;

public record QueryLogRequest(int leagueSettingId) : Query<IList<LogInfo>>;

public class QueryLogHandler(Repository repository)
    : QueryHandler<QueryLogRequest, IList<LogInfo>>
{
    public async Task<IList<LogInfo>> Handle(QueryLogRequest request, CancellationToken cancellationToken)
        => await repository.QueryLog(request.leagueSettingId);

}