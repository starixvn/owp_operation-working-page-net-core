using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OWP.TeamManagement.Handlers;

namespace OWP.TeamManagement;

[Route("api/team-management")]
[Authorize]
[ApiController]
public class TeamApiController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Route("get-info")]
    public async Task<IActionResult> GetTeamInfo([FromQuery] GetTeamInfoRequest request)
    {
        var result = await mediator.Send(request);
        return Ok(JsonConvert.SerializeObject(result));
    }
}