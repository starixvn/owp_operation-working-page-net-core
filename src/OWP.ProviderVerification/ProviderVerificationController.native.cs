

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

    [HttpPost("save-agent-confirm")]
    public async Task<IActionResult> SaveAgentOption(SaveAgentConfirmRequest request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }

    [HttpPost("save-est-note")]
    public async Task<IActionResult> SaveEstNote(SaveEstNoteRequest request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }
}