using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace OWP.Application.Authorize;

[Route("api/application/authorize")]
[ApiController]
[EnableCors]
[Authorize]
public class AuthorizeController : ControllerBase
{
    private readonly AuthorizeService service;

    public AuthorizeController(AuthorizeService service)
    {
        this.service = service;
    }

    [Route("")]
    [HttpPost]
    public async Task<IActionResult> Authorize(AuthorizeModel model)
    {
        var result = await service.Authorize(model);
        return Ok(JsonConvert.SerializeObject(result));
    }
}