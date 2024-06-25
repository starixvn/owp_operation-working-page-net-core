using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace OWP.Application.Header;

[Route("api/application/header")]
[ApiController]
[Authorize]
public class HeaderController : ControllerBase
{
    private readonly HeaderService service;

    public HeaderController(HeaderService service)
    {
        this.service = service;
    }

    [Route("")]
    [HttpGet]
    public async Task<IActionResult> Query()
    {
        var response = await service.Query();
        return Ok(JsonConvert.SerializeObject(response));
    }
}