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

        public string ColorCode { get; set; }
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

            var matchValid = new List<AssignmentModel>();

            foreach (var linkNMatch in linkNMatchPair)
            {
                matchValid.AddRange(linkNMatch.Value);
            }
            foreach (var item in matchValid)
            {
                item.ColorCode = request.ColorCode;
            }
            await logger.InfoAsync("[LinkNGame]" + matchValid?.Select(x => x.OWPMatchId));
            request.Assignments = request.Assignments?.Except(matchValid, new AssignmentComparer())?.ToList();
            return Unit.Value;
        }
    }
}