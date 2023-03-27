using Fanex.Logging;
using MediatR;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services
{
    public interface XMinsGroupService
    {
        Task<Unit> Group(IList<AssignmentModel> assignments, IList<MaxbetModel> maxbetModels);
    }

    public class XMinsGroupServiceImp : XMinsGroupService
    {
        private readonly ILogger logger;

        public XMinsGroupServiceImp(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task<Unit> Group(IList<AssignmentModel> assignments, IList<MaxbetModel> maxbetModels)
        {
            var xMinsMatchGroup = assignments?.Where(a => a.Is15Mins);
            foreach (var item in xMinsMatchGroup)
            {
                item.ColorCode = ColorCodeConst.X_MINS;
            }
            await logger.InfoAsync("[xMinsMatchGroup]" + xMinsMatchGroup?.Select(x => x.OWPMatchId));
            return Unit.Value;
        }
    }
}