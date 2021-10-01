using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OWP.TicketCenter
{
    using Newtonsoft.Json;

    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    [EnableCors("AllowOrigin")]
    public class TicketCenterController : ControllerBase
    {
        private readonly IMediator mediator;

        public TicketCenterController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("query-event-tickets")]
        public async Task<EventTicketViewModel> QueryEventCheckingTickets(QueryEventTicketsRequest request)
        => await mediator.Send(request);

        [HttpPost("new-message")]
        public async Task<InsertMessageResultModel> NewMessage(NewMessageRequest request)
        => await mediator.Send(request);


        [HttpPost("new-ticket-issue-event")]
        public async Task<int> NewTicketIssueEvent(NewTicketIssueEventRequest request)
        {
            var ticketId = await mediator.Send(request);
            await mediator.Send(new NewTicketRecipientRequest
            {
                TicketId = ticketId,
                Recipients = request.Recipients
            });
            return ticketId;
        }

        [HttpPost("new-ticket-issue-other")]
        public async Task<int> NewTicketIssueOther(NewTicketIssueOtherRequest request)
        {
            var ticketId = await mediator.Send(request);
            await mediator.Send(new NewTicketRecipientRequest
            {
                TicketId = ticketId,
                Recipients = request.Recipients
            });
            return ticketId;
        }

        [HttpPost("new-ticket-issue-league")]
        public async Task<int> NewTicketIssueLeague(NewTicketIssueLeagueRequest request)
        {
            var ticketId = await mediator.Send(request);
            await mediator.Send(new NewTicketRecipientRequest
            {
                TicketId = ticketId,
                Recipients = request.Recipients
            });
            return ticketId;
        }

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
        public async Task<IActionResult> GetEventTicket(QueryEventTicketsRequest request)
        {
            var result = await mediator.Send(request);
            return Ok(JsonConvert.SerializeObject(result));
        }

        [HttpGet("get-ticket-type")]
        public async Task<IEnumerable<TicketTypeModel>> GetTicketTye()
            => await mediator.Send(new TicketTypeRequest());

        [HttpPost("ticket-detail/get-league")]
        public async Task<TicketLeagueDetailViewModel> GetTicketLeagueDetail(TicketLeagueDetailRequest request)
            => await mediator.Send(request);

        [HttpPost("ticket-detail/get-match")]
        public async Task<TicketMatchDetailViewModel> GetTicketMatchDetail(TicketMatchDetailRequest request)
            => await mediator.Send(request);

        [HttpPost("ticket-detail/get-other")]
        public async Task<TicketOtherDetailViewModel> GetTicketOtherDetail(TicketOtherDetailRequest request)
            => await mediator.Send(request);

        [HttpPost("ticket-detail/insert-single-recipient")]
        public async Task<int> AddSingleRecipient(InsertSingleRecipientRequest request)
            => await mediator.Send(request);

        [HttpPost("ticket-detail/remove-single-recipient")]
        public async Task<int> RemoveSingleRecipient(InsertSingleRecipientRequest request)
            => await mediator.Send(request);

    }
}