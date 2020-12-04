using Newtonsoft.Json;

namespace OWP.API.SyncMaxbet.Models
{
    public class MaxbetSpread
    {
        [JsonProperty("HDP")]
        public decimal Handicap { get; set; }

        [JsonProperty("OU")]
        public decimal OverUnder { get; set; }

        [JsonProperty("1H_HDP")]
        public decimal FirstHalfHandicap { get; set; }

        [JsonProperty("1H_HDP")]
        public decimal FirstHalfOverUnder { get; set; }
    }
}
