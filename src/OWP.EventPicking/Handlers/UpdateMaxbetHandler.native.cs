
namespace OWP.EventPicking.Handlers;

public class UpdateMaxbetRequest : MaxbetSpread, IRequest<Unit>
{ }

public class UpdateMaxbetHandler : IRequestHandler<UpdateMaxbetRequest, Unit>
{

    public UpdateMaxbetHandler()
    {

    }

    public async Task<Unit> Handle(UpdateMaxbetRequest request, CancellationToken cancellationToken)
    {
        // TODO
        return Unit.Value;
    }
}