using MediatR;
using Microsoft.AspNetCore.Mvc;
using OWP.TicketSetting.TicketTypeRecipient.Handlers;
using OWP.TicketSetting.TicketTypeRecipient.Middleware;

namespace OWP.TicketSetting.TicketTypeRecipient;

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
    public async Task<IEnumerable<Models.TicketTypeRecipient>> GetTicketTypeRecipients() => await mediator.Send(new GetTicketTypeRecipientRequest());

    [HttpPost("save-ticket-recipients")]
    [ServiceFilter(typeof(DuplicateSubtypeValidator))]
    public async Task SaveTicketTypeRecipients(SaveTicketTypeRecipientsCommand command) => await mediator.Send(command);


    [HttpPost("get-recipients")]
    public async Task<object> GetRecipient(GetRecipientRequest command) => await mediator.Send(command);
}