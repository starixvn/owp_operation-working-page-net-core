using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OWP.Kpi.LA.Constant;
using OWP.Kpi.LA.Handlers;
using OWP.Kpi.LA.Models;
using OWP.Kpi.LA.ViewModels;
using OWP.Services;

namespace OWP.Kpi.LA;

[Route("api/kpi/la")]
[ApiController]
[Authorize]
[EnableCors]
public class LAKpiController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly RefResourceService refResourceService;

    public LAKpiController(IMediator mediator, RefResourceService refResourceService)
    {
        this.mediator = mediator;
        this.refResourceService = refResourceService;
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

    [Route("get-level-option")]
    [HttpGet]
    public async Task<IEnumerable<Option>> GetLocations()
    {
        var result = await refResourceService.GetRefResourcesByName("LaKpiLevel");

        return result.Select(x => new Option { Label = x.Value, Value = x.ResourceId });
    }

    [HttpPost("update-target-level")]
    public async Task UpdateTargetLevel(UpdateTargetLevelRequest request)
    {
        await mediator.Send(request);
    }
}