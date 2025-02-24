using MediatR;
using OWP.PrematchAssignment.QueryAndSave.Models;
using OWP.PrematchAssignment.QueryAndSave.Services;

namespace OWP.PrematchAssignment.QueryAndSave.Handlers;

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