

using Newtonsoft.Json;
using OWP.EventPicking.Api.Criteria;
using OWP.EventPicking.Api.Models;

namespace OWP.EventPicking.Api.Handlers;

public class UpdateEventsRequest : IRequest<Unit>
{
    public IList<UpdateEvents> Events { get; set; }
}

public class UpdateMaxbetHandler : IRequestHandler<UpdateEventsRequest, Unit>
{
    private readonly Repository repository;

    private readonly INapSession session;

    public UpdateMaxbetHandler(Repository repository, INapSession session)
    {
        this.repository = repository;
        this.session = session;
    }

    public async Task<Unit> Handle(UpdateEventsRequest request, CancellationToken cancellationToken)
    {
        var TargetJson = JsonConvert.SerializeObject(request.Events);
        var criteria = new UpdateMaxbetSpreadCriteria
        {
            TargetJson = TargetJson,
            ActorId = session.UserId
        };
        await repository.UpdateEvent(criteria);

        return Unit.Value;
    }

}