using System.Globalization;
using Fanex.Logging;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;
using OWP.Enums;

namespace OWP.AssigneeSuggestionList.Services
{
    public interface LinkNGameService
    {
        Task Group((IList<AssignmentModel>, IList<MaxbetModel>) assignments, WorkingSchedule schedules);

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

        public async Task Group((IList<AssignmentModel>, IList<MaxbetModel>) assignments, WorkingSchedule schedules)
        {
            await LinkTwoGame(assignments, schedules);
            await LinkThreeGame(assignments, schedules);
        }

        private async Task LinkTwoGame((IList<AssignmentModel>, IList<MaxbetModel>) assignment, WorkingSchedule schedules)
        {
            var linkNGamePair = GetValidGame(assignment, 2);
            await UpdateSchedule(assignment.Item2, linkNGamePair, schedules, ColorCodeConst.TWO_GAME);
        }

        private async Task LinkThreeGame((IList<AssignmentModel>, IList<MaxbetModel>) assignment, WorkingSchedule schedules)
        {
            var linkNGamePair = GetValidGame(assignment, 3);
            await UpdateSchedule(assignment.Item2, linkNGamePair, schedules, ColorCodeConst.THREE_GAME);
        }

        private async Task UpdateSchedule(
            IList<MaxbetModel> maxbetModels,
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
                var (owpMatchIds, bettypeIds) = SelectGameInfo(linkNGamePair[eventDate]);
                assignmentCell.Text = ScheduleTextHelper.BuildScheduleTextGroupGame(eventDate, owpMatchIds, maxbetModels);
                // LOS: TODO
                workingSchedule.UpdateWorkingSchedule(eventDate,
                 bettypeIds,
                 assignmentCell,
                  schedules,
                 new ETPKUpdateSchedule
                 {
                     IsHasEtPk = true,
                     EtPkValue = ""
                 });

                await logger.InfoAsync("[LinkNGame]" + string.Join(",", linkNGamePair[eventDate]?.Select(x => x.OWPMatchId)));
                await logger.InfoAsync("[LinkNGame]" + string.Join(",", linkNGamePair[eventDate]?.Select(x => x.BettypeId)));
            }
        }

        private static (IList<int>, IList<int>) SelectGameInfo(IList<AssignmentModel> assignments)
        {
            var owpMatchIds = assignments.Select(x => x.OWPMatchId).Distinct().ToList();
            var bettypeIds = assignments.Select(x => x.BettypeId).Distinct().ToList();
            return (owpMatchIds, bettypeIds);
        }

        private IDictionary<DateTime, List<AssignmentModel>> GetValidGame((IList<AssignmentModel>, IList<MaxbetModel>) models, int numberGame)
        => models.Item1?.Where(a => LinkGameConditionHelper.CanGroupNGame(a, models.Item2, maxbet => maxbet <= 12000))
                ?.GroupBy(a => a.EventDateTime)
                .Where(g => g.Select(x => x.OWPMatchId).Distinct().Count() == numberGame)?
                .ToDictionary(x => x.Key, x => x.ToList());
    }
}