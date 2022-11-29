using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OWP.LiveGameList.Services;

namespace OWP.LiveGameList;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
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
        var value = await service.GetEvents(eventDate, statusId);
        return Ok(JsonConvert.SerializeObject(value));
    }
}