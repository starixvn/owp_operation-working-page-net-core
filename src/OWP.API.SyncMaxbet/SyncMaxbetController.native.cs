using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OWP.API.SyncMaxbet.Models;
using OWP.API.SyncMaxbet.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace OWP.API.SyncMaxbet
{
    [Route("api/sync-maxbet")]
    [ApiController]
    [AllowAnonymous]
    public class SyncMaxbetController : ControllerBase
    {
        private readonly MaxbetService maxbetService;

        public SyncMaxbetController(MaxbetService maxbetService)
        {
            this.maxbetService = maxbetService;
        }

        [HttpGet]
        [Route("{matchId}")]
        [SwaggerOperation(Summary = "Get match's maxbet by MatchId")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(MaxbetInfo))]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> GetMaxbet(int matchId)
        {
            var owpMatchId = await maxbetService.GetMatchIdByAdminSiteId(matchId);

            if (owpMatchId == 0)
            {
                return NotFound();
            }

            var maxbetInfo = await maxbetService.GetMaxbetInfo(owpMatchId, matchId);

            return Ok(maxbetInfo);
        }
    }
}