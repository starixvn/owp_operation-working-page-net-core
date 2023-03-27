using Fanex.Logging;
using MediatR;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Handler
{
    public class FantasyRequest : IRequest<Unit>
    {
        public IList<AssignmentModel> Assignments { get; set; }
    }

    public class FantasyHandler : IRequestHandler<FantasyRequest, Unit>
    {
        private readonly ILogger logger;

        public FantasyHandler(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task<Unit> Handle(FantasyRequest request, CancellationToken cancellationToken)
        {
            var fantasyMatchGroup = request.Assignments?.Where(a => a.IsFantasyMatch);
            foreach (var item in fantasyMatchGroup)
            {
                item.ColorCode = ColorCodeConst.FANTASY;
            }
            await logger.InfoAsync("[fantasyMatchGroup]" + fantasyMatchGroup.Select(x => x.OWPMatchId));
            request.Assignments = request.Assignments?.Except(fantasyMatchGroup, new AssignmentComparer())?.ToList();
            return Unit.Value;
        }
    }
}