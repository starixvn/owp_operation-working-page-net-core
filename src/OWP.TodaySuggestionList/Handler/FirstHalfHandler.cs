using MediatR;
using OWP.Enums;
using OWP.TodaySuggestionList.Models;

namespace OWP.TodaySuggestionList.Handler
{
    public class FirstHalfRequest : IRequest<Unit>
    {
        public IList<AssignmentModel> Assignments { get; set; }
    }

    public class FirstHalfHandler : IRequestHandler<FirstHalfRequest, Unit>
    {
        public async Task<Unit> Handle(FirstHalfRequest request, CancellationToken cancellationToken)
        {
            var firstHalfMatchGroup = request.Assignments?.Where(a =>
                    (Equals(a.BettypeId, EnumBettype.FirstHalfHandicap)
                    || Equals(a.BettypeId, EnumBettype.FirstHalfHandicap))
                    && !Equals(a.BettypeId, EnumBettype.Handicap)
                    && !Equals(a.BettypeId, EnumBettype.OverUnder));

            request.Assignments = request.Assignments?.Except(firstHalfMatchGroup, new AssignmentComparer())?.ToList();
            return Unit.Value;
        }
    }
}