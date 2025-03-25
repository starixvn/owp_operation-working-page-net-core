using MediatR;
using OWP.PrematchAssignment.QueryAndSave.Models;
using OWP.PrematchAssignment.QueryAndSave.Services;

namespace OWP.PrematchAssignment.QueryAndSave.Handlers;

public class QueryPreMatchLeagueByIdRequest : IRequest<QueryPreMatchLeagueViewModel>
{
    public int PreMatchLeagueId { get; set; }

    public TimeSpan? StartTime { get; set; }
}

public class QueryPreMatchLeagueByIdHandler : IRequestHandler<QueryPreMatchLeagueByIdRequest, QueryPreMatchLeagueViewModel>
{
    private readonly PreMatchAssignmentService preMatchAssignmentService;

    public QueryPreMatchLeagueByIdHandler(PreMatchAssignmentService preMatchAssignmentService)
    {
        this.preMatchAssignmentService = preMatchAssignmentService;
    }

    public async Task<QueryPreMatchLeagueViewModel> Handle(QueryPreMatchLeagueByIdRequest request, CancellationToken cancellationToken)
    {
        return await preMatchAssignmentService.QueryById(request);
    }
}