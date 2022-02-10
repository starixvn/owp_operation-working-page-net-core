using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
using MediatR;
using OWP.PreMatchReport.Handlers;
using OWP.PreMatchReport.Models;

namespace OWP.PreMatchReport;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
[EnableCors("AllowOrigin")]
public class PreMatchReportController : ControllerBase
{
    private readonly IMediator mediator;

    public PreMatchReportController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost("get-prematch-report")]
        public async Task<IActionResult> GetPreMatchReport(ReportRequest request)
    {
            var sender = await mediator.Send(request);
            return Ok(JsonConvert.SerializeObject(sender));
    }

    [Route("export")]
    [HttpGet]
    public async Task<IActionResult> ExportPreMatchReport()
    {
        var request = new ExportPreMatchReportRequest();
        return File(await mediator.Send(request), "application/xlsx", "PreMatchAssignmentReport.xlsx");
    }
}