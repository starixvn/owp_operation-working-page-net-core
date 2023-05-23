using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace OWP.LiveDetail.User;

[Route("api/live-detail/user")]
[ApiController]
[EnableCors("AllowOrigin")]
[Authorize]
public class LiveDetailUserController : ControllerBase
{
    private readonly UserRepository repository;

    public LiveDetailUserController(UserRepository repository)
    {
        this.repository = repository;
    }

    [Route("suggestion")]
    [HttpGet]
    public async Task<IActionResult> GetSuggestion([FromQuery] GetSuggestionModel model)
    {
        var result = await repository.GetSuggestion(model);
        return Ok(JsonConvert.SerializeObject(result));
    }

    [Route("all")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await repository.GetAll();
        return Ok(JsonConvert.SerializeObject(result));
    }

    [Route("info")]
    [HttpGet]
    public async Task<IActionResult> GetUserInfo(string name)
    {
        var result = await repository.GetUserInfo(name);
        return Ok(JsonConvert.SerializeObject(result));
    }
}