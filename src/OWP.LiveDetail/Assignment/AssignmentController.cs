namespace OWP.LiveDetail.Assignment
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common;
    using Constants;
    using Fanex.NAP.Client.AspNetCore.Attributes;
    using Fanex.NAP.Core.Business;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Services;


    [ApiController]
    //[AllowAnonymous]
    [Authorize]
    [EnableCors("AllowOrigin")]
    [Route("api/live-detail/assignment")]
    public class AssignmentController : ControllerBase
    {
        private readonly AssignmentRepository repository;
        //private readonly NotificationService notificationService;

        public AssignmentController(AssignmentRepository repository)
        {
            this.repository = repository;
            //this.notificationService = notificationService;
        }

        [HttpGet]
        [Route("")]
        [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Read)]
        public async Task<IEnumerable<Assignment>> Query([FromQuery] QueryModel model)
        {
            return await repository.Query(model);
        }

        [HttpPost]
        [Route("validate")]
        [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Host)]
        public async Task<ValidateResult> Validate(ValidateModel model)
        {
            var validateResult = await repository.Validate(model);

            return validateResult;
        }

        [HttpPost]
        [Route("")]
        [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Host)]
        public async Task<CommandResult> Create(CreateModel model)
        {
            var createdResult = await repository.Create(model);
            if (createdResult.Code == 0)
            {
                //await notificationService.Notify(createdResult.SessionId);
            }

            return createdResult;
        }

        [HttpPut]
        [Route("")]
        [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Host)]
        public async Task<CommandResult> Update(UpdateModel model)
        {
            var updatedResult = await repository.Update(model);
            if (updatedResult.Code == 0)
            {
                //await notificationService.Notify(updatedResult.SessionId);
            }

            return updatedResult;
        }

        [HttpDelete]
        [Route("")]
        [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Host)]
        public async Task<CommandResult> Delete([FromQuery] DeleteModel model)
        {
            var deleteResult = await repository.Delete(model);
            if (deleteResult.Code == 0)
            {
                //await notificationService.Notify(deleteResult.SessionId);
            }

            return deleteResult;
        }
    }
}