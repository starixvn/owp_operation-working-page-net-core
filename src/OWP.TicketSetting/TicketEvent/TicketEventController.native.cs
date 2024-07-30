using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OWP.TicketSetting.TicketEvent.Handlers;

namespace OWP.TicketSetting.TicketEvent;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TicketEventController : ControllerBase
{
    private readonly IMediator mediator;

    public TicketEventController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("get-ticket-list")]
    public async Task<IActionResult> QueryMaxBetSetting(int owpMatchId, int actorId)
    {
        var result = await mediator.Send(new GetTicketListRequest
        {
            OwpMatchId = owpMatchId,
            ActorId = actorId
        });

        return Ok(JsonConvert.SerializeObject(result));
    }

    [HttpGet("get-assignees-lga")]
    public async Task<IActionResult> GetAssigneesLga(int owpMatchId)
    {
        var result = await mediator.Send(new QueryAssigneesLgaRequest
        {
            OwpMatchId = owpMatchId
        });

        return Ok(result);
    }
}