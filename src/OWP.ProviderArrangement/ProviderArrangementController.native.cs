using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OWP.Constants;
using OWP.ProviderArrangement.Handlers;
using OWP.ProviderArrangement.Models;

namespace OWP.ProviderArrangement;

using Fanex.NAP.Client.Attributes;
using Fanex.NAP.Client.Helpers.Business;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class ProviderArrangementController : ControllerBase
{
    private readonly IMediator mediator;

    public ProviderArrangementController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("get-provider")]
    [NapAuth(FunctionKey = FunctionKeys.ProviderArrangement, Policy = Constant.Policy.Read)]
    public async Task<IList<ProviderModel>> GetProvider()
    => await mediator.Send(new GetProviderRequest());

    [HttpPost("update-provider-arrangement")]
    public async Task<IActionResult> UpdateProviderArrangement(UpdateProviderArrangementRequest request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }

    [HttpGet("get-log")]
    [NapAuth(FunctionKey = FunctionKeys.ProviderArrangement, Policy = Constant.Policy.Read)]
    public async Task<IList<LogDisplayModel>> GetLog()
       => await mediator.Send(new GetLogRequest());

}
