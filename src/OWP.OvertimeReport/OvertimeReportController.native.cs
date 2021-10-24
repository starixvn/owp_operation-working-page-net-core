namespace OWP.OvertimeReport
{
    using Handlers;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
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
        public async Task<IActionResult> ExportReportByMonth(
            [FromQuery] string location,
            [FromQuery] string trader,
            [FromQuery] int month,
            [FromQuery] int year)
        {
            var request = new ExportReportByMonthRequest
            {
                Location = location ?? string.Empty,
                Trader = trader ?? string.Empty,
                Month = month,
                Year = year
            };
            var excelPackage = await mediator.Send(request);
            var monthYearInString = new DateTime(request.Year, request.Month, 1).ToString("MMM yyyy");
            var fileName = $"Monthly Overtime Report - {monthYearInString}.xlsx";

            return File(excelPackage.GetAsByteArray(), "application/octet-stream", fileName);
        }
    }
}
