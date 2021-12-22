namespace OWP.PreMatchAssignment
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using OWP.PreMatchAssignment.Handlers;
    using OWP.PreMatchAssignment.Models;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [EnableCors("AllowOrigin")]
    public class PreMatchAssignmentController : ControllerBase
    {
        private readonly IMediator mediator;

        public PreMatchAssignmentController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Route("get-working-shift")]
        [HttpGet]
        public async Task<IEnumerable<WorkingShift>> GetWorkingShift(DateTime date)
        {
            var request = new QueryWorkingShiftRequest
            {
                Date = date
            };

            return await mediator.Send(request);
        }

        [Route("save")]
        [HttpPost]
        public async Task<IActionResult> SavePrematchAssignment(UpdateLeagueAssignmentRequest request)
        {
            await mediator.Send(request);
            return Ok();
        }

        [Route("get-log")]
        [HttpGet]
        public async Task<QueryLogViewModel> QueryLog(int pageSize, int pageNumber, bool isDay)
        {
            var request = new QueryLogRequest { PageSize = pageSize, PageNumber = pageNumber, IsDay = isDay };
            return await mediator.Send(request);
        }

        [Route("get-day-shift")]
        [HttpPost]
        public async Task<IActionResult> QueryDayShift(QueryDayShiftPreMatchRequest request)
        {
            var sender = await mediator.Send(request);

            return Ok(JsonConvert.SerializeObject(sender));
        }

        [Route("get-night-shift")]
        [HttpPost]
        public async Task<QueryPreMatchLeagueViewModel> QueryNightShift(QueryNightShiftPreMatchRequest request)
        {
            return await mediator.Send(request);
        }

        [Route("post-message")]
        [HttpPost]
        public async Task<IEnumerable<Message>> PostMessage(PostNewMessageRequest request)
        {
            return await mediator.Send(request);
        }

        [Route("get-message")]
        [HttpPost]
        public async Task<IEnumerable<Message>> GetMessage(GetIssueRequest request)
        {
            return await mediator.Send(request);
        }

        [Route("create-issue")]
        [HttpPost]
        public async Task<QueryMessagesViewModel> CreateNewIssue(CreateNewIssueRequest request)
        {
            return await mediator.Send(request);
        }

        [Route("query-league-by-id")]
        [HttpPost]
        public async Task<QueryPreMatchLeagueViewModel> QueryById(QueryPreMatchLeagueByIdRequest request)
        {
            return await mediator.Send(request);
        }

        [Route("post-note-message")]
        [HttpPost]
        public async Task<Note> PostNoteMessage(PostNewNoteMessageRequest request)
        {
            return await mediator.Send(request);
        }

        [Route("create-note")]
        [HttpPost]
        public async Task<Note> CreateNewNote(CreateNewNoteRequest request)
        {
            return await mediator.Send(request);
        }

        [Route("edit-note-message")]
        [HttpPost]
        public async Task<Note> EditNoteMessage(EditNoteMessageRequest request)
        {
            return await mediator.Send(request);
        }

        [Route("on-click-issue-notification")]
        [HttpPost]
        public async Task OpenChatBoard(OnClickIssueNotificationRequest request)
        {
            await mediator.Send(request);
        }
    }
}