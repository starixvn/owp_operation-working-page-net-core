using Newtonsoft.Json;

namespace OWP.API.SyncMaxbet.model
{
    public class MaxbetSpread
    {
        [JsonProperty("HDP")]
        public decimal Handicap { get; set; }

        [JsonProperty("OU")]
        public decimal OverUnder { get; set; }

        [JsonProperty("1H_HDP")]
        public decimal FirstHandicap { get; set; }

        [JsonProperty("1H_HDP")]
        public decimal FirstOverUnder { get; set; }
    }
}
