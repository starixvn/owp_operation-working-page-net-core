using Newtonsoft.Json;

namespace OWP.WorkingShift.WeeklyWorkingShift.Controllers;

using System.Threading.Tasks;
using Models;
using Repositories;


[Route("api/weekly-working-shift")]
[ApiController]
[EnableCors]
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