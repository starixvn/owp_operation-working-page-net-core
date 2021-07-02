namespace OWP.PreMatchNotification
{
    using System;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using OWP.PreMatchNotification.Assignment.Handlers;
    using OWP.PreMatchNotification.InfoSiteMatchInfo.Handlers;
    using OWP.PreMatchNotification.LinkedMatch;
    using OWP.PreMatchNotification.NewLeague.Handlers;

    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    [EnableCors("AllowOrigin")]
    public class PreMatchNotificationController
    {
        private readonly IMediator mediator;

        public PreMatchNotificationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Route("notify-match-info")]
        [HttpGet]
        public async Task NotifyMatchInfo(string sessionId, DateTime fromDate, DateTime toDate)
            => await mediator.Send(new MatchInfoNotifyRequest { SessionId = sessionId, FromDate = fromDate, ToDate = toDate });

        [Route("notify-new-league")]
        [HttpGet]
        public async Task NotifyNewLeague(string sessionId, DateTime fromDate, DateTime toDate)
            => await mediator.Send(new NewLeagueNotifyRequest { SessionId = sessionId, FromDate = fromDate, ToDate = toDate });

        [Route("notify-linked-lga-match")]
        [HttpGet]
        public async Task NotifyLinkedLGAMatch(string sessionId, DateTime eventDate)
            => await mediator.Send(new LinkedMatchNotifyRequest { SessionId = sessionId, EventDate = eventDate });

        [Route("notify-assignment")]
        [HttpGet]
        public async Task NotifyAssignment(string redisKey)
            => await mediator.Send(new AssignmentNotifyRequest
            {
                RedisKey = redisKey
            });
    }
}