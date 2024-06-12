namespace OWP.WorkingShift.Controllers
{
    [Route("api/working-shift-timezone")]
    [ApiController]
    [EnableCors]
    [Authorize]
    public class WorkingShiftTimezoneApiController : ControllerBase
    {
        private readonly WorkingShiftTimezoneSettingRepository repository;

        public WorkingShiftTimezoneApiController(WorkingShiftTimezoneSettingRepository repository)
        {
            this.repository = repository;
        }

        [Route("")]
        [HttpPut]
        [NapAuth(FunctionKey = FunctionKeys.WorkingShiftManagement_ShiftManagement, Policy = Constant.Policy.Host)]
        public async Task<IActionResult> UpdateTimeZoneSetting(TimezoneUpdateModel model)
        {
            var result = await repository.Update(model);
            return Ok(result);
        }
    }
}