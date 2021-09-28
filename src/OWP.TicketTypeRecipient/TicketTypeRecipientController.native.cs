namespace OWP.TicketTypeRecipient
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using MediatR;
    using Models;
    using OWP.TicketTypeRecipient.Handlers;

    [Route("api/[controller]")]
    [ApiController]
    public class TicketTypeRecipientController : ControllerBase
    {
        private readonly IMediator mediator;

        public TicketTypeRecipientController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Route("get-ticket-recipients")]
        [HttpGet]
        public async Task<IEnumerable<TicketTypeRecipient>> GetTicketTypeRecipients()
        {
            return await mediator.Send(new GetTicketTypeRecipientRequest());
        }

        [Route("save-ticket-recipients")]
        [HttpPost]
        public async Task SaveTicketTypeRecipients(SaveTicketTypeRecipientsCommand command)
        {
            await mediator.Send(command);
        }
    }
}