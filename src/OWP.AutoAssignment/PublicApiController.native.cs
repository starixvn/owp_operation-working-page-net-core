using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OWP.AutoAssignment.Services.PublicApi;
using OWP.AutoAssignment.Services.SaveResponse;
using OWP.Models;

namespace OWP.AutoAssignment
{
    [Route("api/public/auto-assignment")]
    [ApiController]
    [AllowAnonymous]
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
        public new async Task<IActionResult> Request()
        {
            var sender = await getRequestService.GetRequest();
            return Ok(JsonConvert.SerializeObject(sender));
        }

        [HttpPost]
        [Route("save-request")]
        public async Task<ResultDataObject> SaveRequest(HttpRequestMessage request)
          => await saveRequestService.Save(request);
    }
}
