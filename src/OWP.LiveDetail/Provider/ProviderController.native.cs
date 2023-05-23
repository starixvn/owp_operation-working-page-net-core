using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace OWP.LiveDetail.Provider;

[Route("api/live-detail/provider")]
[ApiController]
[Authorize]
[EnableCors("AllowOrigin")]
public class ProviderController : ControllerBase
{
    private readonly ProviderRepository repository;

    public ProviderController(ProviderRepository repository)
    {
        this.repository = repository;
    }

    [Route("all")]
    public async Task<IActionResult> GetAll()
    {
        var result = await repository.GetAll();
        return Ok(JsonConvert.SerializeObject(result));
    }
}