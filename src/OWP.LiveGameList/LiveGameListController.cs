namespace OWP.LiveGameList;

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services;

[Route("api/[controller]")]
[ApiController]
[EnableCors("AllowOrigin")]
public class LiveGameListController : ControllerBase
{
    private readonly EventInformationService service;

    public LiveGameListController(EventInformationService service)
    {
        this.service = service;
    }

    [Route("query")]
    [HttpGet]
    public async Task<IActionResult> Query(DateTime eventDate, string statusId)
    {
        var result = await service.GetEvents(eventDate, statusId);
        return Ok(JsonConvert.SerializeObject(result));
    }
}