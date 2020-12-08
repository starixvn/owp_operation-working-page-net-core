using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OWP.API.SyncMaxbet.Services;

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