using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OWP.Constants;
using OWP.ProviderVerification.Handlers;
using OWP.ProviderVerification.Models;

namespace OWP.ProviderVerification;

[Route("api/[controller]")]
[ApiController]
[EnableCors]
[Authorize]
public class ProviderVerificationController : ControllerBase
{
    private readonly IMediator mediator;

    public ProviderVerificationController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("get-provider-verification")]
    public async Task<IList<ProviderVerificationModel>> GetProviderVerification()
        => await mediator.Send(new GetProviderVerificationRequest());
}