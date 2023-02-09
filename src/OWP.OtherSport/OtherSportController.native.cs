namespace OWP.OtherSport;

using Download;
using EventAssignment;
using EventAssignment.Models;
using Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Submit;
using Submit.Models;
using Validation;

[Route("api/[controller]")]
[ApiController]
[EnableCors("AllowOrigin")]
[Authorize]
public class OtherSportController : ControllerBase
{
    private readonly IMediator mediator;

    public OtherSportController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("last-update")]
    public async Task<LastUpdateInfo> QueryEventAssignment()
        => await mediator.Send(new QueryLastUpdate());

    [Route("get-assignment")]
    [HttpPost]
    public async Task<IEnumerable<EventAssignmentModel>> QueryEventAssignment(QueryEventAssignmentRequest request)
    {
        return await mediator.Send(request);
    }

    [Route("validate")]
    [HttpPost]
    public async Task<IActionResult> Validate()
    {
        if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
        {
            return new UnsupportedMediaTypeResult();
        }

        if (Request.Form.Files.Count != 1)
        {
            return BadRequest();
        }

        await using var ms = new MemoryStream();
        await Request.Form.Files[0].CopyToAsync(ms);

        var request = new ValidationHandler.Request
        {
            Stream = ms,
            FromDate = DateTime.Parse(Request.Form["fromDate"].ToString()),
            ToDate = DateTime.Parse(Request.Form["toDate"].ToString())
        };
        var result = await mediator.Send(request);

        return new JsonResult(result);
    }

    [Route("submit")]
    [HttpPost]
    public async Task<IActionResult> Submit(SubmitRequest request)
    {
        var viewModel = await mediator.Send(request);

        return new JsonResult(viewModel);
    }

    [Route("download")]
    [HttpGet]
    public async Task<IActionResult> Download([FromQuery] DownloadAssignmentRequest request)
    {
        using var excelPackage = await mediator.Send(request);
        var fileName = $"OS Live Game Assignment {request.From.ToString("MMddyyyy")} - {request.To.ToString("MMddyyyy")}.xlsx";
        var bytes = await excelPackage.GetAsByteArrayAsync();
        return File(bytes, "application/octet-stream", fileName);
    }
}