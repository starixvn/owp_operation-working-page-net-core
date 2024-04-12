

namespace OWP.ProviderVerification;

[Route("api/[controller]")]
[ApiController]
[EnableCors]
[Authorize]
public class ProviderVerificationController : ControllerBase
{
    private readonly IMediator mediator;

    public ProviderVerificationController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("get-provider-verification")]
    public async Task<IActionResult> GetProviderVerification(DateTime eventDate)
    {
        var request = new GetProviderVerificationRequest
        {
            EventDate = eventDate
        };
        var result = await mediator.Send(request);

        return Ok(JsonConvert.SerializeObject(result));
    }

    [HttpGet("get-agent-option")]
    public async Task<IActionResult> GetAgentOption()
    {
        var result = await mediator.Send(new GetAgentOptionsRequest());
        return Ok(result);
    }
}