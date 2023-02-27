using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace OWP.LiveDetail.InfositeMatch;

[Route("api/live-detail/infosite-match")]
[ApiController]
[Authorize]
[EnableCors("AllowOrigin")]
public class InfositeMatchController : ControllerBase
{
    private readonly InfositeMatchRepository repository;

    public InfositeMatchController(InfositeMatchRepository repository)
    {
        this.repository = repository;
    }

    [Route("get")]
    [HttpPost]
    public async Task<IActionResult> Get(GetInfositeMatchModel model)
    {
        var result = await repository.Get(model);
        return Ok(JsonConvert.SerializeObject(result));
    }
}