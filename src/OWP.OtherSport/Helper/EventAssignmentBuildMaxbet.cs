using System.Collections.Generic;
using System.Linq;
using OWP.OtherSport.EventAssignment.Models;
using OWP.OtherSport.Submit.Models;

namespace OWP.OtherSport.Helper
{
    public static class EventAssignmentBuildMaxbet
    {
        public static void BuildMaxbet(List<EventAssignmentModel> matchs,
            Dictionary<int, List<MatchMaxBet>> dicMatchMaxBet)
        {
            matchs.ForEach(match =>
            {
                var maxbets = DicHelper.GetValueFromDicBykey(dicMatchMaxBet, match.OSMatchId);
                foreach (var traderMaxBetModel in match.TraderMaxbetModels)
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
