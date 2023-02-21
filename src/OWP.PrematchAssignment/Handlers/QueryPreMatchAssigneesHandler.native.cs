using OWP.PrematchAssignment.Services;

namespace OWP.PrematchAssignment.Handlers;

using MediatR;
using Models;

public class QueryAssigneesRequest : IRequest<IEnumerable<TicketAssignee>>
{
    public int Id { get; set; }
}

public class QueryPreMatchAssigneesHandler : IRequestHandler<QueryAssigneesRequest, IEnumerable<TicketAssignee>>
{
    private readonly PreMatchAssignmentService preMatchAssignmentService;

    public QueryPreMatchAssigneesHandler(PreMatchAssignmentService preMatchAssignmentService)
    {
        this.preMatchAssignmentService = preMatchAssignmentService;
    }

    public async Task<IEnumerable<TicketAssignee>> Handle(QueryAssigneesRequest request, CancellationToken cancellationToken)
    {
        var result = await preMatchAssignmentService.QueryAssigneesByLeagueId(request.Id);

        return result.ToList();
    }
}