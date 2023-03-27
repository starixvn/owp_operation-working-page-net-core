using Fanex.Logging;
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
        private readonly ILogger logger;

        public FirstHalfHandler(ILogger logger)
        {
            this.logger = logger;
        }


        public async Task<Unit> Handle(FirstHalfRequest request, CancellationToken cancellationToken)
        {
            var firstHalfMatchGroup = request.Assignments?.Where(a =>
                    Equals(a.BettypeId, EnumBettype.FirstHalfHandicap)
                    || Equals(a.BettypeId, EnumBettype.FirstHalfHandicap));
            await logger.InfoAsync("[firstHalfMatchGroup]" + firstHalfMatchGroup?.Select(x => x.OWPMatchId));
            request.Assignments = request.Assignments?.Except(firstHalfMatchGroup, new AssignmentComparer())?.ToList();
            return Unit.Value;
        }
    }
}