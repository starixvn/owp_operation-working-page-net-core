using System.Collections.Generic;
using System.Linq;
using OWP.Extensions;
using OWP.OtherSport.EventAssignment.Helper;
using OWP.OtherSport.EventAssignment.Models;
using OWP.OtherSport.Submit.Models;

namespace OWP.OtherSport.EventAssignment.Builder
{
    public interface BettypeBuilder
    {
        public void BuildBettype(
           string employee,
           List<EventAssignmentModel> matchsAssignments,
           Dictionary<int, List<MatchAssignment>> dicMatchAssignment);

        IList<string> BuildBettypeTrader();

        IList<string> BuildBettypeForMatch(string employee, EventAssignmentModel matchsAssignment);
    }

    public class BettypeBuilderImp : BettypeBuilder
    {
        private IList<MatchAssignment> assignments;

        public void BuildBettype(
            string employee,
            List<EventAssignmentModel> matchsAssignments,
            Dictionary<int, List<MatchAssignment>> dicMatchAssignment)
        {
            foreach (var matchAssignment in matchsAssignments)
            {
                assignments = dicMatchAssignment.GetValue(matchAssignment.OSMatchId, new List<MatchAssignment>());

                var bettypes = BuildBettypeForMatch(
                    employee,
                    matchAssignment);

                foreach (var bettype in bettypes)
                {
                    matchAssignment.TraderMaxbetModels.Add(new TraderMaxBetModel
                    {
                        BetTypeName = bettype,
                        TraderAssignment = new TraderAssignment
                        {
                            OSMatchId = matchAssignment.OSMatchId,
                            BetTypeName = bettype
                        },
                        Maxbet = new MatchMaxBet
                        {
                            OSMatchId = matchAssignment.OSMatchId,
                            BetTypeName = bettype
                        }
                    });
                }
            }
        }

        public IList<string> BuildBettypeForMatch(
            string employee,
            EventAssignmentModel matchsAssignment)
        {
            var existsAssignment = assignments.Any(match =>
              match.Username.IsContains(employee) ||
              match.Nickname.IsContains(employee));

            if (existsAssignment)
            {
                return BuildBettypeTrader();
            }

            return new List<string>();
        }

        public IList<string> BuildBettypeTrader()
            => assignments
                .Select(a => a.BetTypeName)
                    .Distinct()
                        .ToList();
    }
}
