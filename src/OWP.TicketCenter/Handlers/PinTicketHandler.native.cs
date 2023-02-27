using MediatR;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Middleware.Models;

namespace OWP.TicketCenter.Handlers;

public class PinTicketRequest : PermissionCheckedModel, IRequest<Unit>
{

    public bool IsPinned { get; set; }
}

public class PinTicketHandler : IRequestHandler<PinTicketRequest, Unit>
{
    private readonly Repository repository;

    public PinTicketHandler(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<Unit> Handle(PinTicketRequest request, CancellationToken cancellationToken)
    {
        await repository.PinTicket(new PinTicketCriteria { ActorId = request.ActorId, TicketId = request.TicketId, IsPinned = request.IsPinned });

        return await Task.FromResult(Unit.Value);
    }
}