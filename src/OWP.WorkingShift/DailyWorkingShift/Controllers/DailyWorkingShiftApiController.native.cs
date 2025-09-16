using Fanex.NAP.Client;
using Newtonsoft.Json;
using QueryModel = OWP.WorkingShift.DailyWorkingShift.Models.QueryModel;
using Repository = OWP.WorkingShift.DailyWorkingShift.Repositories.Repository;

namespace OWP.WorkingShift.DailyWorkingShift.Controllers;

[Route("api/daily-working-shift")]
[ApiController]
[Authorize]
public class DailyWorkingShiftApiController(
    Repository repository,
    ModelService modelService,
    INapSession currentSession)
    : ControllerBase
{
    [Route("query")]
    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.WorkingShiftManagement, Policy = Constant.Policy.Host)]
    public async Task<JsonResult> SearchWorkingShiftDaily(QueryModel model)
    {
        var permission = modelService.BuildPermissionViewModel();
        model.Permission = permission;

        if (!model.Permission.DailyWorkingShiftAllSections.IsRead && !model.Permission.DailyWorkingShiftOwnSection.IsRead)
        {
            model.User = currentSession.UserName;
        }

        var shift = await repository.Query(model);

        return new JsonResult(shift, new JsonSerializerSettings());
    }
}