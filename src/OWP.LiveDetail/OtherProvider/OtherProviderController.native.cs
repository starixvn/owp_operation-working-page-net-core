using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace OWP.LiveDetail.OtherProvider;

[Route("api/live-detail/other-provider")]
[ApiController]
[Authorize]
[EnableCors]
public class OtherProviderController : ControllerBase
{
    private readonly OtherProviderRepository repository;

    public OtherProviderController(OtherProviderRepository repository)
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