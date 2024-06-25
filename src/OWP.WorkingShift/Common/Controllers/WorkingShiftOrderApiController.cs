using Newtonsoft.Json;
using OWP.WorkingShift.Common.Repositories;
using ModelService = OWP.WorkingShift.Common.Services.ModelService;

namespace OWP.WorkingShift.Common.Controllers;

[Route("api/working-shift-order")]
[ApiController]
[Authorize]
public class WorkingShiftOrderApiController : ControllerBase
{
    private readonly WorkingShiftOrderRepository repository;
    private readonly ModelService modelService;

    public WorkingShiftOrderApiController(WorkingShiftOrderRepository repository, ModelService modelService)
    {
        this.repository = repository;
        this.modelService = modelService;
    }

    [Route("")]
    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.WorkingShiftManagement, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> UpdateEmployeeWorkingShiftOrder(UpdateEmployeeWorkingShiftOrderModel model)
    {
        model.OrderUserId = modelService.GetOrderUserId();
        var result = await repository.UpdateEmployeeWorkingShiftOrder(model);
        return new JsonResult(result, new JsonSerializerSettings());
    }
}