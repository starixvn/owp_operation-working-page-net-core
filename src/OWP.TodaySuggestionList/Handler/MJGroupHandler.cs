using Fanex.Data.Repository;
using MediatR;
using OWP.TodaySuggestionList.Criteria;
using OWP.TodaySuggestionList.Models;

namespace OWP.TodaySuggestionList.Handler
{
    public class MJGroupRequest : IRequest<Unit>
    {
        public IList<AssignmentModel> Assignments { get; set; }
    }

    public class MJGroupHandler : IRequestHandler<MJGroupRequest, Unit>
    {
        public async Task<Unit> Handle(MJGroupRequest request, CancellationToken cancellationToken)
        {
            request?.Assignments.Where(a => a.IsMajorLeague);
        }
    }
}