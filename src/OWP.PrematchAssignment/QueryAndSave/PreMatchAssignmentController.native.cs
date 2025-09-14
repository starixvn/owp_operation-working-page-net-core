using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OWP.Constants;
using OWP.PrematchAssignment.AutoAssignment.Handlers;
using OWP.PrematchAssignment.QueryAndSave.Handlers;
using OWP.PrematchAssignment.QueryAndSave.Models;
using OWP.PrematchAssignment.QueryAndSave.Services;
using OWP.Services;

namespace OWP.PrematchAssignment.QueryAndSave;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PreMatchAssignmentController(
    IMediator mediator, Repository repository, AccessCounter accessCounter) : ControllerBase
{
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
        await mediator.Send(new CacheLeagueNameRequest(request.LeaguesName, request.Date,
            !request.StartTime.HasValue));
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
    public async Task<IActionResult> QueryAssignedLeagueId([FromQuery] QueryAssignedLeagueIdRequest request)
    {
        var result = await mediator.Send(request);

        return Ok(JsonConvert.SerializeObject(result));
    }

    [Route("query-league-by-id")]
    [HttpPost]
    public async Task<IActionResult> QueryById(QueryPreMatchLeagueByIdRequest request)
    {
        var result = await mediator.Send(request);

        return Ok(JsonConvert.SerializeObject(result));
    }

    [HttpGet("get-ticket-list")]
    public async Task<IActionResult> QueryTicketList(int id, int actorId, bool isTicketLeague)
    {
        List<TicketBasicInfo> tickets;
        if (isTicketLeague)
        {
            var leagueTicketsTask = await repository.QueryAllLeaguesTicket(new List<int> { id }, actorId);
            tickets = [.. leagueTicketsTask.SelectMany(x => x.Value).ToList()];
        }
        else
        {
            var matchTicketsTask = await repository.QueryAllMatchesTicket(new List<int> { id }, actorId);
            tickets = [.. matchTicketsTask.SelectMany(x => x.Value).ToList()];
        }

        return Ok(JsonConvert.SerializeObject(tickets.OrderByDescending(x => x.LatestCreatedDate).ToList()));
    }

    [Route("auto-assign")]
    [HttpPost]
    public async Task<IActionResult> AutoAssign(AutoAssignRequest request)
    {
        var assignments = await mediator.Send(request);
        return Ok(JsonConvert.SerializeObject(assignments));
    }

    [Route("submit-auto-assign")]
    [HttpPost]
    public async Task<IActionResult> SubmitAutoAssign(SubmitAutoAssignRequest request)
    {
        await mediator.Send(request);
        await mediator.Send(new CacheLeagueNameRequest(request.LeaguesName, request.Date, true));
        await accessCounter.UpdateCounter(AccessCounterConst.DayShiftAssignment_Automatically_Assign_Trader);
        return Ok();
    }
}