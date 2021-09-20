namespace OWP.RecipientGroup
{
    using Handlers;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    [AllowAnonymous]
    //[Authorize]
    public class RecipientGroupController : ControllerBase
    {
        private readonly IMediator mediator;

        public RecipientGroupController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Route("get-recipient-group")]
        [HttpGet]
        public async Task<IEnumerable<TicketGroup>> GetWorkingShift()
        {
            var result = await mediator.Send(new QueryRecipientGroupRequest());
            return result;
        }
    }
}