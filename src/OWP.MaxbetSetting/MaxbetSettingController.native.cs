using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OWP.MaxbetSetting.Handlers;
using OWP.Services;

namespace OWP.MaxbetSetting;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MaxbetSettingController
{
    private readonly MaxbetSettingService maxbetSettingService;

    private readonly IMediator mediator;

    public MaxbetSettingController(MaxbetSettingService maxbetSettingService, IMediator mediator)
    {
        this.maxbetSettingService = maxbetSettingService;
        this.mediator = mediator;
    }

    [HttpGet("get-maxbet-setting")]
    public async Task<IActionResult> QueryMaxbetSetting()
    {
        var result = await maxbetSettingService.QueryMaxbetSetting();

        return new JsonResult(result);
    }
    [HttpGet("get-log")]
    public async Task<IActionResult> GetLogMaxbetSetting(int FT_HDP)
    {
        var request = new GetLogMaxbetSettingRequest
        {
            FT_HDP = FT_HDP
        };
        var result = await mediator.Send(request);

        return new JsonResult(result);
    }

    [Route("update-maxbet-setting")]
    [HttpPost]
    public async Task UpdateMaxbetSetting(UpdateMaxbetSettingRequest request)
    {
        await mediator.Send(request);
    }
}
