using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services
{
    public interface CornerService
    {
        Task Group(IList<AssignmentModel> assignments, WorkingSchedule schedules);

    }

    public class CornerServiceImp : CornerService
    {
        private readonly SingleGameGroupService workingSchedule;

        public CornerServiceImp(SingleGameGroupService workingSchedule)
        {
            this.workingSchedule = workingSchedule;
        }

        public async Task Group(
             IList<AssignmentModel> assignments,
            WorkingSchedule schedules)
        {
            var gameGroup = assignments
            ?.Where(a => a.IsCornerMatch)
             ?.ToList();

            var assignmentCell = new AssignmentCell
            {
                Color = ColorCodeConst.CORNER,
                TextColor = ColorCodeConst.TEXT_CORNER
            };
            await workingSchedule.Group(
                gameGroup,
                schedules,
                 "[CORNER]",
                 BuildScheduleUpdateModel);
        }

        private static ScheduleUpdateModel BuildScheduleUpdateModel(IList<AssignmentModel> assignments)
        {
            var assignmentCell = new AssignmentCell
            {
                Color = ColorCodeConst.CORNER,
                TextColor = ColorCodeConst.TEXT_CORNER,
                Text = ScheduleHelper.BuildScheduleTextSingleGame(assignments.First())
            };
            var (eventDate, owpMatchId, bettypeIds) = ScheduleHelper.GetGameInfo(assignments);

            // TODO
            return new ScheduleUpdateModel
            {
                EventDate = eventDate,
                EtPk = new ETPKUpdateSchedule(),
                AssignmentCell = assignmentCell,
                TimeRange = TimeRangeHelper.CalculateTimeRangeSingleGame(eventDate, bettypeIds, true)
            };
        }
    }
}