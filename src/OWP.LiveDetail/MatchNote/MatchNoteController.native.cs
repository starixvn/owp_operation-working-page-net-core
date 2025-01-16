using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OWP.Constants;
using OWP.RabbitMQ;
using OWP.RabbitMQ.Constants;

namespace OWP.LiveDetail.MatchNote;

using Fanex.NAP.Client.Attributes;
using Fanex.NAP.Client.Helpers.Business;

[Route("api/live-detail/match-note")]
[ApiController]
[Authorize]
public class MatchNoteController(
    MatchNoteRepository repository,
    MessageClient messageClient) : ControllerBase
{
    [HttpGet]
    [Route("")]
    [NapAuth(FunctionKey = FunctionKeys.Note, Policy = Constant.Policy.Read)]
    public async Task<IActionResult> Query([FromQuery] QueryModel model)
    {
        var result = await repository.Query(model);
        return Ok(JsonConvert.SerializeObject(result));
    }

    [HttpPost]
    [Route("")]
    [NapAuth(FunctionKey = FunctionKeys.Note, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> Create(CreateModel model)
    {
        var result = await repository.Create(model);
        await messageClient.SendMessage(new { result.SessionId }, RoutingKey.LiveDetail);
        return Ok(JsonConvert.SerializeObject(result));
    }

    [HttpPut]
    [Route("")]
    [NapAuth(FunctionKey = FunctionKeys.Note, Policy = Constant.Policy.Host)]
    public async Task Update(UpdateModel model)
    {
        var sessionId = await repository.Update(model);
        await messageClient.SendMessage(new { SessionId = sessionId }, RoutingKey.LiveDetail);
    }

    [HttpDelete]
    [Route("")]
    [NapAuth(FunctionKey = FunctionKeys.Note, Policy = Constant.Policy.Host)]
    public async Task Delete(DeleteModel model)
    {
        var sessionId = await repository.Delete(model);
        await messageClient.SendMessage(new { SessionId = sessionId }, RoutingKey.LiveDetail);
    }
}