using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OWP.AccountSetting.Profile.Handler;

namespace OWP.AccountSetting.Profile;
[Route("api/AccountSetting")]
[ApiController]
[Authorize]

public class ProfileController : ControllerBase
{
    private readonly IMediator mediator;

    public ProfileController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("get-user-profile")]
    public async Task<string> GetProvider()
        => await mediator.Send(new GetProfileRequest());

    [HttpPost("save-user-profile")]
    public async Task<IActionResult> SaveUserProfile(SaveProfileRequest request)
    {
        await mediator.Send(request);

        return Ok();
    }
}

