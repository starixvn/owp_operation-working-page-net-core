
namespace OWP.EventPicking.Api.Handlers;

public record InsertOrUpdateEventChangeKoRequest(int InfositeId, DateTimeOffset NewKO): Command<Unit>;

public class InsertOrUpdateEventChangeKoHandler(IDynamicRepository repository) : IRequestHandler<InsertOrUpdateEventChangeKoRequest, Unit>
{
    public async Task<Unit> Handle(InsertOrUpdateEventChangeKoRequest request, CancellationToken cancellationToken)
    {
        await repository.ExecuteAsync(new InsertOrUpdateEventChangeKoCriteria
        {
            InfoSiteId = request.InfositeId,
            NewKo = request.NewKO.DateTime
        });
        return Unit.Value;
    }
}