using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OWP.Extensions;
using OWP.OSLeagueSetting.Handlers;
using OWP.OSLeagueSetting.Middleware;
using OWP.OSLeagueSetting.Models;
using OWP.OSLeagueSetting.Services;

namespace OWP.OSLeagueSetting;

[Route("api/[controller]")]
[ApiController]
[EnableCors]
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

    [HttpGet("export-league-setting")]
    public async Task<IActionResult> Export([FromQuery] ExportLeagueSettingRequest exportRequest)
    {
        var data = await mediator.Send(exportRequest);
        var bytes = await data.GetAsByteArrayAsync();
        var fileName = GetFileName();
        return File(bytes, "application/octet-stream", fileName);
    }

    [TypeFilter(typeof(DuplicateActionFilter))]
    [HttpPost("add-setting")]
    public async Task AddSetting(SettingDetail model)
     => await addService.Execute(model);

    [TypeFilter(typeof(DuplicateActionFilter))]
    [HttpPost("edit-setting")]
    public async Task EditSetting(SettingDetail model)
     => await editService.Execute(model);

    [HttpGet("get-log-setting")]
    public async Task<IActionResult> GetLogSetting(int leagueSettingId)
    {
        var request = new GetLogSettingRequest { LeagueSettingId = leagueSettingId };
        var result = await mediator.Send(request);
        return Ok(result);
    }

    private string GetFileName()
        => $"Basketball Maxbet(Live) {DateTime.Now.ToGmt8():dd MMM yyyy}.xlsx";
}