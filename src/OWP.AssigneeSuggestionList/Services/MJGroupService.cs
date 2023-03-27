using Fanex.Logging;
using MediatR;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;


namespace OWP.AssigneeSuggestionList.Services
{
    public interface MJGroupService
    {
        Task<Unit> Group(IList<AssignmentModel> assignments);
    }

    public class MJGroupServiceImp : MJGroupService
    {
        private readonly ILogger logger;

        public MJGroupServiceImp(ILogger logger)
        {
            this.logger = logger;
        }
        public async Task<Unit> Group(IList<AssignmentModel> assignments)
        {
            var mJGroup = assignments?.Where(a => a.IsMajorLeague);
            foreach (var item in mJGroup)
            {
                item.ColorCode = ColorCodeConst.MJ;
            }
            await logger.InfoAsync("[mJGroup]" + mJGroup?.Select(x => x.OWPMatchId));
            return Unit.Value;
        }
    }
}