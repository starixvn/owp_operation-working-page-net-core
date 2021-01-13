using System.IO;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OWP.Helpers;
using OWP.OtherSport.fake;
using OWP.OtherSport.Submit;
using OWP.OtherSport.Validation;

namespace OWP.OtherSport
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    [AllowAnonymous]
    public class OtherSportController : ControllerBase
    {
        private readonly IMediator mediator;

        public OtherSportController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Route("")]
        [HttpPost]      
        public async Task<IActionResult> Query(QueryModel request)
        {
           
        }

        [Route("validate")]
        [HttpPost]
        public async Task<IActionResult> Validate()
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                return new UnsupportedMediaTypeResult();
            }

            if (Request.Form.Files != null && Request.Form.Files.Count == 1)
            {
                using var ms = new MemoryStream();
                Request.Form.Files[0].CopyTo(ms);

                var request = new ValidationHandler.Request
                {
                    Stream = ms
                };
                var result = await mediator.Send(request);

                return new JsonResult(result);
            }

            return BadRequest();
        }

        [Route("submit")]
        [HttpPost]
        public async Task<IActionResult> Submit(SubmitRequest request)
        {
            var viewModel = await mediator.Send(request);
            return new JsonResult(viewModel);
        }

        [Route("get-trader-assignment")]
        [HttpPost]
        public async Task<IActionResult> GetTraderAssignment()
        {
            var response= FakeData.FakeAssignment();
            return new JsonResult(response);
        }
    }
}