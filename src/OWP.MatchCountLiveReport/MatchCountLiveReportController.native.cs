namespace OWP.MatchCountLiveReport;

using Handlers;
using Helper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models;

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
    [HttpGet]
    public async Task<IList<ReportInfo>> Query([FromQuery] QueryRequest request)
    => await mediator.Send(request);

    [Route("export")]
    [HttpGet]
    public async Task<IActionResult> Export([FromQuery] QueryRequest request)
    {
        var dataSource = await mediator.Send(request);
        using var excelPackage = await mediator.Send(new ExportRequest
        {
            DataSource = dataSource
        });
        var fileName = LiveReportHelper.BuildFileName(request);
        var bytes = await excelPackage.GetAsByteArrayAsync();
        return File(bytes, "application/octet-stream", fileName);
    }
}