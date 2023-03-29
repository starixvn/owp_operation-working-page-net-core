using Fanex.Logging;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

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
            var linkNGamePair = FilterGroupNGame(assignment, 2);
            await UpdateSchedule(linkNGamePair, schedules, ColorCodeConst.TWO_GAME);
        }

        private async Task LinkThreeGame((IList<AssignmentModel>, IList<MaxbetModel>) assignment, WorkingSchedule schedules)
        {
            var linkNGamePair = FilterGroupNGame(assignment, 3);
            await UpdateSchedule(linkNGamePair, schedules, ColorCodeConst.THREE_GAME);
        }

        private async Task UpdateSchedule(
            IDictionary<DateTime, List<AssignmentModel>> linkNGamePair,
            WorkingSchedule schedules, string colorCode)
        {
            foreach (var key in linkNGamePair.Keys)
            {
                // LOS: TODO
                var bettypeIds = linkNGamePair[key].Select(x => x.BettypeId).ToList();
                workingSchedule.UpdateWorkingSchedule(key,
                 bettypeIds,
                 new AssignmentCell
                 {
                     Color = colorCode,
                     Text = "NGame"
                 }, schedules,
                 new ETPKUpdateSchedule
                 {
                     IsHasEtPk = true,
                     EtPkValue = ""
                 });

                await logger.InfoAsync("[LinkNGame]" + string.Join(",", linkNGamePair[key]?.Select(x => x.OWPMatchId)));
                await logger.InfoAsync("[LinkNGame]" + string.Join(",", linkNGamePair[key]?.Select(x => x.BettypeId)));
            }
        }

        private IDictionary<DateTime, List<AssignmentModel>> FilterGroupNGame((IList<AssignmentModel>, IList<MaxbetModel>) models, int numberGame)
        => models.Item1?.Where(a => LinkGameConditionHelper.CanGroupNGame(a, models.Item2, maxbet => maxbet <= 12000))
                ?.GroupBy(a => a.EventDateTime)
                .Where(g => g.Select(x => x.OWPMatchId).Distinct().Count() == numberGame)?
                .ToDictionary(x => x.Key, x => x.ToList());
    }
}