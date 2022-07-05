namespace OWP.Report
{
    using LAReport.Handlers;
    using LAReport.Models;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TraderReport.Handlers;
    using TraderReport.Models;

    [Route("api/report")]
    [ApiController]
    //[AllowAnonymous]
    [Authorize]
    [EnableCors("AllowOrigin")]
    public class ReportApiController : ControllerBase
    {
        private readonly IMediator mediator;

        public ReportApiController(IMediator mediator)
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
        public async Task<IEnumerable<ReportModel>> GetDataLstReport(ReportRequest request)
        {
            return await mediator.Send(request);
        }

        [Route("la")]
        [HttpPost]
        public async Task<IEnumerable<LAReportModel>> GetDataLAReport(LAReportRequest request)
        {
            return await mediator.Send(request);
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

            var excelPackage = await mediator.Send(request);
            var bytes = await excelPackage.GetAsByteArrayAsync();
            return File(bytes, "application/octet-stream", "Trader Report.xlsx");
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

            var excelPackage = await mediator.Send(request);
            var fileName = $"LA Report {reportRequest.FromDate.ToString("ddMMMMyyyy")}-{reportRequest.ToDate.ToString("ddMMMMyyyy")}.xlsx";
            var bytes = await excelPackage.GetAsByteArrayAsync();
            return File(bytes, "application/octet-stream", fileName);
        }
    }
}