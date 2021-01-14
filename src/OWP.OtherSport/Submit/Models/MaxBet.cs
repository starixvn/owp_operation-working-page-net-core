using OWP.OtherSport.EventAssignment.Models;

namespace OWP.OtherSport.Submit.Models
{
    public class MatchMaxBet: TraderMatBex
    {
        public int OSMatchId { get; set; }

        public string BetTypeName { get; set; }

        public decimal? MaxBet { get; set; }
    }
}
