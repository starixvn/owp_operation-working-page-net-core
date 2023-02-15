namespace OWP.TicketTypeRecipient;

using Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Middleware;
using Models;

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