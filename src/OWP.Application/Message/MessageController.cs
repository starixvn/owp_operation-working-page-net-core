using Microsoft.AspNetCore.Mvc;

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
    public async Task<QueryResult> Query([FromQuery] QueryModel model)
    {
        return await repository.Query(model);
    }

    [Route("")]
    [HttpPut]
    public async Task UpdateIsViewed(UpdateIsViewedModel model)
    {
        await repository.UpdateIsViewed(model);
    }
}
