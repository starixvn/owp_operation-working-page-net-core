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
    private readonly MainMatchService mainMatchService;

    private readonly SpecialProductService specialProductService;

    public MaxbetController(MainMatchService mainMatchService,
      SpecialProductService specialProductService)
    {
      this.mainMatchService = mainMatchService;
      this.specialProductService = specialProductService;
    }

    [HttpGet]
    [Route("{matchId}")]
    [Produces("application/json")]
    public async Task<IActionResult> GetMaxbet(int matchId)
    {
      var owpMatchId = await mainMatchService.GetMatchIdByAdminSiteId(matchId);

      if (owpMatchId == 0)
      {
        return NotFound();
      }

      var maxbetInfo = await mainMatchService.GetMaxbetInfo(owpMatchId, matchId);

      return Ok(maxbetInfo);
    }

    [HttpGet]
    [Route("{matchId}/{subType}")]
    [Produces("application/json")]
    public async Task<IActionResult> GetSpecialProduct(int matchId, int subType)
    {
      var bettypeId = MinorBettype.GetMinorBettypeIdBySubtype(subType);
      var owpMatch = await specialProductService.GetMatchIdByAdminSiteId(matchId, bettypeId);
      if (owpMatch is null)
      {
        return NotFound();
      }

      var maxbetInfo = await specialProductService.GetSpecialProductInfo(owpMatch, matchId, bettypeId);

      return Ok(maxbetInfo);
    }
  }
}