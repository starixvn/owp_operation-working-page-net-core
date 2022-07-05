using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OWP.OSLeagueSetting.Models;
using OWP.OSLeagueSetting.Handlers;

namespace OWP.OSLeagueSetting;

[Route("api/[controller]")]
[ApiController]
[EnableCors("AllowOrigin")]
[AllowAnonymous]
public class OSLeagueController : ControllerBase
{
    private readonly IMediator mediator;

    public OSLeagueController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost("league-setting")]
    public async Task<IActionResult> GetLeagueInfo(GetLeagueInfoHandler.Request queryInfo)
    {
        var leagueInfo = await mediator.Send(queryInfo);
        return Ok(leagueInfo);
    }
}