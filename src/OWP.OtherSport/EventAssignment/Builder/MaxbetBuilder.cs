namespace OWP.OtherSport.EventAssignment.Builder
{
    using System.Collections.Generic;
    using System.Linq;
    using OWP.OtherSport.EventAssignment.Models;
    using OWP.OtherSport.Submit.Models;

    public interface MaxbetBuilder
    {
        public void BuildMaxbet(IList<TraderMaxBetModel> traderMaxBetModels,
           IList<MatchMaxBet> matchMaxBets);
    }

    public class MaxbetBuilderImp : MaxbetBuilder
    {
        public void BuildMaxbet(IList<TraderMaxBetModel> traderMaxBetModels,
            IList<MatchMaxBet> matchMaxBets)
        {
            foreach (var traderMaxBetModel in traderMaxBetModels)
            {
                var maxbet = matchMaxBets.FirstOrDefault(match =>
                    string.Equals(match.BetTypeName, traderMaxBetModel.Maxbet.BetTypeName));

                if (maxbet != null)
                {
                    traderMaxBetModel.Maxbet.MaxBet = maxbet.MaxBet;
                }
            }
        }
    }
}
