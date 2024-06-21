using Newtonsoft.Json;
using QueryModel = OWP.WorkingShift.DailyWorkingShift.Models.QueryModel;
using Repository = OWP.WorkingShift.DailyWorkingShift.Repositories.Repository;

namespace OWP.WorkingShift.DailyWorkingShift.Controllers;

[Route("api/daily-working-shift")]
[ApiController]
[EnableCors]
[Authorize]
public class DailyWorkingShiftApiController : ControllerBase
{
    private readonly Repository repository;
    private readonly ModelService modelService;
    private readonly INapSession currentSession;

    public DailyWorkingShiftApiController(Repository repository, ModelService modelService, INapSession currentSession)
    {
        this.repository = repository;
        this.modelService = modelService;
        this.currentSession = currentSession;
    }

    [Route("query")]
    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.WorkingShiftManagement, Policy = Constant.Policy.Host)]
    public async Task<ActionResult<JsonResult>> SearchWorkingShiftDaily(QueryModel model)
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