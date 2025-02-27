
namespace OWP.EventPicking.Api;

[Route("api/Infosite")]
[ApiController]

public class InfositeApiController(IMapper mapper) : ControllerBase
{
    [HttpPost("getEventChangeGameDate")]
    public IActionResult SyncEventChangeGameDate([FromServices] ISender sender, socer_event_change_game_date @event)
    {
        var changeKoRequest=mapper.Map<InsertOrUpdateEventChangeKoRequest>(@event);
        sender.Send(changeKoRequest);
        return Ok("Success");
    }
}