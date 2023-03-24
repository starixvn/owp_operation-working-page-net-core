using MediatR;
using OWP.TodaySuggestionList.Models;

namespace OWP.TodaySuggestionList.Handler
{
    public class FantasyRequest : IRequest<Unit>
    {
        public IList<AssignmentModel> Assignments { get; set; }
    }

    public class FantasyHandler : IRequestHandler<FantasyRequest, Unit>
    {
        public async Task<Unit> Handle(FantasyRequest request, CancellationToken cancellationToken)
        {
            var fantasyMatchGroup = request.Assignments?.Where(a => a.IsFantasyMatch);

            request.Assignments = request.Assignments?.Except(fantasyMatchGroup, new AssignmentComparer())?.ToList();
            return Unit.Value;
        }
    }
}