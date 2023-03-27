using Fanex.Logging;
using MediatR;
using OWP.Enums;
using OWP.TodaySuggestionList.Models;

namespace OWP.TodaySuggestionList.Handler
{
    public class OneGameRequest : IRequest<Unit>
    {
        public IList<AssignmentModel> Assignments { get; set; }

        public IList<MaxbetModel> MaxbetModels { get; set; }
    }

    public class OneGameHandler : IRequestHandler<OneGameRequest, Unit>
    {
        private const decimal MIN_MAXBET = 12000;

        private readonly ILogger logger;

        public OneGameHandler(ILogger logger)
        {
            this.logger = logger;
        }
        public async Task<Unit> Handle(OneGameRequest request, CancellationToken cancellationToken)
        {
            var oneGameGroup = request.Assignments?.Where(a => a.IsMainMatch
             && (Equals(a.BettypeId, EnumBettype.Handicap)
            || Equals(a.BettypeId, EnumBettype.OverUnder))
            && request.MaxbetModels is not null
            && request.MaxbetModels.Any(m => Equals(m.OWPMatchId, a.OWPMatchId)
                                            && Equals(m.BettypeId, a.BettypeId)
                                            && m.Maxbet > MIN_MAXBET));
            await logger.InfoAsync("[oneGameGroup]" + oneGameGroup.Select(x => x.OWPMatchId));
            request.Assignments = request.Assignments?.Except(oneGameGroup, new AssignmentComparer())?.ToList();
            return Unit.Value;
        }
    }
}