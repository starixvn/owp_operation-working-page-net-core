namespace OWP.LiveDetail.User
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/live-detail/user")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    [Authorize]
    public class LiveDetailUserController : ControllerBase
    {
        private readonly UserRepository repository;

        public LiveDetailUserController(UserRepository repository)
        {
            this.repository = repository;
        }

        [Route("suggestion")]
        [HttpGet]
        public async Task<IEnumerable<UserSuggestionEntity>> GetSuggestion([FromQuery] GetSuggestionModel model)
        {
            return await repository.GetSuggestion(model);
        }

        [Route("all")]
        [HttpGet]
        public async Task<IEnumerable<UserInfoModel>> GetAll()
        {
            return await repository.GetAll();
        }

        [Route("info")]
        [HttpGet]
        public async Task<UserInfoModel> GetUserInfo(string name)
        {
            return await repository.GetUserInfo(name);
        }
    }
}