using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OWP.Report.OvertimeReport.Handlers;
using OWP.Report.OvertimeReport.Models;

namespace OWP.Report.OvertimeReport;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OvertimeReportController : ControllerBase
{
    private readonly IMediator mediator;

    public OvertimeReportController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [Route("get-report-by-month")]
    [HttpPost]
    public async Task<OvertimeByMonth> GetReportByMonth(QueryReportByMonthRequest request)
    {
        return await mediator.Send(request);
    }

    [Route("get-report-by-year")]
    [HttpPost]
    public async Task<IEnumerable<OvertimeByYear>> GetReportByYear(QueryReportByYearRequest request)
    {
        return await mediator.Send(request);
    }

    [Route("get-locations")]
    [HttpGet]
    public async Task<IEnumerable<Location>> GetLocations()
    {
        return await mediator.Send(new GetLocationsRequest());
    }

    [Route("export-report-by-month")]
    [HttpGet]
    public async Task<IActionResult> ExportReportByMonth([FromQuery] ExportReportByMonthRequest request)
    {
        using var excelPackage = await mediator.Send(request);
        var monthYearInString = new DateTime(request.Year, request.Month, 1).ToString("MMM yyyy");
        var fileName = $"Monthly Overtime Report - {monthYearInString}.xlsx";
        var bytes = await excelPackage.GetAsByteArrayAsync();
        return File(bytes, "application/octet-stream", fileName);
    }

    [Route("export-report-by-year")]
    [HttpGet]
    public async Task<IActionResult> ExportReportByYear([FromQuery] ExportReportByYearRequest request)
    {
        using var excelPackage = await mediator.Send(request);
        var fileName = $"Annual Overtime Report - {request.Year}.xlsx";
        var bytes = await excelPackage.GetAsByteArrayAsync();
        return File(bytes, "application/octet-stream", fileName);
    }
}