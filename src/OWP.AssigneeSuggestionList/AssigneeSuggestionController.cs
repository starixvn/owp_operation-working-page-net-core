
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OWP.AssigneeSuggestionList.Handlers;

namespace OWP.AssigneeSuggestionList;

[Route("api/[controller]")]
[ApiController]
[EnableCors("AllowOrigin")]
public class AssigneeSuggestionController : ControllerBase
{
    private readonly IMediator mediator;

    public AssigneeSuggestionController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("get-trader-suggestion")]
    public async Task<IActionResult> GetTraderSuggestion([FromQuery] int owpMatchId, [FromQuery] string betType)
    {
        return Ok("Suggestion List");
    }

    [HttpGet("get-match-info")]
    public async Task<IActionResult> GetMatchInfo([FromQuery] int owpMatchId, [FromQuery] string betType)
    {
        var result = await mediator.Send(new QueryMatchInfoRequest
        {
            OwpMatchId = owpMatchId,
            BetType = betType
        });

        return Ok(result);
    }
}