namespace OWP.OvertimeReport
{
    using Handlers;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    [Authorize]
    public class OvertimeReportController : ControllerBase
    {
        private readonly IMediator mediator;

        public OvertimeReportController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Route("get-report-by-month")]
        [HttpPost]
        public async Task<IEnumerable<OvertimeByMonth>> GetReportByMonth(QueryReportByMonthRequest request)
        {
            return await mediator.Send(new QueryReportByMonthRequest());
        }

        [Route("get-report-by-year")]
        [HttpPost]
        public async Task<IEnumerable<OvertimeByYear>> GetReportByYear(QueryReportByYearRequest request)
        {
            return await mediator.Send(new QueryReportByYearRequest());
        }
    }
}
