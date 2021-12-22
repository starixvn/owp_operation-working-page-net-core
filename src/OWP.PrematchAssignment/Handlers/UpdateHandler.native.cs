using MediatR;
using OWP.PreMatchAssignment.Constants;
using OWP.PreMatchAssignment.Models;
using OWP.PreMatchAssignment.Services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

       // private readonly IHttpHandler httpHandler;

        public UpdateHandler(
            PreMatchAssignmentService preMatchAssignmentService
        )
        {
            this.preMatchAssignmentService = preMatchAssignmentService;
            //this.httpHandler = httpHandler;
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

            // TODO
            //var requestLink = $"{AppSettings.NetCoreIP}/api/PreMatchNotification/notify-assignment?redisKey={RedisKey.KEY_PREMATCH_ASSIGNMENT_OLD_VALUE_SNAPSHOT}";

            var fromDateGmt8 = DateTime.Today.AddHours(12);
            var toDateGmt8 = DateTime.Today.AddDays(1).AddHours(11).AddMinutes(59);
            var currentTime = DateTime.Now.TimeOfDay;
            var requestDate = request.Date.Date + currentTime;
            var requestDateGmt8 = requestDate.AddHours(12);
            if (requestDateGmt8 > fromDateGmt8
                && requestDateGmt8 <= toDateGmt8)
            {
                //await httpHandler.GetAsync(requestLink);
            }

            return Unit.Value;
        }
    }
}