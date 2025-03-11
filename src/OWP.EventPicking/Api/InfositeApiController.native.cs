
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace OWP.EventPicking.Api;

[Route("api/Infosite")]
[ApiController]
[AllowAnonymous]
public class InfositeApiController(IMapper mapper) : ControllerBase
{
    [HttpPost("getEventChangeGameDate")]

    public IActionResult SyncEventChangeGameDate(
        [FromServices] IConfiguration configuration,
        [FromServices] ISender sender
        , socer_event_change_game_date @event)
    {
        var enable=configuration.GetValue<bool>("SyncEventChangeGameDate");
        if (enable)
        {
            var changeKoRequest=mapper.Map<InsertOrUpdateEventChangeKoRequest>(@event);
            sender.Send(changeKoRequest);
        }
        return Ok("Success");
    }
}