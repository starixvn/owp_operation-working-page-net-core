namespace OWP.LiveDetail.Agent
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/live-detail/agent")]
    [ApiController]
    [Authorize]
    [EnableCors("AllowOrigin")]
    public class AgentController : ControllerBase
    {
        private readonly AgentRepository repository;

        public AgentController(AgentRepository repository)
        {
            this.repository = repository;
        }

        [Route("all")]
        public async Task<IEnumerable<Agent>> GetAll()
        {
            return await repository.GetAll();
        }
    }
}