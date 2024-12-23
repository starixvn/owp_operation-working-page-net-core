using MediatR;


namespace OWP.WorkingShift.SyncWorkingShift;

[Route("api/")]
[ApiController]
[AllowAnonymous]
public class SyncWorkingShiftController(IMediator mediator) : ControllerBase
{
    [HttpGet("SyncWorkingShift/{from:datetime}/{to:datetime}")]
    [Produces("application/json")]
    public async Task<IEnumerable<Models.WorkingShiftModel>> GetWorkingShiftToHRM(DateTime from, DateTime to)
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

    [HttpGet("HRM/WorkingShift")]
    [Produces("application/json")]
    public async Task<IEnumerable<SyncWorkingShiftModel>> SyncWorkingShift([FromQuery] QueryEmployeeWorkingShiftRequest model)
        => await mediator.Send(model);
}