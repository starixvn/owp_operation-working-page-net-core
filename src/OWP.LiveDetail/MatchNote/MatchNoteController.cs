namespace OWP.LiveDetail.MatchNote
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

    [Route("api/live-detail/match-note")]
    [ApiController]
    [Authorize]
    [EnableCors("AllowOrigin")]
    public class MatchNoteController : ControllerBase
    {
        private readonly MatchNoteRepository repository;
        //private readonly NotificationService notificationService;

        public MatchNoteController(MatchNoteRepository repository)
        {
            this.repository = repository;
            //this.notificationService = notificationService;
        }

        [HttpGet]
        [Route("")]
        [NapAuth(FunctionKey = FunctionKeys.Note, Policy = Constant.Policy.Read)]
        public async Task<IEnumerable<MatchNoteModel>> Query([FromQuery] QueryModel model)
        {
            return await repository.Query(model);
        }

        [HttpPost]
        [Route("")]
        [NapAuth(FunctionKey = FunctionKeys.Note, Policy = Constant.Policy.Host)]
        public async Task<CreateResult> Create(CreateModel model)
        {
            var result = await repository.Create(model);
            //await notificationService.Notify(result.SessionId);
            return result;
        }

        [HttpPut]
        [Route("")]
        [NapAuth(FunctionKey = FunctionKeys.Note, Policy = Constant.Policy.Host)]
        public async Task Update(UpdateModel model)
        {
            var sessionId = await repository.Update(model);
            //await notificationService.Notify(sessionId);
        }

        [HttpDelete]
        [Route("")]
        [NapAuth(FunctionKey = FunctionKeys.Note, Policy = Constant.Policy.Host)]
        public async Task Delete(DeleteModel model)
        {
            var sesionId = await repository.Delete(model);
            //await notificationService.Notify(sesionId);
        }
    }
}