using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OWP.Report.TraderReport.Handlers;

namespace OWP.Report.TraderReport;

[Route("api/report")]
[ApiController]
[AllowAnonymous]
public class TraderReportController : ControllerBase
{
    private readonly IMediator mediator;

    public TraderReportController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [Route("get-server-time")]
    [HttpGet]
    [Produces("application/json")]
    public IActionResult GetServerTime()
    {
        var serverTime = DateTime.Today.AddDays(-1);
        return Ok(serverTime.ToString("MM/dd/yyyy"));
    }

    [Route("lst")]
    [HttpPost]
    public async Task<IActionResult> GetDataLstReport(ReportRequest request)
    {
        var reportModel = await mediator.Send(request);
        return Ok(JsonConvert.SerializeObject(reportModel));
    }

    [Route("lst/export-excel")]
    [HttpGet]
    public async Task<IActionResult> ExportExcel(string fromDate, string toDate)
    {
        var reportRequest = new ReportRequest
        {
            FromDate = DateTime.Parse(fromDate),
            ToDate = DateTime.Parse(toDate)
        };
        var reportModels = await mediator.Send(reportRequest);

        var request = new ExcelRequest
        {
            FromDate = fromDate,
            ToDate = toDate,
            ReportModels = reportModels
        };

        using var excelPackage = await mediator.Send(request);
        var bytes = await excelPackage.GetAsByteArrayAsync();
        return File(bytes, "application/octet-stream", "Trader Report.xlsx");
    }
}
