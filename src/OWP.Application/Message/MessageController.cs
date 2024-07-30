using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace OWP.Application.Message;

[Route("api/application/message")]
public class MessageController: ControllerBase
{
    private readonly MessageRepository repository;

    public MessageController(MessageRepository repository)
    {
        this.repository = repository;
    }

    [Route("")]
    [HttpGet]
    public async Task<IActionResult> Query([FromQuery] QueryModel model)
    {
        var response = await repository.Query(model);
        return Ok(JsonConvert.SerializeObject(response));
    }

    [Route("")]
    [HttpPut]
    public async Task UpdateIsViewed(UpdateIsViewedModel model)
    {
        await repository.UpdateIsViewed(model);
    }
}
