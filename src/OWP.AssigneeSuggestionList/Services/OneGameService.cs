using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services
{
    public interface OneGameService
    {
        Task Group(IList<AssignmentModel> assignments, WorkingSchedule schedules);
    }

    public class OneGameServiceImp : OneGameService
    {
        private const decimal MIN_MAXBET = 12000;

        private readonly SingleGameGroupService workingSchedule;

        public OneGameServiceImp(SingleGameGroupService workingSchedule)
        {
            this.workingSchedule = workingSchedule;
        }

        public async Task Group(IList<AssignmentModel> assignments, WorkingSchedule schedules)
        {
            var oneGameGroup = assignments
            ?.Where(a => LinkGameConditionHelper.CanGroupNGame(a, maxbet => maxbet > MIN_MAXBET))
            .ToList();

            await workingSchedule.Group(
                oneGameGroup,
                schedules,
                "[oneGameGroup]",
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