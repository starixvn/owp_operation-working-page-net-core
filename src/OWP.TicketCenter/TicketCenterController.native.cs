using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using OWP.TicketCenter.Notification;
using OWP.TicketCenter.Notification.Services;

namespace OWP.TicketCenter
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [EnableCors("AllowOrigin")]
    public class TicketCenterController : ControllerBase
    {
        private readonly IMediator mediator;


        public TicketCenterController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("new-message")]
        public async Task<MessageDetailViewModel> NewMessage(NewMessageRequest request)
        => await mediator.Send(request);

        #region new ticket

        [HttpPost("new-ticket-issue-event")]
        public async Task<int> NewTicketIssueEvent(NewTicketIssueEventRequest request)
            => await mediator.Send(request);

        [HttpPost("new-ticket-issue-other")]
        public async Task<int> NewTicketIssueOther(NewTicketIssueOtherRequest request)
            => await mediator.Send(request);

        [HttpPost("new-ticket-issue-league")]
        public async Task<int> NewTicketIssueLeague(NewTicketIssueLeagueRequest request)
        => await mediator.Send(request);
       

        #endregion

        [HttpGet("new-ticket/get-event")]
        [Produces("application/json")]
        public async Task<IActionResult> GetEventInfo(string keyword)
        {
            var eventSender = await mediator.Send(new GetTicketEventOptionRequest
            {
                Keyword = keyword
            });
            return Ok(eventSender);
        }

        [HttpPost("get-event-ticket")]
        public async Task<List<TicketDetailBaseViewModel>> GetEventTicket(QueryEventTicketsRequest request)
            => await mediator.Send(request);

        [HttpGet("get-ticket-type")]
        public async Task<IEnumerable<TicketTypeModel>> GetTicketTye()
            => await mediator.Send(new TicketTypeRequest());

        #region get detail
        [HttpPost("ticket-detail/get-league")]
        public async Task<TicketLeagueDetailViewModel> GetTicketLeagueDetail(TicketLeagueDetailRequest request)
            => await mediator.Send(request);

        [HttpPost("ticket-detail/get-match")]
        public async Task<TicketMatchDetailViewModel> GetTicketMatchDetail(TicketMatchDetailRequest request)
            => await mediator.Send(request);

        [HttpPost("ticket-detail/get-other")]
        public async Task<TicketOtherDetailViewModel> GetTicketOtherDetail(TicketOtherDetailRequest request)
            => await mediator.Send(request);

        #endregion

        [HttpPost("ticket-detail/insert-single-recipient")]
        public async Task<int> AddSingleRecipient(InsertSingleRecipientRequest request)
            => await mediator.Send(request);

        [HttpPost("ticket-detail/remove-single-recipient")]
        public async Task<int> RemoveSingleRecipient(RemoveSingleRecipientRequest request)
            => await mediator.Send(request);

    }
}