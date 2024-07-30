using Newtonsoft.Json;

namespace OWP.WorkingShift.WeeklyWorkingShift.Controllers;

using Models;
using Repositories;
using System.Threading.Tasks;


[Route("api/weekly-working-shift")]
[ApiController]
[Authorize]
public class WeeklyWorkingShiftIFrameApiController : ControllerBase
{
    private readonly Repository weeklyWorkingShiftRepository;

    public WeeklyWorkingShiftIFrameApiController(Repository weeklyWorkingShiftRepository)
    {
        this.weeklyWorkingShiftRepository = weeklyWorkingShiftRepository;
    }


    [Route("search-iframe")]
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> SearchWorkingShiftWeeklyIFrame(QueryModel model)
    {
        model.IsFrame = true;
        model.OrderUserId = 0;
        var result = await weeklyWorkingShiftRepository.SearchWorkingShiftWeekly(model);
        return new JsonResult(result, new JsonSerializerSettings());
    }

}