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

            await workingSchedule.Group(
                mJGroup,
                schedules,
                "[LinkNGame]",
                BuildScheduleUpdateModel);
        }

        public static ScheduleUpdateModel BuildScheduleUpdateModel(IList<AssignmentModel> assignments)
        {
            var assignmentCell = new AssignmentCell
            {
                Color = ColorCodeConst.MJ,
                TextColor = ColorCodeConst.TEXT_MJ,
                Text = ScheduleHelper.BuildScheduleTextSingleGame(assignments.First())
            };
            var gameInfo = ScheduleHelper.GetGameInfo(assignments);
            var etPkValue = ETPKHelper.DetectExtraTime(gameInfo.remark);

            return new()
            {
                EventDate = gameInfo.eventDate,
                EtPk = new ETPKUpdateSchedule
                {
                    IsHasEtPk = !string.IsNullOrEmpty(etPkValue),
                    EtPkValue = etPkValue
                },
                AssignmentCell = assignmentCell,
                TimeRange = TimeRangeHelper.CalculateTimeRangeSingleGame(
                    gameInfo.eventDate,
                    gameInfo.bettypeIds,
                     !string.IsNullOrEmpty(etPkValue))
            };
        }
    }
}