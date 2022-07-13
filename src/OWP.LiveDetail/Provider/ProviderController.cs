namespace OWP.LiveDetail.Provider
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/live-detail/provider")]
    [ApiController]
    [Authorize]
    [EnableCors("AllowOrigin")]
    public class ProviderController : ControllerBase
    {
        private readonly ProviderRepository repository;

        public ProviderController(ProviderRepository repository)
        {
            this.repository = repository;
        }

        [Route("all")]
        public async Task<IEnumerable<Provider>> GetAll()
        {
            return await repository.GetAll();
        }
    }
}