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
    public async Task<IActionResult> GetMaxbet(Request request)
    {
        var result = await mediator.Send(new TraderRequest
        {
            Matches = request.Matches,
            FromDate = request.FromDate,
            ToDate = request.ToDate
        });

        return Ok(result);
    }
}
