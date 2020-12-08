using Newtonsoft.Json;

namespace OWP.API.SyncMaxbet.Models
{
    public class MaxbetInfo
    {
        [JsonProperty("match_id")]
        public int OwpMatchId { get; set; }

        [JsonProperty("traders")]
        public TraderAssignmentSender TraderAssignments { get; set; } = new TraderAssignmentSender();

        [JsonProperty("second_accept_value")]
        public int SecondAcceptValue { get; set; }

        [JsonProperty("spread")]
        public MaxbetSpread Spread { get; set; }

        [JsonProperty("max_bet")]
        public MaxbetSpread Maxbet { get; set; }

        [JsonProperty("max_bet_cn88")]
        public MaxbetSpread MaxbetCN88 { get; set; }

        [JsonProperty("number_of_lines")]
        public NumberOfLines NumberOfLines { get; set; }


    }
}
