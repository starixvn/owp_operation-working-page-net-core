using Fanex.NAP.Client.AspNetCore.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OWP.Constants;
using OWP.ProviderArrangement.Handlers;
using OWP.ProviderArrangement.Models;
using static Fanex.NAP.Core.Business.Constant;

namespace OWP.ProviderArrangement;

[Route("api/[controller]")]
[ApiController]
[EnableCors("AllowOrigin")]
[Authorize]

public class ProviderArrangementController : ControllerBase
{
    private readonly IMediator mediator;

    public ProviderArrangementController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("get-provider")]
    [NapAuth(FunctionKey = FunctionKeys.ProviderArrangement, Policy = Policy.Read)]
    public async Task<IList<ProviderModel>> GetProvider()
    => await mediator.Send(new GetProviderRequest());

    [HttpPost("update-provider-arrangement")]
    public async Task<IActionResult> UpdateProviderArrangement(UpdateProviderArrangementRequest request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }

    [HttpGet("get-log")]
    [NapAuth(FunctionKey = FunctionKeys.ProviderArrangement, Policy = Policy.Read)]
    public async Task<IList<LogDisplayModel>> GetLog()
       => await mediator.Send(new GetLogRequest());

}
