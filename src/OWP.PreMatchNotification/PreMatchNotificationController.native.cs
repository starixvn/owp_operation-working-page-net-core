namespace OWP.PreMatchNotification
{
    using System;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    [AllowAnonymous]
    public class PreMatchNotificationController
    {
        private readonly IMediator mediator;

        public PreMatchNotificationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Route("notify")]
        [HttpGet]
        public async Task Notify(string sessionId, DateTime fromDate, DateTime toDate)
        {
            await mediator.Send(new NotifyRequest { SessionId = sessionId, FromDate = fromDate, ToDate = toDate });
        }
    }
}