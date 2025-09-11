using OWP.EventPicking.Api.Handlers;
using OWP.EventPicking.Api.Middleware;
using OWP.EventPicking.Api.Models;

namespace OWP.EventPicking.Api;

[Route("api/EventPicking")]
[ApiController]
public class EventPickingApiController(IMediator mediator, Repository repository)
{
    [HttpPost("get-event-picking")]
    [ServiceFilter(typeof(GetEventValidator))]
    public async Task<IActionResult> QueryEventPicking(QueryEventPickingRequest request)
    {
        var result = await mediator.Send(request);

        return new JsonResult(result);
    }

    [HttpPost("query-event-picking-edit-mode")]

    public async Task<IActionResult> QueryEventPickingEditMode(EditEventPickingRequest request)
    {
        var result = await mediator.Send(request);

        return new JsonResult(result);
    }

    [HttpPost("update-noOffer-match")]
    public async Task UpdateMaxbetSetting(InsertOrUpdateNoOfferMatchRequest request)
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

    [Route("export-excel")]
    [HttpPost]
    public async Task<FileResult> ExportExcel(ExportExcelRequest request)
        => await mediator.Send(request);

    [HttpPost("update-main-events")]
    public async Task UpdateMainEventMaxbetSpread(UpdateEventsRequest request)
        => await mediator.Send(request);

    [HttpPost("update-special-product")]
    public async Task UpdateSpecialProductMaxbetSpread(SpecialProductUpdateMaxbetSpreadRequest request)
        => await mediator.Send(request);

    [HttpPost("remove-special-product")]
    public async Task RemoveSpecialProduct(RemoveSpecialProductRequest request)
   => await mediator.Send(request);

    [HttpPost("add-special-product")]
    public async Task AddSpecialProduct(AddSpecialProductRequest request)
        => await mediator.Send(request);
}