using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OWP.Kpi.LA.Models;

namespace OWP.Kpi.LA;

[Route("api/kpi/la")]
[ApiController]
[Authorize]
[EnableCors("AllowOrigin")]
public class LAKpiController : ControllerBase
{

    [Route("")]
    [HttpGet]
    public async Task<List<KpiInfo>> GetLAKpiInfo(DateTime fromDate, DateTime toDate)
    {
        return new List<KpiInfo>();
    }
}