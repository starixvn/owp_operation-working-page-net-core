namespace OWP.LiveDetail.ReadyTradeStatus
{
    using System.Threading.Tasks;
    using Constants;
    using Fanex.NAP.Client.AspNetCore.Attributes;
    using Fanex.NAP.Core.Business;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/live-detail/ready-trade-status")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    [Authorize]

    public class ReadyTradeStatusController : ControllerBase
    {
        private readonly TraderStatusRepository repository;

        public ReadyTradeStatusController(TraderStatusRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        [Route("")]
        [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Host)]
        public async Task<IActionResult> InsertReadyTradeStatus(int matchId)
        {
            var result = await repository.InsertReadyTradeStatus(matchId);
            var dateTimeResult = result.Any() ? result.FirstOrDefault().ToString("hh:mm tt MMM dd, yyyy") : string.Empty;

            return Ok(dateTimeResult);
        }
    }
}
