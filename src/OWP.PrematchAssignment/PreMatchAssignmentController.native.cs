namespace OWP.PreMatchAssignment;

using Handlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PrematchAssignment.Handlers;
using PrematchAssignment.Models;
using PreMatchAssignment.Models;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[EnableCors("AllowOrigin")]
public class PreMatchAssignmentController : ControllerBase
{
    private readonly IMediator mediator;

    public PreMatchAssignmentController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [Route("get-assignee-pre-match")]
    [HttpPost]
    public async Task<IEnumerable<TicketAssignee>> GetAssigneesPreMatch(QueryAssigneesRequest request)
    {
        return await mediator.Send(request);
    }

    [Route("get-info-match")]
    [HttpPost]
    public async Task<IActionResult> GetInfoMatchById(QueryInfoMatchRequest request)
    {
        var sender = await mediator.Send(request);
        return Ok(JsonConvert.SerializeObject(sender));
    }

    [Route("get-working-shift")]
    [HttpGet]
    public async Task<IActionResult> GetWorkingShift(DateTime date)
    {
        var result = await mediator.Send(new QueryWorkingShiftRequest { Date = date });

        return Ok(JsonConvert.SerializeObject(result));
    }

    [Route("save")]
    [HttpPost]
    public async Task<IActionResult> SavePrematchAssignment(UpdateLeagueAssignmentRequest request)
    {
        await mediator.Send(request);
        return Ok();
    }

    [Route("get-log")]
    [HttpGet]
    public async Task<IActionResult> QueryLog(int pageSize, int pageNumber, bool isDay)
    {
        var request = new QueryLogRequest { PageSize = pageSize, PageNumber = pageNumber, IsDay = isDay };
        var result = await mediator.Send(request);

        return Ok(JsonConvert.SerializeObject(result));
    }

    [Route("get-day-shift")]
    [HttpPost]
    public async Task<IActionResult> QueryDayShift(QueryDayShiftPreMatchRequest request)
    {
        var result = await mediator.Send(request);

        return Ok(JsonConvert.SerializeObject(result));
    }

    [Route("get-night-shift")]
    [HttpPost]
    public async Task<IActionResult> QueryNightShift(QueryNightShiftPreMatchRequest request)
    {
        var result = await mediator.Send(request);

        return Ok(JsonConvert.SerializeObject(result));
    }

    [Route("post-note-message")]
    [HttpPost]
    public async Task<IActionResult> PostNoteMessage(PostNewNoteMessageRequest request)
    {
        var result = await mediator.Send(request);
        return Ok(JsonConvert.SerializeObject(result));
    }

    [Route("create-note")]
    [HttpPost]
    public async Task<IActionResult> CreateNewNote(CreateNewNoteRequest request)
    {
        var result = await mediator.Send(request);
        return Ok(JsonConvert.SerializeObject(result));
    }

    [Route("edit-note-message")]
    [HttpPost]
    public async Task<IActionResult> EditNoteMessage(EditNoteMessageRequest request)
    {
        var result = await mediator.Send(request);
        return Ok(JsonConvert.SerializeObject(result));
    }

    [Route("get-assigned-league-id")]
    [HttpGet]
    public async Task<IActionResult> QueryAssignedLeagueId([FromQuery]QueryAssignedLeagueIdRequest request)
    {
        var result = await mediator.Send(request);

        return Ok(JsonConvert.SerializeObject(result));
    }
}