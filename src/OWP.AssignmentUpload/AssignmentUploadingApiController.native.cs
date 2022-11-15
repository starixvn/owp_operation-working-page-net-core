

namespace OWP.AssignmentUpload;

using OWP.RabbitMQ;
using System;
using System.IO;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Notification;
using OWP.Helpers;
using OWP.RabbitMQ.Constants;
using Submit;
using Validation;

[Route("api/event-assignment-uploading")]
[ApiController]
[Authorize]
[EnableCors("AllowOrigin")]

public class AssignmentUploadingApiController : ControllerBase
{
    private readonly IMediator mediator;

    private readonly MessageClient rabbitMqClient;

    public AssignmentUploadingApiController(IMediator mediator,
        MessageClient rabbitMqClient)
    {
        this.mediator = mediator;
        this.rabbitMqClient = rabbitMqClient;
    }

    [HttpPost("validate/{eventDate}/{isUploadFMAndAIO}")]
    [Produces("application/json")]
    public async Task<IActionResult> Validate(DateTime eventDate, bool isUploadFMAndAIO)
    {
        if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
        {
            return new UnsupportedMediaTypeResult();
        }

        if (Request.Form.Files.Count != 1)
        {
            return BadRequest();
        }

        var stream = await GetUploadStream();

        var request = new ValidationRequest
        {
            IsUploadFMAndAIO = isUploadFMAndAIO,
            EventDate = eventDate,
            Stream = stream
        };

        var viewModel = await mediator.Send(request);
        return Ok(viewModel);
    }

    [HttpPost("submit")]
    public async Task<IActionResult> Submit(SubmitRequest request)
    {
        var viewModel = await mediator.Send(request);

        var requestNotify = new NotifyRequest
        {
            SessionId = request.SessionId,
            EventDate = request.EventDate,
            IsUploadFMAndAIO = request.IsUploadFMAndAIO
        };

        await Notify(requestNotify);

        NotifyForPrematch(requestNotify);

        return Ok(viewModel);
    }

    private async Task Notify(NotifyRequest request)
    => await mediator.Send(request);

    private void NotifyForPrematch(NotifyRequest request)
        => rabbitMqClient.SendMessage(new
        {
            EventDate = request.EventDate.Date,
            SessionId = request.SessionId
        }, RoutingKey.LinkedLgaMatch);

    private async Task<Stream> GetUploadStream()
    {
        await using var ms = new MemoryStream();
        await Request.Form.Files[0].CopyToAsync(ms);
        return ms;
    }
}