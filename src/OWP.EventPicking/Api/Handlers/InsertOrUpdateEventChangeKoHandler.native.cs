using Newtonsoft.Json;

namespace OWP.EventPicking.Api.Handlers;

public record InsertOrUpdateEventChangeKoRequest(int InfositeId, DateTime OldEventDate, DateTime NewEventDate) : Command<Unit>;

public class InsertOrUpdateEventChangeKoHandler(
    IDynamicRepository repository,
    LogWriterService logger) : IRequestHandler<InsertOrUpdateEventChangeKoRequest, Unit>
{
    private const int DefaultGmt8 = 8;
    public async Task<Unit> Handle(InsertOrUpdateEventChangeKoRequest request, CancellationToken cancellationToken)
    {
        var logs=CreateLogsChangeModels(request);
        await WriteLogForMonitor(request, "GetEventChangeGameDate");
        await SaveLogs(logs);
        return Unit.Value;
    }

    public static IList<LoggedChangeModel> CreateLogsChangeModels(InsertOrUpdateEventChangeKoRequest request)
    {
        var formattedNewDate = request.NewEventDate.AddHours(DefaultGmt8).ToString("MM/dd/yyyy HH:mm:ss");
        var formattedOldDate = request.OldEventDate.AddHours(DefaultGmt8).ToString("MM/dd/yyyy HH:mm:ss");
        var currentDateTime = DateTime.Now.AddHours(12);
       
        return new List<LoggedChangeModel>
        {
            new()
            {
                InfoSiteMatchId = request.InfositeId,
                UpdateTypeId = UpdateTypeConstant.MOVE_KICKOFF_DATE,
                Action = UpdateTypeConstant.UpdateType[UpdateTypeConstant.MOVE_KICKOFF_DATE],
                NewValue = formattedNewDate,
                OldValue = formattedOldDate,
                UpDateTime = currentDateTime,
                FromService = "APIGetEventChangeGameDate"
            },
            new()
            {
                InfoSiteMatchId = request.InfositeId,
                UpdateTypeId = UpdateTypeConstant.RENEW_MATCH,
                Action = UpdateTypeConstant.UpdateType[UpdateTypeConstant.RENEW_MATCH],
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
            CreatedDate = DateTime.Now.AddHours(12),
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
}