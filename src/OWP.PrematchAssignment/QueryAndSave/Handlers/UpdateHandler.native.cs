using MediatR;
using OWP.PrematchAssignment.QueryAndSave.Models;
using OWP.PrematchAssignment.QueryAndSave.Services;

namespace OWP.PrematchAssignment.QueryAndSave.Handlers;

public class UpdateLeagueAssignmentRequest : IRequest<Unit>
{
    public int[] PrematchIdDeleted { get; set; }

    public LeagueAssignment[] PrematchUpdated { get; set; }

    public string[] LeaguesName { get; set; }

    public TimeSpan? StartTime { get; set; }

    public DateTime Date { get; set; }
}

public class UpdateHandler(PreMatchAssignmentService preMatchAssignmentService) : IRequestHandler<UpdateLeagueAssignmentRequest, Unit>
{
    public async Task<Unit> Handle(UpdateLeagueAssignmentRequest request, CancellationToken cancellationToken)
    {
        if (request.StartTime.HasValue)
        {
            await preMatchAssignmentService.UpdateStartTime(request);
        }
        await preMatchAssignmentService.Delete(request);
        await preMatchAssignmentService.Update(request);

        return Unit.Value;
    }
}