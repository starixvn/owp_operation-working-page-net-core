

using Newtonsoft.Json;
using OWP.CQRS;
using OWP.EventPicking.Api.Criteria;
using OWP.EventPicking.Api.Models;

namespace OWP.EventPicking.Api.Handlers;

public class SpecialProductUpdateMaxbetSpreadRequest : Command<Unit>
{
    public IList<SpecialProductMaxbetSpread> Events { get; set; }
}

public class SpecialProductUpdateMaxbetSpreadHandler : CommandHandler<SpecialProductUpdateMaxbetSpreadRequest, Unit>
{
    private readonly Repository repository;

    private readonly INapSession session;

    public SpecialProductUpdateMaxbetSpreadHandler(Repository repository, INapSession session)
    {
        this.repository = repository;
        this.session = session;
    }

    public async Task<Unit> Handle(SpecialProductUpdateMaxbetSpreadRequest request, CancellationToken cancellationToken)
    {
        var TargetJson = JsonConvert.SerializeObject(request.Events);
        var criteria = new SpecialProductUpdateMaxbetSpreadCriteria()
        {
            TargetJson = TargetJson,
            ActorId = session.UserId
        };
        await repository.ExecuteNonQuery(criteria);

        return Unit.Value;
    }

}