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

        [HttpGet("new-ticket/get-event")]
        [Produces("application/json")]
        public async Task<IActionResult> GetEventInfo(string keyword)
        {
            var eventSender = await mediator.Send(new NewTicketRequest
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