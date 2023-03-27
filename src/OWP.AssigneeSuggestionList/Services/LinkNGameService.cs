using Fanex.Logging;
using MediatR;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services
{
    public interface LinkNGameService
    {
        Task<Unit> Group(LinkNGameModel models);

    }

    public class LinkNGameServiceImp : LinkNGameService
    {
        private readonly ILogger logger;

        public LinkNGameServiceImp(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task<Unit> Group(LinkNGameModel models)
        {
            var linkNMatchPair = models.Assignments?.Where(a => LinkGameConditionHelper.CanGroupNGame(a, models.MaxbetModels, maxbet => maxbet <= 12000))
                .GroupBy(a => a.EventDateTime)?
                .Where(g => Equals(g.Count(), models.NumberGame))?
                .ToDictionary(x => x.Key, x => x.ToList());

            var matchValid = new List<AssignmentModel>();

            foreach (var linkNMatch in linkNMatchPair)
            {
                matchValid.AddRange(linkNMatch.Value);
            }
            foreach (var item in matchValid)
            {
                item.ColorCode = models.ColorCode;
            }
            await logger.InfoAsync("[LinkNGame]" + matchValid?.Select(x => x.OWPMatchId));
            return Unit.Value;
        }
    }
}