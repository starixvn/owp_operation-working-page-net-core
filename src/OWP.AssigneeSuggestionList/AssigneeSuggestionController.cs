
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

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
}