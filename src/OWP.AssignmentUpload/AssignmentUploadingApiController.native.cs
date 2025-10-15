

using Fanex.NAP.Client;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OWP.AssignmentUpload.Submit;
using OWP.AssignmentUpload.Validation;
using OWP.Constants;
using OWP.Helpers;
using OWP.RabbitMQ;
using OWP.RabbitMQ.Constants;
using OWP.Services;

namespace OWP.AssignmentUpload;

[Route("api/event-assignment-uploading")]
[ApiController]
[Authorize]

public class AssignmentUploadingApiController(
    IMediator mediator,
    MessageClient rabbitMqClient,
    INapSession currentSession,
    AccessCounter accessCounter)
    : ControllerBase
{
    [HttpPost]
    [Route("validate/{eventDate:datetime}/{isUploadFMAndAIO:bool}")]
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

        await using var ms = new MemoryStream();
        await Request.Form.Files[0].CopyToAsync(ms);
        var request = new ValidationRequest
        {
            IsUploadFMAndAIO = isUploadFMAndAIO,
            EventDate = eventDate,
            Stream = ms
        };

        var viewModel = await mediator.Send(request);
        return Ok(viewModel);
    }

    [HttpPost]
    [Route("submit")]
    [Produces("application/json")]
    public async Task<IActionResult> Submit(SubmitRequest request)
    {
        var viewModel = await mediator.Send(request);

        if (!viewModel.IsSuccessful)
        {
            return Ok(viewModel);
        }

        NotifyForAssignmentUpload(request);
        NotifyForPrematch(request);
        await accessCounter.UpdateCounter(AccessCounterConst.EventUploadNew);
        return Ok(viewModel);
    }

    private void NotifyForAssignmentUpload(SubmitRequest request)
        => rabbitMqClient.SendMessage(new
        {
            request.EventDate,
            request.SessionId,
            request.IsUploadFMAndAIO,
            ActorId = currentSession.UserId,
            currentSession.UserName
        }, RoutingKey.AssignmentUpload);

    private void NotifyForPrematch(SubmitRequest request)
        => rabbitMqClient.SendMessage(new
        {
            EventDate = request.EventDate.Date,
            request.SessionId
        }, RoutingKey.LinkedLgaMatch);
}