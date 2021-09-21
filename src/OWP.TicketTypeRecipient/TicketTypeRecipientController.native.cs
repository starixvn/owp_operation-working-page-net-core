namespace OWP.TicketTypeRecipient
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using MediatR;
    using Models;
    using OWP.TicketTypeRecipient.Handlers;
    using Microsoft.AspNetCore.Authorization;

    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class TicketTypeRecipientController : ControllerBase
    {
        private readonly IMediator mediator;

        public TicketTypeRecipientController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Route("get-ticket-recipients")]
        [HttpGet]
        public async Task<IList<TicketTypeRecipient>> GetTicketTypeRecipients()
        {
            return await mediator.Send(new GetTicketTypeRecipientRequest());
        }
    }
}