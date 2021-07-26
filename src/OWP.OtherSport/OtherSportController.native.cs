namespace OWP.OtherSport
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using OWP.Helpers;
    using OWP.OtherSport.Download;
    using OWP.OtherSport.EventAssignment;
    using OWP.OtherSport.EventAssignment.Models;
    using OWP.OtherSport.Notification;
    using OWP.OtherSport.Submit;
    using OWP.OtherSport.Submit.Models;
    using OWP.OtherSport.Validation;

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

            if (Request.Form.Files != null && Request.Form.Files.Count == 1)
            {
                using var ms = new MemoryStream();
                Request.Form.Files[0].CopyTo(ms);

                var request = new ValidationHandler.Request
                {
                    Stream = ms
                };
                var result = await mediator.Send(request);

                return new JsonResult(result);
            }

            return BadRequest();
        }

        [Route("submit")]
        [HttpPost]
        public async Task<IActionResult> Submit(SubmitRequest request)
        {
            var viewModel = await mediator.Send(request);

            var requestNotify = new NotifyRequest
            {
                SessionId = request.SessionId,
                EventDate = viewModel.EventDates
            };

            await mediator.Send(requestNotify);

            return new JsonResult(viewModel);
        }

        [Route("download")]
        [HttpGet]
        public async Task<IActionResult> Download(
            [FromQuery] DateTime from,
            [FromQuery] DateTime to,
            [FromQuery] string employee,
            [FromQuery] string sports,
            [FromQuery] string matchStatuses)
        {
            var request = new DownloadAssignmentRequest
            {
                From = from,
                To = to,
                Employee = employee,
                Sports = sports,
                MatchStatuses = matchStatuses
            };

            var excelPackage = await mediator.Send(request);
            var fileName = $"OS Live Game Assignment {from.ToString("MMddyyyy")} - {to.ToString("MMddyyyy")}.xlsx";
            return File(excelPackage.GetAsByteArray(), "application/octet-stream", fileName);
        }
    }
}