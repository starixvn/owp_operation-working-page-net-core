using OWP.OtherSport.Submit.Models;

namespace OWP.OtherSport.EventAssignment.Models
{
    public class TraderMaxBetModel
    {
        /// <summary>
        /// Use to collapse/expand on UI
        /// </summary>
        public bool Hiden { get; set; }

        public string BetTypeName { get; set; }

        public TraderAssignment TraderAssignment { get; set; } = new TraderAssignment();

        public MatchMaxBet Maxbet { get; set; } = new MatchMaxBet();
    }
}
