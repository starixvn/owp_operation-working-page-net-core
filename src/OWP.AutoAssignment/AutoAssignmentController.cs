using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OWP.AutoAssignment.Constant;
using OWP.AutoAssignment.Models.InternalApi;
using OWP.AutoAssignment.Models.PublicApi;
using OWP.AutoAssignment.Services.InternalApi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OWP.AutoAssignment
{
    [Route("api/auto-assignment")]
    [ApiController]
    [Authorize]
    [EnableCors("AllowOrigin")]
    public class AutoAssignmentController : ControllerBase
    {
        private readonly CreateRequestService createRequestService;

        public AutoAssignmentController(CreateRequestService createRequestService)
        {
            this.createRequestService = createRequestService;
        }

        [Route("remove-request")]
        [HttpPost]
        public async Task RemoveRequest(CheckRequestModel model)
          => await createRequestService.RemoveRequest(model.RequestId);

        [Route("get-request")]
        [HttpGet]
        public async Task<IEnumerable<SchedulingRequest>> GetRequests()
            => await createRequestService.GetRequests();

        [Route("check-request")]
        [HttpPost]
        public async Task<int> CheckRequest(CheckRequestModel model)
            => await createRequestService.GetRequestStatus(model.RequestId);

        [Route("create-request")]
        [HttpPost]
        public async Task<IActionResult> CreateRequest(CreateRequestModel model)
        {
            var existsMatchValid = await createRequestService.ExistMatchValid(model.EventDate);
            var existsTraderValid = await createRequestService.ExistsSuspenderValid(model.EventDate);
            var requestIdIsWaiting = await createRequestService.GetRequestIsWaiting(model.EventDate);
            if (!string.IsNullOrWhiteSpace(requestIdIsWaiting))
            {
                var message = string.Format(AutoConstant.ExistsRequestIdIsWaiting, requestIdIsWaiting);
                return BadRequest(message);
            }
            if (!existsMatchValid)
            {
                return BadRequest(AutoConstant.NoFindAnyMatchValidOnSelectedDate);
            }
            if (!existsTraderValid)
            {
                return BadRequest(AutoConstant.NoFindAnyTraderValidOnSelectedDate);
            }
            await createRequestService.CreateRequest(model);
            return Ok();
        }
    }
}