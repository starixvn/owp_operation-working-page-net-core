using MediatR;
using OWP.PrematchAssignment.QueryAndSave.Models;
using OWP.PrematchAssignment.QueryAndSave.Services;

namespace OWP.PrematchAssignment.QueryAndSave.Handlers;

public class CreateRCDiscussionRequest : IRequest<Unit>
{
    public int InfoSiteMatchId { get; set; }
}

public class CreateRCDiscussionHandle(PreMatchAssignmentService preMatchAssignmentService) : IRequestHandler<CreateRCDiscussionRequest, Unit>
{
    public async Task<Unit> Handle(CreateRCDiscussionRequest request, CancellationToken cancellationToken)
    {
        var assignments = await preMatchAssignmentService.GetMatchAssignments(request.InfoSiteMatchId);
        return Unit.Value;

    }
}