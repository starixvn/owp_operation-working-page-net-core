using Newtonsoft.Json;
using System;

namespace OWP.AutoAssignment.Models.PublicApi
{
    public class CommonMatchSender
    {
        [JsonProperty("MaxBet_MainMatch")]
        public double MaxBetMainMatch { get; set; }

        public DateTime KOTime { get; set; }

        public string Location { get; set; }

        public int MatchId { get; set; }
    }
}