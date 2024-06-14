using System.Text.Json;
using Newtonsoft.Json;
using OWP.WorkingShift.Common.Models;
using OWP.WorkingShift.Common.Repositories;

namespace OWP.WorkingShift.Common.Controllers;

[Route("api/working-shift-color")]
[ApiController]
[EnableCors]
[Authorize]

public class WorkingShiftColorApiController : ControllerBase
{
    private readonly WorkingShiftColorRepository repository;

    public WorkingShiftColorApiController(WorkingShiftColorRepository repository)
    {
        this.repository = repository;
    }

    [Route("get-all")]
    [HttpGet]
    [NapAuth(FunctionKey = FunctionKeys.WorkingShiftManagement, Policy = Constant.Policy.Read)]
    public async Task<IActionResult> GetAll()
    {
        var data = await repository.GetAll();
        return new JsonResult(data, new JsonSerializerSettings());
    }

    [Route("validation-create")]
    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.WorkingShiftManagement_ShiftManagement, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> ValidateBeforeCreate(QuickInsertWorkingShiftColorModel model)
    {
        var result = await repository.ValidateBeforeCreateAsync(model.ShiftCode);
        return new JsonResult(result, new JsonSerializerSettings());
    }

    [Route("")]
    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.WorkingShiftManagement_ShiftManagement, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> Insert(QuickInsertWorkingShiftColorModel model)
    {
        var result = await repository.Insert(model);
        return new JsonResult(result, new JsonSerializerSettings());
    }

    [Route("")]
    [HttpPut]
    [NapAuth(FunctionKey = FunctionKeys.WorkingShiftManagement_ShiftManagement, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> Update(WorkingShiftColorModel model)
    {
        var result = await repository.Update(model);

        return new JsonResult(result, new JsonSerializerSettings());
    }

    [Route("validation-delete")]
    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.WorkingShiftManagement_ShiftManagement, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> ValidateBeforeDelete(WorkingShiftColorModel model)
    {
        var result = await repository.ValidateBeforeDeleteAsync(model.ShiftId);
        return new JsonResult(result, new JsonSerializerSettings());
    }

    [Route("")]
    [HttpDelete]
    [NapAuth(FunctionKey = FunctionKeys.WorkingShiftManagement_ShiftManagement, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> Delete(WorkingShiftColorModel model)
    {
        var result = await repository.Delete(model.ShiftId);

        return new JsonResult(result, new JsonSerializerSettings());
    }

    [Route("get-all-gmt8")]
    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.WorkingShiftManagement, Policy = Constant.Policy.Read)]
    public async Task<IActionResult> GetAllGmt8()
    {
        var data = await repository.GetAllGmt8();
        return new JsonResult(data, new JsonSerializerSettings());
    }

    private async Task<string> InsertWS(string item)
    {
        try
        {
            await repository.Insert(new QuickInsertWorkingShiftColorModel
            {
                ShiftCode = item.ToUpperInvariant(),
                Background = "#3399cc",
                Color = "#FFFFFF"
            });

            return item + "_" + "Ok";
        }
        catch (Exception ex)
        {
            return item + "_" + ex.Message;
        }
    }

    [Route("buck_insert")]
    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.WorkingShiftManagement, Policy = Constant.Policy.Read)]
    public async Task<IActionResult> BuckInsertWorkingShift([FromBody] IEnumerable<string> models)
    {
        if (models == null || !models.Any())
        {
            return new JsonResult("Data not found", new JsonSerializerSettings());
        }

        var getAll = await repository.GetAll();

        var list = new List<string>();

        foreach (var item in models)
        {
            var findExist = getAll.FirstOrDefault(x => x.ShiftCode.Equals(item.ToUpperInvariant()));
            if (findExist != null)
            {
                list.Add(item + " is existed");
            }
            else
            {
                var result = await repository.ValidateBeforeCreateAsync(item);

                list.Add(result.Code >= 0 ? await InsertWS(item) : item + " validate_" + result.Code);
            }
        }

        return new JsonResult(list, new JsonSerializerSettings());
    }
}