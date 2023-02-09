namespace OWP.TicketCenter.Handlers;

using MediatR;

public class MarkAllTicketsRequest : IRequest<Unit>
{
    public int ActorId { get; set; }
}

public class MarkAllTicketsHandler : IRequestHandler<MarkAllTicketsRequest, Unit>
{
    private readonly Repository repository;

    public MarkAllTicketsHandler(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<Unit> Handle(MarkAllTicketsRequest request, CancellationToken cancellationToken)
    {
        await repository.UpdateTicketsAsRead(request.ActorId);

        return await Task.FromResult(Unit.Value);
    }
}