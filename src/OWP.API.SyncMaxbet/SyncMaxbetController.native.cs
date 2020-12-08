using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OWP.API.SyncMaxbet.Services;

namespace OWP.API.SyncMaxbet
{
    [Route("api/sync-maxbet")]
    [ApiController]
    [AllowAnonymous]
    public class SyncMaxbetController : ControllerBase
    {
        private readonly MaxbetService maxbetService;

        private readonly IMediator mediator;

        public SyncMaxbetController(MaxbetService maxbetService, IMediator mediator)
        {
            this.maxbetService = maxbetService;
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("{matchId}")]
        public async Task<IActionResult> GetMaxbet(int matchId)
        {
            var maxbetInfo = await maxbetService.GetMaxbetInfo(matchId);
            return Ok(JsonConvert.SerializeObject(maxbetInfo));
        }
    }
}