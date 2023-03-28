using Fanex.Logging;
using MediatR;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services
{
    public interface LinkNGameService
    {
        Task<Unit> Group(LinkNGameModel models, WorkingSchedule schedules);

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

        public async Task<Unit> Group(LinkNGameModel models, WorkingSchedule schedules)
        {
            var linkNGamePair = models.Assignments?.Where(a => LinkGameConditionHelper.CanGroupNGame(a, models.MaxbetModels, maxbet => maxbet <= 12000))
                ?.GroupBy(a => new { a.EventDateTime })
                .Where(g => g.Select(x => x.OWPMatchId).Distinct().Count() == models.NumberGame)?
                .ToDictionary(x => x.Key, x => x.ToList());

            foreach (var key in linkNGamePair.Keys)
            {
                var bettypeIds = linkNGamePair[key].Select(x => x.BettypeId).ToList();
                workingSchedule.UpdateWorkingSchedule(key.EventDateTime,
                 bettypeIds,
                 new AssignmentCell
                 {
                     Color = models.ColorCode,
                     Text = "NGame"
                 }, schedules);

                await logger.InfoAsync("[LinkNGame]" + string.Join(",", linkNGamePair[key]?.Select(x => x.OWPMatchId)));
                await logger.InfoAsync("[LinkNGame]" + string.Join(",", linkNGamePair[key]?.Select(x => x.BettypeId)));
            }

            return Unit.Value;
        }
    }
}