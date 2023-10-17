using Fanex.Data.Repository;
using MediatR;
using OWP.Kpi.LA.Criteria;
using OWP.Kpi.LA.Models;
using OWP.Utils;

namespace OWP.Kpi.LA.Handlers;

public class GetLogRequest : IRequest<IEnumerable<LogDisplayModel>>
{
    public int UserId { get; set; }
}

public class GetLogRequestHandler : IRequestHandler<GetLogRequest, IEnumerable<LogDisplayModel>>
{

    private readonly IDynamicRepository repository;

    public GetLogRequestHandler(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IEnumerable<LogDisplayModel>> Handle(GetLogRequest request, CancellationToken cancellationToken)
    {
        var criteria = new GetLogTargetLevelCriteria { UserId = request.UserId };
        var logModel = await repository.FetchAsync<UserLogModel>(criteria, cancellationToken);

        return BuildLogs(logModel);
    }

    public static IEnumerable<LogDisplayModel> BuildLogs(IEnumerable<UserLogModel> logInfos)
    {
        foreach (var log in logInfos)
        {
            var infoBeforeChange = DeserializeXml(log.DataOld);
            var infoAfterChange = DeserializeXml(log.Data);
            yield return new LogDisplayModel
            {
                OldValue = infoBeforeChange.Target,
                NewValue = infoAfterChange.Target,
                ModifyDate = log.ModifiedDate.ToString("dd MMM yyyy hh:mm tt"),
                ModifyBy = log.ModifiedBy
            };
        }
    }

    private static TargetSettingModelDeserialize DeserializeXml(string log)
        => BuildLogXmlHelper.Deserialize<TargetSettingModelDeserialize>(log).FirstOrDefault();
}

public class UserLogModel
{
    public string DataOld { get; set; }

    public string Data { get; set; }

    public DateTime ModifiedDate { get; set; }

    public string ModifiedBy { get; set; }
}
