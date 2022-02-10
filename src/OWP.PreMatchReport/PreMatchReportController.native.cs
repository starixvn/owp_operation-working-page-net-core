namespace OWP.PreMatchReport
{
    using System.Reflection.Metadata;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using MediatR;
    using Handlers;
    using Models;

    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    [EnableCors("AllowOrigin")]
    public class PreMatchReportController : ControllerBase
    {
        private readonly IMediator mediator;

        public PreMatchReportController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost("get-prematch-report")]
        public async Task<IEnumerable<ReportModel>> GetPreMatchReport(ReportRequest request)
        {
            return await mediator.Send(request);
        }   
    }
}