using MediatR;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Handlers;

public class GetStatusHistoryRequest : IRequest<IEnumerable<StatusHistory>>
{
    public int TicketId { get; set; }
}

public class GetStatusHistoryHandler : IRequestHandler<GetStatusHistoryRequest, IEnumerable<StatusHistory>>
{
    private readonly Repository repository;

    public GetStatusHistoryHandler(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<IEnumerable<StatusHistory>> Handle(GetStatusHistoryRequest request, CancellationToken cancellationToken)
        => (await repository.GetStatusHistory(request.TicketId));
}