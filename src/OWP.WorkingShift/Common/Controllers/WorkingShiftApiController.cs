using Newtonsoft.Json;
using OWP.WorkingShift.Common.Repositories;
using OWP.WorkingShift.Common.Utils;

namespace OWP.WorkingShift.Common.Controllers;


[Route("api/working-shift")]
[ApiController]
[Authorize]
public class WorkingShiftApiController : ControllerBase
{
    private readonly WorkingShiftRepository repository;

    public WorkingShiftApiController(WorkingShiftRepository repository)
    {
        this.repository = repository;
    }

    [Route("get-user")]
    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.WorkingShiftManagement, Policy = Constant.Policy.Read)]
    public async Task<IActionResult> GetUserWorkingShift(WorkingShiftQueryByUserModel model)
    {
        var result = await repository.QueryByUser(model);
        return new JsonResult(result, new JsonSerializerSettings());
    }

    [Route("get-single-user")]
    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.WorkingShiftManagement, Policy = Constant.Policy.Read)]
    public async Task<IActionResult> GetSingleUserWorkingShift(WorkingShiftQueryBySingleUserModel model)
    {
        var newModel = new WorkingShiftQueryByUserModel
        {
            UserIds = model.UserId.ToString(),
            FromDate = model.FromDate,
            ToDate = model.ToDate
        };
        var result = await repository.QueryByUser(newModel);
        return new JsonResult(result.ToArray(model.FromDate.Value, model.ToDate.Value), new JsonSerializerSettings());
    }

    [Route("copy-previous")]
    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.WorkingShiftManagement, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> CopyWorkingShiftWeeklyPrevious(CopyWorkingShiftWeeklyPreviousModel model)
    {
        await repository.CopyWorkingShiftWeeklyPrevious(model);
        var queryModel = new WorkingShiftQueryByUserModel
        {
            UserIds = model.UserId.ToString(),
            FromDate = model.StartDateOfWeek.Value,
            ToDate = model.StartDateOfWeek.Value.AddDays(6)
        };
        var result = await repository.QueryByUser(queryModel);
        return new JsonResult(result.ToArray(queryModel.FromDate.Value, queryModel.ToDate.Value), new JsonSerializerSettings());
    }

    [Route("copy-whole-shifts")]
    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.WorkingShiftManagement, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> CopyWholeShifts(CopyWholeShiftsModel model)
    {
        await repository.CopyWholeShifts(model);
        var queryModel = new WorkingShiftQueryByUserModel
        {
            UserIds = model.ToUserId.ToString(),
            FromDate = model.FromDate.Value,
            ToDate = model.FromDate.Value.AddDays(6)
        };
        var result = await repository.QueryByUser(queryModel);
        return new JsonResult(result.ToArray(queryModel.FromDate.Value, queryModel.ToDate.Value), new JsonSerializerSettings());
    }

    [Route("delete-whole-shifts")]
    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.WorkingShiftManagement, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> DeleteWholeShifts(DeleteWholeShiftsModel model)
    {
        var result = await repository.DeleteWholeShifts(model);
        return new JsonResult(result, new JsonSerializerSettings());
    }

    [Route("copy-single-shift")]
    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.WorkingShiftManagement, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> CopySingleShift(CopySingleShiftModel model)
    {
        var result = await repository.CopySingleShift(model);
        var queryModel = new WorkingShiftQueryByUserModel
        {
            UserIds = model.UserId.ToString(),
            FromDate = model.ShiftDate,
            ToDate = model.ShiftDate
        };

        var queryResult = await repository.QueryByUser(queryModel);
        result.Data = queryResult.FirstOrDefault();

        return new JsonResult(result, new JsonSerializerSettings());
    }

    [Route("remove-single-shift")]
    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.WorkingShiftManagement, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> RemoveSingleShift(RemoveSingleShiftModel model)
    {
        var result = await repository.RemoveSingleShift(model);

        var queryModel = new WorkingShiftQueryByUserModel
        {
            UserIds = model.EmployeeId.ToString(),
            FromDate = model.ShiftDate,
            ToDate = model.ShiftDate
        };

        var queryResult = await repository.QueryByUser(queryModel);
        result.Data = queryResult.FirstOrDefault();

        return new JsonResult(result, new JsonSerializerSettings());
    }

    [Route("update-single-shift")]
    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.WorkingShiftManagement, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> UpdateSingleShift(UpdateWorkingShiftCodeModel model)
    {
        var result = await repository.UpdateWorkingShiftCode(model);

        var queryModel = new WorkingShiftQueryByUserModel
        {
            UserIds = model.EmployeeId.ToString(),
            FromDate = model.ShiftDate,
            ToDate = model.ShiftDate
        };

        var queryResult = await repository.QueryByUser(queryModel);
        result.Data = queryResult.FirstOrDefault();

        return new JsonResult(result, new JsonSerializerSettings());
    }
}