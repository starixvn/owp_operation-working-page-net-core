using MediatR;
using OWP.PrematchAssignment.Models;
using OWP.PrematchAssignment.Services;

namespace OWP.PrematchAssignment.AutoAssignment.Handlers;

public class SubmitAutoAssignRequest : IRequest
{
    public DateTime Date { get; set; }
    public IEnumerable<PreMatchLeagueAssignment> Assignments { get; set; }
}

public class SubmitAutoAssignHandler(PreMatchAssignmentService preMatchAssignmentService) : IRequestHandler<SubmitAutoAssignRequest>
{
    public async Task Handle(SubmitAutoAssignRequest request, CancellationToken cancellationToken)
    {
        if (request.Assignments.Any())
        {
            await preMatchAssignmentService.AutoAssign(request.Assignments);
        }
    }
}