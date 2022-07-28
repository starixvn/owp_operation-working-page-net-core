using MediatR;
using OWP.PreMatchAssignment.Models;
using OWP.PreMatchAssignment.Services;

namespace OWP.PreMatchAssignment.Handlers
{
    public class UpdateLeagueAssignmentRequest : IRequest
    {
        public int[] PrematchIdDeleted { get; set; }

        public LeagueAssignment[] PrematchUpdated { get; set; }

        public TimeSpan? StartTime { get; set; }

        public DateTime Date { get; set; }
    }

    public class UpdateHandler : IRequestHandler<UpdateLeagueAssignmentRequest>
    {
        private readonly PreMatchAssignmentService preMatchAssignmentService;

        private readonly IMediator mediator;

        public UpdateHandler(
          PreMatchAssignmentService preMatchAssignmentService,
          IMediator mediator)
        {
            this.preMatchAssignmentService = preMatchAssignmentService;
            this.mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateLeagueAssignmentRequest request, CancellationToken cancellationToken)
        {
            if (request.StartTime.HasValue)
            {
                await preMatchAssignmentService.UpdateStartTime(request);
            }
            await preMatchAssignmentService.Delete(request);
            var preMatchLeagueIdUpdated = request.PrematchUpdated.Select(item => item.PreMatchLeagueId).ToList();
            await preMatchAssignmentService.SaveOldPreMatchAssignment(preMatchLeagueIdUpdated);
            await preMatchAssignmentService.Update(request);

            //var fromDateGmt8 = DateTime.Today.AddHours(12);
            //var toDateGmt8 = DateTime.Today.AddDays(1).AddHours(11).AddMinutes(59);
            //var currentTime = DateTime.Now.TimeOfDay;
            //var requestDate = request.Date.Date + currentTime;
            //var requestDateGmt8 = requestDate.AddHours(12);
            //if (requestDateGmt8 > fromDateGmt8
            //    && requestDateGmt8 <= toDateGmt8)
            //{
            //    var notifyRequest = new PrematchAssignmentNotifyRequest
            //    {
            //        RedisKey = RedisKey.KEY_PREMATCH_ASSIGNMENT_OLD_VALUE_SNAPSHOT
            //    };

            //    await mediator.Send(notifyRequest);
            //}

            return Unit.Value;
        }
    }
}