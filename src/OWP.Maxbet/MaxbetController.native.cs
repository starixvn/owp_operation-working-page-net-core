using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Olympus.OWP.Constants;
using OWP.Maxbet.Constants;
using OWP.Maxbet.Services;

namespace OWP.Maxbet;

[Route("api/[controller]/{matchId:int}")]
[ApiController]
[AllowAnonymous]
[EnableCors]
public class MaxbetController : ControllerBase
{
    private readonly MainMatchService mainMatchService;

    private readonly SpecialProductService specialProductService;

    private readonly ETPKMatchService ETPKMatchService;

    public MaxbetController(MainMatchService mainMatchService,
        SpecialProductService specialProductService, ETPKMatchService ETPKMatchService)
    {
        this.mainMatchService = mainMatchService;
        this.specialProductService = specialProductService;
        this.ETPKMatchService = ETPKMatchService;
    }

    [HttpGet]
    [Route("")]
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
    [Route("{subType:int}")]
    [Produces("application/json")]
    public async Task<IActionResult> GetSpecialProduct(int matchId, int subType)
    {
        if (IsMatchETPK(subType))
        {
            var maxbetETPKInfo = await ETPKMatchService.GetMaxBetETPK(matchId, subType);
            return maxbetETPKInfo is null ? NotFound()
                : Ok(maxbetETPKInfo);
        }
        var bettypeId = MinorBettype.GetMinorBettypeIdBySubtype(subType);
        var owpMatch = await specialProductService.GetMatchIdByAdminSiteId(matchId, bettypeId);
        if (owpMatch is null)
        {
            return NotFound();
        }

        var maxbetInfo = await specialProductService.GetSpecialProductInfo(owpMatch, matchId, bettypeId);

        return Ok(maxbetInfo);
    }

    private bool IsMatchETPK(int subType)
        => Array.Exists(SubTypeETPK.SubTypeETPKs, i => i == subType);
}