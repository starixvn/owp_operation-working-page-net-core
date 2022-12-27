using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OWP.Report.LAReport.Handlers;

namespace OWP.Report;

[Route("api/report")]
[ApiController]
[Authorize]
[EnableCors("AllowOrigin")]
public class LAReportController : ControllerBase
{
    private readonly IMediator mediator;

    public LAReportController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [Route("la")]
    [HttpPost]
    public async Task<IActionResult> GetDataLAReport(LAReportRequest request)
    {
        var reportModel = await mediator.Send(request);
        return Ok(JsonConvert.SerializeObject(reportModel));
    }

    [Route("la/export-excel")]
    [HttpGet]
    public async Task<IActionResult> ExportLAExcel(string fromDate, string toDate)
    {
        var reportRequest = new LAReportRequest
        {
            FromDate = DateTime.Parse(fromDate),
            ToDate = DateTime.Parse(toDate)
        };
        var reportModels = await mediator.Send(reportRequest);

        var request = new LAExcelRequest
        {
            FromDate = fromDate,
            ToDate = toDate,
            ReportModels = reportModels
        };

        using var excelPackage = await mediator.Send(request);
        var fileName = $"LA Report {reportRequest.FromDate.ToString("ddMMMMyyyy")}-{reportRequest.ToDate.ToString("ddMMMMyyyy")}.xlsx";
        var bytes = await excelPackage.GetAsByteArrayAsync();
        return File(bytes, "application/octet-stream", fileName);
    }
}