using MediatR;
using Microsoft.AspNetCore.Mvc;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter;

[Route("api/[controller]")]
[ApiController]
public class ConsolidationController(IMediator mediator) : ControllerBase
{
    
    [HttpGet("get-subtype")]
    public async Task<IList<ConsolidationSubtypeViewModel>> GetSubType()
        => await mediator.Send(new ConsolidationGetSubtypeRequest());
}