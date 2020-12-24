using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OWP.OtherSport.Submit;
using OWP.OtherSport.Validation.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static OWP.OtherSport.Validation.ValidationHandler;

namespace OWP.OtherSport
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    [EnableCors("AllowOrigin")]
    public class OtherSportController : ControllerBase
    {
        private readonly IMediator mediator;

        public OtherSportController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Route("test")]
        [HttpGet]
        public async Task<IActionResult> Test()
        {
            return new JsonResult("Test");
        }

        [Route("validate")]
        [HttpPost]
        public IActionResult Validate()
        {
            // Mock data response
            var validationResult = new Response
            {
                ValidationResults = new List<AssignmentValidationRowResult>
                {
                    new AssignmentValidationRowResult
                    {
                        Row= 1,
                        Message= "KO time invalid"
                    },
                     new AssignmentValidationRowResult
                    {
                        Row= 2,
                        Message= "KO time invalid1"
                    }
                }
            };

            return new JsonResult(new Response());
        }

        [Route("submit")]
        [HttpPost]
        public async Task<IActionResult> Submit(SubmitRequest request)
        {
            var viewModel = await mediator.Send(request);
            return new JsonResult(viewModel);
        }
    }
}