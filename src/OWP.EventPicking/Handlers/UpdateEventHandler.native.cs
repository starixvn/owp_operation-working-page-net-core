
using System.Xml;
using Olympus.Xml;

namespace OWP.EventPicking.Handlers;

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
        var xml = XmlConverter.Serialize(request.Events);
        var criteria = new UpdateMaxbetSpreadCriteria
        {
            Xml = xml,
            ActorId = session.UserId
        };
        await repository.UpdateEvent(criteria);

        return Unit.Value;
    }

}