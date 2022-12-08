namespace OWP.TicketTypeRecipient
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using OWP.TicketTypeRecipient.Handlers;
    using OWP.TicketTypeRecipient.Middleware;

    [Route("api/[controller]")]
    [ApiController]
    public class TicketTypeRecipientController : ControllerBase
    {
        private readonly IMediator mediator;

        public TicketTypeRecipientController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("get-ticket-recipients")]
        public async Task<IEnumerable<TicketTypeRecipient>> GetTicketTypeRecipients() => await mediator.Send(new GetTicketTypeRecipientRequest());

        [HttpPost("save-ticket-recipients")]
        [ServiceFilter(typeof(DuplicateSubtypeValidator))]
        public async Task SaveTicketTypeRecipients(SaveTicketTypeRecipientsCommand command) => await mediator.Send(command);
    }
}