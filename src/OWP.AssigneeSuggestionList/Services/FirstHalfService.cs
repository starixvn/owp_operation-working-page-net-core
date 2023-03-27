using Fanex.Logging;
using MediatR;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;
using OWP.Enums;


namespace OWP.AssigneeSuggestionList.Services
{
    public interface FirstHalfService : IRequest<Unit>
    {
        Task<Unit> Group(IList<AssignmentModel> assignments, IList<MaxbetModel> maxbetModels);

    }

    public class FirstHalfServiceImp : FirstHalfService
    {
        private readonly ILogger logger;

        public FirstHalfServiceImp(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task<Unit> Group(IList<AssignmentModel> assignments, IList<MaxbetModel> maxbetModels)
        {
            var firstHalfMatchGroupPair = assignments?
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
            return Unit.Value;
        }
    }
}