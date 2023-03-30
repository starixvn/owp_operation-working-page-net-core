using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services
{
    public interface FantasyService
    {
        Task Group(
            IList<AssignmentModel> assignments,
            WorkingSchedule schedule
            );
    }

    public class FantasyServiceImp : FantasyService
    {
        private readonly SingleGameGroupService workingSchedule;

        public FantasyServiceImp(SingleGameGroupService workingSchedule)
        {
            this.workingSchedule = workingSchedule;
        }

        public async Task Group(
            IList<AssignmentModel> assignments,
            WorkingSchedule schedule
            )
        {
            var matchGroup = assignments
            ?.Where(a => a.IsFantasyMatch)
            ?.ToList();

            var assignmentCell = new AssignmentCell
            {
                Color = ColorCodeConst.FANTASY,
                TextColor = ColorCodeConst.TEXT_FANTASY
            };
            await workingSchedule.Group(
                matchGroup,
                schedule,
                "[FANTASY]",
                BuildScheduleUpdateModel);
        }

        private static ScheduleUpdateModel BuildScheduleUpdateModel(IList<AssignmentModel> assignments)
        {
            var assignmentCell = new AssignmentCell
            {
                Color = ColorCodeConst.FANTASY,
                TextColor = ColorCodeConst.TEXT_FANTASY,
                Text = ScheduleHelper.BuildScheduleTextSingleGame(assignments.First())
            };
            var gameInfo = ScheduleHelper.GetGameInfo(assignments);
            var etPkValue = ETPKHelper.DetectExtraTime(gameInfo.remark);

            // TODO
            return new ScheduleUpdateModel
            {
                EventDate = gameInfo.eventDate,
                EtPk = new ETPKUpdateSchedule
                {
                    IsHasEtPk = !string.IsNullOrEmpty(etPkValue),
                    EtPkValue = etPkValue
                },
                AssignmentCell = assignmentCell,
                TimeRange = TimeRangeHelper.CalculateTimeRangeSingleGame(gameInfo.eventDate, gameInfo.bettypeIds, !string.IsNullOrEmpty(etPkValue))
            };
        }
    }
}