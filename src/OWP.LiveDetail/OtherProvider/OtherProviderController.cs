namespace OWP.LiveDetail.OtherProvider
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/live-detail/other-provider")]
    [ApiController]
    [Authorize]
    [EnableCors("AllowOrigin")]
    public class OtherProviderController : ControllerBase
    {
        private readonly OtherProviderRepository repository;

        public OtherProviderController(OtherProviderRepository repository)
        {
            this.repository = repository;
        }

        [Route("all")]
        public async Task<IEnumerable<OtherProvider>> GetAll()
        {
            return await repository.GetAll();
        }
    }
}