using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace OWP.LiveDetail.Agent;

[Route("api/live-detail/agent")]
[ApiController]
[Authorize]
[EnableCors]
public class AgentController : ControllerBase
{
    private readonly AgentRepository repository;

    public AgentController(AgentRepository repository)
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