namespace OWP.LiveDetail.Television
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    [Route("api/live-detail/television")]
    [ApiController]
    [AllowAnonymous]
    [EnableCors("AllowOrigin")]
    public class TelevisionController : ControllerBase
    {
        private readonly TelevisionRepository repository;

        public TelevisionController(TelevisionRepository repository)
        {
            this.repository = repository;
        }

        [Route("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await repository.GetAll();
            return Ok(JsonConvert.SerializeObject(result));
        }
    }
}