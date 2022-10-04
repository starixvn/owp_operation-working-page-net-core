namespace OWP.TicketEvent;

using Handlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> QueryMaxBetSetting(int owpMatchId)
    {
        var result = await mediator.Send(new GetTicketListRequest
        {
            OwpMatchId = owpMatchId
        });

        return new JsonResult(result);
    }

    [HttpGet("get-assignees-lga")]
    public async Task<IActionResult> GetAssigneesLga(int owpMatchId)
    {
        var result = await mediator.Send(new QueryAssigneesLgaRequest
        {
            OwpMatchId = owpMatchId
        });

        return new JsonResult(result);
    }
}