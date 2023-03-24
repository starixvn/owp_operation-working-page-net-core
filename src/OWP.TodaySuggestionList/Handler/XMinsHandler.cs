using MediatR;
using OWP.TodaySuggestionList.Models;

namespace OWP.TodaySuggestionList.Handler
{
    public class XMinsHandlerRequest : IRequest<Unit>
    {
        public IList<AssignmentModel> Assignments { get; set; }
    }

    public class XMinsHandlerHandler : IRequestHandler<XMinsHandlerRequest, Unit>
    {
        public async Task<Unit> Handle(XMinsHandlerRequest request, CancellationToken cancellationToken)
        {
            var xMinsMatchGroup = request.Assignments?.Where(a => a.Is15Mins);

            request.Assignments = request.Assignments?.Except(xMinsMatchGroup, new AssignmentComparer())?.ToList();
            return Unit.Value;
        }
    }
}