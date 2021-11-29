using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OWP.AutoAssignment.Models.PublicApi;
using System.Collections.Generic;
using System.Threading.Tasks;
using OWP.AutoAssignment.Services.PublicApi;
using OWP.AutoAssignment.Services.SaveResponse;

namespace OWP.AutoAssignment
{
    [Route("api/public/auto-assignment")]
    [ApiController]
    [Authorize]
    [EnableCors("AllowOrigin")]
    public class PublicApiController : ControllerBase
    {
        private readonly GetRequestService getRequestService;
        private readonly SaveRequestService saveRequestService;

        public PublicApiController(
            SaveRequestService saveRequestService,
            GetRequestService getRequestService)
        {
            this.getRequestService = getRequestService;
            this.saveRequestService = saveRequestService;
        }

        [HttpGet]
        [Route("get-new")]
        public new async Task<IEnumerable<RequestSender>> Request()
        {
            var sender = await getRequestService.GetRequest();
            return sender;
        }
    }
}