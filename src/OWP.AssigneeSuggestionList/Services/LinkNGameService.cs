using Fanex.Logging;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services
{
    public interface LinkNGameService
    {
        Task Group(IList<AssignmentModel> assignments, WorkingSchedule schedules);

    }

    public class LinkNGameServiceImp : LinkNGameService
    {
        private readonly ILogger logger;

        private readonly WorkingScheduleService workingSchedule;

        public LinkNGameServiceImp(ILogger logger, WorkingScheduleService workingSchedule)
        {
            this.logger = logger;
            this.workingSchedule = workingSchedule;
        }

        public async Task Group(IList<AssignmentModel> assignments, WorkingSchedule schedules)
        {
            await LinkTwoGame(assignments, schedules);
            await LinkThreeGame(assignments, schedules);
        }

        private async Task LinkTwoGame(IList<AssignmentModel> assignment, WorkingSchedule schedules)
        {
            var linkNGamePair = GetValidGame(assignment, 2);
            await UpdateSchedule(linkNGamePair, schedules, ColorCodeConst.TWO_GAME);
        }

        private async Task LinkThreeGame(IList<AssignmentModel> assignment, WorkingSchedule schedules)
        {
            var linkNGamePair = GetValidGame(assignment, 3);
            await UpdateSchedule(linkNGamePair, schedules, ColorCodeConst.THREE_GAME);
        }

        private async Task UpdateSchedule(
            IDictionary<DateTime, List<AssignmentModel>> linkNGamePair,
            WorkingSchedule schedules,
             string colorCode)
        {
            var assignmentCell = new AssignmentCell
            {
                Color = colorCode
            };
            foreach (var eventDate in linkNGamePair.Keys)
            {
                var remark = linkNGamePair[eventDate].Select(x => x.Remark).ToList();
                assignmentCell.Text = ScheduleHelper.BuildScheduleTextGroupGame(linkNGamePair[eventDate]);

                var etpkValue = ETPKHelper.BuildEtPkTextDisplay(remark);
                var timeRange = TimeRangeHelper.CalculateTimeRangeFT(eventDate, hasEtpk: string.IsNullOrWhiteSpace(etpkValue));


                var scheduleUpdateModel = new ScheduleUpdateModel
                {
                    EventDate = eventDate,
                    EtPk = new ETPKUpdateSchedule
                    {
                        IsHasEtPk = string.IsNullOrWhiteSpace(etpkValue),
                        EtPkValue = etpkValue
                    },
                    AssignmentCell = assignmentCell,
                    TimeRange = timeRange
                };

                // LOS: TODO
                workingSchedule.UpdateWorkingSchedule(scheduleUpdateModel,
                  schedules);

                await logger.InfoAsync("[LinkNGame]" + string.Join(",", linkNGamePair[eventDate]?.Select(x => x.OWPMatchId)));
                await logger.InfoAsync("[LinkNGame]" + string.Join(",", linkNGamePair[eventDate]?.Select(x => x.BettypeId)));
            }
        }



        private static IDictionary<DateTime, List<AssignmentModel>> GetValidGame(IList<AssignmentModel> models, int numberGame)
        => models.Where(a => LinkGameConditionHelper.CanGroupNGame(a, maxbet => maxbet <= 12000))
                ?.GroupBy(a => a.EventDateTime)
                .Where(g => g.Select(x => x.OWPMatchId).Distinct().Count() == numberGame)?
                .ToDictionary(x => x.Key, x => x.ToList());
    }
}