using Fanex.Logging;
using MediatR;
using OWP.TodaySuggestionList.Constant;
using OWP.TodaySuggestionList.Models;

namespace OWP.TodaySuggestionList.Handler
{
    public class CornerHandlerRequest : IRequest<Unit>
    {
        public IList<AssignmentModel> Assignments { get; set; }
    }

    public class CornerHandlerHandler : IRequestHandler<CornerHandlerRequest, Unit>
    {
        private readonly ILogger logger;

        public CornerHandlerHandler(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task<Unit> Handle(CornerHandlerRequest request, CancellationToken cancellationToken)
        {
            var cornerMatchGroup = request.Assignments?.Where(a => a.IsCornerMatch);
            foreach (var item in cornerMatchGroup)
            {
                item.ColorCode = ColorCodeConst.CORNER;
            }
            await logger.InfoAsync("[cornerMatchGroup]" + cornerMatchGroup.Select(x => x.OWPMatchId));
            request.Assignments = request.Assignments?.Except(cornerMatchGroup, new AssignmentComparer())?.ToList();
            return Unit.Value;
        }
    }
}