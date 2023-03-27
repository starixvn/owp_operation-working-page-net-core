using Fanex.Logging;
using MediatR;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;


namespace OWP.AssigneeSuggestionList.Handler
{
    public class XMinsHandlerRequest : IRequest<Unit>
    {
        public IList<AssignmentModel> Assignments { get; set; }
    }

    public class XMinsHandlerHandler : IRequestHandler<XMinsHandlerRequest, Unit>
    {
        private readonly ILogger logger;

        public XMinsHandlerHandler(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task<Unit> Handle(XMinsHandlerRequest request, CancellationToken cancellationToken)
        {
            var xMinsMatchGroup = request.Assignments?.Where(a => a.Is15Mins);
            foreach (var item in xMinsMatchGroup)
            {
                item.ColorCode = ColorCodeConst.X_MINS;
            }
            await logger.InfoAsync("[xMinsMatchGroup]" + xMinsMatchGroup?.Select(x => x.OWPMatchId));
            request.Assignments = request.Assignments?.Except(xMinsMatchGroup, new AssignmentComparer())?.ToList();
            return Unit.Value;
        }
    }
}