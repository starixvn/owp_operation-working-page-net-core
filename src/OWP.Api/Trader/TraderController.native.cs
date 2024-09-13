using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OWP.Api.Trader.Handlers;

namespace OWP.Api.Trader;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class TraderController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Produces("application/json")]
    public async Task<IActionResult> GetMaxbet(List<MatchRequest> matches)
    {
        var result = await mediator.Send(new TraderRequest { matches = matches });

        return Ok(result);
    }
}
