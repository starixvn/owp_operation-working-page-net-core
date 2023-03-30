using MediatR;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;
using OWP.Enums;


namespace OWP.AssigneeSuggestionList.Services
{
    public interface FirstHalfService : IRequest<Unit>
    {
        Task Group(IList<AssignmentModel> assignments, WorkingSchedule schedule);
    }

    public class FirstHalfServiceImp : FirstHalfService
    {
        private readonly SingleGameGroupService workingSchedule;

        public FirstHalfServiceImp(SingleGameGroupService workingSchedule)
        {
            this.workingSchedule = workingSchedule;
        }

        public async Task Group(IList<AssignmentModel> assignments, WorkingSchedule schedule)
        {
            var gameGroupPair = assignments
            ?.GroupBy(x => new { x.OWPMatchId })
            .Where(x => x.All(a =>
                        Equals(a.BettypeId, EnumBettype.FirstHalfHandicap)
                        || Equals(a.BettypeId, EnumBettype.FirstHalfOverUnder)))
            ?.ToDictionary(a => a.Key, a => a.ToList());

            var gameGroup = new List<AssignmentModel>();
            if (gameGroupPair is not null)
            {
                foreach (var item in gameGroupPair)
                {
                    gameGroup.AddRange(item.Value);
                }
                var assignmentCell = new AssignmentCell
                {
                    Color = ColorCodeConst.FIRST_HALF,
                    TextColor = ColorCodeConst.TEXT_FIRST_HALF
                };
                await workingSchedule.Group(gameGroup,
                 schedule,
                  "[FIRST_HALF]",
                  BuildScheduleUpdateModel);
            }
        }

        private static ScheduleUpdateModel BuildScheduleUpdateModel(IList<AssignmentModel> assignments)
        {
            var assignmentCell = new AssignmentCell
            {
                Color = ColorCodeConst.FIRST_HALF,
                TextColor = ColorCodeConst.TEXT_FIRST_HALF,
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