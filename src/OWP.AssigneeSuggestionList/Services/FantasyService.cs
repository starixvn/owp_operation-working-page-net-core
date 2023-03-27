using Fanex.Logging;
using MediatR;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services
{
    public interface FantasyService
    {
        Task<Unit> Group(IList<AssignmentModel> assignments, IList<MaxbetModel> maxbetModels);
    }

    public class FantasyServiceImp : FantasyService
    {
        private readonly ILogger logger;

        public FantasyServiceImp(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task<Unit> Group(IList<AssignmentModel> assignments, IList<MaxbetModel> maxbetModels)
        {
            var fantasyMatchGroup = assignments?.Where(a => a.IsFantasyMatch);
            foreach (var item in fantasyMatchGroup)
            {
                item.ColorCode = ColorCodeConst.FANTASY;
            }
            await logger.InfoAsync("[fantasyMatchGroup]" + fantasyMatchGroup.Select(x => x.OWPMatchId));
            return Unit.Value;
        }
    }
}