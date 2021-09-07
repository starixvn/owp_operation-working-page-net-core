using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Olympus.OWP.Constants;
using OWP.Maxbet.Services;
using System.Threading.Tasks;

namespace OWP.Maxbet
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    [EnableCors("AllowOrigin")]
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

        [HttpGet]
        [Route("{matchId}/{subType}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetSpecialProduct(int matchId, int subType)
        {
            var bettypeId = MinorBettype.GetMinorBettypeIdBySubtype(subType);
            var owpMatchId = await maxbetService.GetMatchIdByAdminSiteId(matchId, bettypeId);


            if (owpMatchId == 0)
            {
                return NotFound();
            }

            var maxbetInfo = await maxbetService.GetSpecialProductInfo(owpMatchId, matchId, bettypeId);

            return Ok(maxbetInfo);
        }
    }
}