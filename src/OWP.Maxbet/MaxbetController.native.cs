using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OWP.Maxbet.Services;

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