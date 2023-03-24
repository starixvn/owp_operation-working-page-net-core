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
        public async Task<Unit> Handle(OneGameRequest request, CancellationToken cancellationToken)
        {
            var mainMatchGroup = request.Assignments?.Where(a => a.IsMainMatch
             && (Equals(a.BettypeId, EnumBettype.Handicap)
            || Equals(a.BettypeId, EnumBettype.OverUnder))
            && request.MaxbetModels is not null
            && request.MaxbetModels.Any(m => Equals(m.OWPMatchId, a.OWPMatchId)
                                            && Equals(m.BettypeId, a.BettypeId)
                                            && m.Maxbet > MIN_MAXBET));

            request.Assignments = request.Assignments?.Except(mainMatchGroup, new AssignmentComparer())?.ToList();
            return Unit.Value;
        }
    }
}