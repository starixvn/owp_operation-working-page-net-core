using Fanex.Logging;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OWP.Report.LAReport.Handlers;

namespace OWP.Report.LAReport;

[Route("api/report/la")]
[ApiController]
[Authorize]
public class LAReportController : ControllerBase
{
    private readonly IMediator mediator;

    private readonly ILogger logger;

    public LAReportController(IMediator mediator, ILogger logger)
    {
        this.mediator = mediator;
        this.logger = logger;
    }

    [Route("")]
    [HttpPost]
    public async Task<IActionResult> GetDataLAReport(LAReportRequest request)
    {
        var reportModel = await mediator.Send(request);
        return Ok(JsonConvert.SerializeObject(reportModel));
    }

    [HttpGet("export-excel")]

    public async Task<IActionResult> ExportLAExcel(DateTime fromDate,
        DateTime toDate,
        bool activeUser,
        bool resignUser)
    {
        var request = new LAReportRequest
        {
            FromDate = fromDate,
            ToDate = toDate,
            ActiveUser = activeUser,
            ResignUser = resignUser
        };
        await logger.InfoAsync(request.ToString()).ConfigureAwait(false);
        var reportModels = await mediator.Send(request);

        var exportRequest = new LAExcelRequest
        {
            FromDate = fromDate.ToString("MM/dd/yyyy"),
            ToDate = toDate.ToString("MM/dd/yyyy"),
            ReportModels = reportModels
        };

        using var excelPackage = await mediator.Send(exportRequest);
        var fileName = $"LA Report {fromDate:ddMMMMyyyy}-{toDate:ddMMMMyyyy}.xlsx";
        var bytes = await excelPackage.GetAsByteArrayAsync();
        return File(bytes, "application/octet-stream", fileName);
    }
}