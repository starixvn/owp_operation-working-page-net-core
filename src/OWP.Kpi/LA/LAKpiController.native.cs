using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OWP.Kpi.LA.Constant;
using OWP.Kpi.LA.Helper;
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
    public async Task<IList<KpiInfo>> GetLAKpiInfo(DateTime fromDate, DateTime toDate)
    {
        var data = FakeData.FakeKpiInfos();
        var isGap = CalculateGapHelper.IsGap(fromDate, toDate);
        return data;
    }

    [HttpGet("get-condition")]
    public IActionResult GetColorExplain()
        => Ok(ConditionConstant.ConditionInfos);
}