using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OWP.OtherSport
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class OtherSportController : ControllerBase
    {

        [Route("test")]
        [HttpGet]
        public async Task<IActionResult> Test()
        {
            return new JsonResult("Test");
        }

        [Route("validate")]
        [HttpPost]
        public async Task<IActionResult> Validate()
        {
            return Ok();
        }

        [Route("submit")]
        [HttpGet]
        public async Task<IActionResult> Submit()
        {
            return Ok();
        }
    }
}