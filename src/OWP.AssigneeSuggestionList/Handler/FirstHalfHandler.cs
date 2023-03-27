using Fanex.Logging;
using MediatR;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;
using OWP.Enums;


namespace OWP.AssigneeSuggestionList.Handler
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
            var firstHalfMatchGroupPair = request.Assignments?
                .GroupBy(x => x.OWPMatchId)
                .Where(x => x.All(a =>
                        Equals(a.BettypeId, EnumBettype.FirstHalfHandicap)
                        || Equals(a.BettypeId, EnumBettype.FirstHalfOverUnder)))
                .ToDictionary(a => a.Key, a => a.ToList());

            var firstHalfMatchGroup = new List<AssignmentModel>();
            foreach (var item in firstHalfMatchGroupPair)
            {
                firstHalfMatchGroup.AddRange(item.Value);
            }
            foreach (var item in firstHalfMatchGroup)
            {
                item.ColorCode = ColorCodeConst.FIRST_HALF;
            }
            await logger.InfoAsync("[firstHalfMatchGroup]" + firstHalfMatchGroup?.Select(x => x.OWPMatchId));
            request.Assignments = request.Assignments?.Except(firstHalfMatchGroup, new AssignmentComparer())?.ToList();
            return Unit.Value;
        }
    }
}