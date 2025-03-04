using Newtonsoft.Json;

namespace OWP.EventPicking.Api.Handlers;

public record InsertOrUpdateEventChangeKoRequest(int InfositeId, DateTime NewEventDate) : Command<Unit>;

public class InsertOrUpdateEventChangeKoHandler(
    IDynamicRepository repository,
    LogWriterService logger) : IRequestHandler<InsertOrUpdateEventChangeKoRequest, Unit>
{

    public async Task<Unit> Handle(InsertOrUpdateEventChangeKoRequest request, CancellationToken cancellationToken)
    {
        var logs=CreateLogsChangeModels(request, await GetEventDateByInfoSiteId(request.InfositeId));
        await WriteLogForMonitor(request, "GetEventChangeGameDate");
        await WriteLogForMonitor(logs, "LogChangeGameDate");
        //await SaveLogs(logs);
        await SaveLogs(logs);
        return Unit.Value;
    }

    public static IList<LoggedChangeModel> CreateLogsChangeModels(InsertOrUpdateEventChangeKoRequest request, DateTime? oldDateTime)
    {
        if (oldDateTime is null)
        {
            return new List<LoggedChangeModel>();
        }

        var formattedNewDate = request.NewEventDate.ToString("MM/dd/yyyy HH:mm:ss");
        var formattedOldDate = oldDateTime.Value.ToString("MM/dd/yyyy HH:mm:ss");
        var currentDateTime = DateTime.Now.AddHours(12);
        var action = UpdateTypeConst.UpdateType[UpdateTypeConstant.MOVE_KICKOFF_DATE];

        return new List<LoggedChangeModel>
        {
            new()
            {
                InfoSiteMatchId = request.InfositeId,
                UpdateTypeId = UpdateTypeConstant.MOVE_KICKOFF_DATE,
                Action = UpdateTypeConst.UpdateType[UpdateTypeConstant.MOVE_KICKOFF_DATE],
                NewValue = formattedNewDate,
                OldValue = formattedOldDate,
                UpDateTime = currentDateTime,
                FromService = "APIGetEventChangeGameDate"
            },
            new()
            {
                InfoSiteMatchId = request.InfositeId,
                UpdateTypeId = UpdateTypeConstant.RENEW_MATCH,
                Action = UpdateTypeConst.UpdateType[UpdateTypeConstant.RENEW_MATCH],
                NewValue = formattedNewDate,
                OldValue = formattedOldDate,
                UpDateTime = currentDateTime,
                FromService = "APIGetEventChangeGameDate"
            }
        };
    }

    public async Task WriteLogForMonitor(object request, string action)
    {
        await logger.Execute(new InfoSiteLogCriteria()
        {
            CreatedDate = DateTime.Now,
            Action = action,
            Payload = JsonConvert.SerializeObject(request)
        });
    }
    public async Task SaveLogs(IList<LoggedChangeModel> logs)
    {
        var xml = XmlConverter.Serialize(logs);
        var criteria = new SaveLogsChangeCriteria
        {
            Xml = xml
        };
        await repository.ExecuteAsync(criteria);
    }
    public async Task<DateTime?> GetEventDateByInfoSiteId(int infoSiteId)
    {
        var criteria= new GetEventDateByInfoSiteId()
        {
            InfoSiteId = infoSiteId
        };
        await repository.FetchAsync<int>(criteria);

        return criteria.EventDate;
    }
}