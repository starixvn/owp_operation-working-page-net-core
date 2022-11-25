using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OWP.LiveGameList.Services;

namespace OWP.LiveGameList;

[Route("api/[controller]")]
[ApiController]
[EnableCors("AllowOrigin")]
[Authorize]
public class LiveGameListController : ControllerBase
{
    private readonly EventInformationService service;

    public LiveGameListController(EventInformationService service)
    {
        this.service = service;
    }

    [Route("query")]
    [HttpGet]
    public async Task<IList<Object>> Query()
        => await service.GetEvents(object await );
}