using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace OWP.Application.Header;

[Route("api/application/header")]
[ApiController]
[EnableCors]
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
    public async Task<Header> Query()
    {
        return await service.Query();
    }
}