using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OWP.OtherSport.EventAssignment.Models;
using OWP.OtherSport.Submit.Models;

namespace OWP.OtherSport.Helper
{
    public static class EventAssignmentBuildMaxbet
    {
        public static void BuildMaxbet(List<EventAssignmentModel> assignments,
            Dictionary<int, List<MatchMaxBet>> dicMatchMaxBet)
        {
            Parallel.ForEach(assignments, assignment =>
            {
                var maxbets = DicHelper.GetValueFromDicBykey(dicMatchMaxBet, assignment.OSMatchId);
                foreach (var traderMaxBetModel in assignment.TraderMaxbetModels)
                {
                    CreateMaxbet(maxbets, traderMaxBetModel.Maxbet);
                }
            });
        }


        public static void CreateMaxbet(
            IList<MatchMaxBet> matchMaxBets,
            MatchMaxBet matchMaxBet)
        {
            var maxbet = matchMaxBets.FirstOrDefault(match => string.Equals(match.BetTypeName, matchMaxBet.BetTypeName));
            if (maxbet != null)
            {
                matchMaxBet.MaxBet = maxbet.MaxBet;
            }
        }
    }
}
