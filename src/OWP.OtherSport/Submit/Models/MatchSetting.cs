using OWP.Extensions;
using OWP.OtherSport.Submit.Contants;
using System.Collections.Generic;

namespace OWP.OtherSport.Submit.Models
{
    public class MatchSetting
    {
        public int OSMatchId { get; set; }

        public string BetTypeName { get; set; }

        public decimal? MaxBet { get; set; }

        public decimal? Spread { get; set; }
    }
}
