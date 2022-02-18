using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Olympus.OWP.Constants;
using OWP.Maxbet.Services;


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
        [Route("matchId/subType/{matchId}/{subType}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetBettypeId(int matchId, int subType)
        {
            var bettypeId = MinorBettype.GetMinorBettypeIdBySubtype(subType);
            var owpMatch = await maxbetService.GetMatchIdByAdminSiteId(matchId, bettypeId);
            return Ok(new { subType, bettypeId, owpMatch });
        }

        [HttpGet]
        [Route("{matchId}/{subType}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetSpecialProduct(int matchId, int subType)
        {
            var bettypeId = MinorBettype.GetMinorBettypeIdBySubtype(subType);
            var owpMatch = await maxbetService.GetMatchIdByAdminSiteId(matchId, bettypeId);
            if (owpMatch is null)
            {
                return NotFound();
            }

            var maxbetInfo = await maxbetService.GetSpecialProductInfo(owpMatch, matchId, bettypeId);

            return Ok(maxbetInfo);
           
        }
    }
}