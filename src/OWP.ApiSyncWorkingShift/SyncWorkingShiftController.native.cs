using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OWP.ApiSyncWorkingShift.Handlers;
using OWP.ApiSyncWorkingShift.Models;

namespace OWP.ApiSyncWorkingShift;

[Route("api/")]
[ApiController]
[AllowAnonymous]
public class SyncWorkingShiftController : ControllerBase
{
    private readonly IMediator mediator;

    public SyncWorkingShiftController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("SyncWorkingShift/{from:datetime}/{to:datetime}")]
    [Produces("application/json")]
    public async Task<IEnumerable<WorkingShiftModel>> GetWorkingShiftToHRM(DateTime from, DateTime to)
        => await mediator.Send(new QueryWorkingShiftRequest
        {
            From = from,
            To = to
        });

    [HttpGet("SyncWorkingShiftToHRPP/{from:datetime}/{to:datetime}")]
    [Produces("application/json")]
    public async Task<IEnumerable<WorkingShiftHRPPModel>> GetWorkingShiftToHRPP(DateTime from, DateTime to)
        => await mediator.Send(new QueryWorkingShiftToHRPPRequest
        {
            From = from,
            To = to
        });
}