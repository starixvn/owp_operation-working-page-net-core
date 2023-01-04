using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OWP.ProviderArrangement.Handlers;
using OWP.ProviderArrangement.Models;

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
    public async Task<IList<ProviderModel>> GetProvider()
    => await mediator.Send(new GetProviderRequest());

}
