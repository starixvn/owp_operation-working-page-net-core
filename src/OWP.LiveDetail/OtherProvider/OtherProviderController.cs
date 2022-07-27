namespace OWP.LiveDetail.OtherProvider
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    [Route("api/live-detail/other-provider")]
    [ApiController]
    [AllowAnonymous]
    [EnableCors("AllowOrigin")]
    public class OtherProviderController : ControllerBase
    {
        private readonly OtherProviderRepository repository;

        public OtherProviderController(OtherProviderRepository repository)
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