using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OWP.AccountSetting.Profile.Handler;

namespace OWP.AccountSetting.Profile;
[Route("api/AccountSetting/[controller]")]
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

    [HttpGet("get-user-setting-server")]
    public async Task<IActionResult> GetUserSettingServer()
    {
        var request = new GetUserSettingByKeyRequest { Key = "AdminServerSetting" };
        var result = await mediator.Send(request);
        return Ok(result);
    }

    [HttpPost("update-user-setting-server")]
    public async Task<IActionResult> UpdateUserSettingServer(UpdateUserSettingRequest request)
    {
        request.Key = "AdminServerSetting";
        await mediator.Send(request);
        return Ok();
    }

}

