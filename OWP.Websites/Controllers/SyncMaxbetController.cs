using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OWP.API.SyncMaxbet.Services;

namespace OWP.Websites.Controllers
{
    [Route("api/sync-maxbet")]
    [ApiController]
    [AllowAnonymous]
    public class SyncMaxbetController : ControllerBase
    {

        private readonly MaxbetService maxbetService;

        public SyncMaxbetController(MaxbetService maxbetService)
        {
            this.maxbetService = maxbetService;
        }

        [HttpGet]
        [Route("{id}")]
        public JsonResult GetMaxbet(int id)
        {
            var maxbetInfo = maxbetService.GetMaxbetInfo(id);
            return new JsonResult(maxbetInfo);
        }
    }
}
