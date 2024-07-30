using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OWP.Event.TaskList.Handlers;

namespace OWP.Event.TaskList;

[Route("api/[controller]")]
[ApiController]
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