using Fanex.Logging;
using MediatR;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services
{
    public interface OneGameService
    {
        Task<Unit> Group(IList<AssignmentModel> assignments, IList<MaxbetModel> maxbetModels);
    }

    public class OneGameServiceImp : OneGameService
    {
        private const decimal MIN_MAXBET = 12000;

        private readonly ILogger logger;

        public OneGameServiceImp(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task<Unit> Group(IList<AssignmentModel> assignments, IList<MaxbetModel> maxbetModels)
        {
            var oneGameGroup = assignments?.Where(a => LinkGameConditionHelper.CanGroupNGame(a, maxbetModels, maxbet => maxbet > MIN_MAXBET));
            foreach (var item in oneGameGroup)
            {
                item.ColorCode = ColorCodeConst.ONE_GAME;
            }
            await logger.InfoAsync("[oneGameGroup]" + oneGameGroup.Select(x => x.OWPMatchId));
            return Unit.Value;
        }
    }
}