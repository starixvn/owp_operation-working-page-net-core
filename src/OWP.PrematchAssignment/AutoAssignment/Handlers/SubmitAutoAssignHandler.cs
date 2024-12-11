using Fanex.NAP.Client;
using MediatR;
using OWP.PrematchAssignment.Constants;
using OWP.PrematchAssignment.Models;
using OWP.PrematchAssignment.Services;
using OWP.RabbitMQ;
using OWP.RabbitMQ.Constants;

namespace OWP.PrematchAssignment.AutoAssignment.Handlers;

public class SubmitAutoAssignRequest : IRequest
{
    public DateTime Date { get; set; }
    public IEnumerable<PreMatchLeagueAssignment> Assignments { get; set; }
}

public class SubmitAutoAssignHandler(
    PreMatchAssignmentService preMatchAssignmentService,
    MessageClient messageClient,
    INapSession napSession) : IRequestHandler<SubmitAutoAssignRequest>
{
    public async Task Handle(SubmitAutoAssignRequest request, CancellationToken cancellationToken)
    {
        if (request.Assignments.Any())
        {
            var preMatchLeagueIdUpdated = request.Assignments.Select(item => item.PreMatchLeagueId).Distinct().ToList();
            await preMatchAssignmentService.SaveOldPreMatchAssignment(preMatchLeagueIdUpdated);
            await preMatchAssignmentService.AutoAssign(request.Assignments);

            var fromDateGmt8 = DateTime.Today.AddHours(12);
            var toDateGmt8 = DateTime.Today.AddDays(1).AddHours(11).AddMinutes(59);
            var currentTime = DateTime.Now.TimeOfDay;
            var requestDate = request.Date.Date + currentTime;
            var requestDateGmt8 = requestDate.AddHours(12);

            if (requestDateGmt8 > fromDateGmt8 && requestDateGmt8 <= toDateGmt8)
            {
                messageClient.SendMessage(new
                {
                    RedisKey = RedisKey.KEY_PREMATCH_ASSIGNMENT_OLD_VALUE_SNAPSHOT,
                    ActorId = napSession.UserId
                }, RoutingKey.PrematchAssignment);
            }
        }
    }
}