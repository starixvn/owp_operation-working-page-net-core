using Fanex.Logging;
using MediatR;
using OWP.Enums;
using OWP.TodaySuggestionList.Models;

namespace OWP.TodaySuggestionList.Handler
{
    public class LinkNGameRequest : IRequest<Unit>
    {
        public IList<AssignmentModel> Assignments { get; set; }

        public int NumberGame { get; set; }
    }

    public class LinkNGameHandler : IRequestHandler<LinkNGameRequest, Unit>
    {
        private readonly ILogger logger;

        public LinkNGameHandler(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task<Unit> Handle(LinkNGameRequest request, CancellationToken cancellationToken)
        {
            var linkNMatchPair = request.Assignments?.Where(a => a.IsMainMatch
                    && (Equals(a.BettypeId, EnumBettype.Handicap)
                    || Equals(a.BettypeId, EnumBettype.OverUnder)))?
                .GroupBy(a => a.EventDateTime)?
                .Where(g => Equals(g.Count(), request.NumberGame))?
                .ToDictionary(x => x.Key, x => x.ToList());

            var matchExclude = new List<AssignmentModel>();

            foreach (var linkNMatch in linkNMatchPair)
            {
                matchExclude.AddRange(linkNMatch.Value);
            }
            await logger.InfoAsync("[LinkNGame]" + matchExclude?.Select(x => x.OWPMatchId));
            request.Assignments = request.Assignments?.Except(matchExclude, new AssignmentComparer())?.ToList();
            return Unit.Value;
        }
    }
}