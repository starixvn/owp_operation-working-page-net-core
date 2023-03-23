
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace OWP.TodaySuggestionList;

[Route("api/[controller]")]
[ApiController]
[EnableCors("AllowOrigin")]
public class TodaySuggestionController : ControllerBase
{
    private readonly IMediator mediator;

    public TodaySuggestionController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("get-trader-suggestion")]
    public async Task<IActionResult> GetTraderSuggestion([FromQuery] int owpMatchId, [FromQuery] string betType)
    {
        return Ok("Suggestion List");
    }
}