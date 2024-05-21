using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OWP.Event.LiveGameList.Services;

namespace OWP.Event.LiveGameList;

[Route("api/[controller]")]
[ApiController]
[EnableCors]
public class LiveGameListController : ControllerBase
{
    private readonly GetEventByDateService service;

    public LiveGameListController(GetEventByDateService service)
    {
        this.service = service;
    }

    [Route("query")]
    [HttpGet]
    public async Task<IActionResult> Query(DateTime eventDate, string statusId, string userIds, string matchType)
    {
        var result = await service.GetEvents(eventDate, statusId, userIds, matchType);

        return Ok(JsonConvert.SerializeObject(result));
    }
}