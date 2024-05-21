using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OWP.AccountSetting.Profile;
using OWP.AccountSetting.Profile.Model;

namespace OWP.AccountSetting;

[Route("api/[controller]")]
[ApiController]
[EnableCors]
[Authorize]

public class AccountSettingController : ControllerBase
{
    private readonly IMediator mediator;

    public AccountSettingController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("get-user-profile")]
    public async Task<string> GetProvider()
        => await mediator.Send(new GetProfileRequest());

    [HttpGet("get-quick-message")]
    public async Task<List<QuickMessage>> GetQuickMessage()
        => await mediator.Send(new GetQuickMessageRequest());
}

