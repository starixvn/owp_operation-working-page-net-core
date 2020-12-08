using System.Text.Json.Serialization;

namespace OWP.API.SyncMaxbet.Models
{
    public class MaxbetInfo
    {
        public int MatchId { get; set; }

        [JsonPropertyName("traders")]
        public TraderAssignmentSender Traders { get; set; } = new TraderAssignmentSender();

        public int SecondAcceptValue { get; set; }

        public MaxbetSpread Spread { get; set; }

        public MaxbetSpread Maxbet { get; set; }

        public MaxbetSpread MaxbetCN88 { get; set; }

        public NumberOfLines NumberOfLines { get; set; }
    }
}