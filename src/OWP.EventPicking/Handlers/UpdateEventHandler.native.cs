
namespace OWP.EventPicking.Handlers;

public class UpdateEventsRequest : UpdateEvents, IRequest<Unit>
{ }

public class UpdateMaxbetHandler : IRequestHandler<UpdateEventsRequest, Unit>
{

    public UpdateMaxbetHandler()
    {

    }

    public async Task<Unit> Handle(UpdateEventsRequest request, CancellationToken cancellationToken)
    {
        // TODO
        return Unit.Value;
    }
}