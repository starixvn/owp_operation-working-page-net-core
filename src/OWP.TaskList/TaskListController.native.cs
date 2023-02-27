using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OWP.TaskList.Handlers;

namespace OWP.TaskList;

[Route("api/[controller]")]
[ApiController]
[EnableCors("AllowOrigin")]
[Authorize]
public class TaskListController : ControllerBase
{
    private readonly IMediator mediator;

    public TaskListController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost("task-list-table")]
    public async Task<IActionResult> TaskListTable(TaskListTableRequest request)
    {
        var result = await mediator.Send(request);
        return Ok(JsonConvert.SerializeObject(result));
    }

    [HttpPost("update-trade-status")]
    public async Task<string> UpdateReadyTradeStatus(UpdateReadyTradeStatusRequest request)
        => await mediator.Send(request);
}