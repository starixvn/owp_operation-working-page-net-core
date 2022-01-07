using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Middleware;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Services;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    [EnableCors("AllowOrigin")]
    public class TicketCenterController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly PermissionService permissionService;

        public TicketCenterController(IMediator mediator, PermissionService permissionService)
        {
            this.mediator = mediator;
            this.permissionService = permissionService;
        }

        [HttpPost("new-message")]
        [ServiceFilter(typeof(PermissionFilter))]
        public async Task<IActionResult> NewMessage(NewMessageRequest request)
        {
            var result = await mediator.Send(request);
            return Ok(result);
        }

        [Route("on-click-issue-notification")]
        [HttpPost]
        [ServiceFilter(typeof(PermissionOnOpenTicketDetail))]
        public async Task OpenChatBoard(OnClickIssueNotificationRequest request)
            => await mediator.Send(request);

        #region new ticket

        [HttpPost("new-ticket-issue-event")]
        public async Task<int> NewTicketIssueEvent(NewTicketIssueEventRequest request)
            => await mediator.Send(request);

        [HttpPost("new-ticket-issue-other")]
        public async Task<int> NewTicketIssueOther(NewTicketIssueOtherRequest request)
            => await mediator.Send(request);

        [HttpPost("new-ticket-issue-support")]
        public async Task<int> NewTicketIssueSupport(NewTicketIssueSupportRequest request)
            => await mediator.Send(request);

        [HttpPost("new-ticket-issue-league")]
        public async Task<int> NewTicketIssueLeague(NewTicketIssueLeagueRequest request)
        => await mediator.Send(request);

        #endregion new ticket

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
        [ServiceFilter(typeof(PermissionFilter))]
        public async Task<IActionResult> GetTicketLeagueDetail(TicketLeagueDetailRequest request)
        {
            var result = await mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("ticket-detail/get-match")]
        [ServiceFilter(typeof(PermissionFilter))]
        public async Task<IActionResult> GetTicketMatchDetail(TicketMatchDetailRequest request)
        {
            var result = await mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("ticket-detail/get-other")]
        [ServiceFilter(typeof(PermissionFilter))]
        public async Task<IActionResult> GetTicketOtherDetail(TicketOtherDetailRequest request)
        {
            var result = await mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("ticket-detail/get-support")]
        [ServiceFilter(typeof(PermissionFilter))]
        public async Task<IActionResult> GetTicketSupportDetail(TicketSupportDetailRequest request)
        {
            var result = await mediator.Send(request);
            return Ok(result);
        }

        #endregion get detail

        [HttpPost("ticket-detail/insert-single-recipient")]
        [ServiceFilter(typeof(PermissionFilter))]
        public async Task<IActionResult> AddSingleRecipient(InsertSingleRecipientRequest request)
        {
            var result = await mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("ticket-detail/remove-single-recipient")]
        [ServiceFilter(typeof(PermissionFilter), Order = 0)]
        [ServiceFilter(typeof(PermissionRemoveLastRecipient), Order = 1)]
        public async Task<IActionResult> RemoveSingleRecipient(RemoveSingleRecipientRequest request)
        {
            var result = await mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("ticket-detail/update-ticket-status")]
        [ServiceFilter(typeof(PermissionFilter))]
        public async Task<IActionResult> UpdateTicketStatus(UpdateTicketStatusRequest request)
        {
            var result = await mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("ticket-detail/get-status-history")]
        public async Task<IEnumerable<StatusHistory>> GetStatusHistoryByTicketId(int ticketId)
            => await mediator.Send(new GetStatusHistoryRequest { TicketId = ticketId });

        [HttpPost("pin-ticket")]
        public async Task PinTicket(PinTicketRequest request)
            => await mediator.Send(request);
    }
}