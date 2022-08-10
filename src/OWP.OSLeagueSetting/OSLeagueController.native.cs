using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OWP.OSLeagueSetting.Handlers;
using OWP.OSLeagueSetting.Models;
using OWP.OSLeagueSetting.Services;
namespace OWP.OSLeagueSetting;

[Route("api/[controller]")]
[ApiController]
[EnableCors("AllowOrigin")]
[AllowAnonymous]
public class OSLeagueController : ControllerBase
{
    private readonly IMediator mediator;

    private readonly AddSettingService addService;

    private readonly EditSettingService editService;

    public OSLeagueController(
        IMediator mediator,
        AddSettingService addService,
        EditSettingService editService)
    {
        this.mediator = mediator;
        this.addService = addService;
        this.editService = editService;
    }

    [HttpPost("league-setting")]
    public async Task<IActionResult> GetLeagueInfo(GetLeagueInfoHandler.Request queryInfo)
    {
        var leagueInfo = await mediator.Send(queryInfo);
        return Ok(leagueInfo);
    }

    [HttpPost("add-setting")]
    public async Task AddSetting(AddSetting model)
   => await addService.Execute(model);

    [HttpPost("edit-setting")]
    public async Task EditSetting(EditSetting model)
    => await editService.Execute(model);

    [HttpGet("get-log-setting")]
    public async Task<IActionResult> GetLogSetting(int leagueSettingId)
    {
        var request = new GetLogSettingRequest { LeagueSettingId = leagueSettingId };
        var result = await mediator.Send(request);
        return Ok(result);
    }
}