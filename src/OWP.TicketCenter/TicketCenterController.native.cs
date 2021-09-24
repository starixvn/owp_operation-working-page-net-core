using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OWP.TicketCenter
{
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



        [HttpPost("new-ticket-issue-event")]

        public async Task<int> NewTicketIssueEvent(NewTicketIssueEventRequest request)
        {
            var ticketId = await mediator.Send(request);
            await mediator.Send(new NewTicketRecipientRequest
            {
                TicketId = ticketId,
                Recipient = request.Recipients
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
                Recipient = request.Recipients
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
                Recipient = request.Recipients
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

        [HttpGet("gettickettype")]
        public async Task<IEnumerable<TicketType>> GetTicketTye()
            => await mediator.Send(new TicketTypeRequest());
    }
}