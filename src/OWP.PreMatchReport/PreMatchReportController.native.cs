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

    [Route("export-prematch-report")]
    [HttpPost]
    public async Task<IActionResult> ExportPreMatchReport(QueryModel query)
    {
        var request = new ExportPreMatchReportRequest()
        {
            Query = query
        };

        var result = await mediator.Send(request);

        return File(result.Content, result.ContentType, result.Name);
    }
}