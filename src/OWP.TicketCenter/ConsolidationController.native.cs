using MediatR;
using Microsoft.AspNetCore.Mvc;
using OWP.TicketCenter.Handlers;

namespace OWP.TicketCenter;

[Route("api/[controller]")]
[ApiController]
public class ConsolidationController : ControllerBase
{
    private readonly IMediator mediator;


    public ConsolidationController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("get-subtype")]
    public async Task GetSubType()
        => await mediator.Send(new ConsolidationGetSubtypeRequest());
}