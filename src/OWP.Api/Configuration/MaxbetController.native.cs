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
    [Route("get/{key}")]
    [Produces("application/json")]
    public async Task<IActionResult> GetAdminSiteOsUrls(string key)
    {
        return Ok(configuration.GetSection("LiveOpsUrl:Os").Get<OsUrls>());
    }
}