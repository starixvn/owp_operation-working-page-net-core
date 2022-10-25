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
    using Newtonsoft.Json;
    using RabbitMQ;
    using RabbitMQ.Constants;
    using Services;


    [ApiController]
    [Authorize]
    [EnableCors("AllowOrigin")]
    [Route("api/live-detail/assignment")]
    public class AssignmentController : ControllerBase
    {
        private readonly AssignmentRepository repository;

        private readonly MessageClient messageClient;

        public AssignmentController(AssignmentRepository repository, MessageClient messageClient)
        {
            this.repository = repository;
            this.messageClient = messageClient;
        }

        [HttpGet]
        [Route("")]
        [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Read)]
        public async Task<IActionResult> Query([FromQuery] QueryModel model)
        {
            var result = await repository.Query(model);
            return Ok(JsonConvert.SerializeObject(result));
        }

        [HttpPost]
        [Route("validate")]
        [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Host)]
        public async Task<IActionResult> Validate(ValidateModel model)
        {
            var validateResult = await repository.Validate(model);
            return Ok(JsonConvert.SerializeObject(validateResult));
        }

        [HttpPost]
        [Route("")]
        [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Host)]
        public async Task<IActionResult> Create(CreateModel model)
        {
            var createdResult = await repository.Create(model);
            if (createdResult.Code == 0)
            {
                messageClient.SendMessage(new { SessionId = createdResult.SessionId }, RoutingKey.LiveDetail);
            }


            return Ok(JsonConvert.SerializeObject(createdResult));
        }

        [HttpPut]
        [Route("")]
        [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Host)]
        public async Task<IActionResult> Update(UpdateModel model)
        {
            var updatedResult = await repository.Update(model);
            if (updatedResult.Code == 0)
            {
                messageClient.SendMessage(new { SessionId = updatedResult.SessionId }, RoutingKey.LiveDetail);
            }

            return Ok(JsonConvert.SerializeObject(updatedResult));
        }

        [HttpDelete]
        [Route("")]
        [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Host)]
        public async Task<IActionResult> Delete([FromQuery] DeleteModel model)
        {
            var deleteResult = await repository.Delete(model);
            if (deleteResult.Code == 0)
            {
                messageClient.SendMessage(new { SessionId = deleteResult.SessionId }, RoutingKey.LiveDetail);
            }

            return Ok(JsonConvert.SerializeObject(deleteResult));
        }
    }
}