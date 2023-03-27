using Fanex.Logging;
using MediatR;
using OWP.TodaySuggestionList.Models;

namespace OWP.TodaySuggestionList.Handler
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
            await logger.InfoAsync("[mJGroup]" + mJGroup?.Select(x => x.OWPMatchId));
            request.Assignments = request.Assignments?.Except(mJGroup, new AssignmentComparer())?.ToList();
            return Unit.Value;
        }
    }
}