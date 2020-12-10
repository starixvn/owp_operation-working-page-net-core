using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OWP.Maxbet.Models;
using OWP.Maxbet.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace OWP.Maxbet
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class MaxbetController : ControllerBase
    {
        private readonly MaxbetService maxbetService;

        public MaxbetController(MaxbetService maxbetService)
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