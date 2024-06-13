using OWP.WorkingShift.Common.Models;
using OWP.WorkingShift.Common.Repositories;
using OWP.WorkingShift.Common.Utils;

namespace OWP.WorkingShift.Common.Controllers
{
    [Route("api/working-shift-publish")]
    [ApiController]
    [EnableCors]
    [Authorize]
    public class WorkingShiftPublishApiController : ControllerBase
    {
        private readonly WorkingShiftPublishRepository repository;

        public WorkingShiftPublishApiController(WorkingShiftPublishRepository repository)
        {
            this.repository = repository;
        }

        [Route("")]
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.WorkingShiftManagement, Policy = Constant.Policy.Host)]
        public async Task<IActionResult> PublishWeeklyWorkingShift(PublishWeeklyWorkingShiftModel model)
        {
            IEnumerable<GroupWorkingShiftByUserModel> groupByUser = new List<GroupWorkingShiftByUserModel>();
            var result = await repository.PublishSection(model);
            var getSectionPublish = await repository.GetSectionPublishWorkingShift(model.FromDate, model.ToDate, model.SectionId.ToString());
            result.Data = getSectionPublish.FirstOrDefault();
            if (model.IsPublished)
            {
                var queryModel = new WorkingShiftQueryBySectionModel
                {
                    SectionId = model.SectionId.ToString(),
                    FromDate = model.FromDate,
                    ToDate = model.FromDate.AddDays(6)
                };
                var weeklyWorkingShiftForSection = await repository.GetWeeklyWorkingShiftBySection(queryModel);
                groupByUser = weeklyWorkingShiftForSection.GroupWorkingShiftByUser(queryModel.FromDate, queryModel.ToDate);
            }


            return Ok(new { result, groupByUser });
        }
    }
}