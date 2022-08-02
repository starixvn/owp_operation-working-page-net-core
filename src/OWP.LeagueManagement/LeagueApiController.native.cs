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
    using Newtonsoft.Json;

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
        public async Task<IActionResult> GetLeagueGroup()
        {
            var result = await mediator.Send(new GetLeagueGroupRequest());
            return Ok(JsonConvert.SerializeObject(result));
        }

        [HttpPost]
        [Route("update-tv-list")]
        public async Task<IActionResult> UpdateTvList([FromBody] IEnumerable<TVList> models)
        {
            var request = new UpdateTvListRequest { Models = models };
            var result = await mediator.Send(request);
            return Ok(JsonConvert.SerializeObject(result));
        }

        [HttpPost]
        [Route("update-condition")]
        public async Task<IActionResult> UpdateCondition([FromBody] LeagueSettingCondition models)
        {
            var request = new UpdateConditionRequest { Models = models };
            var result = await mediator.Send(request);
            return Ok(JsonConvert.SerializeObject(result));
        }

        [HttpGet]
        [Route("get-condition")]
        public async Task<IActionResult> GetCondition([FromQuery] int LeagueSettingId)
        {
            var request = new GetConditionRequest
            {
                LeagueSettingId = LeagueSettingId
            };
            var result = await mediator.Send(request);
            return Ok(JsonConvert.SerializeObject(result));
        }

        [Route("viewlog")]
        [HttpPost]
        public async Task<IActionResult> ViewLog([FromBody] QueryLogModel model)
        {
            var request = new ViewLogRequest
            {
                Model = model
            };
            var result = await mediator.Send(request);
            return Ok(JsonConvert.SerializeObject(result));
        }

        [Route("update-special-product")]
        [HttpPost]
        public async Task<IActionResult> UpdateSpecialProduct([FromBody] SpecialProduct model)
        {
            var request = new UpdateSpecialProductRequest
            {
                Model = model
            };
            var result = await mediator.Send(request);
            return Ok(JsonConvert.SerializeObject(result));
        }

        [Route("get-all-league")]
        [HttpPost]
        public async Task<IActionResult> GetAllLeague([FromBody] LeagueInfoQuery query)
        {
            var request = new GetAllLeagueRequest
            {
                Query = query
            };
            var result = await mediator.Send(request);
            return Ok(JsonConvert.SerializeObject(result));
        }
    }
}