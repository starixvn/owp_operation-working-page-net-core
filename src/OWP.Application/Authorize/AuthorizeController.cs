using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace OWP.Application.Authorize;

[Route("api/application/authorize")]
[ApiController]
[EnableCors]
[Authorize]
public class AuthorizeController : ControllerBase
{
    private readonly Service service;

    public AuthorizeController(Service service)
    {
        this.service = service;
    }

    [Route("")]
    [HttpPost]
    public async Task<IActionResult> Authorize(AuthorizeModel model)
    {
        var result = await service.Authorize(model);
        return Ok(result);
    }
}