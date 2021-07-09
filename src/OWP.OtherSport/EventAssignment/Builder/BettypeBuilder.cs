using System.Collections.Generic;
using System.Linq;
using OWP.Extensions;
using OWP.OtherSport.EventAssignment.Models;
using OWP.OtherSport.Submit.Models;

namespace OWP.OtherSport.EventAssignment.Builder
{
    public interface BettypeBuilder
    {
        public void BuildBettype(
           List<EventAssignmentModel> matchsAssignments,
           Dictionary<int, List<MatchAssignment>> dicMatchAssignment);
    }

    public class BettypeBuilderImp : BettypeBuilder
    {
        public void BuildBettype(
            List<EventAssignmentModel> matchsAssignments,
            Dictionary<int, List<MatchAssignment>> dicMatchAssignment)
        {
            foreach (var matchAssignment in matchsAssignments)
            {
                var assignments = dicMatchAssignment.GetValue(matchAssignment.OSMatchId, new List<MatchAssignment>());

                var bettypes = assignments.Select(a => a.BetTypeName).Distinct().ToList();

                foreach (var bettype in bettypes)
                {
                    matchAssignment.TraderMaxbetModels.Add(new TraderMaxBetModel
                    {
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
    }
}
