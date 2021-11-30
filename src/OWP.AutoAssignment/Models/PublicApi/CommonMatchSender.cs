using Newtonsoft.Json;
using System;

namespace OWP.AutoAssignment.Models.PublicApi
{
    public class CommonMatchSender
    {
        [JsonProperty("MaxBet_MainMatch")]
        public decimal? MaxBetMainMatch { get; set; }

        public string KOTime { get; set; }

        public string Location { get; set; }

        public int MatchId { get; set; }
    }
}