using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OWP.EventPicking.Handlers;
using OWP.EventPicking.Middleware;
using OWP.EventPicking.Models;

namespace OWP.EventPicking;

[Route("api/[controller]")]
[ApiController]
[EnableCors]
public class EventPickingController
{
    private readonly IMediator mediator;
    private readonly Repository repository;

    public EventPickingController(IMediator mediator, Repository repository)
    {
        this.mediator = mediator;
        this.repository = repository;
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

    [Route("get-logs-history")]
    [HttpGet]
    public async Task<IList<LogModel>> GetLogHistory([FromQuery] GetLogRequest request)
        => await mediator.Send(request);

    [HttpPost("update-confirmed")]
    public async Task UpdateConfirmed(UpdateConfirmedRequest request)
        => await mediator.Send(request);

    [Route("get-update-type")]
    [HttpGet]
    public async Task<IActionResult> QueryUpdateType()
    {
        var result = await repository.GetUpdateType();

        return new JsonResult(result);
    }

    [Route("get-league-name")]
    [HttpGet]
    public async Task<IActionResult> QueryLeagueName(DateTime date)
    {
        var result = await repository.GetLeagueName(date);

        return new JsonResult(result);
    }

    [Route("export-excel")]
    [HttpPost]
    public async Task<FileResult> ExportExcel(ExportExcelRequest request)
    => await mediator.Send(request);

    [Route("export-excel-lst-format")]
    [HttpPost]
    public async Task<FileResult> ExportLSTFormat(ExportLSTFormatRequest request)
        => await mediator.Send(request);

}