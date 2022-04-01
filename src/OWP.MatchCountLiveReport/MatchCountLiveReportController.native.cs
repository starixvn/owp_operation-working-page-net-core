using OWP.MatchCountLiveReport.Models;

namespace OWP.OvertimeReport
{
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using OWP.MatchCountLiveReport.Handlers;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    [Authorize]
    public class MatchCountLiveReportController : ControllerBase
    {
        private readonly IMediator mediator;

        public MatchCountLiveReportController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Route("query")]
        [HttpPost]
        public async Task<IList<ReportInfo>> Query([FromQuery] ExportRequest request)
        {
            return new List<ReportInfo>();
        }

        [Route("export")]
        [HttpPost]
        public async Task<IActionResult> Export([FromQuery] ExportRequest request)
        {
            var excelPackage = await mediator.Send(request);
            var fileName = $"OS Live Game Assignment {request.FromDate.ToString("MMddyyyy")} - {request.ToDate.ToString("MMddyyyy")}.xlsx";
            var bytes = await excelPackage.GetAsByteArrayAsync();
            return File(bytes, "application/octet-stream", fileName);

        }
    }
}