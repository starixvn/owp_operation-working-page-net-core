namespace OWP.LeagueManagement
{
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;
    using OWP.LeagueManagement.Handlers;
    using OWP.Models;

    [Route("api/league-management")]
    [Authorize]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class LeagueApiController : ControllerBase
    {
        private readonly IMediator mediator;

        public LeagueApiController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet]
        [Route("get-league-group")]
        public async Task<IEnumerable<ReferenceResource>> GetLeagueGroup()
            => await mediator.Send(new GetLeagueGroupRequest());

        [HttpPost]
        [Route("update-tv-list")]
        public async Task<IActionResult> UpdateTvList([FromBody] IEnumerable<TVList> models)
        {
            var request = new UpdateTvListRequest { Models = models };
            var result = await mediator.Send(request);
            return Ok(result);
        }

        [HttpPost]
        [Route("update-condition")]
        public async Task<IActionResult> UpdateCondition([FromBody] LeagueSettingCondition models)
        {
            var request = new UpdateConditionRequest { Models = models };
            var result = await mediator.Send(request);
            return Ok(result);
        }

        [HttpGet]
        [Route("get-condition")]
        public async Task<LeagueSettingCondition> GetCondition(GetConditionRequest request)
        => await mediator.Send(request);

        [Route("viewlog")]
        [HttpPost]
        public async Task<IList<LogDisplayModel>> ViewLog([FromBody] QueryLogModel model)
        {
            var request = new ViewLogRequest
            {
                Model = model
            };
            return await mediator.Send(request);
        }

        [Route("update-special-product")]
        [HttpPost]
        public async Task<ProductInfo> UpdateSpecialProduct([FromBody] SpecialProduct model)
        {
            var request = new UpdateSpecialProductRequest
            {
                Model = model
            };
            return await mediator.Send(request);
        }

        [Route("get-all-league")]
        [HttpPost]
        public async Task<IList<LeagueInfoResponse>> GetAllLeague([FromBody] LeagueInfoQuery query)
        {
            var request = new GetAllLeagueRequest
            {
                Query = query
            };
            return await mediator.Send(request);
        }
    }
}