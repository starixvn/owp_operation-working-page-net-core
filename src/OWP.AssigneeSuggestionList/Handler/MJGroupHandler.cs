using Fanex.Logging;
using MediatR;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;


namespace OWP.AssigneeSuggestionList.Handler
{
    public class MJGroupRequest : IRequest<Unit>
    {
        public IList<AssignmentModel> Assignments { get; set; }
    }

    public class MJGroupHandler : IRequestHandler<MJGroupRequest, Unit>
    {
        private readonly ILogger logger;

        public MJGroupHandler(ILogger logger)
        {
            this.logger = logger;
        }
        public async Task<Unit> Handle(MJGroupRequest request, CancellationToken cancellationToken)
        {
            var mJGroup = request.Assignments?.Where(a => a.IsMajorLeague);
            foreach (var item in mJGroup)
            {
                item.ColorCode = ColorCodeConst.MJ;
            }
            await logger.InfoAsync("[mJGroup]" + mJGroup?.Select(x => x.OWPMatchId));
            request.Assignments = request.Assignments?.Except(mJGroup, new AssignmentComparer())?.ToList();
            return Unit.Value;
        }
    }
}