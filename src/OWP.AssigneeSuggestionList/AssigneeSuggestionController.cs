using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Handlers;

namespace OWP.AssigneeSuggestionList;

[Route("api/[controller]")]
[ApiController]
[EnableCors("AllowOrigin")]
[AllowAnonymous]
public class AssigneeSuggestionController : ControllerBase
{
    private readonly IMediator mediator;

    public AssigneeSuggestionController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("get-trader-suggestion")]
    public async Task<IActionResult> GetTraderSuggestion([FromQuery] QueryTraderSuggestionRequest request)
    {
        var result = await mediator.Send(request);

        return Ok(result);
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

    [HttpGet("get-color-explain")]
    public IActionResult GetColorExplain()
    => Ok(ColorCodeConst.ColorExplain);
}