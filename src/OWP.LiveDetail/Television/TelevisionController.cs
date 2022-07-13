namespace OWP.LiveDetail.Television
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/live-detail/television")]
    [ApiController]
    [Authorize]
    [EnableCors("AllowOrigin")]
    public class TelevisionController : ControllerBase
    {
        private readonly TelevisionRepository repository;

        public TelevisionController(TelevisionRepository repository)
        {
            this.repository = repository;
        }

        [Route("all")]
        public async Task<IEnumerable<Television>> GetAll()
        {
            return await repository.GetAll();
        }
    }
}