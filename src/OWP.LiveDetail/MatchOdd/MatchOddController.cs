namespace OWP.LiveDetail.MatchOdd
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Constants;
    using Fanex.NAP.Client.AspNetCore.Attributes;
    using Fanex.NAP.Core.Business;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Services;

    [Route("api/live-detail/match-odd")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    [Authorize]
    public class MatchOddController : ControllerBase
    {
        private readonly MatchOddRepository repository;
        private readonly NotificationService notificationService;

        public MatchOddController(MatchOddRepository repository, NotificationService notificationService)
        {
            this.repository = repository;
            this.notificationService = notificationService;
        }

        [HttpGet]
        [Route("")]
        [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Read)]
        public async Task<IEnumerable<MatchOdd>> Query([FromQuery] QueryModel model)
        {
            return await repository.Query(model);
        }

        [HttpPost]
        [Route("")]
        [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Host)]
        public async Task<CreateResult> Create(CreateModel model)
        {
            var result = await repository.Create(model);
            await notificationService.Notify(result.SessionId);
            return result;
        }

        [HttpPut]
        [Route("")]
        [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Host)]
        public async Task<ResponseResult> Update(UpdateModel model)
        {
            var result = await repository.Update(model);
            await notificationService.Notify(result.SessionId);
            return result;
        }

        [HttpDelete]
        [Route("")]
        [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Host)]
        public async Task<ResponseResult> Delete(DeleteModel model)
        {
            var result = await repository.Delete(model);
            await notificationService.Notify(result.SessionId);
            return result;
        }
    }
}