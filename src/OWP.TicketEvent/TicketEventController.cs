using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OWP.TicketEvent.Handlers;

namespace OWP.TicketEvent;

[Route("api/[controller]")]
[ApiController]
[EnableCors("AllowOrigin")]
[Authorize]
public class TicketEventController
{
    private readonly IMediator mediator;

    public TicketEventController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("get-ticket-list")]
    public async Task<IActionResult> QueryMaxbetSetting(int OWPMatchId)
    {
        var result = await mediator.Send(new GetTicketListRequest
        {
            OWPMatchId = OWPMatchId
        });

        return new JsonResult(result);
    }
}
