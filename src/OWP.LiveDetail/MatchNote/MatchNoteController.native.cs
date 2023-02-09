namespace OWP.LiveDetail.MatchNote;

using Constants;
using Fanex.NAP.Client.AspNetCore.Attributes;
using Fanex.NAP.Core.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ;
using RabbitMQ.Constants;

[Route("api/live-detail/match-note")]
[ApiController]
[Authorize]
[EnableCors("AllowOrigin")]
public class MatchNoteController : ControllerBase
{
    private readonly MessageClient messageClient;
    private readonly MatchNoteRepository repository;

    public MatchNoteController(MatchNoteRepository repository, MessageClient messageClient)
    {
        this.repository = repository;
        this.messageClient = messageClient;
    }

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
        messageClient.SendMessage(new { result.SessionId }, RoutingKey.LiveDetail);
        return Ok(JsonConvert.SerializeObject(result));
    }

    [HttpPut]
    [Route("")]
    [NapAuth(FunctionKey = FunctionKeys.Note, Policy = Constant.Policy.Host)]
    public async Task Update(UpdateModel model)
    {
        var sessionId = await repository.Update(model);
        messageClient.SendMessage(new { SessionId = sessionId }, RoutingKey.LiveDetail);
    }

    [HttpDelete]
    [Route("")]
    [NapAuth(FunctionKey = FunctionKeys.Note, Policy = Constant.Policy.Host)]
    public async Task Delete(DeleteModel model)
    {
        var sessionId = await repository.Delete(model);
        messageClient.SendMessage(new { SessionId = sessionId }, RoutingKey.LiveDetail);
    }
}