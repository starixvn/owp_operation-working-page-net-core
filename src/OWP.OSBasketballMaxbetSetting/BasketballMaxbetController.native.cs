using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OWP.OSBasketballMaxbetSetting.Models;
using OWP.OSBasketballMaxbetSetting.Handlers;

namespace OWP.OSBasketballMaxbetSetting;

[Route("api/[controller]")]
[ApiController]
[EnableCors("AllowOrigin")]
[AllowAnonymous]
//[Authorize]
public class BasketballMaxbetController : ControllerBase
{
    private readonly IMediator mediator;

    public BasketballMaxbetController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("get-status-dropdown-option")]
    public async Task<IList<DropdownOption>> GetStatusDropdownOption()
    => await mediator.Send(new GetStatusDropdownOptionHandler.Request());

    [HttpGet("get-category-dropdown-option")]
    public async Task<IList<DropdownOption>> GetCategoryDropdownOption()
   => await mediator.Send(new GetCategoryDropdownOptionHandler.Request());

    [HttpGet("get-round-dropdown-option")]
    public async Task<IList<DropdownOption>> GetRoundDropdownOption()
   => await mediator.Send(new GetRoundDropdownOptionHandler.Request());

    [HttpGet("league-info")]
    [Produces("application/json")]
    public async Task<IActionResult> GetLeagueInfo(GetLeagueInfoHandler.Request queryInfo)
    {
        var leagueInfo = await mediator.Send(queryInfo);
        return Ok(leagueInfo);
    }
}