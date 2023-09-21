using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OWP.Kpi.LA.Constant;
using OWP.Kpi.LA.Handlers;
using OWP.Kpi.LA.ViewModels;

namespace OWP.Kpi.LA;

[Route("api/kpi/la")]
[ApiController]
[Authorize]
[EnableCors]
public class LAKpiController : ControllerBase
{
    private readonly IMediator mediator;

    public LAKpiController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [Route("")]
    [HttpGet]
    public async Task<DataViewModel> GetLAKpiInfo(DateTime fromDate, DateTime toDate)
    {
        var data = await mediator.Send(new CalculateKpiRequest
        {
            FromDate = fromDate,
            ToDate = toDate
        });

        return data;
    }

    [HttpGet("get-condition")]
    public IActionResult GetColorExplain()
        => Ok(ConditionConstant.ConditionInfos);
}