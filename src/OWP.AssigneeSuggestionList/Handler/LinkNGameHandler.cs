using Fanex.Logging;
using MediatR;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Handler
{
    public class LinkNGameRequest : IRequest<Unit>
    {
        public IList<AssignmentModel> Assignments { get; set; }

        public IList<MaxbetModel> MaxbetModels { get; set; }

        public int NumberGame { get; set; }

        public decimal Maxbet { get; set; }

        public string ColorCode { get; set; }

        public Func<decimal, bool> MaxbetComparerFunc { get; set; }
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
            var linkNMatchPair = request.Assignments?.Where(a => LinkGameConditionHelper.CanGroupNGame(a, request.MaxbetModels, request.MaxbetComparerFunc))
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