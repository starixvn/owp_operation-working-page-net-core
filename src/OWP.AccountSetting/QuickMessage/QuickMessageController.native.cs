using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OWP.AccountSetting.QuickMessage.Handler;

namespace OWP.AccountSetting.QuickMessage;

using OWP.AccountSetting.QuickMessage.Model;

[Route("api/AccountSetting/[controller]")]
[ApiController]
[Authorize]

public class QuickMessageController : ControllerBase
{
    private readonly IMediator mediator;

    public QuickMessageController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("get-quick-message")]
    public async Task<List<QuickMessage>> GetQuickMessage()
        => await mediator.Send(new GetQuickMessageRequest());

    [HttpPost("save-quick-message")]
    public async Task<IActionResult> SaveQuickMessage(SaveQuickMessageRequest request)
    {
        await mediator.Send(request);

        return Ok();
    }

    [HttpPost("delete-quick-message")]
    public async Task<IActionResult> DeleteQuickMessage(DeleteQuickMessageRequest request)
    {
        await mediator.Send(request);

        return Ok();
    }
}

