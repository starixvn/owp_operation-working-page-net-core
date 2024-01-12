using Fanex.Logging;
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

    private readonly ILogger logger;

    public LAKpiController(IMediator mediator, RefResourceService refResourceService, INapSession session, ILogger logger)
    {
        this.mediator = mediator;
        this.refResourceService = refResourceService;
        this.session = session;
        this.logger = logger;
    }

    [Route("")]
    [HttpGet]
    public async Task<DataViewModel> GetLAKpiInfo([FromQuery] CalculateKpiRequest request)
        => await mediator.Send(request);

    [HttpGet("export")]

    public async Task<IActionResult> DownloadLAKpiInfo(
        DateTime fromDate,
        DateTime toDate,
        string sectionIds,
        bool activeUser,
        bool resignUser)
    {
        var request = new ExportKpiRequest
        {
            FromDate = fromDate,
            ToDate = toDate,
            SectionIds = sectionIds,
            ActiveUser = activeUser,
            ResignUser = resignUser
        };

        await logger.InfoAsync(request.ToString()).ConfigureAwait(false);
        using var data = await mediator.Send(request);

        var bytes = await data.GetAsByteArrayAsync();
        var fileName = GetFileName(request.FromDate, request.ToDate);
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

    [HttpGet("get-all-section")]
    public async Task<IEnumerable<SectionInfo>> UpdateTargetLevel(int userId)
    {
        var data = await mediator.Send(new GetAllSectionRequest { UserId = userId });
        return data;
    }

    [Route("get-log-target-level")]
    [HttpGet]
    public async Task<IEnumerable<LogDisplayModel>> GetLogTargetLevel(int userId)
    {
        var result = await mediator.Send(new GetLogRequest { UserId = userId });
        return result;
    }

    private string GetFileName(DateTime fromDate, DateTime toDate)
        => $"LA KPI - {fromDate:MMM yyyy} to {toDate:MMM yyyy} - Export by {session.UserName}.xlsx";
}