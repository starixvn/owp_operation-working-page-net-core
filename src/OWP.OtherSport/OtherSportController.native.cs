using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OWP.Helpers;
using OWP.Models;
using OWP.OtherSport.Download;
using OWP.OtherSport.EventAssignment;
using OWP.OtherSport.EventAssignment.Models;
using OWP.OtherSport.Submit;
using OWP.OtherSport.Submit.Models;
using OWP.OtherSport.Validation;
using OWP.Services;

namespace OWP.OtherSport;

[Route("api/[controller]")]
[ApiController]
[EnableCors]
[Authorize]
public class OtherSportController : ControllerBase
{
    private readonly IMediator mediator;

    private readonly AccessCounter accessCounter;

    private readonly AccessCounterModel counterModel;

    public OtherSportController(IMediator mediator, AccessCounter accessCounter, IOptions<AccessCounterModel> counterOptions)
    {
        this.mediator = mediator;
        this.accessCounter = accessCounter;
        this.counterModel = counterOptions.Value;
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
        await accessCounter.UpdateCounter(counterModel.OsUpload);
        return new JsonResult(viewModel);
    }

    [Route("download")]
    [HttpGet]
    public async Task<IActionResult> Download([FromQuery] DownloadAssignmentRequest request)
    {
        using var excelPackage = await mediator.Send(request);
        var fileName = $"OS Live Game Assignment {request.From:MMddyyyy} - {request.To:MMddyyyy}.xlsx";
        var bytes = await excelPackage.GetAsByteArrayAsync();
        await accessCounter.UpdateCounter(counterModel.OsExportExcel);
        return File(bytes, "application/octet-stream", fileName);
    }
}