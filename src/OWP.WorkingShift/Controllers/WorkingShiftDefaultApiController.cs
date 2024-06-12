namespace OWP.WorkingShift.Controllers;

[Route("api/working-shift-default")]
[ApiController]
[EnableCors]
[Authorize]
public class WorkingShiftDefaultApiController : ControllerBase
{
    private readonly WorkingShiftDefaultRepository repository;

    public WorkingShiftDefaultApiController(WorkingShiftDefaultRepository repository)
    {
        this.repository = repository;
    }

    [Route("")]
    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.WorkingShiftManagement, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> UpdateDefaultShift(UpdateDefaultShiftModel model)
    {
        await repository.UpdateDefaultShift(model);

        return Ok();
    }
}