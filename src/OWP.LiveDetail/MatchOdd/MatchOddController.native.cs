using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OWP.Constants;
using OWP.RabbitMQ;
using OWP.RabbitMQ.Constants;

namespace OWP.LiveDetail.MatchOdd;

using Fanex.NAP.Client.Attributes;
using Fanex.NAP.Client.Helpers.Business;

[Route("api/live-detail/match-odd")]
[ApiController]
[Authorize]
public class MatchOddController(
    MatchOddRepository repository,
    MessageClient messageClient) : ControllerBase
{
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
        await messageClient.SendMessage(new { result.SessionId }, RoutingKey.LiveDetail);
        return Ok(JsonConvert.SerializeObject(result));
    }

    [HttpPut]
    [Route("")]
    [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> Update(UpdateModel model)
    {
        var result = await repository.Update(model);
        await messageClient.SendMessage(new { result.SessionId }, RoutingKey.LiveDetail);
        return Ok(JsonConvert.SerializeObject(result));
    }

    [HttpDelete]
    [Route("")]
    [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> Delete(DeleteModel model)
    {
        var result = await repository.Delete(model);
        await messageClient.SendMessage(new { result.SessionId }, RoutingKey.LiveDetail);
        return Ok(JsonConvert.SerializeObject(result));
    }
}