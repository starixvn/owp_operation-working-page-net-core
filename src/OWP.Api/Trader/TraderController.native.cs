using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OWP.Api.Trader.Handlers;
using OWP.Api.Trader.Model;

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
        var result = await mediator.Send(new TraderRequest { Matches = matches });

        return Ok(result);
    }

    [HttpPost("time-range")]
    [Produces("application/json")]
    public async Task<IActionResult> GetTraderByTimeRange(Request request)
    {
        var result = await mediator.Send(new TraderRequestByTimeRange
        {
            Matches = request.Matches,
            FromDate = request.FromDate,
            ToDate = request.ToDate
        });

        return Ok(result);
    }
}
