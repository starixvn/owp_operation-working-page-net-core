using Fanex.Logging;
using MediatR;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;


namespace OWP.AssigneeSuggestionList.Services
{
    public interface CornerService
    {
        Task<Unit> Group(IList<AssignmentModel> assignments, IList<MaxbetModel> maxbetModels);

    }

    public class CornerServiceImp : CornerService
    {
        private readonly ILogger logger;

        public CornerServiceImp(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task<Unit> Group(IList<AssignmentModel> assignments, IList<MaxbetModel> maxbetModels)
        {
            var cornerMatchGroup = assignments?.Where(a => a.IsCornerMatch);
            foreach (var item in cornerMatchGroup)
            {
                item.ColorCode = ColorCodeConst.CORNER;
            }
            await logger.InfoAsync("[cornerMatchGroup]" + cornerMatchGroup.Select(x => x.OWPMatchId));
            return Unit.Value;
        }
    }
}