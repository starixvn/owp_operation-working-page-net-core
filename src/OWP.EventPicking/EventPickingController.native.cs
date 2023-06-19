using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OWP.EventPicking.Handlers;
using OWP.EventPicking.Middleware;

namespace OWP.EventPicking;

[Route("api/[controller]")]
[ApiController]
[EnableCors("AllowOrigin")]
public class EventPickingController
{
    private readonly IMediator mediator;

    public EventPickingController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost("get-event-picking")]
    [ServiceFilter(typeof(GetEventValidator))]
    public async Task<IActionResult> QueryEventPicking(QueryEventPickingRequest request)
    {
        var result = await mediator.Send(request);

        return new JsonResult(result);
    }

    [HttpPost("update-noOffer-match")]
    public async Task UpdateMaxbetSetting(InsertOrDeleteNoOfferMatchRequest request)
        => await mediator.Send(request);

    [Route("export-misc-match")]
    [HttpGet]
    public async Task<FileResult> ExportMiscMatch([FromQuery] ExportMiscMatchRequest request)
        => await mediator.Send(request);
}