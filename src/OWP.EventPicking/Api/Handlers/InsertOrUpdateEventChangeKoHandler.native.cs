using Newtonsoft.Json;
using NPOI.OpenXmlFormats.Dml;

namespace OWP.EventPicking.Api.Handlers;

public record InsertOrUpdateEventChangeKoRequest(int InfositeId, DateTime OldEventDate, DateTime NewEventDate) : Command<Unit>;

public class InsertOrUpdateEventChangeKoHandler(
    IDynamicRepository repository,
    LogWriterService logger) : IRequestHandler<InsertOrUpdateEventChangeKoRequest, Unit>
{
    private const int DefaultGmt8 = 8;
    public async Task<Unit> Handle(InsertOrUpdateEventChangeKoRequest request, CancellationToken cancellationToken)
    {
        var logs=await CreateLogsChangeModels(request);
        if (logs.Any())
        {
            await WriteLogForMonitor(request, "GetEventChangeGameDate");
            await WriteLogForMonitor(logs,"ChangeKODate");
        }
        //await SaveLogs(logs);
        return Unit.Value;
    }

    public async Task<IList<LoggedChangeModel>> CreateLogsChangeModels(InsertOrUpdateEventChangeKoRequest request)
    {
        var oldEventDateGmt8 = request.NewEventDate.AddHours(DefaultGmt8);
        var newEventDateGmt8 = request.OldEventDate.AddHours(DefaultGmt8);

        var formattedOldDate = oldEventDateGmt8.ToString("MM/dd/yyyy HH:mm:ss");
        var formattedNewDate = newEventDateGmt8.ToString("MM/dd/yyyy HH:mm:ss");
        var currentDateTime = DateTime.Now.AddHours(12);

        if (IsMoveKODate(oldEventDateGmt8, newEventDateGmt8))
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
        await WriteLogForMonitor($"Ignore build log because only adjustment time from {formattedOldDate} to {newEventDateGmt8}", "ChangeKODate");
        return new List<LoggedChangeModel>();


    }

    private static bool IsMoveKODate(DateTime oldGameDate, DateTime newGameDate)
    {
        
        var milestoneDate = oldGameDate.TimeOfDay >= new TimeSpan(12, 0, 0)
        ? oldGameDate.Date : oldGameDate.Date.AddDays(-1);

        var from = milestoneDate.AddHours(12);
        var to = milestoneDate.AddDays(1).AddHours(12).AddSeconds(-1);

        return newGameDate < from || newGameDate > to;
    }

    private async Task WriteLogForMonitor(object request, string action)
    {
        await logger.Execute(new InfoSiteLogCriteria()
        {
            CreatedDate = DateTime.Now.AddHours(12),
            Action = action,
            Payload = JsonConvert.SerializeObject(request)
        });
    }
    private async Task SaveLogs(IList<LoggedChangeModel> logs)
    {
        var xml = XmlConverter.Serialize(logs);
        var criteria = new SaveLogsChangeCriteria
        {
            Xml = xml
        };
        await repository.ExecuteAsync(criteria);
    }
}