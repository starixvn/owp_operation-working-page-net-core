using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OWP.TicketSetting.RecipientGroup.Handlers;
using OWP.TicketSetting.RecipientGroup.Models;

namespace OWP.TicketSetting.RecipientGroup;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RecipientGroupController : ControllerBase
{
    private readonly IMediator mediator;

    public RecipientGroupController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [Route("get-recipient-group")]
    [HttpGet]
    public async Task<IEnumerable<TicketGroup>> GetRecipientGroup()
    {
        return await mediator.Send(new QueryRecipientGroupRequest());
    }

    [Route("save-recipient-group")]
    [HttpPost]
    public async Task<IActionResult> SaveRecipientGroup(SaveRecipientGroupRequest model)
    {
        await mediator.Send(model);
        return Ok();
    }
}