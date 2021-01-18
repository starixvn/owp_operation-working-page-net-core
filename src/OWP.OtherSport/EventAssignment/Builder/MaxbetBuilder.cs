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
        private IList<MatchMaxBet> matchMaxBets;

        public void BuildMaxbet(IList<TraderMaxBetModel> traderMaxBetModels,
            IList<MatchMaxBet> matchMaxBets)
        {
            this.matchMaxBets = matchMaxBets;
            foreach (var traderMaxBetModel in traderMaxBetModels)
            {
                CreateMaxbet(traderMaxBetModel.Maxbet);
            }
        }

        public void CreateMaxbet(MatchMaxBet matchMaxBet)
        {
            var maxbet = matchMaxBets.FirstOrDefault(match => string.Equals(match.BetTypeName, matchMaxBet.BetTypeName));
            if (maxbet != null)
            {
                matchMaxBet.MaxBet = maxbet.MaxBet;
            }
        }
    }
}
