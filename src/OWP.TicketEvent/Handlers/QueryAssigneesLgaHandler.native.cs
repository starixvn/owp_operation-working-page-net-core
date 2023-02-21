using MediatR;
using OWP.TicketEvent.Models;

namespace OWP.TicketEvent.Handlers;

public class QueryAssigneesLgaRequest: IRequest<IEnumerable<TicketAssignees>>
{
    public int OwpMatchId { get; set; }
}

public class QueryAssigneesLgaHandler : IRequestHandler<QueryAssigneesLgaRequest, IEnumerable<TicketAssignees>>
{
    private readonly Repository repository;

    public QueryAssigneesLgaHandler(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<IEnumerable<TicketAssignees>> Handle(QueryAssigneesLgaRequest request, CancellationToken cancellationToken)
    {
        var result = await repository.QueryAssigneesByLeagueId(request.OwpMatchId);

        return result;
    }
}