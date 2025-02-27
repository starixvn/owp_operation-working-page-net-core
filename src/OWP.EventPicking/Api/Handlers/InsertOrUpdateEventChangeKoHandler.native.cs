


using Newtonsoft.Json;

namespace OWP.EventPicking.Api.Handlers;

public record InsertOrUpdateEventChangeKoRequest(int InfositeId, DateTime NewEventDate) : Command<Unit>;

public class InsertOrUpdateEventChangeKoHandler(
    IDynamicRepository repository,
    LogWriterService logger) : IRequestHandler<InsertOrUpdateEventChangeKoRequest, Unit>
{

    public async Task<Unit> Handle(InsertOrUpdateEventChangeKoRequest request, CancellationToken cancellationToken)
    {
        await repository.ExecuteAsync(new InsertOrUpdateEventChangeKoCriteria
        {
            InfoSiteId = request.InfositeId,
            NewEventDate = request.NewEventDate
        });

        await logger.Execute(new InfoSiteLogCriteria()
        {
            CreatedDate = DateTime.Now,
            Action = "GetEventChangeKo",
            Payload = JsonConvert.SerializeObject(request)
        });
        return Unit.Value;
    }
}