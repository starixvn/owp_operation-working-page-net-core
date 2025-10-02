namespace OWP.NewLeagueSetting.Query;

public record QueryLogRequest(int leagueSettingId) : Query<IList<UILogInfo>>;

public class QueryLogHandler(Repository repository)
    : QueryHandler<QueryLogRequest, IList<UILogInfo>>
{
    public async Task<IList<UILogInfo>> Handle(QueryLogRequest request, CancellationToken cancellationToken)
    {
        var dbLogInfo=await repository.QueryLog(request.leagueSettingId);
        return dbLogInfo.Select(l => new UILogInfo
        {
            LogId= l.LogId,
            SettingId=l.SettingId,
            CreatedDate =l.CreatedDate,
            ModifiedBy= l.ModifiedBy,
            Action = l.Action,
            Details = JsonConvert.DeserializeObject<IList<LogDetail>>(l.Content)
        }).ToList();
    } 
}