using MediatR;
using OWP.TodaySuggestionList.Models;

namespace OWP.TodaySuggestionList.Handler
{
    public class CornerHandlerRequest : IRequest<Unit>
    {
        public IList<AssignmentModel> Assignments { get; set; }
    }

    public class CornerHandlerHandler : IRequestHandler<CornerHandlerRequest, Unit>
    {
        public async Task<Unit> Handle(CornerHandlerRequest request, CancellationToken cancellationToken)
        {
            var cornerMatchGroup = request.Assignments?.Where(a => a.IsCornerMatch);

            request.Assignments = request.Assignments?.Except(cornerMatchGroup, new AssignmentComparer())?.ToList();
            return Unit.Value;
        }
    }
}