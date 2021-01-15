using OWP.OtherSport.Submit.Models;

namespace OWP.OtherSport.EventAssignment.Models
{
    public class TraderMaxBetModel
    {
        public string BetTypeName { get; set; }

        public TraderAssignment TraderAssignment { get; set; } = new TraderAssignment();

        public MatchMaxBet Maxbet { get; set; } = new MatchMaxBet();
    }
}
