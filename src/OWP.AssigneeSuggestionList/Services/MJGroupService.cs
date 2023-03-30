using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;


namespace OWP.AssigneeSuggestionList.Services
{
    public interface MJGroupService
    {
        Task Group(IList<AssignmentModel> assignments,
            WorkingSchedule schedules);
    }

    public class MJGroupServiceImp : MJGroupService
    {
        private readonly SingleGameGroupService workingSchedule;

        public MJGroupServiceImp(SingleGameGroupService workingSchedule)
        {
            this.workingSchedule = workingSchedule;
        }
        public async Task Group(
            IList<AssignmentModel> assignments,
            WorkingSchedule schedules
            )
        {
            var mJGroup = assignments.Where(a => a.IsMajorLeague)
            ?.ToList();
            var assignmentCell = new AssignmentCell
            {
                Color = ColorCodeConst.MJ,
                TextColor = ColorCodeConst.TEXT_MJ
            };
            await workingSchedule.Group(
                mJGroup,
                schedules,
                "[LinkNGame]",
                BuildScheduleUpdateModel);
        }

        private ScheduleUpdateModel BuildScheduleUpdateModel(IList<AssignmentModel> assignments)
        {
            var assignmentCell = new AssignmentCell
            {
                Color = ColorCodeConst.ONE_GAME,
                TextColor = ColorCodeConst.TEXT_ONE_GAME,
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