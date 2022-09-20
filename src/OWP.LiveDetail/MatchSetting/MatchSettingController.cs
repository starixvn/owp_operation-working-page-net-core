namespace OWP.LiveDetail.MatchSetting;

using Constants;
using Fanex.NAP.Client.AspNetCore.Attributes;
using Fanex.NAP.Core.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ;
using RabbitMQ.Constants;

[Route("api/live-detail/match-setting")]
[ApiController]
[Authorize]
[EnableCors("AllowOrigin")]
public class MatchSettingController : ControllerBase
{
    private readonly MessageClient messageClient;
    private readonly MatchSettingRepository repository;

    public MatchSettingController(MatchSettingRepository repository, MessageClient messageClient)
    {
        this.repository = repository;
        this.messageClient = messageClient;
    }

    [HttpGet]
    [Route("")]
    [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Read)]
    public async Task<IActionResult> Query([FromQuery] QueryModel model)
    {
        var result = await repository.Query(model);
        return Ok(JsonConvert.SerializeObject(result));
    }

    [HttpPost]
    [Route("")]
    [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> Create(CreateModel model)
    {
        var result = await repository.Create(model);
        messageClient.SendMessage(new { SessionId = result.SessionId }, RoutingKey.LiveDetail);
        return Ok(JsonConvert.SerializeObject(result));
    }

    [HttpPut]
    [Route("")]
    [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> Update(UpdateModel model)
    {
        var result = await repository.Update(model);
        messageClient.SendMessage(new { SessionId = result.SessionId }, RoutingKey.LiveDetail);
        return Ok(JsonConvert.SerializeObject(result));
    }


    [HttpDelete]
    [Route("")]
    [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> Delete(DeleteModel model)
    {
        var result = await repository.Delete(model);
        messageClient.SendMessage(new { SessionId = result.SessionId }, RoutingKey.LiveDetail);
        return Ok(JsonConvert.SerializeObject(result));
    }
}