using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;


namespace OWP.Kpi.LA;

[Route("api/kpi/la")]
[ApiController]
[Authorize]
[EnableCors("AllowOrigin")]
public class LAKpiController : ControllerBase
{
   
    [Route("")]
    [HttpGet]
    public async Task<IActionResult> GetLAKpiInfo(DateTime fromDate, DateTime toDate)
    {
        // TODO
        throw new NotImplementedException();
    }
}