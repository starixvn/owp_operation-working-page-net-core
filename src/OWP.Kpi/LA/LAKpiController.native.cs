using Fanex.NAP.Client.Helpers.Web;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OWP.Kpi.LA.Constant;
using OWP.Kpi.LA.Handlers;
using OWP.Kpi.LA.Models;
using OWP.Kpi.LA.ViewModels;
using OWP.Services;


namespace OWP.Kpi.LA;

[Route("api/kpi/la")]
[ApiController]
[Authorize]
[EnableCors]
public class LAKpiController : ControllerBase
{
    private readonly RefResourceService refResourceService;

    private readonly INapSession session;

    private readonly IMediator mediator;

    public LAKpiController(IMediator mediator,RefResourceService refResourceService,INapSession session)
    {
        this.mediator = mediator;
        this.refResourceService = refResourceService;
        this.session = session;
    }

    [Route("")]
    [HttpGet]
    public async Task<DataViewModel> GetLAKpiInfo(DateTime fromDate, DateTime toDate)
    {
        var data = await mediator.Send(new CalculateKpiRequest
        {
            FromDate = fromDate,
            ToDate = toDate
        });

        return data;
    }

    [HttpGet("export")]
    public async Task<IActionResult> DownloadLAKpiInfo(DateTime fromDate, DateTime toDate)
    {
        using var data = await mediator.Send(new ExportKpiRequest
        {
            FromDate = fromDate,
            ToDate = toDate
        });
      
        var bytes = await data.GetAsByteArrayAsync();
        var fileName = GetFileName(fromDate, toDate);
        return File(bytes, "application/octet-stream", fileName);
    }

    [HttpGet("get-condition")]
    public IActionResult GetColorExplain()
        => Ok(ConditionConstant.ConditionInfos);

    [Route("get-level-option")]
    [HttpGet]
    public async Task<IEnumerable<Option>> GetLocations()
    {
        var result = await refResourceService.GetRefResourcesByName("LaKpiLevel");

        return result.Select(x => new Option { Label = x.Value, Value = x.ResourceId });
    }

    [HttpPost("update-target-level")]
    public async Task UpdateTargetLevel(UpdateTargetLevelRequest request)
    {
        await mediator.Send(request);
    }

    private string GetFileName(DateTime fromDate, DateTime toDate)
        => $"LA KPI - {fromDate:MMM yyyy} to {toDate:MMM yyyy} - Export by {session.UserName}.xlsx";
}