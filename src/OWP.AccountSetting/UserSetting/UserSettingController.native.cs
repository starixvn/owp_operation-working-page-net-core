using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OWP.AccountSetting.Profile.Handler;
using OWP.AccountSetting.UserSetting.Handler;
using OWP.Constants;

namespace OWP.AccountSetting.Profile;
[Route("api/AccountSetting/[controller]")]
[ApiController]
[Authorize]

public class UserSettingController : ControllerBase
{
    private readonly IMediator mediator;

    public UserSettingController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("get-user-setting-server")]
    public async Task<IActionResult> GetUserSettingServer()
    {
        var request = new GetUserSettingByKeyRequest { Key = UserSettingKey.AdminServerSetting};
        var result = await mediator.Send(request);
        return Ok(result);
    }

    [HttpPost("update-user-setting-server")]
    public async Task<IActionResult> UpdateUserSettingServer(UpdateUserSettingRequest request)
    {
        request.Key = UserSettingKey.AdminServerSetting;
        await mediator.Send(request);
        return Ok();
    }

}

