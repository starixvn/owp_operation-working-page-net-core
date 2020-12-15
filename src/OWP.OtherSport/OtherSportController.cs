using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OWP.OtherSport
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtherSportController : ControllerBase
    {
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