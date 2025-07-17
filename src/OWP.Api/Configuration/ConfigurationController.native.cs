using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OWP.Api.Configuration.Models;

namespace OWP.Api.Maxbet;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class ConfigurationController(IConfiguration configuration) : ControllerBase
{


    [HttpGet]
    [Route("os/get")]
    [Produces("application/json")]
    public async Task<IActionResult> GetAdminSiteOsUrls()
    {
        return Ok(configuration.GetSection("LiveOpsUrl:Os").Get<OsUrls>());
    }

    [HttpGet]
    [Route("soccer/get")]
    [Produces("application/json")]
    public async Task<IActionResult> GetAdminSitSoccerUrls()
    {
        var soccerUrls = configuration.GetSection("LiveOpsUrl:Soccer").Get<SoccerUrls>();
        var servers = configuration.GetSection("LiveOpsUrl:Servers").Get<List<ServerInfo>>();
        soccerUrls.Servers = servers;
        return Ok(soccerUrls);
    }
}